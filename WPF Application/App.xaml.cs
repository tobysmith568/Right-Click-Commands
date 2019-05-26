using Right_Click_Commands.Models.ContextMenu;
using Right_Click_Commands.Models.JSON_Converter;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Runner;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.Models.Updater;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using IconPicker;
using Right_Click_Commands.WPF.Models.ContextMenu;
using Right_Click_Commands.WPF.Models.Settings;
using Right_Click_Commands.WPF.Models.MessagePrompts;
using Right_Click_Commands.WPF.Models.Runner;
using Right_Click_Commands.WPF.Models.Updater;
using Right_Click_Commands.WPF.Views;
using Right_Click_Commands.WPF.Models.Scripts;

namespace Right_Click_Commands.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //  Constants
        //  =========

        private const string INSTALLER = "INSTALLER";

        //  Variables
        //  =========

        private readonly string NL = Environment.NewLine;
        private readonly IUnityContainer container = new UnityContainer();
        private IMessagePrompt messagePrompt;
        private IRunner runner;
        private ISettings settings;

        //  Events
        //  ======

        /// <exception cref="InvalidOperationException">Ignore.</exception>
        /// <exception cref="System.Security.SecurityException">Ignore.</exception>
        protected async override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length == 1 && e.Args[0] == INSTALLER)
            {
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo("explorer.exe", ResourceAssembly.Location)
                    {
                        Verb = string.Empty
                    };
                    Process.Start(processStartInfo);
                }
                catch
                {
                }

                Environment.Exit(0);
                return;
            }

            container.AddExtension(new Diagnostic());

            base.OnStartup(e);
            SetUpImplementations();
            messagePrompt = container.Resolve<IMessagePrompt>();
            runner = container.Resolve<IRunner>();
            settings = container.Resolve<ISettings>();

            if (settings.JustInstalled)
            {
                settings.Upgrade();
                settings.JustInstalled = false;
                settings.SaveAll();
            }

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
            container.RegisterType<IUpdater, WindowsUpdater>();
            container.RegisterType<IJSONConverter, JSONConverter>();
            container.RegisterType<IIconPicker, IconPickerDialog>();
            container.RegisterType<IScriptStorageModel, RegistryScriptStorageModel>();
            container.RegisterType<IScriptFactory<IScriptStorageModel>, WindowsScriptFactory>();
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
