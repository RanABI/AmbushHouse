using Ambush.Client;
using Ambush.Components;
using Ambush.UserControls;
using Ambush.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using static Ambush.Enums;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace Ambush.Timers
{
    /*
     * Summary :
     * Create and initialize new Timer that will check every X minutes that all devices that should function are connected to LAN 
     * */
    public class BroadcastMessageTimer
    {
        public static System.Timers.Timer Timer = null;
        public static bool flag = false;

        public BroadcastMessageTimer(double eventTimerInterval)

        {
            try
            {
                Timer = new System.Timers.Timer(eventTimerInterval);
                Timer.Enabled = true;
                Timer.Elapsed += new System.Timers.ElapsedEventHandler(BroadcastMessage);
                Timer.AutoReset = true;
                BroadcastMessageTimer.Timer.Start();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }

        static void BroadcastMessage(object Sender, System.Timers.ElapsedEventArgs e)
        {
            bool[] cpxStates = Play.cpxStates;
            for (int i = 0; i < cpxStates.Length; i++)
            {
                cpxStates[i] = false;
            }
            //Play.setCpxStates(cpxStates);

            try
            {
                List<string> ips = new List<string>();
                string query = "SELECT * FROM MiniController";
                DataTable dataTable = Db_Utils.GetDataTable(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    //TODO ------> UPDATE MINICONTROLLER TABLE 
                    ips.Add(row.ItemArray[0].ToString());

                }
                int numOfMiniControllers = ips.Count;
                Console.WriteLine("Sending TCP messages to all minicontrollers");
                /* ------>UNFREEZE<------ */
                for (int i = 0; i < numOfMiniControllers; i++)
                {
                    //using (TCPClient TcpClient = new TCPClient(Constants.Broadcast_Message, ips[i], Constants.TCP_Broadcast_Port)) { }
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            try
            {
                Console.WriteLine("Sending UDP broadcast message to all RPI's");
                //Create client and endpoint

                UdpClient client = new UdpClient();
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Constants.Broadcast_IP), Constants.UDP_Broadcast_Port);

                //Encode message and broadcast it to network
                byte[] msg = Encoding.ASCII.GetBytes(Constants.Broadcast_Message);
                client.Send(msg, msg.Length, ip);

                //Get response from devices
                byte[] response = client.Receive(ref ip);
                string rsp = System.Text.Encoding.ASCII.GetString(response, 0, response.Length);

                //Close connection
                client.Close();

                //Update which devices are online
                updateConnectedClients(rsp);
            }
            catch (Exception ex) { Console.WriteLine("Exception @Send() BroadcastMessage " + ex.ToString()); }


            return;
        }

        public static void updateConnectedClients(string rsp)
        {
            string[] response = rsp.Split(':');
            CPX cpx = Play.getCpxByRpiId(response[2]);
            cpx.isOn = true;
        }


    }
}
