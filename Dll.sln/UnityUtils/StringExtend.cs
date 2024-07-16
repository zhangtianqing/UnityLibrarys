using System.IO;
using System.Text;

namespace Dll.UnityUtils
{
    public static class StringExtend
    {
        /// <summary>
        /// 剔除字符串中不合法的文件名
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <param name="replaceStr">替换字符串</param>
        /// <returns></returns>
        public static string FilterFileName(string strFileName, string replaceStr = "")
        {
            StringBuilder rBuilder = new StringBuilder(strFileName);
            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
                rBuilder.Replace(rInvalidChar.ToString(), replaceStr);
            return rBuilder.ToString();
        }
        /// <summary>
        /// 剔除字符串中不合法的文件路径字符
        /// </summary>
        /// <param name="rPath">文件路径</param>
        /// <param name="replaceStr">替换字符串</param>
        /// <returns></returns>
        public static string FilterDirectoryPath(string rPath, string replaceStr = "")
        {
            StringBuilder rBuilder = new StringBuilder(rPath);
            foreach (char rInvalidChar in Path.GetInvalidPathChars())
                rBuilder.Replace(rInvalidChar.ToString(), replaceStr);
            return rBuilder.ToString();
        }
    }
}