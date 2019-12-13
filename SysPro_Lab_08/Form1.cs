using System;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace SysPro_Lab_08
{
    public partial class MainForm : Form
    {
        private string FileText;
        private string FileName;
        private const string KeyName = @"SOFTWARE\SPZ";
        private const string ValueName = "COUNTER";
        private uint LaunchCounter = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DoRegisters();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Text files|*.txt";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                FileName = OFD.FileName;

                FileText = File.ReadAllText(FileName);
                TextBox.Text = FileText;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "Text files|*.txt";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                FileName = SFD.FileName;
                FileText = TextBox.Text;
                File.WriteAllText(FileName, FileText);
            }
        }

        private void DoRegisters()
        {
            RegistryKey HKLM_key = Registry.LocalMachine;
            RegistryKey MySubKey;

            if (HKLM_key.OpenSubKey(KeyName) == null)//if no key
                HKLM_key.CreateSubKey(KeyName, true);//create one

            MySubKey = HKLM_key.OpenSubKey(KeyName, true);//

            if (MySubKey.GetValue(ValueName) == null)//if no value
                MySubKey.SetValue(ValueName, LaunchCounter);//set 0 value
            else//if there is
                LaunchCounter = Convert.ToUInt16(MySubKey.GetValue(ValueName));//get it

            MessageBox.Show(LaunchCounter.ToString() + " times this app was launched", "Counter", MessageBoxButtons.OK);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LaunchCounter++;
            RegistryKey HKLM_key = Registry.LocalMachine;
            RegistryKey MySubKey;
            MySubKey = HKLM_key.OpenSubKey(KeyName, true);
            MySubKey.SetValue(ValueName, LaunchCounter);
        }
    }
}
