using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace UHFDemo
{
    class Symmetric_Encrypted
    {
        private AesManaged aes;        
        public byte[] encryptedtext;
        private string keyFormat = "";
        private string tonerVolumeFile = "";
        private decimal currentTonerVolume = 0;
        private const decimal tonerMAXVolume = 2000000000000; //max is 2L
        public Symmetric_Encrypted()
        {
            aes = new AesManaged();
        }

        public void loadKey(byte [] keyValue)
        {
            aes.Key = keyValue;
        }

        public void readVolumeFile(string filePath)
        {
            tonerVolumeFile = filePath;
        }

        public decimal readVolume()
        {//1L = 1,000,000,000,000 pL
            return currentTonerVolume;
        }

        public void addVolume(string labelRead)
        {
            int beginWith = labelRead.IndexOf('A');
            int endWith = labelRead.IndexOf('C');

            currentTonerVolume = Convert.ToDecimal(File.ReadAllText(tonerVolumeFile), new CultureInfo("en-US")) ;
            currentTonerVolume += Convert.ToDecimal(labelRead.Substring(beginWith + 1, endWith - beginWith - 1), new CultureInfo("en-US")) * 1000000000;

            if (currentTonerVolume > tonerMAXVolume)
                currentTonerVolume = tonerMAXVolume;

            File.WriteAllText(tonerVolumeFile, currentTonerVolume.ToString());
        }

        public void loadLabelFormat(string input)
        {
            var lableFormatBytes = GetBytesFromBinaryString(File.ReadAllText(input));
            keyFormat = Encoding.ASCII.GetString(lableFormatBytes);
        }

        public string readLabelFormat()
        {
            return keyFormat;
        }

        public string readKey()
        {//test only
            return Convert.ToBase64String(aes.Key);
        }

        public bool verifyLabel(string inputData)
        {
            bool bVerified = true;
            string labelFormat = keyFormat;

            if (inputData == "")
                return false;

            for (int i = 0; i < labelFormat.Length; i++)
            {
                if (inputData[i] != labelFormat[i])
                {
                    if (labelFormat[i] == '_')
                    {
                        continue;
                    }
                    else
                    {
                        //WriteLog(lrtxtLog, "Verified data failed " + decryptMsg[i] + " vs " + labelFormat[i], 0);
                        bVerified = false;
                        break;
                    }
                }
            }
            return bVerified;
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
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            // Return encrypted data    
            return encrypted;
        }

        public string DecryptFromHEX(string inputEncrypt)
        {
            byte[] cipherText;
            byte[] iv = new byte[16];
            string plaintext = null;
            // Create AesManaged    
            try
            {
                if (aes.Key == null || inputEncrypt == "")//cipherText == null)
                {
                    return null;
                }                
                cipherText = System.Convert.FromBase64String(HEXToASCII(inputEncrypt));

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
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return plaintext + " Error: " + e.Message;
            }
            return plaintext;
        }

        public string ASCIIToHex(string input)
        {
            //return String.Concat(input.Select(x => ((int)x).ToString("x")));
            StringBuilder sb = new StringBuilder();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            foreach (byte b in inputBytes)
            {
                sb.Append(string.Format("{0:x2} ", b));
            }
            return sb.ToString();
        }

        public string HEXToASCII(string input)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < input.Length; i += 3)
            {
                var hexChar = input.Substring(i, 3).Trim();
                sb.Append((char)Convert.ToByte(hexChar, 16));
            }
            return sb.ToString();
        }

        public string AddHexSpace(string input)
        {
            return String.Join(" ", Regex.Matches(input, @"\d{2}")
                        .OfType<Match>()
                        .Select(m => m.Value).ToArray());
        }

        public string StringInt2Hex(string input)
        {
            return AddHexSpace(Int32.Parse(input).ToString("X20"));
        }

        public Byte[] GetBytesFromBinaryString(String input)
        {
            var list = new List<Byte>();
            var binaryList = input.Split(' ');

            for (int i = 0; i < binaryList.Length; i++)
            {
                list.Add(Convert.ToByte(binaryList[i], 2));
            }
            return list.ToArray();
        }
    }
}
