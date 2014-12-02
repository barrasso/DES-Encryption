using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace Lab7
{
    public partial class Form1 : Form
    {
        // strings
        public string fileNameString;
        public string outputFileName;
        public string keyString;

        // byte array
        public byte[] key;

        // default constructor
        public Form1()
        {
            InitializeComponent();
        }

        private void fileButton_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            // error check
            if (!errorWithEncryption())
            {
                // set key 
                this.setKey();

                // add .des extention
                string output = string.Concat(fileNameString, ".des");

                // check if file exists
                if (File.Exists(output))
                {
                    // prompt for overwrite
                    if (MessageBox.Show("Output file exists. Overwrite?", "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                }

                // perform encryption
                encryptFile(fileNameString, output, this.key, this.key);
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            // error check
            if (!errorWithDecryption())
            {
                // decrypt, prompt for overwrites
            }
        }

        private void openFile()
        {
            // init dialog
            OpenFileDialog openDialog = new OpenFileDialog();

            // user pressed ok
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                // set file text box as file name
                fileNameString = openDialog.FileName;
                fileTextBox.Text = fileNameString;
            }
        }

        private void encryptFile(string fileNameIn, string fileNameOut, byte[] desKey, byte[] desIV)
        {
            // must init file streams
            FileStream inStream = null;
            FileStream outStream = null;

            try
            {
                // init file streams to handle input and output
                inStream = new FileStream(fileNameIn, FileMode.Open, FileAccess.Read);
                outStream = new FileStream(fileNameOut, FileMode.OpenOrCreate, FileAccess.Write);
                outStream.SetLength((long)0);
            }

            catch
            {
                MessageBox.Show("Could not open source or destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                if (inStream != null)
                    inStream.Close();
                if (outStream != null)                
                    outStream.Close();

                return;
            }

            // init byte array
            byte[] byteArray = new byte[100];
            long var = (long)0;
            long len = inStream.Length;

            // instantiate DES provider
            DES des = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(outStream, des.CreateEncryptor(desKey, desIV), CryptoStreamMode.Write);                

            // read input file and write to output using provided key
            while (var < len)
            {
                int dummy = inStream.Read(byteArray, 0, 100);
                encStream.Write(byteArray, 0, dummy);
                var = var + (long)dummy;
            }

            // close streams
            encStream.Close();
            outStream.Close();
            inStream.Close();
            Console.WriteLine("Encryption Success. Output: {0}",fileNameOut);     
        }

        private void decryptFile(string fileNameIn, string fileNameOut, string secretKey)
        {

        }

        private void setKey()
        {
            // init key
            this.key = new byte[8];
            int dummy = 0;
            for (int i = 0; i < keyString.Length; i++)
            {
                this.key[dummy] = (byte)(this.key[dummy] + (byte)this.keyString[i]);
                dummy = (dummy + 1) % 8;
            }
        }

        /* Error detection methods */
        private bool errorWithEncryption()
        {
            // check key text box
            keyString = keyTextBox.Text.ToString();
            if (keyString.Length == 0)
            {
                MessageBox.Show("Please enter a key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            // check for valid file
            fileNameString = fileTextBox.Text.ToString();
            if (fileNameString.Length == 0)
            {
                MessageBox.Show("Could not open source or destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            else
                return false;
        }

        private bool errorWithDecryption()
        {
            // check key text box
            keyString = keyTextBox.Text.ToString();
            if (keyString.Length == 0)
            {
                MessageBox.Show("Please enter a key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            // check for valid DES file OR empty file text box


            else
                return false;
        }
    }
}
