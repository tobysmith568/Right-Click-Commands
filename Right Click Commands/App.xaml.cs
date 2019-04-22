using Right_Click_Commands.Models.ContextMenu;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Runner;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.ViewModels;
using Right_Click_Commands.Views;
using Right_Click_Commands.Views.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Resolution;

namespace Right_Click_Commands
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //  Variables
        //  =========

        private readonly string NL = Environment.NewLine;
        private readonly IUnityContainer container = new UnityContainer();
        private IMessagePrompt messagePrompt;
        private IRunner runner;

        //  Events
        //  ======

        /// <exception cref="InvalidOperationException">Ignore.</exception>
        /// <exception cref="System.Security.SecurityException">Ignore.</exception>
        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetUpImplementations();
            messagePrompt = container.Resolve<IMessagePrompt>();
            runner = container.Resolve<IRunner>();

            if (e.Args.Length == 0)
            {
                ShowMainWindow();
                return;
            }

            if (e.Args.Length >= 3 && (e.Args[0] == "run"))
            {
                await RunScript(e.Args);
                Environment.Exit(0);
                return;
            }

            messagePrompt.PromptOK("Unknown command arguments", "Error", MessageType.Error);
            Environment.Exit(0);
        }

        //  Methods
        //  =======

        private void SetUpImplementations()
        {
            container.RegisterType<IContextMenuWorker, RegistryWorker>();
            container.RegisterType<ISettings, WindowsSettings>();
            container.RegisterType<IMessagePrompt, WinDialogBox>();
            container.RegisterType<IRunner, WindowsRunner>();
        }

        /// <exception cref="InvalidOperationException"></exception>
        private void ShowMainWindow()
        {
            var mainWindowViewModel = container.Resolve<MainWindowViewModel>();
            var window = new MainWindow { DataContext = mainWindowViewModel };
            window.Show();
        }

        private async Task RunScript(string[] args)
        {
            try
            {
                string fileName = args[1];
                string fileArguements = args[2];
                fileArguements = fileArguements.Replace('|', '"');

                await runner.Run(fileName, fileArguements);
            }
            catch (ExecutionException e)
            {
                try
                {
                    new ScriptError(e.InnerException.StackTrace).ShowDialog();
                }
                catch
                {
                    messagePrompt.PromptOK($"Your script failed with the following exception:{NL}{e.Message}", "Script failed", MessageType.Error);
                }
            }
        }
    }
}
