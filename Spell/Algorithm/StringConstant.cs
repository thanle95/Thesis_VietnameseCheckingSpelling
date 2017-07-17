using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spell.Algorithm
{
    public class StringConstant
    {
        private static StringConstant instance = new StringConstant();
        public static StringConstant Instance
        {
            get
            {
                return instance;
            }

        }
        private StringConstant()
        {

        }
        public const int MAX_KEYBOARD_ROW = 3;
        public const int MAX_KEYBOARD_COL = 10;
        public const int MAXGROUP_REGION_CONFUSED = 6;
        public const int MAXCASE_REGION_CONFUSED = 4;
        public const int QUESTION_MASK = 2;
        public const int TILDE = 3;
        public const int MAX_VOWEL_NO = 12;
        public const int MAX_SIGN_NO = 5;
        public const int MAXGROUP_VNCHARMATRIX = 6;
        public const int MAXCASE_VNCHARMATRIX= 3;
        public char[,] KeyBoardMatrix_LowerCase = new char[MAX_KEYBOARD_ROW, MAX_KEYBOARD_COL]  {
            {'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p' },
            {'a', 's', 'd', 'f', 'g', 'h', 'j', 'k' ,'l', ';' },
            {'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.', '/' }
        };

        public char[,] KeyBoardMatrix_UperCase = new char[MAX_KEYBOARD_ROW, MAX_KEYBOARD_COL]  {
            {'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P' },
            {'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K' ,'L', ';' },
            {'Z', 'X', 'C', 'V', 'B', 'N', 'M', ',', '.', '/' }
        };

        public char[] VNAlphabetArr_LowerCase = new char[]
        {
            'a', 'ă', 'â', 'b', 'c', 'd', 'đ', 'e', 'ê', 'g', 'h', 'i',
            'k', 'l', 'm', 'n', 'o','ô', 'ơ', 'p','q', 'r', 's', 't',
            'u','ư', 'v', 'x', 'y'
        };

        public char[] VNAlphabetArr_UpperCase = new char[]
        {
            'A', 'Ă', 'Â', 'B', 'C', 'D', 'Đ', 'E', 'Ê', 'G', 'H', 'I',
            'K', 'L', 'M', 'N', 'O','Ô', 'Ơ', 'P','Q', 'R', 'S', 'T',
            'U','Ư', 'V', 'X', 'Y'
        };

        // nguyên âm
        public char[] VNVowelArr_LowerCase = new char[]
        {
            'a', 'ă', 'â', 'e', 'ê',  'o','ô', 'ơ', 'i','u','ư', 'y'
        };

        public char[] VNVowelArr_UpperCase = new char[]
        {
            'A', 'Ă', 'Â', 'E', 'Ê', 'O','Ô', 'Ơ', 'I', 'U','Ư', 'Y'
        };

        public char[] VNVowelWithSignArr_LowerCase = new char[]
        {
            'á', 'ắ', 'ấ', 'é', 'ế',
            'ó','ố', 'ớ', 'í',
            'ú','ứ', 'ý', // thanh sắc
            'à', 'ằ', 'ầ', 'è', 'ề',
            'ò','ồ', 'ờ','ì',
            'ù','ừ', 'ỳ', // thanh huyền
            'ả', 'ẳ', 'ẩ', 'ẻ', 'ể',
            'ỏ','ổ', 'ở','ỉ',
            'ủ','ử', 'ỷ', // thanh hỏi
            'ã', 'ẵ', 'ẫ', 'ẽ', 'ễ',
            'õ','ỗ', 'ỡ','ĩ',
            'ũ','ữ', 'ỹ', // thanh ngã
            'ạ', 'ặ', 'ậ', 'ẹ', 'ệ',
            'ọ','ộ', 'ợ','ị',
            'ụ','ự', 'ỵ' // thanh nặng
        };
        public string source = "àảãáạằẳẵắặầẩẫấậèẻẽéẹềểễếệìỉĩíịòỏõóọồổỗốộờởỡớợùủũúụừửữứựỳỷỹýỵ";

        public string dest = "aaaaaăăăăăâââââeeeeeêêêêêiiiiioooooôôôôôơơơơơuuuuuưưưưưyyyyy";
        public string VNSign = "frxsj";
        public char[] VNVowelWithSignArr_UpperCase = new char[]
        {
            'Á', 'Ắ', 'Ấ', 'É', 'Ế','Ó','Ố', 'Ớ', 'Í','Ú','Ứ', 'Ý', // thanh sắc
            'À', 'Ằ', 'Ầ', 'È', 'Ề', 'Ò','Ồ', 'Ờ','Ì','Ù','Ừ', 'Ỳ', // thanh huyền
            'Ả', 'Ẳ', 'Ẩ', 'Ẻ', 'Ể','Ỏ','Ổ', 'Ở', 'Ỉ','Ủ','Ử', 'Ỷ', // thanh hỏi
            'Ã', 'Ẵ', 'Ẫ', 'Ẽ', 'Ễ','Õ','Ỗ', 'Ỡ', 'Ĩ','Ũ','Ữ', 'Ỹ', // thanh ngã
            'Ạ', 'Ặ', 'Ậ', 'Ẹ', 'Ệ','Ọ','Ộ', 'Ợ', 'Ị','Ụ','Ự', 'Ỵ' // thanh nặng
        };

        public char[,] VNVowelWithSignMatrix_LowerCase = new char[MAX_SIGN_NO, MAX_VOWEL_NO]
        {
           { 'á', 'ắ', 'ấ', 'é', 'ế','ó','ố', 'ớ', 'í','ú','ứ', 'ý'}, // thanh sắc
            { 'à', 'ằ', 'ầ', 'è', 'ề','ò','ồ', 'ờ', 'ì','ù','ừ', 'ỳ' }, // thanh huyền
            { 'ả', 'ẳ', 'ẩ', 'ẻ', 'ể', 'ỏ','ổ', 'ở', 'ỉ', 'ủ','ử', 'ỷ' }, // thanh hỏi
            { 'ã', 'ẵ', 'ẫ', 'ẽ', 'ễ', 'õ','ỗ', 'ỡ', 'ĩ','ũ','ữ', 'ỹ' }, // thanh ngã
            { 'ạ', 'ặ', 'ậ', 'ẹ', 'ệ',  'ọ','ộ', 'ợ','ị', 'ụ','ự', 'ỵ' } // thanh nặng
        };

        public char[,] VNVowelWithSignMatrix_UpperCase = new char[MAX_SIGN_NO, MAX_VOWEL_NO]
        {
            { 'Á', 'Ắ', 'Ấ', 'É', 'Ế','Ó','Ố', 'Ớ', 'Í','Ú','Ứ', 'Ý'}, // thanh sắc
            { 'À', 'Ằ', 'Ầ', 'È', 'Ề','Ò','Ồ', 'Ờ', 'Ì','Ù','Ừ', 'Ỳ' }, // thanh huyền
            { 'Ả', 'Ẳ', 'Ẩ', 'Ẻ', 'Ể','Ỏ','Ổ', 'Ở', 'Ỉ','Ủ','Ử', 'Ỷ' }, // thanh hỏi
            { 'Ã', 'Ẵ', 'Ẫ', 'Ẽ', 'Ễ','Õ','Ỗ', 'Ỡ', 'Ĩ','Ũ','Ữ', 'Ỹ' }, // thanh ngã
            { 'Ạ', 'Ặ', 'Ậ', 'Ẹ', 'Ệ', 'Ọ','Ộ', 'Ợ','Ị','Ụ','Ự', 'Ỵ' } // thanh nặng
        };


        //nguyên âm mang dấu phụ
        public char[] subVNVowelArr_LowerCase = new char[]
        {
           'ă', 'â', 'ê','ô', 'ơ','ư'
        };
        public char[,] vnCharacterMatrix = new char[,]
        {
            {'d','đ', ' '},
            {'e','ê', ' '},
            {'a','ă','â'},
            {'o','ô', 'ơ'},
            {'u','ư', ' '},
            {'ư', 'â', ' ' }
        };
        public string vnCharacter = "ăâêôơưđ";
        public string[] vnCharacter_Telex = new string[]
        {
            "aw", "aa", "ee", "oo", "ow", "uw", "dd"
        };
        public char[] subVNVowelArr_UpperCase = new char[]
        {
           'Ă', 'Â', 'Ê','Ô', 'Ơ','Ư'
        };

        // phụ âm
        public string[] VNConsonantArr_LowerCase = new string[]
        {
            "ngh","ng", "nh","ph", "th", "tr", "kh","b", "ch" ,"c","d", "đ","gi", "gh", "g",  "h", "k",
            "l", "m", "n" ,  "qu", "r", "s"
           , "t", "v", "x"
        };

        public string[] VNConsonantArr_UpperCase = new string[]
        {
            "NGH","NG", "NH","PH", "TH", "TR", "KH","B", "CH" ,"C","D", "Đ","GI", "GH", "G", "H", "K",
            "L", "M", "N" ,  "QU", "R", "S"
           , "T", "V", "X"
        };

        public string[,] VNRegion_Confused_Matrix_LowerCase = new string[,]
        {
            
            {"i", "y", "",""},
            {"s", "x", "",""},
            {"l", "n", "",""},
            {"n", "ng", "nh",""},
            {"d", "gi" ,"v","r"},
            {"ch", "tr", "",""},
            {"t", "c", "", "" }
        };

        public string[,] VNRegion_Confused_Matrix_UperCase = new string[,]
        {
            {"S", "X", "",""},
            {"L", "N", "",""},
            {"V","R", "D", "GI" },
            {"CH", "TR", "",""}
        };

        public string patternMiddleSymbol = "[-|/|\\|>|<|\\[|\\]|,|\"|(|)|“|”]";
        public string patternEndMiddleSymbol = "[”|,|)]";
        public string patternEndSentenceCharacter = "[.!?;:…]";

        public string patternCheckSpecialChar = "[-|\\/|\\|>|<|\\[|\\]|\"|(|)|“|”|@|#|$|%|^|&|\\*|\\d|\\W]";
        public string patternHasWord = "[\\w]";
        public string[] VNAcronym = new string[]
        {
            "XHCN", "CNXH", "ĐCS", "CHXHCNVN", "MTDTGPMNVN", "QDND","QLVNCH","VNQDĐ","VNQDD","VNCH","VNDCCH",
            "ĐH","TS", "PGS", "CLB"
        };
        public string patternSignSentence = "[-|/|\\|>|<|\\[|\\]|,|\"|(|)|“|”|.!?;:…]";
        public string patternCheckWord = "[-|\\/|\\|>|<|\\[|\\]|,|\"|(|)|“|”|@|#|$|%|^|&|\\*|\\d]";

        public string patternSPEC = "[-|\\/|\\|>|<|\\[|\\]||@|#|$|%|^|&|\\*|\\d|]";
        public string patternOPEN = "[(|“]";
        public string patternCLOSE = "[,|)|”]";
    }
}
