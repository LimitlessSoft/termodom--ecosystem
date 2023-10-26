using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2
{
    public static class SMSGateway
    {
        private static Socket _socket { get; set; }
        public static bool PosaljiSms(string mobilni, string text, string set = null)
        {
            try
            {
                TDOffice.Partner partner = TDOffice.Partner.List($"MOBILNI = '{MobileNumber.Collate(mobilni)}'").FirstOrDefault();

                if(partner == null)
                    partner = TDOffice.Partner.Get(TDOffice.Partner.InsertBrziKontakt(mobilni));

                if (_socket == null)
                    InitializeGatewaySocket();

                if (string.IsNullOrWhiteSpace(text))
                    return false;
                
                byte[] bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new SMS() { Mobile = mobilni, Text = text }));

                int bytesSent = 0;
                Debug.Log("Socket Send " + DateTime.Now.ToString("[ dd.MM.yyyy HH:mm:ss ]"));
                bytesSent = _socket.Send(bytes, bytes.Length, 0);
                while (bytesSent == 0)
                {
                    Debug.Log("Error sending new message to socket! " + DateTime.Now.ToString("[ dd.MM.yyyy HH:mm:ss ]"));
                    InitializeGatewaySocket();
                    _socket.Send(bytes, bytes.Length, 0);
                }

                Debug.Log("Socket Waiting Response... " + DateTime.Now.ToString("[ dd.MM.yyyy HH:mm:ss ]"));
                byte[] responseBuffer = new byte[256];
                int responseBytes = _socket.Receive(responseBuffer);

                string response = Encoding.UTF8.GetString(responseBuffer);

                Debug.Log("Got Response: " + response + " - " + DateTime.Now.ToString("[ dd.MM.yyyy HH:mm:ss ]"));
                if (response.Contains("SMS queued succesfully"))
                {
                    Debug.Log("Success...");
                    if(string.IsNullOrWhiteSpace(set))
                    {
                        Debug.Log("Set List...");
                        List<string> sets = TDOffice.SMSIstorija.SetList();

                        Debug.Log("Generate Random Hash...");
                        set = TDOffice.SMSIstorija.GenerateRandomSetHash().Substring(0, 8);

                        while(sets.Contains(set))
                        {
                            Debug.Log("While Hash...");
                            set = TDOffice.SMSIstorija.GenerateRandomSetHash().Substring(0, 8);
                        }
                    }
                    Debug.Log("SMS Hist Insert....");
                    TDOffice.SMSIstorija.Insert(partner.ID, text, DateTime.Now, set);
                    Debug.Log("return True");
                    return true;
                }

                Debug.Log("return False");
                return false;
            }
            catch(Exception ex)
            {
                Debug.Log(ex.ToString());
                return false;
            }
        }
        public class SMS
        {
            public string Mobile { get; set; }
            public string Text { get; set; }
        }
        private static void InitializeGatewaySocket()
        {
            Debug.Log("Initializing new socket!");
            int[] sockets = new int[3] { 28256, 28270, 36472 };

            IPHostEntry hostEntry = null;

            hostEntry = Dns.GetHostEntry("localhost");

            for (int i = 0; i < sockets.Length; i++)
            {
                try
                {
                    foreach (IPAddress address in hostEntry.AddressList)
                    {
                        IPEndPoint ipe = new IPEndPoint(address, sockets[i]);
                        Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        tempSocket.Connect(ipe);

                        if (tempSocket.Connected)
                        {
                            _socket = tempSocket;
                            Debug.Log("New socket initialized!");
                            return;
                        }
                    }
                }
                catch
                {
                    Debug.Log($"Socket port {sockets[i]} failed");
                }
            }
            Debug.Log("Failed initializing socket!");
        }
    }
}
