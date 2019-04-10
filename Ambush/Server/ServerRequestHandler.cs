using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ambush.Enums;
using Ambush.Utils;
using Ambush.Components;
using System.Media;
using Ambush.UserControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using Ambush.Timers;
using Ambush.CustomEventArgs;

namespace Ambush.Server
{
    public class ServerRequestHandler
    {
        public Direction direction;


        void UpdateDbNewState(string table, string state, int physicalID)
        {
            string query = "UPDATE " + table + " SET state='" + state + "' WHERE physicalID='" + physicalID.ToString() + "'";
            Db_Utils.ExecuteSql(query);
        }

        public void saveToLog(string component, int physicalID, string action, string answer, string isAgain)
        {
            /* Log to file   */

            StringBuilder builder = new StringBuilder();
            builder.Append(DateTime.Now.ToString());
            builder.Append(" :");
            builder.Append(component + "#");
            builder.Append(physicalID.ToString());
            builder.Append(" -");
            builder.Append(action);
            builder.Append(":");
            builder.Append(answer);
            builder.Append(":");
            if (isAgain != "")
            {
                builder.Append(isAgain);
            }
            Logger.LogHelper.Log(Logger.Logger.LogTarget.File, builder.ToString());

        }

        // --------------------------------------------
        #region Events
        public delegate void BroadcastRplyEventHandler(object sender, BroadcastRplyEventArgs e);
        public static event BroadcastRplyEventHandler BroadcastRply;
        // ----------------------------------------------
        public delegate void ChangeDoorUIEventHandler(object sender, CustomEventArgs.DoorStateChangeArgs e);
        public static event ChangeDoorUIEventHandler ChangeDoorUI;
        // ----------------------------------------------
        public delegate void InvokeTriggerEventHandler(object sender, CustomEventArgs.InvokeTriggerEventArgs e);
        public static event InvokeTriggerEventHandler InvokerTrigger;
        #endregion


