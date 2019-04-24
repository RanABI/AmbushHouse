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
using static Ambush.Enums;

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
            Play game = Play.Instance;
            string query;
            TextBox laser_id = FindName("laser_id") as TextBox;
            TextBox detect_id = FindName("detect_id") as TextBox;
            string default_con_state = "";
            MiniController detect_con = new MiniController("", new List<Detector>(), detect_id.Text.ToString(), Constants.DT);

            MiniController laser_con = new MiniController("", new List<Laser>(), laser_id.Text.ToString(), Constants.LS);
            

            if (laser_id.Text.ToString() == "" || detect_id.Text.ToString() == "")
            {
                MessageBox.Show("Id's must be filled out.");
                return;
            }

            for (int i = 0; FindName("c" + i.ToString()) != null; i++)
            {
                TextBox physicalId = FindName("c" + i.ToString()) as TextBox;
                ComboBox state = FindName("state" + i.ToString()) as ComboBox;
                State st;
                if (state.Text.ToString() == "On")
                {
                    default_con_state += "1";
                    st = State.ON;
                }
                else
                {
                    default_con_state += "0";
                    st = State.OFF;
                }
                if(physicalId.Text.ToString() != "")
                {
                    detect_con.detectors.Add(new Detector(Int32.Parse(physicalId.Text.ToString()), i,st));
                    query = "INSERT INTO Detector VALUES ('" + physicalId.Text.ToString() + "','" + i.ToString() + "','" + detect_id.Text.ToString() + "','" + Enums.StateToString(st) + "')";
                    InputToDb(query);

                    laser_con.lasers.Add(new Laser(Int32.Parse(physicalId.Text.ToString()), i,st));
                    query = "INSERT INTO Laser VALUES ('" + physicalId.Text.ToString() + "','" + i.ToString() + "','" + laser_id.Text.ToString() + "','" + Enums.StateToString(st) + "')";
                    InputToDb(query);
                    
                }

            }
            query = "INSERT INTO MiniController VALUES ('" + laser_con.id.ToString() + "','')";
            InputToDb(query);
            query = "INSERT INTO MiniController VALUES ('" + detect_con.id.ToString() + "','')";
            InputToDb(query);

            laser_con.default_state_string = default_con_state;
            detect_con.default_state_string = default_con_state;
            
            game.AddToDetectorToLaserController(detect_id.Text.ToString(), laser_id.Text.ToString());
            game.addController(laser_con);
            game.addController(detect_con);

            MessageBox.Show("Database updated successfully");
            this.Close();


        }
    }
}
