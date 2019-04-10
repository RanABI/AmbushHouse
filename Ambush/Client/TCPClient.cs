using Ambush.Components;
using Ambush.Server;
using Ambush.Utils;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ambush.Client
{ /*
       This class will create new TCP clients by request and communicate with the server to proccess the user's request
       Written by : Ran Abitbul
       Version : 1.0
     */
    class TCPClient : IDisposable
    {
        ServerRequestHandler requestHandler;
        private object _lock = new object();
        public DateTime Date { get; }
        private string _message;
        public string Message
        {
            get
            {
                lock (_lock)
                {
                    return _message;
                }
            }
            set
            {
                lock (_lock)
                {
                    _message = value;
                }
            }
        }
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public TCPClient(string Message, CPX cpx)
        {


            try
            {

                //Create new TCP client and connect to designated server
                TcpClient client = new TcpClient(cpx.ip, CPX.port);

                this.Message = Message;

                //Logger.LogHelper.Log(Logger.Logger.LogTarget.File, "Client " + client.ToString() +" connected");
                requestHandler = new ServerRequestHandler();
                //Start client thread and process request
                Thread t = new Thread(ProcessClientRequest);
                t.Start(client);

            }
            catch (Exception ex) { ex.ToString(); }

        }
        public TCPClient(string Message, string ip, int port)
        {


            try
            {

                //Create new TCP client and connect to designated server
                TcpClient client = new TcpClient(ip, port);

                this.Message = Message;

                Logger.LogHelper.Log(Logger.Logger.LogTarget.File, "Client " + client.ToString() + " connected");
                requestHandler = new ServerRequestHandler();
                //Start client thread and process request
                Thread t = new Thread(ProcessClientRequest);
                t.Start(client);

            }
            catch (Exception ex) { ex.ToString(); }

        }


        #region Event Handlers


        #endregion Event Handlers


        private void ProcessClientRequest(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;

            //Convert from string to BytesArray
            var message = System.Text.Encoding.ASCII.GetBytes(Message);
            var stream = client.GetStream();

            //sends bytes to server
            stream.Write(message, 0, message.Length);
            //Logger.LogHelper.Log(Logger.Logger.LogTarget.File, "Transmitting.....\n" );
            var data = new byte[128];

            //gets next 128 bytes when sent to client
            try
            {
                int respLength = stream.Read(data, 0, data.Length);

            }
            catch (Exception e) { e.StackTrace.ToString(); }
            var response = System.Text.Encoding.Default.GetString(data);
            requestHandler.HandleClientRequest(response);
            //Close connection
            stream.Close();
            client.Close();
        }



        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }
    }
}
