using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;

namespace TDOffice_v2.Forms
{
	public partial class fm_WebUredjivanjeProizvoda : Form
	{
		private static System.Drawing.Color WebInfoBoja = System.Drawing.Color.FromArgb(
			((int)(((byte)(255)))),
			((int)(((byte)(224)))),
			((int)(((byte)(192))))
		);
		private static System.Drawing.Color KomercijalnoInfoBoja = System.Drawing.Color.FromArgb(
			((int)(((byte)(192)))),
			((int)(((byte)(255)))),
			((int)(((byte)(255))))
		);

		private class WebUredjivanjeProizvodaProizvodiGetDto
		{
			public int RobaId { get; set; }
			public string WebKatBr { get; set; }
			public string WebNaziv { get; set; }
			public string WebJm { get; set; }
			public string KomercijalnoKatBr { get; set; }
			public string KomercijalnoNaziv { get; set; }
			public string KomercijalnoJm { get; set; }
		}

		public fm_WebUredjivanjeProizvoda()
		{
			InitializeComponent();
		}

		private void fm_WebUredjivanjeProizvoda_Load(object sender, EventArgs e)
		{
			LoadDataAsync();
		}

		private Task LoadDataAsync()
		{
			this.Enabled = false;
			return Task.Run(async () =>
			{
				var tableData = await TDAPI.GetAsync<List<WebUredjivanjeProizvodaProizvodiGetDto>>(
					"/web-uredjivanje-proizvoda/proizvodi"
				);
				if (tableData.Status != System.Net.HttpStatusCode.OK)
				{
					MessageBox.Show(
						$"Greska u komunikaciji sa API serverom! [{tableData.Status.ToString()}]"
					);
					this.Invoke(
						(MethodInvoker)
							delegate
							{
								this.Enabled = true;
							}
					);
					return;
				}

				var dt = new DataTable();
				dt.Columns.Add("RobaId", typeof(int));

				dt.Columns.Add("KatBr Web", typeof(string));
				dt.Columns.Add("Naziv Web", typeof(string));
				dt.Columns.Add("JM Web", typeof(string));

				dt.Columns.Add("KatBr Komercijalno", typeof(string));
				dt.Columns.Add("Naziv Komercijalno", typeof(string));
				dt.Columns.Add("JM Komercijalno", typeof(string));

				foreach (var data in tableData.Payload)
				{
					var dr = dt.NewRow();
					dr["RobaId"] = data.RobaId;
					dr["Naziv Web"] = data.WebNaziv;
					dr["KatBr Web"] = data.WebKatBr;
					dr["Jm Web"] = data.WebJm;
					dr["Naziv Komercijalno"] = data.KomercijalnoNaziv;
					dr["KatBr Komercijalno"] = data.KomercijalnoKatBr;
					dr["Jm Komercijalno"] = data.KomercijalnoJm;
					dt.Rows.Add(dr);
				}

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							dataGridView1.DataSource = dt;
							dataGridView1.AutoSizeColumnsMode =
								DataGridViewAutoSizeColumnsMode.AllCells;

							dataGridView1.Columns["RobaId"].DefaultCellStyle.BackColor = System
								.Drawing
								.Color
								.Orange;

							dataGridView1.Columns["KatBr Web"].DefaultCellStyle.BackColor =
								WebInfoBoja;
							dataGridView1.Columns["Naziv Web"].DefaultCellStyle.BackColor =
								WebInfoBoja;
							dataGridView1.Columns["JM Web"].DefaultCellStyle.BackColor =
								WebInfoBoja;

							dataGridView1.Columns["KatBr Komercijalno"].DefaultCellStyle.BackColor =
								KomercijalnoInfoBoja;
							dataGridView1.Columns["Naziv Komercijalno"].DefaultCellStyle.BackColor =
								KomercijalnoInfoBoja;
							dataGridView1.Columns["JM Komercijalno"].DefaultCellStyle.BackColor =
								KomercijalnoInfoBoja;

							this.Enabled = true;
						}
				);
			});
		}

		private void button1_Click(object sender, EventArgs e)
		{
			LoadDataAsync();
		}
	}
}
