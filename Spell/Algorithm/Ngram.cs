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

        //chứa tất cả những cặp (ngram, số lượng) phục vụ cho việc tính xác suất
        public Dictionary<string, int> DictionaryForNgram { get; set; }
        public HashSet<string> Dictionary { get; set; } //bộ từ điển, chứa những từ không trùng lắp
        public string Text { get; set; }//dùng để lưu corpus đọc từ file dna.txt

        public Dictionary<string, int> UniGramCount; //chứa tất cả xác suất của unigram
        public Dictionary<string, int> BiGramCount; //chứa tất cả xác suất của bigram
        public Dictionary<string, int> TriGramCount; //chứa tất cả xác suất của trigram
        private int _sumUni = 0, sumBi = 0, sumTri = 0;

        private Dictionary<string, int> _uniPos = new Dictionary<string, int>(); //Chưa key và position của unigram
        private Dictionary<int, string> _posUni = new Dictionary<int, string>(); //Chưa key và position của unigram
        private Dictionary<string, int> _uniAmount = new Dictionary<string, int>(); // Chứa Key và giá trị tần số của unigram
        public Dictionary<string, int> _biAmount { get; set; } // Chứa Key và giá trị tần số của bigram
        public Dictionary<string, int> _triAmount { get; set; } // Chứa Key và giá trị tần số của trigram

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
            this._biAmount = new Dictionary<string, int>();
            this._triAmount = new Dictionary<string, int>();
            runFirst();
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
            //readFileCorpus();
            //generateUnigram();
            //generateBigram();
            //generateTrigram();

            //countingNgams();


            //generateProbabilitySet();
            //writeFileProbability();
            //generateUnigram();
            string uniPath = @"E:\Google Drive\Document\luan van\source\github\Thesis_VietnameseCheckingSpelling\Spell\Resources\uni.txt";
            string biPath = @"E:\Google Drive\Document\luan van\source\github\Thesis_VietnameseCheckingSpelling\Spell\Resources\bi.txt";
            readUni(uniPath);


            readBiAmount(biPath);
            //readTriAmount(triPath);
            //generateBigram();
            //generateTrigram();

            //readUniBySQL();
            //readBiBySQL();
        }

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
        public double calBiNgram(string w1, string w2)
        {
            int MAX = 10;
            string key = w1 + " " + w2;
            int Cw1 = MAX;
            int Cw1w2 = 0;
            if (_uniAmount.ContainsKey(w1.ToLower()))
                Cw1 = _uniAmount[w1.ToLower()];
            if (_biAmount.ContainsKey(key.ToLower()))
                Cw1w2 = _biAmount[key.ToLower()];
            double ret = (double)(Cw1w2 + 1) / (Cw1 + _sumUni);
            return ret;
        }

        public double calTriNgram(string w1, string w2, string w3)
        {
            int MAX = 10;
            string key = w1 + " " + w2 + " " + w3;
            string bi = w1 + " " + w2;
            int Cw1w2 = MAX;
            int Cw1w2w3 = 0;
            if (_biAmount.ContainsKey(bi.ToLower()))
                Cw1w2 = _biAmount[bi.ToLower()];
            if (_triAmount.ContainsKey(key.ToLower()))
                Cw1w2w3 = _triAmount[key.ToLower()];
            return (double)(Cw1w2w3 + 1) / (Cw1w2 + sumBi);
        }


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
                sumBi += temp.Value;
            }
        }
        /// <summary>
        /// khơi tạo bộ ngram với tham số n và str
        /// </summary>
        /// <param name="n">loại ngram muốn tạo</param>
        /// <param name="str">bộ ngữ liệu để tạo ngram</param>
        private void generateNgramSet(int n, string str)
        {
            string[] words = new Regex("\\s+|,\\s*|\\.\\s*").Split(str);
            string key = "";
            for (int i = 0; i < words.Length - n + 1; i++)
            {
                key = generateEachClusterNgram(words, i, i + n);
                key = key.ToLower();
                if (this.DictionaryForNgram.ContainsKey(key))
                    this.DictionaryForNgram[key] += 1;
                else this.DictionaryForNgram.Add(key, 1);
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
            }
            string output = "";
            foreach (KeyValuePair<string, int> temp in uniPos)
                output += temp.Key + "-" + temp.Value + "-" + uniAmount[temp.Key] + "\n";
            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\UniNgram\uni.txt", output);
        }

        /// <summary>
        /// Đọc dữ liệu của unigram: key và value (vị trí).
        /// </summary>
        /// <param name="path">Đường dẫn tới thư mục</param>
        public void readUni(string path)
        {
            string[] uniGram = File.ReadAllLines(path);
            foreach (string line in uniGram)
            {
                string[] uni = line.Split(' ');
                _uniPos.Add(uni[0], Int32.Parse(uni[1]));
                _posUni.Add(Int32.Parse(uni[1]), uni[0]);
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

            string output = "";
            count = 1;
            foreach (KeyValuePair<string, int> temp in biAmount)
            {
                count++;
                string[] sylls = temp.Key.Split(' ');
                int firstSyllIndex = _uniPos[sylls[0]];
                int secondSyllIndex = _uniPos[sylls[1]];
                string index = toInt32(firstSyllIndex, secondSyllIndex);
                output += convertBinToDec(index) + "-" + temp.Value + "\n";
                if (count % 1000 == 0 || count == biAmount.Count)
                    Console.WriteLine(string.Format("Converting bigram: {0}/{1}", count, biAmount.Count));
            }

            File.WriteAllText(@"Resources\bi.txt", output);
        }
        //đọc file bigram
        //chuyển dec sang int32, được 32bit
        //chuyển 16 bit đầu, và 16 bit sau lần lượt sang int16 dạng hec ---->1200, 1245
        public void readBiAmount(string path)
        {
            string[] biGram = File.ReadAllLines(path);
            foreach (string line in biGram)
            {
                string[] biParts = line.Split('_');
                string[] sylls = biParts[0].Split(' ');
                
                string firstSyll = _posUni[Int16.Parse( sylls[0])];
                string secondSyll = _posUni[Int16.Parse(sylls[1])];
                _biAmount.Add(firstSyll + " " + secondSyll, Int32.Parse(biParts[1]));
            }
        }
        #endregion


        #region TriGram
        /// <summary>
        /// 
        /// </summary>
        public void generateTrigram()
        {
            //Dictionary<string, int> biGram = new Dictionary<string, int>();
            string folderPath = @"E:\Google Drive\Document\luan van\ngram\input\";
            int count = 1;// da chay toi 5
            Stopwatch stopWatch = new Stopwatch();
            string[] getFile = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
            Dictionary<string, int> triAmount = new Dictionary<string, int>();
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
                        if (triAmount.ContainsKey(key))
                            triAmount[key] += 1;
                        else
                        {
                            triAmount.Add(key, amount);
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

            string output = "";
            count = 1;
            foreach (KeyValuePair<string, int> temp in triAmount)
            {
                count++;
                string[] sylls = temp.Key.Split(' ');
                int firstSyllIndex = _uniPos[sylls[0]];
                int secondSyllIndex = _uniPos[sylls[1]];
                ushort lastSyllIndex = (ushort)_uniPos[sylls[2]];
                //Console.WriteLine(toInt32(firstSyllIndex, secondSyllIndex));
                ////Console.WriteLine(Int32.Parse(toInt16(amtiet1)));
                //Console.WriteLine(getFirstSyllableIndex(toInt32(firstSyllIndex, secondSyllIndex)));
                //Console.WriteLine(getSecondSyllableIndex(toInt32(firstSyllIndex, secondSyllIndex)));
                string index = toInt32(firstSyllIndex, secondSyllIndex);
                output += convertBinToDec(index) + "-" + lastSyllIndex + "-" + temp.Value + "\n";
                if (count % 1000 == 0 || count == triAmount.Count)
                    Console.WriteLine(string.Format("Converting trigram: {0}/{1}", count, triAmount.Count));
            }

            File.WriteAllText(@"E:\Google Drive\Document\luan van\ngram\Trigram\tri.txt", output);
        }

        public void readTriAmount(string path)
        {
            string[] triGram = File.ReadAllLines(path);
            foreach (string line in triGram)
            {
                string[] tri = line.Split('-');
                string index = toInt32(tri[0]);
                string firstSyll = _posUni[getFirstSyllableIndex(index)];
                string secondSyll = _posUni[getSecondSyllableIndex(index)];
                string lastSyll = _posUni[ushort.Parse( tri[1])];
                _triAmount.Add(firstSyll + " " + secondSyll + " " + lastSyll, Int16.Parse(tri[2]));
            }
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
        /// tính xác suất cho unigram
        /// </summary>
        /// <param name="w">unigram cần tính</param>
        /// <returns></returns>
        private int calProbability(string w)
        {
            return calPartialProbability(this.DictionaryForNgram[w], 1);
        }
        /// <summary>
        /// tính xác suất cho bigram
        /// </summary>
        /// <param name="w1">gram thứ nhất</param>
        /// <param name="w2">gram thứ hai cần tính</param>
        /// <returns></returns>
        private int calProbability(string w1, string w2)
        {
            return calPartialProbability(this.DictionaryForNgram[w1 + " " + w2], 1);
        }
        /// <summary>
        /// tính xác suất cho trigram
        /// </summary>
        /// <param name="w1">gram thứ nhất</param>
        /// <param name="w2">gram thứ hai</param>
        /// <param name="w3">gram thứ ba cần tính</param>
        /// <returns></returns>
        private int calProbability(string w1, string w2, string w3)
        {
            return calPartialProbability(this.DictionaryForNgram[w1 + " " + w2 + " " + w3], 1);
        }
        /// <summary>
        /// Mô tả chi tiết việc tính xác suất theo yêu cầu
        /// </summary>
        /// <param name="item">ngram cần tính</param>
        /// <param name="n">loại gram cần tính</param>
        /// <returns></returns>
        private int calPartialProbability(Object item, int n)
        {
            if (item == null)
                return 0;
            int value = (int)item;
            int sum = 0;
            if (n == 1)
                sum = _sumUni;
            else if (n == 2)
                sum = sumBi;
            else if (n == 3)
                sum = sumTri;
            return (int)Math.Round(((double)value / (int)sum) * 100000) / 100000;
        }

        private void countingNgams()
        {
            foreach (KeyValuePair<string, int> pair in this.DictionaryForNgram)
            {
                string[] tmp = pair.Key.Split(' ');
                if (tmp.Length == 1)
                    UniGramCount.Add(pair.Key, pair.Value);
                else if (tmp.Length == 2)
                    BiGramCount.Add(pair.Key, pair.Value);
                else if (tmp.Length == 3)
                    TriGramCount.Add(pair.Key, pair.Value);
            }
        }
        /// <summary>
        /// tạo từng bộ xác suất
        /// </summary>
        private void generateProbabilitySet()
        {
            foreach (KeyValuePair<string, int> pair in this.DictionaryForNgram)
            {
                string[] tmp = pair.Key.Split(' ');
                if (tmp.Length == 1)
                    UniGramCount.Add(pair.Key, calProbability(pair.Key));
                else if (tmp.Length == 2)
                    BiGramCount.Add(pair.Key, calProbability(pair.Key));
                else if (tmp.Length == 3)
                    TriGramCount.Add(pair.Key, calProbability(pair.Key));
            }

            //for (int i = 0; i < tmp.Length; i++)
            //    if (!tmp[i].Equals(""))
            //        UniGramPro.Add(tmp[i], calProbability(tmp[i]));

            //for (int i = 0; i < tmp.Length - 1; i++)
            //    if (!tmp[i].Equals("") && !tmp[i + 1].Equals(""))
            //        BiGramPro.Add(tmp[i] + " " + tmp[i + 1],
            //                calProbability(tmp[i], tmp[i + 1]));

            //for (int i = 0; i < tmp.Length - 2; i++)
            //    if (!tmp[i].Equals("") && !tmp[i + 1].Equals("")
            //            && !tmp[i + 2].Equals(""))
            //        TriGramPro.Add(tmp[i] + " " + tmp[i + 1] + " " + tmp[i + 2],
            //                calProbability(tmp[i], tmp[i + 1], tmp[i + 2]));

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
            return Dictionary.Add(X);
        }
    }
}
