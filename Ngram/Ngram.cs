using Spell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ngram
{
    public class Ngram
    {



        public Dictionary<string, int> UniGramCount; //chứa tất cả xác suất của unigram
        public Dictionary<string, int> BiGramCount; //chứa tất cả xác suất của bigram
        public Dictionary<string, int> TriGramCount; //chứa tất cả xác suất của trigram
        private int sumUni = 0, sumBi = 0, sumTri = 0;

        public Dictionary<string, int> UniPos { get; set; }
        public Dictionary<int, string> PosUni { get; set; }//Chưa key và position của unigram
        public Dictionary<string, int> UniAmount { get; set; }// Chứa Key và giá trị tần số của unigram
        public Dictionary<string, int> BiAmount { get; set; }// Chứa Key và giá trị tần số của bigram
        public Dictionary<string, int> TriAmount { get; set; } // Chứa Key và giá trị tần số của trigram

        /// <summary>
        /// 
        /// </summary>
        private Ngram()
        {
            //this.DictionaryForNgram = new Dictionary<string, int>();
            //this.Text = "";
            //this.UniGramCount = new Dictionary<string, int>();
            //this.BiGramCount = new Dictionary<string, int>();
            //this.TriGramCount = new Dictionary<string, int>();

            UniPos = new Dictionary<string, int>();
            PosUni = new Dictionary<int, string>();
            UniAmount = new Dictionary<string, int>();
            BiAmount = new Dictionary<string, int>();
            TriAmount = new Dictionary<string, int>();
            //runFirst();
            //generateUnigram();

        }
        private static Ngram instance = new Ngram();
        public static Ngram Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// dùng để khởi tạo bộ ngram, và sinh bộ xác suất
        /// </summary>
        public void runFirst()
        {
            //readUni(@"E:\Google Drive\Document\luan van\ngram\UniNgram\uniExtended.txt");
            //convertFileBigram(@"E:\Google Drive\Document\luan van\ngram\BiNgram\bi.txt");
            //sortUni();
            //generateBigram();
            //readBiAmount(@"E:\Google Drive\Document\luan van\ngram\BiNgram\bi.txt");
            //readTriAmount(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri4.txt");
            //generateTrigram(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri4.txt");
            //tachFileTrigam();
            gopFileTrigram();


            //changeToNumber();
            //chonLocFileUniGram();
            //chonLocFileBiGram();
            //chonLocFileTriGram();
        }

        private void chonLocFileUniGram()
        {
            string[] uniArr = File.ReadAllLines("Resources\\uni.txt");
            StringBuilder builder = new StringBuilder();
            int n = 0;
            int length = uniArr.Length;
            foreach (string uni in uniArr)
            {

                string[] uniPart = uni.Split(' ');
                if (VNDictionary.getInstance.isSyllableVN(uniPart[0]) && uniPart[0].Length <= 7 && Int32.Parse(uniPart[2]) > 80)
                {
                    n++;
                    builder.AppendFormat("{0} {1} {2}{3}", uniPart[0], n, uniPart[2], "\n");
                    Console.WriteLine(string.Format("{0}/{1} = {2}%", n, length, Math.Round((double)n * 100 / length, 2)));
                }
            }
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\UniNgram\uniExtended.txt", builder.ToString());
        }
        private void chonLocFileBiGram()
        {
            string[] biArr = File.ReadAllLines("Resources\\bi2.txt");
            StringBuilder builder = new StringBuilder();
            int n = 0;
            int length = biArr.Length;
            foreach (string bi in biArr)
            {

                string[] biParts = bi.Split('_');
                string[] sylls = biParts[0].Split(' ');
                if (PosUni.ContainsKey(Int16.Parse(sylls[0])) 
                    && PosUni.ContainsKey(Int16.Parse(sylls[1])) 
                    && Int32.Parse(biParts[1]) > 15)
                {
                    n++;
                    builder.AppendFormat("{0} {1} {2}{3}", sylls[0], sylls[1], biParts[1], "\n");
                    Console.WriteLine(string.Format("{0}/{1} = {2}%", n, length, Math.Round((double)n * 100 / length, 2)));
                }
            }
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\BiNgram\biExtended.txt", builder.ToString());
        }
        private void chonLocFileTriGram()
        {
            string[] triArr = File.ReadAllLines("Resources\\tri1.txt");
            StringBuilder builder = new StringBuilder();
            int n = 0;
            int length = triArr.Length;
            foreach (string tri in triArr)
            {
                if (tri.Contains('_'))
                {
                    string[] triParts = tri.Split('_');
                    string[] sylls = triParts[0].Split(' ');
                    if (PosUni.ContainsKey(Int16.Parse(sylls[0]))
                        && PosUni.ContainsKey(Int16.Parse(sylls[1]))
                        && PosUni.ContainsKey(Int16.Parse(sylls[2]))
                        && Int32.Parse(triParts[1]) > 3)
                    {
                        n++;
                        builder.AppendFormat("{0} {1} {2} {3}{4}", sylls[0], sylls[1], sylls[2], triParts[1], "\n");
                        Console.WriteLine(string.Format("{0}/{1} = {2}%", n, length, Math.Round((double)n * 100 / length, 2)));
                    }
                }
            }
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri1Extended.txt", builder.ToString());
        }
        #region convert between dec and bin
        private string toInt16(int tokenIndex)
        {
            string s = Convert.ToString(tokenIndex, 2);
            int missBitNumber = 16 - s.Length;
            return s.Insert(0, new string('0', missBitNumber));
        }

        private string toInt32(int tokenIndex1, int tokenIndex2)
        {
            return toInt16(tokenIndex1) + toInt16(tokenIndex2);
        }
        private string toInt32(string tokenIndex)
        {
            string s = Convert.ToString(Int32.Parse(tokenIndex), 2);
            int missBitNumber = 32 - s.Length;
            return s.Insert(0, new string('0', missBitNumber));
        }
        private int convertBinToDec(string number)
        {
            return Convert.ToInt32(number, 2);
        }
        private int getFirstSyllableIndex(string number)
        {
            return convertBinToDec(number.Substring(0, 16));
        }
        private int getSecondSyllableIndex(string number)
        {
            return convertBinToDec(number.Substring(16, 16));
        }
        #endregion
        public double calBiNgram(string w1, string w2)
        {
            int MAX = 10;
            string key = w1 + " " + w2;
            int Cw1 = MAX;
            int Cw1w2 = 0;
            if (UniGramCount.ContainsKey(w1.ToLower()))
                Cw1 = UniGramCount[w1.ToLower()];
            if (BiGramCount.ContainsKey(key.ToLower()))
                Cw1w2 = BiGramCount[key.ToLower()];
            return (double)(Cw1w2 + 1) / (Cw1 + sumUni);
        }

        /// <summary>
        /// Đếm tổng số lần xuất hiện của từng từ trong corpus
        /// </summary>
        public void sumWordInCorpus()
        {
            foreach (KeyValuePair<string, int> temp in UniGramCount)
            {
                sumUni += temp.Value;
            }
        }

        /// <summary>
        /// tạo từng key cho Dictionary
        /// </summary>
        /// <param name="words">bộ ngữ liệu</param>
        /// <param name="start">vị trí bắt đầu</param>
        /// <param name="end">vị trí kết thúc</param>
        /// <returns></returns>
        private string generateEachClusterNgram(string[] words, int start, int end)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = start; i < end; i++)
                sb.Append((i > start ? " " : "") + words[i]);
            return sb.ToString();
        }

        #region UniGram

        /// <summary>
        /// Gọi hàm khoiTaoBoNgram với tham số là 1 và bộ ngữ liệu
        /// </summary>
        public void generateUnigram()
        {
            string folderPath = @"E:\Google Drive\Document\luan van\ngram\input\";
            int count = 1;// da chay toi 5
            Stopwatch stopWatch = new Stopwatch();
            string[] getFile = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
            Dictionary<string, int> uniPos = new Dictionary<string, int>();
            Dictionary<string, int> uniAmount = new Dictionary<string, int>();
            int pos = 1;
            int amount = 1;
            foreach (string file in getFile)
            {
                //System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" + ++count + ".txt");

                //continue;

                stopWatch.Start();
                string input = File.ReadAllText(file);
                string[] words = new Regex("\\s+|,\\s*|\\.\\s*").Split(input);
                string key = "";
                int n = 1; //uni
                for (int i = 0; i < words.Length - n + 1; i++)
                {
                    key = generateEachClusterNgram(words, i, i + n);
                    key = key.ToLower();
                    if (key.Length > 0)
                        if (uniPos.ContainsKey(key))
                            uniAmount[key] += 1;
                        else
                        {
                            uniPos.Add(key, pos++);
                            uniAmount.Add(key, amount);
                        }
                }

                //System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" + file.Substring(folderPath.Length, file.Length - folderPath.Length - ".txt".Length) + ".txt");
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapseTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                    ts.Hours, ts.Minutes, ts.Seconds,
                                    ts.Milliseconds / 10);
                Console.WriteLine(string.Format("Creating unigram: {0}/{1}--------{2}", count++, getFile.Length, elapseTime));
                if (count == 2)
                    break;
            }
            string output = "";
            foreach (KeyValuePair<string, int> temp in uniPos)
                output += temp.Key + " " + temp.Value + " " + uniAmount[temp.Key] + "\n";
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\UniNgram\uni.txt", output);
        }

        /// <summary>
        /// Đọc dữ liệu của unigram: key và value (vị trí).
        /// </summary>
        /// <param name="path">Đường dẫn tới thư mục</param>
        public void readUni(string path)
        {
            string[] uniGram = File.ReadAllLines(path);
            int n = 0;
            StringBuilder builer = new StringBuilder();
            foreach (string line in uniGram)
            {
                n++;
                string[] uni = line.Split(' ');
                string key = uni[0];
                if (key.Contains("­­­­­-"))
                    continue;
                int pos = Int16.Parse(uni[1]);
                int amount = Int32.Parse(uni[2]);
                UniPos.Add(key, pos);
                PosUni.Add(pos, key);
                UniAmount.Add(key, amount);
                builer.AppendFormat("{0} {1} {2}{3}", key, pos, amount, "\n");
            }
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\UniNgram\newuni.txt", builer.ToString());
        }
        private void sortUni()
        {
            List<string> keys = UniPos.Keys.ToList();
            keys.Sort((key1, key2) => key2.Length.CompareTo(key1.Length)); ;
            StringBuilder ouput = new StringBuilder();
            int count = 0;
            foreach (string key in keys)
            {
                ouput.AppendFormat("{0} {1} {2}{3}", key, ++count, UniAmount[key], "\n");
            }
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\UniNgram\uni1.txt", ouput.ToString());
        }
        /// <summary>
        /// Đọc dữ liệu của unigram: key và amount (giá trị tần số).
        /// </summary>
        /// <param name="path">Đường dẫn tới thư mục</param>
        public void readUniAmount(string path)
        {
            string[] uniGram = File.ReadAllLines(path);
            foreach (string line in uniGram)
            {
                string[] uni = line.Split('-');
                UniAmount.Add(uni[0], Int32.Parse(uni[2]));
            }
        }
        #endregion

        #region BiGram
        /// <summary>
        /// Gọi hàm khoiTaoBoNgram với tham số là 2 và bộ ngữ liệu
        /// </summary>
        //ví dụ bigram: máy tính
        //máy có index trong unigram là 1200
        //tính có index trong unigram là 1245
        //chuyển 1200 và 1245 sang int16 dạng bin
        //cộng 2 chuỗi lại được int32 dạng bin
        //chuyển sang int32 dạng dec và ghi file


        public void generateBigram()
        {
            //Dictionary<string, int> biGram = new Dictionary<string, int>();
            string folderPath = @"E:\Google Drive\Document\luan van\ngram\input\";
            int count = 1;
            Stopwatch stopWatch = new Stopwatch();
            string[] getFile = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
            Dictionary<string, int> biAmount = new Dictionary<string, int>();
            int pos = 1;
            int amount = 1;
            foreach (string file in getFile)
            {
                //System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" + ++count + ".txt");

                //continue;

                stopWatch.Start();
                string input = File.ReadAllText(file);
                string[] words = new Regex("\\s+|,\\s*|\\.\\s*").Split(input);
                string key = "";
                int n = 2; //bi
                for (int i = 0; i < words.Length - n + 1; i++)
                {
                    key = generateEachClusterNgram(words, i, i + n);
                    key = key.ToLower();
                    string[] sylls = key.Split(' ');
                    if (sylls[0].Length > 0 && sylls[1].Length > 0)
                        if (biAmount.ContainsKey(key))
                            biAmount[key] += 1;
                        else
                        {
                            biAmount.Add(key, amount);
                        }
                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapseTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                    ts.Hours, ts.Minutes, ts.Seconds,
                                    ts.Milliseconds / 10);
                Console.WriteLine(string.Format("Creating bigram: {0}/{1}--------{2}", count++, getFile.Length, elapseTime));
            }
            //tổ chức lưu trữ

            StringBuilder output = new StringBuilder();
            count = 1;
            Console.WriteLine(biAmount.Count);
            foreach (KeyValuePair<string, int> temp in biAmount)
            {
                count++;
                string[] sylls = temp.Key.Split(' ');
                //int firstSyllIndex = _uniPos[sylls[0]];
                //int secondSyllIndex = _uniPos[sylls[1]];
                //string index = toInt32(firstSyllIndex, secondSyllIndex);
                //output += convertBinToDec(index) + "-" + temp.Value + "\n";
                //output.AppendFormat("{0}_{1}{2}", temp.Key, temp.Value, "\n");
                output.AppendFormat("{0} {1}_{2}{3}", UniPos[sylls[0]], UniPos[sylls[1]], temp.Value, "\n");
                if (count % 1000000 == 0 || count == biAmount.Count)
                    Console.WriteLine(string.Format("Converting bigram: {0}/{1}", count, biAmount.Count));
            }
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\BiNgram\bi2.txt", output.ToString());
        }
        //đọc file bigram
        //chuyển dec sang int32, được 32bit
        //chuyển 16 bit đầu, và 16 bit sau lần lượt sang int16 dạng hec ---->1200, 1245
        public void readBiAmount(string path)
        {
            string[] biGram = File.ReadAllLines(path); int count = 0;
            foreach (string line in biGram)
            {
                count++;
                string[] bi = line.Split('_');
                //string index = toInt32(bi[0]);
                //string firstSyll = _posUni[getFirstSyllableIndex(index)];
                //string secondSyll = _posUni[getSecondSyllableIndex(index)];
                //_biAmount.Add(firstSyll + " " + secondSyll, Int32.Parse(bi[1]));
                try
                {
                    BiAmount.Add(bi[0], Int32.Parse(bi[1]));
                    if (count % 100000 == 0 || count == biGram.Length)
                        Console.WriteLine(string.Format("reading bigram: {0}/{1}", count, biGram.Length));
                }
                catch
                {
                    Console.WriteLine(bi[0] + "-" + bi[1]);
                }
            }
        }
        private void convertFileBigram(string path)
        {
            string[] biArr = File.ReadAllLines(path);
            int count = 1;
            StringBuilder output = new StringBuilder();
            foreach (string temp in biArr)
            {
                count++;
                string[] bi = temp.Split('_');
                string[] sylls = bi[0].Split(' ');
                //int firstSyllIndex = _uniPos[sylls[0]];
                //int secondSyllIndex = _uniPos[sylls[1]];
                //string index = toInt32(firstSyllIndex, secondSyllIndex);
                //output += convertBinToDec(index) + "-" + temp.Value + "\n";
                //output.AppendFormat("{0}_{1}{2}", temp.Key, temp.Value, "\n");
                output.AppendFormat("{0} {1}_{2}{3}", UniPos[sylls[0]], UniPos[sylls[1]], bi[1], "\n");
                if (count % 1000000 == 0 || count == biArr.Length)
                    Console.WriteLine(string.Format("Converting bigram: {0}/{1}", count, biArr.Length));
            }
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\BiNgram\bi2.txt", output.ToString());
        }
        #endregion


        #region TriGram


        /// <summary>
        /// 
        /// </summary>
        public void generateTrigram(string sourcePath, string desPath)
        {
            //Dictionary<string, int> biGram = new Dictionary<string, int>();
            string folderPath = @"E:\Google Drive\Document\luan van\ngram\input\";
            int count = 1;// da chay toi 5
            Stopwatch stopWatch = new Stopwatch();
            string[] getFile = Directory.GetFiles(sourcePath, "*.txt", SearchOption.AllDirectories);
            //Dictionary<string, int> TriAmount = new Dictionary<string, int>();
            Console.WriteLine("triAmount count[before]: " + this.TriAmount.Count);
            int pos = 1;
            int amount = 1;
            foreach (string file in getFile)
            {
                //System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" + ++count + ".txt");

                //continue;

                stopWatch.Start();
                string input = File.ReadAllText(file);
                string[] words = new Regex("\\s+|,\\s*|\\.\\s*").Split(input);
                string key = "";
                int n = 3; //tri
                for (int i = 0; i < words.Length - n + 1; i++)
                {
                    key = generateEachClusterNgram(words, i, i + n);
                    key = key.ToLower();
                    string[] sylls = key.Split(' ');
                    if (sylls[0].Length > 0 && sylls[1].Length > 0 && sylls[2].Length > 0)
                        if (TriAmount.ContainsKey(key))
                            TriAmount[key] += 1;
                        else
                        {
                            TriAmount.Add(key, amount);
                        }
                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapseTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                    ts.Hours, ts.Minutes, ts.Seconds,
                                    ts.Milliseconds / 10);
                Console.WriteLine(string.Format("Creatting trigram: {0}/{1}--------{2}", count++, getFile.Length, elapseTime));
            }
            //tổ chức lưu trữ
            Console.WriteLine("triAmount count[after]: " + TriAmount.Count);
            StringBuilder output = new StringBuilder();
            count = 1;
            foreach (KeyValuePair<string, int> temp in TriAmount)
            {
                count++;
                //string[] sylls = temp.Key.Split(' ');
                //int firstSyllIndex = _uniPos[sylls[0]];
                //int secondSyllIndex = _uniPos[sylls[1]];
                //ushort lastSyllIndex = (ushort)_uniPos[sylls[2]];
                //string index = toInt32(firstSyllIndex, secondSyllIndex);
                output.AppendFormat("{0}_{1}{2}", temp.Key, temp.Value, "\n");
                if (count % 1000000 == 0 || count == TriAmount.Count)
                    Console.WriteLine(string.Format("Converting trigram: {0}/{1}", count, TriAmount.Count));
            }

            File.WriteAllText(desPath, output.ToString());
        }
        /// <summary>
        /// pos in unigram
        /// </summary>
        private void changeToNumber()
        {
            Dictionary<string, int> tri4 = readTriAmount(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri4.txt");
            Console.WriteLine("triAmount count[after]: " + tri4.Count);
            StringBuilder output = new StringBuilder();
            int count = 1;

            foreach (string pair in tri4.Keys)
            {
                count++;
                string[] key = pair.Split(' ');
                output.AppendFormat("{0} {1} {2}_{3}{4}", UniPos[key[0]], UniPos[key[1]], UniPos[key[2]], tri4[pair], "\n");
                if (count % 1000000 == 0 || count == tri4.Count)
                    Console.WriteLine(string.Format("Converting trigram: {0}/{1}", count, tri4.Count));
            }
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\TriGram\new\tri4.txt", output.ToString());
        }
        /// <summary>
        /// pos in unigram
        /// </summary>

        private void tachFileTrigam()
        {
            string[] lines = File.ReadAllLines(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri24.txt");
            StringBuilder builder = new StringBuilder();
            int length = lines.Length;
            for (int i = 0; i < length; i++)
            {
                builder.AppendLine(lines[i]);
                if (i == length / 3)
                {
                    File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri4.txt", builder.ToString());
                    builder.Remove(0, builder.Length - 1);
                }
                if (i == 2 * length / 3)
                {
                    File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri5.txt", builder.ToString());
                    builder.Remove(0, builder.Length - 1);
                }
                if (i % 100000 == 0 || i == length - 1)
                    Console.WriteLine(string.Format("{0}/{1}", i, length));
            }
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri6.txt", builder.ToString());

        }
        private void gopFileTrigram()
        {

            Dictionary<string, int> tri1 = readTriAmount(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri1Extended.txt");
            Dictionary<string, int> tri2 = readTriAmount(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri2Extended.txt");
            Dictionary<string, int> tri3 = readTriAmount(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri3Extended.txt");
            Dictionary<string, int> tri4 = readTriAmount(@"E:\Google Drive\Document\luan van\ngram\TriGram\tri4Extended.txt");
            foreach (var pair in tri1)
            {
                if (tri2.ContainsKey(pair.Key))
                    tri2[pair.Key] += 1;
                else
                {
                    tri2.Add(pair.Key, pair.Value);
                }
            }
            foreach (var pair in tri3)
            {
                if (tri2.ContainsKey(pair.Key))
                    tri2[pair.Key] += 1;
                else
                {
                    tri2.Add(pair.Key, pair.Value);
                }
            }
            foreach (var pair in tri4)
            {
                if (tri2.ContainsKey(pair.Key))
                    tri2[pair.Key] += 1;
                else
                {
                    tri2.Add(pair.Key, pair.Value);
                }
            }
            Console.WriteLine("triAmount count[after]: " + tri2.Count);
            StringBuilder output = new StringBuilder();
            int count = 1;
            foreach (KeyValuePair<string, int> temp in tri2)
            {
                count++;
                //string[] sylls = temp.Key.Split(' ');
                //int firstSyllIndex = _uniPos[sylls[0]];
                //int secondSyllIndex = _uniPos[sylls[1]];
                //ushort lastSyllIndex = (ushort)_uniPos[sylls[2]];
                //string index = toInt32(firstSyllIndex, secondSyllIndex);
                output.AppendFormat("{0} {1}{2}", temp.Key, temp.Value, "\n");
                if (count % 1000000 == 0 || count == tri2.Count)
                    Console.WriteLine(string.Format("Converting trigram: {0}/{1}", count, tri2.Count));
            }

            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\TriGram\triExtended.txt", output.ToString());
        }

        public Dictionary<string, int> readTriAmount(string path)
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();

            string[] triGram = File.ReadAllLines(path);
            int count = 0;
            string[] tri;
            foreach (string line in triGram)
            {
                count++;
                if (line.Length > 0)
                {
                    tri = line.Split(' ');
                    //try
                    //{
                    ret.Add(String.Format("{0} {1} {2}", tri[0],tri[1],tri[2]), Int32.Parse(tri[3]));
                    if (count % 100000 == 0 || count == triGram.Length)
                        Console.WriteLine(string.Format("reading trigram: {0}/{1}", count, triGram.Length));
                    //}
                    //catch(Exception e)
                    //{

                    //Console.WriteLine(tri[0] + "-" + tri[1]);
                    //}

                    //string index = toInt32(tri[0]);
                    //string firstSyll = PosUni[getFirstSyllableIndex(index)];
                    //string secondSyll = PosUni[getSecondSyllableIndex(index)];
                    //string lastSyll = PosUni[ushort.Parse( tri[1])];
                    //TriAmount.Add(firstSyll + " " + secondSyll + " " + lastSyll, Int16.Parse(tri[2]));
                }
            }
            //foreach (var pair in TriAmount)
            //    ret.Add(pair.Key, pair.Value);
            return ret;
        }

        #endregion

        /// <summary>
        /// đọc từ file dna.txt trong resources và gán giá trị vào Text
        /// </summary>
        private void readFileCorpus()
        {

            string folderPath = @"C:\Users\Kiet\OneDrive\Thesis\Ngram\Input\";
            int count = 3;// da chay toi 5
            Stopwatch stopWatch = new Stopwatch();
            string[] getFile = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

            foreach (string file in getFile)
            {
                //System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" + ++count + ".txt");

                //continue;

                stopWatch.Start();
                string input = File.ReadAllText(file);
                //string inputPurified = purifyText(input);
                //File.WriteAllText(@"E:\Google Drive\Document\luan van\normalizedDocs\0 - 1000(1).txt", purifyText(input));
                //string[] inputArr = input.Split('\n');
                //dựa trên từ điển âm tiết
                string output = "";
                //foreach (string s in inputArr)
                //{
                //    string[] sArr = s.Split(' ');
                //    foreach (string iSArr in sArr)
                //    {
                //        if (VNDictionary.getInstance.isSyllableVN(iSArr))
                //            output += iSArr + " ";
                //    }
                //    output += "\n";
                //}
                File.WriteAllText(string.Format(@"C:\Users\Kiet\OneDrive\Thesis\Corpus\{0}.txt", ++count), output);
                System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" + file.Substring(folderPath.Length, file.Length - folderPath.Length - ".txt".Length) + ".txt");
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapseTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                    ts.Hours, ts.Minutes, ts.Seconds,
                                    ts.Milliseconds / 10);
                Console.WriteLine(count + "------ " + elapseTime);
            }


            //try
            //{
            //    string[] data = File.ReadAllLines(@"Resources\dna.txt");
            //    for (int i = 0; i < data.Length; i++)
            //        this.Text += data[i];
            //}
            //catch (IOException e)
            //{
            //    Console.WriteLine("Khong doc duoc file" + e);
            //}
        }
        /// <summary>
        /// ghi 3 file xác suất
        /// </summary>
        private void writeFileProbability()
        {
            writePartialFileProbability("UniGram-Probability.txt", this.UniGramCount);
            writePartialFileProbability("BiGram-Probability.txt", this.BiGramCount);
            writePartialFileProbability("TriGram-Probability.txt", this.TriGramCount);
        }
        /// <summary>
        /// Ghi file xác suất theo yêu cầu
        /// </summary>
        /// <param name="fileName">tên file cần ghi</param>
        /// <param name="ngramPro">loại file</param>
        private void writePartialFileProbability(string fileName, Dictionary<string, int> ngramPro)
        {
            try
            {
                string[] data = new string[ngramPro.Count];
                int i = 0;
                foreach (KeyValuePair<string, int> pair in ngramPro)
                    data[i++] = pair.Value + " " + pair.Key;
                File.WriteAllLines(fileName, data);
            }
            catch { }
        }


        /// <summary>
        /// Thêm 1 từ vào từ điển.
        /// 
        /// Sử dụng khi người dùng cho rằng từ đang được chương trình nghi ngờ là lỗi,
        /// thì không phải là lỗi
        /// và muốn bỏ qua lỗi đó cho những lần sau
        /// </summary>
        /// <param name="X">từ cần thêm vào từ điển</param>
        public bool addToDictionary(string X, string x1, string x2, Position pos)
        {
            switch (pos)
            {
                case Position.X:
                case Position.xX:
                case Position.Xx:
                case Position.xxX:
                case Position.xXx:
                case Position.Xxx:
                    break;
            }
            return false;
        }
    }
}
