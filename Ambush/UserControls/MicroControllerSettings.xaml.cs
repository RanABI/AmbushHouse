using Ambush.Components;
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
    /// Interaction logic for MicroControllerSettings.xaml
    /// </summary>
    public partial class MicroControllerSettings : Window
    {
        public MicroControllerSettings()
        {
            InitializeComponent();
        }


        public void InputToDb(string query)
        {
            try
            {
                Db_Utils.ExecuteSql(query);
            }
            catch (Exception e) { Console.WriteLine("Exception caught while trying to update database (MINICONTROLLER)"); } //Logger.LogHelper.Log(Logger.Logger.LogTarget.File, "Exception caught while trying to update database (RPISettings)" + e.StackTrace); }

        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MiniController con = null;
            string query;
            ComboBox type = FindName("type") as ComboBox;
            TextBox conId = FindName("conId") as TextBox;
            if (type.Text.ToString() == "Detector")
            {
                con = new MiniController("", new List<Detector>(), conId.Text.ToString(), Constants.DT);
            }
            else if (type.Text.ToString() == "Laser")
            {
                con = new MiniController("", new List<Laser>(), conId.Text.ToString(), Constants.LS);
            }

            if (type.Text.ToString() == "" || conId.Text.ToString() == "")
            {
                MessageBox.Show("Type and Id must be filled.");
                return;
            }

            for (int i = 0; FindName("c" + i.ToString()) != null; i++)
            {
                TextBox physicalId = FindName("c" + i.ToString()) as TextBox;
                if(physicalId.Text.ToString() != "")
                {
                    if(type.Text.ToString() == "Detector")
                    {
                        con.detectors.Add(new Detector(Int32.Parse(physicalId.Text.ToString()), i));
                        query = "INSERT INTO Detector VALUES ('" + physicalId.Text.ToString() + "','" + i.ToString() + "','" + conId.Text.ToString() + "')";
                        InputToDb(query);
                    }
                    else if(type.Text.ToString() == "Laser")
                    {
                        con.lasers.Add(new Laser(Int32.Parse(physicalId.Text.ToString()), i));
                        query = "INSERT INTO Laser VALUES ('" + physicalId.Text.ToString() + "','" + i.ToString() + "','" + conId.Text.ToString() + "')";
                        InputToDb(query);
                    }
                }

            }
            Play.addController(con);
            MessageBox.Show("Database updated successfully");
            this.Close();


        }
    }
}
