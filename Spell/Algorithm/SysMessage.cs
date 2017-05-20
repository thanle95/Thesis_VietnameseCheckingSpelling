using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spell.Algorithm
{
    class SysMessage
    {
        private static SysMessage instance = new SysMessage();
        private SysMessage()
        {

        }
        public static SysMessage Instance
        {
            get
            {
                return instance;
            }
        }

        public string Feature_is_updating
        {
            get
            {
                return "Tính năng đang được cập nhật.";
            }
        }
        public string No_error
        {
            get
            {
                return "Chúc mừng. Tài liệu của bạn không có lỗi.";
            }
        }
        public string Caption_Notify_Fix_Error
        {
            get
            {
                return "Microsoft Word Spelling";
            }
        }
        public string Message_Notify_Fix_Error(int count)
        {
            return string.Format("Có {0} lỗi. Bạn có muốn bắt đầu sửa lỗi ngay?", count);
        }
        public string IsNotError(string word)
        {
            return string.Format("\"{0}\" không phải là một lỗi!", word);
        }
    }
}
