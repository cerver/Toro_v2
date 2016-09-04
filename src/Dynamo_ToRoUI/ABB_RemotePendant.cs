using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using WinForm = System.Windows.Forms;
using System.Xml;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;

using ProtoCore.AST.AssociativeAST;
using Dynamo.Events;
using Dynamo_TORO.NodeUI;
using Microsoft.Win32;
using String = System.String;
using Task = ABB.Robotics.Controllers.RapidDomain.Task;


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

        #endregion

        #region properties
        [IsVisibleInDynamoLibrary(false)]
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
        public int SelectedCtrlIndex
        {
            get
            {
                return _selectedCtrlIndex;
            }
            set
            {
                _selectedCtrlIndex = value;
                RaisePropertyChanged("NodeMessage");
                if (_selectedCtrlIndex >= 0)
                {
                    CustomUi.ProgramPanel.enableButtons();
                }
                else
                {
                    CustomUi.ProgramPanel.disableButtons();
                }
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

            //(string filePath, List<string> cnstList, List<string> instList, List<string> toolList, List<string> wobjList)

            // When you create a UI node, you need to do the
            // work of setting up the ports yourself. To do this,
            // you can populate the InPortData and the OutPortData
            // collections with PortData objects describing your ports.
            //InPortData.Add(new PortData("Space", Resources.DMInPortToolTip));
            InPortData.Add( new PortData("cnstList","List of constants"));
            InPortData.Add(new PortData("instList", "List of instructions"));
            InPortData.Add(new PortData("toolList", "Tool data (EOAT)"));
            InPortData.Add(new PortData("wobjList", "Work Object data"));
         
            // Nodes can have an arbitrary number of inputs and outputs.
            // If you want more ports, just create more PortData objects.
            OutPortData.Add(new PortData("Program", "This is the Rapid Program"));
            OutPortData.Add(new PortData("Controler", "This is the current controler"));

            // This call is required to ensure that your ports are
            // properly created.
            RegisterAllPorts();

            // The arugment lacing is the way in which Dynamo handles
            // inputs of lists. If you don't want your node to
            // support argument lacing, you can set this to LacingStrategy.Disabled.
            ArgumentLacing = LacingStrategy.Shortest;
            BtGetExistRapidFile = new DelegateCommand(GetExistingRapidFileBtnClicked, IsOk);
            BtMakeNewRapidFile = new DelegateCommand(MakeNewRapidFileBtnClicked, IsOk);
            BtSendProgramToRs = new DelegateCommand(SendProgramToRsClicked, IsOk);
            BtSetProgramPointer = new DelegateCommand(SetProgramPointer, IsOk);
            BtPlayFromPointer = new DelegateCommand(PlayFromPointer, IsOk);
            BtStop = new DelegateCommand(StopSim, IsOk);
            ProgramPointerChangedCommand = new DelegateCommand(StopSim, IsOk);

            //  ExecutionEvents.GraphPostExecution += ExecutionEvents_GraphPostExecution;
            // ExecutionEvents.GraphPreExecution += ExecutionEvents_GraphPreExecution;
            NodeModel = this;
            if (CustomUi != null)
            {
                CustomUi.ProgramPanel.pointerUpdated += ProgramPanelOnPointerUpdated;
                
            }
      

        }

        private void ProgramPanelOnPointerUpdated(object sender, EventArgs eventArgs)
        {
            ProgramPosition pp = (ProgramPosition) sender;
            var a = 1;
        }


        private void ExecutionEvents_GraphPreExecution(Dynamo.Session.IExecutionSession session)
        {
            _exacutionComplete = false;
        }

        private void ExecutionEvents_GraphPostExecution(Dynamo.Session.IExecutionSession session)
        { 
            _exacutionComplete = true;

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
            //run the getNodeInputs in async 
            GetInputDelegate getInput = new GetInputDelegate(this.GetNodeInputs);

            //setup list to hold elements
            List<string> cnstList = new List<string>();
            List<string> instList = new List<string>();
            List<string> toolList = new List<string>();
            List<string> wobjList = new List<string>();


            //get the inputs
            if (NodeModel != null && NodeView != null)
            {
                if (HasConnectedInput(0))
                    cnstList = getInput(0).OfType<string>().ToList();
                if (HasConnectedInput(1))
                    instList = getInput(1).OfType<string>().ToList();
                if (HasConnectedInput(2))
                    toolList = getInput(2).OfType<string>().ToList();
                if (HasConnectedInput(3))
                    wobjList = getInput(3).OfType<string>().ToList();

                

                Func<string, List<string>, List<string>, List<string>, List<string>, string> func =
                    ToroUIfunctions.processUIdata;

                string progData = string.Empty;

                _fileExists = false;
                if (CustomUi != null && _ModFileLoc != null)
                {

                    _fileExists = File.Exists(_ModFileLoc);
                    if (_fileExists && HasConnectedInput(0) && HasConnectedInput(1) && HasConnectedInput(2) &&
                        HasConnectedInput(3))
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

            AssociativeNode _controler;
            if (CustomUi != null && CustomUi.SetupPanel.selectedControler != null)
                _controler = AstFactory.BuildExprList(new List<string>(CustomUi.SetupPanel.selectedControler));
            else
            {
                _controler = AstFactory.BuildNullNode();
            }

            return new[]
            {

                AstFactory.BuildAssignment( GetAstIdentifierForOutputIndex(0), _programData),
                AstFactory.BuildAssignment( GetAstIdentifierForOutputIndex(1), _controler)
            };
        }

        protected override void SerializeCore(XmlElement nodeElement, SaveContext context)
        {
            base.SerializeCore(nodeElement, context);
           
            if (true)
            {

                XmlElement modFileLocElement = nodeElement.OwnerDocument.CreateElement("ModFileLocation");
                modFileLocElement.SetAttribute("value", _ModFileLoc);
                nodeElement.AppendChild(modFileLocElement);

                XmlElement pgfFileLocElement = nodeElement.OwnerDocument.CreateElement("PgfFileLocation");
                pgfFileLocElement.SetAttribute("value", _PgfFileLoc);
                nodeElement.AppendChild(pgfFileLocElement);

                XmlElement trob1DirLocElement = nodeElement.OwnerDocument.CreateElement("Trob1DirLocation");
                trob1DirLocElement.SetAttribute("value", _trob1DirLoc);
                nodeElement.AppendChild(trob1DirLocElement);
            }
           

        }

        protected override void DeserializeCore(XmlElement nodeElement, SaveContext context)
        {
            base.DeserializeCore(nodeElement, context);
     

            foreach (XmlNode subNode in nodeElement.ChildNodes)
            {
                if (subNode.Name.Equals("PgfFileLocation"))
                {
                    try
                    {
                        _PgfFileLoc = subNode.Attributes[0].Value;
                    }
                    catch { }
                }
                if (subNode.Name.Equals("Trob1DirLocation"))
                {
                    try
                    {
                        _trob1DirLoc = subNode.Attributes[0].Value;
                    }
                    catch { }
                }

                if (subNode.Name.Equals("ModFileLocation"))
                {
                    try
                    {
                        _ModFileLoc = subNode.Attributes[0].Value;
                        GetFileDataAndPopulatePanel(true);
                    }
                    catch { }
                }
              


            }

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
                        CustomUi.SetupPanel.TxRapidFileLoc.Content = _ModFileLoc;
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
            fbDialog.Description = "Select a folder where the new Rapid files will be placed";

            if (fbDialog.ShowDialog() == WinForm.DialogResult.OK)
            {
                try
                {
                    string currentDir = fbDialog.SelectedPath + @"\T_ROB1";
                    Directory.CreateDirectory(currentDir);
                    string newModFile = currentDir + @"\MainModule.mod";
                    File.WriteAllText(newModFile, "");

                    _ModFileLoc = newModFile;
                    var pgfFileLoc = currentDir + @"\T_ROB1.pgf";
                    _PgfFileLoc = pgfFileLoc;

                    GetFileDataAndPopulatePanel(true);
                    CustomUi.ProgramPanel.TbFileLoc.Text = _ModFileLoc;
                    CustomUi.SetupPanel.TxRapidFileLoc.Content = _ModFileLoc;

                    

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
                    CustomUi.SetupPanel.TxRapidFileLoc.Content = _ModFileLoc;

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
