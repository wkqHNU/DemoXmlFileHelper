using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static DemoXmlFileHelper.EnumList;

namespace DemoXmlFileHelper
{
    public class Xml2ParaList
    {
        public static Dictionary<string, object> ParaList;
        public static string testSystem;
        public static string testMode;

        /// <summary>
        /// 从xml中获取参数到ParaList
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void LoadParaData(string path)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                XmlNode para_nd = xml.ChildNodes[1];


                testSystem = ReadXmlValue<string>(para_nd, "TestSystem");
                testMode = ReadXmlValue<string>(para_nd, "TestMode");
                ParaList = null;
                string key;
                object value;
                if (para_nd.HasChildNodes)
                {
                    ParaList = new Dictionary<string, object>();
                    key = System.IO.Path.GetFileName(path);//文件名
                    value = path;//全路径名
                    AddPara(ParaList, key, value);

                    ReadXmlNodeStart(para_nd.ChildNodes);
                    Log.PrintLog(LogMsgType.Normal, "Template[" + System.IO.Path.GetFileName(path) + "] load success");
                }
            }
            catch (Exception ex)
            {
                Log.PrintLog(LogMsgType.Normal, ex.ToString());
                Log.PrintLog(LogMsgType.Normal, "Template[" + System.IO.Path.GetFileName(path) + "] load failure");
                throw new ParaListException(ex.ToString());
            }
        }
        
        /// <summary>
        /// 判断数值大小
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="valueName"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="openMin"></param>
        /// <param name="openMax"></param>
        /// <returns></returns>
        public static T CheckRange<T>(T value, string valueName, T min, T max, bool openMin = false, bool openMax = false)
        {
            TypeCode tc = Type.GetTypeCode(typeof(T));
            switch (tc)
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                    long valL = long.Parse(value.ToString());
                    long minL = long.Parse(min.ToString());
                    long maxL = long.Parse(max.ToString());
                    long leftL = valL - minL;
                    long rightL = valL - maxL;
                    if (minL > maxL)
                    {
                        throw new ParaListException(
                            string.Format("When judging the {0} variable range, the error appears: min={1} > max={2}",
                            valueName, minL, maxL));
                    }
                    if (leftL < 0) // 值小于最小值
                    {
                        if (!openMin)
                            throw new ParaListException(
                                string.Format("The {0} variable should be greater than or equal to the minimum {1}, but now {2} = {3}",
                                valueName, minL, valueName, value));
                        else
                            throw new ParaListException(
                                string.Format("The {0} variable should be greater than the minimum {1}, but now {2} = {3}",
                                valueName, minL, valueName, value));
                    }
                    else if ((openMin && leftL == 0)) // 值等于于最小值，默认不报错，看openMin
                    {
                        throw new ParaListException(
                            string.Format("The {0} variable should be greater than the minimum {1}, but now {2} = {3}",
                            valueName, minL, valueName, value));
                    }
                    else if (rightL > 0) // 值大于最大值
                    {
                        if (!openMin)
                            throw new ParaListException(
                                string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                                valueName, maxL, valueName, value));
                        else
                            throw new ParaListException(
                                string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                                valueName, maxL, valueName, value));
                    }
                    else if (openMax && rightL == 0) // 值等于于最大值，默认不报错，看openMax
                    {
                        throw new ParaListException(
                            string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                            valueName, maxL, valueName, value));
                    }
                    return value;
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Byte:
                    ulong valUL = ulong.Parse(((object)value).ToString());
                    ulong minUL = ulong.Parse(((object)min).ToString());
                    ulong maxUL = ulong.Parse(((object)max).ToString());
                    ulong leftUL = valUL - minUL;
                    ulong rightUL = valUL - maxUL;
                    if (minUL < maxUL)
                    {
                        throw new ParaListException(
                            string.Format("When judging the {0} variable range, the error appears: min={1} > max={2}",
                            valueName, minUL, maxUL));
                    }
                    if (leftUL < 0) // 值小于最小值
                    {
                        if (!openMin)
                            throw new ParaListException(
                                string.Format("The {0} variable should be greater than or equal to the minimum {1}, but now {2} = {3}",
                                valueName, minUL, valueName, value));
                        else
                            throw new ParaListException(
                                string.Format("The {0} variable should be greater than the minimum {1}, but now {2} = {3}",
                                valueName, minUL, valueName, value));
                    }
                    else if ((openMin && leftUL == 0)) // 值等于于最小值，默认不报错，看openMin
                    {
                        throw new ParaListException(
                            string.Format("The {0} variable should be greater than the minimum {1}, but now {2} = {3}",
                            valueName, minUL, valueName, value));
                    }
                    else if (rightUL > 0) // 值大于最大值
                    {
                        if (!openMin)
                            throw new ParaListException(
                                string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                                valueName, maxUL, valueName, value));
                        else
                            throw new ParaListException(
                                string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                                valueName, maxUL, valueName, value));
                    }
                    else if (openMax && rightUL == 0) // 值等于于最大值，默认不报错，看openMax
                    {
                        throw new ParaListException(
                            string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                            valueName, maxUL, valueName, value));
                    }
                    return value;
                case TypeCode.Double:
                case TypeCode.Single:
                    double valD = ulong.Parse(((object)value).ToString());
                    double minD = ulong.Parse(((object)min).ToString());
                    double maxD = ulong.Parse(((object)max).ToString());
                    double leftD = valD - minD;
                    double rightD = valD - maxD;
                    if (minD < maxD)
                    {
                        throw new ParaListException(
                            string.Format("When judging the {0} variable range, the error appears: min={1} > max={2}",
                            valueName, minD, maxD));
                    }
                    if (Math.Abs(leftD) < Double.Epsilon) // 值小于最小值
                    {
                        if (!openMin)
                            throw new ParaListException(
                                string.Format("The {0} variable should be greater than or equal to the minimum {1}, but now {2} = {3}",
                                valueName, minD, valueName, value));
                        else
                            throw new ParaListException(
                                string.Format("The {0} variable should be greater than the minimum {1}, but now {2} = {3}",
                                valueName, minD, valueName, value));
                    }
                    else if (openMin && Math.Abs(leftD) < Double.Epsilon) // 值等于于最小值，默认不报错，看openMin
                    {
                        throw new ParaListException(
                            string.Format("The {0} variable should be greater than the minimum {1}, but now {2} = {3}",
                            valueName, minD, valueName, value));
                    }
                    else if (Math.Abs(rightD) < Double.Epsilon) // 值大于最大值
                    {
                        if (!openMin)
                            throw new ParaListException(
                                string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                                valueName, maxD, valueName, value));
                        else
                            throw new ParaListException(
                                string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                                valueName, maxD, valueName, value));
                    }
                    else if (openMax && Math.Abs(rightD) < Double.Epsilon) // 值等于于最大值，默认不报错，看openMax
                    {
                        throw new ParaListException(
                            string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                            valueName, maxD, valueName, value));
                    }
                    return value;
                case TypeCode.Decimal: //Decimal表示的数据范围却比float和double类型小
                    Decimal valM = Decimal.Parse(value.ToString());
                    Decimal minM = Decimal.Parse(min.ToString());
                    Decimal maxM = Decimal.Parse(max.ToString());
                    Decimal leftM = valM - minM;
                    Decimal rightM = valM - maxM;
                    if (minM < maxM)
                    {
                        throw new ParaListException(
                            string.Format("When judging the {0} variable range, the error appears: min={1} > max={2}",
                            valueName, minM, maxM));
                    }
                    if (Math.Abs(leftM) < (Decimal)Double.Epsilon) // 值小于最小值
                    {
                        if (!openMin)
                            throw new ParaListException(
                                string.Format("The {0} variable should be greater than or equal to the minimum {1}, but now {2} = {3}",
                                valueName, minM, valueName, value));
                        else
                            throw new ParaListException(
                                string.Format("The {0} variable should be greater than the minimum {1}, but now {2} = {3}",
                                valueName, minM, valueName, value));
                    }
                    else if (openMin && Math.Abs(leftM) < (Decimal)Double.Epsilon) // 值等于于最小值，默认不报错，看openMin
                    {
                        throw new ParaListException(
                            string.Format("The {0} variable should be greater than the minimum {1}, but now {2} = {3}",
                            valueName, minM, valueName, value));
                    }
                    else if (Math.Abs(rightM) < (Decimal)Double.Epsilon) // 值大于最大值
                    {
                        if (!openMin)
                            throw new ParaListException(
                                string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                                valueName, maxM, valueName, value));
                        else
                            throw new ParaListException(
                                string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                                valueName, maxM, valueName, value));
                    }
                    else if (openMax && Math.Abs(rightM) < (Decimal)Double.Epsilon) // 值等于于最大值，默认不报错，看openMax
                    {
                        throw new ParaListException(
                            string.Format("The {0} variable should be less than or equal to the maximum {1}, but now {2} = {3}",
                            valueName, maxM, valueName, value));
                    }
                    return value;
                case TypeCode.Char:
                case TypeCode.String:
                case TypeCode.Boolean:
                case TypeCode.Object:
                case TypeCode.Empty:
                case TypeCode.DBNull:
                default:
                    return value;
            }
        }

        /// <summary>
        /// 根据关键字在参数列表中获取某个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paras"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetParaValue<T>(Dictionary<string, object> paras, string key)
        {
            if (!paras.ContainsKey(key))
                throw new ParaListException(
                    string.Format("There is not the parameter {0} in the parameter list.", key));
            try
            {
                if (!typeof(T).IsValueType)
                {
                    if (typeof(T).Equals(typeof(string)))
                        return (T)paras[key];
                }
                else if (typeof(T).IsValueType)
                {
                    if (typeof(T).Equals(typeof(int)))
                        return (T)(object)int.Parse(paras[key].ToString());
                    if (typeof(T).Equals(typeof(long)))
                        return (T)(object)long.Parse(paras[key].ToString());
                    if (typeof(T).Equals(typeof(short)))
                        return (T)(object)short.Parse(paras[key].ToString());
                    if (typeof(T).Equals(typeof(byte)))
                        return (T)(object)byte.Parse(paras[key].ToString());
                    if (typeof(T).Equals(typeof(bool)))
                        return (T)(object)bool.Parse(paras[key].ToString());
                    if (typeof(T).Equals(typeof(float)))
                        return (T)(object)float.Parse(paras[key].ToString());
                    if (typeof(T).Equals(typeof(double)))
                        return (T)(object)double.Parse(paras[key].ToString());
                    throw new ParaListException("Unexpected type for Utils.GetParaValue<T>.");
                }
                return (T)paras[key];
            }
            catch (Exception ex)
            {
                Log.PrintLog(LogMsgType.Normal, "Para[{0}] search failure.\r\n{1}", key, ex.ToString());
                throw new ParaListException(string.Format("Para[{0}] search failure.\r\n{1}", key, ex.ToString()));
            }
        }

        /// <summary>
        /// 从xml中获取参数到ParaList
        /// </summary>
        /// <param name="nds"></param>
        private static void ReadXmlNodeStart(XmlNodeList nds)
        {
            foreach (XmlNode nd in nds)
            {
                if (nd.Attributes["DisplayStyle"] == null)
                {
                    ReadXmlNodeStart(nd.ChildNodes);
                }
                else
                {
                    string key;
                    object value;
                    NodeStyle DisplayStyle = ReadXmlValue<NodeStyle>(nd, "DisplayStyle");
                    switch (DisplayStyle)
                    {
                        case NodeStyle.ComboBox:
                        case NodeStyle.FileSelect:
                        case NodeStyle.TextBox:
                        case NodeStyle.NoDisplay:
                            key = nd.Name;
                            value = ReadXmlValue<string>(nd, "Value");
                            AddPara(ParaList, key, value);
                            break;
                        case NodeStyle.FrequencyList:
                            key = nd.Name;
                            value = ReadXmlValue<string>(nd, "Value");
                            AddPara(ParaList, key, value);
                            
                            XmlNode child = nd.ChildNodes[0];
                            key = child.Name;
                            List<FreqBlock> linear = new List<FreqBlock>();
                            foreach (XmlNode ndTemp in child.ChildNodes)
                            {
                                FreqBlock fb = new FreqBlock();

                                fb.Start = ReadXmlValue<double>(ndTemp, "Start");
                                fb.Stop = ReadXmlValue<double>(ndTemp, "Stop");
                                fb.Step = ReadXmlValue<double>(ndTemp, "Step");
                                linear.Add(fb);
                            }
                            value = linear;
                            AddPara(ParaList, key, value);
                            
                            child = nd.ChildNodes[1];
                            key = child.Name;
                            List<FreqBlock> list = new List<FreqBlock>();
                            foreach (XmlNode ndTemp in child.ChildNodes)
                            {
                                FreqBlock fb = new FreqBlock();
                                fb.Start = ReadXmlValue<double>(ndTemp, "Value");
                                list.Add(fb);
                            }
                            value = list;
                            AddPara(ParaList, key, value);
                            
                            child = nd.ChildNodes[2];
                            key = child.Name;
                            if (child.HasChildNodes)
                                value = ReadXmlValue<double>(child.ChildNodes[0], "Value");
                            AddPara(ParaList, key, value);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 从xml中读取数值，不需要判断大小是否合理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nd"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static T ReadXmlValue<T>(XmlNode nd, string key)
        {
            T data = default(T);
            if (nd.Attributes[key] == null)
                throw new ParaListException(string.Format("Missing the {0} node in xml", key));
            if (nd.Attributes[key].Value == String.Empty)
                throw new ParaListException(string.Format("Missing the value of the {0} node in xml", key));
            try
            {
                if (!typeof(T).IsValueType)
                {
                    if (typeof(T).Equals(typeof(string)))
                        data = (T)(object)nd.Attributes[key].Value;
                }
                else if (typeof(T).IsValueType)
                {
                    if (typeof(T).Equals(typeof(int)))
                        data = (T)(object)int.Parse(nd.Attributes[key].Value);
                    else if (typeof(T).Equals(typeof(long)))
                        data = (T)(object)long.Parse(nd.Attributes[key].Value);
                    else if (typeof(T).Equals(typeof(short)))
                        data = (T)(object)short.Parse(nd.Attributes[key].Value);
                    else if (typeof(T).Equals(typeof(byte)))
                        data = (T)(object)byte.Parse(nd.Attributes[key].Value);
                    else if (typeof(T).Equals(typeof(bool)))
                        data = (T)(object)bool.Parse(nd.Attributes[key].Value);
                    else if (typeof(T).Equals(typeof(float)))
                        data = (T)(object)float.Parse(nd.Attributes[key].Value);
                    else if (typeof(T).Equals(typeof(double)))
                        data = (T)(object)double.Parse(nd.Attributes[key].Value);
                    else if (typeof(T).Equals(typeof(NodeStyle)))
                        data = (T)Enum.Parse(typeof(T), nd.Attributes[key].Value);
                    else
                        throw new ParaListException("Unexpected type for Utils.Attributes<T>!");
                }
            }
            catch (FormatException)
            {
                throw new FormatException(string.Format("The value in the {0} node is not in the correct format in xml", key));
            }
            catch (Exception ex)
            {
                throw new ParaListException(ex.ToString());
            }
            return data;
        }

        /// <summary>
        /// 向参数列表增加一组KeyValue
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="key"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        private static bool AddPara(Dictionary<string, object> paras, string key, object add)
        {
            if (paras.ContainsKey(key))
                throw new ParaListException(
                    string.Format("A parameter {0} already exists in the parameter list {1}. " +
                    "Please do not submit the same key {3} repeatedly.", key, nameof(paras), key));
            paras.Add(key, add);
            return true;
        }

        /// <summary>
        /// 修改参数列表中某组KeyValue的值,暂时还没想好在哪里用这个函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paras"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        //public static void SetParaValue<T>(Dictionary<string, Parameter<object>> paras, string key, T value)
        //{
        //    if (!paras.ContainsKey(key))
        //        throw new ParaListException(
        //            string.Format("There is no parameter {0} in the parameter list {1}", nameof(paras), key));
        //    paras[key].Value = value;
        //}

    }
}