using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LuaInterface;

namespace Mugen
{

	public enum AI_Type
	{
		none,
		VarSet,
		ChangeState
	}

	public class AI_Command
	{
		public string name {
			get;
			set;
		}

		public AI_Type type
		{
			get;
			set;
		}

		public string value {
			get;
			set;
		}

        public bool AniLoop
        {
            get;
            set;
        }

		public string command
		{
			get;
			set;
		}

        public Func<LuaTable, string, bool> OnTriggerEvent
        {
            get;
            set;
        }

        [NoToLuaAttribute]
		public bool CanTrigger(PlayerDisplay display, string cmdName)
        {
			if (display == null)
				return false;
			if (OnTriggerEvent == null || display.LuaPly == null)
                return true;
			//display.LuaPly.Get
			return OnTriggerEvent(display.LuaPly, cmdName);
        }
	}

    // 用户按键映射
    public class Cmd_Remap: IConfigPropertys
    {

		public Cmd_Remap()
		{
			this.x = "x";
			this.y = "y";
			this.z = "z";
			this.a = "a";
			this.b = "b";
			this.c = "c";
		}

        public string ConfigName
        {
            get
            {
                return string.Empty;
            }
        }

        // name: old key, value: new key
        public string x
        {
            get;
            set;
        }

        public string y
        {
            get;
            set;
        }

        public string z
        {
            get;
            set;
        }

        public string a
        {
            get;
            set;
        }

        public string b
        {
            get;
            set;
        }

        public string c
        {
            get;
            set;
        }

		public string s
        {
            get;
            set;
        }
    }

    public class Cmd_Command
    {
        public string name
        {
            get;
            set;
        }

//#if UNITY_EDITOR
        [NoToLuaAttribute]
        public bool isEditorActive
        {
            get;
            set;
        }
//#endif

        [NoToLuaAttribute]
        // 按键组
        public string[] keyCommands
        {
            get;
            set;
        }

        public void AttachKeyCommands(string keys)
        {
            keyCommands = ConfigSection.Split(keys);
        }

        // 多久时间按下按键为有效（单位：帧）
        public int time
        {
            get;
            set;
        }

        public int buffer__time
        {
            get;
            set;
        }

		public string aiName {
			get;
			set;
		}
    }


	public class CmdConfig
	{
        private Cmd_Remap m_Remap = null;
        // 默认接受指令时间(>=1)
        private int m_Command__Time = 1;
        // 默认指令蓄力的时间(>=1 and <= 30)
        private int m_Command__Buffer__Time = 1;
        private Dictionary<string, Cmd_Command> m_CommandMap = null;
		private Dictionary<string, AI_Command> m_AICmdMap = null;

        // LUA可以调用
        public Cmd_Command CreateCommand(string name, string aiName = "")
        {
            if (string.IsNullOrEmpty(name))
                return null;
            Cmd_Command ret = GetCommand(name);
            if (ret != null)
                return ret;
            ret = new Cmd_Command();
            ret.name = name;
            ret.aiName = aiName;
            AddCommand(ret);
            return ret;
        }

        // LUA可以调用
        public AI_Command CreateAICommand(string aiName, string commandName = "")
        {
            if (string.IsNullOrEmpty(aiName))
                return null;
            if (string.IsNullOrEmpty(commandName))
                commandName = aiName;
            AI_Command ret;
            if (m_AICmdMap == null)
            {
                m_AICmdMap = new Dictionary<string, AI_Command>();
                ret = new AI_Command();
                ret.name = aiName;
                ret.command = commandName;
            } else
            {
                if (!m_AICmdMap.TryGetValue(aiName, out ret))
                {
                    ret = new AI_Command();
                    ret.name = aiName;
                    ret.command = commandName;
                } else
                {
                    ret.command = commandName;
                }
            }
            m_AICmdMap[aiName] = ret;
            return ret;
        }

        public Cmd_Command GetCommand(string name)
        {
            if (string.IsNullOrEmpty(name) || m_CommandMap == null)
                return null;
            Cmd_Command ret;
            if (!m_CommandMap.TryGetValue(name, out ret))
                ret = null;
            return ret;
        }

        public AI_Command GetAICommand(string aiName)
        {
            if (string.IsNullOrEmpty(aiName) || m_AICmdMap == null)
                return null;
            if (!string.IsNullOrEmpty(aiName))
            {
                AI_Command cmd;
                if (!m_AICmdMap.TryGetValue(aiName, out cmd))
                    cmd = null;
                return cmd;
            }

            return null;
        }

