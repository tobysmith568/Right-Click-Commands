using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Right_Click_Commands.WPF.Views
{
    /// <summary>
    /// Interaction logic for ScriptError.xaml
    /// </summary>
    public partial class ScriptError : Window
    {
        public ScriptError(string error)
        {
            InitializeComponent();
            TbError.Text = error ?? string.Empty;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
