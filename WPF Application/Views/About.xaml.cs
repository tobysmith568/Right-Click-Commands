using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;

namespace Right_Click_Commands.WPF.Views
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public Library[] Projects
        {
            get => new Library[]
            {
                new Library
                {
                    Name = "Castle Core",
                    URL = "http://www.castleproject.org",
                    Licence = "Apache License 2.0"
                },
                new Library
                {
                    Name = "log4net",
                    URL = "http://logging.apache.org/log4net/",
                    Licence = "Apache License 2.0"
                },
                new Library
                {
                    Name = "Moq",
                    URL = "https://github.com/moq/moq4",
                    Licence = "BSD 3-Clause License"
                },
                new Library
                {
                    Name = "Newtonsoft.Json",
                    URL = "https://www.newtonsoft.com/json",
                    Licence = "MIT Licence"
                },
                new Library
                {
                    Name = "NUnit",
                    URL = "http://nunit.org",
                    Licence = "MIT License"
                },
                new Library
                {
                    Name = "RestSharp",
                    URL = "http://restsharp.org",
                    Licence = "Apache License 2.0"
                },
                new Library
                {
                    Name = "Unity",
                    URL = "https://github.com/unitycontainer/unity",
                    Licence = "Apache License 2.0"
                }
            };
        }

        //  Constructors
        //  ============

        public About()
        {
            InitializeComponent();

            tbVersion.Text = "Version: v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        //  Events
        //  ======

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            }
            catch (Exception)
            {
                MessageBox.Show($"Unable to open URL: {e.Uri.AbsoluteUri}", "Error opening link", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            e.Handled = true;
        }

        private void BtnCloSe_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public struct Library
    {
        //  Properties
        //  ==========

        public string Name { get; set; }
        public string URL { get; set; }
        public string Licence { get; set; }
    }
}
