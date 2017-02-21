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
    /// Interaction logic for RemotePanelMain.xaml
    /// </summary>
    public partial class RemotePanelMain : UserControl
    {
        public RemotePanelMain()
        {
            InitializeComponent();
        }

        private void ControlerExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            remotePnlMain.Height = 65;
            remotePnlMain.Width = 100;
           

        }

        private void ControlerExpander_Expanded(object sender, RoutedEventArgs e)
        {
            remotePnlMain.Height = 400;
            remotePnlMain.Width = 600;
     
        }
    }
}
