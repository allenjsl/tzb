using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Data.Linq.SqlClient;
using Enow.TZB.Model;
using Enow.TZB.Utility;

namespace Enow.TZB.BLL
{
	/// <summary>
    /// ����Ա�û���֤
    /// </summary>
    public class ManageUserAuth
	{
        #region ��������
        private static string _RedirectUrl = "/";
        /// <summary>
        /// ת���ַ
        /// </summary>
        public static string RedirectUrl
        {
            get { return _RedirectUrl; }
            set { _RedirectUrl = value; }
        }
        private const string _AuthorName = "ManageLogin";
        /// <summary>
        /// ���������֤��key
        /// </summary>
        public static string AuthorName
        {
            get { return _AuthorName; }
        }
        private const string _Key = "12$#@!#@5tr%u8wsfr543$,23ve7w%$#";
        /// <summary>
        /// ���������֤��key
        /// </summary>
        public static string Key
        {
            get { return _Key; }
        }
        private const string _IV = "!54~1)e74&%3+-q#";
        /// <summary>
        /// ���������֤��IV
        /// </summary>
        public static string IV
        {
            get { return _IV; }
        }
        #endregion
		#region ����Ա�û���½
        /// <summary>
        /// ����Ա��½��֤
        /// </summary>        
        /// <param name="UserName">�û���</param>
        /// <param name="Password">����</param>
        /// <returns>1:��½�ɹ� 2:������½  -1:�û��������벻��ȷ</returns>
        public static int UserLogin(string UserName, string Password)
        {
            using (FWDC rdc = new FWDC())
            {
                Enow.TZB.Utility.HashCrypto CrypTo = new Enow.TZB.Utility.HashCrypto();
                Password = CrypTo.MD5Encrypt(Password);
                CrypTo.Dispose();
                ManagerList model = rdc.ManagerList.FirstOrDefault(u => u.UserName == UserName && u.Password == Password && u.IsEnable ==true);
                if (model != null)
                {
                    #region ���µ�½��Ϣ
                    model.LoginCount = model.LoginCount + 1;
                    model.LastLoginIp = Enow.TZB.Utility.StringValidate.GetRemoteIP();
                    model.LastLoginTime = DateTime.Now;
                    rdc.SubmitChanges();
                    //д��½��־
                    string EventInfo = model.UserName + ",������"+ model.ContactName +",�� " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��½ϵͳ!";
                    EventLog.Add(new tbl_EventLog {
                        TypeId = (int)Model.EnumType.EventType.��½��־,
                        OperatorId = model.Id,
                        OperatorName = model.ContactName,
                        EventTitle = EventInfo,
                        EventInfo = EventInfo,
                        Ip = Enow.TZB.Utility.StringValidate.GetRemoteIP()
                    });
                    #endregion
                    WriteCookieManageInfo(model);
                    if (model.FieldId != "00000000-0000-0000-0000-000000000000")
                    {
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }
        /// <summary>
        /// ��֤�û����Ƿ����
        /// </summary>        
        /// <param name="UserName">�û���</param>
        /// <returns></returns>
        public static bool IsUserLogin(string UserName)
        {
            using (FWDC rdc = new FWDC())
            {
                ManagerList model = rdc.ManagerList.FirstOrDefault(u => u.UserName == UserName && u.IsEnable == true);
                if (model != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion        

        #region ��֤����Ա�û�Ȩ��
        /// <summary>
        /// ��֤����Ա�û�Ȩ��
        /// </summary>
        /// <param name="mp"></param>
        /// <returns></returns>
        public static bool CheckAdminAuth(int mp)
        {
            bool IsAuth = false;
            ManagerList model = GetManageUserModel();
            if (model != null)
            {
                string[] PermissionList = Enow.TZB.Utility.StringValidate.Split(model.PermissionList, ",");
                if (PermissionList.Contains(mp.ToString()))
                IsAuth = true;
            }
            return IsAuth;
        }
        #endregion

        #region ����Ա�û��˳�
        /// <summary>
        /// �û��˳�
        /// </summary>
        /// <param name="Url">�˳���ת����ַ</param>
        public static void Logout(string Url)
        {
            HttpContext.Current.Response.Cookies[AuthorName].Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Redirect(Url, true);
        }
        #endregion

        #region ����Ա�û���֤��ȡ���ѵ�½���û���Ϣ
        /// <summary>
        /// ��֤�û��Ƿ�ɹ���½
        /// </summary>
        public static void ManageLoginCheck()
        {
            ManagerList model = GetManageUserModel();
            if (model == null)
                HttpContext.Current.Response.Redirect(RedirectUrl, true);

        }
        /// <summary>
        /// ��֤�û��Ƿ�ɹ���½
        /// </summary>
        public static void SYLoginCheck()
        {
            ManagerList model = GetManageUserModel();
            if (model == null)
            {
                HttpContext.Current.Response.Redirect(RedirectUrl, true);
            }
            else { 
                if(String.IsNullOrWhiteSpace(model.FieldId) || model.FieldId == "0")
                    HttpContext.Current.Response.Redirect(RedirectUrl, true);
            }

        }
        /// <summary>
        /// ��֤�û��Ƿ�ɹ���½
        /// </summary>
        public static void ManageLoginUrlCheck()
        {
            ManagerList model = GetManageUserModel();
            if (model == null)
                HttpContext.Current.Response.Redirect(RedirectUrl, true);
            string[] PermissionList = StringValidate.Split(model.PermissionList, ",");
            string Url = HttpContext.Current.Request.ServerVariables["URL"];
            int PerId = InitMenu.GetChildMenuId(Url);
            if (PerId > 0)
            {
                if (!PermissionList.Contains(PerId.ToString())){
                    MessageBox.ShowAndRedirect("�Բ�����û�в�������Ŀ��Ȩ��", RedirectUrl);
                    return;
                }
            }
            else { MessageBox.ShowAndRedirect("�Բ�����û�в�������Ŀ��Ȩ��", RedirectUrl); return; }

        }
        /// <summary>
        /// ��֤�û��Ƿ�ɹ���½
        /// </summary>
        /// <returns>true:�ѵ�½ false:δ��½</returns>
        public static bool ManageIsLoginCheck()
        {
            ManagerList model = GetManageUserModel();
            if (model == null)
                return false;
            else
                return true;

        }
        /// <summary>
        /// ��֤�û��Ƿ�ɹ���½
        /// </summary>
        /// <param name="url">��ת��URL</param>
        public static void ManageLoginCheck(string url)
        {
            ManagerList model = GetManageUserModel();
            if (model == null)
                HttpContext.Current.Response.Redirect(url, true);
        }
        /// <summary>
        /// ȡ�õ�½�û���ƾ������
        /// </summary>
        /// <returns>���ص�½�û���ʵ��</returns>
        public static ManagerList GetManageUserModel()
        {
            HttpCookie hc = HttpContext.Current.Request.Cookies[AuthorName];
            if (hc != null)
            {
                try
                {
                    bool IsLoginState = true;
                    ManagerList model = new ManagerList();
                    Enow.TZB.Utility.HashCrypto crypto = new Enow.TZB.Utility.HashCrypto();
                    crypto.Key = Key;
                    crypto.IV = IV;
                    if (hc["Id"] != null && String.Empty != hc["Id"])
                    {
                        model.Id = int.Parse(crypto.DeRC2Encrypt(hc["Id"]));
                    }
                    else
                    {
                        IsLoginState = false;
                    }

                    if (hc["EmployeeId"] != null && string.Empty != hc["EmployeeId"])
                    {
                        model.EmployeeId = int.Parse(crypto.DeRC2Encrypt(hc["EmployeeId"]));
                    }
                    else
                    {
                        //IsLoginState = false;
                    }
                    if (hc["UserName"] != null && String.Empty != hc["UserName"])
                    {
                        model.UserName = crypto.DeRC2Encrypt(hc["UserName"]);
                    }
                    else
                    {
                        IsLoginState = false;
                    }
                    if (hc["ContactName"] != null && String.Empty != hc["ContactName"])
                    {
                        model.ContactName = crypto.DeRC2Encrypt(hc["ContactName"]);
                    }
                    if (hc["PermissionList"] != null && String.Empty != hc["PermissionList"])
                    {
                        model.PermissionList = crypto.DeRC2Encrypt(hc["PermissionList"]);
                    }
                    if (hc["FieldId"] != null && String.Empty != hc["FieldId"])
                    {
                        model.FieldId = crypto.DeRC2Encrypt(hc["FieldId"]);
                    }
                    if (hc["FieldName"] != null && String.Empty != hc["FieldName"])
                    {
                        model.FieldName = crypto.DeRC2Encrypt(hc["FieldName"]);
                    }
                    if (hc["IsAllCity"] != null && String.Empty != hc["IsAllCity"])
                    {
                        model.IsAllCity = crypto.DeRC2Encrypt(hc["IsAllCity"])=="1";
                    }
                    if (hc["CityList"] != null && String.Empty != hc["CityList"])
                    {
                        model.CityList = crypto.DeRC2Encrypt(hc["CityList"]);
                    }
                    if (IsLoginState)
                        return model;
                    else
                        return null;
                }
                catch { return null; }
            }
            else { return null; }
        }
        #endregion

        #region ��������û�ƾ֤
        /// <summary>
        /// �����û���½ƾ֤
        /// </summary>
        /// <param name="model">�û�ʵ��</param>
        /// <param name="cookieName">ƾ֤����</param>
        private static void WriteCookieManageInfo(ManagerList model)
        {
            Enow.TZB.Utility.HashCrypto CrypTo = new Enow.TZB.Utility.HashCrypto();
            CrypTo.Key = Key;
            CrypTo.IV = IV;
            HttpCookie Hc = new HttpCookie(AuthorName);
            Hc.Values.Add("Id", CrypTo.RC2Encrypt(model.Id.ToString()));
            Hc.Values.Add("UserName", CrypTo.RC2Encrypt(model.UserName));
            Hc.Values.Add("ContactName", CrypTo.RC2Encrypt(model.ContactName));
            Hc.Values.Add("EmployeeId", CrypTo.RC2Encrypt(model.EmployeeId.ToString()));
            Hc.Values.Add("IsAllCity", CrypTo.RC2Encrypt(model.IsAllCity?"1":"0"));
            if (!String.IsNullOrWhiteSpace(model.CityList))
                Hc.Values.Add("CityList", CrypTo.RC2Encrypt(model.CityList));
            else
                Hc.Values.Add("CityList","");
            //�򳡱��
            Hc.Values.Add("FieldId", CrypTo.RC2Encrypt(model.FieldId));
            //������
            if (!String.IsNullOrWhiteSpace(model.FieldName))
            {
                Hc.Values.Add("FieldName", CrypTo.RC2Encrypt(model.FieldName));
            }
            else
            {
                Hc.Values.Add("FieldName", "");
            }
            if (!String.IsNullOrEmpty(model.PermissionList))
            {
                Hc.Values.Add("PermissionList", CrypTo.RC2Encrypt(model.PermissionList));
              
            }
            else
            {
                Hc.Values.Add("PermissionList", "");
            }
            HttpContext.Current.Response.Cookies.Add(Hc);
            CrypTo.Dispose();
            CrypTo = null;
            model = null;
        }
        #endregion
	}
}