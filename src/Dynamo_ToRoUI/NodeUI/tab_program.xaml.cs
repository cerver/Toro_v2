using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;
using Microsoft.Win32;

namespace Dynamo_TORO.NodeUI
{
    public delegate void PointerUpdatedHandler(object sender, EventArgs e);

    /// <summary>
    /// Interaction logic for tab_program.xaml
    /// </summary>
    public partial class tab_program : UserControl
    {
        public event PointerUpdatedHandler pointerUpdated;
        protected virtual void OnPointerChanged(EventArgs e)
        {
            pointerUpdated?.Invoke(this, e);
        }

        public string fileLoc = null;
        public List<ProgramItem> program = new List<ProgramItem>();

        public string[] selectedControler = null;
        public Controller controller = null;

        public tab_program()
        {
            InitializeComponent();
            foreach (var prg in program)
            {
                ProgramList.Items.Add(prg);
            }
            UIconnectToRS.Visibility = Visibility.Hidden;
            UIconnectToRS.MouseLeave += UIconnectToRsOnMouseLeave;
            btSendToRS.IsEnabled = false;
            btSetProgramPtr.IsEnabled = false;
            btPlayFromPointer.IsEnabled = false;
            btStop.IsEnabled = false;

        }

    

        private void ProgramList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Guid systemId = new Guid(selectedControler[1]);
            controller = new Controller(systemId);
            controller.Logon(UserInfo.DefaultUser);
            
            var newTask = controller.Rapid.GetTask("T_ROB1");
            using (Mastership.Request(controller.Rapid))
            {
                newTask.MotionPointerChanged += NewTaskOnMotionPointerChanged;
            }
            
        }

        private void NewTaskOnMotionPointerChanged(object sender, ProgramPositionEventArgs programPositionEventArgs)
        {
            OnPointerChanged(EventArgs.Empty);
        }


        private void btGetControlers_Click(object sender, RoutedEventArgs e)
        {
            if (UIconnectToRS.Visibility == Visibility.Hidden)
            {
                UIconnectToRS.Visibility = Visibility.Visible;
                UIconnectToRS.RefreshSearch();

            }
            else
            {
                UIconnectToRS.Visibility = Visibility.Hidden;
                if (UIconnectToRS.selectedControler != null)
                {

                    selectedControler = UIconnectToRS.selectedControler;
                    btSendToRS.IsEnabled = true;
                    btSetProgramPtr.IsEnabled = true;
                    btPlayFromPointer.IsEnabled = true;
                    btStop.IsEnabled = true;

                }
            }
        }

        private void UIconnectToRsOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            UIconnectToRS.Visibility = Visibility.Hidden;
            if (UIconnectToRS.selectedControler != null)
            {

                selectedControler = UIconnectToRS.selectedControler;
                btSendToRS.IsEnabled = true;
                btSetProgramPtr.IsEnabled = true;
                btPlayFromPointer.IsEnabled = true;
                btStop.IsEnabled = true;

            }
        }

    }


}
