using System;
using System.Collections.Generic;
//using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RFIDApplication
{
    class Symmetric_Encrypted
    {
        private AesManaged aes;        
        public byte[] encryptedtext;
        private string accessCode = "";
 
        public Symmetric_Encrypted()
        {
            aes = new AesManaged();
        }

        public void loadAccessCode(string code)
        {
            accessCode = code;
        }

        public string readAccessCode()
        {
            return accessCode;
        }

        public void loadKey(byte [] keyValue)
        {
            aes.Key = keyValue;
        }

 
        public string readKey()
        {//test only
            return Convert.ToBase64String(aes.Key);
        }

       
        public byte[] Encrypt(string plainText)//, byte[] Key)
        {
            byte[] encrypted;
            byte[] iv = new byte[16];
            try
            {
                if(aes.Key == null || plainText == "")
                {
                    return null;
                }
                // Create a new AesManaged.    
                using (AesManaged aesTemp = new AesManaged())
                {
                    ICryptoTransform encryptor = aesTemp.CreateEncryptor(aes.Key, iv);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                        // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                        // to encrypt    
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            // Create StreamWriter and write data to a stream    
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                            encrypted = ms.ToArray();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Debug.WriteLine(e.Message);
                return null;
            }
            // Return encrypted data    
            return encrypted;
        }

        public string DecryptFromHEX(string inputEncrypt)
        {
            byte[] iv = new byte[16];
            string plaintext = null;
            // Create AesManaged    
            try
            {
                if (inputEncrypt.StartsWith(" "))
                    inputEncrypt = inputEncrypt.Remove(0, 1) + " ";

                if (aes.Key == null || inputEncrypt == "" || inputEncrypt.StartsWith(" 00"))//cipherText == null)
                {//can't covert 0x00 to ASCII word
                    return "";
                }
                string hex2Ascii = RFIDTagInfo.HEXToASCII(inputEncrypt);
                //byte[] cipherText = ASCIIEncoding.ASCII.GetBytes(hex2Ascii);
                byte[] cipherText = System.Convert.FromBase64String(hex2Ascii);
                using (AesManaged aesTemp = new AesManaged())
                {
                    // Create a decryptor
                    aesTemp.IV = iv;
                    ICryptoTransform decryptor = aesTemp.CreateDecryptor(aes.Key, aesTemp.IV);
                    // Create the streams used for decryption.    
                    using (MemoryStream ms = new MemoryStream(cipherText))
                    {
                        // Create crypto stream    
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            // Read crypto stream    
                            using (StreamReader reader = new StreamReader(cs))
                                plaintext = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return "";
            }
            return plaintext;
        }

    }
}
