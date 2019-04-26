using Right_Click_Commands.Views.WPF;
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
            ShowAbout = new Command(DoShowAbout);
            InitializeComponent();
        }

        public Command ShowAbout { get; }

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VMLoadedEvent.Command.Execute(e);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FocusStealer.Focus();
            VMClosingEvent.Command.Execute(e);
        }

        private void MenuItem_Click_DeleteSelected(object sender, RoutedEventArgs e)
        {
            MIDelete.Command.Execute(e);
        }

        private void MenuItem_Click_MoveSelectedUp(object sender, RoutedEventArgs e)
        {
            MIMoveUp.Command.Execute(e);
        }

        private void MenuItem_Click_MoveSelectedDown(object sender, RoutedEventArgs e)
        {
            MIMoveDown.Command.Execute(e);
        }

        //  Methods
        //  =======

        private void ResetPartition()
        {
            GrdMain.ColumnDefinitions[0].Width = new GridLength(165, GridUnitType.Pixel);
            GrdMain.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
        }

        private void DoShowAbout()
        {
            try
            {
                new About().ShowDialog();
            }
            catch
            {

            }
        }
    }
}
