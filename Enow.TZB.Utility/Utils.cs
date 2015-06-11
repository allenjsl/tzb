﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;

namespace Enow.TZB.Utility
{
    #region 农历信息业务实体
    /// <summary>
    /// 农历信息业务实体
    /// </summary>
    public class LunarCalendar
    {
        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 日
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 月份中的天数
        /// </summary>
        public int DaysInMonth { get; set; }
        /// <summary>
        /// 年份中的天数
        /// </summary>
        public int DaysInYear { get; set; }
    }
    #endregion

    #region 浏览器信息业务实体
    /// <summary>
    /// 浏览器信息业务实体
    /// </summary>
    [Serializable]
    public class BrowserInfo
    {
        string _browser, _version, _platform, _useragent;

        /// <summary>
        /// default constructor
        /// </summary>
        public BrowserInfo()
        {
            var request = HttpContext.Current.Request;
            if (request == null) return;
            var browser = request.Browser;
            if (browser == null) return;

            _useragent = request.UserAgent;
            _browser = browser.Browser;
            _version = browser.Version;
            _platform = browser.Platform;

            request = null;
            browser = null;
        }

        /// <summary>
        /// 获取由浏览器在 User-Agent 请求标头中发送的浏览器字符串（如果有）。
        /// </summary>
        public string Browser { get { return _browser; } }
        /// <summary>
        /// 获取浏览器的完整（整数和小数）版本号。
        /// </summary>
        public string Version { get { return _version; } }
        /// <summary>
        /// 获取客户端使用的平台的名称（如果已知）。
        /// </summary>
        public string Platform { get { return _platform; } }
        /// <summary>
        /// 获取客户端浏览器的原始用户代理信息。
        /// </summary>
        public string UserAgent { get { return _useragent; } }

        /// <summary>
        /// to json string
        /// </summary>
        /// <returns></returns>
        public string ToJsonString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
    #endregion

    #region Utils
    /// <summary>
    /// utils
    /// </summary>
    public class Utils
    {
        #region static constants
        //static constants
        /// <summary>
        /// 允许上传的文件类型
        /// </summary>
        public const string UploadFileExtensions = "*.xls;*.rar;*.pdf;*.doc;*.swf;*.jpg;*.gif;*.jpeg;*.png;*.dot;*.bmp;*.zip;*.7z;*.docx;*.xlsx;*.txt";
        #endregion

        /// <summary>
        /// 获取网站根目录的绝对地址。
        /// </summary>
        /// <value>返回的地址以'/'结束.</value>
        public static Uri AbsoluteWebRoot
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context == null)
                    throw new System.Net.WebException("The current HttpContext is null");

                if (context.Items["absoluteurl"] == null)
                    context.Items["absoluteurl"] = new Uri(context.Request.Url.GetLeftPart(UriPartial.Authority) + RelativeWebRoot);

