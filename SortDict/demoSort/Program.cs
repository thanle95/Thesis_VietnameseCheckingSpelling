using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoSort
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<char, string> codeVN = new Dictionary<char, string>();

            codeVN.Add('a', "00"); codeVN.Add('á', "01");
            codeVN.Add('à', "02"); codeVN.Add('ả', "03");
            codeVN.Add('ã', "04"); codeVN.Add('ạ', "05");
            codeVN.Add('ă', "06"); codeVN.Add('ắ', "07");
            codeVN.Add('ằ', "08"); codeVN.Add('ẳ', "09");
            codeVN.Add('ẵ', "10"); codeVN.Add('ặ', "11");
            codeVN.Add('â', "12"); codeVN.Add('ấ', "13");
            codeVN.Add('ầ', "14"); codeVN.Add('ẩ', "15");
            codeVN.Add('ẫ', "16"); codeVN.Add('ậ', "17");
            codeVN.Add('b', "18"); codeVN.Add('c', "19");
            codeVN.Add('d', "20"); codeVN.Add('đ', "21");
            codeVN.Add('e', "22"); codeVN.Add('é', "23");
            codeVN.Add('è', "24"); codeVN.Add('ẻ', "25");
            codeVN.Add('ẽ', "26"); codeVN.Add('ẹ', "27");
            codeVN.Add('ê', "28"); codeVN.Add('ế', "29");
            codeVN.Add('ề', "30"); codeVN.Add('ể', "31");
            codeVN.Add('ễ', "32"); codeVN.Add('ệ', "33");
            codeVN.Add('f', "34"); codeVN.Add('g', "35");
            codeVN.Add('h', "36"); codeVN.Add('i', "37");
            codeVN.Add('í', "38"); codeVN.Add('ì', "39");
            codeVN.Add('ỉ', "40"); codeVN.Add('ĩ', "41");
            codeVN.Add('ị', "42"); codeVN.Add('j', "43");
            codeVN.Add('k', "44"); codeVN.Add('l', "45");
            codeVN.Add('m', "46"); codeVN.Add('n', "47");
            codeVN.Add('o', "48"); codeVN.Add('ó', "49");
            codeVN.Add('ò', "50"); codeVN.Add('ỏ', "51");
            codeVN.Add('õ', "52"); codeVN.Add('ọ', "53");
            codeVN.Add('ô', "54"); codeVN.Add('ố', "55");
            codeVN.Add('ồ', "56"); codeVN.Add('ổ', "57");
            codeVN.Add('ỗ', "58"); codeVN.Add('ộ', "59");
            codeVN.Add('ơ', "60"); codeVN.Add('ớ', "61");
            codeVN.Add('ờ', "62"); codeVN.Add('ở', "63");
            codeVN.Add('ỡ', "64"); codeVN.Add('ợ', "65");
            codeVN.Add('p', "66"); codeVN.Add('q', "67");
            codeVN.Add('r', "68"); codeVN.Add('s', "69");
            codeVN.Add('t', "70"); codeVN.Add('u', "71");
            codeVN.Add('ú', "72"); codeVN.Add('ù', "73");
            codeVN.Add('ủ', "74"); codeVN.Add('ũ', "75");
            codeVN.Add('ụ', "76");
            codeVN.Add('ư', "77"); codeVN.Add('ứ', "78");
            codeVN.Add('ừ', "79"); codeVN.Add('ử', "80");
            codeVN.Add('ữ', "81"); codeVN.Add('ự', "82");
            codeVN.Add('v', "83"); codeVN.Add('x', "84");
            codeVN.Add('y', "85"); codeVN.Add('z', "86");
            codeVN.Add(' ', "87");


            char a = '4';
            Console.WriteLine(Int32.Parse(a + ""));

        }
        public static string generator(string input, Dictionary<char, string> codeVN)
        {
            string result = "";
            char[] b = input.ToLower().ToCharArray();
            for (int i = 0; i < b.Length; i++)
            {
                result += codeVN[b[i]];
            }
            return result;
        }

    }
}