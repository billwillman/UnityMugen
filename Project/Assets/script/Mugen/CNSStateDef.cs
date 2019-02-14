using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

namespace Mugen
{
	public enum Cns_Type
	{
		none = 0,
		S,  // 站立
		C,  // 蹲
		A,  // 空中
		L   // 倒立
	}

	public enum Cns_MoveType
	{
		none = 0,
		A, //攻击
		I, // 非攻击
		H  // 受击
	}

	public enum Cns_PhysicsType
	{
		none = 0,
		S,
		C,
		A,
        N
	}

	public class CNSStateDef
	{
		private Cns_Type m_Type = Cns_Type.none;
		private Cns_MoveType m_MoveType = Cns_MoveType.none;
		private Cns_PhysicsType m_PhysicsType = Cns_PhysicsType.none;
        // 事件注册
        private Dictionary<int, List<CNSState>> m_StateEventsMap = null;

		private int m_Juggle;
		private float m_Velset_x;
		private float m_Velset_y;
		private int m_Ctrl; 			// 设定可控与否,没有则，不改变
		private int m_Anim; 			// 改变动作，不写则不改变
		private int m_PowerAdd;
		private int m_Sprpriority;

		private static readonly int _cNoVaildVelset = -9999;
		private static readonly int _cNoVaildCtrl = -1;
		private static readonly int _cNoVaildAnim = (int)PlayerState.psNone;

        [NoToLuaAttribute]
        public void OnStateEvent(CnsStateType evtType)
        {
            // 触发状态事件
        }

        public CNSState CreateStateEvent(CnsStateType evtType)
        {
            if (evtType == CnsStateType.none)
                return null;
            CNSState state = new CNSState(evtType);
            int key = (int)evtType;
            List<CNSState> list;
            if (m_StateEventsMap == null)
            {
                m_StateEventsMap = new Dictionary<int, List<CNSState>>();
                list = null;
            } else
            {
                if (!m_StateEventsMap.TryGetValue(key, out list))
                    list = null;
            }
            if (list == null)
            {
                list = new List<CNSState>();
                m_StateEventsMap[key] = list;
            }
            list.Add(state);
            return state;
        }

        public int Sprpriority
        {
            get
            {
                return m_Sprpriority;
            }
            set
            {
                m_Sprpriority = value;
            }
        }

		public Cns_PhysicsType PhysicsType
		{
			get
			{
				return m_PhysicsType;
			}
			set
			{
				m_PhysicsType = value;
			}
		}

		public Cns_MoveType MoveType
		{
			get
			{
				return m_MoveType;
			}
			set
			{
				m_MoveType = value;
			}
		}

		public Cns_Type Type
		{
			get
			{
				return m_Type;
			}
			set
			{
				m_Type = value;
			}
		}

		public int PowerAdd
		{
			get
			{
				return m_PowerAdd;
			}
			set
			{
				m_PowerAdd = value;
			}
		}

		public int Animate
		{
			get
			{
				return m_Anim;
			}
			set
			{
				m_Anim = value;
			}
		}

		public int Ctrl
		{
			get
			{
				return m_Ctrl;
			}
			set
			{
				m_Ctrl = value;
			}
		}

		public float Velset_y
		{
			get
			{
				return m_Velset_y;
			}
			set
			{
				m_Velset_y = value;
			}
		}

		public int Juggle
		{
			get
			{
				return m_Juggle;
			}
			set
			{
				m_Juggle = value;
			}
		}

		public float Velset_x
		{
			get
			{
				return m_Velset_x;
			}
			set
			{
				m_Velset_x = value;
			}
		}

		internal int id {
			get;
			set;
		}

		[NoToLuaAttribute]
        public PlayerState Anim
        {
            get
            {
                PlayerState ret = (PlayerState)m_Anim;
                return ret;
            }
        }

		private void ResetVars()
		{
			m_Type = Cns_Type.none;
			m_MoveType = Cns_MoveType.none;
			m_PhysicsType = Cns_PhysicsType.none;
			m_Juggle = 0;
			m_Velset_x = _cNoVaildVelset;
			m_Velset_y = _cNoVaildVelset;
			m_Ctrl = _cNoVaildCtrl;
			m_Anim = _cNoVaildAnim;
			m_PowerAdd = 0;
			m_Sprpriority = 0; 
		}

		[NoToLuaAttribute]
		public bool LoadConfigReader(ConfigSection section)
		{
			ResetVars ();
			if (section == null)
				return false;

			for (int i = 0; i < section.ContentListCount; ++i) {
				string key, value;
				if (section.GetKeyValue (i, out key, out value)) {
					if (string.Compare (key, "type", true) == 0) {
						if (string.Compare (value, "S", true) == 0)
							m_Type = Cns_Type.S;
						else if (string.Compare (value, "C", true) == 0)
							m_Type = Cns_Type.C;
						else if (string.Compare (value, "A", true) == 0)
							m_Type = Cns_Type.A;
						else if (string.Compare (value, "L", true) == 0)
							m_Type = Cns_Type.L;
						else
							m_Type = Cns_Type.none;
					} else if (string.Compare (key, "movetype", true) == 0) {
						if (string.Compare (value, "A", true) == 0)
							m_MoveType = Cns_MoveType.A;
						else if (string.Compare (value, "I", true) == 0)
							m_MoveType = Cns_MoveType.I;
						else if (string.Compare (value, "H", true) == 0)
							m_MoveType = Cns_MoveType.H;
						else
							m_MoveType = Cns_MoveType.none;
					} else if (string.Compare (key, "physics", true) == 0) {
						if (string.Compare (value, "S", true) == 0)
							m_PhysicsType = Cns_PhysicsType.S;
						else if (string.Compare (value, "C", true) == 0)
							m_PhysicsType = Cns_PhysicsType.C;
						else if (string.Compare (value, "A", true) == 0)
							m_PhysicsType = Cns_PhysicsType.A;
						else
							m_PhysicsType = Cns_PhysicsType.none;
					} else if (string.Compare (key, "juggle", true) == 0) {
						m_Juggle = int.Parse (value);
					} else if (string.Compare (key, "velset", true) == 0) {
						string[] vs1 = ConfigSection.Split (value);
						if (vs1 != null && vs1.Length >= 2) {
							m_Velset_x = float.Parse (vs1 [0]);
							m_Velset_y = float.Parse (vs1 [1]);
						}
					} else if (string.Compare (key, "ctrl", true) == 0) {
						m_Ctrl = int.Parse (value);
					} else if (string.Compare (key, "anim", true) == 0) {
						m_Anim = int.Parse (value);
					}

				}
			}

			return true;
		}
	}
}