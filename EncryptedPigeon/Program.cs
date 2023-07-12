using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EncryptedPigeon
{
    internal class Program
    {
        string username = "user";
        int key = 11;

        static void Main(string[] args)
        {
            Console.WriteLine("Encrypted Pigeon Client");

            Program p = new Program();
            p.Start();

            
            //while (true)
            //{
            //    string input = Console.ReadLine();
            //    string encrypted = p.StringToUnicode(input, 21000);
            //    Console.WriteLine(encrypted);
            //    Console.WriteLine(p.StringFromUnicode(encrypted, 21000));
            //}
            
        }

        void Start()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (input == null)
                    continue;

                if (!Command(input))
                {
                    byte[] packetData = EncryptedData($"{username}: {input}", key);

                    // Port and IP data for socket client
                    string ip = "127.0.0.1";
                    int port = 904;

                    // Send packet
                    IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                    Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    client.SendTo(packetData, ep);
                }
            }
        }

        byte[] EncryptedData(string input, int key)
        {
            int shift = 0;

            List<byte> bytes = new List<byte>();
            foreach (char c in input)
            {
                int unicodeValue = Convert.ToInt32(c);
                unicodeValue += (key + shift);
                bytes.Add((byte)unicodeValue);

                shift += 3;
                if (shift > 15)
                    shift = 0;
            }

            return bytes.ToArray();
        }

        bool Command(string input)
        {
            if (input.StartsWith("/username "))
            {
                input = input.Remove(0, "/username ".Length);
                username = input;
                Console.WriteLine($"Username changed to: [{username}]");
                return true;
            }
            if (input.StartsWith("/key "))
            {
                input = input.Remove(0, "/key ".Length);
                try
                {
                    key = int.Parse(input);
                    Console.WriteLine($"key updated");
                }
                catch
                {
                    Console.WriteLine($"wrong input");
                }
                
                return true;
            }
            if (input.StartsWith("/cls"))
            {
                Console.Clear();
                Console.WriteLine($"Encrypted Pigeon Client");
                return true;
            }
            else if (input.StartsWith("/"))
            {
                Console.WriteLine($"Unrecognized command \"{input}\"");
                return true;
            }

            return false;
        }




        string StringToUnicode(string text, int unicodeShift)
        {

            string encryptedInput = "";

            foreach (char c in text)
            {
                long unicodeValue = Convert.ToInt32(c) + unicodeShift;
                char output = Convert.ToChar(unicodeValue);
                encryptedInput += output;
            }

            return encryptedInput;
        }

        string StringFromUnicode(string text, int unicodeShift)
        {

            string encryptedInput = "";

            foreach (char c in text)
            {
                long unicodeValue = Convert.ToInt32(c) - unicodeShift;
                char output = Convert.ToChar(unicodeValue);
                encryptedInput += output;
            }

            return encryptedInput;
        }



    }
}