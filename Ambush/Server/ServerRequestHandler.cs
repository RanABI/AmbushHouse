using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ambush.Enums;
using Ambush.Utils;
using Ambush.Components;
using System.Media;

namespace Ambush.Server
{
    class ServerRequestHandler
    {
        public void HandleClientRequest(string dataReceived)
        {
            StringBuilder builder = new StringBuilder();
            string[] inputs = dataReceived.Split(':');
            string command;
            //AR:SET:DR:ID:ON
            switch (inputs[(int)MsgVars.Action])
            {

                case "REPLY":
                    {
                        switch (inputs[(int)MsgVars.Component])
                        {
                            case "TR":
                                {
                                    TargetHandler handler = new TargetHandler(Int32.Parse(inputs[(int)MsgVars.Id]));
                                    //Turn on led lights Source:Id:True/False (send to MicroController)
                                    handler.TargetLedControl(State.ON);
                                    //Invoke sound
                                    handler.InvokeSound();
                                    //Add points to player (according to strength of hit)

                                    //Wait few seconds and turn off led lights
                                    break;
                                }
                            case "DR":
                                {
                                    //Two options for REPLY regarding doors
                                    //AR:REPLY:DR:ID:SUCCESS/FAILURE (Set action)
                                    //AR:REPLY:DR:ID:STATUS (Get action)

                                    builder.Append("Door #");
                                    builder.Append(inputs[(int)MsgVars.Id]);

                                    if (inputs[(int)MsgVars.Answer] == "SUCCESS" || inputs[(int)MsgVars.Answer] == "FAILURE")
                                    {

                                        builder.Append(" Set");
                                        builder.Append(inputs[(int)MsgVars.Answer]);
                                        builder.Append("\n");
                                        Logger.LogHelper.Log(Logger.Logger.LogTarget.File, builder.ToString());
                                        //handleSet(Int32.Parse(inputs[(int)MsgVars.Id]), inputs[(int)MsgVars.Answer]);
                                    }
                                    else if (inputs[(int)MsgVars.Answer] == "UP" || inputs[(int)MsgVars.Answer] == "DOWN" || inputs[(int)MsgVars.Answer] == "MIDDLE")
                                    {
                                        builder.Append(" Status: ");
                                        builder.Append(inputs[(int)MsgVars.Answer]);
                                        ///TODO ::: Return MSG to GET request
                                        Logger.LogHelper.Log(Logger.Logger.LogTarget.File, builder.ToString());
                                    }
                                    break;
                                }

                        }
                        break;
                    }
                case "GET":
                    break;

                case "NOTIFY":
                    {
                        if (inputs[(int)MsgVars.Component] != "SN")
                        {
                            throw new ArgumentException("Invalid Component notifying");
                        }

                        else
                        {
                            
                            //Enable Sensor on GUI
                            //Raise event 

                        }
                        break;
                    }
            }

        }

        public void handleSet(int id, string answer)
        {
            if (answer == "FAILURE")
            {
                //Take care of failed set action
            }
            else
            {
                //Update database
                //Updata GUI
                //Update argument OR WILL IT HAPPEN WHERE THE SET ACTION IS REQUESTED
            }

        }

        public void handleGet(string message) { }

        public void handleNotify(string message) { }

    }
}
