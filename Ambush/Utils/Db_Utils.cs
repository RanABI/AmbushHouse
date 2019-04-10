using Ambush.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Ambush.Enums;

namespace Ambush.Utils
{
    public static class Db_Utils
    {
        public static Component CheckIfTriggerNeeded(string physicalId)
        {
            string query = "SELECT * FROM OnEvent";
            DataTable dataTable = GetDataTable(query);
            foreach (DataRow row in dataTable.Rows)
            {
                if (physicalId == row.ItemArray[0].ToString())
                {
                    Component comp = Play.getComponentByPhysicalID(row.ItemArray[1].ToString());
                    return comp;
                }
                return null;
            }

            return null;
        }

        public static DataTable GetDataTable(string query) {
            /* Preform SELECT operations */
            MySqlConnection cn_Connection = GetConnection();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, cn_Connection);
            adapter.Fill(table);

            return table;
        }

        public static string getSelelctSingle(string query)
        {
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = query;
            MySqlDataReader reader = cmd.ExecuteReader();
            string result = "";
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = reader.GetInt32(0).ToString();
                    MessageBox.Show(reader.GetInt32(0).ToString());
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();

            return result;
        }

        public static MySqlConnection GetConnection()
        {
            /* Get Connection string and connect to DB */
            MySqlConnection cn_Connection = new MySqlConnection();
            cn_Connection.ConnectionString = Properties.Settings.Default.connection_string;

            if (cn_Connection.State != System.Data.ConnectionState.Open)
                try
                {
                    cn_Connection.Open();
                    Console.WriteLine("Connected to database");
                }
                catch (MySql.Data.MySqlClient.MySqlException ex) {
                    Console.WriteLine("Failed to connect database");
                    Console.WriteLine(ex.Message);
                }
            else
            {
                Console.WriteLine(cn_Connection.State);
            }
                

            return cn_Connection;
        }

        public static void ExecuteSql(string query)
        {
            /* Execute UPDATE or INSERT operations */
                  
            MySqlConnection cn_Connection = GetConnection();

            MySqlCommand cmd = new MySqlCommand(query, cn_Connection);

            cmd.ExecuteNonQuery();
           
        }

        public static void CloseConnection()
        {
            MySqlConnection cn_Connection = GetConnection();
            cn_Connection.Close();
        }



        public static List<Component> FillDoorObjects(DataTable dataTable)
        {
            string physicalID = "";
            string virtualID = "";
            Direction dir = Direction.None;
            List<Component> comps = new List<Component>();
            string defaultState;
            foreach (DataRow row in dataTable.Rows)
            {
                //physicalId , virtualId, rpi,state

                try
                {
                    physicalID = row.ItemArray[0].ToString();
                    virtualID = row.ItemArray[1].ToString();
                    dir = Conversions.stringToDirection(row.ItemArray[3].ToString());
                    defaultState = row.ItemArray[4].ToString();
                    Door door = new Door(Int32.Parse(virtualID), Int32.Parse(physicalID));
                    door.defaultState = defaultState;
                    comps.Add(door);

                }
                catch (Exception e) { Console.WriteLine("@FillDorObjects " + e.ToString()); }





            }
            return comps;
        }

        public static List<Component> FillTargetObjects(List<Component> comps, DataTable dataTable, List<Sensor> sensors)
        {
            
            string physicalID = "";
            string virtualID = "";
            Sensor sn = null;
            Direction dir;
            string defaultState="";
            foreach (DataRow row in dataTable.Rows)
            {
                //physicalId , virtualId, rpi,state

                try
                {
                    physicalID = row.ItemArray[0].ToString();
                    virtualID = row.ItemArray[1].ToString();
                    dir = Conversions.stringToDirection(row.ItemArray[3].ToString());
                    defaultState = row.ItemArray[4].ToString();

                }
                catch (Exception e) { Console.WriteLine("@FillDorObjects " + e.ToString()); }

                foreach (Sensor sensor in sensors)
                {
                    if (sensor.targetID == Int32.Parse(physicalID))
                        sn = sensor;
                }
                Target target = new Target(Int32.Parse(virtualID), sn, Int32.Parse(physicalID));
                target.defaultState = defaultState;
                comps.Add(target);


            }
            return comps;
        }

