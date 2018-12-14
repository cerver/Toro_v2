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
using ABB.Robotics.Controllers.RapidDomain;
//using ABB.Robotics.Controllers;
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

        public List<ProgramItem> program = new List<ProgramItem>();
        public int selectecIndex = 0;

        public tab_program()
        {
            InitializeComponent();
            foreach (var prg in program)
            {
                ProgramList.Items.Add(prg);
            }

            disableButtons();

        }

    

      

        private void NewTaskOnMotionPointerChanged(object sender, ProgramPositionEventArgs programPositionEventArgs)
        {
            OnPointerChanged(EventArgs.Empty);
        }

        public void enableButtons()
        {
            btSendToRS.IsEnabled = true;
            btSetProgramPtr.IsEnabled = true;
            btPlayFromPointer.IsEnabled = true;
            btStop.IsEnabled = true;

        }
        public void disableButtons()
        {
            btSendToRS.IsEnabled = false;
            btSetProgramPtr.IsEnabled = false;
            btPlayFromPointer.IsEnabled = false;
            btStop.IsEnabled = false;

        }


        private void ProgramList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectecIndex = ProgramList.SelectedIndex;
        }
    }


}
