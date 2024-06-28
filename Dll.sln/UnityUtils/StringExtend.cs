using System.IO;
using System.Text;

namespace Dll.UnityUtils
{
    public static class StringExtend
    {
        /// <summary>
        /// 剔除字符串中不合法的文件名
        /// </summary>
        /// <param name="strFileName"></param>
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
        /// <param name="rPath"></param>
        /// <param name="replaceStr"></param>
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