using Ambush.Components;
using Ambush.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Ambush.UserControls
{

    public partial class RPISettings : UserControl
    {
        public static string currentRPI;
        private void clearFields()
        {
            for (int i = 1; FindName("d" + i.ToString()) != null; i++)
            {
                TextBox text = FindName("d" + i.ToString()) as TextBox;
                if (text.Text.ToString() != "")
                {
                    text.Text = "";
                }
            }
        }

        
        private string buildInsertString(string physicalID, string virtualID, string component,string targetID,string defaultState)
        { //Build string according to the component that have his values have changed.
            string table = component;

            switch (component)
            {
                case Constants.Door:
                    {
                        string query = "INSERT INTO " + table + " (physicalID,virtualID,rpi,state,defaultState) VALUES ('" + physicalID + "','" + virtualID + "','" + currentRPI.ToString() + "','" + Constants.Middle.ToString() + "','" + defaultState + "')";
                        return query;
                    }
                case Constants.Target:
                    {
                        string query = "INSERT INTO " + table + " (physicalID,virtualID,rpi,state,defaultState) VALUES ('" + physicalID + "','" + virtualID + "','" + currentRPI.ToString() + "','" + Constants.Middle.ToString() + "','" + defaultState + "')";
                        return query;
                    }
                case Constants.Sensor:
                    {
                        string query = "INSERT INTO " + table + " (physicalID,virtualID,targetID,rpi) VALUES ('" + physicalID + "','" + virtualID + "','" + targetID + "','" + currentRPI.ToString() + "')";
                        return query;
                    }
                default:
                    {
                        Logger.LogHelper.Log(Logger.Logger.LogTarget.File, "Invalid table given at buildInsertString (RPISettings) " + "\n");
                        Console.WriteLine("Invalid table given at buildInsertString (RPISettings) ");
                        throw new ArgumentException("Invalid table given at buildInsertString (RPISettings) ");
                    }

            }
        }

        private void insertToDb(string query)
        {//Execute query string 

            try
            {
                Db_Utils.ExecuteSql(query);
            }
            catch (Exception e) { Logger.LogHelper.Log(Logger.Logger.LogTarget.File, "Exception caught while trying to update database (RPISettings)" + e.StackTrace); }
        }
        private void getInputToDb()
        {//Get user input and update database 
            TextBox rpid = FindName("rpid") as TextBox;
            currentRPI = rpid.Text.ToString();
            for (int i = 1; FindName("c" + i.ToString()) != null; i++)
            {

                ComboBox box = FindName("c" + i.ToString()) as ComboBox;
                TextBox defaultState = FindName("state" + i.ToString()) as TextBox;
                TextBox physicalID = FindName("d" + i.ToString()) as TextBox;
                
                if (box.Text.ToString() == Constants.Door)
                {
                    string query = buildInsertString(physicalID.Text.ToString(), i.ToString(), Constants.Door,"",defaultState.Text.ToString());
                    insertToDb(query);
                }
                else if (box.Text.ToString() == Constants.Target)
                {
                    ComboBox sensor = FindName("s" + i.ToString()) as ComboBox;
                    string sensorVirtualID = sensor.Text.ToString();
                    TextBox sensorPhysicalID = FindName("p" + i.ToString()) as TextBox;

                    if (sensor == null || sensorVirtualID == "" || sensorPhysicalID == null)
                    {
                        MessageBox.Show("Sensor must be assigned to target");
                    }
                    

                    string targetQuery = buildInsertString(physicalID.Text.ToString(), i.ToString(), Constants.Target,"", defaultState.Text.ToString());
                    string sensorQuery = buildInsertString(sensorPhysicalID.Text.ToString(), sensorVirtualID,Constants.Sensor ,physicalID.Text.ToString(),"");
                    
                    insertToDb(targetQuery);
                    insertToDb(sensorQuery);
                }


            }
        }


        public RPISettings()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                getInputToDb();
                Db_Utils.InitComponents(currentRPI);
                clearFields();
                Db_Utils.CloseConnection();
                MessageBox.Show("Settings updated successfully");
            }
            catch (Exception d) { Console.WriteLine(d.ToString()); }

        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            string info1 = "הגדרה ראשונית של רכיבים.\n";
            string info2 = "יצירת הקשרים בין הישויות במתחם, אם אין צורך - נא לא לגעת !";

            GeneralUtils.InfoMessageBox(info1 + info2);
        }
    }
}
