using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;

namespace TDOffice_v2
{
    public partial class fm_Backup_Napravi_Index : Form
    {
        public fm_Backup_Napravi_Index()
        {
            InitializeComponent();
            tb_tempPath.Text = System.IO.Path.GetTempPath();
        }
        private void fm_Backup_Napravi_Index_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> Filenames = new List<string>();
            //String stdir = @"\\4monitor\Poslovanje\Baze";
            string stdir = @tbPathDoBaza.Text;// @"D:\IvanR\";

            if (System.IO.Directory.Exists(stdir))
            {
                string[] allfiles = Directory.GetFiles(stdir, "*.*", SearchOption.AllDirectories);
                foreach (string f in allfiles)
                {
                    string ext = Path.GetExtension(stdir + f).ToUpper();
                    if (ext == ".FDB")
                    {
                        string ff = f.ToUpper();
                        if (!ff.Contains("KOPIJA") && !ff.Contains("COPY"))
                            Filenames.Add(f);
                    }
                }
                MessageBox.Show(Filenames.Count.ToString());
                foreach (string f in Filenames)
                {
                    checkedListBox1.Items.Add(f);
                }
            }
            else
            {
                MessageBox.Show("Direktorijum " + stdir + " nedostupan ili ne postoji");
            }
        }
        private Task UploadAsync(string strURL, string FileName, string localFilePath)
        {
            return Task.Run(() =>
            {
                string url = strURL + FileName;
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                    request.Credentials = new NetworkCredential(TDFtp.Settings.GetUsername(), TDFtp.Settings.GetPassword());
                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    using (Stream fileStream = File.OpenRead(@localFilePath))
                    using (Stream ftpStream = request.GetRequestStream())
                    {
                        this.Invoke(
                            (MethodInvoker)delegate
                            {
                                progressBar1.Maximum = (int)fileStream.Length;
                            });

                        byte[] buffer = new byte[10240];
                        int read;
                        while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ftpStream.Write(buffer, 0, read);
                            this.Invoke(
                                (MethodInvoker)delegate
                                {
                                    progressBar1.Value = (int)fileStream.Position;
                                });
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }
        private async void btnZappocniBekap_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            string tempPath = tb_tempPath.Text + @"\".Replace(@"\\", @"\");
            if (checkedListBox1.CheckedItems.Count != 0)
            {
                List<string> ListaFajlovaZaBeckup = new List<string>();
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    if (File.Exists(checkedListBox1.CheckedItems[i].ToString()))
                    {
                        FileInfo fi = new FileInfo(checkedListBox1.CheckedItems[i].ToString());
                        string strNameFile = Path.GetFileNameWithoutExtension(fi.Name);
                        string strExtenzija = fi.Extension;
                        string strNameFileNew = $"{strNameFile}-tdbackup-{DateTime.Now.ToString("dd-MM-yyyy-HH-mm")}";

                        File.Copy(checkedListBox1.CheckedItems[i].ToString(), tempPath + strNameFileNew);

                        ListaFajlovaZaBeckup.Add(strNameFileNew);
                    }
                }

                // Kopiranje na FTP
                int brFajlova = ListaFajlovaZaBeckup.Count();
                int fajl = 0;
                textBox1.Text = $"{fajl}/{brFajlova}";
                foreach (string LF in ListaFajlovaZaBeckup)
                {
                    labelInfo.Text = "Upload-uje se << " + tempPath + LF + $" >> na ftp << {TDFtp.Settings.GetUrl()}>>";
                    fajl++;
                    textBox1.Text = $"{fajl}/{brFajlova}";

                    await UploadAsync(TDFtp.Settings.GetUrl(), LF, tempPath + LF);
                    File.Delete(tempPath + LF);
                }
                progressBar1.Value = 0;
                labelInfo.Text = "Zavrseno kopiranje";
            }
            panel1.Enabled = true;
        }
    }
}
