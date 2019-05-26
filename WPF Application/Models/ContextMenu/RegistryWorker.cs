using IconPicker;
using Microsoft.Win32;
using Right_Click_Commands.Models.ContextMenu;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.WPF.Models.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Right_Click_Commands.WPF.Models.ContextMenu
{
    public class RegistryWorker : IContextMenuWorker
    {
        //  Constants
        //  =========

        private const string MUIVerb = "MUIVerb";
        private const string Icon = "Icon";
        private const string RCC_ = "RCC_";
        private const string command = "command";
        private const string reg_AnyWordThenRun = "^\".+?\" run ";

        //  Variables
        //  =========

        private static readonly Regex regex = new Regex(reg_AnyWordThenRun);

        private readonly IScriptFactory<IScriptStorageModel> scriptFactory;
        private readonly IMessagePrompt messagePrompt;
        private readonly ISettings settings;
        private readonly IIconPicker iconPicker;

        private readonly string RCCLocation = Assembly.GetEntryAssembly().Location;

        private readonly Dictionary<MenuLocation, string> classesRootOptions = new Dictionary<MenuLocation, string>()
        {
            { MenuLocation.Background, @"Software\Classes\Directory\Background\shell" },
            { MenuLocation.Directory, @"Software\Classes\Directory\shell" }
        };

        //  Constructors
        //  ============

        public RegistryWorker(IScriptFactory<IScriptStorageModel> scriptFactory, IMessagePrompt messagePrompt, ISettings settings, IIconPicker iconPicker)
        {
            this.scriptFactory = scriptFactory;
            this.messagePrompt = messagePrompt;
            this.settings = settings;
            this.iconPicker = iconPicker;
        }

        //  Methods
        //  =======

        public ICollection<IScriptConfig> GetScriptConfigs()
        {
            List<IScriptConfig> results = new List<IScriptConfig>();

            foreach (KeyValuePair<MenuLocation, string> location in classesRootOptions)
            {
                try
                {
                    ReadScriptLocation(location, ref results);
                }
                catch // TODO
                {
                    //Unable to read a classesRoot key
                }
            }

            return results;
        }

        public void SaveScriptConfigs(ICollection<IScriptConfig> configs)
        {
            foreach (KeyValuePair<MenuLocation, string> location in classesRootOptions)
            {
                try
                {
                    DeleteAllRCCKeys(location.Value);
                }
                catch // TODO
                {
                }

                foreach (IScriptConfig scriptConfig in configs)
                {
                    if (scriptConfig.IsForLocation(location.Key))
                    {
                        try
                        {
                            CreateScriptConfig(location.Value, scriptConfig);
                        }
                        catch
                        {
                            messagePrompt.PromptOK($"Unable to create or save the script [{scriptConfig.Label}]", "Error creating/saving", MessageType.Error);
                        }
                    }
                }
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
        private void ReadScriptLocation(KeyValuePair<MenuLocation, string> location, ref List<IScriptConfig> results)
        {
            using (RegistryKey scriptLocation = Registry.CurrentUser.OpenSubKey(location.Value, true))
            {
                foreach (string subkeyName in scriptLocation.GetSubKeyNames())
                {
                    try
                    {
                        if (!HasRCCPrefix(subkeyName))
                        {
                            continue;
                        }

                        RegistryScriptStorageModel script = MapRegistryScriptStorageModel(scriptLocation.OpenSubKey(subkeyName));
                        IScriptConfig newConfig = scriptFactory.Generate(script, location.Key);

                        IScriptConfig original = results.FirstOrDefault(r => r.Name == newConfig.Name);
                        if (original == default(IScriptConfig))
                        {
                            results.Add(newConfig);
                        }
                        else
                        {
                            original.ModifyLocation(location.Key, true);
                        }
                    }
                    catch (InvalidDataException e)
                    {
                        messagePrompt.PromptOK(e.Message, "Invalid Data", MessageType.Error);
                    }
                }
            }
        }

        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="IOException"/>
        private void DeleteAllRCCKeys(string location)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(location, true))
            {
                foreach (string subkeyName in key.GetSubKeyNames())
                {
                    try
                    {
                        if (!HasRCCPrefix(subkeyName))
                        {
                            continue;
                        }

                        key.DeleteSubKeyTree(subkeyName, false);
                    }
                    catch // TODO
                    {
                        //Unable to read a child classesRoot key's values
                    }
                }
            }
        }

        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="IOException"/>
        private void CreateScriptConfig(string location, IScriptConfig scriptConfig)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(location, true))
            {
                using (RegistryKey childKey = key.CreateSubKey($"RCC_{scriptConfig.ID}_{scriptConfig.Name}", true))
                {
                    childKey.SetValue(MUIVerb, scriptConfig.Label, RegistryValueKind.String);
                    childKey.SetValue(Icon, scriptConfig.Icon?.ToString() ?? string.Empty);

                    using (RegistryKey commandKey = childKey.CreateSubKey(command))
                    {
                        commandKey.SetValue(string.Empty, $"\"{RCCLocation}\" run {scriptConfig.ScriptArgs}");
                    }
                }
            }
        }

        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private RegistryScriptStorageModel MapRegistryScriptStorageModel(RegistryKey registryKey)
        {
            registryKey = registryKey ?? throw new ArgumentNullException(nameof(registryKey));

            return new RegistryScriptStorageModel()
            {
                Name = registryKey.Name,
                Label = registryKey.GetValue(MUIVerb, string.Empty).ToString(),
                Icon = registryKey.GetValue(Icon, string.Empty).ToString(),
                Command = GetCommandValue(registryKey)
            };

            string GetCommandValue(RegistryKey key)
            {
                string commandValue;
                try
                {
                    using (RegistryKey commandKey = key.OpenSubKey(command))
                    {
                        if (commandKey == null)
                        {
                            return string.Empty;
                        }

                        commandValue = commandKey.GetValue(string.Empty, string.Empty).ToString();
                    }

                    if (!regex.IsMatch(commandValue))
                    {
                        return null;
                    }

                    commandValue = regex.Replace(commandValue, string.Empty);
                }
                catch (Exception)
                {
                    return string.Empty;
                }

                return commandValue;
            }
        }

        private bool HasRCCPrefix(string name)
        {
            return name.Length >= 4 && name.Substring(0, 4) == RCC_;
        }
    }
}
