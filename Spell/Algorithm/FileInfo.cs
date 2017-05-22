using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spell.Algorithm
{
    class FileInfo
    {
        private static FileInfo instance = new FileInfo();
        private string Directory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        public string SyllDict
        {
            get
            {
                return Directory + @"Resources\SyllableDictByViet39K.txt";
            }
        }
        public string CompoundWordDict
        {
            get
            {
                return Directory + @"Resources\sortedCompoundWordDict.txt";
            }
        }
        public string CompoundWordLinkedList
        {
            get
            {
                return Directory + @"Resources\newCompoundWordByViet39K.txt";
            }
        }
        public string UniGram
        {
            get
            {
                return Directory + @"Resources\filteredUni.txt";
            }
        }
        public string BiGram
        {
            get
            {
                return Directory + @"Resources\filteredBi.txt";
            }
        }
        public string RightWordScore
        {
            get
            {
                return Directory + @"Resources\rightWord.txt";
            }
        }
        public string RightWordCand
        {
            get
            {
                return Directory + @"Resources\rightWordCandidate.txt";
            }
        }
        public string WrongWord
        {
            get
            {
                return Directory + @"Resources\wrongWord.txt";

            }
        }
        private FileInfo()
        {

        }

        public static FileInfo Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
