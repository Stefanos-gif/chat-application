using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatApplication
{
    class Client
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 8888);

                NetworkStream stream = client.GetStream();

                Thread readThread = new Thread(() => ReadData(stream));
                readThread.Start();

                while (true)
                {
                    string message = Console.ReadLine();
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
        }

        static void ReadData(NetworkStream stream)
        {
            byte[] bytes = new byte[1024];
            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                string data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine($"Received: {data}");
            }
        }
    }
}
