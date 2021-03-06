﻿using ComponentAce.Compression.Libs.ZLib;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;

namespace RFIDApplication
{
    public class RFIDTagData
    {
        public string EPC_ID;
        public ulong EPC_PS_Num;
        public string EPC_data;
        public int readCount;
        public int notUpdateCount;
        public bool bUpdate;
        public string label;
        public string tagInfo;
        public string OdooTagInfo;
        public string tagTIDInfo;
        public int rssi;
        public int writeTestCount;
        public int writeReceiveCount;
        public TagStatus tagStatus;

        public enum TagStatus
        {
            IDNotUpdate,
            IDUpdated,
            AccessCodeNotUpdate,
            AccessCodeUpdated,
            TIDUpdated,
            DataNotUpdate,
            DataErased,
            DataUpdated
        }

        public RFIDTagData()
        {
            bUpdate = true;
            EPC_ID = "";
            EPC_PS_Num = 0;
            EPC_data = "";
            readCount = 0;
            notUpdateCount = 0;
            rssi = 0;
            label = "";
            tagInfo = "";
            OdooTagInfo = "";
            tagTIDInfo = "";
            writeTestCount = 0;
            writeReceiveCount = 0;
            tagStatus = TagStatus.IDNotUpdate;
            RFIDTagInfo.reserverData = "";
        }
    }
    public class TagQC
    {
        public enum TagStatus
        {
            Default,
            Pass,
            Fail
        }

        public struct TagResult
        {
            public const string DEFAULT = "";//"PASS/FAIL";
            public const string PASS = "PASS";
            public const string FAIL = "FAIL";
        }

        public struct TagIDText
        {
            public const string DEFAULT = "Waiting...";
            public const string EMPTY = "Empty";
        }

        public struct TagAccessCodeText
        {
            public const string DEFAULT = "Waiting...";
            public const string LOCKED = "Encrypted";//"Tag is locked";
            public const string NONLOCK = "Not Encrypted";//"Tag is not locked";
            public const string LOCKFAIL = "Encryption failed";//"Tag is lock failed";
            public const string LOCKSIZE = "Size is 8 bytes";
        }

        public struct TagDataText
        {
            public const string DEFAULT = "Waiting...";
            public const string EMPTY = "Tag is empty";
            public const string USED = "Tag has been authenticated";
            public const string ACTIVATED = "Tag has been activated";
            public const string DATASIZE = "Size is 64 bytes";
            public const string READFAILED = "Read data failed!";
        }
    }

    class RFIDTagInfo
    {
        public struct WriteData
        {
            public static byte[] ID = new byte[12];
            public static byte[] reserve = new byte[8]; 
            public static byte[] data = new byte[44];
        }

        public const string multipleTagsFound =
                "Multiple Tags detected. Please ensure only one tag is within reader’s detection range.";

        public const char serialSep = '=';
        public const byte DATASIZE = 32; //64 bytes
        public const byte RESERVESIZE = 4; //8 bytes
        public static string currentTagID = "";
        public static int writeTestReceiveCount = 0;
        public static int writeTestRssiTotal = 0;
        private static string labelFormat = "";        
        public static string reserverData;
        public static byte[] accessCode = null;       
        //public static List<string> labelList = new List<string>();
        public static bool bAccessCode(string verifyCode, string strData)
        {
            if (strData == null || strData == "")
                return false;

            if (strData.Trim().Contains(verifyCode.Trim()))
            {
                return true;
            }
            return false;
        }

        public static void loadLabelFormat(byte[] lableFormatBytes)
        {
            labelFormat = Encoding.ASCII.GetString(lableFormatBytes);
        }

        public static string readLabelFormat()
        {
            return labelFormat;
        }

        public static void addLog(string label, string data)
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (label == "" || data == "")
                return;

            string[] tmpList = data.Split(serialSep);
            string strHeadType = tmpList[1].Substring(0, 2);
            string strIntType = tmpList[1].Substring(2, 2);
            string strVolume = tmpList[1].Substring(4, 4);
            string strDate = tmpList[1].Substring(8, 4);
            string strSupplier = tmpList[1].Substring(12, 2);

            using (StreamWriter sw = File.AppendText(path + "\\log.dat"))
            {
                DateTime localDate = DateTime.Now; 
                sw.Write("{0},", localDate.ToString("MM/dd/yy HH:mm:ss"));
                sw.Write(label + "," + strHeadType + "," + strIntType + ",");
                sw.WriteLine(strVolume + "," + strDate + "," + strSupplier);
            }
        }

