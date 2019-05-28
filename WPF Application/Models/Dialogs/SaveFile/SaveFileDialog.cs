using Right_Click_Commands.Models.Dialogs.SaveFile;
using System;
using System.IO;

namespace Right_Click_Commands.WPF.Models.Dialogs.SaveFile
{
    public class SaveFileDialog : ISaveFileDialog
    {
        //  Methods
        //  =======

        public bool Save(string content, string suggestedFileExtension = null, string suggestedFileExtensionName = null, string suggestedFileName = null)
        {
            string filter = "All Files|*.*";

            if (suggestedFileExtension != null)
            {
                filter = $"{suggestedFileExtensionName ?? string.Empty}|*{suggestedFileExtension}|" + filter;
            }

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = suggestedFileName ?? string.Empty,
                DefaultExt = suggestedFileExtension ?? string.Empty,
                Filter = filter
            };

            switch (dlg.ShowDialog())
            {
                case true:
                    return WriteFile(dlg.FileName, content);
                default:
                    return false;
            }
        }

        private bool WriteFile(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                return true;
            }
            catch (PathTooLongException)
            {
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (System.Security.SecurityException)
            {
                return false;
            }
        }
    }
}
