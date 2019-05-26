using System.IO;

namespace Right_Click_Commands.Models.Scripts
{
    public interface IScriptFactory<T> where T : IScriptStorageModel
    {
        //  Methods
        //  =======

        /// <exception cref="InvalidDataException"></exception>
        IScriptConfig Generate(T input, MenuLocation menuLocation);

        IScriptConfig Generate(string scriptType, string id);
    }
}