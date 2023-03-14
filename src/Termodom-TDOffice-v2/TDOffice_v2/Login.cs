using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class Login : Form
    {
        private static List<TDOffice.User> users;
        public Login()
        {
            InitializeComponent();
            users = TDOffice.User.List();
        }
        
        private void Login_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(LocalSettings.Settings.lastUsername))
                username_txt.Text = LocalSettings.Settings.lastUsername;

            version_lbl.Text = "v" + (new AssemblyName(Assembly.GetExecutingAssembly().FullName)).Version;
        }

        private void Login_btn_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(username_txt.Text))
            {
                MessageBox.Show("Korisnicko ime nije ispravno!");
                return;
            }

            if (string.IsNullOrWhiteSpace(password_txt.Text))
            {
                MessageBox.Show("Sifra nije ispravna!");
                return;
            }

            TDOffice.User user = users.Where(x => x.Username == username_txt.Text).FirstOrDefault();

            if(user == null)
            {
                MessageBox.Show("Korisnik ne postoji!");
                return;
            }

            if(password_txt.Text == user.Password)
            {
                LocalSettings.Settings.lastUsername = username_txt.Text;
                LocalSettings.Update();

                Main m = new Main(user);
                this.Hide();
                m.ShowDialog();
                Application.Exit();
                return;
            }
            else
            {
                MessageBox.Show("Pogresna lozinka!");
                return;
            }
        }

        private void Username_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                login_btn.PerformClick();
        }

        private void Login_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(LocalSettings.Settings.lastUsername))
                password_txt.Focus();
        }
    }
}
