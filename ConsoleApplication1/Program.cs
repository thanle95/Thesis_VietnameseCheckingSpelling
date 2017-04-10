using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            //int amtiet1 = 3500;
            //int amtiet2 = 6789;
            //Console.WriteLine(toInt32(amtiet1, amtiet2));
            ////Console.WriteLine(Int32.Parse(toInt16(amtiet1)));
            //Console.WriteLine(getFirstSyllableIndex(toInt32(amtiet1, amtiet2)));
            //Console.WriteLine(getSecondSyllableIndex(toInt32(amtiet1, amtiet2)));


            //string htmlDocument = File.ReadAllText(@"E:\Google Drive\Document\luan van\normalizedDocs\0-1000.txt");
            //StringWriter myWriter = new StringWriter();
            //WebUtility.HtmlDecode(htmlDocument, myWriter);
            //File.WriteAllText(@"E:\Google Drive\Document\luan van\1.txt", myWriter.ToString());

            string folderPath = @"C:\Users\Kiet\OneDrive\Thesis\normalizedDocs\";
            int count = 151;// da chay toi 349
            Stopwatch stopWatch = new Stopwatch();
            
            string[] getFile = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
            StringBuilder output = new StringBuilder();
            bool flag = false;
            foreach (string file in getFile)
            {
                stopWatch.Start();
                List<string> lstPhrase = getPhrase(purifyText(File.ReadAllText(file)));
                Console.WriteLine(lstPhrase.Count);
                foreach (string phrase in lstPhrase)
                {
                    if (phrase.Trim().Length > 0)
                    {
                        string[] wordArr = phrase.Split(' ');
                        foreach (string word in wordArr)
                            if (VNDictionary.getInstance.isSyllableVN(word.Trim()))
                            {
                                output.Append(word.Trim() + " ");
                                flag = true;
                            }
                        if (flag)
                        {
                            output.Append("\n");
                            flag = false;

                        }
                    }
                }
                File.WriteAllText(string.Format(@"C:\Users\Kiet\OneDrive\Thesis\NewCorpus\{0}.txt", ++count), output.ToString());
                System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\NewFiltered\" + file.Substring(folderPath.Length, file.Length - folderPath.Length - ".txt".Length) + ".txt");
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapseTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                    ts.Hours, ts.Minutes, ts.Seconds,
                                    ts.Milliseconds / 10);
                Console.WriteLine(count + "------ " + elapseTime);
                output.Remove(0, output.Length);
            }

            //foreach (string file in getFile)
            //{
            //    //System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" + ++count + ".txt");

            //    //continue;

            //    stopWatch.Start();
            //    string input = purifyText(File.ReadAllText(file));
            //    //string inputPurified = purifyText(input);
            //    //File.WriteAllText(@"E:\Google Drive\Document\luan van\normalizedDocs\0 - 1000(1).txt", purifyText(input));
            //    string[] inputArr = input.Split('\n');
            //    //dựa trên từ điển âm tiết
            //    string output = "";
            //    foreach (string s in inputArr)
            //    {
            //        string[] sArr = s.Split(' ');
            //        foreach (string iSArr in sArr)
            //        {
            //            if (VNDictionary.getInstance.isSyllableVN(iSArr))
            //                output += iSArr + " ";
            //        }
            //        output += "\n";
            //    }
            //    File.WriteAllText(string.Format(@"C:\Users\Kiet\OneDrive\Thesis\normalizedDocs\{0}.txt", ++count), output);
            //    //System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" +file.Substring(folderPath.Length, file.Length -folderPath.Length - ".txt".Length)  + ".txt");
            //    stopWatch.Stop();
            //    TimeSpan ts = stopWatch.Elapsed;
            //    string elapseTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //                        ts.Hours, ts.Minutes, ts.Seconds,
            //                        ts.Milliseconds / 10);
            //    Console.WriteLine(count + "------ " + elapseTime);
            //}
        }

        public static string toInt16(int tokenIndex)
        {
            string s = Convert.ToString(tokenIndex, 2);
            int missBitNumber = 16 - s.Length;
            return s.Insert(0, new string('0', missBitNumber));
        }

        public static string toInt32(int tokenIndex1, int tokenIndex2)
        {
            return toInt16(tokenIndex1) + toInt16(tokenIndex2);
        }
        public static int convertBinToDec(string number)
        {
            return Int16.Parse(Convert.ToString(Convert.ToInt32(number, 2), 10));
        }
        public static int getFirstSyllableIndex(string number)
        {
            return convertBinToDec(number.Substring(0, 16));
        }
        public static int getSecondSyllableIndex(string number)
        {
            return convertBinToDec(number.Substring(16, 16));
        }

        public static string purifyText(string text)
        {
            string reporterName = @"Ảnh: \w+";
            //string tag = @"<<\w|/\w>>|»"; //(\A<<t(\w|\W)+/t>>\z)|
            //string clean = @"\s*[a|c|t]\s";
            string tag = @"<<t[^>>]+/t>>|<<\w|/\w>>|»";
            string comment = @"<!--.*?-->";
            string link = @"\b(?:https?://|(www)\.)\S+\b";
            string date = @"(\d{1,2})[\/ ]\s*(\d{1,2})(?:/|, )(\d{2,4})";
            string res = Regex.Replace(text, reporterName + "|" + tag + "|" + comment + "|" + link + "|" + date, "");
            //string res = Regex.Replace(text, reporterName + "|" + tag + "|" + link + "|" + date, ""); "|" + clean +
            return res.Trim();
        }

        public static List<string> getPhrase(string text)
        {
            string[] phraseArr = new Regex(StringConstant.getInstance.patternGetSentenceCharacter).Split(text);
            return phraseArr.ToList();
        }
    }
}