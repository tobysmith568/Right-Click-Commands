using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Dialogs.SaveFile
{
    public interface ISaveFileDialog
    {
        bool Save(string content, string suggestedFileExtension = null, string suggestedFileExtensionName = null, string suggestedFileName = null);
    }
}
