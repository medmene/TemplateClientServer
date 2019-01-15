#define server
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

namespace TemplateClientServer
{
    class Networking
    {
        TcpClient client; 
        NetworkStream stream = null; 
        string data = "";
        Encoding enc;

        /// <summary>
        /// for server
        /// </summary>
        /// <param name="tcpClient">TCP client</param>
        public Networking(TcpClient tcpClient, Encoding _enc){
            client = tcpClient; enc = _enc;
        }
        /// <summary>
        /// for client
        /// </summary>
        /// <param name="tcpClient">TCP client</param>
        /// <param name="helloMsg">Hello message</param>
        public Networking(TcpClient tcpClient, string helloMsg, Encoding _enc){
            client = tcpClient; data = helloMsg; enc = _enc;
        }
        /// <summary>
        /// Get message
        /// </summary>
        /// <param name="stream">Network stream</param>
        /// <param name="coder">Encoding</param>
        /// <returns></returns>
        string GetMsg(NetworkStream stream, Encoding coder)
        {
            byte[] data = new byte[2048]; 
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do{
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(coder.GetString(data, 0, bytes));
                Thread.Sleep(300);
            }
            while (stream.DataAvailable);

            return builder.ToString();
        }
        /// <summary>
        /// Sending message
        /// </summary>
        /// <param name="stream">Network stream</param>
        /// <param name="msg">Message to send</param>
        void SendMsg(NetworkStream stream, string msg, Encoding coder)
        {
            byte[] data = coder.GetBytes(msg);
            stream.Write(data, 0, data.Length);
        }
        /// <summary>
        /// Main connection function
        /// </summary>
        public void Process(){
            try{
                stream = client.GetStream();                
                
                while (true)
                {
#if server
                    data = GetMsg(stream, enc);
#else
                    SendMsg(stream, data, enc); //first connection on client
                    data = GetMsg(stream, enc);
#endif
                    //processing of data
                    switch (data){
                        default: break;
                    }
                }
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
                if (stream != null) stream.Close();
                if (client != null) client.Close();
            }
            finally{
                if (stream != null) stream.Close();
                if (client != null) client.Close();
            }
        }
    }
}