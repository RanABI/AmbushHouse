﻿using Ambush.Components;
using Ambush.UserControls;
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
        public static MySqlConnection cn_Connection;

        
        public static Component CheckIfTriggerNeeded(string physicalId)
        {
            Play game = Play.Instance;
            string query = "SELECT * FROM OnEvent";
            DataTable dataTable = GetDataTable(query);
            if (dataTable.Rows.Count == 0)
                return null;
            foreach (DataRow row in dataTable.Rows)
            {
                if (physicalId == row.ItemArray[0].ToString())
                {
                    Component comp = game.getComponentByPhysicalID(row.ItemArray[1].ToString());
                    return comp;
                }
                return null;
            }

            return null;
        }

        public static DataTable GetDataTable(string query) {
            /* Preform SELECT operations */
            cn_Connection = GetConnection();
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
            if(cn_Connection == null)
            {
                cn_Connection = new MySqlConnection();
                cn_Connection.ConnectionString = Properties.Settings.Default.connection_string;
            }

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
            try
            {
                cn_Connection = GetConnection();

                MySqlCommand cmd = new MySqlCommand(query, cn_Connection);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e) { Console.WriteLine("Exception caught @ Db_Utils.ExecuteSql"); }


        }

        public static void CloseConnection()
        {
            cn_Connection = GetConnection();
            cn_Connection.Close();
        }



        public static List<Component> FillDoorObjects(DataTable dataTable)
        {
            string physicalID = "";
            string virtualID = "";
            Direction dir = Direction.None;
            List<Component> comps = new List<Component>();
            string defaultState;
            if (dataTable.Rows.Count == 0)
                return null;
            foreach (DataRow row in dataTable.Rows)
            {
                //physicalId , virtualId, rpi,state

                try
                {
                    physicalID = row.ItemArray[0].ToString();
                    virtualID = row.ItemArray[1].ToString();
                    dir = Conversions.stringToDirection(row.ItemArray[3].ToString());
                    defaultState = row.ItemArray[4].ToString();
                    Components.Door door = new Components.Door(Int32.Parse(virtualID), Int32.Parse(physicalID));
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
            if (dataTable.Rows.Count == 0)
                return null;
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
            if (dataTable.Rows.Count == 0)
                return null;
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
            Play game = Play.Instance;
            List<CPX> cPXes = game.cPXes;
            string id = "";
            string ip = "";
            if (dataTable.Rows.Count == 0)
                return;
            foreach (DataRow row in dataTable.Rows)
            {
                id = row.ItemArray[0].ToString();
                ip = row.ItemArray[1].ToString();
                cPXes.Add(new CPX(Int32.Parse(id), ip, null));
            }
            game.setCPXes(cPXes);
            return;
        }
        public static void FillMiniConrollers(DataTable dataTable)
        {
            Play game = Play.Instance;
            List<MiniController> cons = game.nods;
            string id = "";
            string ip = "";
            if (dataTable.Rows.Count == 0)
                return;
            foreach(DataRow row in dataTable.Rows)
            {
                id = row.ItemArray[0].ToString();
                ip = row.ItemArray[1].ToString();
                cons.Add(new MiniController(ip,  id));
            }
            game.setControllers(cons);
            return;
        }

        public static List<CPX> InitComponents()
        {
            Play game = Play.Instance;
            string rpid = "";

            //Add CPX objects to CPX list
            string query = "SELECT * FROM RPI";
            DataTable dataTable = Db_Utils.GetDataTable(query);
            FillCPXList(dataTable);
            List<CPX> cPXes = game.cPXes;

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
            query = "SELECT * FROM OnEvent";
            dataTable = GetDataTable(query);
            Dictionary<string, string> triggers = FillTriggersDictionary(dataTable);
            Play.setxTriggeredY(triggers);
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


        public static List<MiniController> InitControllers()
        {
            Play game = Play.Instance;
            //List<Laser> lasers = GetLaserList();
            string query = "SELECT * FROM MiniController";

            DataTable dataTable = Db_Utils.GetDataTable(query);
            FillMiniConrollers(dataTable);
            List<MiniController> cons = game.nods;

            foreach (MiniController con in cons)
            {
                query = "SELECT * FROM Laser WHERE controllerID ='" + con.id.ToString() + "'";
                DataTable data = Db_Utils.GetDataTable(query);
                if (data.Rows.Count != 0)
                {
                    con.type = Constants.LS;
                    con.setLaserList(GetLaserList(data));
                }
                query = "SELECT * FROM Detector WHERE controllerID ='" + con.id.ToString() + "'";
                data = Db_Utils.GetDataTable(query);
                if (data.Rows.Count != 0)
                {
                    con.type = Constants.DT;
                    con.setDetectorList(GetDetectorList(data));
                }

            }
            return cons;
        }
        
        public static List<Laser> GetLaserList(DataTable data)
        {
            string physicalID;
            string virtualID;
            List<Laser> lasers = new List<Laser>();

            foreach (DataRow row in data.Rows)
            {
                physicalID = row.ItemArray[0].ToString();
                virtualID = row.ItemArray[1].ToString();
                lasers.Add(new Laser(Int32.Parse(physicalID), Int32.Parse(virtualID)));
            }
            return lasers;
        }
        public static List<Detector> GetDetectorList(DataTable data)
        {
            string physicalID;
            string virtualID;
            List<Detector> det = new List<Detector>();
            foreach (DataRow row in data.Rows)
            {
                physicalID = row.ItemArray[0].ToString();
                virtualID = row.ItemArray[1].ToString();
                det.Add(new Detector(Int32.Parse(physicalID), Int32.Parse(virtualID)));
            }
            return det;
        }
    }
}
