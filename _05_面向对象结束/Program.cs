using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace _05_面向对象结束
{
    class Program
    {
        //定义一组运算符
        private static string[] symbols = { "加:+", "减:-", "乘:*", "除:/", "整除://", "求指:**", "取余:%", "阶乘:!" };
        private static string[] otherOp = { "运算:Enter", "归零:rzero", "清屏:clear", "退出:exit" };    // 只有提示功能
        private static string[] symbolsKey = MyUtils.CutArrayString(symbols, ':', false);
        static void Main(string[] args)
        {
            /**
             *  利用面向对象 做一个计算器（实现加减乘除基本功能）
             */
            Begin();



        }

        /// <summary>
        /// 写头部
        /// </summary>
        private static void WriteHeader()
        {
            Console.Write("xxx计算器   ");
            foreach (var item in symbols)
            {
                Console.Write("{0}   ", item);
            }
            foreach (var item in otherOp)
            {
                Console.Write("{0}   ", item);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 计算器的开始
        /// </summary>
        private static void Begin()
        {
            // 初始化所有操作符，返回Hashtable，key：操作符，value：运算器对象
            Hashtable allCalculators = InitAllCalculator();
            // 用来存储结果值，初值为double.MinValue
            double result = double.MinValue;
            // 用来记录算了几步，算了一部str+="=="
            string str = "==";

            WriteHeader();   // 写头

            while (true)
            {
                // 打印出算的步数，及上次计算结果 如：====>25
                Console.Write("{0}>  {1}", str, result == double.MinValue ? "" : result.ToString());
                // 接收用户输入
                string input = MyUtils.RemoveSpace(Console.ReadLine());

                switch (input)
                {
                    case "exit":
                        Console.WriteLine("最终结果是：{0}", result);
                        Console.WriteLine("按任意键退出");
                        Console.ReadKey();
                        return;
                    case "clear":
                        Console.Clear();   // 清屏
                        WriteHeader();
                        str = "==";
                        break;
                    case "rzero":
                        Console.Clear();   // 清屏
                        WriteHeader();
                        str = "==";
                        result = double.MinValue;
                        break;
                    default:
                        input = result == double.MinValue ? input : result.ToString() + input;
                        string[] temp;
                        if (AnalyzeSingle(input, out temp))   // 分析成功
                        {
                            // 如果是一元运算，即temp[1] == null;则，第二个操作数作废，随便赋值
                            if (string.IsNullOrEmpty(temp[1]))
                            {
                                temp[1] = "1";   // 一元操作符；
                            }

                            // 根据运算符取到对应运算器
                            Calculator calculator = (Calculator)allCalculators[temp[2]];
                            try
                            {
                                double tempNum = calculator.Operate(double.Parse(temp[0]), double.Parse(temp[1]));
                                if (tempNum == double.MaxValue)
                                {
                                    Console.WriteLine("数据溢出，请重新输入");
                                }
                                else if(tempNum == double.MinValue)
                                {
                                    Console.WriteLine("表达式输入不正确");
                                }
                                else
                                {
                                    result = tempNum;
                                    str += "==";
                                }
                            }
                            catch
                            {
                                Console.WriteLine("表达式输入不正确");
                            }

                        }
                        else   // 分析失败 也就是表达式不正确
                        {
                            Console.WriteLine("表达式输入不正确");
                        }
                        break;
                }

            }
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
                case "**":
                    return Exp.Create();
                case "!":
                    return Factorial.Create();
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
            for (int i = 0; i < symbolsKey.Length; i++)
            {
                if (GetCalculator(symbolsKey[i]) != null)   // 如果该操作符有对应的运算器，则添加
                {
                    allCalculators.Add(symbolsKey[i], GetCalculator(symbolsKey[i]));
                }
            }

            return allCalculators;
        }

        /// <summary>
        /// 分析单运算表达式，将表达式分解成[n1,n2,o]
        /// </summary>
        /// <param name="expression">单运算表达式</param>
        /// <param name="result">返回的结果数组</param>
        /// <returns></returns>
        public static bool AnalyzeSingle(string expression, out string[] result)
        {
            result = new string[3];
            if (string.IsNullOrEmpty(expression)||expression.Length<2)   // 表达式为空
            {
                return false;
            }
            string express = @"\W+";   // 正则表达式 分割规则
            string operation = Regex.Match((MyUtils.RemoveSpace(expression.Replace(".","")).Substring(1)), express).Value;
            if (string.IsNullOrEmpty(operation))   // 操作符没有或大于1
            {
                return false;
            }
            else   // 操作符有1个
            {
                result[2] = operation;
                if (MyUtils.IsInArray(operation, symbolsKey))   // 操作符在操作数组中
                {
                    // 分割字符串，并将分割结果放入result中
                    // result[1] == null 表示是一元操作符
                    expression.Split(operation).CopyTo(result, 0);
                    return true;
                }
                else
                {
                    return false;
                }
            }
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

    /// <summary>
    /// 求指器
    /// </summary>
    public sealed class Exp : Calculator
    {
        private static Exp exp = new Exp();  // 饿汉式单例设计模式

        private Exp() { }

        /// <summary>
        /// 创建指数器
        /// </summary>
        /// <returns>返回指数器</returns>
        public static Exp Create()
        {
            return exp;
        }

        /// <summary>
        /// 指数操作
        /// </summary>
        /// <param name="num1">第一个计算数</param>
        /// <param name="num2">第二个计算数</param>
        /// <returns>返回指数运算的结果</returns>
        double Calculator.Operate(double num1, double num2)
        {
            if (num2 >= 0)   // 大于等于0 取指
            {
                return Math.Pow(num1, num2);
            }
            else
            {
                return Math.Pow(num1, 1.0 / (-num2));   // 开方
            }
        }
    }

    /// <summary>
    /// 阶乘器
    /// </summary>
    public sealed class Factorial : Calculator
    {
        private static Factorial factorial = new Factorial();

        private Factorial() { }

        /// <summary>
        /// 创建阶乘器
        /// </summary>
        /// <returns>返回阶乘器</returns>
        public static Factorial Create()
        {
            return factorial;
        }

        /// <summary>
        /// 指数操作
        /// </summary>
        /// <param name="num1">第一个计算数</param>
        /// <param name="num2">第二个计算数</param>
        /// <returns>返回阶乘运算的结果</returns>
        double Calculator.Operate(double num1, double num2)
        {
            if (num2 != 1)   // 如果阶乘表达式为 5!2 则返回double.MinValue
            {
                return double.MinValue;
            }
            int num = int.Parse(num1.ToString("0"));
            int result = 1;
            for (int i = num; i > 0; i--)
            {
                result *= i;
                if (result == 0)
                {
                    return double.MaxValue;
                }
            }
            return result * 1.0;
        }
    }

    /// <summary>
    /// 自定义工具类
    /// </summary>
    public static class MyUtils
    {
        /// <summary>
        /// 字符串数组去空，去掉诸如{"","",""}无用的元素
        /// </summary>
        /// <param name="strs">要去空的字符串数组</param>
        /// <returns>去空后的字符串数组</returns>
        public static string[] StringArrayTrim(string[] strs)
        {
            System.Collections.ArrayList tempArray = new ArrayList();
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i] != "")
                {
                    tempArray.Add(strs[i]);
                }
            }

            if (tempArray.Count <= 0)
            {
                return null;
            }
            else
            {

                string[] tempStrs = new string[tempArray.Count];
                for (int j = 0; j < tempArray.Count; j++)
                {
                    tempStrs[j] = (string)tempArray[j];
                }
                return tempStrs;
            }

        }

        /// <summary>
        /// 取字符串数组所有字符串的前部或后部 如：{"1:one","2:two"} 分隔符为':' 取{"1","2"}或{"one","two"}
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="separator"></param>
        /// <param name="firstCol"></param>
        /// <returns></returns>
        public static string[] CutArrayString(string[] strs, char separator, bool firstCol)
        {
            string[] result = new string[strs.Length];

            for (int i = 0; i < strs.Length; i++)
            {
                if (firstCol)
                {
                    result[i] = (strs[i].Substring(0, strs[i].IndexOf(separator))).Trim();
                }
                else
                {
                    result[i] = strs[i].Substring(strs[i].IndexOf(separator) + 1);
                }
            }

            return result;
        }

        /// <summary>
        /// 判断某个字符串是否在该数组中
        /// </summary>
        /// <param name="str">判断的字符串</param>
        /// <param name="array">字符串数组</param>
        /// <returns>在或不在</returns>
        public static bool IsInArray(string str, string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(str))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 字符串去除所有空格(包括字符串里面的空格）
        /// </summary>
        /// <param name="str">要去空格的字符串</param>
        /// <returns>去掉空格后的字符串</returns>
        public static string RemoveSpace(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string[] temp = StringArrayTrim(str.Split(" "));
            foreach (var item in temp)
            {
                sb.Append(item);
            }
            return sb.ToString();
        }
    }
}
