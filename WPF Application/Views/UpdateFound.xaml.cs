using System;
using System.Windows;
using System.Windows.Controls;

namespace Right_Click_Commands.WPF.Views
{
    /// <summary>
    /// Interaction logic for UpdateFound.xaml
    /// </summary>
    public partial class UpdateFound : UserControl
    {
        //  Properties
        //  ==========

        public new UpdateWindow Parent { get; set; }

        //  Constructors
        //  ============

        public UpdateFound(UpdateWindow parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));

            InitializeComponent();
        }

        //  Events
        //  ======

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Parent.No(e);
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Parent.Yes(e);
        }

        private void BtnGithub_Click(object sender, RoutedEventArgs e)
        {
            Parent.VisitGithub(e);
        }
    }
}