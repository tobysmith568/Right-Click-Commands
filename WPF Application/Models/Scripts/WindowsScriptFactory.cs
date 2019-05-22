using IconPicker;
using Microsoft.Win32;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.WPF.Models.ContextMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Right_Click_Commands.WPF.Models.Scripts
{
    public class WindowsScriptFactory : IScriptFactory<RegistryKey>
    {
        //  Constants
        //  =========

        private const string RCC = "RCC";
        private const string cmd = "\"cmd\"";
        private const string powershell = "\"powershell\"";
        private const string command = "command";
        private const string reg_AnyWordThenRun = "^\".+?\" run ";

        //  Variables
        //  =========

        private readonly ISettings settings;
        private static readonly Regex regex = new Regex(reg_AnyWordThenRun);

        //  Constructors
        //  ============

        public WindowsScriptFactory(ISettings settings)
        {
            this.settings = settings;
        }

        //  Methods
        //  =======

        /// <exception cref="InvalidDataException"></exception>
        public IScriptConfig Generate(RegistryKey registryKey, MenuLocation menuLocation)
        {
            if (!IsValidRegistryKeyName(registryKey.Name, out RegistryName registryName))
            {
                throw new InvalidDataException($"The given registry key's name [{registryKey.Name}] must be [RRC_XX_YYY] where [XX] is a number and [YYY] is of any length greater than 0");
            }

            IScriptConfig newConfig = null;

            try
            {
                newConfig = TryCastToBatScriptConfig(registryKey, registryName, menuLocation);

                if (newConfig == null)
                {
                    newConfig = TryCastToPowershellScriptConfig(registryKey, registryName, menuLocation);
                }

                if (newConfig == null)
                {
                    throw new InvalidDataException($"The right-click command [{registryName.Name}] appears to be corrupt. Please delete and re-create it");
                }
            }
            catch (UnauthorizedAccessException)
            {
                //Nothing needed
            }

            return newConfig;
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

        /// <exception cref="UnauthorizedAccessException"></exception>
        private BatScriptConfig TryCastToBatScriptConfig(RegistryKey registryKey, RegistryName registryName, MenuLocation location)
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
        private PowershellScriptConfig TryCastToPowershellScriptConfig(RegistryKey registryKey, RegistryName registryName, MenuLocation location)
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
