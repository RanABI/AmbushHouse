using Ambush.Utils;
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

namespace Ambush.UserControls
{
    /// <summary>
    /// Interaction logic for Trigger.xaml
    /// </summary>
    public partial class Trigger : Window
    {
        public Trigger()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1; FindName("d" + i.ToString()) != null; i++)
            {
                
                //Get the component that triggers the event
                TextBox subject = FindName("d" + i.ToString()) as TextBox;
                TextBox newState = FindName("s" + i.ToString()) as TextBox;
                string triggered = subject.Text.ToString();
                if(triggered != "")
                {
                    subject = FindName("a" + i.ToString()) as TextBox;
                    string invoke = subject.Text.ToString();

                    string query = "INSERT INTO OnEvent (currentComp,nextComp,nextState) VALUES ('" + triggered + "','" + invoke + "','" + newState.Text.ToString() + "')";
                    Db_Utils.ExecuteSql(query);

                }
            }
            MessageBox.Show("Triggers have been successfully been updated");


           }

        private void Clear_Click(object sender,RoutedEventArgs e)
        {
            //Delete all rows of specified level
            string query = "DELETE * FROM OnEvent";
            Db_Utils.ExecuteSql(query);
            MessageBox.Show("Triggers have been cleared.");
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("עריכה של פרופילים - העמודה השמאלית מכוונת לאובייקט שברגע שהמשחק מגיע אליו - הוא יקרא לאובייקט מהעמודה הימנית. לדוגמא, כאשר מטרה מספר עשר נפגעת - פתח דלת מס' שבע\n יש לדעת - ברגע שלוחצים על כפתור השמירה, כל הפרופילים הקודמים שהיו לאותה רמה שנבחרה - ימחקו ! ");
        }
    }
}
