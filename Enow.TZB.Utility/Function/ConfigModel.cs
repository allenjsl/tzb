using System;
namespace Enow.TZB.Utility
{
	/// <summary>
	/// Web.config ������
	///  Copyright (C) 2006-2008 ChenZhiRen(Adpost) All Right Reserved.
	/// ����Ϊ���ɼ̳���
	/// typeof(System.Configuration.NameValueFileSectionHandler).Assembly.FullName.ToString()
	/// </summary>
	public sealed class ConfigClass
	{
		/// <summary>
		/// ȡ�������ļ��е��ַ���KEY
		/// </summary>
		/// <param name="SectionName">�ڵ�����</param>
		/// <param name="key">KEY��</param>
		/// <returns>����KEYֵ</returns>
		public static string GetConfigString(string SectionName,string key)
		{
			if(SectionName==null || SectionName == "")
			{
				System.Collections.Specialized.NameValueCollection cfgName = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationSettings.GetConfig("appSettings");
				if(cfgName[key] == null || cfgName[key] == "")
				{
					//throw (new Exception("��Web.config�ļ���δ����������: \"" + key.ToString() + "\""));
					return "";
				}
				else
				{
					return cfgName[key];
				}
			}
			else
			{
				System.Collections.Specialized.NameValueCollection cfgName = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationSettings.GetConfig(SectionName);
				if(cfgName[key] == null || cfgName[key] == "")
				{
					//throw (new Exception("��Web.config�ļ���δ����������: \"" + key.ToString() + "\""));
					return "";
				}
				else
				{
					return cfgName[key];
				}
			}
		}
		/// <summary>
		/// ȡ��Ĭ�Ͻڵ������
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetConfigString(string key)
		{
				System.Collections.Specialized.NameValueCollection cfgName = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationSettings.GetConfig("appSettings");
				if(cfgName[key] == null || cfgName[key] == "")
				{
					//throw (new Exception("��Web.config�ļ���δ����������: \"" + key.ToString() + "\""));
					return "";
				}
				else
				{
					return cfgName[key];
				}
		}
  
