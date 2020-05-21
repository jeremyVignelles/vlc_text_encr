using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vlcplayerCsharp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
                private void VlcControl1_VlcLibDirectoryNeeded_1(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            // It will generate logs
            VlcControl1.VlcMediaplayerOptions = new[] { "--file-logging", "-vvv", "--logfile=Logs.log" };

            // These lines will set libvlc dir
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            VlcControl1.Stop();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            // read file path from TextBox3.Text
            var FileStream = new FileStream(path: TextBox3.Text, mode: FileMode.Open, access: FileAccess.Read);
            RijndaelManaged AES = new RijndaelManaged();
            SHA256Cng SHA256 = new SHA256Cng();

            // read key from TextBox4.Text
            AES.Key = SHA256.ComputeHash(Encoding.ASCII.GetBytes(TextBox4.Text));
            AES.Mode = CipherMode.ECB;


            // cryptostream starts here
            var cryptoStream = new CryptoStream(FileStream, AES.CreateDecryptor(), CryptoStreamMode.Read);

            // reading decrypted cryptostream and saves into video.mp4
           VlcControl1.Play(cryptoStream);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == DialogResult.OK)

            {
                TextBox3.Text = o.FileName;
            }
        }
    }


}
