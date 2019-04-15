using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ambush.Utils
{
    public static class GeneralUtils
    {

        public  static void InfoMessageBox(string msg)
        {
            System.Windows.Forms.MessageBox.Show(msg, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information, System.Windows.Forms.MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.RightAlign);

        }

        public static bool yesNoMessageBox(string msg)
        {
            return System.Windows.MessageBox.Show(msg, "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }
    }
}
