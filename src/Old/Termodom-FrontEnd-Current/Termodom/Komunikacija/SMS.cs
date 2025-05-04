using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Termodom.Models;

namespace Termodom
{
	public class SMS
	{
		public DateTime Datum { get; set; }
		public string Broj { get; set; }

		private static Tuple<LSSms, bool>[] _smsQueue = new Tuple<LSSms, bool>[64];

		static SMS()
		{
			Task.Run(() =>
			{
				while (true)
				{
					try
					{
						Thread.Sleep(2000);
						for (int i = _smsQueue.Length - 1; i >= 0; i--)
						{
							if (_smsQueue[i] != null)
							{
								AKC.Insert(
									$"SENDSMS|{_smsQueue[i].Item1.Mobile}|{_smsQueue[i].Item1.Message}|0",
									_smsQueue[i].Item1.SenderID
								);
								// TDShop.AKC.Add(string.Format("SENDSMS|{0}|{1}|{2}", _smsQueue[i].Item1.Mobile, _smsQueue[i].Item1.Message, _smsQueue[i].Item2 == true ? "0" : AR.WebShop.Config.Get("SMS_SEND_STATUS", null)[0].Value), -1);
								// _smsQueue[i].Item1.Log();
								_smsQueue[i] = null;
							}
						}
					}
					catch (Exception) { }
				}
			});
		}

		public static void AdministratorSendSMS(
			string text,
			int senderID,
			bool sendImmediatelly = false
		)
		{
			string[] mobile = File.ReadAllLines(
				Path.Combine(Program.WebRootPath, "Admin", "telefoni.txt")
			);
			for (int i = 0; i < mobile.Length; i++)
			{
				SendSMS(mobile[i], text, senderID, sendImmediatelly);
			}
		}

		public static Task SendSMSAsync(
			string Mobilni,
			string text,
			int SenderID,
			bool sendImmediatelly = false
		)
		{
			return Task.Run(() =>
			{
				SendSMS(Mobilni, text, SenderID, sendImmediatelly);
			});
		}

		public static void SendSMS(
			string Mobilni,
			string text,
			int SenderID,
			bool sendImmediatelly = false
		)
		{
			if (text.Length > 140 || string.IsNullOrWhiteSpace(text))
				throw new Exception("Tekst poruke je neispravan!");

			if (text.Contains('|'))
				throw new Exception("Text nesme sadrzati znak | ");

			if (string.IsNullOrWhiteSpace(Mobilni))
				throw new Exception("Mobilni nije ispravan!");

			if (Mobilni.Length != 9 && Mobilni.Length != 10)
				throw new Exception("Mobilni nije ispravan!");

			try
			{
				LSSms sms = new LSSms(Mobilni, text, SenderID);
				sms.Log();
				for (int i = 0; i < _smsQueue.Length - 1; i++)
				{
					if (_smsQueue[i] == null)
					{
						_smsQueue[i] = new Tuple<LSSms, bool>(sms, sendImmediatelly);
						return;
					}
				}
				return;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
	}
}
