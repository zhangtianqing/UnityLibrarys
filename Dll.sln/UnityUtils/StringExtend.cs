using System.IO;
using System.Text;

namespace Dll.UnityUtils
{
    public static class StringExtend
    {
        /// <summary>
        /// �޳��ַ����в��Ϸ����ļ���
        /// </summary>
        /// <param name="strFileName">�ļ���</param>
        /// <param name="replaceStr">�滻�ַ���</param>
        /// <returns></returns>
        public static string FilterFileName(string strFileName, string replaceStr = "")
        {
            StringBuilder rBuilder = new StringBuilder(strFileName);
            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
                rBuilder.Replace(rInvalidChar.ToString(), replaceStr);
            return rBuilder.ToString();
        }
        /// <summary>
        /// �޳��ַ����в��Ϸ����ļ�·���ַ�
        /// </summary>
        /// <param name="rPath">�ļ�·��</param>
        /// <param name="replaceStr">�滻�ַ���</param>
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