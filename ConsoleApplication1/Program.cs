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

            //string folderPath = @"C:\Users\Kiet\OneDrive\Thesis\normalizedDocs\";
            //int count = 349 ;// da chay toi 349
            //Stopwatch stopWatch = new Stopwatch();
            //string[] getFile = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

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
            //    File.WriteAllText(string.Format(@"C:\Users\Kiet\OneDrive\Thesis\Corpus\{0}.txt",++count), output);
            //    System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" +file.Substring(folderPath.Length, file.Length -folderPath.Length - ".txt".Length)  + ".txt");
            //    stopWatch.Stop();
            //    TimeSpan ts = stopWatch.Elapsed;
            //    string elapseTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //                        ts.Hours, ts.Minutes, ts.Seconds,
            //                        ts.Milliseconds / 10);
            //    Console.WriteLine(count + "------ " + elapseTime);
            //}
            List<string[]> l = new List<string[]>
            {
                new string[]{"nghiêng", "nghi"},
                new string[]{"bằng", "bằgn"},
                new string[]{"vi", "thuvi"},
                new string[]{"vi", "nhiên"},
                new string[]{"nghiêng", "nghiêtng"},
                new string[]{"nghiêng", "máy"},
                new string[]{"nắng", "những"},
            };

            foreach (string[] a in l)
            {
                double cost = calScore_Similarity(a[0], a[1]);
                Console.WriteLine("{0}, {1} = {2}",
                    a[0],
                    a[1],
                    cost);
            }
            //Console.WriteLine(extractSignVN("ộng"));
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
            string tag = @"<<t[^>>]+/t>>|<<\w|/\w>>|»";
            string comment = @"<!--.*?-->";
            string link = @"\b(?:https?://|(www)\.)\S+\b";
            string date = @"(\d{1,2})[\/ ]\s*(\d{1,2})(?:/|, )(\d{2,4})";
            string res = Regex.Replace(text, reporterName + "|" + tag + "|" + comment + "|" + link + "|" + date, "");
            return res.Trim();
        }
        public static int levenshtein(string a, string b)
        {

            if (string.IsNullOrEmpty(a))
            {
                if (!string.IsNullOrEmpty(b))
                {
                    return b.Length;
                }
                return 0;
            }

            if (string.IsNullOrEmpty(b))
            {
                if (!string.IsNullOrEmpty(a))
                {
                    return a.Length;
                }
                return 0;
            }

            int cost;
            int[,] d = new int[a.Length + 1, b.Length + 1];
            int min1;
            int min2;
            int min3;

            for (int i = 0; i <= d.GetUpperBound(0); i += 1)
            {
                d[i, 0] = i;
            }

            for (int i = 0; i <= d.GetUpperBound(1); i += 1)
            {
                d[0, i] = i;
            }

            for (int i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (int j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    cost = Convert.ToInt32(!(a[i - 1] == b[j - 1]));

                    min1 = d[i - 1, j] + 1;
                    min2 = d[i, j - 1] + 1;
                    min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];

        }
        public static double calEditDist(string token, string candidate)
        {
            double ret = 0;
            int editDist = levenshtein(token, candidate);
            ret = (double)1 / (1 + editDist);
            return ret;
        }
        public static int triIntersection(string x, string y)
        {
            HashSet<string> hSetTriX = getHSetTri(x);
            HashSet<string> hSetTriY = getHSetTri(y);
            IEnumerable<string> both = hSetTriX.Intersect(hSetTriY);
            return both.Count();
        }
        public static HashSet<string> getHSetTri(string x)
        {
            HashSet<string> hsetTri = new HashSet<string>();
            if (x.Length < 4)
                return hsetTri;
            for (int i = 0; i < x.Length - 2; i++)
            {
                hsetTri.Add(x.Substring(i, 3));
            }
            return hsetTri;
        }
        public static double calTri(string x, string y)
        {
            double ret = 0;
            int triX = tri(x);
            int triY = tri(y);
            int triXY = triIntersection(x, y);
            ret = (double)1 / (1 + triX + triY - 2 * (triXY));
            return ret;
        }
        public static int tri(string x)
        {
            return getHSetTri(x).Count;
        }
        public static double calStringSim(string token, string candidate)
        {
            double simEdit = calEditDist(token, candidate);

            double simTri = calTri(token, candidate);
            return simEdit + simTri;
        }
        public static double calScore_Similarity_Keyboard(char token, char candidate)
        {
            int isFound = 0;
            int iCandidate = -1, iToken = -1, jCandidate = -1, jToken = -1;
            //duyệt qua từng dòng trong keyboard matrix
            for (int row = 0; row < StringConstant.MAX_KEYBOARD_ROW; row++)
            {
                //duyệt qua từng cột trong keyboard matrix
                for (int col = 0; col < StringConstant.MAX_KEYBOARD_COL; col++)
                {
                    if (StringConstant.Instance.KeyBoardMatrix_LowerCase[row, col] == candidate)
                    {
                        iCandidate = row;
                        jCandidate = col;
                        isFound++;
                    }
                    if (StringConstant.Instance.KeyBoardMatrix_LowerCase[row, col] == token)
                    {
                        iToken = row;
                        jToken = col;
                        isFound++;
                    }
                    if (isFound == 2)
                        break;
                }

                if (isFound == 2)
                    break;

            }
            int rowScore = /*StringConstant.MAX_KEYBOARD_ROW -*/ Math.Abs(iCandidate - iToken);
            int colScore = /*StringConstant.MAX_KEYBOARD_COL -*/ Math.Abs(jCandidate - jToken);
            return Math.Round((Math.Sqrt(Math.Pow(rowScore, 2) + Math.Pow(colScore, 2))) / 30, 2);
        }
        public static double calScore_StringDiff(string token, string candidate)
        {
            double diffScore = 1;
            string extToken = extractSignVN(token);
            string extCandidate = extractSignVN(candidate);
            string shorterWord = "";
            string longerWord = "";
            if (extToken.Length > extCandidate.Length)
            {
                longerWord = extToken;
                shorterWord = extCandidate;
            }
            else
            {
                longerWord = extCandidate;
                shorterWord = extToken;
            }
            int x = 0;
            for (int i = 0; i < shorterWord.Length; i++)
            {
                //nếu shorterWord[i] == longerWord[i] ---> bỏ qua
                if (shorterWord[i] == longerWord[i])
                    continue;
                //nếu shorterWord[i...k] == longerWord[i + x...k + x] ---> trừ 0.1
                //ví dụ: nawngs với nhuwngs
                if (x == 0)
                {
                    x = longerWord.IndexOf(shorterWord[i], i) - i;
                    diffScore -= 0.1;
                }
                else if (x == longerWord.IndexOf(shorterWord[i], i) - i)
                    diffScore -= 0.05 * x;
                else {
                    //nếu shorterWord[i] ~bàn phím~ longerWord[i]
                    //--------------------------------------lệch n ---> trừ nE-2
                    diffScore -= calScore_Similarity_Keyboard(shorterWord[i], longerWord[i]);
                    x = 0;
                }
            }
            //diffScore -= calScore_Similarity_Region(shorterWord, longerWord
            int deltaLength = longerWord.Length - shorterWord.Length;
            if (deltaLength > shorterWord.Length)
                diffScore -= (deltaLength + shorterWord.Length) * 0.1;
            else
                diffScore -= deltaLength * 0.1;
            if (diffScore < 0)
                return 0;
            return diffScore;
        }
        public static double calScore_Similarity(string token, string candidate)
        {
            double score = 1;
            token = token.ToLower();
            candidate = candidate.ToLower();
            double simScore = calStringSim(token, candidate);
            double diffScore = calScore_StringDiff(token, candidate);
            score = 0.5 * simScore + 0.5 * diffScore;
            if (score > 1)
                return 1;
            if (score < 0)
                return 0;
            return score;
        }

        public static string extractSignVN(string word)
        {
            string ret = "";
            int iSource = 0;
            char sign = ' ';
            char vnChar;
            int iVNChar = 0;
            foreach (char c in word)
            {
                iSource = StringConstant.Instance.source.IndexOf(c);
                //không mang dấu tiếng việt
                if (iSource == -1)
                {
                    iVNChar = StringConstant.Instance.vnCharacter.IndexOf(c);
                    if (iVNChar == -1)
                    {
                        ret += c;
                    }
                    else
                        ret += StringConstant.Instance.vnCharacter_Telex[iVNChar];
                }
                else
                {
                    vnChar = StringConstant.Instance.dest[iSource];
                    sign = StringConstant.Instance.VNSign[iSource % 5];
                    iVNChar = StringConstant.Instance.vnCharacter.IndexOf(vnChar);
                    if (iVNChar == -1)
                        ret += vnChar;
                    else ret += StringConstant.Instance.vnCharacter_Telex[iVNChar];
                }
            }
            if (sign != ' ')
                ret += sign;
            return ret;
        }
    }
}