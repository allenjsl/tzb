using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text;

namespace Enow.TZB.Utility
{
    /// <summary>
    /// WEB��ʾ
    /// </summary>
    public abstract class MessageBox
    {
        #region ���� ����
        /// <summary>
        /// ��model �����з���ֵ����
        /// </summary>
        /// <param name="FunctionName">��������</param>
        /// <param name="VarName">���ر�����</param>
        /// <param name="OpenUrl">�򿪴���ĵ�ַ</param>
        /// <param name="WindowName">��������</param>
        /// <param name="WindowArg">�������</param>
        public static void showModalDialog(string FunctionName, string VarName, string OpenUrl, string WindowArg)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<script language=\"javascript\">\n");
            //str.Append("///<!--	����ģʽ����	-->\n");
            str.Append("function " + FunctionName + "(){\n");
            str.Append("var " + VarName + " = window.showModalDialog(\"" + OpenUrl + "\",window,\"" + WindowArg + "\");\n");
            str.Append("}\n");
            str.Append("</script>");
            string srn = Guid.NewGuid().ToString(); 
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
                cs.RegisterClientScriptBlock(typeof(string),srn, str.ToString());
        }
        /// <summary>
        /// ģʽ����
        /// </summary>
        /// <param name="FunctionName">��������</param>
        /// <param name="OpenUrl">����ַ</param>
        /// <param name="WindowArg">�������</param>
        public static void showModalDialog(string FunctionName, string OpenUrl, string WindowArg)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<script language=\"javascript\">\n");
            //str.Append("///<!--	����ģʽ����	-->\n");
            str.Append("function " + FunctionName + "(){\n");
            str.Append("window.showModalDialog(\"" + OpenUrl + "\",window,\"" + WindowArg + "\");\n");
            str.Append("}\n");
            str.Append("</script>");
            string srn = Guid.NewGuid().ToString(); 
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterClientScriptBlock(typeof(string), srn, str.ToString());
            }
        }
        /// <summary>
        /// ģʽ����
        /// </summary>
        /// <param name="FunctionName">��������</param>
        public static void showModalDialog(string FunctionName)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<script language=\"javascript\">\n");
            //str.Append("///<!--	����ģʽ����	-->\n");
            str.Append("function " + FunctionName + "(OpenUrl,WindowArg){\n");
            str.Append("var OpenUrl=OpenUrl;\nvar WindowArg = WindowArg;\n");
            str.Append("window.showModalDialog(OpenUrl,window,WindowArg);\n");
            str.Append("}\n");
            str.Append("</script>");
            string srn = Guid.NewGuid().ToString(); 
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterClientScriptBlock(typeof(string), srn, str.ToString());
            }
        }
        /// <summary>
        /// ���´���
        /// </summary>
        /// <param name="FunctionName">������</param>
        public static void showOpenWindowJS(string FunctionName)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<script language=\"javascript\">\n");
            if (FunctionName != "")
            {
                str.Append("function " + FunctionName + "(theURL,winName,features,iWidth,iHeight){\n");
            }
            else
            {
                str.Append("function OpenNewWindow(theURL,winName,features,iWidth,iHeight){\n");
            }
            str.Append("var  iTop=(window.screen.height-iHeight)/2;\nvar  iLeft=(window.screen.width-iWidth)/2;\n");
            str.Append("if(features != \"\"){\n");
            str.Append("window.open(theURL,winName,features + \",width=\" + iWidth + \",height=\" + iHeight + \",top=\" + iTop + \",left=\" + iLeft + \"\");\n");
            str.Append("}else{\n");
            str.Append("window.open(theURL,winName,\"width=\" + iWidth + \",height=\" + iHeight + \",top=\" + iTop + \",left=\" + iLeft + \"\");\n");
            str.Append("}\n");
            str.Append("}\n");
            str.Append("</script>");
            string srn = Guid.NewGuid().ToString(); 
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterClientScriptBlock(typeof(string), srn, str.ToString());
            }
        }
        /// <summary>
        /// дjavascript ����
        /// </summary>
        /// <param name="script">javascript ����</param>
        public static void ResponseScript(string script)
        {
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, "<script language='javascript'>" + script + "</script>");
            }
        }
        /// <summary>
        /// javascript ��ʾ
        /// </summary>
        /// <param name="msg">��ʾ����</param>
        public static void Show(string msg)
        {
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, "<script language='javascript'>alert('" + msg.ToString() + "');</script>");
            }
        }
        /// <summary>
        /// ��ʾ��ʾ������
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="HistoryNo"></param>
        public static void ShowAndReturnBack(string msg)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.AppendFormat("alert('{0}');", msg);
            builder1.AppendFormat("history.back();");
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        /// ��ʾ��ʾ������
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="HistoryNo"></param>
        public static void ShowAndReturnBack(string msg, int HistoryNumber)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.AppendFormat("alert('{0}');", msg);
            builder1.AppendFormat("history.back({0});", HistoryNumber);
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString(); 
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        /// ��ʾ��ʾ��ˢ�µ�ǰ����
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="HistoryNo"></param>
        public static void ShowAndReload(string msg)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.AppendFormat("alert('{0}');", msg);
            builder1.AppendFormat("document.URL=location.href;");
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        /// ��ʾ��ʾ��ˢ�µ�ǰ����
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="HistoryNo"></param>
        public static void ShowAndParentReload(string msg)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.AppendFormat("alert('{0}');", msg);
            builder1.AppendFormat("window.parent.location.reload();");
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        ///	��ʾ��ת��
        /// </summary>
        /// <param name="msg">��ʾ����</param>
        /// <param name="url">ת���ַ</param>
        public static void ShowAndRedirect(string msg, string url)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.AppendFormat("alert('{0}');", msg);
            builder1.AppendFormat("window.location.href='{0}'", url);
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString(); 
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        /// ��ʾ��ת��
        /// </summary>
        /// <param name="msg">��ʾ����</param>
        /// <param name="url">ת���ַ</param>
        /// <param name="windowName">Ҫת��Ĵ�����</param>
        public static void ShowAndRedirect(string msg, string url, string windowName)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.AppendFormat("alert('{0}');", msg);
            builder1.AppendFormat(windowName + ".location.href='{0}'", url);
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString(); 
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        /// ��ʾ���رմ���
        /// </summary>
        /// <param name="msg">��ʾ����</param>
        public static void ShowAndClose(string msg)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.AppendFormat("alert('{0}');", msg);
            builder1.Append("window.close();");
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        /// ��ʾ���رմ���
        /// </summary>
        /// <param name="msg">��ʾ����</param>
        public static void ShowAndBoxClose(string msg, string iframeId)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.AppendFormat("alert('{0}');", msg);
            builder1.Append("parent.Boxy.getIframeDialog('" + iframeId + "').hide();");
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        /// ��ʾ���رմ���
        /// </summary>
        /// <param name="msg">��ʾ����</param>
        public static void ShowAndParentReloadAndBoxClose(string msg, string iframeId)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.AppendFormat("alert('{0}');", msg);
            builder1.Append("window.parent.location.reload();");
            builder1.Append("parent.Boxy.getIframeDialog('" + iframeId + "').hide();");
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        /// ��ʾ���رմ���
        /// </summary>
        /// <param name="msg">��ʾ����</param>
        public static void ShowAndSelfClose(string msg)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.AppendFormat("alert('{0}');", msg);
            builder1.Append("self.close();");
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        /// �رմ���
        /// </summary>
        public static void WindowClose()
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.Append("window.close();");
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="Control">�ؼ�</param>
        /// <param name="msg">��ʾ����</param>
        public static void ShowConfirm(System.Web.UI.WebControls.WebControl Control, string msg)
        {
            Control.Attributes.Add("onclick", "return confirm('" + msg + "');");
        }
        /// <summary>
        /// ��ַת��
        /// </summary>
        /// <param name="Control">�ؼ�</param>
        /// <param name="Url">��ַ</param>
        public static void LocationUrl(System.Web.UI.WebControls.WebControl Control, string Url)
        {
            Control.Attributes.Add("onclick", "window.location.href='" + Url + "';return false;");
        }
        /// <summary>
        /// ��ַת��
        /// </summary>
        /// <param name="Url"></param>
        public static void LocationUrl(string Url)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.Append("window.location.href='" + Url + "';");
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }

        /// <summary>
        /// ��ַת�� ��������
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="windowName"></param>
        public static void LocationUrl(string Url, string windowName)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<script language='javascript'>");
            builder1.Append(windowName + ".location.href='" + Url + "';");
            builder1.Append("</script>");
            string srn = Guid.NewGuid().ToString();
            ClientScriptManager cs = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
            if (!cs.IsStartupScriptRegistered(srn))
            {
                cs.RegisterStartupScript(typeof(string), srn, builder1.ToString());
            }
        }
        /// <summary>
        /// �ؼ������֤
        /// </summary>
        /// <param name="Control"></param>
        public static void ValidateDigitalControl(System.Web.UI.WebControls.WebControl Control,string _Event)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("if(isNaN(this.value)){alert('��������ȷ�Ľ��');event.returnValue=false;}");
            if (_Event != "")
            {
                builder1.Append(_Event);
            }
            Control.Attributes.Add("onchange", builder1.ToString());
            
        }        
        #endregion
    }
}
