using Microsoft.Win32;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.ContextMenu
{
    public class RegistryWorker : IContextMenuWorker
    {
        //  Constants
        //  =========

        private const string MUIVerb = "MUIVerb";
        private const string Icon = "Icon";
        private const string RCC = "RCC";
        private const string RCC_ = "RCC_";
        private const string cmd = "cmd";
        private const string command = "command";
        private const string NewScript = "New Script";

        //  Variables
        //  =========

        private readonly string RCCLocation = Assembly.GetEntryAssembly().Location;

        private readonly Dictionary<MenuLocation, string> classesRootOptions = new Dictionary<MenuLocation, string>()
        {
            { MenuLocation.Background, @"Software\Classes\Directory\Background\shell" },
            { MenuLocation.Directory, @"Software\Classes\Directory\shell" }
        };

        private readonly IMessagePrompt messagePrompt;

        //  Constructors
        //  ============

        public RegistryWorker(IMessagePrompt messagePrompt)
        {
            this.messagePrompt = messagePrompt;
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
                    ReadClassRoot(location, ref results);
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
                    try
                    {
                        if (scriptConfig.IsForLocation(location.Key))
                        {
                            CreateScriptConfig(location.Value, scriptConfig);
                        }
                    }
                    catch // TODO
                    {
                    }
                }
            }
        }

        /// <exception cref="ScriptAccessException"></exception>
        public IScriptConfig New(ScriptType scriptType, string id)
        {
            IScriptConfig result;

            switch (scriptType)
            {
                case ScriptType.Batch:
                    result = new BatScriptConfig(DateTime.UtcNow.Ticks.ToString(), id);
                    break;
                case ScriptType.Powershell:
                    result = new PowershellScriptConfig(DateTime.UtcNow.Ticks.ToString(), id);
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
        private void ReadClassRoot(KeyValuePair<MenuLocation, string> location, ref List<IScriptConfig> results)
        {
            using (RegistryKey classRoot = Registry.CurrentUser.OpenSubKey(location.Value, true))
            {
                foreach (string subkey in classRoot.GetSubKeyNames())
                {
                    try
                    {
                        if (subkey.Length < 4 || subkey.Substring(0, 4) != RCC_)
                        {
                            continue;
                        }

                        IScriptConfig newConfig = MapScriptConfig(classRoot.OpenSubKey(subkey), location.Key);
                        IScriptConfig original = results.FirstOrDefault(r => r.Name == newConfig.Name);

                        if (original == null)
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
                    catch (Exception e)// TODO
                    {

                    }
                }
            }
        }

        /// <exception cref="ScriptAccessException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        private IScriptConfig MapScriptConfig(RegistryKey registryKey, MenuLocation location)
        {
            if (!IsValidRegistryKeyName(registryKey.Name, out RegistryName registryName))
            {
                throw new ArgumentException($"The given registry keys name [{registryKey.Name}] must be [RRC_XX_YYY] where [XX] is a number and [YYY] is of any length greater than 0");
            }

            IScriptConfig newConfig = registryKey.TryCastToBatScriptConfig(registryName, location);

            if (newConfig == null)
            {
                throw new NotImplementedException();
            }

            return newConfig;
        }

        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="IOException"/>
        private void DeleteAllRCCKeys(string location)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(location, true))
            {
                foreach (string subkey in key.GetSubKeyNames())
                {
                    try
                    {
                        if (subkey.Length < 4 || subkey.Substring(0, 4) != RCC_)
                            continue;

                        key.DeleteSubKeyTree(subkey, false);
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
                    childKey.SetValue(Icon, string.Empty);// TODO

                    using (RegistryKey commandKey = childKey.CreateSubKey(command))
                    {
                        commandKey.SetValue(string.Empty, $"\"{RCCLocation}\" run {scriptConfig.ScriptArgs}");
                    }
                }
            }
        }

        private bool IsValidRegistryKeyName(string value, out RegistryName registryName)
        {
            registryName = new RegistryName();

            if (value == null)
            {
                return false;
            }

            string[] fullAddressParts = Path.GetFileName(value).Split('_');

            if (fullAddressParts.Length != 3)
            {
                return false;
            }

            if (fullAddressParts[0] != RCC)
            {
                return false;
            }

            if (fullAddressParts[1].Length != 2 || !int.TryParse(fullAddressParts[1], out int @int))
            {
                return false;
            }

            if (fullAddressParts[2].Length < 1)
            {
                return false;
            }

            registryName = new RegistryName(fullAddressParts[1], fullAddressParts[2]);
            return true;
        }
    }
}
