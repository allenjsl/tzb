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
	/// ��ҳ�ؼ�
	/// </summary>
	/// �޸���:��־��	�޸�ʱ��:2009-04-08
	/// �޸�����:��ҳ��ʽ3:�޸�Ϊ�������ķ�ҳ��ʽ.
	/// ---------------------------------------------------------------
	/// �޸���:��־��	�޸�ʱ��:2009-05-25
	/// �޸�����:��ҳ��ʽ3:�޸�Ϊ�������ķ�ҳ��ʽ ���޼�¼ʱ������ʾΪ��һҳ
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:ExportPageInfo runat=server></{0}:ExportPageInfo>")]
	public class ExportPageInfo : System.Web.UI.WebControls.WebControl
	{
		private int _intRecordCount = 0,_CurrencyPage = 1,_intPageSize = 10,_LinkType = 3;
		private string _prePic = "/images/shangyici.gif",_nextPic = "/images/xiayici.gif";
		private string _PageLinkURL = "",_CurrencyPageCssClass = "RedFnt", _LinkCssClass = "";
		private NameValueCollection _UrlParams=new NameValueCollection();
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
		[Bindable(true), Category("Behavior"), DefaultValue("RedFnt")]
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
		}//������ʾ��CSS
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
            string retval = "", retval2 = "",retval3 = "",tmpReturnValue = "";
            StringBuilder retdropval = new StringBuilder();
			int intPageCount = 0,BasePage = 0,pageNumber=0;
			string NumLinkClass = " class=\"" + _CurrencyPageCssClass + "\"";
			_PageLinkURL=_PageLinkURL+BuildUrlString(_UrlParams);
			if(_intRecordCount % _intPageSize == 0)
			{
				intPageCount = Convert.ToInt32(_intRecordCount / _intPageSize);
			}
			else
			{
				intPageCount = Convert.ToInt32(_intRecordCount / _intPageSize) + 1;
			}
			if(intPageCount>0)
			{
                retval = "ÿҳ " + _intPageSize + " ��/�� " + _intRecordCount + " ����¼ |  �� " + _CurrencyPage.ToString() + " ҳ/�� " + intPageCount.ToString() + " ҳ ";
                retdropval.Append("ÿҳ " + _intPageSize + " ��/�� " + _intRecordCount + " ����¼ |  ��<font color=\"green\">" + _CurrencyPage.ToString() + "</font>/<font color=\"red\">" + intPageCount.ToString() + "</font>ҳ ");
			}
			else
			{
                retval = "ÿҳ " + _intPageSize + " ��/�� " + _intRecordCount + " ����¼ |  �� " + _CurrencyPage.ToString() + " ҳ/�� 1 ҳ ";
                retdropval.Append("ÿҳ " + _intPageSize + " ��/�� " + _intRecordCount + " ����¼ |  ��<font color=\"green\">" + _CurrencyPage.ToString() + "</font>ҳ/��<font color=\"red\">1</font>ҳ ");
			}
			retval3 = retval;
			retval3 += " <a href=\"" + _PageLinkURL + "Page=1\">��ҳ</a>  ";
            if (_LinkType == 3) //3��ʱ��Ϊ������
            {
                retdropval.Append(" |  <select id=\"epi_ddlPageOption\" name=\"epi_ddlPageOption\" onchange='javascript:window.location.href=\"" + _PageLinkURL + "Page=\"+this.options[this.selectedIndex].value;'>");
                //������ҳ��ʼ
                if (intPageCount > 0)
                {
                    for (int i = 1; i <= intPageCount; i++)
                    {
                        if (_CurrencyPage != i)
                        {
                          retdropval.Append( "<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>");
                        }
                        else
                        {
                           retdropval.Append( "<option value=\"" + i.ToString() + "\" selected=\"selected\">" + i.ToString() + "</option>");
                        }
                    }
                }
                else
                {
                   retdropval.Append( "<option value=\"1\">1</option>");
                }
               retdropval.Append( "</select>");
                //������ҳ����
            }
			if(_CurrencyPage <= 1)
			{
				retval = retval + "��ҳ ��һҳ ";
				retdropval.Append( "��ҳ ��һҳ ");
			}
			else
			{
				retval = retval + " <a href=\"" + _PageLinkURL + "Page=1\">��ҳ</a>  <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString(_CurrencyPage - 1) + "\">��һҳ</a>  ";
				retdropval.Append(" <a href=\"" + _PageLinkURL + "Page=1\">��ҳ</a>  <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString(_CurrencyPage - 1) + "\">��һҳ</a>  ");
			}
			if(_CurrencyPage >= intPageCount)
			{
				retval = retval + " ��һҳ ĩҳ";
				retdropval.Append(" ��һҳ ĩҳ");
			}
			else
			{
				retval = retval + " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString(_CurrencyPage + 1) + "\">��һҳ</a>  <a href=\"" + _PageLinkURL + "Page=" + intPageCount.ToString() + "\">ĩҳ</a> ";
				retdropval.Append(" <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString(_CurrencyPage + 1) + "\">��һҳ</a>  <a href=\"" + _PageLinkURL + "Page=" + intPageCount.ToString() + "\">ĩҳ</a> ");
			}
			//������ַ�ҳ
			BasePage = Convert.ToInt32((_CurrencyPage / 10) * 10);
			if(BasePage > 0)
			{
				retval2 += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((BasePage - 9)) + "\"" + NumLinkClass + ">&lt;&lt;</a>";
                retval3 += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((BasePage - 9)) + "\"" + NumLinkClass + "><<</a>";//luofuxian--<img src=\"" + _prePic + "\" border=\"0\"></a>
			}
			for(int i = 1; i<=10; i++)
			{
				pageNumber = BasePage + i;
				if(pageNumber > intPageCount)
				{
					i = 11;
				}
				else
				{
					if(pageNumber == _CurrencyPage)
					{
						retval2 += " <span class=\"RedFnt\">" + pageNumber.ToString() + "</span>";
						retval3 += " <span class=\"RedFnt\">" + pageNumber.ToString() + "</span>";
					}
					else
					{
						retval2 += " <a href=\"" + _PageLinkURL + "Page=" + pageNumber.ToString() + "\">" + pageNumber.ToString() + "</a>";
						retval3 += " <a href=\"" + _PageLinkURL + "Page=" + pageNumber.ToString() + "\">" + pageNumber.ToString() + "</a>";
					}
				}
			}
			if(intPageCount > pageNumber)
			{
				retval2 += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((BasePage + 11)) + "\">&gt;&gt;</a>";
                retval3 += " <a href=\"" + _PageLinkURL + "Page=" + Convert.ToString((BasePage + 11)) + "\">>></a>";//luofuxian---<img src=\"" + _nextPic + "\" border=\"0\">
			}
			retval3 += "  <a href=\"" + _PageLinkURL + "Page=" + intPageCount.ToString() + "\">ĩҳ</a> ";
			switch(_LinkType)
			{
				case 1:
					tmpReturnValue = retval;
					break;
				case 2:
					tmpReturnValue = retval2;
					break;
				case 3:
					tmpReturnValue = retdropval.ToString();
					break;
				case 4:
					tmpReturnValue = retval3;
					break;
				default:
					tmpReturnValue = retval + "<br>" + retval2;
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
				if (newColKeys[i] != "page" )
				{					
					sb.Append(newColKeys[i]);
					sb.Append("=");				
					sb.Append(Page.Server.UrlEncode(newCol[i]));
					sb.Append("&");
				}
			}
			return sb.ToString();
		}
	}
}
