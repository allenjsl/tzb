using System;
using System.Web.UI;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace Enow.TZB.Utility.ExportPageSet
{
    /// <summary>
    /// ExporPageByBtn ��ժҪ˵����
    /// </summary>
    [DefaultProperty("Text"),
      ToolboxData("<{0}:ExporPageByBtn runat=server></{0}:ExporPageByBtn>")]
    public class ExporPageByBtn : System.Web.UI.WebControls.WebControl
    {
        private int _intRecordCount = 0, _CurrencyPage = 1, _intPageSize = 10, _LinkType = 3, index = 1;
        private string _prePic = "/images/shangyici.gif", _nextPic = "/images/xiayici.gif";
        private string _PageLinkURL = "", _CurrencyPageCssClass = "current", _LinkCssClass = "", _DivCurrencyPageCssClass = "yahoo2", _NextBtnText = "��һҳ", _PrevBtnText = "��һҳ";
        private NameValueCollection _UrlParams = new NameValueCollection();
        #region ȡ����Դ�е�ͼƬ����
        //Assembly ab = Assembly.GetExecutingAssembly();
        //string resName = 
        //_prePic = System.Drawing.

        #endregion ����
        #region model
        //��ҳ��С
        [Bindable(true), Category("Behavior"), DefaultValue(10)]
        public virtual int intPageSize
        {
            get
            {
                return _intPageSize;
            }

            set
            {
                _intPageSize = value;
            }
        }
        //�ܼ�¼��
        [Bindable(true), Category("Behavior"), DefaultValue(0)]
        public virtual int intRecordCount
        {
            get
            {
                return _intRecordCount;
            }

            set
            {
                _intRecordCount = value;
            }
        }
        //��ǰҳ����
        [Bindable(true), Category("Behavior"), DefaultValue(1)]
        public virtual int CurrencyPage
        {
            get
            {
                return _CurrencyPage;
            }

            set
            {
                _CurrencyPage = value;
            }
        }
        //��ǰ��ͼƬ
        [Bindable(true), Category("Behavior"), DefaultValue(1)]
        public virtual string PrePicture
        {
            get
            {
                return _prePic;
            }

            set
            {
                _prePic = value;
            }
        }
        //���ͼƬ
        [Bindable(true), Category("Behavior"), DefaultValue(1)]
        public virtual string NextPictrue
        {
            get
            {
                return _nextPic;
            }

            set
            {
                _nextPic = value;
            }
        }
        //��ҳ���ӵ�ַ
        [Bindable(true), Category("Behavior"), DefaultValue("")]
        public virtual string PageLinkURL
        {
            get
            {
                return _PageLinkURL;
            }

            set
            {
                _PageLinkURL = value;
            }
        }
        //��ҳ��ʾ��ʽ
        [Bindable(true), Category("Behavior"), DefaultValue(3)]
        public virtual int LinkType
        {
            get
            {
                return _LinkType;
            }

            set
            {
                _LinkType = value;
            }
        }
        //��ǰҳ������ʾCSS
        [Bindable(true), Category("Behavior"), DefaultValue("current")]
        public virtual string CurrencyPageCssClass
        {
            get
            {
                return _CurrencyPageCssClass;
            }

            set
            {
                _CurrencyPageCssClass = value;
            }
        }
        //���ְ�ť����DIV����ʾCSS
        [Bindable(true), Category("Behavior"), DefaultValue("yahoo2")]
        public virtual string DivCurrencyPageCssClass
        {
            get
            {
                return _DivCurrencyPageCssClass;
            }

            set
            {
                _DivCurrencyPageCssClass = value;
            }
        }

        //��һҳ(��)��ť������
        [Bindable(true), Category("Behavior"), DefaultValue("��һҳ")]
        ///��һҳ(��)��ť������
        public virtual string NextBtnText
        {
            get
            {
                return _NextBtnText;
            }

            set
            {
                _NextBtnText = value;
            }
        }

        //��һҳ(��)��ť������
        [Bindable(true), Category("Behavior"), DefaultValue("��һҳ")]
        public virtual string PrevBtnText
        {
            get
            {
                return _PrevBtnText;
            }

            set
            {
                _PrevBtnText = value;
            }
        }

        //������ʾ��CSS
        [Bindable(true), Category("Behavior"), DefaultValue("")]
        public virtual string LinkCssClass
        {
            get
            {
                return _LinkCssClass;
            }

            set
            {
                _LinkCssClass = value;
            }
        }
        public virtual NameValueCollection UrlParams
        {
            get
            {
                return _UrlParams;
            }

            set
            {
                _UrlParams = value;
            }
        }
        #endregion

        /// <summary> 
        /// ���˿ؼ����ָ�ָ�������������
        /// </summary>
        /// <param name="output"> Ҫд������ HTML ��д�� </param>
        protected override void Render(HtmlTextWriter output)
        {
            string retval = "", retval2 = "", retval3 = "", retval4 = "", tmpReturnValue = "";
            int intPageCount = 0, BasePage = 0, pageNumber = 0;
            string NumLinkClass = " class=\"" + _CurrencyPageCssClass + "\"";
            string NumDivLinkClass = " class=\"" + _DivCurrencyPageCssClass + "\"";
            _PageLinkURL = _PageLinkURL + BuildUrlString(_UrlParams);
            if (_intRecordCount % _intPageSize == 0)
            {
                intPageCount = Convert.ToInt32(_intRecordCount / _intPageSize);
            }
            else
            {
                intPageCount = Convert.ToInt32(_intRecordCount / _intPageSize) + 1;
            }

            if (intPageCount <= 1)
                return;

            //������ַ�ҳ
            BasePage = Convert.ToInt32((_CurrencyPage / 10) * 10);
            retval += "<div" + NumDivLinkClass + ">";
            retval2 += "<div" + NumDivLinkClass + ">";
            retval3 += "<div" + NumDivLinkClass + ">";
            retval4 += "<div" + NumDivLinkClass + ">";

            retval2 += " <span " + NumLinkClass + " >��" + intPageCount + "ҳ</span>";
            retval4 += "ÿҳ" + _intPageSize + "�� ��" + _intRecordCount + "����Ϣ ��" + intPageCount + "ҳ";

            if (CurrencyPage > 1)
            {
                retval += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((CurrencyPage - 1)) + "\">" + SafeRequest(PrevBtnText) + "</a>";
                retval2 += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((CurrencyPage - 1)) + "\">" + SafeRequest(PrevBtnText) + "</a>";
                retval4 += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((CurrencyPage - 1)) + "\">" + SafeRequest(PrevBtnText) + "</a>";
            }

            //�ں�һ�鰴ťǰ�����ǰһ�鰴ť�����������ť
            if (BasePage > 0)
            {
                index = -1;
                retval3 += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((BasePage - 9)) + "\">" + PrevBtnText + "</a>";
            }

            for (int i = index; i <= 10; i++)
            {
                pageNumber = BasePage + i;
                if (pageNumber > intPageCount)
                {
                    i = 11;
                }
                else
                {

                    if (pageNumber == _CurrencyPage)
                    {
                        retval += " <span " + NumLinkClass + " >" + pageNumber.ToString() + "</span>";
                        retval2 += " <span " + NumLinkClass + " >" + pageNumber.ToString() + "</span>";
                        retval3 += " <span " + NumLinkClass + ">" + pageNumber.ToString() + "</span>";
                        retval4 += " <span " + NumLinkClass + ">" + pageNumber.ToString() + "</span>";
                        //retval2 += "<span class=\"current\">" + pageNumber + "</span>";
                        //retval3 += "<span class=\"current\">" + pageNumber + "</span>";
                    }
                    else
                    {
                        retval += " <a href=\"" + _PageLinkURL + "Page=" + pageNumber.ToString() + "\">" + pageNumber.ToString() + "</a>";
                        retval2 += " <a href=\"" + _PageLinkURL + "Page=" + pageNumber.ToString() + "\">" + pageNumber.ToString() + "</a>";
                        retval3 += " <a href=\"" + _PageLinkURL + "Page=" + pageNumber.ToString() + "\">" + pageNumber.ToString() + "</a>";
                        retval4 += " <a href=\"" + _PageLinkURL + "Page=" + pageNumber.ToString() + "\">" + pageNumber.ToString() + "</a>";
                    }
                }
            }

            if (intPageCount > CurrencyPage)
            {
                retval += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((CurrencyPage + 1)) + "\">" + SafeRequest(NextBtnText) + "</a>";
                retval2 += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((CurrencyPage + 1)) + "\">" + SafeRequest(NextBtnText) + "</a>";
                retval4 += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((CurrencyPage + 1)) + "\">" + SafeRequest(NextBtnText) + "</a>";
            }

            if (intPageCount > pageNumber)
            {
                retval3 += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((BasePage + 11)) + "\">" + SafeRequest(NextBtnText) + "</a>";
            }

            retval2 += "</div>";
            retval3 += "</div>";
            retval4 += "</div>";

            switch (_LinkType)
            {
                case 1:
                    tmpReturnValue = retval;
                    break;
                case 2:
                    tmpReturnValue = retval2;
                    break;
                case 3:
                    tmpReturnValue = retval3;
                    break;
                case 4:
                    tmpReturnValue = retval4;
                    break;
                default:
                    tmpReturnValue = retval3;
                    break;
            }
            output.Write(tmpReturnValue);
        }

        //����URL����
        private string BuildUrlString(NameValueCollection urlParams)
        {
            NameValueCollection newCol = new NameValueCollection(urlParams);
            NameValueCollection col = new NameValueCollection();
            string[] newColKeys = newCol.AllKeys;
            int i;
            for (i = 0; i < newColKeys.Length; i++)
            {
                if (newColKeys[i] != null)
                {
                    newColKeys[i] = newColKeys[i].ToLower();
                }
            }
            StringBuilder sb = new StringBuilder();
            for (i = 0; i < newCol.Count; i++)
            {
                if (newColKeys[i] != "page")
                {
                    sb.Append(newColKeys[i]);
                    sb.Append("=");
                    sb.Append(Page.Server.UrlEncode(newCol[i]));
                    sb.Append("&");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// ����">"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string SafeRequest(string str)
        {
            if (str != null && str != string.Empty)
            {
                //str = str.Replace("'", "&#39");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
            }
            else
            {
                str = "";
            }
            return str;
        }
    }
}
