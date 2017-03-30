using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ngram
{
    public class VNDictionary
    {
        public List<string> SyllableDict;
        //public List<string> CompoundDict;

        private VNDictionary()
        {
            this.SyllableDict = new List<string>();
            this.SyllableDict = readSyllableDict();
            //this.CompoundDict = new List<string>();
            //this.CompoundDict = readCompoundWordDict();
        }

        private static VNDictionary instance = new VNDictionary();
        public static VNDictionary getInstance
        {
            get
            {
                return instance;
            }
        }


        /// <summary>
        /// Đọc từ điển tiếng âm tiết lên
        /// </summary>
        public List<string> readSyllableDict()
        {

            List<string> result = new List<string>();
            try
            {
                //properties vào fileName, chọn copy always
                string[] tempDict = File.ReadAllLines(@"Resources\SyllableDictByViet39K.txt");
                foreach (string i in tempDict)
                {
                    result.Add(i);
                }
            }
            catch (Exception e)
            {
            }
            return result;
        }
        /// <summary>
        /// đọc từ điển từ ghép tiếng Việt
        /// </summary>
        /// <returns></returns>
        
        public bool isSyllableVN(string token)
        {
            // dung giai thuat tim kiem am tiet 'token' trong tu dien
            // Neu co am tiet nay thi return true

            return this.SyllableDict.BinarySearch(token.ToLower()) >= 0; // Neu am tiet nay ko co trong tu dien 
        }
        /// <summary>
        /// Kiểm tra một cụm từ có là từ ghép hay không
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        //public bool isCompoundVN(string word)
        //{
        //    return CompoundWordVN.Instance..BinarySearch((word).ToLower()) >= 0;
        //}


        /// <summary>
        /// trả về từ ghép liền trước token dạng X X+1
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
      
      
    }
}