        public static List<Sensor> GetSensorList(DataTable dataTable)
        {
            List<Sensor> sensors = new List<Sensor>();

            string physicalID = "";
            string virtualID = "";
            string targetId = "";
            foreach (DataRow row in dataTable.Rows)
            {
                //physicalId , virtualId, rpi,state

                try
                {
                    physicalID = row.ItemArray[0].ToString();
                    virtualID = row.ItemArray[1].ToString();
                    targetId = row.ItemArray[2].ToString();
                }
                catch (Exception e) { Console.WriteLine("@FillDorObjects " + e.ToString()); }
                sensors.Add(new Sensor(Int32.Parse(virtualID), Int32.Parse(physicalID), Int32.Parse(targetId)));




            }
            return sensors;

        }

        public static void FillCPXList(DataTable dataTable)
        {
            List<CPX> cPXes = Play.cPXes;
            string id = "";
            string ip = "";

            foreach (DataRow row in dataTable.Rows)
            {
                id = row.ItemArray[0].ToString();
                ip = row.ItemArray[1].ToString();
                cPXes.Add(new CPX(Int32.Parse(id), ip, null));
            }
            return;
        }

        public static List<CPX> InitComponents()
        {
            string rpid = "";

            //Add CPX objects to CPX list
            string query = "SELECT * FROM RPI";
            DataTable dataTable = Db_Utils.GetDataTable(query);
            FillCPXList(dataTable);
            List<CPX> cPXes = Play.cPXes;

            foreach(CPX cpx in cPXes)
            {
                rpid = cpx.id.ToString();

                //Add Door objects to components list
                query = "SELECT * FROM Door WHERE rpi='" + rpid + "'";
                dataTable = Db_Utils.GetDataTable(query);
                List<Component> comps = FillDoorObjects(dataTable);
        
                //Get Sensor list
                query = "SELECT * FROM Sensor";
                dataTable = Db_Utils.GetDataTable(query);
                List<Sensor> sensors = GetSensorList(dataTable);

                //Add Target and Sensors to components list
                query = "SELECT * FROM Target WHERE rpi='" + rpid + "'";
                dataTable = Db_Utils.GetDataTable(query);
                comps = FillTargetObjects(comps, dataTable, sensors);

                //Update CPX components
                Play.setCpxComponents(rpid, comps);
            }
            return cPXes;
        }

        public static Dictionary<string,string> FillTriggersDictionary(DataTable dataTable)
        {
            Dictionary<string, string> triggers = new Dictionary<string, string>();
            Dictionary<string, string> triggeredToState = new Dictionary<string, string>();
            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    triggers.Add(row.ItemArray[0].ToString(), row.ItemArray[1].ToString());
                    triggeredToState.Add(row.ItemArray[1].ToString(), row.ItemArray[2].ToString());
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("An element with this key already exists.");
                }
            }
            Play.setTriggeredToState(triggeredToState);


            return triggers;
        }

        public static void InitComponents(string rpid)
        {   
                
            //Add Door objects to components list
            string query = "SELECT * FROM Door WHERE rpi='" + rpid + "'";
            DataTable dataTable = Db_Utils.GetDataTable(query);
            List<Component> comps = FillDoorObjects(dataTable);

            //Get Sensor list
            query = "SELECT * FROM Sensor";
            dataTable = Db_Utils.GetDataTable(query);
            List<Sensor> sensors = GetSensorList(dataTable);

            //Add Target and Sensors to components list
            query = "SELECT * FROM Target WHERE rpi='" + rpid + "'";
            dataTable = Db_Utils.GetDataTable(query);
            comps = FillTargetObjects(comps, dataTable, sensors);

            //Update CPX components
            Play.setCpxComponents(rpid, comps);

            query = "SELECT * FROM OnEvent";
            dataTable = GetDataTable(query);
            Dictionary<string, string> triggers = FillTriggersDictionary(dataTable);
            Play.setxTriggeredY(triggers);
            
        }
    }
}
