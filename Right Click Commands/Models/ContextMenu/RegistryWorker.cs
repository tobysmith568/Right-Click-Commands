using Microsoft.Win32;
using Right_Click_Commands.Models.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private const string RCC_ = "RCC_";
        private const string cmd = "cmd";
        private const string KeepCMDOpen = "/K";
        private const string command = "command";
        private const string keepCMDOpen = "/K";
        private const string closeCMD = "/C";

        //  Variables
        //  =========
        
        private readonly Dictionary<MenuLocation, string> classesRootOptions = new Dictionary<MenuLocation, string>()
        {
            { MenuLocation.Background, @"Software\Classes\Directory\Background\shell" },
            { MenuLocation.Directory, @"Software\Classes\Directory\shell" }
        };

        //  Methods
        //  =======

        public ICollection<ScriptConfig> GetScriptConfigs()
        {
            List<ScriptConfig> results = new List<ScriptConfig>();

            foreach (KeyValuePair<MenuLocation, string> location in classesRootOptions)
            {
                try
                {
                    ReadParentKey(location, ref results);
                }
                catch // TODO
                {
                    //Unable to read a classesRoot key
                }
            }

            return results;
        }

        public void SaveScriptConfigs(ICollection<ScriptConfig> configs)
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

                foreach (ScriptConfig scriptConfig in configs)
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

        public ScriptConfig New(string id)
        {
            return new BatScriptConfig(DateTime.UtcNow.Ticks.ToString(), id)
            {
                Label = "New Script",
                OnBackground = true,
                OnDirectory = true
            };
        }

        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        private void ReadParentKey(KeyValuePair<MenuLocation, string> location, ref List<ScriptConfig> results)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(location.Value, true))
            {
                foreach (string subkey in key.GetSubKeyNames())
                {
                    try
                    {
                        if (subkey.Length < 4 || subkey.Substring(0, 4) != RCC_)
                        {
                            continue;
                        }

                        ScriptConfig newConfig = MapScriptConfig(key.OpenSubKey(subkey), location.Key);
                        ScriptConfig original = results.FirstOrDefault(r => r.Name == newConfig.Name);

                        if (original == null)
                        {
                            results.Add(newConfig);
                        }
                        else
                        {
                            original.ModifyLocation(location.Key, true);
                        }
                    }
                    catch (Exception e) // TODO
                    {
                        //Unable to read a child classesRoot key's values
                    }
                }
            }
        }

        /// <exception cref="UnauthorizedAccessException"></exception>
        private BatScriptConfig MapScriptConfig(RegistryKey registryKey, MenuLocation location)
        {
            try
            {
                string[] fullAddressParts = Path.GetFileName(registryKey.Name).Split('_');

                if (fullAddressParts.Length != 3)
                {
                    ThrowFoundInvalidKey(registryKey.Name);
                }

                if (fullAddressParts[0] != "RCC")
                {
                    ThrowFoundInvalidKey(registryKey.Name);
                }

                if (fullAddressParts[1].Length != 2 || !int.TryParse(fullAddressParts[1], out int @int))
                {
                    ThrowFoundInvalidKey(registryKey.Name);
                }

                if (fullAddressParts[2].Length < 1)
                {
                    ThrowFoundInvalidKey(registryKey.Name);
                }

                BatScriptConfig newConfig = new BatScriptConfig(fullAddressParts[2], fullAddressParts[1])
                {
                    Label = registryKey.GetValue(MUIVerb, string.Empty).ToString(),
                    Icon = registryKey.GetValue(Icon, string.Empty).ToString()// TODO
                };
                newConfig.ModifyLocation(location, true);

                using (RegistryKey commandKey = registryKey.OpenSubKey(command))
                {
                    if (commandKey == null)
                    {
                        ThrowFoundCorruptKey(newConfig.Label);
                    }

                    string command = commandKey.GetValue(string.Empty, string.Empty).ToString();

                    if (command.Length <= 6 || command.Substring(0, 3) != cmd)
                    {
                        ThrowFoundCorruptKey(newConfig.Label);
                    }

                    if (command.Substring(4, 2) == keepCMDOpen)
                    {
                        newConfig.KeepWindowOpen = true;
                    }
                    else if (command.Substring(4, 2) == closeCMD)
                    {
                        newConfig.KeepWindowOpen = false;
                    }
                    else
                    {
                        ThrowFoundCorruptKey(newConfig.Label);
                    }
                }

                return newConfig;
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException("Cannot access registry key value", e);
            }
        }

        /// <exception cref="InvalidDataException"></exception>
        private void ThrowFoundCorruptKey(string label)
        {
            throw new InvalidDataException($"The right-click command [{label}] appears to be corrupt. Please delete and re-create it");
        }

        /// <exception cref="InvalidDataException"></exception>
        private void ThrowFoundInvalidKey(string name)
        {
            throw new ArgumentException($"The given registry keys name [{name}] must be [RRC_XX_YYY] where [XX] is a number and [YYY] is of any length greater than 0");
        }

        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
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

        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
        private void CreateScriptConfig(string location, ScriptConfig scriptConfig)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(location, true))
            {
                using (RegistryKey childKey = key.CreateSubKey($"RCC_{scriptConfig.ID}_{scriptConfig.Name}", true))
                {
                    childKey.SetValue(MUIVerb, scriptConfig.Label, RegistryValueKind.String);
                    childKey.SetValue(Icon, string.Empty);// TODO

                    using (RegistryKey commandKey = childKey.CreateSubKey(command))
                    {
                        commandKey.SetValue("", $"cmd {(scriptConfig.KeepWindowOpen ? keepCMDOpen : closeCMD)} TITLE {scriptConfig.Label}&\"{scriptConfig.ScriptLocation}\"");
                    }
                }
            }
        }
    }
}
