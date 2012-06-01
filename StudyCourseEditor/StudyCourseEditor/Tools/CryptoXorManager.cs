namespace StudyCourseEditor.Tools
{
    /// <summary>
    /// Simple XOR cripto class
    /// </summary>
    public class CryptoXorManager
    {
        public static string Process(string text, int code)
        {
            char[] chars = text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char) (chars[i] ^ code);
            }

            return new string(chars);
        }
    }
}