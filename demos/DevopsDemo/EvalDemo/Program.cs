// See https://aka.ms/new-console-template for more information
using Devops.ReSDK;
using Evaluator;
using System.Text.RegularExpressions;

Console.WriteLine("字符串表达式计算工具");

var result = new Devops.Common.EvalSDK.EvalParser().EvalNumber("1+1", null);
Console.WriteLine(result);

EvalTest();

void EvalTest()
{
    Console.WriteLine("----------------------------------------------------");
    var parse = new EvalParser();
    Console.Write("请输入表达式：");//a+b*3/5+a
    var evalStr = Console.ReadLine();
    if (string.IsNullOrEmpty(evalStr))
    {
        Console.WriteLine("Game Over");
        return;
    }
    //解析其中的变量并让用户输入
    var matchs = Regex.Matches(evalStr, @"\b[\w$]+\b");
    var paramsDic = new Dictionary<string, object>();
    //预定义参数
    paramsDic.Add("now_year", DateTime.Now.Year);
    paramsDic.Add("now_month", DateTime.Now.Month);
    paramsDic.Add("now_day", DateTime.Now.Day);
    foreach (Match match in matchs)
    {
        if (decimal.TryParse(match.Value, out decimal kp))
            continue;
        if (!paramsDic.ContainsKey(match.Value))
        {
            Console.Write($"请输入数字变量【{match.Value}】：");
            var paramValue = Console.ReadLine();
            decimal dvalue;
            while (!decimal.TryParse(paramValue, out dvalue))
            {
                Console.WriteLine($"输入有误，请输入数字变量【{match.Value}】：");
                paramValue = Console.ReadLine();
            }
            paramsDic.Add(match.Value, dvalue);
        }
    }
    var result = parse.EvalNumber(evalStr, paramsDic);
    Console.WriteLine($"结果：{result}");
    EvalTest();
}