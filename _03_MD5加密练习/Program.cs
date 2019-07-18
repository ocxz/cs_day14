using System;
using System.Security.Cryptography;
using System.Text;

namespace _03_MD5加密练习
{
    public enum Operations
    {
        exit,
        Exit,
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("请输入要进行MD5加密的密码：");
                string input = Console.ReadLine();
                if (input=="exit" || input=="Exit")
                {
                    Console.WriteLine("退出");
                    break;
                }
                Console.WriteLine("加密后的密文为{0}", GetMD5(input));
            }
        }

        /// <summary>
        /// 给传入的字符串，进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的密文</returns>
        public static string GetMD5(string str)
        {
            // 通过MD5.Create()方法创建MD5对象

            MD5 md5 = MD5.Create();

            // 调用ComputeHash(bytes[])方法进行加密，参数：字节数组、返回值：字节数组
            byte[] buffer = md5.ComputeHash(Encoding.Default.GetBytes(str));

            // 创建StringBuilder对象，对循环转换字节数组后的字符串追加保存
            StringBuilder sb = new StringBuilder();

            // 循环遍历字节数组，转换，追加到sb中国
            foreach (var item in buffer)
            {
                sb.Append(item.ToString("x2"));   // 16进制转换，每次都是2位，不够0填充
            }

            // 将StringBuilder对象，转换为字符串，返回
            return sb.ToString();
        }
    }
}