        [NoToLuaAttribute]
        // 自动找到一个合适的触发器
        public AI_Command GetAutoCheckAICommand(PlayerDisplay display, out bool mustCheckTrigger, string scriptCmdName = "")
        {
            mustCheckTrigger = true;
            if (m_AICmdMap == null)
                return null;
            if (display == null || display.LuaPly == null)
                return null;

            AI_Command finder = null;
            // 遍历所有条件，如果没有满足的再调用LUA的方法GetAIName
            var iter = m_AICmdMap.GetEnumerator();
            while (iter.MoveNext())
            {
                var aiCmd = iter.Current.Value;
                if (aiCmd != null && aiCmd.OnTriggerEvent != null)
                {
                    if (aiCmd.CanTrigger(display, scriptCmdName))
                    {
                        mustCheckTrigger = false;
                        finder = aiCmd;
                        break;
                    }
                }
            }
            iter.Dispose();
            if (finder != null)
                return finder;

            string aiName = display.Call_LuaPly_GetAIName(scriptCmdName);
            if (string.IsNullOrEmpty(aiName))
                return null;
            AI_Command cmd;
            if (!m_AICmdMap.TryGetValue(aiName, out cmd))
                cmd = null;
            return cmd;
        }

        [NoToLuaAttribute]
		public AI_Command GetAICommand(Cmd_Command command, PlayerDisplay display, out bool mustCheckTrigger)
		{
			mustCheckTrigger = true;
            if (command == null || m_AICmdMap == null)
				return null;
            string aiName = command.aiName;
            if (!string.IsNullOrEmpty(aiName))
            {
                AI_Command cmd;
                if (!m_AICmdMap.TryGetValue(aiName, out cmd))
                    cmd = null;
                return cmd;
            } else
            {
                if (display == null || display.LuaPly == null)
                    return null;

				AI_Command finder = null;
				// 遍历所有条件，如果没有满足的再调用LUA的方法GetAIName
				var iter = m_AICmdMap.GetEnumerator();
				while (iter.MoveNext ()) {
					var aiCmd = iter.Current.Value;
					if (aiCmd != null && aiCmd.OnTriggerEvent != null) {
						if (aiCmd.CanTrigger (display, command.name)) {
							mustCheckTrigger = false;
							finder = aiCmd;
							break;
						}
					}
				}
				iter.Dispose ();
				if (finder != null)
					return finder;

                aiName = display.Call_LuaPly_GetAIName(command.name);
                if (string.IsNullOrEmpty(aiName))
                    return null;
                AI_Command cmd;
                if (!m_AICmdMap.TryGetValue(aiName, out cmd))
                    cmd = null;
                return cmd;
            }
		}

        [NoToLuaAttribute]
        public Cmd_Command[] GetCommandArray()
        {
            if (m_CommandMap == null)
                return null;
            Cmd_Command[] ret = m_CommandMap.Values.ToArray();
            return ret;
        }

        protected void AddCommand(Cmd_Command command)
        {
            if (command == null)
                return;
            if (m_CommandMap == null)
                m_CommandMap = new Dictionary<string, Cmd_Command>();
            m_CommandMap[command.name] = command;
        }

        public int Command__Time
        {
            get
            {
                return m_Command__Time;
            }
        }

        public int Command__Buffer__Time
        {
            get
            {
                return m_Command__Buffer__Time;
            }
        }

		public void AttachRemap(string x, string y, string z, string a, string b, string c)
		{
			if (m_Remap == null)
				m_Remap = new Cmd_Remap ();
			m_Remap.x = x;
			m_Remap.y = y;
			m_Remap.z = z;
			m_Remap.a = a;
			m_Remap.b = b;
			m_Remap.c = c;
		}

        [NoToLuaAttribute]
        public Cmd_Remap reMap
        {
            get
            {
                return m_Remap;
            }
        }

		public string TransRemap(string input)
		{
			if (string.IsNullOrEmpty (input) || m_Remap == null)
				return input;
			if (string.Compare(m_Remap.x, input, true) == 0)
				return "x";
			else if (string.Compare(m_Remap.y, input, true) == 0)
				return "y";
			else if (string.Compare(m_Remap.z, input, true) == 0)
				return "z";
			else if (string.Compare(m_Remap.a, input, true) == 0)
				return "a";
			else if (string.Compare(m_Remap.b, input, true) == 0)
				return "b";
			else if (string.Compare(m_Remap.c, input, true) == 0)
				return "c";
			return input;
		}

        [NoToLuaAttribute]
		public bool LoadFromFile(string fileName)
		{
            Clear();
			if (string.IsNullOrEmpty (fileName))
				return false;
			string str = AppConfig.GetInstance ().Loader.LoadText (fileName);
            if (string.IsNullOrEmpty(str))
                return false;
			return LoadFromStr (str);
		}

