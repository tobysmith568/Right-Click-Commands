using Microsoft.Win32;
using Right_Click_Commands.Models.ContextMenu;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.Utils;
using Right_Click_Commands.WPF.Models.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Right_Click_Commands.WPF.Models.ContextMenu
{
    public class RegistryWorker : IContextMenuWorker
    {
        //  Constants
        //  =========

        public const string MUIVerb = "MUIVerb";
        public const string Icon = "Icon";
        private const string RCC_ = "RCC_";
        private const string command = "command";
        private const string NewScript = "New Script";

        //  Variables
        //  =========

        private readonly IScriptFactory<RegistryKey> scriptFactory;
        private readonly IMessagePrompt messagePrompt;
        private readonly ISettings settings;

        private readonly string RCCLocation = Assembly.GetEntryAssembly().Location;

        private readonly Dictionary<MenuLocation, string> classesRootOptions = new Dictionary<MenuLocation, string>()
        {
            { MenuLocation.Background, @"Software\Classes\Directory\Background\shell" },
            { MenuLocation.Directory, @"Software\Classes\Directory\shell" }
        };


        //  Constructors
        //  ============

        public RegistryWorker(IScriptFactory<RegistryKey> scriptFactory, IMessagePrompt messagePrompt, ISettings settings)
        {
            this.scriptFactory = scriptFactory;
            this.messagePrompt = messagePrompt;
            this.settings = settings;
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

        /// <exception cref="ScriptAccessException"></exception>
        public IScriptConfig New(string scriptType, string id)
        {
            IScriptConfig result;

            switch (scriptType.ToEnum<ScriptType>())
            {
                case ScriptType.Batch:
                    result = new BatScriptConfig(DateTime.UtcNow.Ticks.ToString(), id, settings, messagePrompt);
                    break;
                case ScriptType.Powershell:
                    result = new PowershellScriptConfig(DateTime.UtcNow.Ticks.ToString(), id, settings, messagePrompt);
                    break;
                default:
                    throw new ArgumentException($"The scriptType of [{scriptType}] is not valid for the [RegistryWorker]");
            }

            result.Label = NewScript;
            result.OnBackground = true;
            result.OnDirectory = true;

            return result;
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

                        IScriptConfig newConfig = scriptFactory.Generate(scriptLocation.OpenSubKey(subkeyName), location.Key);

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

        private bool HasRCCPrefix(string name)
        {
            return name.Length >= 4 && name.Substring(0, 4) == RCC_;
        }
    }
}
