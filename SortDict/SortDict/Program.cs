using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SortDict
{
    class Program
    {
        public const int MAX = 49124;

        public List<string> readCompoundWordDict()
        {
            List<string> result = new List<string>(); try
            {
                string[] tempDict = File.ReadAllLines(@"Resources\compoundWordDict.txt");
                foreach (string i in tempDict)
                {
                    result.Add(i.ToLower());
                }
            }
            catch
            {
                Console.WriteLine("khong doc duoc file");
            }
            return result;
        }

        public static string[] readCompoundWordDict(string path)
        {
            string[] tempDict = new string[MAX];
            try
            {
                tempDict = File.ReadAllLines(path);
                for (int i = 0; i < tempDict.Length; i++)
                {
                    tempDict[i] = tempDict[i].ToLower();
                }
            }
            catch
            {
                Console.WriteLine("khong doc duoc file");
            }
            return tempDict;
        }

        static void Main(string[] args)
        {
            //List<string> lst = new List<string>();
            //HashSet<string> hSet = new HashSet<string>();
            string[] tempDict = readCompoundWordDict(@"Resources\compoundWordDict.txt");
            string[] dict = new string[MAX];
            Dictionary<char, string> codeVN = new Dictionary<char, string>();
            codeVN.Add(' ', "00");
            codeVN.Add('a', "01");
            codeVN.Add('á', "02");
            codeVN.Add('à', "03");
            codeVN.Add('ả', "04");
            codeVN.Add('ã', "05");
            codeVN.Add('ạ', "06");
            codeVN.Add('ă', "07");
            codeVN.Add('ắ', "08");
            codeVN.Add('ằ', "09");
            codeVN.Add('ẳ', "10");
            codeVN.Add('ẵ', "11");
            codeVN.Add('ặ', "12");
            codeVN.Add('â', "13");
            codeVN.Add('ấ', "14");
            codeVN.Add('ầ', "15");
            codeVN.Add('ẩ', "16");
            codeVN.Add('ẫ', "17");
            codeVN.Add('ậ', "18");
            codeVN.Add('b', "19");
            codeVN.Add('c', "20");
            codeVN.Add('d', "21");
            codeVN.Add('đ', "22");
            codeVN.Add('e', "23");
            codeVN.Add('é', "24");
            codeVN.Add('è', "25");
            codeVN.Add('ẻ', "26");
            codeVN.Add('ẽ', "27");
            codeVN.Add('ẹ', "28");
            codeVN.Add('ê', "29");
            codeVN.Add('ế', "30");
            codeVN.Add('ề', "31");
            codeVN.Add('ể', "32");
            codeVN.Add('ễ', "33");
            codeVN.Add('ệ', "34");
            codeVN.Add('f', "35");
            codeVN.Add('g', "36");
            codeVN.Add('h', "37");
            codeVN.Add('i', "38");
            codeVN.Add('í', "39");
            codeVN.Add('ì', "40");
            codeVN.Add('ỉ', "41");
            codeVN.Add('ĩ', "42");
            codeVN.Add('ị', "43");
            codeVN.Add('j', "44");
            codeVN.Add('k', "45");
            codeVN.Add('l', "46");
            codeVN.Add('m', "47");
            codeVN.Add('n', "48");
            codeVN.Add('o', "49");
            codeVN.Add('ó', "50");
            codeVN.Add('ò', "51");
            codeVN.Add('ỏ', "52");
            codeVN.Add('õ', "53");
            codeVN.Add('ọ', "54");
            codeVN.Add('ô', "55");
            codeVN.Add('ố', "56");
            codeVN.Add('ồ', "57");
            codeVN.Add('ổ', "58");
            codeVN.Add('ỗ', "59");
            codeVN.Add('ộ', "60");
            codeVN.Add('ơ', "61");
            codeVN.Add('ớ', "62");
            codeVN.Add('ờ', "63");
            codeVN.Add('ở', "64");
            codeVN.Add('ỡ', "65");
            codeVN.Add('ợ', "66");
            codeVN.Add('p', "67");
            codeVN.Add('q', "68");
            codeVN.Add('r', "69");
            codeVN.Add('s', "70");
            codeVN.Add('t', "71");
            codeVN.Add('u', "72");
            codeVN.Add('ú', "73");
            codeVN.Add('ù', "74");
            codeVN.Add('ủ', "75");
            codeVN.Add('ũ', "76");
            codeVN.Add('ụ', "77");
            codeVN.Add('ư', "78");
            codeVN.Add('ứ', "79");
            codeVN.Add('ừ', "80");
            codeVN.Add('ử', "81");
            codeVN.Add('ữ', "82");
            codeVN.Add('ự', "83");
            codeVN.Add('v', "84");
            codeVN.Add('x', "85");
            codeVN.Add('y', "86");
            codeVN.Add('ý', "87");
            codeVN.Add('ỳ', "88");
            codeVN.Add('ỷ', "89");
            codeVN.Add('ỹ', "90");
            codeVN.Add('ỵ', "91");
            codeVN.Add('z', "92");


            int j = 0;
            foreach (string i in tempDict)
                dict[j++] = generator(i, codeVN);

            HeapSort hs = new HeapSort();
            string[] numberArr = hs.PerformHeapSort(dict);
            string[] result = new string[numberArr.Length];
            for (int i = 0; i < numberArr.Length; i++)
            {
                string tmp = "";
                for (int k = 0; k < numberArr[i].Length; k += 2)
                {
                    foreach (KeyValuePair<char, string> pair in codeVN)
                        if (pair.Value.Equals(numberArr[i].Substring(k, 2)))
                        {
                            tmp += pair.Key;
                                break;
                        }
                }
                result[i] = tmp;
            }
            File.WriteAllLines(@"D:\Study\Thesis\newCompoundDict.txt", result);
        }
        public static string generator(string input, Dictionary<char, string> codeVN)
        {
            string result = "";
            char[] b = input.ToLower().ToCharArray();
            for (int i = 0; i < b.Length; i++)
            {
                try
                {
                    result += codeVN[b[i]];
                }
                catch (KeyNotFoundException)
                {
                    //File.WriteAllText(@"D:\Study\Thesis\a.txt", b[i] + "");
                }
            }
            return result;
        }
        //File.WriteAllLines(@"D:\Study\Thesis\newCompoundWordDict.txt", tempDict);
        //Console.ReadLine();

        //Console.WriteLine((hs.IsLessThan("nắn", "náng")));
    }

    class HeapSort
    {
        private int heapSize;

        private void BuildHeap(string[] arr)
        {
            heapSize = arr.Length - 1;
            for (int i = heapSize / 2; i >= 0; i--)
            {
                Heapify(arr, i);
            }
        }

        private void Swap(string[] arr, int x, int y)//function to swap elements
        {
            string temp = arr[x];
            arr[x] = arr[y];
            arr[y] = temp;
        }
        private void Heapify(string[] arr, int index)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int largest = index;
            if (left <= heapSize && IsGreaterThan(arr[left], arr[index]))
            {
                largest = left;
            }

            if (right <= heapSize && IsGreaterThan(arr[right], arr[largest]))
            {
                largest = right;
            }

            if (largest != index)
            {
                Swap(arr, index, largest);
                Heapify(arr, largest);
            }
        }
        public string[] PerformHeapSort(string[] arr)
        {
            BuildHeap(arr);
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                Swap(arr, 0, i);
                heapSize--;
                Heapify(arr, 0);
            }
            DisplayArray(arr);
            return arr;
        }
        public bool IsLessThan(string a, string b)
        {
            return Int32.Parse(a) < Int32.Parse(b);
        }

        public bool IsGreaterThan(string a, string b)
        {

            if (a.Length < b.Length)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (Int32.Parse(a[i] + "") < Int32.Parse(b[i] + ""))
                        return false;
                    else if (Int32.Parse(a[i] + "") > Int32.Parse(b[i] + ""))
                        return true;
                }
                return false;
            }
            else
            {
                for (int i = 0; i < b.Length; i++)
                {
                    if (Int32.Parse(a[i] + "") < Int32.Parse(b[i] + ""))
                        return false;
                    else if (Int32.Parse(a[i] + "") > Int32.Parse(b[i] + ""))
                        return true;
                }
                return true;
            }
        }
        private void DisplayArray(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            { Console.Write("[{0}]", arr[i]); }
        }
    }
}
