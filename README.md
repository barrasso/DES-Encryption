DES Encryption Example
===================
This a useful utility to encrypt files using DES encryption. 


#Overview
Clicking on the folder button opens a dialog to select either the encrypted or unencrypted file. Encrypted files have the extension ".des." This is enforced by  the decrypt function.

An arbitrary length key is entered in the key text box. It must be at least one character and can use any characters including punctuation.

Hitting either encrypt or decrypt performs the corresponding operation. The file extension ".des" is appended to the file name when encrypting and that is the file written. When decrypting, the .des extension is removed to name the destination file.

#Passwords and Keys

Keys are 64 bits which are represented by an 8 byte array. The key is formed from the password string by taking the low order 8 bits of each Unicode character (cast to a byte) and storing it in the byte array. If the string is more than 8 characters the 9th character's 8 bit value is added to the first byte in the array, the 10th character is added to the second byte etc. Continue to wrap around and add character values until the string is depleted. The byte array is initialized to zero.

Use the key for the initial vector value as well.



#Error Detection and Reporting

The following errors must be reported by a message box and the program should recover properly:

* Missing key

* Try to decrypt a file not ending in .des

* Incorrect key when decrypting

* Failure to open a source or destination file

* Overwrite of existing file

