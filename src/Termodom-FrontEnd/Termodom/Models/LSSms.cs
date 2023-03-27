using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Termodom.Models
{
    public class LSSms
    {
        private object ____LogLock = new object();
        public string Mobile { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int SenderID { get; set; }

        public LSSms()
        {

        }
        public LSSms(string Mobile, string Message, int SenderID)
        {
            this.Mobile = Mobile;
            this.Message = Message;
            this.SenderID = SenderID;
        }

        public void Log()
        {
            lock (____LogLock)
            {
                if (!Directory.Exists("Ls"))
                    Directory.CreateDirectory("Ls");

                if (!File.Exists(Path.Combine("Ls", "sentSms.ar")))
                    File.Create(Path.Combine("Ls", "sentSms.ar"));

                List<LSSms> smss = JsonConvert.DeserializeObject<List<LSSms>>(File.ReadAllText(Path.Combine("Ls", "sentSms.ar")));

                if (smss == null)
                    smss = new List<LSSms>();

                smss.Add(this);

                File.WriteAllText(Path.Combine("Ls", "sentSms.ar"), JsonConvert.SerializeObject(smss));
            }
        }

        public static Task<List<LSSms>> GetSentAsync()
        {
            Task<List<LSSms>> t = new Task<List<LSSms>>(() => {
                if (!Directory.Exists("Ls"))
                    Directory.CreateDirectory("Ls");

                if (!File.Exists(Path.Combine("Ls", "sentSms.ar")))
                    File.Create(Path.Combine("Ls", "sentSms.ar"));

                return JsonConvert.DeserializeObject<List<LSSms>>(File.ReadAllText(Path.Combine("Ls", "sentSms.ar")));
            });

            t.Start();

            Task.WaitAll(t);

            return t;
        }
        public static Task<List<LSSms>> GetByMobile(string mobile)
        {
            return Task.Run<List<LSSms>>(() =>
            {
                return GetSentAsync().Result.Where(t => t.Mobile == mobile).ToList();
            });
        }
    }
}
