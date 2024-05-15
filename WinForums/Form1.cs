using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.IO;

namespace WinForums
{


    public partial class Form1 : Form
    {
        private string executableDirectory;
        private String stage2;
        private String Interface;


        bool Retry = false;

        int selectedFirmware = 0;
        public Form1()
        {
            executableDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            InitializeComponent();
            label1.Text = "Stage2 Payload";
            button1.Text = "...";
            checkBox1.Text = "Auto Retry";
            button2.Text = "Do the funny";
            label2.Text = "Network Adapters";
            comboBox1.Items.Add("750");
            comboBox1.Items.Add("751");
            comboBox1.Items.Add("755");
            comboBox1.Items.Add("800");
            comboBox1.Items.Add("801");
            comboBox1.Items.Add("803");
            comboBox1.Items.Add("900");
            comboBox1.Items.Add("903");
            comboBox1.Items.Add("904");
            comboBox1.Items.Add("950");
            comboBox1.Items.Add("951");
            comboBox1.Items.Add("960");
            comboBox1.Items.Add("1000");
            comboBox1.Items.Add("1001");
            comboBox1.Items.Add("1050");
            comboBox1.Items.Add("1070");
            comboBox1.Items.Add("1071");
            comboBox1.Items.Add("1100");
            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                comboBox2.Items.Add(networkInterface.Name);
            }

        }



        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Browse for Stage2 payload";
            dialog.InitialDirectory = @"C:\";
            dialog.Filter = "Bin files (*.bin) |*.bin";
            dialog.Multiselect = false;
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // User selected a file, save the file path to the stage2 variable
                stage2 = dialog.FileName;
                textBox1.Text = stage2;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (Retry) { Retry = false; }
            else { Retry = true; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //TODO: run PPPwn

            // Combine the executable directory with "stage1.bin"
            string stage1 = Path.Combine(executableDirectory, "stage1.bin");

            // Set the path to PPPwn.exe
            string pppwnPath = Path.Combine(executableDirectory, "PPPwn.exe");

            if (!File.Exists(pppwnPath))
            {
                MessageBox.Show("Error: PPPwn.exe not found in the current directory.");
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = pppwnPath;
            if (Retry)
            {
                startInfo.Arguments = $"--interface \"{Interface}\" --fw \"{selectedFirmware}\" --stage1 \"{stage1}\" --stage2 \"{stage2}\" --auto-retry";
            }
            else
            {
                startInfo.Arguments = $"--interface \"{Interface}\" --fw \"{selectedFirmware}\" --stage1 \"{stage1}\" --stage2 \"{stage2}\"";
            }

            startInfo.WorkingDirectory = executableDirectory;

            // Start the process
            using (Process process = Process.Start(startInfo))
            {
                // Wait for the process to exit
                process.WaitForExit();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Interface = comboBox2.SelectedItem.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            selectedFirmware = int.Parse(comboBox1.SelectedItem.ToString());

        }
    }
}
