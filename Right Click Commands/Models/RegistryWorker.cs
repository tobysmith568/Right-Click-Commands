using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

            try
            {
                foreach (string location in classesRootOptions)
                {
                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(location, true))
                    {
                        foreach (string subkey in key.GetSubKeyNames().Where(n => n.StartsWith("RCC_")))
                        {
                            results.Add(AddScriptConfig(key.OpenSubKey(subkey)));
                        }
                    }
                }
            }
            catch // TODO
            {

            }

            return results;
        }

        private ScriptConfig AddScriptConfig(RegistryKey registryKey)
        {
            ScriptConfig result = new ScriptConfig();
            string[] parts = registryKey.Name.Split('\\');
            result.Label = parts[parts.Length - 1].Substring(4);
            return result;
        }
    }
}
