using System.Windows;
using System.Windows.Input;

namespace Right_Click_Commands.WPF.Views
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

#if !DEBUG
            ScriptGrid.Visibility = Visibility.Hidden;
#endif
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
