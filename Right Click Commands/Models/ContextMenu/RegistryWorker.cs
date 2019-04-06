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

        //  Variables
        //  =========

        private readonly string[] classesRootOptions = new string[]
        {
            @"Software\Classes\Directory\Background\shell",
            //@"Software\Classes\Directory\shell"
        };

        //  Methods
        //  =======

        public ICollection<IScriptConfig> GetScriptConfigs()
        {
            List<IScriptConfig> results = new List<IScriptConfig>();

            foreach (string location in classesRootOptions)
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
            foreach (string location in classesRootOptions)
            {
                try
                {
                    DeleteAllRCCKeys(location);
                }
                catch // TODO
                {
                }

                foreach (IScriptConfig scriptConfig in configs)
                {
                    try
                    {
                        CreateScriptConfig(location, scriptConfig);
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
        private void ReadParentKey(string location, ref List<IScriptConfig> results)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(location, true))
            {
                foreach (string subkey in key.GetSubKeyNames())
                {
                    try
                    {
                        if (subkey.Length < 4 || subkey.Substring(0, 4) != RCC_)
                            continue;

                        results.Add(MapScriptConfig(key.OpenSubKey(subkey)));
                    }
                    catch // TODO
                    {
                        //Unable to read a child classesRoot key's values
                    }
                }
            }
        }

        /// <exception cref="UnauthorizedAccessException"></exception>
        private BatScriptConfig MapScriptConfig(RegistryKey registryKey)
        {
            try
            {
                return new BatScriptConfig(Path.GetFileName(registryKey.Name))
                {
                    Label = registryKey.GetValue(MUIVerb, "").ToString(),
                    Icon = registryKey.GetValue(Icon, "").ToString()// TODO
                };
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException("Cannot access registry key value", e);
            }
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
                    childKey.SetValue(Icon, "");// TODO
                }
            }
        }
    }
}
