using System.Windows.Forms;
using System.IO;

namespace kaolunch
{
    public partial class Form1 : Form
    {
        private GlobalHotKey globalHotKey;

        static DirectoryInfo d = new DirectoryInfo("./emotions");
        static FileInfo[] files = d.GetFiles("*.txt");
        static string[] fileNames = files
            .Select(f => Path.GetFileNameWithoutExtension(f.Name).ToLower())
            .ToArray();

        public Form1()
        {


        InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Lime;
            this.TransparencyKey = Color.Lime;

            this.Opacity = 0;
            this.CenterToScreen();
            this.Top = 200;
            this.TopMost = true;

            this.ShowInTaskbar = false;
            this.Hide();

            ShowIcon = false;
            notifyIcon1.Visible = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.AutoCompleteMode = AutoCompleteMode.Append;
            this.textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            this.listBox1.Visible = false;

            AutoCompleteStringCollection source = new AutoCompleteStringCollection();

            source.AddRange(fileNames);

            this.textBox1.AutoCompleteCustomSource = source;

            globalHotKey = new GlobalHotKey(Constants.CTRL, Keys.F12, this);
            bool registered = globalHotKey.Register();

            if (!registered)
            {
                MessageBox.Show("Hotkey failed to register");
            }
            listBox1.ScrollAlwaysVisible = true;

        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
            {
                HandleHotkey();
            }
            
            base.WndProc(ref m);
        }

        private void HandleHotkey()
        {
            if(this.Opacity == 0)
            {
                this.Opacity = 1;
                this.FormBorderStyle = FormBorderStyle.None;
                this.BringToFront();
                this.Activate();
                this.textBox1.Focus();
                this.textBox1.Select();
            } else
            {
                this.Opacity = 0;
                textBox1.Text = "";
                listBox1.Items.Clear();
                this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(listBox1.Items[listBox1.SelectedIndex].ToString());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if (fileNames.Any(textBox1.Text.Contains))
            {
                listBox1.Items.Clear();
                foreach (String line in File.ReadLines("./emotions/" + textBox1.Text + ".txt"))
                {
                    listBox1.Items.Add(line);
                }
                listBox1.Visible = true;
                listBox1.SelectedIndex = -1;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}