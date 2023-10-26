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

namespace TDOffice_v2
{
    public partial class fm_Backup_Skini_Index : Form
    {
        public fm_Backup_Skini_Index()
        {
            InitializeComponent();
        }
        private Task DownloadAsync(string strURL, string FileName, string localFilePath, string userName, string psw)
        {
            return Task.Run(() =>
            {
                string url = strURL + FileName;
                try
                {
                    var credentials = new NetworkCredential(userName, psw);

                    WebRequest sizeRequest = WebRequest.Create(url);
                    sizeRequest.Credentials = credentials;
                    sizeRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                    int size = (int)sizeRequest.GetResponse().ContentLength;

                    progressBar1.Invoke(
                        (MethodInvoker)(() => progressBar1.Maximum = size));

                    WebRequest request = WebRequest.Create(url);
                    request.Credentials = credentials;
                    request.Method = WebRequestMethods.Ftp.DownloadFile;

                    using (Stream ftpStream = request.GetResponse().GetResponseStream())
                    using (Stream fileStream = File.Create(@localFilePath))
                    {
                        byte[] buffer = new byte[10240];
                        int read;
                        while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, read);
                            int position = (int)fileStream.Position;
                            progressBar1.Invoke(
                                (MethodInvoker)(() => progressBar1.Value = position));
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
        private async void btnIzlistajBaze_Click(object sender, EventArgs e)
        {
            List<string> Filenames = new List<string>();
            string stdir = tbPathDoBaza.Text;

            panel1.Enabled = false;
            await Task.Run(() =>
            {
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@stdir);
                    request.Method = WebRequestMethods.Ftp.ListDirectory;

                    request.Credentials = new NetworkCredential(TDFtp.Settings.GetUsername(), TDFtp.Settings.GetPassword());
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string names = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                    Filenames = names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                foreach (string f in Filenames)
                {
                    this.Invoke((MethodInvoker) delegate
                    {
                        checkedListBox1.Items.Add(f);
                    });
                }
            });
            panel1.Enabled = true;
        }
        private async void btnSkiniBaze_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            string str_localPath = tb_tempPath.Text + @"\";
            str_localPath = str_localPath.Replace(@"\\", @"\");
            if (checkedListBox1.CheckedItems.Count != 0)
            {
                List<string> ListaFajlovaZaDownLoad = new List<string>();
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                    ListaFajlovaZaDownLoad.Add(checkedListBox1.CheckedItems[i].ToString());

                //DownLoad sa FTP
                int brFajlova = ListaFajlovaZaDownLoad.Count();
                int fajl = 0;
                textBox1.Text = $"{fajl}/{brFajlova}";
                foreach (string LF in ListaFajlovaZaDownLoad)
                {
                    labelInfo.Text = "Kopira se << " + LF + $" >> sa ftp << {TDFtp.Settings.GetUrl()}>> na " + str_localPath;
                    fajl++;
                    textBox1.Text = $"{fajl}/{brFajlova}";

                    int copyN = 0;
                    while (File.Exists(str_localPath + LF))
                    {
                        copyN++;
                        str_localPath = Path.GetFileNameWithoutExtension(str_localPath) + $" ({copyN})" + Path.GetExtension(str_localPath);
                    }

                    await DownloadAsync(TDFtp.Settings.GetUrl(), LF, str_localPath + LF + ".tmp", TDFtp.Settings.GetUsername(), TDFtp.Settings.GetPassword());

                    File.Move(str_localPath + LF + ".tmp", str_localPath + LF);
                }
                progressBar1.Value = 0;
                labelInfo.Text = "Zavrsen download";
            }
            panel1.Enabled = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = null;

            openFileDialog1.InitialDirectory = "C:\\Users";
            openFileDialog1.ValidateNames = false;
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.FileName = "Folder Selection.";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                filePath = openFileDialog1.FileName;

            tb_tempPath.Text = Path.GetDirectoryName(filePath);
        }
    }
}
