using System;
using Mugen;
using LuaInterface;

//namespace Mugen
//{
public class LuaCnsConfig
	{
		private CNSConfig m_CnsConfig = new CNSConfig();
		
		public int CreateStateDef(string name)
		{
			int ret;
			if (!m_CnsConfig.CreateStateDef(name, out ret))
				ret = -1;
			return ret;
		}

	[NoToLuaAttribute]
	public bool LoadFromFile(string fileName)
	{
		if (string.IsNullOrEmpty(fileName))
			return false;
		return AppConfig.GetInstance().DoFile(fileName);
	}

	[NoToLuaAttribute]
	public CNSConfig CnsCfg
	{
		get
		{
			return m_CnsConfig;
		}
	}

		public CNSStateDef GetStateDef(int id)
		{
			CNSStateDef ret = m_CnsConfig.GetStateDef(id);
			return ret;
		}
		
	}
//}

