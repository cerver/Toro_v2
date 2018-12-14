using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WinForm = System.Windows.Forms;
using System.Xml;
//using ABB.Robotics.Controllers;
//using ABB.Robotics.Controllers.RapidDomain;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;

using ProtoCore.AST.AssociativeAST;
using Dynamo.Events;
using Dynamo.ViewModels;
using Dynamo_TORO.NodeUI;
using String = System.String;
using Dynamo_TORO;
using Newtonsoft.Json;

//created by Robert Cervellione
namespace Dynamo_TORO
{
    [NodeName("ABB_RemotePendant")]
    [NodeDescription("Control Various function of a virtual or real ABB controler such as IRC5")]
    [NodeCategory("TORO.RobComm")]
    [IsDesignScriptCompatible]
    public class AbbRemotePendant : NodeModel
    {

        public RemotePanelMain CustomUi = null;
        public NodeView NodeView = null;
        public NodeModel NodeModel = null;
        #region private members

        private AssociativeNode _programData = AstFactory.BuildNullNode();
        bool _fileExists = false;

        //  private Dictionary<string, DMCurve> SelectedItems;

        private string _currentFile = "";
        private bool _differUpdate = false;
        private bool _exacutionComplete = false;
        private string[] _fileContents;
        private string _ModFileLoc = null;
        private string _PgfFileLoc = null;
        private string _trob1DirLoc = null;
        private int _selectedCtrlIndex = -1;


        private List<ProgramItem> _program = new List<ProgramItem>();

        internal System.Threading.Tasks.Task RunUiProgUpdate;
        internal System.Threading.Tasks.Task RunProcessRapidCode;

        //run the getNodeInputs in async 
        internal GetInputDelegate getInput;

        #endregion

        
        [IsVisibleInDynamoLibrary(false)]
        [JsonProperty(PropertyName = "ProgModFileLoc")]
        public string ProgModFileLoc
        {
            get
            {
                return _ModFileLoc;
            }
            set
            {
                _ModFileLoc = value;
                RaisePropertyChanged("NodeMessage");
               
            }
        }

        [IsVisibleInDynamoLibrary(false)]
        [JsonProperty(PropertyName = "SelectedCtrlIndex")]
        public int SelectedCtrlIndex
        {
            get
            {
                return _selectedCtrlIndex;
            }
            set
            {
                _selectedCtrlIndex = value;
                
                if (_selectedCtrlIndex >= 0)
                {
                    CustomUi.ProgramPanel.enableButtons();
                    OnNodeModified();
                }
                else
                {
                    CustomUi.ProgramPanel.disableButtons();
                }
                RaisePropertyChanged("NodeMessage");
            }
        }

        // Use the VMDataBridge to safely retrieve our input values
        #region databridge callback
        /// <summary>
        /// Register the data bridge callback.
        /// </summary>
        protected override void OnBuilt()
        {
            base.OnBuilt();
            VMDataBridge.DataBridge.Instance.RegisterCallback(GUID.ToString(), DataBridgeCallback);
         
        }

        /// <summary>
        /// Callback method for DataBridge mechanism.
        /// This callback only gets called when 
        ///     - The AST is executed
        ///     - After the BuildOutputAST function is executed 
        ///     - The AST is fully built
        /// </summary>
        /// <param name="data">The data passed through the data bridge.</param>
        private void DataBridgeCallback(object data)
        {
            try
            {
                var dataObj = data;
              
            }
            catch
            {
                Warning("DataBridge callback failed");
            }
        }

