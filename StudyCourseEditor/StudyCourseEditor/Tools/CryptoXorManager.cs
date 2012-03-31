using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace StudyCourseEditor.Tools
{
    public class CryptoXorManager
    {
        public static string Encrypt(string text, int code)
        {
            var chars = text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)(chars[i] ^ code);
            }

            return new string(chars);
        }
    }
}