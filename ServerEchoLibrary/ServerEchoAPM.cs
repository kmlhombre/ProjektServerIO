using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ServerEchoLibrary
{
    public class ServerEchoAPM : ServerEcho
    {
        public delegate void TransmissionDataDelegate(NetworkStream stream);

        public ServerEchoAPM(IPAddress IP, int port) : base(IP, port)
        {

        }

        protected override void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = TcpListener.AcceptTcpClient();
                Stream = tcpClient.GetStream();
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);

                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, tcpClient);
            }
        }


        private void TransmissionCallback(IAsyncResult ar)
        {
            TcpClient tcpClient = (TcpClient)ar.AsyncState;
            tcpClient.Close();
        }
        protected override void BeginDataTransmission(NetworkStream stream)
        {
            byte[] buffer = new byte[Buffer_size];
            byte[] responseBuffer = new byte[Buffer_size];
            string secretKey = "etna";
            string word;

            while (true)
            {
                try
                {
                    string menu = "\n1. Szyfr cezara\n2. Szyfr przekatnokolumnowy z slowem klucz\n9. Wyjscie";
                    responseBuffer = Encoding.ASCII.GetBytes(menu);
                    stream.Write(responseBuffer, 0, responseBuffer.Length);

                    stream.Read(buffer, 0, Buffer_size);
                    string received = Encoding.ASCII.GetString(buffer);

                    Array.Clear(responseBuffer, 0, menu.Length);

                    switch (received[0])
                    {
                        case '1':
                            menu = "Wybrano szyfr cezara. Podaj slowo do zaszyfrowania: ";
                            responseBuffer = Encoding.ASCII.GetBytes(menu);
                            stream.Write(responseBuffer, 0, responseBuffer.Length);

                            stream.Read(buffer, 0, Buffer_size);
                            received = Encoding.ASCII.GetString(buffer);
                            word = CaesarCipher(getMessage(received));

                            responseBuffer = Encoding.ASCII.GetBytes(word);
                            stream.Write(responseBuffer, 0, word.Length);
                            break;
                        case '2':
                            menu = "Wybrano szyfr przekatnokolumnowy. Podaj slowo do zaszyfrowania: ";
                            responseBuffer = Encoding.ASCII.GetBytes(menu);
                            stream.Write(responseBuffer, 0, responseBuffer.Length);

                            stream.Read(buffer, 0, Buffer_size);
                            received = Encoding.ASCII.GetString(buffer);
                            word = DiagonalColumnTranspositionCipherWithKey(getMessage(received), secretKey);

                            responseBuffer = Encoding.ASCII.GetBytes(word);
                            stream.Write(responseBuffer, 0, word.Length);
                            break;
                        default:
                            menu = "Q";
                            responseBuffer = Encoding.ASCII.GetBytes(menu);
                            stream.Write(responseBuffer, 0, responseBuffer.Length);
                            break;
                    }
                    
                    Array.Clear(buffer, 0, buffer.Length);
                    Array.Clear(responseBuffer, 0, responseBuffer.Length);
                }
                catch (IOException e)
                {
                    break;
                }
            }
        }

        string getMessage(string received)
        {
            Regex rx = new Regex(@"\b(?<word>\w+)\b");
            MatchCollection matches = rx.Matches(received);
            GroupCollection groupCollection = matches[0].Groups;
            return groupCollection["word"].Value;
        }

        public override void Start()
        {
            Console.WriteLine("Start serwera");
            StartListening();
            //transmission starts within the accept function
            AcceptClient();
        }

        protected static string CaesarCipher(string word)
        {
            var rand = new Random();
            // losowanie wartosci przesuniecia
            int liczba = rand.Next(1, 21);
            StringBuilder stringBuilder = new StringBuilder(word);

            for(int i=0; i<word.Length; i++)
            {
                stringBuilder[i] += (char) liczba;
            }

            return stringBuilder.ToString();
        }

        protected static string DiagonalColumnTranspositionCipherWithKey(string word, string secretKey)
        {
            string cipher = "";
            int rows = (int)Math.Ceiling(word.Length * 1.0 / secretKey.Length);
            int secretKeyLength = secretKey.Length;
            char[,] array = new char[rows, secretKeyLength];
            int counter = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < secretKeyLength; j++)
                {
                    if (counter < word.Length)
                        array[i, j] = word[counter];
                    else
                        array[i, j] = 'x';
                    counter++;
                }
            }
            int[] indexes = getSortedIndex(secretKey);
            int row, temp;

            for (int i = 0; i < indexes.Length; i++)
            {
                row = 0;
                temp = indexes[i];
                for (int j = 0; j < rows; j++)
                {
                    cipher += array[row, temp];
                    row++;
                    temp--;
                    if (temp < 0)
                        temp = secretKey.Length - 1;
                }

            }

            return cipher;
        }

        private static int[] getSortedIndex(string input)
        {
            int[] indexes = new int[input.Length];
            char[] characters = input.ToArray();
            Array.Sort(characters);
            string sorted = new string(characters);
            int index = 0;

            for (int i = 0; i < sorted.Length; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    if (sorted[i] == input[j])
                    {
                        index = j;
                        if (!checkIndex(index, indexes))
                            break;
                    }
                }
                indexes[i] = index;
            }

            return indexes;
        }

        private static bool checkIndex(int index, int[] indexes)
        {
            for (int i = 0; i < indexes.Length; i++)
            {
                if (index == indexes[i])
                    return true;
            }
            return false;
        }
    }
}