        /// <summary>
        /// DelegateCommand objects allow you to bind
        /// UI interaction to methods on your data context.
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand BtGetExistRapidFile { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand BtMakeNewRapidFile { get; set; }

        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand BtSendProgramToRs { get; set; }

        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand BtSetProgramPointer { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand BtPlayFromPointer { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand BtStop { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand ProgramPointerChangedCommand { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand BtUpdateNode { get; set; }

        [IsVisibleInDynamoLibrary(false)]
        internal delegate List<object> GetInputDelegate(int inputNum);


        [IsVisibleInDynamoLibrary(false)]
        internal delegate void SendFileToRs(bool run, string[] controller, string file);

        #endregion

        #region constructor

        /// <summary>
        /// The constructor for a NodeModel is used to create
        /// the input and output ports and specify the argument
        /// lacing.
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public AbbRemotePendant()
        {
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("cnstList", "List of constants")));
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("instList", "List of instructions")));
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("toolList", "Tool data (EOAT)")));
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("wobjList", "Work Object data")));

            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("Program", "This is the Rapid Program")));
            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("Controler", "This is the current controler")));

            RegisterAllPorts();

            ArgumentLacing = LacingStrategy.Longest;

            BtGetExistRapidFile = new DelegateCommand(GetExistingRapidFileBtnClicked, IsOk);
            BtMakeNewRapidFile = new DelegateCommand(MakeNewRapidFileBtnClicked, IsOk);
            BtSendProgramToRs = new DelegateCommand(SendProgramToRsClicked, IsOk);
            BtSetProgramPointer = new DelegateCommand(SetProgramPointer, IsOk);
            BtPlayFromPointer = new DelegateCommand(PlayFromPointer, IsOk);
            BtStop = new DelegateCommand(StopSim, IsOk);
            ProgramPointerChangedCommand = new DelegateCommand(StopSim, IsOk);
            BtUpdateNode = new DelegateCommand(updateNode, IsOk);

            NodeModel = this;

   
        }

        [JsonConstructor]
        AbbRemotePendant(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {

        }


        #endregion

        #region public methods

        /// <summary>
        /// If this method is not overriden, Dynamo will, by default
        /// pass data through this node. But we wouldn't be here if
        /// we just wanted to pass data through the node, so let's 
        /// try using the data.
        /// </summary>
        /// <param name="inputAstNodes"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {

            getInput = GetNodeInputs;

            //setup list to hold elements
            List<object> cnstList = new List<object>();
            List<string> instList = new List<string>();
            List<string> toolList = new List<string>();
            List<string> wobjList = new List<string>();


            //get the inputs
            if (NodeModel != null && NodeView != null)
            {
              
                try
                {
                    cnstList = getInput.Invoke(0).OfType<object>().ToList();
                    instList = getInput.Invoke(1).OfType<string>().ToList();
                    toolList = getInput.Invoke(2).OfType<string>().ToList();
                    wobjList = getInput.Invoke(3).OfType<string>().ToList();
                }catch
                {
                    Warning("All inputs need valid connections");
                }
                    
            


                Func<string, List<object>, List<string>, List<string>, List<string>, string> func =
                    ToroUIfunctions.processUIdata;

                string progData = string.Empty;

                _fileExists = false;
                if (CustomUi != null && _ModFileLoc != null)
                {

                    _fileExists = File.Exists(_ModFileLoc);
                    //if (_fileExists && HasConnectedInput(0) && HasConnectedInput(1) && HasConnectedInput(2) && HasConnectedInput(3))
                    if (_fileExists)
                    {
                        RunProcessRapidCode =
                            new TaskFactory().StartNew(
                                () =>
                                    progData =
                                        ToroUIfunctions.processUIdata(_ModFileLoc, cnstList, instList, toolList, wobjList));
                        RunProcessRapidCode.Wait();
                        _programData = AstFactory.BuildStringNode(progData);
                        GetFileDataAndPopulatePanel(false);

                    }

                }
            }
            else
            {
                _programData = AstFactory.BuildStringNode("No program data created");
            }

            List<AssociativeNode> _controler = new List<AssociativeNode>(6);
            if (CustomUi != null && CustomUi.SetupPanel.hasController)
            {
                foreach (var s in CustomUi.SetupPanel.selectedControler)
                {
                    _controler.Add(AstFactory.BuildStringNode(s));
                }
            }
                
            else
            {
                _controler = new List<AssociativeNode>(1) {AstFactory.BuildNullNode()};
            }

            return new[]
            {

                AstFactory.BuildAssignment( GetAstIdentifierForOutputIndex(0), _programData),
                AstFactory.BuildAssignment( GetAstIdentifierForOutputIndex(1), AstFactory.BuildExprList(_controler))
            };
        }

    
        #endregion

        #region command methods

        internal static bool IsOk(object obj)
        {
            return true;
        }

        internal void GetExistingRapidFileBtnClicked(object obj)
        {
            try
            {
                WinForm.FolderBrowserDialog fbDialog = new WinForm.FolderBrowserDialog();
                fbDialog.Description = "Select a folder where the existing Rapid files are. This is the directory which contains the MainModule.mod and the T_ROB1.pgf file ";

                if (fbDialog.ShowDialog() == WinForm.DialogResult.OK)
                {
                    _trob1DirLoc = fbDialog.SelectedPath;
                    _PgfFileLoc = fbDialog.SelectedPath + @"\T_ROB1.pgf";
                    _ModFileLoc = fbDialog.SelectedPath + @"\MainModule.mod";

                    if (File.Exists(_PgfFileLoc) && File.Exists(_ModFileLoc))
                    {
                        GetFileDataAndPopulatePanel(true);
                        CustomUi.ProgramPanel.TbFileLoc.Text = _ModFileLoc;
                        CustomUi.SetupPanel.TxRapidFileLoc.Text = _ModFileLoc;
                    }
                    else
                    {
                        throw new Exception("The MainModule.mod file or the T_ROB1.pgf file is missing");
                    }

                }
            }
            catch (Exception exception)
            {
        
            }
           

        }
        internal void MakeNewRapidFileBtnClicked(object obj)
        {
            WinForm.FolderBrowserDialog fbDialog = new WinForm.FolderBrowserDialog();
            fbDialog.Description = @"Select a folder where the new RAPID files will be placed";

            if (fbDialog.ShowDialog() == WinForm.DialogResult.OK)
            {
                try
                {
                    string currentDir = fbDialog.SelectedPath;
                    if (File.Exists(currentDir + @"\MainModule.mod") && File.Exists(currentDir + @"\T_ROB1.pgf"))
                    {
                        var messageResult = MessageBox.Show("RAPID files exist in the directory you have chosen. Do you want to overwrite them?",
                            "Warning Files Exist", MessageBoxButton.YesNo);
                        if (messageResult != MessageBoxResult.Yes)
                        {
                            return;
                        }
                    }

                    Directory.CreateDirectory(currentDir);
                    string newModFile = currentDir + @"\MainModule.mod";
                    File.WriteAllText(newModFile, "");

                    _ModFileLoc = newModFile;
                    var pgfFileLoc = currentDir + @"\T_ROB1.pgf";
                    _PgfFileLoc = pgfFileLoc;

                    GetFileDataAndPopulatePanel(true);
                    CustomUi.ProgramPanel.TbFileLoc.Text = _ModFileLoc;
                    CustomUi.SetupPanel.TxRapidFileLoc.Text = _ModFileLoc;

                    

                    File.WriteAllText(pgfFileLoc, @"<?xml version=""1.0"" encoding=""ISO-8859-1"" ?>
<Program>
 <Module>MainModule.mod</Module>
</Program> ");
                    
                

                }
                catch (Exception exception)
                {
                   
                }
                

            }

        }

        internal void SendProgramToRsClicked(object obj)
        {
            if (CustomUi.SetupPanel.hasController & File.Exists(_PgfFileLoc))
            {
                SendFileToRs sendFile = new SendFileToRs(RobComm.ProgramFileToController);
                try
                {
                    sendFile(true, CustomUi.SetupPanel.selectedControler, _PgfFileLoc);
                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }
            }

            
            

        }
        
        internal void SetProgramPointer(object obj)
        {
            if (CustomUi.SetupPanel.hasController)
            {
                RobComm.setProgramPointer(true, CustomUi.SetupPanel.selectedControler,
                    CustomUi.ProgramPanel.selectecIndex + 1);
            }
        }
        internal void PlayFromPointer(object obj)
        {
            if (CustomUi.SetupPanel.hasController)
            {
                RobComm.playFromPointerLoc(true, CustomUi.SetupPanel.selectedControler);
            }
        }
        internal void StopSim(object obj)
        {
            if (CustomUi.SetupPanel.hasController)
            {
                RobComm.StopSim(true, CustomUi.SetupPanel.selectedControler);
            }
        }

        internal void updateNode(object obj)
        {
            OnNodeModified(true);
        }


        #endregion
        [IsVisibleInDynamoLibrary(false)]
        public override void Dispose()
        {
            base.Dispose();


        }

        internal static bool IsFileReady(String sFilename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (inputStream.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal List<object> GetNodeInputs(int inputNum)
        {
            var inputNamenode = NodeModel.InPorts[inputNum].Connectors[0].Start.Owner;
            var inputNameIndex = NodeModel.InPorts[inputNum].Connectors[0].Start.Index;
            var inputNameId = inputNamenode.GetAstIdentifierForOutputIndex(inputNameIndex).Name;
            var inputNameMirror = NodeView.ViewModel.DynamoViewModel.Model.EngineController.GetMirror(inputNameId);

            var input = new List<object>();

            if (inputNameMirror == null || inputNameMirror.GetData() == null) return input;

            var data = inputNameMirror.GetData();
            if (data != null)
            {
                if (data.IsCollection)
                {
                    input.AddRange(data.GetElements().Select(e => e.Data).OfType<object>());
                }
                else
                {
                    var inData = data.Data as Object;
                    if (inData != null)
                        input.Add(inData);
                }
            }
            RaisePropertyChanged("NodeMessage");
            return input;
        }

        internal void GetFileDataAndPopulatePanel(bool isFromUi)
        {
            if (_ModFileLoc != null && CustomUi != null)
            {
                if (File.Exists(_ModFileLoc) && File.Exists(_PgfFileLoc))
                {
                    _fileContents = System.IO.File.ReadAllLines(_ModFileLoc);
                    _program = new List<ProgramItem>(_fileContents.Length);
                    if (CustomUi.ProgramPanel.ProgramList.IsInitialized)
                        CustomUi.ProgramPanel.ProgramList.Items.Clear();
                    CustomUi.ProgramPanel.TbFileLoc.Text = _ModFileLoc;
                    CustomUi.SetupPanel.TxRapidFileLoc.Text = _ModFileLoc;

                    for (int i = 0; i < _fileContents.Length; i++)
                    {
                        _program.Add(new ProgramItem(i+1, _fileContents[i]));
                        CustomUi.ProgramPanel.ProgramList.Items.Add(_program[i]);
                    }
                }
                if (isFromUi)
                {
                   // programData = AstFactory.BuildStringNode(string.Join("", fileContents));
                    OnNodeModified(true);
                }

            }
            

        }

    }



    /// <summary>
    ///     View customizer for CustomNodeModel Node Model.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class RemotePendantViewCustomization : INodeViewCustomization<AbbRemotePendant>
    {

        /// <summary>
        /// At run-time, this method is called during the node 
        /// creation. Here you can create custom UI elements and
        /// add them to the node view, but we recommend designing
        /// your UI declaratively using xaml, and binding it to
        /// properties on this node as the DataContext.
        /// </summary>
        /// <param name="model">The NodeModel representing the node's core logic.</param>
        /// <param name="nodeView">The NodeView representing the node in the graph.</param>
        public void CustomizeView(AbbRemotePendant model, NodeView nodeView)
        {
            // The view variable is a reference to the node's view.
            // In the middle of the node is a grid called the InputGrid.
            // We reccommend putting your custom UI in this grid, as it has
            // been designed for this purpose.

            // Create an instance of our custom UI class (defined in xaml),
            // and put it into the input grid.
            var selectNodeControl = new RemotePanelMain();
            nodeView.inputGrid.Children.Add(selectNodeControl);

            // Set the data context for our control to be this class.
            // Properties in this class which are data bound will raise 
            // property change notifications which will update the UI.
            selectNodeControl.DataContext = model;
            model.CustomUi = selectNodeControl;
            model.NodeView = nodeView;
            if (File.Exists(model.ProgModFileLoc))
            {
                model.GetFileDataAndPopulatePanel(true);
                
            }
        }

        /// <summary>
        /// Here you can do any cleanup you require if you've assigned callbacks for particular 
        /// UI events on your node.
        /// </summary>
        public void Dispose()
        {
           
        }
    }


    public class ProgramItem
    {
        public ProgramItem(int num, string data)
        {
            LineNum = num;
            LineData = data;
        }

        public int LineNum { get; set; }
        public string LineData { get; set; }
    }

    
}
