﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuthorizationForms
{
    public class RFIDTagData
    {
        public string EPC_ID;
        public ulong EPC_PS_Num;
        public string EPC_data;
        public static string label;
        public static string reserverData;
        public static string tagInfo;
        public int readCount;
        public int notUpdateCount;
        public int rssi;
        public TagStatus tagStatus;

        public enum TagStatus
        {
            IDNotUpdate,
            IDUpdated,
            AccessCodeNotUpdate,
            AccessCodeUpdated,
            DataNotUpdate,
            DataUpdated
        }

        public RFIDTagData()
        {
            EPC_ID = "";
            EPC_PS_Num = 0;
            EPC_data = "";
            readCount = 0;
            rssi = 0;
            tagStatus = TagStatus.IDNotUpdate;
        }
    }

    class RFIDTagInfo
    {
        private static string labelFormat = "";
        public const char serialSep = '=';
        public static byte[] accessCode = null;
        public static List<string> labelList = new List<string>();

        public static bool bAccessCode(string verifyCode, string strData)
        {
            if (strData == null || strData == "")
                return false;

            if (strData.EndsWith(verifyCode.Remove(verifyCode.Length - 1, 1)))
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

        public static bool verifyData(string inputData)
        {
            if (inputData == "")
                return false;
                       
            string[] checkData = inputData.Split(RFIDTagInfo.serialSep);
            if(!checkData[0].StartsWith("PS"))
            {
                return false;
            }
            
            for (int i = 0; i < labelFormat.Length && i < checkData[0].Length; i++)
            {
                if (checkData[0][i] != labelFormat[i])
                {
                    if (labelFormat[i] == '_')
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }          
            return true;
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

        //strAuthData
        //DateTime, Serial number, dongle ID
        public static int checkAuthDate(string strAuthDate, int dayAllowed)
        {//return 0, mean found the tag
         //return -1, mean verified failed
         //return 1, mean verified successful
            DateTime parsedDate = DateTime.Now;
            try
            {
                parsedDate = DateTime.ParseExact(strAuthDate, "MMddyy", CultureInfo.InvariantCulture);
                if (DateTime.Now > parsedDate)
                    return -1;
                else
                    return 1;
            }
            catch (FormatException exp)
            {
                return 0;
            }
        }
/*
        public static string getLogData(bool bUseTagInfo, string inputTagLabel)
        {
            string strHeadType = "", strIntType = "", strVolume = "",
                   strDate = "", strSupplier = "", tagLabel = "";

            if (bUseTagInfo)
            {
                if (tagInfo.label == "" || tagInfo.tagDecryptedInfo == "")
                    return "";

                string[] tmpList = tagInfo.tagDecryptedInfo.Split(serialSep);
                strHeadType = tmpList[1].Substring(0, 2);
                strIntType = tmpList[1].Substring(2, 2);
                strVolume = tmpList[1].Substring(4, 4);
                strDate = tmpList[1].Substring(8, 4);
                strSupplier = tmpList[1].Substring(12, 2);
                tagLabel = tagInfo.label;
            }
            else
            {
                tagLabel = inputTagLabel;
            }

            string inputData = "";
            DateTime localDate = DateTime.Now;
            inputData = localDate.ToString("MM/dd/yy HH:mm:ss") + ",";
            inputData += tagLabel + "," + strHeadType + "," + strIntType + ",";
            inputData += strVolume + "," + strDate + "," + strSupplier + ",";
            inputData += readVolumeFile(out dongleID).ToString();
            return inputData;
        }*/
    }
}