        public static bool verifyData(string inputData, string sourceFilePath)
        {
            if (inputData == "")
                return false;

            int num = 0;
            string[] checkData = inputData.Split(RFIDTagInfo.serialSep);
            if(!checkData[0].StartsWith("PS") && int.TryParse(checkData[0].Substring(2), out num))
            {
                return false;
            }
                      
            //check ink type, ink volume, and expire date supplier ID
            using (StreamReader sr = new StreamReader(sourceFilePath + @"\reference\lookUpTable.csv"))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null)
                {
                    
                    string[] dataRow = currentLine.Split(',');
                    if (dataRow[0].Trim().Contains(checkData[1].Substring(0, 3)))
                    {
                        return true;
                    }
                    else if (dataRow[1].Trim().Contains(checkData[1].Substring(0, 4)) &&
                             dataRow[2].Trim().Contains(checkData[1].Substring(4, 4)))
                    {
                        int dateNowDigit = 0;
                        int dateInDigit = 0;
                        int supplierID = 0;
                        if (int.TryParse(checkData[1].Substring(8, 4), out dateInDigit) &&
                            int.TryParse(DateTime.Now.ToString("MMyy"), out dateNowDigit) &&
                            int.TryParse(checkData[1].Substring(12, 2), out supplierID))
                        {
                            if(dateInDigit > dateNowDigit)
                            {
                                return true;
                            }                            
                        }
                    }                    
                }
            }
            return false;
        }

        public static bool checkZeroAccessCode(byte[] input)
        {//return true if read 0 access code
            try
            {
                if(input == null)                
                    return false;
                
                else if ((input[0] == 0) && (input[1] == 0) &&
                         (input[2] == 0) &&  (input[3] == 0))
                    return true;                
            }
            catch (Exception exp) { }
            return false;
        }

        public static string ASCIIToHex(string input)
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

        public static string readEPCLabel(string strEPC, out ulong uEPCNumber)
        {
            uEPCNumber = 0;
            try
            {
                string EPClabel = RFIDTagInfo.HEXToASCII(strEPC.Substring(0, 6).ToUpper());
                string serialNumber = strEPC.Substring(7).Replace(" ", "");
                uEPCNumber = ulong.Parse(serialNumber);
                string EPSnumber = uEPCNumber.ToString("D20");
                if(EPClabel != RFIDTagInfo.readLabelFormat().Substring(0,2))
                {
                    uEPCNumber = 0;
                    return "";
                }
                return EPClabel + EPSnumber;
            }
            catch (Exception exp) { return ""; }
        }
        //byte array to string 
        //Encoding.ASCII.GetString

        public static string HEXToASCII(string input)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < input.Length; i += 3)
            {
                var hexChar = input.Substring(i, 3).Trim();
                sb.Append((char)Convert.ToByte(hexChar, 16));
            }
            return sb.ToString();
        }

        public static string AddHexSpace(string input)
        {
            return String.Join(" ", Regex.Matches(input, @"\d{2}")
                        .OfType<Match>()
                        .Select(m => m.Value).ToArray());
        }

        public static Byte[] GetBytesFromBinaryString(String input)
        {
            var list = new List<Byte>();
            var binaryList = input.Split(' ');

            for (int i = 0; i < binaryList.Length; i++)
            {
                list.Add(Convert.ToByte(binaryList[i], 2));
            }
            return list.ToArray();
        } 

        public static void playSound(bool bPASS)
        {
            if(bPASS)
            {
                //SystemSounds.Exclamation.Play();
                //SystemSounds.Question.Play();
#if true
                PlayWav(Properties.Resources.juntos, false);
#else
                using (var soundPlayer = new SoundPlayer(@"c:\Windows\Media\Windows Notify System Generic.wav"))
                {
                    soundPlayer.Play(); // can also use soundPlayer.PlaySync()
                }
#endif
                /*
                 * System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.Sound);
                 * player.Play();
                */
            }
            else
            {
#if true
                PlayWav(Properties.Resources.Bad, false);
#else
                using (var soundPlayer = new SoundPlayer(@"c:\Windows\Media\Windows Error.wav"))
                //using (var soundPlayer = new SoundPlayer(@"c:\Windows\Media\ringout.wav"))
                {
                    soundPlayer.Play(); // can also use soundPlayer.PlaySync()
                }
#endif
            }
        }

        // The player making the current sound.
        private static SoundPlayer Player = null;

        // Dispose of the current player and
        // play the indicated WAV file.
        private static void PlayWav(Stream stream, bool play_looping)
        {
            // Stop the player if it is running.
            if (Player != null)
            {
                Player.Stop();
                Player.Dispose();
                Player = null;
            }

            // If we have no stream, we're done.
            if (stream == null) return;

            // Make the new player for the WAV stream.
            Player = new SoundPlayer(stream);

            // Play.
            if (play_looping)
                Player.PlayLooping();
            else
                Player.Play();
        }

    }
}
