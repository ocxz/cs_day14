using System;
using System.Security.Cryptography;
using System.Text;

namespace _02_MD5加密
{
    class Program
    {
        static void Main(string[] args)
        {
            /**
             *  MD5加密
             * 
             */
            Console.WriteLine("请输入要加密的密码");
            string input = Console.ReadLine();
            Console.WriteLine(GetMD5(input));
            Console.ReadKey();
        }

        /// <summary>
        /// 通过MD5加密，获得加密后密文
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的密文</returns>
        public static string GetMD5(string str)
        {
            // 第一步：创建MD5对象
            MD5 md5 = MD5.Create();

            // 第二步：调用方法，进行加密
            byte[] md5Buffer = md5.ComputeHash(Encoding.Default.GetBytes(str));

            // 返回加密后的密文

            //return md5Buffer.ToString();   // 不能直接数组ToString 应该循环遍历ToString
            StringBuilder sb = new StringBuilder();     // 利用strigBuilder来循环追加转换过来的字符串
            for (int i = 0; i < md5Buffer.Length; i++)
            {
                sb.Append(md5Buffer[i].ToString("x2"));   // 16进制转换，每次转换2位，不够0填充
            }
            return sb.ToString();   // 返回转换sb为字符串，并返回
        }
    }
}
