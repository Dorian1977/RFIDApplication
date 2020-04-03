using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RFIDApplication
{
    class RFIDTagInfo
    {
        private static string dataFormat = "";
        private static string labelFormat = "";
        private static string tonerVolumeFile = "";
        private static decimal currentTonerVolume = 0;
        private static bool bScan = true;
        private const decimal tonerMAXVolume = 2000000000000; //max is 2L
        public const char serialSep = '=';
        public static string label;
        public static string tagInfo;
        public static List<string> labelList = new List<string>();

        public static void setToScan()
        {
            bScan = true;
        }
        
        public static void setToReadWrite()
        {
            bScan = false;
        }

        public static bool bIsScan()
        {
            return bScan;
        }

        public static void loadVolumeFile(string filePath)
        {
            tonerVolumeFile = filePath;
        }

        public static decimal readVolume()
        {//1L = 1,000,000,000,000 pL
            return currentTonerVolume;
        }

        public static void addVolumeToFile(string labelRead)
        {
            string[] tempList = labelRead.Split(serialSep); 
            int intToneVolume = Int32.Parse(tempList[1].Substring(4, 4));
            try
            {//1. check file existed
             //2. read data inside
             //3. if read data is empty, write volume read from tag
             //4. if read data not empty, add the volume read from tag, if over max, set to max
                if(!File.Exists(tonerVolumeFile))
                {
                    File.WriteAllText(tonerVolumeFile, ((ulong)intToneVolume * 1000000000).ToString());
                    return;
                }
                string readData = File.ReadAllText(tonerVolumeFile);
                if(readData == "")
                {
                    File.WriteAllText(tonerVolumeFile, ((ulong)intToneVolume * 1000000000).ToString());
                    return;
                }
                else
                {
                    currentTonerVolume = Convert.ToDecimal(File.ReadAllText(tonerVolumeFile), new CultureInfo("en-US"));
                    currentTonerVolume += (ulong)intToneVolume * 1000000000;

                    if(currentTonerVolume > tonerMAXVolume)
                        currentTonerVolume = tonerMAXVolume;

                    File.WriteAllText(tonerVolumeFile, currentTonerVolume.ToString());
                }
            }
            catch(Exception exp)
            {
                
            }
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
                sw.Write("{0},", localDate.ToString(new CultureInfo("en-US")));
                sw.Write(label + "," + strHeadType + "," + strIntType + ",");
                sw.WriteLine(strVolume + "," + strDate + "," + strSupplier);
            }
        }

        public static bool verifyData(string inputData, bool bVerifyData, bool bRead2Erase)
        {//1. input label = correct, bRead2Erase = false,
         //2. input label = 0, bRead2Erase = true;
            if (inputData == "")
                return false;

            if (bVerifyData)
            {
                string[] checkData = inputData.Split(RFIDTagInfo.serialSep);
                if(checkData[0].StartsWith("PS"))
                {
                    return true;
                }
                return false;
            }                
            else
            {
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
                            return false;
                        }
                    }
                }
            }

            if (bRead2Erase)
            {
                for (int i = 0; i < dataFormat.Length; i++)
                {
                    if (inputData[i] != '0')
                    {
                        return false;
                    }
                }
                return true;
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
            string EPClabel = RFIDTagInfo.HEXToASCII(strEPC.Substring(0, 6).ToUpper());
            string serialNumber = strEPC.Substring(7).Replace(" ", "");
            uEPCNumber = ulong.Parse(serialNumber);
            string EPSnumber = uEPCNumber.ToString("D20");
            return EPClabel + EPSnumber; ;
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
    }
}
