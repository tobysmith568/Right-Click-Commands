using Microsoft.Win32;
using Right_Click_Commands.Models.Scripts;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Right_Click_Commands.Models.ContextMenu
{
    public static class RegistryKeyUtils
    {

        private const string MUIVerb = "MUIVerb";
        private const string Icon = "Icon";
        private const string command = "command";
        private const string cmd = "\"cmd\"";
        private const string keepCMDOpen = "/K";
        private const string closeCMD = "/C";

        /// <exception cref="ScriptAccessException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public static BatScriptConfig TryCastToBatScriptConfig(this RegistryKey registryKey, RegistryName registryName, MenuLocation location)
        {
            BatScriptConfig newConfig = null;
            try
            {
                newConfig = new BatScriptConfig(registryName.Name, registryName.ID)
                {
                    Label = registryKey.GetValue(MUIVerb, string.Empty).ToString(),
                    Icon = registryKey.GetValue(Icon, string.Empty).ToString()// TODO
                };
                newConfig.LoadScript();
                newConfig.ModifyLocation(location, true);

                using (RegistryKey commandKey = registryKey.OpenSubKey(command))
                {
                    if (commandKey == null)
                    {
                        ThrowFoundCorruptKey(newConfig.Label);
                    }

                    string command = commandKey.GetValue(string.Empty, string.Empty).ToString();

                    Regex regex = new Regex("^\".+?\" run ");

                    if (!regex.IsMatch(command))
                    {
                        ThrowFoundCorruptKey(newConfig.Label);
                    }

                    command = regex.Replace(command, string.Empty);

                    if (command.Length <= 8 || command.Substring(0, 5) != cmd)
                    {
                        ThrowFoundCorruptKey(newConfig.Label);
                    }

                    if (command.Substring(7, 2) == keepCMDOpen)
                    {
                        newConfig.KeepWindowOpen = true;
                    }
                    else if (command.Substring(7, 2) == closeCMD)
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
            catch (Exception e) when (!(e is InvalidDataException))
            {
                throw new InvalidDataException($"The right-click command [{newConfig.Label}] appears to be corrupt. Please delete and re-create it", e);
            }
        }

        /// <exception cref="InvalidDataException"/>
        private static void ThrowFoundCorruptKey(string label, Exception inner = null)
        {
            throw new InvalidDataException($"The right-click command [{label}] appears to be corrupt. Please delete and re-create it", inner);
        }
    }
}