        public void Clear()
        {
            m_Remap = null;
            m_Command__Buffer__Time = 1;
            m_Command__Time = 1;
            if (m_CommandMap != null)
                m_CommandMap.Clear();
        }

        [NoToLuaAttribute]
		public bool LoadFromStr(string str)
		{
            Clear();
			if (string.IsNullOrEmpty(str))
				return false;
			ConfigReader reader = new ConfigReader ();
			reader.LoadString (str);
			return LoadFromReader (reader);
		}

        [NoToLuaAttribute]
		public bool LoadFromReader(ConfigReader reader)
		{
            Clear();
			if (reader == null)
				return false;

            ConfigSection section = reader.GetSection("Remap");
            if (section != null)
            {
                m_Remap = new Cmd_Remap();
                if (!section.GetPropertysValues(m_Remap))
                    m_Remap = null;
            }

            section = reader.GetSection("Defaults");
            if (section != null)
            {
                for (int i = 0; i < section.ContentListCount; ++i)
                {
                    string key, value;
                    if (section.GetKeyValue(i, out key, out value))
                    {
                        if (string.Compare(key, "command.time", true) == 0)
                            m_Command__Time = int.Parse(value);
                        else if (string.Compare(key, "command.buffer.time", true) == 0)
                            m_Command__Buffer__Time = int.Parse(value);
                    }
                }
            }

            // 创建Command
            for (int i = 0; i < reader.SectionCount; ++i)
            {
                section = reader.GetSections(i);
                if (section == null)
                    continue;
				if (string.Compare (section.Tile, "Command", true) == 0) {
					Cmd_Command cmd = null;
					for (int j = 0; j < section.ContentListCount; ++j) {
						string key, value;
						if (section.GetKeyValue (j, out key, out value)) {
							if (string.Compare (key, "name", true) == 0) {
								if (cmd == null)
									cmd = new Cmd_Command ();
								cmd.name = value;
							} else if (string.Compare (key, "time", true) == 0) {
								if (cmd == null)
									cmd = new Cmd_Command ();
								cmd.time = int.Parse (value);
							} else if (string.Compare (key, "buffer.time", true) == 0) {
								if (cmd == null)
									cmd = new Cmd_Command ();
								cmd.buffer__time = int.Parse (value);
							} else if (string.Compare (key, "command", true) == 0) {
								if (cmd == null)
									cmd = new Cmd_Command ();
								cmd.keyCommands = ConfigSection.Split (value);
							}
						}
					}
					if (cmd != null && !string.IsNullOrEmpty (cmd.name))
						AddCommand (cmd);
				} else if (section.Tile.StartsWith ("State -1", StringComparison.CurrentCultureIgnoreCase)) {
					
					string[] names = ConfigSection.Split (section.Tile);
					if (names == null || names.Length < 2)
						continue;
					
					string aiName = names [1];
					AI_Type aiType = AI_Type.none;
					AI_Command aiCmd = null;

					for (int j = 0; j < section.ContentListCount; ++j) {
						string key, value;
						if (section.GetKeyValue (j, out key, out value)) {
							if (string.Compare (key, "type", true) == 0) {
								if (string.Compare (value, "ChangeState", true) == 0) {
									aiType = AI_Type.ChangeState;
									if (aiCmd == null) {
										aiCmd = new AI_Command ();
										aiCmd.type = aiType;
										aiCmd.name = aiName;
									}
								}
							} else {
								if (aiCmd == null)
									continue;
								if (string.Compare (key, "value", true) == 0) {
									aiCmd.value = value;
								} else if (string.Compare (key, "triggerall", true) == 0) {
									if (value.StartsWith ("command", StringComparison.CurrentCultureIgnoreCase)) {
										int idx = value.IndexOf ("=");
										if (idx >= 0) {
											aiCmd.command = value.Substring (idx + 1, value.Length - idx - 1).Trim ();
											if (!string.IsNullOrEmpty (aiCmd.command)) {
												Cmd_Command cmdCmd = GetCommand (aiCmd.command);
												if (cmdCmd != null) {
													cmdCmd.aiName = aiCmd.name;
												}
											}
										}
									}
								}
							}
						}
					}


					if (aiCmd == null || aiCmd.type == AI_Type.none)
						continue;
					if (m_AICmdMap == null)
						m_AICmdMap = new Dictionary<string, AI_Command> ();
					m_AICmdMap [aiCmd.name] = aiCmd;

				}
            }

            return true;
		}
	}
}

