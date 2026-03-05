using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOMSSubmarine
{
	public partial class EmailSettingsForm : Form
	{
		private TextBox[] textBoxes;

		public EmailSettingsForm()
		{
			InitializeComponent();

			textBoxes = new TextBox[Constants.MaxEmailReceiver]
			{
				textEmail1, textEmail2, textEmail3, textEmail4, textEmail5,
				textEmail6, textEmail7, textEmail8, textEmail9, textEmail10
			};
		}

		private void Form_Load(object sender, EventArgs e)
		{
			for (int i = 0; i < Constants.MaxEmailReceiver; ++i)
			{
				textBoxes[i].Text = Emails[i];
			}

			textServer.Text = Server;
			checkBoxEncryptedConnection.Checked = EncryptedConnection;
			textUserID.Text = UserID;
			textPassword.Text = Password;
			numericUpDownPort.Value = Port;
			checkBoxUseDefault.Checked = UseDefaultPort;
			textFrom.Text = SendFrom;
			textTo.Text = SendTo;
			textTheme.Text = SendTheme;
		}

		public string[] Emails { get; set; } = new string[Constants.MaxEmailReceiver];
		public string Server { get; set; }
		public bool EncryptedConnection { get; set; }
		public string UserID { get; set; }
		public string Password { get; set; }
		public uint Port { get; set; }
		public bool UseDefaultPort { get; set; }
		public string SendFrom { get; set; }
		public string SendTo { get; set; }
		public string SendTheme { get; set; }

		private void buttonOk_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < Constants.MaxEmailReceiver; ++i)
			{
				Emails[i] = textBoxes[i].Text;
			}

			Server = textServer.Text;
			EncryptedConnection = checkBoxEncryptedConnection.Checked;
			UserID = textUserID.Text;
			Password = textPassword.Text;
			Port = (uint)numericUpDownPort.Value;
			UseDefaultPort = checkBoxUseDefault.Checked;
			SendFrom = textFrom.Text;
			SendTo = textTo.Text;
			SendTheme = textTheme.Text;
		}

		private void checkBoxUseDefault_CheckedChanged(object sender, EventArgs e)
		{
			numericUpDownPort.Enabled = checkBoxUseDefault.Checked ? false : true;
		}
	}
}
