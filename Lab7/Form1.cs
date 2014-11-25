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


namespace Lab7
{
    public partial class Form1 : Form
    {
        // strings
        public string fileNameString;
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
                // encrypt
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            // error check
            if (!errorWithDecryption())
            {
                // decrypt, prompt for overrites
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
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error: Could not read file from disk." + e.Message);
                }
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
