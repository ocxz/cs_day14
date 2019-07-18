using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace _01_序列化和反序列化
{
    class Program
    {
        static void Main(string[] args)
        {
            /**
             *  序列化：对象转换为二进制
             *  反序列化：二进制转化为对象
             *  作用：传输数据
             */
            #region 序列化  服务端 将序列化后的数据 写入客户端
            //Person per = new Person("张三", 10, '男');

            //// 1、如果需要对类进行序列化，需要标记类为可序列化 方法：类上面写[Serializable]
            //// 2、利用FileStream流进行序列化

            //string path = @"personSerializable.txt";   // 序列化后，写到该文件里
            //using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            //{
            //    // 创建序列化对象
            //    BinaryFormatter bf = new BinaryFormatter();

            //    // 调用对象的序列化方法
            //    bf.Serialize(fsWrite, per);
            //}

            //Console.WriteLine("序列化完成");
            //Console.ReadKey();

            #endregion

            #region 反序列化 客户端 将数据反序列化 还原得到服务端传来的对象

            //string path = @"personSerializable.txt";   // 序列化文件

            //Person person;
            //using(FileStream fsRead = new FileStream(path, FileMode.Open, FileAccess.Read))
            //{
            //    // 创建序列化对象
            //    BinaryFormatter bf = new BinaryFormatter();

            //    // 调用反序列化方法
            //    person = (Person)bf.Deserialize(fsRead);
            //}

            //Console.WriteLine(person.Name);
            //Console.WriteLine(person.Age);
            //Console.WriteLine(person.Gender);
            //Console.ReadKey();

            #endregion


        }
    }

    [Serializable]
    public class Person
    {

        public Person(string name,int age,char gender)
        {
            this.Name = name;
            this.Age = age;
            this.Gender = gender;
        }

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
    }
}
