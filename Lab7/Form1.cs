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
        // key
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
                this.checkKey();

                // add .des extention
                string output = string.Concat(fileTextBox.Text, ".des");

                // check if file exists
                if (File.Exists(output))
                {
                    // prompt for overwrite
                    if (MessageBox.Show("Output file exists. Overwrite?", "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                }

                // perform encryption
                encryptFile(fileTextBox.Text, output, this.key, this.key);
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            // error check
            if (!errorWithDecryption())
            {
                // check key
                this.checkKey();

                // check if valid des extension
                if (fileTextBox.Text.EndsWith(".des"))
                {
                    // remove .des extension
                    string output = fileTextBox.Text.Substring(0, fileTextBox.Text.Length - 3);

                    // check if file exists
                    if (File.Exists(output))
                    {
                        // prompt for overwrite
                        if (MessageBox.Show("Output file exists. Overwrite?", "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                    }

                    // perform decryption
                    decryptFile(fileTextBox.Text, output, this.key, this.key);
                }
                
                // else its not a des file
                else
                {
                    MessageBox.Show("Not a .des file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
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
                this.fileTextBox.Text = openDialog.FileName;
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
            Console.WriteLine("Encrypting...");     

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

        private void decryptFile(string fileNameIn, string fileNameOut, byte[] desKey, byte[] desIV)
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

            // instantiate crypto provider
            DES des = new DESCryptoServiceProvider();

            // check key
            KeySizes[] legalKeySizes = des.LegalKeySizes;
            int blockSize = des.BlockSize;

            CryptoStream encStream = new CryptoStream(outStream, des.CreateDecryptor(desKey, desIV), CryptoStreamMode.Write);
            Console.WriteLine("Decrypting...");     

            // flag to check if decryption failed
            bool decryptFail = false;
            try
            {
                // read input file and write to output using provided key
                while (var < len)
                {
                    int dummy = inStream.Read(byteArray, 0, 16);
                    encStream.Write(byteArray, 0, dummy);
                    var = var + (long)dummy;
                }
                encStream.Close();
                Console.WriteLine("Decryption Success. Output: {0}", fileNameOut);     
            }
            catch
            {
                MessageBox.Show("Bad key or file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                decryptFail = true;
            }

            // close streams
            outStream.Close();
            inStream.Close();

            // if decryption failed, delete old file
            if (decryptFail) File.Delete(fileNameOut);
        }

        private void checkKey()
        {
            // init key
            this.key = new byte[8];
            int dummy = 0;
            for (int i = 0; i < keyTextBox.Text.Length; i++)
            {
                this.key[dummy] = (byte)(this.key[dummy] + (byte)this.keyTextBox.Text[i]);
                dummy = (dummy + 1) % 8;
            }
        }

        /* Error detection methods */
        private bool errorWithEncryption()
        {
            // check key text box
            if (this.keyTextBox.Text == "")
            {
                MessageBox.Show("Please enter a key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            // check for valid file
            if (fileTextBox.Text == "")
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
            if (this.keyTextBox.Text == "")
            {
                MessageBox.Show("Please enter a key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            // check for valid DES file OR empty file text box
            if (fileTextBox.Text == "")
            {
                MessageBox.Show("Not a .des file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            else
                return false;
        }
    }
}
