
using System.IO;
using UnityEngine;

namespace Assets.Script.Tools.FileLoad
{
    public class LoadFile 
    {
        public void fileLoad() {
            string path = @"X:\xxx\xxx";

            //第一种方法获取指定格式
            var files = Directory.GetFiles(path, "*.txt");

            foreach (var file in files)
                System.Console.WriteLine(file);

            //第二种方法获取指定格式
            DirectoryInfo folder = new DirectoryInfo(path);

            foreach (FileInfo file in folder.GetFiles("*.txt"))
            {
                System.Console.WriteLine(file.FullName);
            }
            //获取所有的文件名，不管什么格式

            foreach (FileInfo file in folder.GetFiles("*"))
            {
                System.Console.WriteLine(file.FullName);
            }
        
        }
    }
}