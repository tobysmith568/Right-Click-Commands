using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models
{
    public class RegistryWorker : IScriptWorker
    {
        private readonly string[] classesRootOptions = new string[]
        {
            @"Directory\Background\shell",
            @"Directory\shell"
        };

        public ICollection<ScriptConfig> GetScriptConfigs()
        {
            List<ScriptConfig> results = new List<ScriptConfig>();

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

        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        private void ReadParentKey(string location, ref List<ScriptConfig> results)
        {
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(location, true))
            {
                foreach (string subkey in key.GetSubKeyNames())
                {
                    try
                    {
                        if (subkey.Length < 4 || subkey.Substring(0, 4) != "RCC_")
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
        private ScriptConfig MapScriptConfig(RegistryKey registryKey)
        {
            try
            {
                return new ScriptConfig(Path.GetFileName(registryKey.Name))
                {
                    Label = registryKey.GetValue("MUIVerb", "").ToString(),
                    Icon = registryKey.GetValue("Icon", "").ToString()
                };
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException("Cannot access registry key value", e);
            }
        }
    }
}
