using Ambush.Components;
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

namespace Ambush.Client
{ /*
       This class will create new TCP clients by request and communicate with the server to proccess the user's request
       Written by : Ran Abitbul
       Version : 1.0
     */
    class TCPClient : IDisposable
    {
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
        List<CPX> CPXes;
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public TCPClient(string Message,int CPXId)
        {
            CPXes = MainWindow.getCPXList();
            CPX cpx = CPXes.ElementAt(CPXId);
            
            try
            {
                 
                //Create new TCP client and connect to designated server
                TcpClient client = new TcpClient(cpx.IP, cpx.port);

                this.Message = Message;

                Logger.LogHelper.Log(Logger.Logger.LogTarget.File, "Client " + client.ToString() +" connected");

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
            Logger.LogHelper.Log(Logger.Logger.LogTarget.File, "Transmitting.....\n" );
            var data = new byte[128];

            //gets next 128 bytes when sent to client
            try
            {
                int respLength = stream.Read(data, 0, data.Length);

            }
            catch (Exception e) { e.StackTrace.ToString(); }

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
