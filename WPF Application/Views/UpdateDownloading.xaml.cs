using System;
using System.Windows;
using System.Windows.Controls;

namespace Right_Click_Commands.WPF.Views
{
    /// <summary>
    /// Interaction logic for UpdateDownloading.xaml
    /// </summary>
    public partial class UpdateDownloading : UserControl
    {
        //  Properties
        //  ==========

        public new UpdateWindow Parent { get; set; }

        public int DownloadPercentage
        {
            set => pbDownloadingProgress.Value = value;
        }

        public bool InstallButtonIsVisible
        {
            set => BtnInstall.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }

        public string Label
        {
            set => lblActivity.Content = value;
        }

        //  Constructors
        //  ============

        public UpdateDownloading(UpdateWindow parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));

            InitializeComponent();
        }

        //  Events
        //  ======

        private void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            Parent.Install(e);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Parent.Cancel(e);
        }

        private void UpdateFoundUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Parent.Download(e);
        }
    }
}