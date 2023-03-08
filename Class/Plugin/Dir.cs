using System.IO;

namespace OctogeddonUnpack.Class
{
    public class Dir
    {
        /// <summary>
        /// 获取文件名和目录名混合体的目录名，若不存在目录名则返回string.Empty
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetPath(string fileName)
        {
            int index;
            if ((index = fileName.LastIndexOf('\\')) >= 0)
            {
                return fileName.Substring(index + 1);
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 新建文件夹，若上级目录不存在则会新建上级目录
        /// </summary>
        /// <param name="pthName"></param>
        public static void NewDir(string pthName)
        {
            if (!Directory.Exists(pthName))
            {
                NewDir(pthName.Substring(0, pthName.LastIndexOf("\\")));
                Directory.CreateDirectory(pthName);
            }
        }
        /// <summary>
        /// 新建文件夹，若上级目录不存在则会新建上级目录
        /// </summary>
        /// <param name="pthName"></param>
        /// <param name="toEnd"></param>
        public static void NewDir(string pthName, bool toEnd)
        {
            if (toEnd)
            {
                NewDir(pthName);
            }
            else
            {
                NewDir(pthName.Substring(0, pthName.LastIndexOf("\\")));
            }
        }
        static string[] fileNameLib;
        static int fileNum;
        /// <summary>
        /// 获取一个目录下的所有文件，包含子目录里的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path)
        {
            fileNameLib = new string[100000];
            fileNum = 0;
            GetFile(path);
            string[] ansLib = new string[fileNum];
            for (int i = 0; i < fileNum; i++)
            {
                ansLib[i] = fileNameLib[i];
                fileNameLib[i] = null;
            }
            fileNameLib = null;
            fileNum = 0;
            return ansLib;
        }
        /// <summary>
        /// 递归获取子目录下文件
        /// </summary>
        /// <param name="path"></param>
        static void GetFile(string path)
        {
            string[] p = Directory.GetDirectories(path);
            for (int i = 0; i < p.Length; i++)
            {
                GetFile(p[i]);
            }
            p = Directory.GetFiles(path);
            for (int i = 0; i < p.Length; i++)
            {
                fileNameLib[fileNum++] = p[i];
            }
        }
        /// <summary>
        /// 按照文件系统方式格式化文件目录
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FormatPath(string filePath)
        {
            return filePath.Replace('/', '\\').Replace("\\\\", "\\").Replace(" \\", "\\");
        }
    }
}
