using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TDBrain_v3
{
    public static class GSMModem
    {
        private class SMS
        {
            public string Mobile { get; set; }
            public string Text { get; set; }

            public SMS(string mobile, string text)
            {
                this.Mobile = mobile;
                this.Text = text;
            }
        }
        private static Socket? _socket { get; set; } = null;

        private static void CheckSocket()
        {
            if(_socket == null)
            {
                ConnectSocket();
            }
            else
            {
                lock (_socket)
                {
                    if (_socket.Poll(1000, SelectMode.SelectRead) && _socket.Available == 0)
                    {
                        ConnectSocket();
                    }
                }
            }
        }
        private static void ConnectSocket()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, 28256);
            _socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(remoteEndPoint);
        }

        /// <summary>
        /// Salje SMS Advanced Gateway-u. Tekst se salje na niz brojeva.
        /// Pre nego sto se zapocne slanje, dodaju se kontrolni brojevi kojima ce biti prosledjen sms.
        /// Ukoliko je prosledjeni niz null, onda se prosledjuje samo kontrolnim brojevima.
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="message"></param>
        public static void QueueSMS(List<string>? numbers, string message)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            try
            {
                if (numbers == null)
                    numbers = new List<string>();

                if (!numbers.Any(x => x.Contains("693691472")))
                    numbers.Add("+381693691472");

                if (!numbers.Any(x => x.Contains("641083932")))
                        numbers.Add("+381641083932");

                foreach (string number in numbers)
                {
                    Debug.Log($"Queue SMS: [{number}] - {message}");
                    CheckSocket();

                    byte[] msg = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new SMS(number, message)));

                    int bytesSent = _socket.Send(msg);
                    Debug.Log("SMS prosledjen Advanced Gateway-u, cekam odgovor...");
                    byte[] bytes = new byte[1024];
                    int bytesRec = _socket.Receive(bytes);
                    string get = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    Debug.Log(get);
                }
            }
            catch (Exception)
            {
                Debug.Log("Pokusao sam da se konektujem na SMS modem ali nisam uspeo.");
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
