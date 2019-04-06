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

        public ICollection<IScriptConfig> GetScriptConfigs()
        {
            List<IScriptConfig> results = new List<IScriptConfig>();

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

        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        private void ReadParentKey(KeyValuePair<MenuLocation, string> location, ref List<IScriptConfig> results)
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

                        IScriptConfig newConfig = MapScriptConfig(key.OpenSubKey(subkey), location.Key);
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
                    catch // TODO
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
                BatScriptConfig newConfig = new BatScriptConfig(Path.GetFileName(registryKey.Name))
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
        private void CreateScriptConfig(string location, IScriptConfig scriptConfig)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(location, true))
            {
                using (RegistryKey childKey = key.CreateSubKey(scriptConfig.Name, true))
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
