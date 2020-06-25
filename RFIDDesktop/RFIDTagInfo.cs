using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        public string reserverData;
        public string label;
        public string tagInfo;
        public int rssi;
        public bool tagIDNeedUpdate;
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
            reserverData = "";
            label = "";
            tagInfo = "";
            tagIDNeedUpdate = false;
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
            if ((input[0] == 0) &&
               (input[1] == 0) &&
               (input[2] == 0) &&
               (input[3] == 0))
                return true;
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
    }
}
