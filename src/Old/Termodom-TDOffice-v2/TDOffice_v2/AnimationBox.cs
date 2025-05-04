using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
	public partial class AnimationBox : Form
	{
		private int _currentProgress
		{
			get
			{
				int val = 0;

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							if (this.IsDisposed)
								return;
							val = progressBar1.Value;
						}
				);

				return val;
			}
			set
			{
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							progressBar1.Value = value;

							if (_currentProgress >= _totalProgress)
							{
								if (OnFinish != null)
									OnFinish();

								if (_closeOnFinish)
									this.Close();
							}
						}
				);
			}
		}
		private int _totalProgress
		{
			get { return progressBar1.Maximum; }
			set
			{
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							progressBar1.Maximum = value;
						}
				);
			}
		}
		private bool _closeOnFinish { get; set; } = false;
		public int CurrentProgress
		{
			get { return _currentProgress; }
		}
		public bool CloseOnFinish
		{
			get { return _closeOnFinish; }
			set { _closeOnFinish = value; }
		}

		public delegate void OnFinishHandler();

		public OnFinishHandler OnFinish;

		private AnimationBox(string naslov)
		{
			InitializeComponent();

			naslov_txt.Text = naslov;

			this.TopMost = true;
			this.TopLevel = true;
		}

		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		private void AnimationBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		public void UpdateProgress(int newProgress)
		{
			_currentProgress = newProgress;
		}

		public void SetTotalProgress(int totalProgress)
		{
			_totalProgress = totalProgress;
		}

		public new void Dispose()
		{
			this.Invoke(
				(MethodInvoker)
					delegate
					{
						base.Dispose();
					}
			);
		}

		public static AnimationBox Show(string naslov)
		{
			AnimationBox box = new AnimationBox(naslov);
			box.Show();
			box._totalProgress = 100;

			Task.Run(() =>
			{
				while (!box.IsDisposed)
				{
					if (box._currentProgress + 1 > box._totalProgress)
						box._currentProgress = 0;
					else
						box._currentProgress += 1;

					Thread.Sleep(50);
				}
			});
			return box;
		}

		public static AnimationBox Show(string naslov, int totalProgress)
		{
			AnimationBox box = new AnimationBox(naslov);
			box.Show();
			box._totalProgress = totalProgress;
			box._closeOnFinish = true;
			return box;
		}
	}
}
