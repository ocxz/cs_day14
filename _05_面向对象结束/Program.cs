using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace _05_面向对象结束
{
    class Program
    {
        private static string[] symbols = { "+", "-", "*", "/", "//", "%" };  // 定义一组运算符
        static void Main(string[] args)
        {
            /**
             *  利用面向对象 做一个计算器（实现加减乘除基本功能）
             */
        }

        /// <summary>
        /// 根据运算符，得到相应的运算器对象
        /// </summary>
        /// <param name="symbol">运算符</param>
        /// <returns>运算器对象</returns>
        public static Calculator GetCalculator(string symbol)
        {
            switch (symbol)
            {
                case "+":
                    return Summator.Create();
                case "-":
                    return Subtractor.Create();
                case "*":
                    return Multiplier.Create();
                case "/":
                    return Divider.Create();
                case "//":
                    return Aliquot.Create();
                case "%":
                    return Remainder.Create();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 初始化所有的运算器，放入Hashtable中，使用运算符来取出
        /// </summary>
        /// <returns>返回所有运算器的Hashtable集合</returns>
        public static Hashtable InitAllCalculator()
        {
            Hashtable allCalculators = new Hashtable();
            for (int i = 0; i < symbols.Length; i++)
            {
                if (GetCalculator(symbols[i]) != null)   // 如果该操作符有对应的运算器，则添加
                {
                    allCalculators.Add(symbols[i], GetCalculator(symbols[i]));
                }
            }

            return allCalculators;
        }

        public static bool AnalyzeSingle(string expression, out string[] result)
        {
            result = new string[3];
            string[] operations = Regex.Split(expression, @"\d");
            if (operations.Length > 1)
            {
                return false;
            }
            else
            {
                switch (operations[0])
                {
                    
                }
            }
            return true;
        }
    }


    /// <summary>
    /// 创建一个计算器接口
    /// </summary>
    public interface Calculator
    {
        /// <summary>
        /// 提供计算方法，由不同计算器来实现
        /// </summary>
        /// <param name="num1">第一个计算数</param>
        /// <param name="num2">第二个计算数</param>
        /// <returns>返回计算的结果</returns>
        double Operate(double num1, double num2);
    }

    /// <summary>
    /// 加法器  不能被继承
    /// </summary>
    public sealed class Summator : Calculator
    {
        private static Summator summator = null;    // 懒加载设计模式
        private Summator() { }    // 私有化构造器

        /// <summary>
        /// 加法操作
        /// </summary>
        /// <param name="num1">第一个计算数</param>
        /// <param name="num2">第二个计算数</param>
        /// <returns>返回加法运算的结果</returns>
        double Calculator.Operate(double num1, double num2)
        {
            return num1 + num2;
        }

        /// <summary>
        /// 创建加法器
        /// </summary>
        /// <returns>返回加法器</returns>
        public static Summator Create()
        {
            if (summator == null)
            {
                summator = new Summator();   // 如果计算器为空，则创建
            }
            return summator;
        }
    }

    /// <summary>
    /// 减法器
    /// </summary>
    public sealed class Subtractor : Calculator
    {
        private static Subtractor subtractor = new Subtractor();   // 饿汉式单例设计模式

        private Subtractor() { }   // 私有化构造器

        /// <summary>
        /// 创建减法器对象
        /// </summary>
        /// <returns>返回减法器对象</returns>
        public static Subtractor Create()
        {
            return subtractor;
        }

        /// <summary>
        /// 减法操作
        /// </summary>
        /// <param name="num1">第一个计算数</param>
        /// <param name="num2">第二个计算数</param>
        /// <returns>返回减法运算的结果</returns>
        double Calculator.Operate(double num1, double num2)
        {
            return num1 - num2;
        }
    }

    // 乘法器
    public sealed class Multiplier : Calculator
    {
        private static Multiplier multiplier = new Multiplier();  // 饿汉式单例设计模式

        private Multiplier() { }   // 私有化构造器

        /// <summary>
        /// 创建乘法器
        /// </summary>
        /// <returns>返回乘法器</returns>
        public static Multiplier Create()
        {
            return multiplier;
        }

        /// <summary>
        /// 乘法操作
        /// </summary>
        /// <param name="num1">第一个计算数</param>
        /// <param name="num2">第二个计算数</param>
        /// <returns>返回乘法运算的结果</returns>
        double Calculator.Operate(double num1, double num2)
        {
            return num1 * num2;
        }
    }

    /// <summary>
    /// 除法器
    /// </summary>
    public sealed class Divider : Calculator
    {
        private static Divider divider = new Divider();  // 饿汉式单例设计模式

        private Divider() { }

        /// <summary>
        /// 创建除法器
        /// </summary>
        /// <returns>返回除法器</returns>
        public static Divider Create()
        {
            return divider;
        }

        /// <summary>
        /// 除法操作
        /// </summary>
        /// <param name="num1">第一个计算数</param>
        /// <param name="num2">第二个计算数</param>
        /// <returns>返回除法运算的结果</returns>
        double Calculator.Operate(double num1, double num2)
        {
            return num2 == 0 ? double.MaxValue : num1 / num2;
        }
    }

    /// <summary>
    /// 取余器
    /// </summary>
    public sealed class Remainder : Calculator
    {
        private static Remainder remainder = new Remainder();  // 饿汉式单例设计模式

        private Remainder() { }

        /// <summary>
        /// 创建取余器
        /// </summary>
        /// <returns>返回取余器</returns>
        public static Remainder Create()
        {
            return remainder;
        }

        /// <summary>
        /// 取余操作
        /// </summary>
        /// <param name="num1">第一个计算数</param>
        /// <param name="num2">第二个计算数</param>
        /// <returns>返回取余运算的结果</returns>
        double Calculator.Operate(double num1, double num2)
        {
            return num2 == 0 ? double.MaxValue : num1 % num2;
        }
    }

    /// <summary>
    /// 整除器
    /// </summary>
    public sealed class Aliquot : Calculator
    {
        private static Aliquot aliquot = new Aliquot();   // 饿汉式单例设计模式

        private Aliquot() { }

        /// <summary>
        /// 创建整除器
        /// </summary>
        /// <returns>返回整除器</returns>
        public static Aliquot Create()
        {
            return aliquot;
        }

        /// <summary>
        /// 整除操作
        /// </summary>
        /// <param name="num1">第一个计算数</param>
        /// <param name="num2">第二个计算数</param>
        /// <returns>返回整除运算的结果</returns>
        double Calculator.Operate(double num1, double num2)
        {
            return num2 == 0 ? double.MaxValue : (int)(num1 / num2);
        }
    }
}
