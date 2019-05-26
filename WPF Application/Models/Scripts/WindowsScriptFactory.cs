using IconPicker;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.WPF.Models.ContextMenu;
using Right_Click_Commands.Utils;
using System;
using System.IO;

namespace Right_Click_Commands.WPF.Models.Scripts
{
    public class WindowsScriptFactory : IScriptFactory<IScriptStorageModel>
    {
        //  Constants
        //  =========

        private const string RCC = "RCC";
        private const string cmd = "\"cmd\"";
        private const string powershell = "\"powershell\"";
        private const string command = "command";
        private const string NewScript = "New Script";

        //  Variables
        //  =========

        private readonly ISettings settings;
        private readonly IMessagePrompt messagePrompt;
        private readonly IIconPicker iconPicker;

        //  Constructors
        //  ============

        public WindowsScriptFactory(ISettings settings, IMessagePrompt messagePrompt, IIconPicker iconPicker)
        {
            this.settings = settings;
            this.messagePrompt = messagePrompt;
            this.iconPicker = iconPicker;
        }

        //  Methods
        //  =======

        /// <exception cref="InvalidDataException"></exception>
        public IScriptConfig Generate(IScriptStorageModel script, MenuLocation menuLocation)
        {
            if (!IsValidRegistryKeyName(script.Name, out RegistryName registryName))
            {
                throw new InvalidDataException($"The given registry key's name [{script.Name}] must be [RRC_XX_YYY] where [XX] is a number and [YYY] is of any length greater than 0");
            }

            IScriptConfig newConfig = null;

            try
            {
                newConfig = TryCastToBatScriptConfig(script, registryName, menuLocation);

                if (newConfig == null)
                {
                    newConfig = TryCastToPowershellScriptConfig(script, registryName, menuLocation);
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

        public IScriptConfig Generate(string scriptTypeString, string id)
        {
            ScriptType scriptType = scriptTypeString.ToEnum<ScriptType>();
            return Generate(scriptType, id);
        }

        private IScriptConfig Generate(ScriptType scriptType, string id)
        {
            IScriptConfig result;

            switch (scriptType)
            {
                case ScriptType.Batch:
                    result = new BatScriptConfig(DateTime.UtcNow.Ticks.ToString(), id, settings, messagePrompt, iconPicker);
                    break;
                case ScriptType.Powershell:
                    result = new PowershellScriptConfig(DateTime.UtcNow.Ticks.ToString(), id, settings, messagePrompt, iconPicker);
                    break;
                default:
                    throw new ArgumentException($"The scriptType of [{scriptType}] is not valid for the [RegistryWorker]");
            }

            result.Label = NewScript;
            result.OnBackground = true;
            result.OnDirectory = true;

            return result;
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
        private BatScriptConfig TryCastToBatScriptConfig(IScriptStorageModel script, RegistryName registryName, MenuLocation location)
        {
            BatScriptConfig newConfig = null;
            try
            {
                IIconReference iconReference = null;
                if (!string.IsNullOrWhiteSpace(script.Icon))
                {
                    iconReference = new IconReference(script.Icon);
                }

                newConfig = new BatScriptConfig(registryName.Name, registryName.ID, settings, messagePrompt, iconPicker)
                {
                    Label = script.Label,
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

            if (script.Command.Length <= 8 || script.Command.Substring(0, 5) != cmd)
            {
                return null;
            }

            if (script.Command.Substring(7, 2) == BatScriptConfig.keepCMDOpen)
            {
                newConfig.KeepWindowOpen = true;
            }
            else if (script.Command.Substring(7, 2) == BatScriptConfig.closeCMD)
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
        private PowershellScriptConfig TryCastToPowershellScriptConfig(IScriptStorageModel script, RegistryName registryName, MenuLocation location)
        {
            PowershellScriptConfig newConfig = null;
            try
            {
                IIconReference iconReference = null;
                if (!string.IsNullOrWhiteSpace(script.Icon))
                {
                    iconReference = new IconReference(script.Icon);
                }

                newConfig = new PowershellScriptConfig(registryName.Name, registryName.ID, settings, messagePrompt, iconPicker)
                {
                    Label = script.Label,
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

            if (script.Command.Length <= 12 || script.Command.Substring(0, 12) != powershell)
            {
                return null;
            }

            newConfig.KeepWindowOpen = script.Command.Contains(PowershellScriptConfig.noExit);

            return newConfig;
        }
    }
}
