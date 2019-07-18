using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _04_序列化和反序列化练习
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 测试序列化和反序列化

            // 准备要测试要序列化的对象
            Person p1 = new Person("张三丰", 18, '男');
            Person p2 = new Person("张三", 18, '男');
            Person p3 = new Person("李四", 18, '男');
            Person p4 = new Person("王五五", 16, '女');
            Person p5 = new Person("郑凯男", 26, '女');
            Person p6 = new Person("李燕婷", 17, '女');
            Person p7 = new Person("任我行", 86, '男');
            Person[] pers = { p1, p2, p3, p4, p5, p6, p7 };

            for (int i = 0; i < pers.Length; i++)
            {
                // 服务端 对person对象进行序列化
                string path = @"序列化文件\" + pers[i].Name + ".serializable";
                Server.Serialize(path, pers[i]);
                Console.WriteLine("{0}序号化成功", i + 1);
            }
            Console.WriteLine("序列化完成，请按任意键进行反序列化测试");
            Console.ReadKey();

            Person[] results = new Person[pers.Length];
            for (int i = 0; i < pers.Length; i++)
            {
                string path = @"序列化文件\" + pers[i].Name + ".serializable";
                // 客户端 对序列化文件进行
                if (Client.DeSerialize(path) != null)
                {
                    results[i] = (Person)Client.DeSerialize(path);

                    Console.WriteLine("{0}号序列化文件反序列化成功", i + 1);
                }
            }
            Console.WriteLine("反序列化完成，请按任意键查看结果");
            Console.ReadKey();

            for (int i = 0; i < results.Length; i++)
            {
                results[i].SayHello();
            }

            Console.WriteLine("测试完成，请按任意键退出");
            Console.ReadKey();
            #endregion
        }
    }


    // 准备要序列化的类
    [Serializable]
    public class Person
    {
        public string Name
        {
            set;
            get;
        }
        public int Age
        {
            set;
            get;
        }
        public char Gender
        {
            set;
            get;
        }

        public Person() { }
        public Person(string name, int age, char Gender)
        {
            this.Name = name;
            this.Age = age;
            this.Gender = Gender;
        }

        public void SayHello()
        {
            Console.WriteLine("大家好，我叫{0}，我今年{1}了，是个{2}的", this.Name, this.Age, this.Gender);
        }
    }

    public static class Server
    {
        /// <summary>
        /// 提供序列化对象的方法
        /// </summary>
        /// <param name="path">序列化文件的路径</param>
        /// <param name="obj">要序列化的对象</param>
        public static void Serialize(string path, Object obj)
        {
            // 打开文件流，将序列化对象后的字节数组写入到文件流中
            using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                // 创建序列化对象
                BinaryFormatter bf = new BinaryFormatter();

                // 调用Serialize(fs,obj)对对象进行序列化
                bf.Serialize(fsWrite, obj);
            }
        }
    }

    public static class Client
    {
        /// <summary>
        /// 对序列化文件进行反序列化
        /// </summary>
        /// <param name="path">序列化文件路径</param>
        /// <returns>返回反序列化后的对象</returns>
        public static Object DeSerialize(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("序号化文件不存在");
                return null;
            }
            // 打开文件流，将文件流中的字节读出，进行反序列化成对象，进行返回
            using (FileStream fsRead = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // 创建序列化对象
                BinaryFormatter bf = new BinaryFormatter();

                // 调用bf.Deserialize(fsRead)进行反序列化，返回对象
                return bf.Deserialize(fsRead);
            }
        }
    }
}
