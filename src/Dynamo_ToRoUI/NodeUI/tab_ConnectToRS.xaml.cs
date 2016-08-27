using System;
using System.Collections.Generic;
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

namespace Dynamo_TORO.NodeUI
{
    /// <summary>
    /// Interaction logic for tab_ConnectToRS.xaml
    /// </summary>
    public partial class tab_ConnectToRS : UserControl
    {
        private Dictionary<string, List<string[]>> controlers = new Dictionary<string, List<string[]>>(4); 
        private List<string[]>fullCtrlList = new List<string[]>(); 
        public string[] selectedControler = null;

        public tab_ConnectToRS()
        {
            InitializeComponent();
        }

        private void btRefreshSearch_Click(object sender, RoutedEventArgs e)
        {
            RefreshSearch();
        }

        private void controlerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (controlerListView.Items.Count > 0)
            {
                selectedControler = fullCtrlList[controlerListView.SelectedIndex];
                tbSelectedCtrl.Text =
                    string.Format(
                        "--SELECTED CONTROLER --\nRobotCtrl: {0} | {1}\n Availability: {0}\n Version: {1}\n IP:{2}",
                        selectedControler[0], selectedControler[1], selectedControler[2], selectedControler[3],selectedControler[4]);
            }
        }

        public void RefreshSearch()
        {
            controlers = RobComm.findControllers(true);

            if (controlers.Count > 0)
            {

                controlerListView.Items.Clear();
                fullCtrlList = new List<string[]>();

                ListViewItem itemsForList;
                foreach (var ctrl in controlers["robotController"])
                {
                    itemsForList = new ListViewItem();
                    itemsForList.Content = $"RobotCtrl: {ctrl[0]}";
                    itemsForList.ToolTip = $"Availability: {ctrl[2]}\n Version: {ctrl[3]}\n IP:{ctrl[4]}";

                    controlerListView.Items.Add(itemsForList);
                    fullCtrlList.Add(ctrl);
                }
                foreach (var ctrl in controlers["virtualController"])
                {
                    itemsForList = new ListViewItem();
                    itemsForList.Content = $"VirtualCtrl: {ctrl[0]}";
                    itemsForList.ToolTip = $"Availability: {ctrl[2]}\n Version: {ctrl[3]}\n IP:{ctrl[4]}";

                    controlerListView.Items.Add(itemsForList);
                    fullCtrlList.Add(ctrl);
                }

            }
            if (controlers.Count == 0)
                tbSelectedCtrl.Text = "No Controlers found.";
        }


    }
}
