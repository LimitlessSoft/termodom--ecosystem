using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
	public static class Extensions
	{
		/// <summary>
		/// Dodaje desni klik > opcije opsega datuma kontroli.
		/// Kontrola mora imati tacno 2 DateTimePicker-a.
		/// DateTimePickeri moraju imati razliciti TabIndex i to
		/// OD datuma ima manji, DO datuma ima veci index.
		/// </summary>
		/// <param name="container"></param>
		public static void DesniKlik_DatumRange(
			this Control container,
			EventHandler eventHandler = null
		)
		{
			List<DateTimePicker> list = container.Controls.OfType<DateTimePicker>().ToList();

			if (list.Count() != 2)
				throw new Exception(
					"Kontrola mora imati tacno 2 DateTimePickera da bi ovo funkcionisalo!"
				);

			DateTimePicker dtp1 = list[0];
			DateTimePicker dtp2 = list[1];

			if (dtp1.TabIndex == dtp2.TabIndex)
				throw new Exception(
					"DateTimePickeri moraju imati razlicite vrednosti TabIndex varijable!"
				);

			DateTimePicker odDatuma = dtp1.TabIndex < dtp2.TabIndex ? dtp1 : dtp2;
			DateTimePicker doDatuma = dtp1.TabIndex > dtp2.TabIndex ? dtp1 : dtp2;

			ContextMenuStrip menuStrip = new ContextMenuStrip();
			container.ContextMenuStrip = menuStrip;

			#region Items
			ToolStripMenuItem danasItem = new ToolStripMenuItem("Danas");
			danasItem.Click += (sender, args) =>
			{
				odDatuma.Value = DateTime.Now;
				doDatuma.Value = DateTime.Now;

				if (eventHandler != null)
					eventHandler(null, null);
			};

			ToolStripMenuItem juceItem = new ToolStripMenuItem("Juce");
			juceItem.Click += (sender, args) =>
			{
				odDatuma.Value = DateTime.Now.AddDays(-1);
				doDatuma.Value = DateTime.Now;

				if (eventHandler != null)
					eventHandler(null, null);
			};

			ToolStripMenuItem poslednjihSedamDanaItem = new ToolStripMenuItem("Poslednjih 7 dana");
			poslednjihSedamDanaItem.Click += (sender, args) =>
			{
				odDatuma.Value = DateTime.Now.AddDays(-7);
				doDatuma.Value = DateTime.Now;

				if (eventHandler != null)
					eventHandler(null, null);
			};

			ToolStripMenuItem poslednjihDvadesetOsamDanaItem = new ToolStripMenuItem(
				"Poslednjih 28 dana"
			);
			poslednjihDvadesetOsamDanaItem.Click += (sender, args) =>
			{
				odDatuma.Value = DateTime.Now.AddDays(-28);
				doDatuma.Value = DateTime.Now;

				if (eventHandler != null)
					eventHandler(null, null);
			};

			ToolStripMenuItem tekucaNedeljaItem = new ToolStripMenuItem("Tekuca nedelja");
			tekucaNedeljaItem.Click += (sender, args) =>
			{
				DayOfWeek dow = DateTime.Now.DayOfWeek;

				odDatuma.Value = DateTime.Now.AddDays(-((int)dow - 1));
				doDatuma.Value = DateTime.Now;

				if (eventHandler != null)
					eventHandler(null, null);
			};

			ToolStripMenuItem tekuciMesecItem = new ToolStripMenuItem("Tekuci mesec");
			tekuciMesecItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
				doDatuma.Value = DateTime.Now;

				if (eventHandler != null)
					eventHandler(null, null);
			};

			ToolStripMenuItem tekucaGodinaItem = new ToolStripMenuItem("Tekuca godina");
			tekucaGodinaItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 1, 1);
				doDatuma.Value = DateTime.Now;

				if (eventHandler != null)
					eventHandler(null, null);
			};

			ToolStripMenuItem mesecPodmeni = new ToolStripMenuItem("Mesec");
			ToolStripMenuItem mesecJanuarItem = new ToolStripMenuItem("Januar");
			ToolStripMenuItem mesecFebruarItem = new ToolStripMenuItem("Februar");
			ToolStripMenuItem mesecMartItem = new ToolStripMenuItem("Mart");
			ToolStripMenuItem mesecAprilItem = new ToolStripMenuItem("April");
			ToolStripMenuItem mesecMajItem = new ToolStripMenuItem("Maj");
			ToolStripMenuItem mesecJunItem = new ToolStripMenuItem("Jun");
			ToolStripMenuItem mesecJulItem = new ToolStripMenuItem("Jul");
			ToolStripMenuItem mesecAvgustItem = new ToolStripMenuItem("Avgust");
			ToolStripMenuItem mesecSeptembarItem = new ToolStripMenuItem("Septembar");
			ToolStripMenuItem mesecOktobarItem = new ToolStripMenuItem("Oktobar");
			ToolStripMenuItem mesecNovembarItem = new ToolStripMenuItem("Novembar");
			ToolStripMenuItem mesecDecembarItem = new ToolStripMenuItem("Decembar");

			mesecJanuarItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 1, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					1,
					DateTime.DaysInMonth(DateTime.Now.Year, 1)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecFebruarItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 2, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					2,
					DateTime.DaysInMonth(DateTime.Now.Year, 2)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecMartItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 3, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					3,
					DateTime.DaysInMonth(DateTime.Now.Year, 3)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecAprilItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 4, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					4,
					DateTime.DaysInMonth(DateTime.Now.Year, 4)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecMajItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 5, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					5,
					DateTime.DaysInMonth(DateTime.Now.Year, 5)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecJunItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 6, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					6,
					DateTime.DaysInMonth(DateTime.Now.Year, 6)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecJulItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 7, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					7,
					DateTime.DaysInMonth(DateTime.Now.Year, 7)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecAvgustItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 8, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					8,
					DateTime.DaysInMonth(DateTime.Now.Year, 8)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecSeptembarItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 9, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					9,
					DateTime.DaysInMonth(DateTime.Now.Year, 9)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecOktobarItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 10, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					10,
					DateTime.DaysInMonth(DateTime.Now.Year, 10)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecNovembarItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 11, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					11,
					DateTime.DaysInMonth(DateTime.Now.Year, 11)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecDecembarItem.Click += (sender, args) =>
			{
				odDatuma.Value = new DateTime(DateTime.Now.Year, 12, 1);
				doDatuma.Value = new DateTime(
					DateTime.Now.Year,
					12,
					DateTime.DaysInMonth(DateTime.Now.Year, 12)
				);

				if (eventHandler != null)
					eventHandler(null, null);
			};

			mesecPodmeni.DropDownItems.Add(mesecJanuarItem);
			mesecPodmeni.DropDownItems.Add(mesecFebruarItem);
			mesecPodmeni.DropDownItems.Add(mesecMartItem);
			mesecPodmeni.DropDownItems.Add(mesecAprilItem);
			mesecPodmeni.DropDownItems.Add(mesecMajItem);
			mesecPodmeni.DropDownItems.Add(mesecJunItem);
			mesecPodmeni.DropDownItems.Add(mesecJulItem);
			mesecPodmeni.DropDownItems.Add(mesecAvgustItem);
			mesecPodmeni.DropDownItems.Add(mesecSeptembarItem);
			mesecPodmeni.DropDownItems.Add(mesecOktobarItem);
			mesecPodmeni.DropDownItems.Add(mesecNovembarItem);
			mesecPodmeni.DropDownItems.Add(mesecDecembarItem);
			#endregion

			menuStrip.SuspendLayout();
			menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuStrip.Items.AddRange(
				new System.Windows.Forms.ToolStripItem[]
				{
					danasItem,
					juceItem,
					poslednjihSedamDanaItem,
					poslednjihDvadesetOsamDanaItem,
					tekucaNedeljaItem,
					tekuciMesecItem,
					tekucaGodinaItem,
					mesecPodmeni
				}
			);
			menuStrip.Name = "desniKlikDatumRangeMenuStrip";
			menuStrip.Size = new System.Drawing.Size(164, 25 * menuStrip.Items.Count);
			menuStrip.ResumeLayout();
		}

		public static void DesniKlik_DatumRangeDGV(
			this Control container,
			EventHandler eventHandler = null
		)
		{
			DataGridView dgv = container.Controls.OfType<DataGridView>().First();

			ContextMenuStrip menuStrip = new ContextMenuStrip();
			container.ContextMenuStrip = menuStrip;

			ToolStripMenuItem danasItem = new ToolStripMenuItem("Danas");
			danasItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					DateTime dtNow = DateTime.Now;
					r.Cells["OdDatuma"].Value = new DateTime(
						dtselect.Year,
						dtNow.Month,
						DateTime.Now.Day
					);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			ToolStripMenuItem juceItem = new ToolStripMenuItem("Juce");
			juceItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					DateTime dtNow = DateTime.Now.AddDays(-1);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, dtNow.Month, dtNow.Day);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			ToolStripMenuItem poslednjihSedamDanaItem = new ToolStripMenuItem("Poslednjih 7 dana");
			poslednjihSedamDanaItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					DateTime dtNow = DateTime.Now.AddDays(-7);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, dtNow.Month, dtNow.Day);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};

			ToolStripMenuItem poslednjihDvadesetOsamDanaItem = new ToolStripMenuItem(
				"Poslednjih 28 dana"
			);
			poslednjihDvadesetOsamDanaItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					DateTime dtNow = DateTime.Now.AddDays(-28);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, dtNow.Month, dtNow.Day);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};

			ToolStripMenuItem tekucaNedeljaItem = new ToolStripMenuItem("Tekuca nedelja");
			tekucaNedeljaItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					DayOfWeek dow = dtselect.DayOfWeek;
					DateTime dtNow = DateTime.Now.AddDays(-((int)dow - 1));
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, dtNow.Month, dtNow.Day);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};

			ToolStripMenuItem mesecPodmeni = new ToolStripMenuItem("Mesec");
			ToolStripMenuItem mesecJanuarItem = new ToolStripMenuItem("Januar");
			ToolStripMenuItem mesecFebruarItem = new ToolStripMenuItem("Februar");
			ToolStripMenuItem mesecMartItem = new ToolStripMenuItem("Mart");
			ToolStripMenuItem mesecAprilItem = new ToolStripMenuItem("April");
			ToolStripMenuItem mesecMajItem = new ToolStripMenuItem("Maj");
			ToolStripMenuItem mesecJunItem = new ToolStripMenuItem("Jun");
			ToolStripMenuItem mesecJulItem = new ToolStripMenuItem("Jul");
			ToolStripMenuItem mesecAvgustItem = new ToolStripMenuItem("Avgust");
			ToolStripMenuItem mesecSeptembarItem = new ToolStripMenuItem("Septembar");
			ToolStripMenuItem mesecOktobarItem = new ToolStripMenuItem("Oktobar");
			ToolStripMenuItem mesecNovembarItem = new ToolStripMenuItem("Novembar");
			ToolStripMenuItem mesecDecembarItem = new ToolStripMenuItem("Decembar");

			mesecJanuarItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 1, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecFebruarItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 2, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecMartItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 3, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecAprilItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 4, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecMajItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 5, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecJunItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 6, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecJulItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 7, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecAvgustItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 8, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecSeptembarItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 9, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecOktobarItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 10, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecNovembarItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 11, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecDecembarItem.Click += (sender, args) =>
			{
				foreach (DataGridViewRow r in dgv.SelectedRows)
				{
					DateTime dtselect = Convert.ToDateTime(r.Cells["OdDatuma"].Value);
					r.Cells["OdDatuma"].Value = new DateTime(dtselect.Year, 12, 1);
				}

				if (eventHandler != null)
					eventHandler(null, null);
			};
			mesecPodmeni.DropDownItems.Add(mesecJanuarItem);
			mesecPodmeni.DropDownItems.Add(mesecFebruarItem);
			mesecPodmeni.DropDownItems.Add(mesecMartItem);
			mesecPodmeni.DropDownItems.Add(mesecAprilItem);
			mesecPodmeni.DropDownItems.Add(mesecMajItem);
			mesecPodmeni.DropDownItems.Add(mesecJunItem);
			mesecPodmeni.DropDownItems.Add(mesecJulItem);
			mesecPodmeni.DropDownItems.Add(mesecAvgustItem);
			mesecPodmeni.DropDownItems.Add(mesecSeptembarItem);
			mesecPodmeni.DropDownItems.Add(mesecOktobarItem);
			mesecPodmeni.DropDownItems.Add(mesecNovembarItem);
			mesecPodmeni.DropDownItems.Add(mesecDecembarItem);

			menuStrip.SuspendLayout();
			menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuStrip.Items.AddRange(
				new System.Windows.Forms.ToolStripItem[]
				{
					danasItem,
					juceItem,
					poslednjihSedamDanaItem,
					poslednjihDvadesetOsamDanaItem,
					tekucaNedeljaItem,
					mesecPodmeni
				}
			);
			menuStrip.Name = "desniKlikDatumRangeMenuStripDGV";
			menuStrip.Size = new System.Drawing.Size(164, 25 * menuStrip.Items.Count);
			menuStrip.ResumeLayout();
		}

		public static Task<fm_Help> InitializeHelpModulAsync(this Form sender, int helpModulID)
		{
			return Task.Run(() =>
			{
				fm_Help helpForm = new fm_Help(TDOffice.Modul.GetWithInsert(helpModulID));
				sender.KeyPreview = true;
				sender.KeyDown += (s, args) =>
				{
					if (args.Control && args.KeyCode == Keys.H)
					{
						helpForm.ShowDialog();
					}
				};

				sender.Disposed += (s, a) =>
				{
					helpForm.Close();
					helpForm.Dispose();
				};

				return helpForm;
			});
		}

		public static Task<fm_Help> InitializeHelpModulAsync(this Form sender, Modul modul)
		{
			return InitializeHelpModulAsync(sender, (int)modul);
		}

		public static bool IsNumber(this Keys key)
		{
			Keys[] allowed =
			{
				Keys.D1,
				Keys.D2,
				Keys.D3,
				Keys.D4,
				Keys.D5,
				Keys.D6,
				Keys.D7,
				Keys.D8,
				Keys.D9,
				Keys.D0,
				Keys.NumPad0,
				Keys.NumPad1,
				Keys.NumPad2,
				Keys.NumPad3,
				Keys.NumPad4,
				Keys.NumPad5,
				Keys.NumPad6,
				Keys.NumPad7,
				Keys.NumPad8,
				Keys.NumPad9,
			};

			if (allowed.Contains(key))
				return true;

			return false;
		}

		public static string DivideOnCapital(
			this string sender,
			bool firstLetterIsCapital = true,
			bool separatedLetterCapital = false
		)
		{
			sender = sender.Trim();

			string finish = "";
			for (int i = 0; i < sender.Length; i++)
			{
				if (i == 0)
				{
					finish += char.ToUpper(sender[i]);
					continue;
				}

				if (char.IsUpper(sender[i]))
				{
					finish += ' ';
					finish += separatedLetterCapital
						? char.ToUpper(sender[i])
						: char.ToLower(sender[i]);
					continue;
				}

				finish += sender[i];
			}

			return finish;
		}

		public static string[] DivideInPieces(this string sender, int charCount)
		{
			if (charCount <= 0)
				throw new Exception("Invalid charCount. Must be larger than 0!");

			List<string> list = new List<string>();

			string temp = "";
			foreach (char c in sender)
			{
				temp += c;
				if (temp.Length >= charCount)
				{
					list.Add(temp);
					temp = "";
				}
			}

			if (!string.IsNullOrWhiteSpace(temp))
				list.Add(temp);

			return list.ToArray();
		}

		public static string ToString(this DayOfWeek sender, bool dummy = false)
		{
			switch ((int)sender)
			{
				case 0:
					return "Nedelja";
				case 1:
					return "Ponedeljak";
				case 2:
					return "Utorak";
				case 3:
					return "Sreda";
				case 4:
					return "Cetvrtak";
				case 5:
					return "Petak";
				case 6:
					return "Subota";
				default:
					return "unknown";
			}
		}

		public static string ToStringOrDefault(this object sender)
		{
			if (sender == null || sender is DBNull)
				return null;

			return sender.ToString();
		}

		public static string ToSystemShortDateFormatString(this DateTime sender)
		{
			return sender.ToString(
				System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern
			);
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector
		)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

		public static IEnumerable<Control> GetAllControlsAndSubControlls(this Control control)
		{
			var controls = control.Controls.Cast<Control>();

			return controls
				.SelectMany(ctrl => GetAllControlsAndSubControlls(ctrl))
				.Concat(controls);
		}

		public static IEnumerable<ToolStripItem> GetAllItems(this MenuStrip menuStrip)
		{
			return menuStrip.Items.Cast<ToolStripItem>();
		}

		public static DataTable RemoveDuplicateRows(DataTable dTable, string colName)
		{
			Hashtable hTable = new Hashtable();
			ArrayList duplicateList = new ArrayList();

			//Add list of all the unique item value to hashtable, which stores combination of key, value pair.
			//And add duplicate item value in arraylist.
			foreach (DataRow drow in dTable.Rows)
			{
				if (hTable.Contains(drow[colName]))
					duplicateList.Add(drow);
				else
					hTable.Add(drow[colName], string.Empty);
			}

			//Removing a list of duplicate items from datatable.
			foreach (DataRow dRow in duplicateList)
				dTable.Rows.Remove(dRow);

			//Datatable which contains unique records will be return as output.
			return dTable;
		}

		public static bool NotOk(this HttpResponseMessage sender)
		{
			return Convert.ToInt16(sender.StatusCode).ToString()[0] != '2';
		}
	}
}
