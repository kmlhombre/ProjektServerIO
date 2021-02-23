﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Klient
{
    class Program
    {
        static void Main(string[] args)
        {

            byte[] message = new byte[1024];
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000));
            NetworkStream stream = client.GetStream();
            string line;
            string converted;

            while (true)
            {
                stream.Read(message, 0, message.Length);
                converted = Encoding.ASCII.GetString(message, 0, message.Length);
                if (converted == "Q")
                {
                    break;
                }
                Console.Write(converted);

                if (stream.DataAvailable == false)
                {
                    Console.Write("\n");
                    line = Console.ReadLine();
                    message = new ASCIIEncoding().GetBytes(line);
                    client.GetStream().Write(message, 0, message.Length);
                    Array.Clear(message, 0, message.Length);
                    converted = "";
                }
            }
        }
    }
}

