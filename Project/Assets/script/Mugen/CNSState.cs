using UnityEngine;
using System.Collections;
using LuaInterface;

namespace Mugen
{
	public enum CnsStateType
	{
		none = 0,
		// 残影
		AfterImage,
		// 残影时间
		AfterImageTime,
		AllPalFX,
		// 角度加
		AngleAdd,
		// 角度绘制
		AngleDraw,
		// 角度乘
		AngleMul,
		// 角度设置
		AngleSet,
		// 超必杀暂停
		SuperPause,

		// 飞行道具
		Projectile,
	}

	// 触发时机
	public enum CnsStateTriggerType
    {
        none = 0,
		AnimElem,
    }

    public class CNSState
    {
        private string m_Name = string.Empty;
		private CnsStateTriggerType m_TriggeType = CnsStateTriggerType.none;
		private CnsStateType m_Type = CnsStateType.none;
        
        [NoToLuaAttribute]
		public CNSState(CnsStateTriggerType triggeType, CnsStateType type)
        {
            m_TriggeType = triggeType;
			m_Type = type;
        }

		public int time {
			get;
			set;
		}

		public int movetime {
			get;
			set;
		}

		public int anim {
			get;
			set;
		}

		public int darken {
			get;
			set;
		}

		public int poweradd {
			get;
			set;
		}

		public int p2defmul {
			get;
			set;
		}



        public int projid
        {
            get;
            set;
        }

        public int projanim
        {
            get;
            set;
        }

		public System.Action<LuaTable, CNSState> OnTriggerEvent {
			get;
			set;
		}


		[NoToLuaAttribute]
		public void Call_TriggerEvent(PlayerDisplay display)
		{
			if (OnTriggerEvent == null)
				return;
			
			if (display == null)
				return;
			var lua = display.LuaPly;
			if (lua == null)
				return;
			OnTriggerEvent (lua, this);
		}
    }
}