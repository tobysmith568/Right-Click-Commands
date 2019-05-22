using System.IO;

namespace Right_Click_Commands.Models.Scripts
{
    public interface IScriptFactory<T>
    {
        //  Methods
        //  =======

        /// <exception cref="InvalidDataException"></exception>
        IScriptConfig Generate(T input, MenuLocation menuLocation);
    }
}