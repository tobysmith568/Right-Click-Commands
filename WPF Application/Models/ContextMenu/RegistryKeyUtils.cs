using IconPicker;
using Microsoft.Win32;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.WPF.Models.Scripts;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Right_Click_Commands.WPF.Models.ContextMenu
{
    public static class RegistryKeyUtils
    {
        //  Constants
        //  =========
        
        private const string command = "command";
        private const string cmd = "\"cmd\"";
        private const string powershell = "\"powershell\"";
        private const string reg_AnyWordThenRun = "^\".+?\" run ";

        //  Variables
        //  =========

        private static readonly Regex regex = new Regex(reg_AnyWordThenRun);

        //  Methods
        //  =======

        /// <exception cref="UnauthorizedAccessException"></exception>
        public static BatScriptConfig TryCastToBatScriptConfig(this RegistryKey registryKey, RegistryName registryName, MenuLocation location, ISettings settings)
        {
            BatScriptConfig newConfig = null;
            try
            {
                IIconReference iconReference = null;
                string iconRef = registryKey.GetValue(RegistryWorker.Icon, string.Empty).ToString();
                if (!string.IsNullOrWhiteSpace(iconRef))
                {
                    iconReference = new IconReference(iconRef);
                }

                newConfig = new BatScriptConfig(registryName.Name, registryName.ID, settings)
                {
                    Label = registryKey.GetValue(RegistryWorker.MUIVerb, string.Empty).ToString(),
                    Icon = iconReference
                };

                newConfig.LoadScript();
                newConfig.ModifyLocation(location, true);
            }
            catch (System.Security.SecurityException)
            {
                return null;
            }
            catch (ObjectDisposedException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
            catch (ScriptAccessException)
            {
                return null;
            }

            string commandValue = GetCommandValue(registryKey);

            if (commandValue.Length <= 8 || commandValue.Substring(0, 5) != cmd)
            {
                return null;
            }

            if (commandValue.Substring(7, 2) == BatScriptConfig.keepCMDOpen)
            {
                newConfig.KeepWindowOpen = true;
            }
            else if (commandValue.Substring(7, 2) == BatScriptConfig.closeCMD)
            {
                newConfig.KeepWindowOpen = false;
            }
            else
            {
                return null;
            }

            return newConfig;
        }

        /// <exception cref="UnauthorizedAccessException"></exception>
        public static PowershellScriptConfig TryCastToPowershellScriptConfig(this RegistryKey registryKey, RegistryName registryName, MenuLocation location, ISettings settings)
        {
            PowershellScriptConfig newConfig = null;
            try
            {
                IIconReference iconReference = null;
                string iconRef = registryKey.GetValue(RegistryWorker.Icon, string.Empty).ToString();
                if (!string.IsNullOrWhiteSpace(iconRef))
                {
                    iconReference = new IconReference(iconRef);
                }

                newConfig = new PowershellScriptConfig(registryName.Name, registryName.ID, settings)
                {
                    Label = registryKey.GetValue(RegistryWorker.MUIVerb, string.Empty).ToString(),
                    Icon = iconReference
                };

                newConfig.LoadScript();
                newConfig.ModifyLocation(location, true);
            }
            catch (System.Security.SecurityException)
            {
                return null;
            }
            catch (ObjectDisposedException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
            catch (ScriptAccessException)
            {
                return null;
            }

            string commandValue = GetCommandValue(registryKey);

            if (commandValue.Length <= 12 || commandValue.Substring(0, 12) != powershell)
            {
                return null;
            }

            newConfig.KeepWindowOpen = commandValue.Contains(PowershellScriptConfig.noExit);

            return newConfig;
        }

        private static string GetCommandValue(RegistryKey key)
        {
            key = key ?? throw new ArgumentNullException(nameof(key));

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

        /// <exception cref="UnauthorizedAccessException"></exception>
        private static void ThrowNoRegistryAccess(Exception e = null)
        {
            throw new UnauthorizedAccessException("Unable to access your right-click menu. Please close and re-open the program", e);
        }
    }
}
