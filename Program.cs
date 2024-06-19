using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread serverThread = new Thread(StartServer);
            serverThread.Start();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void StartServer()
        {
            TcpListener server = null;
            try
            {
                server = new TcpListener(IPAddress.Any, 8888);
                server.Start();

                byte[] bytes = new byte[1024];
                string data;

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    NetworkStream stream = client.GetStream();

                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine($"Received: {data}");

                        byte[] msg = Encoding.ASCII.GetBytes(data);
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine($"Sent: {data}");
                    }

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
