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

        // default constructor
        public Form1()
        {
            InitializeComponent();
        }


        /* Button clicked methods */

        private void fileButton_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            // error check
            if (!errorWithEncryption())
            {
                // encrypt, prompt for overwrites
                encryptFile(fileNameString, outputFileName, keyString);
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


        /* Utility methods */

        private void openFile()
        {
            // init stream and dialog
            Stream myStream = null;
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = "c:\\";
            openDialog.RestoreDirectory = true;

            // user pressed ok
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // if the file stream is not empty
                    if ((myStream = openDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // set file text box as file name
                            fileNameString = openDialog.FileName;
                            fileTextBox.Text = fileNameString;
 
                            // set output file name
                            outputFileName = fileNameString + ".des";
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error: Could not read file from disk." + e.Message);
                }
            }
        }

        private static void encryptFile(string fileNameIn, string fileNameOut, string secretKey)
        {
            try
            {
                // init file streams to handle input and output
                FileStream inStream = new FileStream(fileNameIn, FileMode.Open, FileAccess.Read);
                FileStream outStream = new FileStream(fileNameOut, FileMode.OpenOrCreate, FileAccess.Write);
                outStream.SetLength(0);

                // instantiate DES provider
                DES des = new DESCryptoServiceProvider();
                des.Key = ASCIIEncoding.ASCII.GetBytes(secretKey);  // MUST FIX:  ONLY WORKS IF KEY IS 8 CHARS
                des.IV = ASCIIEncoding.ASCII.GetBytes(secretKey);   // MUST FIX:  ONLY WORKS IF KEY IS 8 CHARS

                // create instance of CryptoStream to obtain encrypting object
                ICryptoTransform desEncrypt = des.CreateEncryptor();
                CryptoStream encStream = new CryptoStream(outStream, desEncrypt, CryptoStreamMode.Write);

                // read input file and write to output using provided key
                byte[] byteInput = new byte[inStream.Length - 1]; // MUST FIX:  ONLY WORKS IF KEY IS 8 CHARS
                inStream.Read(byteInput, 0, byteInput.Length);
                encStream.Write(byteInput, 0, byteInput.Length);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
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
