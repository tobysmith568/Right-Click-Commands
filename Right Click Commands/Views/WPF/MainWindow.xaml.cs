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

namespace Right_Click_Commands.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //  Constructors
        //  ============

        public MainWindow()
        {
            InitializeComponent();
        }

        //  Events
        //  ======

        private void GridSplitter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ResetPartition();
            }
        }

        private void ResetPartition_Click(object sender, RoutedEventArgs e)
        {
            ResetPartition();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FocusStealer.Focus();
            VMClosingEvent.Command.Execute(e);
        }

        //  Methods
        //  =======

        private void ResetPartition()
        {
            GrdMain.ColumnDefinitions[0].Width = new GridLength(165, GridUnitType.Pixel);
            GrdMain.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
        }
    }
}
