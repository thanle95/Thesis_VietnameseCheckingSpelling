using Spell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spell.Algorithm
{
    public class Ngram
    {
        private int _sumUni = 0, _sumBi = 0;

        //private Dictionary<string, int> _uniPos { get; set; }  //Chứa key và position của unigram
        //private Dictionary<int, string> _posUni { get; set; }  //Chứa position và key của unigram
        private Dictionary<string, int> _uniAmount { get; set; }  //Chứa Key và giá trị số lượng của unigram
        public Dictionary<string, int> _biAmount { get; set; } //Chứa Key và giá trị số lượng của bigram
        //public Dictionary<string, int> _triAmount { get; set; } //Chứa Key và giá trị số lượng của trigram

        public string START_STRING
        {
            get
            {
                return "<s>";
            }
        }
        public string END_STRING
        {
            get
            {
                return "</s>";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private Ngram()
        {
            //this._uniPos = new Dictionary<string, int>();
            //this._posUni = new Dictionary<int, string>();
            this._uniAmount = new Dictionary<string, int>();
            this._biAmount = new Dictionary<string, int>();
            //this._triAmount = new Dictionary<string, int>();
            //runFirst();
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
            //generateUnigram();
            readUni(FileManager.Instance.UniGram);
            //filter_uni();
            readBiAmount(FileManager.Instance.BiGram);
            //readTriAmount(triPath);
            sumWordInCorpus();

        }
        /// <summary>
        /// loc file uni gram
        /// </summary>
        public void filter_uni()
        {
            StringBuilder filterdUni = new StringBuilder();
            string[] uni_file = File.ReadAllLines(@"E:\Google Drive\Document\luan van\source\github\Thesis_VietnameseCheckingSpelling\Spell\Resources\filteredUni.txt");
            foreach (string line in uni_file)
            {
                string[] uni = line.Split(' ');
                if (Int32.Parse(uni[2]) > 10)
                    filterdUni.AppendFormat("{0} {1}{2}", uni[0], uni[2], "\n");
            }
            File.WriteAllText(@"E:\Google Drive\Document\luan van\source\github\Thesis_VietnameseCheckingSpelling\Spell\Resources\filteredUni1.txt", filterdUni.ToString());
        }


        /// <summary>
        /// loc file bi gram
        /// </summary>
        public void filter_bi()
        {
            StringBuilder filterdBi = new StringBuilder();
            string[] bi_file = File.ReadAllLines(@"C:\Users\Kiet\OneDrive\Thesis\Thesis\Thesis_VietnameseCheckingSpelling\bi.txt");
            foreach (string line in bi_file)
            {
                string[] bi = line.Split(' ');
                if (Int32.Parse(bi[2]) > 50 && _uniAmount.ContainsKey(bi[0]) && _uniAmount.ContainsKey(bi[1]))
                    filterdBi.AppendFormat("{0} {1} {2}{3}", bi[0], bi[1], bi[2], "\n");
            }
            File.WriteAllText(@"C:\Users\Kiet\OneDrive\Thesis\Thesis\Thesis_VietnameseCheckingSpelling\filterdBi.txt", filterdBi.ToString());
        }

        private void sortUni(string uniPath)
        {
            readUni(@"C:\Users\Kiet\OneDrive\Thesis\Ngram\uni.txt");
            List<int> lstAmout = new List<int>();
            lstAmout = _uniAmount.Values.ToList();
            lstAmout.Sort();
            string[] arr = new string[lstAmout.Count];
            int j = 0;
            foreach (int i in lstAmout)
                arr[j++] = i.ToString();

            File.WriteAllLines(@"C:\Users\Kiet\OneDrive\Thesis\Ngram\uni_Sort.txt", arr);

        }
        #region convert between bin and dec
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

        #region calculate ngram
        public double calBigram(string w1, string w2)
        {
            string key = w1 + " " + w2;
            int Cw1 = 0;
            int Cw1w2 = 0;
            double alpha = 0.1;
            if (_uniAmount.ContainsKey(w1.ToLower()))
                Cw1 = _uniAmount[w1.ToLower()];
            if (_biAmount.ContainsKey(key.ToLower()))
                Cw1w2 = _biAmount[key.ToLower()];
            double ret = (double)(Cw1w2 + alpha) / (Cw1 + _sumUni*alpha);
            return ret;

        }
        public int getBiGram(string w1, string w2)
        {
            string key = w1 + " " + w2;
            if (_biAmount.ContainsKey(key.ToLower()))
                return _biAmount[key.ToLower()];
            return 0;
        }
        public double calTrigram(string w1, string w2, string w3)
        {
            ////int MAX = 10;
            //string key = w1 + " " + w2 + " " + w3;
            //string bi = w1 + " " + w2;
            //int Cw1w2 = 0;
            //int Cw1w2w3 = 0;
            //if (_biAmount.ContainsKey(bi.ToLower()))
            //    Cw1w2 = _biAmount[bi.ToLower()];
            //if (_triAmount.ContainsKey(key.ToLower()))
            //    Cw1w2w3 = _triAmount[key.ToLower()];
            //double ret = (double)(Cw1w2w3 + 1) / (Cw1w2 + _sumBi);
            //return ret;
            return 0;
        }
        #endregion

        /// <summary>
        /// Đếm tổng số lần xuất hiện của từng từ trong corpus
        /// </summary>
        public void sumWordInCorpus()
        {
            foreach (KeyValuePair<string, int> temp in _uniAmount)
            {
                _sumUni += temp.Value;
            }
            foreach (KeyValuePair<string, int> temp in _biAmount)
            {
                _sumBi += temp.Value;
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
            int count = 1;// da chay toi 5
            Stopwatch stopWatch = new Stopwatch();
            string[] getFile = Directory.GetFiles(@"E:\Google Drive\Document\luan van\ngram\input", "*.txt", SearchOption.AllDirectories);
            Dictionary<string, int> uniPos = new Dictionary<string, int>();
            Dictionary<string, int> uniAmount = new Dictionary<string, int>();
            int amount = 1;
            foreach (string file in getFile)
            {
                //System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" + ++count + ".txt");

                //continue;

                stopWatch.Start();
                string[] phraseArr = File.ReadAllLines(file);
                foreach (string phrase in phraseArr)
                {
                    string newPhrase = String.Format("{0} {1} {2}", "<s>", phrase.ToLower(), "</s>");
                    string[] words = newPhrase.Split(' ');
                    foreach (string key in words)
                    {
                        if (key.Length > 0)
                            if (uniAmount.ContainsKey(key))
                                uniAmount[key] += 1;
                            else
                            {
                                uniAmount.Add(key, amount);
                            }
                    }
                }

                //System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" + file.Substring(folderPath.Length, file.Length - folderPath.Length - ".txt".Length) + ".txt");
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapseTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                    ts.Hours, ts.Minutes, ts.Seconds,
                                    ts.Milliseconds / 10);
                Console.WriteLine(string.Format("Creating unigram: {0}/{1}--------{2}", count++, getFile.Length, elapseTime));
            }
            string output = "";
            foreach (KeyValuePair<string, int> pair in uniPos)
                if (uniAmount[pair.Key] > 100)
                    output += pair.Key + " " + pair.Value + " " + uniAmount[pair.Key] + "\n";
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\UniNgram\uni.txt", output);
        }

        /// <summary>
        /// Đọc dữ liệu của unigram: key và value (vị trí).
        /// </summary>
        /// <param name="path">Đường dẫn tới thư mục</param>
        public void readUni(string path)
        {
            List<string> uniList = Properties.Resources.filteredUni.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string[] uniArr;
            if (uniList.Count == 1)
            {
                uniArr = uniList[0].Split('\n');
                uniList = uniArr.ToList();
            }
            foreach (string line in uniList)
            {
                if (line.Length == 0)
                    continue;
                string[] uni = line.Split(' ');
                //_uniPos.Add(uni[0], Int32.Parse(uni[1]));
                //_posUni.Add(Int32.Parse(uni[1]), uni[0]);
                _uniAmount.Add(uni[0], Int32.Parse(uni[2]));
                _sumUni += Int32.Parse(uni[2]);
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
            ////Dictionary<string, int> biGram = new Dictionary<string, int>();
            //string folderPath = @"E:\Google Drive\Document\luan van\ngram\input\";
            //int count = 1;
            //Stopwatch stopWatch = new Stopwatch();
            //string[] getFile = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
            //Dictionary<string, int> biAmount = new Dictionary<string, int>();
            //int amount = 1;
            //foreach (string file in getFile)
            //{
            //    //System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" + ++count + ".txt");

            //    //continue;

            //    stopWatch.Start();
            //    string input = File.ReadAllText(file);
            //    string[] words = new Regex("\\s+|,\\s*|\\.\\s*").Split(input);
            //    string key = "";
            //    int n = 2; //bi
            //    for (int i = 0; i < words.Length - n + 1; i++)
            //    {
            //        key = generateEachClusterNgram(words, i, i + n);
            //        key = key.ToLower();
            //        string[] sylls = key.Split(' ');
            //        if (sylls[0].Length > 0 && sylls[1].Length > 0)
            //            if (biAmount.ContainsKey(key))
            //                biAmount[key] += 1;
            //            else
            //            {
            //                biAmount.Add(key, amount);
            //            }
            //    }
            //    stopWatch.Stop();
            //    TimeSpan ts = stopWatch.Elapsed;
            //    string elapseTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //                        ts.Hours, ts.Minutes, ts.Seconds,
            //                        ts.Milliseconds / 10);
            //    Console.WriteLine(string.Format("Creating bigram: {0}/{1}--------{2}", count++, getFile.Length, elapseTime));
            //}
            ////tổ chức lưu trữ

            //string output = "";
            //count = 1;
            //foreach (KeyValuePair<string, int> temp in biAmount)
            //{
            //    count++;
            //    string[] sylls = temp.Key.Split(' ');
            //    int firstSyllIndex = _uniPos[sylls[0]];
            //    int secondSyllIndex = _uniPos[sylls[1]];
            //    string index = toInt32(firstSyllIndex, secondSyllIndex);
            //    output += convertBinToDec(index) + "-" + temp.Value + "\n";
            //    if (count % 1000 == 0 || count == biAmount.Count)
            //        Console.WriteLine(string.Format("Converting bigram: {0}/{1}", count, biAmount.Count));
            //}

            //File.WriteAllText(@"Resources\bi.txt", output);
        }
        //đọc file bigram
        //chuyển dec sang int32, được 32bit
        //chuyển 16 bit đầu, và 16 bit sau lần lượt sang int16 dạng hec ---->1200, 1245
        public void readBiAmount(string path)
        {
            List<string> biList = Properties.Resources.filteredBi.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string[] biArr;
            if (biList.Count == 1)
            {
                biArr = biList[0].Split('\n');
                biList = biArr.ToList();
            }
            foreach (string line in biList)
            {
                if (line.Length == 0)
                    continue;
                try {
                    string[] bi = line.Split(' ');

                    string firstSyll = bi[0];
                    string secondSyll = bi[1];
                    _biAmount.Add(firstSyll + " " + secondSyll, Int32.Parse(bi[2]));
                }
                catch
                {

                }
            }
        }
        #endregion


        #region TriGram
        /// <summary>
        /// 
        /// </summary>
        public void generateTrigram()
        {
            ////Dictionary<string, int> biGram = new Dictionary<string, int>();
            //string folderPath = @"E:\Google Drive\Document\luan van\ngram\input\";
            //int count = 1;// da chay toi 5
            //Stopwatch stopWatch = new Stopwatch();
            //string[] getFile = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
            //Dictionary<string, int> triAmount = new Dictionary<string, int>();
            //int amount = 1;
            //foreach (string file in getFile)
            //{
            //    //System.IO.File.Move(file, @"C:\Users\Kiet\OneDrive\Thesis\Filtered\" + ++count + ".txt");

            //    //continue;

            //    stopWatch.Start();
            //    string input = File.ReadAllText(file);
            //    string[] words = new Regex("\\s+|,\\s*|\\.\\s*").Split(input);
            //    string key = "";
            //    int n = 3; //tri
            //    for (int i = 0; i < words.Length - n + 1; i++)
            //    {
            //        key = generateEachClusterNgram(words, i, i + n);
            //        key = key.ToLower();
            //        string[] sylls = key.Split(' ');
            //        if (sylls[0].Length > 0 && sylls[1].Length > 0 && sylls[2].Length > 0)
            //            if (triAmount.ContainsKey(key))
            //                triAmount[key] += 1;
            //            else
            //            {
            //                triAmount.Add(key, amount);
            //            }
            //    }
            //    stopWatch.Stop();
            //    TimeSpan ts = stopWatch.Elapsed;
            //    string elapseTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //                        ts.Hours, ts.Minutes, ts.Seconds,
            //                        ts.Milliseconds / 10);
            //    Console.WriteLine(string.Format("Creatting trigram: {0}/{1}--------{2}", count++, getFile.Length, elapseTime));
            //}
            ////tổ chức lưu trữ

            //string output = "";
            //count = 1;
            //foreach (KeyValuePair<string, int> temp in triAmount)
            //{
            //    count++;
            //    string[] sylls = temp.Key.Split(' ');
            //    int firstSyllIndex = _uniPos[sylls[0]];
            //    int secondSyllIndex = _uniPos[sylls[1]];
            //    ushort lastSyllIndex = (ushort)_uniPos[sylls[2]];
            //    //Console.WriteLine(toInt32(firstSyllIndex, secondSyllIndex));
            //    ////Console.WriteLine(Int32.Parse(toInt16(amtiet1)));
            //    //Console.WriteLine(getFirstSyllableIndex(toInt32(firstSyllIndex, secondSyllIndex)));
            //    //Console.WriteLine(getSecondSyllableIndex(toInt32(firstSyllIndex, secondSyllIndex)));
            //    string index = toInt32(firstSyllIndex, secondSyllIndex);
            //    output += convertBinToDec(index) + "-" + lastSyllIndex + "-" + temp.Value + "\n";
            //    if (count % 1000 == 0 || count == triAmount.Count)
            //        Console.WriteLine(string.Format("Converting trigram: {0}/{1}", count, triAmount.Count));
            //}

            //File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\Trigram\tri.txt", output);
        }

        public void readTriAmount(string path)
        {
            //string[] triGram = File.ReadAllLines(path);
            //foreach (string line in triGram)
            //{
            //    string[] tri = line.Split(' ');
            //    string firstSyll = _posUni[Int32.Parse(tri[0])];
            //    string secondSyll = _posUni[Int32.Parse(tri[1])];
            //    string lastSyll = _posUni[Int32.Parse(tri[2])];
            //    _triAmount.Add(firstSyll + " " + secondSyll + " " + lastSyll, Int32.Parse(tri[3]));
            //}
        }

        #endregion

        /// <summary>
        /// đọc từ file dna.txt trong resources và gán giá trị vào Text
        /// </summary>
        private void purifyFileCorpus()
        {

            string folderPath = @"C:\Users\Kiet\OneDrive\Thesis\Ngram\Input\";
            int count = 3;// da chay toi 3
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