        public void HandleClientRequest(string dataReceived)
        {
            string currentState = "";
            int physicalID = 0;
            CPX cpx = null;
            string[] inputs = dataReceived.Split(':');

            /* Possible incoming messages
            ----------------------------------------------------------------
            AR:SET_REPLY:DR:ID:[SUCCESS/FAILURE]:DOOR_DIRECTION:PHYSICALID
            AR:SET_REPLY:TR:ID:[SUCCESS/FAILURE]:TARGET_STATE:PHYSICALID
            ----------------------------------------------------------------
            AR:GET_REPLY:DR:ID:VALUE:PHYSICALID
            AR:GET_REPLY:TR:ID:VALUE:PHYSICALID
            AR:CPX_REPLY:CPX_ID:[ARR_VALS -> Delimiter: '-']
            ----------------------------------------------------------------
            AR:BROD_REPLY:ID:IP
            ----------------------------------------------------------------
            AR:Dxx:SRE:OP_CODE:xxxxxxxx
            ----------------------------------------------------------------
            */
            if (inputs[(int)MsgVars.Action] != Constants.Broadcast_Reply) //Not a broadcast message
            {
                currentState = inputs[(int)MsgVars.State];
                physicalID = Int32.Parse(inputs[(int)MsgVars.PhysicalId]);
                cpx = Play.getCpxByPhysicalId(physicalID);
            }
            if (inputs[(int)MsgVars.Action].StartsWith("D"))
            {
                //Get controller by Id , parse 8-bit message and set lasers state by these values
                string id = inputs[Constants.Detector_ID].Replace("D", string.Empty);
                MiniController con = Play.getControllerById(id);
                switch (inputs[Constants.OP_CODE])
                {
                    /* [AR:Dxx:SRE:OP_CODE:xxxxxxxx] */

                    case Constants.Laser_States:
                        {
                            /* Op_Code = 00 */

                            //Dxx --> Detector, 2 bit id 
                            List<Laser> lasers = new List<Laser>();

                            byte[] bytes = Encoding.ASCII.GetBytes(inputs[5]);
                            for (int i = 0; i < bytes.Length; i++)
                            {
                                var ans = System.Text.Encoding.ASCII.GetString(new[] { bytes[i] });
                                if (ans == "1")
                                    lasers.Add(new Laser(i, State.ON));
                                else lasers.Add(new Laser(i, State.OFF));
                            }
                            con
                                .setLaserList(lasers);
                            break;
                        }
                    case Constants.Laser_Crossed:
                        {
                            /* Op_Code = 11 */

                            Laser laser = null;
                            byte[] bytes = Encoding.ASCII.GetBytes(inputs[5]);
                            for (int i = 0; i < bytes.Length; i++)
                            {
                                var ans = System.Text.Encoding.ASCII.GetString(new[] { bytes[i] });
                                if (ans == "1")
                                {
                                    laser = con.getLaserById(i);
                                    InvokeTriggerEventArgs args = new InvokeTriggerEventArgs();
                                    //args.physicalID = 
                                    //TODO -- > Check how to determine PhysicalID of Laser component and activate following Trigger
                                }
                            }
                            break;
                        }
                    default:
                        {

                            break;
                        }
                }
            }

            switch (inputs[(int)MsgVars.Action])
            {

                case Constants.SET_REPLY:
                    {
                        switch (inputs[(int)MsgVars.Component])
                        {
                            case Constants.TR:
                                {
                                    Target target = null;
                                    /*
                                      AR:SET_REPLY:TR:ID:SUCCESS/FAILURE:DOOR_DIRECTION:PHYSICALID (Set action)
                                    */
                                    if (inputs[(int)MsgVars.Answer] == Constants.Success)
                                    {
                                        /* Save to log & Update Database */
                                        saveToLog(Constants.Target, physicalID, Constants.SET, currentState, "");
                                        UpdateDbNewState(Constants.Target, currentState, physicalID);

                                        /*Update Object  */
                                        foreach (Component cmp in cpx.components)
                                        {
                                            if (cmp.physicalID == physicalID)
                                            {
                                                target = (Components.Target)cmp;
                                                target.state = Conversions.stringToDirection(inputs[(int)MsgVars.State]);
                                                break;
                                            }
                                        }


                                    }
                                    else if (inputs[(int)MsgVars.Answer] == Constants.Failure)
                                    {

                                        /* Retry in case of a failed SET */
                                        foreach (Component cmp in cpx.components)
                                        {
                                            if (cmp.physicalID == physicalID)
                                            {
                                                target = (Target)cmp;
                                                break;
                                            }
                                        }
                                        Direction dir = Conversions.stringToDirection(inputs[(int)MsgVars.State]);
                                        saveToLog(Constants.Target, physicalID, Constants.SET, currentState, "Set failed. Trying again");
                                        if (target != null)
                                            target.setTargetState(dir);

                                    }
                                    break;
                                }
                            case Constants.DR:
                                {

                                    /*AR:SET_REPLY:DR:ID:SUCCESS/FAILURE:DOOR_DIRECTION:PHYSICALID (Set action)*/

                                    Components.Door door = null;
                                    if (inputs[(int)MsgVars.Answer] == Constants.Success)
                                    {

                                        /*Update Object  */
                                        door = (Ambush.Components.Door)Play.getComponentByPhysicalID(physicalID.ToString());
                                        door.failCount = 0;
                                        CustomEventArgs.DoorStateChangeArgs args = new CustomEventArgs.DoorStateChangeArgs();
                                        args.physicalId = door.physicalID;
                                        args.state = Conversions.stringToDirection(inputs[(int)MsgVars.State]);
                                        ChangeDoorUI?.Invoke(this, args);
                                        /* Save to log & Update Database */
                                        saveToLog(Constants.Door, physicalID, Constants.SET, currentState, "");
                                        UpdateDbNewState(Constants.Door, currentState, physicalID);
                                        break;

                                    }
                                    else if (inputs[(int)MsgVars.Answer] == Constants.Failure)
                                    {

                                        /* Retry in case of a failed SET */
                                        door = (Ambush.Components.Door)Play.getComponentByPhysicalID(physicalID.ToString());
                                        Direction dir = Conversions.stringToDirection(inputs[(int)MsgVars.State]);
                                        saveToLog(Constants.Door, physicalID, Constants.SET, currentState, "Set failed. Trying again");
                                        if (door != null)
                                        {
                                            if (door.failCount > 3)
                                                door.setDoorState(dir);
                                            else
                                            {
                                                MessageBox.Show("Door #" + physicalID.ToString() + " is not responding. Please check for errors");
                                                return;
                                            }
                                        }


                                    }
                                    break;
                                }

                        }
                        break;
                    }
                case Constants.GET_REPLY:
                    {
                        switch (inputs[(int)MsgVars.Component])
                        {
                            case Constants.TR:
                                {
                                    /*        
                                       AR:GET_REPLY:TR:ID:[SUCCESS/FAIL]:DOOR_DIRECTION:PYHSICALID
                                    */
                                    Target target = null;
                                    Direction dir = Conversions.stringToDirection(inputs[(int)MsgVars.State]);
                                    foreach (Component cmp in cpx.components)
                                    {
                                        if (cmp.physicalID == physicalID)
                                        {
                                            target = (Target)cmp;
                                            break;
                                        }
                                    }
                                    if (target != null)
                                    {
                                        target.state = dir; ;
                                    }
                                    break;
                                }
                            case Constants.DR:
                                {
                                    /*        
                                       AR:GET_REPLY:DR:ID:[SUCCESS/FAIL]:DOOR_DIRECTION:PYHSICALID
                                    */

                                    Components.Door door = null;
                                    Direction dir = Conversions.stringToDirection(inputs[(int)MsgVars.Answer]);
                                    foreach (Component cmp in cpx.components)
                                    {
                                        if (cmp.physicalID == physicalID)
                                        {
                                            door = (Components.Door)cmp;
                                            break;
                                        }
                                    }
                                    if (door != null)
                                    {
                                        door.state = dir;
                                        //Update database as well
                                    }
                                    break;
                                }

                        }
                        break;
                    }

                case Constants.HIT:
                    {
                        if (inputs[(int)MsgVars.Component] != Constants.SN)
                        {
                            throw new ArgumentException("Invalid Component notifying");
                        }

                        else
                        {
                            /*  AR:HIT:TR:SNVirtualID:HIT_STRENGTH:CPX_ID)  */

                            string SNVirtualID = inputs[(int)MsgVars.VirtualId];
                            string hitStrength = inputs[(int)MsgVars.Answer];
                            string cpxID = inputs[(int)MsgVars.State];
                            string physicalId = "";
                            string query = "SELECT targetID from Sensor WHERE rpi='" + cpxID + "' and sensor='" + SNVirtualID + "'";
                            DataTable dataTable = Db_Utils.GetDataTable(query);
                            foreach (DataRow row in dataTable.Rows)
                            {
                                physicalId = row.ItemArray[0].ToString();
                            }
                            InvokeTriggerEventArgs args = new InvokeTriggerEventArgs();
                            args.physicalID = physicalId;
                            InvokerTrigger(this, args);
                            //TargetClientHandler handler = new TargetClientHandler(Int32.Parse(inputs[(int)MsgVars.Id]));
                            //Turn on led lights Source:Id:True/False (send to MicroController)
                            //handler.TargetLedControl(State.ON);
                            //Invoke sound
                            //handler.InvokeSound();
                            //Add points to player (according to strength of hit)

                            //Wait few seconds and turn off led lights
                            //Enable Sensor on GUI
                            //Raise event 

                        }
                        break;
                    }
                case Constants.INIT:
                    {
                        List<CPX> cPXes = Play.cPXes;
                        int id = Int32.Parse(inputs[2]);
                        string ip = inputs[3];
                        cPXes.Add(new CPX(id, ip, null));
                        Play.setCPXes(cPXes);
                        break;
                    }
                case Constants.Broadcast_Reply:
                    {
                        BroadcastRply?.Invoke(this, new BroadcastRplyEventArgs(inputs[2]));
                        break;
                    }
                case Constants.CPX_REPLY:
                    {
                        //AR: CPX_REPLY: ID:[ARR_VALS]
                        IncomingStatsMessageArgs args = new IncomingStatsMessageArgs(inputs[2], inputs[3].Split('-'));
                        Utils.GlobalEvents.InvokeOnIncomingStatsMessage(this, args);
                        //TODO ==> Change script to return whole array



                        break;
                    }

            }

        }

    }
}
