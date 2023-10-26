using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class FloatBox : Form
    {
        [System.ComponentModel.Browsable(false)]
        protected override bool ShowWithoutActivation => true;
        private const int WM_MOUSEACTIVATE = 0x0021, MA_NOACTIVATE = 0x0003;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE)
            {
                m.Result = (IntPtr)MA_NOACTIVATE;
                return;
            }
            base.WndProc(ref m);
        }


        private string _bodyText { get; set; }
        public string BodyText {
            get
            {
                return _bodyText;
            }
            set
            {
                _bodyText = value;
                _thisForm.Invoke((MethodInvoker) delegate
                {
                    richTextBox1.Text = _bodyText;
                });
            }
        }


        private FloatBox _thisForm { get; set; }

        public FloatBox(string bodyText)
        {
            InitializeComponent();

            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, Screen.PrimaryScreen.Bounds.Height - this.Height - 30);
            this.ShowInTaskbar = false;

            _thisForm = this;
            _bodyText = bodyText;
        }

        private void FloatBox_Load(object sender, EventArgs e)
        {
            BodyText = _bodyText;
        }

        public void Show(TimeSpan closeTimeout)
        {
            this.Show();

            Thread t = new Thread(() =>
            {
                Thread.Sleep(closeTimeout);
                _thisForm.Invoke((MethodInvoker)delegate
                {
                    this.Close();
                    this.Dispose();
                });
            });
            t.IsBackground = true;
            t.Start();
        }
    }
}
