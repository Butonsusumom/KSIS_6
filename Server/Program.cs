using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {

            TcpListener tcpListener, timerListener;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(localAddr, 3333);
            tcpListener.Start();
            timerListener = new TcpListener(localAddr, 3334);
            timerListener.Start();

            Console.WriteLine("Waiting for connections... ");
            TcpClient client = tcpListener.AcceptTcpClient();
            TcpClient timerClient = timerListener.AcceptTcpClient();

            Console.WriteLine("Client connected. ");
            NetworkStream netStream, timerStream;
            TimerCallback timerCallback = new TimerCallback(SendTime);
            Timer timer = new Timer(timerCallback, null, 0, 1000);
            while (true)
            {
                Console.WriteLine("Waiting for requests...");
                netStream = client.GetStream();
                byte[] data = new byte[256];
                //int bytes = netStream.Read(data, 0, data.Length);
                BinaryFormatter formatter = new BinaryFormatter();
                Request request = (Request)formatter.Deserialize(netStream);
                if (request is RequestFile)
                {
                    RequestFile req = (RequestFile)request;
                    byte[] response = Tasks.ReadBlock(req.FileName, req.Offset, req.Length, req.Text);
                    netStream.Write(response, 0, response.Length);
                    Console.WriteLine("Sended {0} bytes from {1}", response.Length, req.FileName);
                }
                else if(request is RequestDisconnect)
                {
                    timer.Dispose();
                    netStream.Close();
                    client.Close();
                    break;
                }
            }
            Console.WriteLine("\nPress <Enter> to terminate the server.");
            Console.ReadLine();

            void SendTime(object obj)
            {
                timerStream = timerClient.GetStream();
                byte[] data = Tasks.GetTime();
                timerStream.WriteAsync(data, 0, data.Length);
            }
        }
    }
}
