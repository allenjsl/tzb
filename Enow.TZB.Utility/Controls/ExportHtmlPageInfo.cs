using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Enow.TZB.Utility.ExportPageSet
{
	/// <summary>
	/// ExportHtmlPageInfo ��ժҪ˵����
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:ExportHtmlPageInfo runat=server></{0}:ExportHtmlPageInfo>")]
	public class ExportHtmlPageInfo : System.Web.UI.WebControls.WebControl
	{
		private int _intRecordCount = 0,_CurrencyPage = 1,_intPageSize = 10,_LinkType = 3;
		private string _PageLinkURL = "",_FileFxt = ".aspx",_CurrencyPageCssClass="RedFnt", _LinkCssClass="";
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
		//������ҳ���ӵ�ַ
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
		//�����ļ���׺��
		[Bindable(true), Category("Behavior"), DefaultValue(".aspx")]
		public virtual string FileFxt 
		{
			get
			{
				return _FileFxt;
			}

			set
			{
				_FileFxt = value;
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
		#endregion

		/// <summary> 
		/// ���˿ؼ����ָ�ָ�������������
		/// </summary>
		/// <param name="output"> �����ҳ���� </param>
		protected override void Render(HtmlTextWriter output)
		{
			string  retval = "", retval2 = "",tmpReutrnValue = "";
			int intPageCount = 0,BasePage = 0,pageNumber=0;
			string NumLinkClass = " class=\"" + _CurrencyPageCssClass + "\"";
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
				retval = "�� " + _CurrencyPage.ToString() + " ҳ/�� " + intPageCount.ToString() + " ҳ ";
			}
			else
			{
				retval = "�� " + _CurrencyPage.ToString() + " ҳ/�� 1 ҳ ";
			}
			if(_CurrencyPage <= 1)
			{
				retval = retval + "��ҳ ǰҳ ";
			}
			else
			{
				retval = retval + " <a href=\"" + _PageLinkURL + "1" + _FileFxt + "\">��ҳ</a>  <a href=\"" + _PageLinkURL + Convert.ToString(_CurrencyPage - 1) + _FileFxt + "\">ǰҳ</a>  ";
			}
			if(_CurrencyPage >= intPageCount)
			{
				retval = retval + " ��ҳ ĩҳ";
			}
			else
			{
				retval = retval + " <a href=\"" + _PageLinkURL + Convert.ToString(_CurrencyPage + 1) + _FileFxt + "\">��ҳ</a>  <a href=\"" + _PageLinkURL + intPageCount.ToString() + _FileFxt + "\">ĩҳ</a> ";
			}
			//������ַ�ҳ
			retval2 = retval2 + "<br>";
			BasePage = Convert.ToInt32((_CurrencyPage / 10) * 10);
			if(BasePage > 0)
			{
				retval2 = retval2 + " <a href=\"" + _PageLinkURL + Convert.ToString((BasePage - 9)) + _FileFxt + "\"" + NumLinkClass + ">&lt;&lt;</a>";
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
						retval2 = retval2 + " <span class=\"RedFnt\">" + pageNumber.ToString() + "</span>";
					}
					else
					{
						retval2 = retval2 + " <a href=\"" + _PageLinkURL + pageNumber.ToString() + _FileFxt + "\">" + pageNumber.ToString() + "</a>";
					}
				}
			}
			if(intPageCount > pageNumber)
			{
				retval2 = retval2 + " <a href=\"" + _PageLinkURL + Convert.ToString((BasePage + 11)) + _FileFxt + "\">&gt;&gt;</a><br>";
			}
			switch(_LinkType)
			{
				case 1:
					tmpReutrnValue = retval;
					break;
				case 2:
					tmpReutrnValue = retval2;
					break;
				default:
					tmpReutrnValue = retval + retval2;
					break;
			}
			output.Write(tmpReutrnValue);
		}
	}
}
