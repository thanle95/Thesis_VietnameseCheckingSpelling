using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spell.Algorithm
{
    public class CompoundWordVn
    {
        private CompoundWordVn()
        {
            readCompoundWordVNFile();
        }
        private void readCompoundWordVNFile()
        {
            compoundWordVnDict = new Dictionary<string, List<string>>();
            string path = @"C:\Users\Kiet\OneDrive\Thesis\Thesis\Thesis_VietnameseCheckingSpelling\Spell\Resources\newCompoundWordByViet39K.txt";
            string[] compoundDictArr = File.ReadAllLines(path);
            string key = "";
            List<string> value;
            foreach(string i in compoundDictArr)
            {
                string[] iArr = i.Split('@');
                key = iArr[0];
                string[] valueArr = iArr[1].Split('_');
                value = new List<string>();
                foreach(string iValueArr in valueArr)
                    value.Add(iValueArr);
                compoundWordVnDict.Add(key, value);
            }
        }
        private void createCompoundWordVNFile()
        {
            compoundWordVnDict = new Dictionary<string, List<string>>();
            string[] compoundDictArr = File.ReadAllLines("Resources\\newCompoundDict.txt");
            string key = "";
            List<string> value = new List<string>();
            foreach (string s in compoundDictArr)
            {
                string[] sArr = s.Split(' ');
                //đổi key
                if (!sArr[0].Equals(key))
                {
                    if (key.Length > 0)
                    {
                        compoundWordVnDict.Add(key, value);
                        value = new List<string>();
                    }
                    key = sArr[0];
                    value.Add(s.Substring(key.Length)); // thêm khoảng trắng
                }
                else
                {
                    //cùng key
                    value.Add(s.Substring(key.Length)); // thêm khoảng trắng
                }
            }
        }
        private static CompoundWordVn instance = new CompoundWordVn();
        public static CompoundWordVn Instance { get { return instance; } }
        public Dictionary<string, List<string>> compoundWordVnDict;
    }
}