                return context.Items["absoluteurl"] as Uri;
            }
        }

        private static string _RelativeWebRoot;

        /// <summary>
        /// 获取网站根目录的相对路径。
        /// </summary>
        /// <value>返回的地址以'/'结束.</value>
        public static string RelativeWebRoot
        {
            get
            {
                if (_RelativeWebRoot == null)
                    _RelativeWebRoot = VirtualPathUtility.ToAbsolute("~/");

                return _RelativeWebRoot;
            }
        }

        /// <summary>
        /// 获取域名后缀。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDomainSuffix(Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                if (host.Split('.').Length > 2)
                {
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);
                    return host.Substring(index + 1);
                }
            }

            return null;
        }

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.IndexOf("\\") == 0)
                {
                    strPath = strPath.Substring(1);
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 取得客户的IP数据
        /// </summary>
        /// <returns>客户的IP</returns>
        public static string GetRemoteIP()
        {
            string Remote_IP = "";
            try
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    Remote_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    Remote_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
            }
            catch
            {
            }
            return Remote_IP;
        }

        /// <summary>
        /// 获取当前页面地址
        /// </summary>
        /// <returns></returns>
        public static string GetRequestUrl()
        {
            string RequestUrl = "";
            try
            {
                if (HttpContext.Current.Request.Url != null)
                {
                    RequestUrl = HttpContext.Current.Request.Url.ToString();
                }
            }
            catch
            {
            }
            return RequestUrl;
        }
        /// <summary>
        /// 将127.0.0.1 形式的IP地址转换成10进制整数，这里没有进行任何错误处理
        /// </summary>
        /// <param name="strIP">IP地址转换</param>
        /// <returns>返回0进制整数</returns>
        public static long IpToLong(string strIP)
        {
            if (string.IsNullOrEmpty(strIP))
                return 0;

            string[] strIPs = strIP.Trim().Split('.');
            if (strIPs.Length != 4)
                return 0;

            long[] ip = new long[4];
            for (int i = 0; i < strIPs.Length; i++)
            {
                ip[i] = long.Parse(strIPs[i]);
            }

            return ip[0] * 256 * 256 * 256 + ip[1] * 256 * 256 + ip[2] * 256 + ip[3];
        }

        /// <summary>
        /// 替换XML敏感字符
        /// </summary>
        /// <param name="s">输入字符串</param>
        /// <returns></returns>
        public static string ReplaceXmlSpecialCharacter(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
            }

            return s;
        }

        /// <summary>
        /// 将字符串转化为数字(无符号整数) 若值不是数字返回defaultValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(string key, int defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }

            int result = 0;
            bool b = Int32.TryParse(key, out result);

            if (b) return result;

            return defaultValue;
        }

        /// <summary>
        /// 将字符串转化为数字(无符号整数) 若值不是数字返回0
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        /// <summary>
        /// get Nullable<int>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? GetIntNullable(string key, int? defaultValue)
        {
            if (string.IsNullOrEmpty(key)) return defaultValue;

            int result = 0;
            bool b = int.TryParse(key, out result);

            if (b) return result;

            return defaultValue;
        }

        /// <summary>
        /// 将字符串转化为decimal 若值不是数字返回defaultValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetDecimal(string key, decimal defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }

            decimal result = 0;
            bool b = decimal.TryParse(key, out result);

            if (b) return result;

            return defaultValue;
        }

        /// <summary>
        /// 将字符串转化为decimal 若值不是数字返回0
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetDecimal(string key)
        {
            return GetDecimal(key, 0);
        }

        /// <summary>
        /// 将字符串转化为double 若值不是数字返回defaultValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double GetDouble(string key, double defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }

            double result = 0;
            bool b = double.TryParse(key, out result);

            if (b) return result;

            return defaultValue;
        }

        /// <summary>
        /// 将字符串转化为double 若值不是数字返回0
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double GetDouble(string key)
        {
            return GetDouble(key, 0);
        }

        /// <summary>
        /// 将格式为yyyyMMdd(如：2010-12-05)的字符串转换成日期格式 若不能转换成日期将返回defaultValue
        /// </summary>
        /// <param name="s">要转换的字符串 格式(yyyyMMdd 如：20101205)</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetDateTime112(string s, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(s))
            {
                return defaultValue;
            }

            DateTime result = defaultValue;
            bool b = DateTime.TryParse(s.Substring(0, 4) + "-" + s.Substring(4, 2) + "-" + s.Substring(6, 2), out result);

            return result;
        }

        /// <summary>
        /// 将格式为yyyyMMdd(如：2010-12-05)的字符串转换成日期格式 若不能转换成日期将返回DateTime.MinValue
        /// </summary>
        /// <param name="s">要转换的字符串 格式(yyyyMMdd 如：20101205)</param>
        /// <returns></returns>
        public static DateTime GetDateTime112(string s)
        {
            return GetDateTime112(s, DateTime.MinValue);
        }

        /// <summary>
        /// 将字符串转换成日期格式 若不能转换成日期将返回defaultValue
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(string s, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(s))
            {
                return defaultValue;
            }

            DateTime result = defaultValue;
            bool b = DateTime.TryParse(s, out result);

            if (b) return result;

            return defaultValue;
        }

        /// <summary>
        /// 将字符串转换成日期格式 若不能转换成日期将返回DateTime.MinValue
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <returns></returns>
        public static DateTime GetDateTime(string s)
        {
            return GetDateTime(s, DateTime.MinValue);
        }

        /// <summary>
        /// 获取当前农历日期
        /// </summary>
        /// <returns></returns>
        public static LunarCalendar GetLunarCalendar()
        {
            return GetLunarCalendar(DateTime.Today);
        }

        /// <summary>
        /// 将指定日期转换成农历日期
        /// </summary>
        /// <param name="d">公历日期</param>
        /// <returns></returns>
        public static LunarCalendar GetLunarCalendar(DateTime d)
        {
            LunarCalendar lunarCalendar = new LunarCalendar();

            ChineseLunisolarCalendar chineseLunisolarCalendar = new ChineseLunisolarCalendar();
            lunarCalendar.Year = chineseLunisolarCalendar.GetYear(d);
            lunarCalendar.Month = chineseLunisolarCalendar.GetMonth(d);
            lunarCalendar.Day = chineseLunisolarCalendar.GetDayOfMonth(d);
            lunarCalendar.DaysInMonth = chineseLunisolarCalendar.GetDaysInMonth(lunarCalendar.Year, lunarCalendar.Month);
            lunarCalendar.DaysInYear = chineseLunisolarCalendar.GetDaysInYear(lunarCalendar.Year);

            return lunarCalendar;
        }

        /// <summary>
        /// 根据整型数组生成半角逗号分割的Sql字符串
        /// </summary>
        /// <param name="arrIds">整型数组</param>
        /// <returns>半角逗号分割的Sql字符串</returns>
        public static string GetSqlIdStrByArray(int[] arrIds)
        {
            if (arrIds == null || arrIds.Length <= 0)
                return string.Empty;

            string strTmp = arrIds.Where(i => i > 0).Aggregate(string.Empty, (current, i) => current + (i + ","));
            strTmp = strTmp.Trim(',');

            return strTmp;
        }

        /// <summary>
        /// 根据整型集合生成半角逗号分割的的Sql字符串
        /// </summary>
        /// <param name="ids">整形集合</param>
        /// <returns></returns>
        public static string GetSqlIdStrByList(IList<int> ids)
        {
            if (ids == null || ids.Count <= 0) return "0";
            StringBuilder s = new StringBuilder();
            s.AppendFormat("{0}", ids[0].ToString());

            for (int i = 1; i < ids.Count; i++)
            {
                s.AppendFormat(",{0}", ids[i].ToString());
            }

            return s.ToString();
        }

        /// <summary>
        /// 获取SQL IN 字符串
        /// </summary>
        /// <param name="ids">匹配字符串数组</param>
        /// <returns></returns>
        public static string GetSqlInExpression(string[] ids)
        {
            if (ids == null || ids.Length == 0) return "''";

            StringBuilder s = new StringBuilder();
            s.AppendFormat("'{0}'", ids[0]);

            for (int i = 1; i < ids.Length; i++)
            {
                s.AppendFormat(",'{0}'", ids[i]);
            }

            return s.ToString();
        }

        /// <summary>
        /// 获取SQL IN 字符串
        /// </summary>
        /// <param name="ids">匹配字符串集合</param>
        /// <returns></returns>
        public static string GetSqlInExpression(IList<string> ids)
        {
            if (ids == null || ids.Count == 0) return "''";

            StringBuilder s = new StringBuilder();
            s.AppendFormat("'{0}'", ids[0]);

            for (int i = 1; i < ids.Count; i++)
            {
                s.AppendFormat(",'{0}'", ids[i]);
            }

            return s.ToString();
        }

        #region XElement
        /// <summary>
        /// Get XAttribute Value
        /// </summary>
        /// <param name="XAttribute">xAttribute</param>
        /// <returns>Value</returns>
        public static string GetXAttributeValue(XAttribute xAttribute)
        {
            if (xAttribute == null)
                return string.Empty;

            return xAttribute.Value;
        }

        /// <summary>
        /// Get XAttribute Value
        /// </summary>
        /// <param name="xElement">XElement</param>
        /// <param name="attributeName">Attribute Name</param>
        /// <returns></returns>
        public static string GetXAttributeValue(XElement xElement, string attributeName)
        {
            return GetXAttributeValue(xElement.Attribute(attributeName));
        }

        /// <summary>
        /// Get XElement
        /// </summary>
        /// <param name="xElement">parent xElement</param>
        /// <param name="xName">xName</param>
        /// <returns>XElement</returns>
        public static XElement GetXElement(XElement xElement, string xName)
        {
            XElement x = xElement.Element(xName);

            if (x != null) return x;

            return new XElement(xName);
        }

        /// <summary>
        /// Get XElements
        /// </summary>
        /// <param name="xElement">parent xElement</param>
        /// <param name="xName">xName</param>
        /// <returns>XElements</returns>
        public static IEnumerable<XElement> GetXElements(XElement xElement, string xName)
        {
            var x = xElement.Elements(xName);
            if (x == null)
                return new List<XElement>();

            return x;
        }
        #endregion

        /// <summary>
        /// 传的like key参数经过toSqlLike格式化
        /// </summary>
        /// <param name="s">匹配字符串</param>
        /// <returns>格式化字符串</returns>
        public static string ToSqlLike(string s)
        {
            return string.IsNullOrEmpty(s) ? s : ((new Regex(@"(\[|\]|\*|_|%)")).Replace(s, "[$1]").Replace("'", "''"));
        }

        /// <summary>
        /// 获取XML文档指定属性值
        /// </summary>
        /// <param name="xml">XML文档</param>
        /// <param name="attribute">属性</param>
        /// <returns>属性值</returns>
        public static string GetValueFromXmlByAttribute(string xml, string attribute)
        {
            if (string.IsNullOrEmpty(xml)) return "";
            var xRoot = XElement.Parse(xml);
            var xRows = GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                return GetXAttributeValue(xRow, attribute);
            }
            return "";
        }

        ///// <summary>
        ///// get enum value
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="s">转换的字符串</param>
        ///// <param name="defaultValue">默认值</param>
        ///// <returns></returns>
        //public static int? GetEnumValue(Type enumType, string s, int? defaultValue)
        //{
        //    int? _enum = GetIntNullable(s, null);
        //    if (!_enum.HasValue) return defaultValue;

        //    if (!Enum.IsDefined(enumType, _enum)) return defaultValue;

        //    return _enum;
        //}

        /// <summary>
        /// get enum value
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="s">转换的字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetEnumValue(Type enumType, string s, int defaultValue)
        {
            int? _enum = GetIntNullable(s, null);
            if (!_enum.HasValue) return defaultValue;

            if (!Enum.IsDefined(enumType, _enum)) return defaultValue;

            return _enum.Value;
        }

        /// <summary>
        /// get enum value
        /// </summary>
        /// <param name="s">转换的字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <typeparam name="T">typeof(T).IsEnum==true</typeparam>
        /// <returns></returns>
        public static T GetEnumValue<T>(string s, T defaultValue)
        {
            if (typeof(T).IsEnum)
            {
                int? _enum = GetIntNullable(s, null);
                if (!_enum.HasValue) return defaultValue;

                if (!Enum.IsDefined(typeof(T), _enum.Value)) return defaultValue;

                return (T)(object)_enum.Value;
            }

            return defaultValue;
        }


        /// <summary>
        /// 获取SQL IN 字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string GetSqlIn<T>(ICollection<T> ids)
        {

            if (typeof(T).IsEnum)
            {
                if (ids == null || ids.Count < 1) return "-1";
                StringBuilder s1 = new StringBuilder();
                foreach (var item in ids)
                {
                    s1.AppendFormat(",{0}", Convert.ToInt32(item));
                }

                return s1.ToString().Substring(1);
            }

            if (typeof(T) == typeof(int) || typeof(T) == typeof(byte))
            {
                if (ids == null || ids.Count < 1) return "0";
                StringBuilder s2 = new StringBuilder();
                foreach (var item in ids)
                {
                    s2.AppendFormat(",{0}", item);
                }
                return s2.ToString().Substring(1);
            }

            if (typeof(T) == typeof(string))
            {
                if (ids == null || ids.Count < 1) return "''";
                StringBuilder s3 = new StringBuilder();
                foreach (var item in ids)
                {
                    s3.AppendFormat(",'{0}'", item);
                }
                return s3.ToString().Substring(1);
            }

            return "-1";
        }

        /// <summary>
        /// 将字符串转换为可空的日期类型，如果字符串不是有效的日期格式，则返回null
        /// </summary>
        /// <param name="s">进行转换的字符串</param>
        /// <returns></returns>
        public static DateTime? GetDateTimeNullable(string s)
        {
            return GetDateTimeNullable(s, null);
        }
        /// <summary>
        /// 将字符串转换为可空的日期类型，如果字符串不是有效的日期格式，则返回defaultValue
        /// </summary>
        /// <param name="s">进行转换的字符串</param>
        /// <param name="defaultValue">要返回的默认值</param

        /// <returns></returns>
        public static DateTime? GetDateTimeNullable(string s, DateTime? defaultValue)
        {
            if (string.IsNullOrEmpty(s))
            {
                return defaultValue;
            }

            DateTime result = DateTime.MinValue;
            bool b = DateTime.TryParse(s, out result);

            if (b) return result;

            return defaultValue;
        }

        /// <summary>
        /// 获取SQL比较运算符((int)XZ:EyouSoft.Model.EnumType.FinStructure.EqualSign)
        /// </summary>
        /// <param name="_operator">运算符</param>
        /// <returns></returns>
        public static string GetSqlBiJiaoYunSuanFu(int _operator)
        {
            string s = string.Empty;

            switch (_operator)
            {
                case 0: s = ">="; break;
                case 1: s = "="; break;
                case 2: s = "<="; break;
                case 3: s = "<>"; break;
                default: break;
            }

            return s;
        }

        /// <summary>
        /// 确保用户的输入没有恶意代码
        /// </summary>
        /// <param name="text">要过滤的字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns>过滤后的字符串</returns>
        public static string InputText(string text, int maxLength)
        {
            if (text == null)
            {
                return string.Empty;
            }
            text = text.Trim();
            if (text == string.Empty)
            {
                return string.Empty;
            }
            if (text.Length > maxLength)
            {
                text = text.Substring(0, maxLength);
            }
            //text = Regex.Replace(text, "[\\s]{2,}", " ");	//将连续的空格转换为一个空格
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
            text = text.Replace("'", "''");
            //text = FormatKeyWord(text);//过滤敏感字符
            return text;
        }

        public static string InputText(string text)
        {
            return InputText(text, Int32.MaxValue);
        }

        public static string InputText(object text)
        {
            if (text == null)
            {
                return string.Empty;
            }
            return InputText(text.ToString());
        }
        public static string GetQueryStringValue(string key)
        {
            string tmp = HttpContext.Current.Request.QueryString[key] != null ? HttpContext.Current.Request.QueryString[key].ToString() : "";
            return InputText(tmp);
        }

        //处理分页的URL 
        public static string GetUrlForPage(System.Web.HttpRequest request)
        {
            //如果是分页以后的链接
            if (request.RawUrl.ToUpper().IndexOf("ASPX") < 0)
            {
                //判断是否是查询以后的链接
                if (request.Url.ToString().ToUpper().IndexOf("PAGE=") >= 0)
                {
                    string newUrl = request.RawUrl;
                    //如果是分页以后的，那么进行截取
                    newUrl = newUrl.Substring(0, newUrl.LastIndexOf('_'));
                    return newUrl;
                }
                else
                {
                    return request.RawUrl;
                }
            }
            //分页之前的链接
            else
            {
                return request.Url.ToString();
            }
        }

        /// <summary>
        /// 过滤编辑器输入的恶意代码
        /// </summary>
        /// <param name="key">需要过滤的字符串</param>
        /// <returns></returns>
        public static string EditInputText(string text)
        {
            if (text == null || text.Trim() == string.Empty)
            {
                return string.Empty;
            }
            if (text.Length > Int32.MaxValue)
            {
                text = text.Substring(0, Int32.MaxValue);
            }
            text = text.Replace("'", "''");
            return Microsoft.Security.Application.AntiXss.GetSafeHtmlFragment(text);
        }

        /// <summary>
        /// 获取表单的值
        /// </summary>
        /// <param name="key">表单的key</param>
        /// <returns></returns>
        public static string GetFormValue(string key)
        {
            return GetFormValue(key, Int32.MaxValue);
        }
        /// <summary>
        /// 获取表单的值
        /// </summary>
        /// <param name="key">表单的key</param>
        /// <param name="maxLength">接受的最大长度</param>
        /// <returns></returns>
        public static string GetFormValue(string key, int maxLength)
        {
            string tmp = HttpContext.Current.Request.Form[key] != null ? HttpContext.Current.Request.Form[key].ToString() : "";
            return InputText(tmp, maxLength);
        }

        public static string[] GetFormValues(string key)
        {
            string[] tmps = HttpContext.Current.Request.Form.GetValues(key);
            if (tmps == null)
            {
                return new string[] { };
            }
            for (int i = 0; i < tmps.Length; i++)
            {
                tmps[i] = InputText(tmps[i]);
            }
            return tmps;
        }
        /// <summary>
        /// 获取编辑器表单值
        /// </summary>
        /// <param name="name">input name</param>
        /// <returns></returns>
        public static string GetFormEditorValue(string key)
        {
            if (string.IsNullOrEmpty(key)) return "";
            string _values = HttpContext.Current.Request.Form[key].ToString();
            if (_values == null || _values == "") return "";
            _values = Microsoft.Security.Application.AntiXss.GetSafeHtmlFragment(_values);
            return _values;
        }

        /// <summary>
        /// 获取编辑器表单值
        /// </summary>
        /// <param name="name">input name</param>
        /// <returns></returns>
        public static string[] GetFormEditorValues(string name)
        {
            if (string.IsNullOrEmpty(name)) return new string[] { };
            string[] _values = HttpContext.Current.Request.Form.GetValues(name);
            if (_values == null) return new string[] { };

            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = Microsoft.Security.Application.AntiXss.GetSafeHtmlFragment(_values[i]);
            }

            return _values;
        }

        /// <summary>
        /// 若字符串为null或Empty，则返回指定的defaultValue.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetString(string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }


        /// <summary>
        /// 将字符串转化为数字(有符号整数) 若值不是数字返回defaultValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetIntSign(string key, int defaultValue)
        {
            if (string.IsNullOrEmpty(key) || !StringValidate.IsIntegerSign(key))
            {
                return defaultValue;
            }


            int result = 0;
            bool b = Int32.TryParse(key, out result);

            return result;
        }

        /// <summary>
        /// 将字符串转化为数字(有符号整数) 若值不是数字返回0
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetIntSign(string key)
        {
            return GetIntSign(key, 0);
        }

        /// <summary>
        /// 将字符串转化为Int可空类型，若不是数字指定的defaultValue
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? GetIntNull(string s, int? defaultValue)
        {
            if (string.IsNullOrEmpty(s)) return defaultValue;

            int result = 0;
            bool b = int.TryParse(s, out result);

            if (b) return result;

            return defaultValue;
        }

        /// <summary>
        /// 将字符串转化为Int可空类型，若不是数字返回null的Int?.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int? GetIntNull(string s)
        {
            return GetIntNull(s, null);
        }

        public static decimal? GetDecimal(string key, decimal? defaultValue)
        {
            if (string.IsNullOrEmpty(key) || !StringValidate.IsDecimalSign(key))
            {
                return defaultValue;
            }
            return Decimal.Parse(key);
        }

        public static decimal? GetDecimalNull(string key)
        {
            return GetDecimal(key, null);
        }

        /// <summary>
        /// 获得当月的第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetFristDayOfMonth()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        /// <summary>
        /// 获得当月的最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth()
        {
            return GetFristDayOfMonth().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 判断输入的字符串是否是有效的电话号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhone(string input)
        {
            return StringValidate.IsRegexMatch(input, @"^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-?)?[1-9]\d{6,7}(\-\d{1,4})?$");
        }
        /// <summary>
        /// 判断输入的字符串是否是有效的手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobile(string input)
        {
            return StringValidate.IsRegexMatch(input, @"^(13|15|18|14)\d{9}$");
        }
        /// <summary>
        /// 判断输入的字符串是否是有效的电话号码或者手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(string input)
        {
            return IsPhone(input) || IsMobile(input);
        }

        /// <summary>
        /// 判断输入的字符串是否是有效的身份证号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIDcard(string input)
        {
            return StringValidate.IsIDcard(input);
        }

        /// <summary>
        /// 根据指定的消息显示Alert消息对话框，并跳转到指定的url地址
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        public static void ShowAndRedirect(string msg, string url)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Write("<script>alert('");
            response.Write(msg);
            response.Write("');window.location.href='");
            response.Write(url);
            response.Write("';");
            response.Write("</script>");
            response.End();
        }
        public static void ShowAndRedirect(string msg)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Write("<script>alert('");
            response.Write(msg);
            response.Write("');");
            response.Write("</script>");
            response.End();
        }
        /// <summary>
        /// 顶级跳转
        /// </summary>
        /// <param name="url"></param>
        public static void TopRedirect(string url)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Write("<script>window.location.href='");
            response.Write(url);
            response.Write("';");
            response.Write("</script>");
            response.End();
        }
        /// <summary>
        /// 顶级页面刷新
        /// </summary>
        /// <param name="url"></param>
        public static void TopRedirect()
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Write("<script>window.location.href=window.location.href");
            response.Write("</script>");
            response.End();
        }
        /// <summary>
        /// 弹出提示消息关闭Boxy对话框
        /// </summary>
        /// <param name="msg">提示消息</param>
        /// <param name="IframeId">boxyId</param>
        /// <param name="IsRefresh">是否刷新父页面</param>
        public static void ShowMsgAndCloseBoxy(string msg, string IframeId, bool IsRefresh)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Write("<script>alert('");
            response.Write(msg);
            response.Write("');");
            response.Write("window.parent.Boxy.getIframeDialog('" + IframeId + "').hide();");
            if (IsRefresh)
                response.Write("parent.location.href=parent.location.href;");
            response.Write("</script>");
            response.End();
        }
        /// <summary>
        /// 清空页面，输出指定的字符串
        /// </summary>
        /// <param name="msg"></param>
        public static void Show(string msg)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Write(msg);
            response.End();
        }

        /// <summary>
        /// 后台alert 信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string ShowMsg(string msg)
        {
            return "javascript:alert('" + msg + "');";
        }

        /// <summary>
        /// 判断是否是有效的密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string input)
        {
            return StringValidate.IsRegexMatch(input, @"^[a-zA-Z\W_\d]{6,16}$");
        }


        /// <summary>
        /// 将相对url地址转换为绝对url地址.
        /// </summary>
        public static Uri ConvertToAbsolute(Uri relativeUri)
        {
            return ConvertToAbsolute(relativeUri.ToString()); ;
        }

        /// <summary>
        /// 将相对url地址转换为绝对url地址.
        /// </summary>
        public static Uri ConvertToAbsolute(string relativeUri)
        {
            if (String.IsNullOrEmpty(relativeUri))
                throw new ArgumentNullException("relativeUri");

            string absolute = AbsoluteWebRoot.ToString();
            int index = absolute.LastIndexOf(RelativeWebRoot.ToString());

            return new Uri(absolute.Substring(0, index) + relativeUri);
        }

        /// Retrieves the subdomain from the specified URL.
        /// </summary>
        /// <param name="url">The URL from which to retrieve the subdomain.</param>
        /// <returns>The subdomain if it exist, otherwise null.</returns>
        public static string GetSubDomain(Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                if (host.Split('.').Length > 2)
                {
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);
                    return host.Substring(0, index);
                }
            }

            return null;
        }
        public static void ResponseGoBack()
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("<script>window.history.go(-1);</script>");
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 根据当前的时间和文件扩展名生成文件名
        /// </summary>
        /// <param name="fileExt">文件扩展名 带.</param>
        /// <returns></returns>
        public static string GenerateFileName(string fileExt)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfffff") + new Random().Next(1, 99).ToString() + fileExt;
        }
        public static string GenerateFileName(string fileExt, string suffix)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfffff") + new Random().Next(1, 99).ToString() + "_" + suffix + fileExt;
        }



        public static string GetQQ(string qq)
        {
            return GetQQ(qq, string.Empty);
        }

        /// <summary>
        /// 输出QQ链接
        /// </summary>
        /// <param name="qq">QQ号码</param>
        /// <param name="s">提示文字</param>
        /// <returns></returns>
        public static string GetQQ(string qq, string s)
        {
            string tmp = string.Empty;
            if (!String.IsNullOrEmpty(qq))
            {
                tmp = string.Format("<a href=\"tencent://message/?websitename=qzone.qq.com&menu=yes&uin={0}\" title=\"在线即时交谈\"><img src=\"/images/qqicon.gif\" border=\"0\">{1}</a>", qq, s);
            }
            return tmp;
        }

        /// <summary>
        /// 根据MQ号码获取大图片的MQ洽谈
        /// </summary>
        /// <param name="mq"></param>
        public static string GetBigImgMQ(string mq)
        {
            string Result = string.Empty;
            if (!string.IsNullOrEmpty(mq))
            {
                //Result = string.Format("<a href=\"javascript:void(0)\" style=\"vertical-align:middle;\" onclick=\"window.open('http://im.tongye114.com:9000/webmsg.cgi?version=1&amp;uid={0}')\" title=\"点击MQ图标洽谈！\"><img src='{1}/images/mqonline.gif' /></a>", mq, Domain.ServerComponents);
            }
            return Result;
        }
        /// <summary>
        /// 根据MQ号码获取大图片的MQ洽谈2
        /// </summary>
        /// <param name="mq"></param>
        /// <returns></returns>
        public static string GetBigImgMQ2(string mq)
        {
            string Result = string.Empty;
            if (!string.IsNullOrEmpty(mq))
            {
                //Result = string.Format("<a href=\"javascript:void(0)\" style=\"vertical-align:middle;\" onclick=\"window.open('http://im.tongye114.com:9000/webmsg.cgi?version=1&amp;uid={0}')\" title=\"点击MQ图标洽谈！\"><img src='{1}/images/jipiao/MQ-online.jpg' /></a>", mq, Domain.ServerComponents);
            }
            return Result;
        }

        /// <summary>
        /// 将英文星期几转化为中文星期几
        /// </summary>
        /// <param name="DayOfWeek"></param>
        /// <returns></returns>
        public static string ConvertWeekDayToChinese(DateTime time)
        {
            string DayOfWeek = time.DayOfWeek.ToString();
            switch (DayOfWeek)
            {
                case "Monday":
                    DayOfWeek = "周一";
                    break;
                case "Tuesday":
                    DayOfWeek = "周二";
                    break;
                case "Wednesday":
                    DayOfWeek = "周三";
                    break;
                case "Thursday":
                    DayOfWeek = "周四";
                    break;
                case "Friday":
                    DayOfWeek = "周五";
                    break;
                case "Saturday":
                    DayOfWeek = "周六";
                    break;
                case "Sunday":
                    DayOfWeek = "周日";
                    break;
                default:
                    break;
            }
            return DayOfWeek;
        }
        /// <summary>
        /// 如果指定的字符串的长度超过了maxLength，则截取
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetText(string text, int maxLength)
        {
            return GetText(text, maxLength, false);
        }
        /// <summary>
        ///  如果指定的字符串的长度超过了maxLength，则截取
        /// </summary>
        /// <param name="text">要截取的字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="isShowEllipsis">是否在字符串结尾显示省略号</param>
        /// <returns></returns>
        public static string GetText(string text, int maxLength, bool isShowEllipsis)
        {
            if (String.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            else
            {
                if (text.Length >= maxLength)
                {
                    if (isShowEllipsis)
                    {
                        return text.Substring(0, maxLength) + "...";
                    }
                    else
                    {
                        return text.Substring(0, maxLength);
                    }
                }
                else
                {
                    return text;
                }
            }
        }
        /// <summary>
        /// 将字符串控制在指定数量的汉字以内，两个字母、数字相当于一个汉字，其他的标点符号算做一个汉字
        /// </summary>
        /// <param name="text">要控制的字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="isShowEllipsis">是否在字符串结尾添加【...】</param>
        /// <returns></returns>
        public static string GetText2(string text, int maxLength, bool isShowEllipsis)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            double mlength = (double)maxLength;
            if (text.Length <= mlength)
            {
                return text;
            }
            System.Text.StringBuilder strb = new System.Text.StringBuilder();

            char c;
            for (int i = 0; i < text.Length; i++)
            {
                if (mlength > 0)
                {
                    c = text[i];
                    strb.Append(c);
                    mlength = mlength - GetCharLength(c);
                }
                else
                {
                    break;
                }
            }
            if (isShowEllipsis)
                strb.Append("…");
            return strb.ToString();
        }
        /// <summary>
        /// 判断字符是否是中文字符
        /// </summary>
        /// <param name="c">要判断的字符</param>
        /// <returns>true:是中文字符,false:不是</returns>
        public static bool IsChinese(char c)
        {
            System.Text.RegularExpressions.Regex rx =
                new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5]$");
            return rx.IsMatch(c.ToString());
        }

        /// <summary>
        /// 判断是否英文字母或数字的C#正则表达式 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsNatural_Number(char c)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(c.ToString());
        }

        /// <summary>
        /// 获取字符长度,汉字为1，英文或数字0.5，其余为1
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static double GetCharLength(char c)
        {
            if (IsChinese(c) == true)
            {
                return 1;
            }
            else if (IsNatural_Number(c) == true)
            {
                return 0.5;
            }
            else
            {
                return 1;
            }
        }


        /// <summary>
        /// 获得字符串的字节长度
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public static int GetByteLength(string value)
        {
            int len = 0;
            if (string.IsNullOrEmpty(value))  //字符串为null或空
                return len;
            else
                return Encoding.Default.GetBytes(value).Length;
        }
        /// <summary>
        /// httpwebrequest 字符编码为utf-8
        /// </summary>
        /// <param name="requestUriString">Internet资源的URI</param>
        /// <returns></returns>
        public static string GetWebRequest(string requestUriString)
        {
            return GetWebRequest(requestUriString, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// httpwebrequest
        /// </summary>
        /// <param name="requestUriString">Internet资源的URI</param>
        /// <param name="encoding">System.Text.Encoding</param>
        /// <returns></returns>
        public static string GetWebRequest(string requestUriString, Encoding encoding)
        {
            StringBuilder responseHtml = new StringBuilder();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
                request.Timeout = 2000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                System.IO.Stream resStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(resStream, encoding);

                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);

                while (count > 0)
                {
                    string s = new String(read, 0, count);
                    responseHtml.Append(s);
                    count = readStream.Read(read, 0, 256);
                }

                resStream.Close();
            }
            catch { }

            return responseHtml.ToString();
        }
        /// <summary>
        /// 过滤小数后末尾的0，字符串处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FilterEndOfTheZeroDecimal(decimal value)
        {
            string result = value.ToString();
            return FilterEndOfTheZeroString(result);
        }
        /// <summary>
        /// 过滤小数后末尾的0，字符串处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FilterEndOfTheZeroString(string value)
        {
            if (value.Contains('.'))
            {
                value = Regex.Replace(value, @"(?<=\d)\.0+$|0+$", "", RegexOptions.Multiline);
            }
            return value;
        }
        public static string[] Split(string Content, string SplitString)
        {
            if ((Content != null) && (Content != string.Empty))
            {
                return Regex.Split(Content, SplitString, RegexOptions.IgnoreCase);
            }
            return new string[1];
        }

        #region
        /// <summary>
        /// 将字符串转换为整型数组
        /// </summary>
        /// <param name="strValue">字符串</param>
        /// <param name="space">分割符</param>
        /// <returns></returns>
        public static int[] GetIntArray(string strValue, string space)
        {
            if (string.IsNullOrEmpty(strValue) || string.IsNullOrEmpty(space))
                return null;
            string[] strArray = null;
            int[] intArray = null;
            if (strValue != "")
            {
                strArray = strValue.TrimEnd(space.ToCharArray()).Split(space.ToCharArray());
                if (strArray != null && strArray.Length > 0)
                {
                    intArray = new int[strArray.Length];
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        intArray[i] = int.Parse(strArray[i]);
                    }
                }
            }
            return intArray;
        }
        /// <summary>
        /// 将用特殊符号分割的guid字符串转换为字符串数组
        /// </summary>
        /// <param name="strValue">字符串</param>
        /// <param name="space">分割符</param>
        /// <returns></returns>
        public static List<string> GetStringArray(string strValue, string space)
        {
            if (string.IsNullOrEmpty(strValue) || string.IsNullOrEmpty(space))
                return null;

            var strArray = strValue.TrimEnd(space.ToCharArray()).Split(space.ToCharArray());
            if (strArray.Length <= 0) return null;
            return strArray.Where(c => !string.IsNullOrEmpty(c)).ToList();
        }
        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemIndex">数据索引</param>
        /// <param name="recordSum">数据总数</param>
        /// <param name="TdCount">每个TR中TD的数量</param>
        /// <returns></returns>
        public static string IsOutTrOrTd(int itemIndex, int recordSum, int TdCount)
        {
            //先判断当前itemIndex是否是最后一条数据
            if ((itemIndex + 1) == recordSum)
            {
                System.Text.StringBuilder strb = new System.Text.StringBuilder();
                //判断当前itemInex是否已经到一行的末尾(一行显示4个Td)
                if (((itemIndex + 1) % TdCount) == 0)
                {
                    strb.Append("</tr>");
                }
                else
                {
                    int leaveTdCount = (TdCount - ((itemIndex + 1) % TdCount));
                    for (int i = 0; i < leaveTdCount; i++)
                    {
                        if (i + 1 <= leaveTdCount)
                        {
                            strb.Append("<td align='center'>&nbsp;</td>");
                        }
                    }
                    strb.Append("</tr>");
                }

                return strb.ToString();
            }
            //判断当前itemInex是否已经到一行的末尾(一行显示4个Td)
            else if (((itemIndex + 1) % TdCount) == 0)
            {
                return "</td><tr>";
            }
            else
            {
                return "</td>";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemIndex">数据索引</param>
        /// <param name="recordSum">数据总数</param>
        /// <param name="TdCount">每个TR中TD的数量</param>
        /// <param name="groupTdNum">每组td数</param>
        /// <returns></returns>
        public static string IsOutTrOrTd(int itemIndex, int recordSum, int TdCount, int groupTdNum)
        {
            //先判断当前itemIndex是否是最后一条数据
            if ((itemIndex + 1) == recordSum)
            {
                System.Text.StringBuilder strb = new System.Text.StringBuilder();
                //判断当前itemInex是否已经到一行的末尾(一行显示4个Td)
                if (((itemIndex + 1) % TdCount) == 0)
                {
                    strb.Append("</tr>");
                }
                else
                {
                    int leaveTdCount = (TdCount - ((itemIndex + 1) % TdCount));
                    for (int i = 0; i < leaveTdCount * groupTdNum; i++)
                    {
                        if (i + 1 <= leaveTdCount * groupTdNum)
                        {
                            strb.Append("<td align='center'>&nbsp;</td>");
                        }
                    }
                    strb.Append("</tr>");
                }

                return strb.ToString();
            }
            //判断当前itemInex是否已经到一行的末尾(一行显示4个Td)
            else if (((itemIndex + 1) % TdCount) == 0)
            {
                return "</td><tr>";
            }
            else
            {
                return "</td>";
            }
        }

        #region 设置cookie

        /// <summary>
        /// 设置cookie
        /// </summary>
        public static void SetCookie(string key, string value)
        {
            HttpContext.Current.Response.Cookies.Remove(key);
            HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);

            //Add Cookie.
            //LongTimeUserName_Cookie.
            HttpCookie cookies = new HttpCookie(key);
            cookies.Value = HttpUtility.UrlEncode(value, System.Text.Encoding.UTF8);
            HttpContext.Current.Response.AppendCookie(cookies);
        }
        #endregion

        /// <summary>
        /// 根据key键值 在【key1=value1&key2=value2】格式的字符串中获取对应的Value.
        /// </summary>
        /// <param name="url">url字符串</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetFromQueryStringByKey(string url, string key)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            Regex re = new Regex(key + @"\=([^\&\?]*)");
            string result = re.Match(url).Value;
            if (result != string.Empty)
            {
                result = result.Substring(key.Length + 1);
            }

            return result;
        }

        /// <summary>
        /// 根据TD在表格中的索引位置，返回对应的Css Class
        /// 该方法主要用于为嵌套中的Table中的TD定义样式名，一般在打印单的嵌套表格中使用。
        /// </summary>
        /// <param name="tdIndex">当前TD在表格中的索引位置，从1开始</param>
        /// <param name="tdCountPerTr">该表格每行显示的td数量</param>
        /// <param name="totalTdCount">该表格的总TD数量</param>
        /// <returns></returns>
        public static string GetTdClassNameInNestedTableByIndex(int tdIndex, int tdCountPerTr, int totalTdCount)
        {
            string className = "";
            int rowIndex = (int)Math.Ceiling((double)tdIndex / (double)tdCountPerTr);//当前td所在行
            int rowCount = totalTdCount / tdCountPerTr;//表格的行数
            bool isLastOneInRow = tdIndex % tdCountPerTr == 0 ? true : false;//指定是否是行中最后一个TD
            bool isLastRow = rowIndex == rowCount ? true : false;//指定是否是最后一行

            //根据td位置返回对应的css class name.
            if (isLastRow == false && isLastOneInRow == false)//不在最后一行，也不在一行中的最后一个。
            {
                className = "td_r_b_border";
            }
            else if (isLastRow == false && isLastOneInRow == true)//不在最后一行，但是是一行中的最后一个。
            {
                className = "td_b_border";
            }
            else if (isLastRow == true && isLastOneInRow == false)//在最后一行，也不是一行中的最后一个。
            {
                className = "td_r_border";
            }
            else if (isLastRow == true && isLastOneInRow == true)//在最后一行，也是一行中的最后一个。
            {
                className = "";
            }

            return className;
        }

        /// <summary>
        /// 将字符串数组转化成整型数组
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int[] ConvertToIntArray(string[] source)
        {
            int[] to = new int[source.Length];
            for (int i = 0; i < source.Length; i++)//将全部的数字存到数组里。
            {
                if (!string.IsNullOrEmpty(source[i].ToString()))
                {
                    to[i] = Utils.GetInt(source[i].ToString());
                }
            }
            if (to[0] == 0)
            {
                return null;
            }
            return to;
        }

        /// <summary>
        /// 将字符串(数字间用逗号间隔)转化成整型数组
        /// </summary>
        /// <param name="s">输入字符串(数字间用逗号间隔)</param>
        /// <returns></returns>
        public static int[] ConvertToIntArray(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;

            return ConvertToIntArray(s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// 根据计划状态设置 是否可以被修改删除
        /// </summary>
        /// <param name="planState"></param>
        /// <returns></returns>
        public static bool PlanIsUpdateOrDelete(string planState)
        {
            if (planState == "财务核算" || planState == "核算结束")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 根据指定的文件扩展名获取相应的文件MIME类型
        /// </summary>
        /// <param name="fileExtension">文件扩展名,带.</param>
        /// <returns>文件MIME类型</returns>
        public static string GetMimeTypeByFileExtension(string fileExtension)
        {
            string mime = "";
            fileExtension = fileExtension.ToLower();
            switch (fileExtension)
            {
                case ".gif":
                    mime = "image/gif";
                    break;
                case ".png":
                    mime = "image/png image/x-png";
                    break;
                case ".jpeg":
                    mime = "image/jpeg";
                    break;
                case ".jpg":
                    mime = "image/pjpeg";
                    break;
                case ".bmp":
                    mime = "image/bmp";
                    break;
                case ".xls":
                case ".xlsx":
                    mime = "application/vnd.ms-excel";
                    break;
            }
            return mime;
        }

        /// <summary>
        /// get enum value
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="s">转换的字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int? GetEnumValueNull(Type enumType, string s, int? defaultValue)
        {
            int? _enum = GetIntNull(s, null);
            if (!_enum.HasValue) return defaultValue;

            if (!Enum.IsDefined(enumType, _enum)) return defaultValue;

            return _enum;
        }

        /// <summary>
        /// get enum value
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="s">转换的字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int? GetEnumValueNull(Type enumType, string s)
        {
            return GetEnumValueNull(enumType, s, null);
        }


        /// <summary>
        /// 创建脚本标记
        /// </summary>
        /// <param name="s">脚本内容</param>
        /// <returns></returns>
        public static string CreateScriptTags(string s)
        {
            return string.Format(@"<script type=""text/javascript"">
//<![CDATA[
{0}//]]>
</script>", s);
        }

        /// <summary>
        /// register client script
        /// </summary>
        /// <param name="s">script</param>
        /// <param name="ltr">literal</param>
        /// <returns></returns>
        public static void RegisterClientScript(string s, System.Web.UI.WebControls.Literal ltr)
        {
            ltr.Text = CreateScriptTags(s);
        }

        /// <summary>
        /// Response.Clear(),Response.Write(),Response.End()
        /// </summary>
        /// <param name="s">要写入 HTTP 输出流的字符串</param>
        public static void RCWE(string s)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.Write(s);
            response.End();
        }

        /// <summary>
        /// textarea to html
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TextareaToHTML(object s)
        {
            if (s == null) return string.Empty;

            return StringValidate.TextToHtml(s.ToString());
        }


    }
    #endregion

}
