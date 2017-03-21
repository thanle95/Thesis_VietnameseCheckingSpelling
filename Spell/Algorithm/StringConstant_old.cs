using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spell.Algorithm
{
    public class StringConstant_old
    {
        private StringConstant_old()
        {

        }
        private static StringConstant_old instance = new StringConstant_old();
        public static StringConstant_old Instance
        {
            get
            {
                return instance;
            }
        }

        public string patternSymbolstring = "[^\\w]";

        public string patternEndSentenceCharacter = "[.!?;:…]";

        public string patternMiddleSymbol = "[-|/|\\|>|<|\\[|\\]|,|\"|(|)|“|”]";

        public string patternUpperCharacter = "[A-YÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬÈẺẼÉẸÊỀỂỄẾỆÌỈĨÍỊÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢÙỦŨÚỤƯỪỬỮỨỰỲỶỸÝỴ]";

        public int _length = 60;

        public string fullCharacter = "[A-YÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬÈẺẼÉẸÊỀỂỄẾỆÌỈĨÍỊÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢÙỦŨÚỤƯỪỬỮỨỰỲỶỸÝỴabcdeghiklmnopqrstuvxyàảãáạăằẳẵắặâầẩẫấậđèẻẽéẹêềểễếệìỉĩíịòỏõóọôồổỗốộơờởỡớợùủũúụưừửữứựỳỷỹýỵ]";

        public string vowel = "aăâeêioôơuưy";

        public string vnCharacter = "ăâêôơưđ";

        public string[] vnCharacterExtractsign = { "aw", "aa", "ee", "oo", "ow", "uw", "dd" };

        public string[] keyBoard = { "qwertyuiop[]", "asdfghjkl;'", "zxcvbnm,./" };

        public string date = "";

        public string emailCheck = "";

        public string webCheck = "()";

        public string numberCheck = "";

        public string source = "àảãáạằẳẵắặầẩẫấậèẻẽéẹềểễếệìỉĩíịòỏõóọồổỗốộờởỡớợùủũúụừửữứựỳỷỹýỵ";

        public string dest = "aaaaaăăăăăâââââeeeeeêêêêêiiiiioooooôôôôôơơơơơuuuuuưưưưưyyyyy";

        public string sign = "frxsjfrxsjfrxsjfrxsjfrxsjfrxsjfrxsjfrxsjfrxsjfrxsjfrxsjfrxsj";

        public string[] initConsonant = { "ch", "tr",//1
            "d", "đ", "gi", "nh", "r", "v", //2
            "kh", "h", //3
            "qu", "q",//4
            "s", "x",//5
            "ng", "ngh",//6
            "l", "n",//7
            "t","th",//8
            "c", "k",//9
            "ph", //10
            "g", "gh",//11
            "b", "p", //12
            "m", //13
            };

        public int[] initConsonantCheck = { 1, 1,
            2, 2, 2, 2, 2, 2,
            3, 3,
            4, 4,
            5, 5,
            6, 6,
            7, 7,
            8, 8,
            9, 9,
            10,
            11, 11,
            12, 12,
            13
        };

        public string[] monothong = { "a", "ă", "â",//1
            "e", "ê",//2
            "i", "y",//3
            "o", "ô", "ơ",//4
            "u", "ư"//5
        };
        public int[] monothongCheck = { 1, 1, 1,
            2, 2,
            3, 3,
            4, 4, 4,
            5, 5
        };
        public string[] diphthong = { "ai", "ay", "ây", //1
            "ao", "au", "âu",                           //2
            "eo", "êu",                                 //3
            "ia", "iu", "iê",                           //4
            "oa", "oă",                                 //5
            "oe", "oi", "ôi", "ơi",                     //6
            "ua", "uâ", "uê",                           //7
            "ui", "uô", "uy",                           //8
            "ưa", "ưi", "ươ", "ưu",                     //9
            "ia"                                        //10
        };
        public int[] diphthongCheck = { 1, 1, 1,
            2, 2, 2,
            3, 3,
            4, 4, 4,
            5, 5,
            6, 6, 6, 6,
            7, 7, 7,
            8,8,8,
            9,9,9,9,
            10};
        public string[] triphthong = { "iêu",//1
            "oai", "oay", "uây",//2
            "uôi", "ươi", "ươu",//3
            "uya", "uyê" //4
        };
        public int[] triphthongCheck = { 1,
            2, 2, 2,
            3, 3, 3,
            4, 4
        };
        public string[] endError = { "ch",//1
            "nh", "n","ng", "ngh",//2
            "t", "c",//3
            };

        public int[] endErrorCheck = { 1,
            2,2,2,2,
            3,3
        };

        public int signErrorLength = 5;
        public string signError = "fsrxj";
    }
}
