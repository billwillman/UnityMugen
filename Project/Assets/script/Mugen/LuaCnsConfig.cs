using System;
using Mugen;
using LuaInterface;

//namespace Mugen
//{
public class LuaCnsConfig
	{
		private CNSConfig m_CnsConfig = new CNSConfig();
        private CmdConfig m_CmdConfig = new CmdConfig();
        private LuaTable m_LuaClass = null;
		
		public int CreateStateDef(string name)
		{
			int ret;
			if (!m_CnsConfig.CreateStateDef(name, out ret))
				ret = -1;
			return ret;
		}

        public Cmd_Command CreateCmd(string name, string aiName = "")
        {
            return m_CmdConfig.CreateCommand(name, aiName);
        }

        public AI_Command CreateAICmd(string aiName, string commandName = "")
        {
            return m_CmdConfig.CreateAICommand(aiName, commandName);
        }

        public AI_Command GetAICommand(string aiName)
        {
            return m_CmdConfig.GetAICommand(aiName);
        }


        public bool HasStateDef
        {
            get
            {
                return m_CnsConfig.HasStateDef;
            }

        }

	[NoToLuaAttribute]
	public bool LoadFromFile(string fileName)
	{
		if (string.IsNullOrEmpty(fileName))
			return false;
		m_LuaClass =  AppConfig.GetInstance().DoFile<LuaTable>(fileName);
        return m_LuaClass != null;
	}

    [NoToLuaAttribute]
    public void Dispose()
    {
        if (m_LuaClass != null)
        {
            m_LuaClass.Dispose();
            m_LuaClass = null;
        }
    }

    [NoToLuaAttribute]
    public LuaTable NewLuaPlayer(PlayerDisplay display)
    {
        if (m_LuaClass  == null || display == null)
            return null;
        LuaTable ret = m_LuaClass.Invoke<LuaTable, LuaTable>("new", m_LuaClass);
        if (ret != null)
        {
            ret.Call<LuaTable, PlayerDisplay>("OnInit", ret, display);
        }
        return ret;
    }

	[NoToLuaAttribute]
	public CNSConfig CnsCfg
	{
		get
		{
			return m_CnsConfig;
		}
	}

    [NoToLuaAttribute]
    public CmdConfig CmdCfg
    {
        get
        {
            return m_CmdConfig;
        }
    }

		public CNSStateDef GetStateDef(int id)
		{
			CNSStateDef ret = m_CnsConfig.GetStateDef(id);
			return ret;
		}
		
	}
//}