		/// <summary>
		/// �õ������ļ��е�����decimal��Ϣ
		/// </summary>
		/// <param name="SectionName">�ڵ�����</param>
		/// <param name="key">KEY����</param>
		/// <returns>���ظ�����</returns>
		public static decimal GetConfigDecimal(string SectionName,string key)
		{
			decimal result = 0;
			string cfgVal = GetConfigString(SectionName,key);
			if(null != cfgVal && string.Empty != cfgVal)
			{
				result = Convert.ToDecimal(cfgVal);
				/*
				try
				{
				result = decimal.Parse(cfgVal);
				}
				catch(FormatException)
				{
				  //Ignore format exceptions.
				}
				*/
			}

			return result;
		}
		/// <summary>
		/// ȡ�������ļ��� Ĭ�Ͻڵ�� ��������
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static decimal GetConfigDecimal(string key)
		{
			decimal result = 0;
			string cfgVal = GetConfigString(key);
			if(null != cfgVal && string.Empty != cfgVal)
			{
				result = Convert.ToDecimal(cfgVal);
				/*
				try
				{
				result = decimal.Parse(cfgVal);
				}
				catch(FormatException)
				{
				  //Ignore format exceptions.
				}
				*/
			}

			return result;
		}
		/// <summary>
		/// �õ������ļ��е�����int��Ϣ
		/// </summary>
		/// <param name="SectionName">�ڵ�����</param>
		/// <param name="key">KEY��</param>
		/// <returns>��������</returns>
		public static int GetConfigInt(string SectionName,string key)
		{
			int result = 0;
			string cfgVal = GetConfigString(SectionName,key);
			if(null != cfgVal && string.Empty != cfgVal)
			{
				result = Convert.ToInt32(cfgVal);
				/*
				 try
				{
				result = Int32.Parse(cfgVal);
				}
				catch(FormatException)
				{
				  //Ignore format exceptions.
				}
				*/
			}

			return result;
		}
		/// <summary>
		/// �õ������ļ��е�Ĭ�Ͻڵ�����int��Ϣ
		/// </summary>
		/// <param name="key">KEY��</param>
		/// <returns>��������</returns>
		public static int GetConfigInt(string key)
		{
			int result = 0;
			string cfgVal = GetConfigString(key);
			if(null != cfgVal && string.Empty != cfgVal)
			{
				result = Convert.ToInt32(cfgVal);
				/*
				 try
				{
				result = Int32.Parse(cfgVal);
				}
				catch(FormatException)
				{
				 // Ignore format exceptions.
				}
				*/
			}

			return result;
		}
		/// <summary>
		/// д��,���������ļ��ڵ�
		/// </summary>
		/// <param name="SectionName">�ڵ�����</param>
		/// <parma name="key">����</param>
		/// <parma name="value">��ֵ</param>
		public static void SetConfigKeyValue(string SectionName,string key,string keyvalue)
		{
			//���������ļ�
			System.Xml.XmlDocument doc = loadConfigDocument();
			//����ȡ�� �ڵ���
			System.Xml.XmlNode node= doc.SelectSingleNode("//" + SectionName);
			if (node == null)
				throw new InvalidOperationException(SectionName + " section not found in config file.");

			try
			{
				// �� 'add'Ԫ�� ��ʽ���Ƿ�������� 
				// select the 'add' element that contains the key
				System.Xml.XmlElement elem = (System.Xml.XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));

				if (elem != null)
				{
					//�޸Ļ���Ӽ�ֵ
					elem.SetAttribute("value", keyvalue);
				}
				else
				{
					//���û�з��ּ��������������ü����ͼ�ֵ
					elem = doc.CreateElement("add");
					elem.SetAttribute("key", key);
					elem.SetAttribute("value", keyvalue); 
					node.AppendChild(elem);
				}
				doc.Save(getConfigFilePath());
			}
			catch
			{
				throw;
			}

		}
		/// <summary>
		/// д��,���������ļ�Ĭ�Ͻڵ�
		/// </summary>
		/// <parma name="key">����</param>
		/// <parma name="value">��ֵ</param>
		public static void SetConfigKeyValue(string key,string keyvalue)
		{
			//���������ļ�
			string SectionName = "appSettings";
			System.Xml.XmlDocument doc = loadConfigDocument();
			//����ȡ�� �ڵ���
			System.Xml.XmlNode node= doc.SelectSingleNode("//" + SectionName);
			if (node == null)
				throw new InvalidOperationException(SectionName + " section not found in config file.");

			try
			{
				// �� 'add'Ԫ�� ��ʽ���Ƿ�������� 
				// select the 'add' element that contains the key
				System.Xml.XmlElement elem = (System.Xml.XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));

				if (elem != null)
				{
					//�޸Ļ���Ӽ�ֵ
					elem.SetAttribute("value", keyvalue);
				}
				else
				{
					//���û�з��ּ��������������ü����ͼ�ֵ
					elem = doc.CreateElement("add");
					elem.SetAttribute("key", key);
					elem.SetAttribute("value", keyvalue); 
					node.AppendChild(elem);
				}
				doc.Save(getConfigFilePath());
			}
			catch
			{
				throw;
			}

		}
		/// <summary>
		/// ɾ�������ļ��ڵ�
		/// </summary>
		/// <param name="SectionName">������</param>
		/// <param name="key">Ҫɾ���ļ�</param>
		public static void RemoveSectionKey(string SectionName,string key)
		{
			//���������ļ�
			System.Xml.XmlDocument doc = loadConfigDocument();

			//����ȡ�� �ڵ���
			System.Xml.XmlNode node= doc.SelectSingleNode("//" + SectionName);

			try
			{
				if (node == null)
					throw new InvalidOperationException(SectionName + " section not found in config file.");
				else
				{
					// �� 'add' ������ʽ key��value
					node.RemoveChild(node.SelectSingleNode(string.Format("//add[@key='{0}']", key)));
					doc.Save(getConfigFilePath());
				}
			}
			catch (NullReferenceException e)
			{
				throw new Exception(string.Format("The key {0} does not exist.", key), e);
			}
		}
		/// <summary>
		/// ɾ��Ĭ�Ͻڵ�
		/// </summary>
		/// <param name="key"></param>
		public static void RemoveSectionKey(string key)
		{
			string SectionName = "appSettings";
			//���������ļ�
			System.Xml.XmlDocument doc = loadConfigDocument();

			//����ȡ�� �ڵ���
			System.Xml.XmlNode node= doc.SelectSingleNode("//" + SectionName);

			try
			{
				if (node == null)
					throw new InvalidOperationException(SectionName + " section not found in config file.");
				else
				{
					// �� 'add' ������ʽ key��value
					node.RemoveChild(node.SelectSingleNode(string.Format("//add[@key='{0}']", key)));
					doc.Save(getConfigFilePath());
				}
			}
			catch (NullReferenceException e)
			{
				throw new Exception(string.Format("The key {0} does not exist.", key), e);
			}
		}
		/// <summary>
		/// ���������ļ�
		/// </summary>
		private static System.Xml.XmlDocument loadConfigDocument()
		{
			System.Xml.XmlDocument doc = null;
			try
			{
				doc = new System.Xml.XmlDocument();
				doc.Load(getConfigFilePath());
				return doc;
			}
			catch (System.IO.FileNotFoundException e)
			{
				throw new Exception("No configuration file found.", e);
			}
		}
		/// <summary>
		/// ȡ�����ļ�·��������
		/// </summary>
		private static string getConfigFilePath()
		{
			return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			//return Assembly.GetExecutingAssembly().Location + ".config";
		}

  
	}
}

