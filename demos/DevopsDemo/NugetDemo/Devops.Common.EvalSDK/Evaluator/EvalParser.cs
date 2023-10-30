using Devops.ReSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Devops.Common.EvalSDK
{
    /// <summary>
    /// 解析器
    /// </summary>
    public class EvalParser
    {
        public const char AddOprator = '+';
        public const char SubOperator = '-';
        public const char DivOperator = '/';
        public const char MulOperator = '*';
        public const char LBraceOperator = '(';
        public const char RBraceOperator = ')';


        private static readonly OperatorChar AddOpratorChar = new OperatorChar() { Operator = AddOprator };
        private static readonly OperatorChar SubOperatorChar = new OperatorChar() { Operator = SubOperator };
        private static readonly OperatorChar DivOperatorChar = new OperatorChar() { Operator = DivOperator };
        private static readonly OperatorChar MulOperatorChar = new OperatorChar() { Operator = MulOperator };
        private static readonly OperatorChar LBraceOperatorChar = new OperatorChar() { Operator = LBraceOperator };
        private static readonly OperatorChar RBraceOperatorChar = new OperatorChar() { Operator = RBraceOperator };

        /// <summary>
        /// 符号转换字典
        /// </summary>
        private static Dictionary<char, string> OperatorToTextDic = new Dictionary<char, string>()
        {
            { '+', "_JIA_" },
            { '-', "_JIAN_" },
            { '/', "_CHENG_" },
            { '*', "_CHU_" },
            { '(', "_ZKH_" },
            { ')', "_YKH_" }
        };

        #region 数据内存计算

        /// <summary>
        /// 预处理计算表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="dynamicObject">参数</param>
        /// <param name="isCompile">是否是编译</param>
        /// <returns></returns>
        public string PreParserInfixExpression(string expression, Dictionary<string, object> dynamicObject, bool isCompile = false)
        {
            expression = expression.Trim();
            string pattern = @"\((.*?)\)";
            Match match = Regex.Match(expression, pattern);
            if (match.Success && match.Groups.Count > 1)
            {
                var constText = match.Groups[0].Value;
                var constValue = match.Groups[1].Value;
                string numPattern = @"\(([\s|0-9|\+\-\*\/|\.]+)\)";
                //纯数字计算 或者 不是编译预约
                if (Regex.IsMatch(constText, numPattern) || !isCompile)
                {
                    var evalValue = EvalNumber(constValue, dynamicObject);
                    if (evalValue == null)
                        return string.Empty;
                    var replaceText = evalValue.ToString();
                    expression = expression.Replace(constText, replaceText);
                }
                else if (isCompile)
                {
                    //编译计算
                    var completeText = Compile(constValue, dynamicObject).ToString();
                    //临时参数Key
                    var tempPramKey = "temp_" + Guid.NewGuid().ToString("n");
                    dynamicObject.Add(tempPramKey, completeText);
                    expression = expression.Replace(constText, tempPramKey);
                }
                else
                {
                    return expression;
                }
                return PreParserInfixExpression(expression, dynamicObject, isCompile);
            }
            return expression;
        }

        /// <summary>
        /// 文本转符号
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="dynamicObject"></param>
        /// <returns></returns>
        public string PreReplaceTextToOprator(string expression, Dictionary<string, object> dynamicObject)
        {
            //如果是参数里面包含了括号,将其中的参数替换成特殊字符
            var existOperatorKeys = dynamicObject.Keys.Where(s => OperatorToTextDic.Keys.Any(s2 => s.Contains(s2))).ToList();
            //存在特殊字符变量的
            if (existOperatorKeys.Any())
            {
                //将符号替换成字母
                existOperatorKeys.ForEach(s =>
                {
                    foreach (var s2 in OperatorToTextDic)
                    {
                        expression = expression.Replace(s2.Value, s2.Key.ToString());
                    }
                });
            }
            return expression;
        }
        /// <summary>
        /// 预处理参数符号转文本
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="dynamicObject"></param>
        /// <returns></returns>
        public string PreReplaceOpratorToText(string expression, Dictionary<string, object> dynamicObject)
        {
            //如果是参数里面包含了括号,将其中的参数替换成特殊字符
            var existOperatorKeys = dynamicObject.Keys.Where(s => OperatorToTextDic.Keys.Any(s2 => s.Contains(s2))).ToList();
            //存在特殊字符变量的
            if (existOperatorKeys.Any())
            {
                //将符号替换成字母
                foreach (var s in existOperatorKeys)
                {
                    var newKey = s;
                    foreach (var s2 in OperatorToTextDic)
                    {
                        newKey = newKey.Replace(s2.Key.ToString(), s2.Value);
                    }
                    expression = expression.Replace(s, newKey);
                }
            }
            return expression;
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="dynamicObject"></param>
        /// <param name="isComplete"></param>
        /// <returns></returns>
        public Queue<EvalItem> ParserInfixExpression(string expression, Dictionary<string, object> dynamicObject, bool isComplete = false)
        {
            var queue = new Queue<EvalItem>();
            if (string.IsNullOrEmpty(expression))
                return queue;
            expression = PreReplaceOpratorToText(expression, dynamicObject);
            expression = PreParserInfixExpression(expression, dynamicObject, isComplete);
            var operatorStack = new Stack<OperatorChar>();

            int index = 0;
            int itemLength = 0;
            //当第一个字符为+或者-的时候
            char firstChar = expression[0];
            if (firstChar == AddOprator || firstChar == SubOperator)
            {
                expression = string.Concat("0", expression);
            }
            int expressionLength = expression.Length;
            using (var scanner = new StringReader(expression))
            {
                string operatorPreItem = string.Empty;
                while (scanner.Peek() > -1)
                {
                    char currentChar = (char)scanner.Read();
                    switch (currentChar)
                    {
                        case AddOprator:
                        case SubOperator:
                        case DivOperator:
                        case MulOperator:
                        case LBraceOperator:
                        case RBraceOperator:
                            //直接把数字压入到队列中
                            operatorPreItem = expression.Substring(index, itemLength);
                            if (operatorPreItem != "")
                            {
                                var numberItem = new EvalItem(EItemType.Value, operatorPreItem);
                                queue.Enqueue(numberItem);
                            }
                            index = index + itemLength + 1;
                            itemLength = -1;
                            //当前操作符
                            var currentOperChar = new OperatorChar() { Operator = currentChar };
                            if (operatorStack.Count == 0)
                            {
                                operatorStack.Push(currentOperChar);
                                break;
                            }
                            //处理当前操作符与操作字符栈进出
                            var topOperator = operatorStack.Peek();
                            //若当前操作符为(或者栈顶元素为(则直接入栈
                            if (currentOperChar == LBraceOperatorChar || topOperator == LBraceOperatorChar)
                            {
                                operatorStack.Push(currentOperChar);
                                break;
                            }
                            //若当前操作符为),则栈顶元素顺序输出到队列,至到栈顶元素(输出为止,单(不进入队列,它自己也不进入队列
                            if (currentOperChar == RBraceOperatorChar)
                            {
                                while (operatorStack.Count > 0)
                                {
                                    if (operatorStack.Peek() != LBraceOperatorChar)
                                    {
                                        var operatorItem = new EvalItem(EItemType.Operator, operatorStack.Pop().GetContent());
                                        queue.Enqueue(operatorItem);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                if (operatorStack.Count > 0 && operatorStack.Peek() == RBraceOperatorChar)
                                {
                                    operatorStack.Pop();
                                }
                                break;
                            }
                            //若栈顶元素优先级高于当前元素，则栈顶元素输出到队列,当前元素入栈
                            if (topOperator.Level > currentOperChar.Level || topOperator.Level == currentOperChar.Level)
                            {
                                var topActualOperator = operatorStack.Pop();
                                var operatorItem = new EvalItem(EItemType.Operator, topActualOperator.GetContent());
                                queue.Enqueue(operatorItem);

                                while (operatorStack.Count > 0)
                                {
                                    var tempTop = operatorStack.Peek();
                                    if (tempTop.Level > currentOperChar.Level || tempTop.Level == currentOperChar.Level)
                                    {
                                        var topTemp = operatorStack.Pop();
                                        var operatorTempItem = new EvalItem(EItemType.Operator, topTemp.GetContent());
                                        queue.Enqueue(operatorTempItem);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                operatorStack.Push(currentOperChar);
                            }
                            //当当前元素小于栈顶元素的时候，当前元素直接入栈
                            else
                            {
                                operatorStack.Push(currentOperChar);
                            }
                            break;
                        default:
                            break;
                    }
                    itemLength++;
                }
            }
            //剩余无符号的字符串
            if (index < expressionLength)
            {
                string lastNumber = expression.Substring(index, expressionLength - index);
                var lastNumberItem = new EvalItem(EItemType.Value, lastNumber);
                queue.Enqueue(lastNumberItem);
            }
            //弹出栈中所有操作符号
            if (operatorStack.Count > 0)
            {
                while (operatorStack.Count != 0)
                {
                    var topOperator = operatorStack.Pop();
                    var operatorItem = new EvalItem(EItemType.Operator, topOperator.GetContent());
                    queue.Enqueue(operatorItem);
                }
            }
            return queue;
        }

        /// <summary>
        /// 计算表达式的计算结果
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="dynamicObject">动态对象</param>
        /// <param name="precision">精度 默认2</param>
        /// <returns>计算的结果</returns>
        public decimal? EvalNumber(string expression, Dictionary<string, object> dynamicObject, int precision = 2)
        {
            var values = dynamicObject ?? new Dictionary<string, object>();
            //中缀表达式，转换成后缀表达式并入列
            var queue = ParserInfixExpression(expression, values);
            var cacheStack = new Stack<Expression>();
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (item.ItemType == EItemType.Value && item.IsConstant)
                {
                    var itemExpression = Expression.Constant(item.Value);
                    cacheStack.Push(itemExpression);
                    continue;
                }
                if (item.ItemType == EItemType.Value && !item.IsConstant)
                {
                    var propertyName = item.Content.Trim();
                    //将参数替换回来
                    propertyName = PreReplaceTextToOprator(propertyName, values);
                    //参数为空的情况
                    if (!values.ContainsKey(propertyName) || values[propertyName] == null || !decimal.TryParse(values[propertyName].ToString(), out decimal propertyValue))
                        return null;
                    //var propertyValue = decimal.Parse(values[propertyName].ToString());
                    var itemExpression = Expression.Constant(propertyValue);
                    cacheStack.Push(itemExpression);
                }
                if (item.ItemType == EItemType.Operator)
                {
                    if (cacheStack.Count <= 1)
                        continue;
                    Expression firstParamterExpression = Expression.Empty();
                    Expression secondParamterExpression = Expression.Empty();
                    switch (item.Content[0])
                    {
                        case EvalParser.AddOprator:
                            firstParamterExpression = cacheStack.Pop();
                            secondParamterExpression = cacheStack.Pop();
                            var addExpression = Expression.Add(secondParamterExpression, firstParamterExpression);
                            cacheStack.Push(addExpression);
                            break;
                        case EvalParser.DivOperator:
                            firstParamterExpression = cacheStack.Pop();
                            secondParamterExpression = cacheStack.Pop();
                            var divExpression = Expression.Divide(secondParamterExpression, firstParamterExpression);
                            cacheStack.Push(divExpression);
                            break;
                        case EvalParser.MulOperator:
                            firstParamterExpression = cacheStack.Pop();
                            secondParamterExpression = cacheStack.Pop();
                            var mulExpression = Expression.Multiply(secondParamterExpression, firstParamterExpression);
                            cacheStack.Push(mulExpression);
                            break;
                        case EvalParser.SubOperator:
                            firstParamterExpression = cacheStack.Pop();
                            secondParamterExpression = cacheStack.Pop();
                            var subExpression = Expression.Subtract(secondParamterExpression, firstParamterExpression);
                            cacheStack.Push(subExpression);
                            break;
                        case EvalParser.LBraceOperator:
                        case EvalParser.RBraceOperator:
                            continue;
                        default:
                            throw new Exception("计算公式错误");
                    }
                }
            }
            if (cacheStack.Count == 0)
                return null;
            var lambdaExpression = Expression.Lambda<Func<decimal>>(cacheStack.Pop());
            try
            {
                // 除0 溢出
                var value = lambdaExpression.Compile()();
                return Math.Round(value, precision);
            }
            catch (Exception ex)
            {
                //System.OverflowException
                //System.DivideByZeroException
                if (ex is DivideByZeroException
                    || ex is OverflowException)
                    return null;
                throw ex;
            }
        }

        /// <summary>
        /// 计算表达式的日期结果
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="dynamicObject">动态对象</param>
        /// <returns>计算的结果</returns>
        public DateTime? EvalDate(string expression, Dictionary<string, object> dynamicObject)
        {
            var dateNumValue = EvalNumber(expression, dynamicObject);
            if (dateNumValue == null)
                return null;
            if (long.TryParse(dateNumValue.ToString(), out long dateNum))
            {
                return JsTimeToDateTime(dateNum);
            }
            return null;
        }

        /// <summary>
        /// 毫秒级时间戳转成 DateTime
        /// </summary>
        /// <param name="unixTimestamp"></param>
        /// <returns></returns>
        private DateTime JsTimeToDateTime(long unixTimestamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTimestamp).LocalDateTime;
        }
        #endregion

        /// <summary>
        /// 计算表达式的计算结果
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="dynamicObject">动态对象</param>
        /// <returns>计算的结果</returns>
        public string Compile(string expression, Dictionary<string, object> dynamicObject)
        {
            var queue = ParserInfixExpression(expression, dynamicObject, true);
            var values = dynamicObject ?? new Dictionary<string, object>();

            var cacheStack = new Stack<Expression>();
            var jsonStack = new Stack<string>();
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (item.ItemType == EItemType.Value && item.IsConstant)
                {
                    var itemExpression = Expression.Constant(item.Value);
                    cacheStack.Push(itemExpression);
                    jsonStack.Push(itemExpression.ToString());
                    continue;
                }
                if (item.ItemType == EItemType.Value && !item.IsConstant)
                {
                    var propertyName = item.Content.Trim();
                    //将参数替换回来
                    propertyName = PreReplaceTextToOprator(propertyName, values);
                    if (!values.ContainsKey(propertyName))
                        throw new Exception("无效的变量名：" + propertyName);
                    var propertyValue = values[propertyName];
                    var itemExpression = Expression.Constant(propertyValue);
                    //var itemExpression = Expression.Constant("'$formData." + propertyName + "'");
                    cacheStack.Push(itemExpression);
                    jsonStack.Push(itemExpression.ToString().Trim('\"'));
                }
                if (item.ItemType == EItemType.Operator)
                {
                    if (cacheStack.Count <= 1)
                        continue;
                    //将字符串对象包裹的引号去掉
                    string firstParamterExpression = string.Empty;
                    string secondParamterExpression = string.Empty;

                    //Console.WriteLine($"{secondParamterExpression} {item.Content[0]} {firstParamterExpression}");
                    jsonStack.Push($"{secondParamterExpression} {item.Content[0]} {firstParamterExpression}");
                    switch (item.Content[0])
                    {
                        case EvalParser.AddOprator:
                            firstParamterExpression = cacheStack.Pop()?.ToString().Trim('\"');
                            secondParamterExpression = cacheStack.Pop()?.ToString().Trim('\"');
                            //var addExpression = Expression.Add(secondParamterExpression, firstParamterExpression);
                            var addExpression = Expression.Constant("{'$add':[" + secondParamterExpression + "," + firstParamterExpression + "]}");
                            cacheStack.Push(addExpression);
                            jsonStack.Push(addExpression.ToString().Trim('\"'));
                            break;
                        case EvalParser.DivOperator:
                            firstParamterExpression = cacheStack.Pop()?.ToString().Trim('\"');
                            secondParamterExpression = cacheStack.Pop()?.ToString().Trim('\"');
                            //var divExpression = Expression.Divide(secondParamterExpression, firstParamterExpression);
                            //除0的情况处理
                            var divExpression = Expression.Constant("{'$cond': [{ '$ne': [" + firstParamterExpression + ", 0] },{'$divide':[" + secondParamterExpression + "," + firstParamterExpression + "]},0]}");
                            //var divExpression = Expression.Constant("{'$divide':[" + secondParamterExpression + "," + firstParamterExpression + "]}");
                            cacheStack.Push(divExpression);
                            jsonStack.Push(divExpression.ToString().Trim('\"'));
                            break;
                        case EvalParser.MulOperator:
                            firstParamterExpression = cacheStack.Pop()?.ToString().Trim('\"');
                            secondParamterExpression = cacheStack.Pop()?.ToString().Trim('\"');
                            //var mulExpression = Expression.Multiply(secondParamterExpression, firstParamterExpression);
                            var mulExpression = Expression.Constant("{'$multiply':[" + secondParamterExpression + "," + firstParamterExpression + "]}");
                            cacheStack.Push(mulExpression);
                            jsonStack.Push(mulExpression.ToString().Trim('\"'));
                            break;
                        case EvalParser.SubOperator:
                            firstParamterExpression = cacheStack.Pop()?.ToString().Trim('\"');
                            secondParamterExpression = cacheStack.Pop()?.ToString().Trim('\"');
                            //var subExpression = Expression.Subtract(secondParamterExpression, firstParamterExpression);
                            var subExpression = Expression.Constant("{'$subtract':[" + secondParamterExpression + "," + firstParamterExpression + "]}");
                            cacheStack.Push(subExpression);
                            jsonStack.Push(subExpression.ToString().Trim('\"'));
                            break;
                        case EvalParser.LBraceOperator:
                        case EvalParser.RBraceOperator:
                            continue;
                        default:
                            throw new Exception("wrong operator");
                    }
                }
            }

            return jsonStack.Pop();
        }

        /// <summary>
        /// 编译数值
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="dynamicObject">动态对象</param>
        /// <param name="precision">精度处理。4.4支持 $round,4.0不支持</param>
        /// <returns>计算的结果</returns>
        public string CompileNumber(string expression, Dictionary<string, object> dynamicObject, int precision = 2)
        {
            var result = Compile(expression, dynamicObject);
            return "{'$convert':{'input':" + result + ",to:'double',onError:null,onNull:null}}";
        }

        /// <summary>
        /// 编译日期
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="dynamicObject">动态对象</param>
        /// <returns>计算的结果</returns>
        public string CompileDate(string expression, Dictionary<string, object> dynamicObject)
        {
            var result = Compile(expression, dynamicObject);
            return "{'$convert':{'input':" + result + ",to:'date',onError:null,onNull:null}}";
        }


        /// <summary>
        /// 打印逆波兰后缀表达式
        /// </summary>
        /// <param name="queue">后缀表达式队列</param>
        /// <returns>后缀表达式字符串</returns>
        public string PrintPostfixExpression(Queue<EvalItem> queue)
        {
            StringBuilder text = new StringBuilder();
            while (queue.Count != 0)
            {
                var evalItem = queue.Dequeue();
                text.AppendFormat("{0}", evalItem.Content);
            }
            return text.ToString();
        }
    }
}
