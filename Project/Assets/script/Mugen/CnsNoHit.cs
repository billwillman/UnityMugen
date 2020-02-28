using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;

namespace Mugen {
    public class CnsNotHit {
        public bool m_IsEnabled = false;
        public byte m_StandTypes = 0;
        public byte m_MoveTypes = 0;
        public byte m_PhysicsTypes = 0;
		public bool m_NoProj = false;

        public float m_Time = 1f;

        public void UpdateTime(float tick) {
            if (m_IsEnabled) {
                m_Time -= tick;
                if (m_Time <= 0) {
                    Reset();
                }
            }
        }

        public void Reset() {
            m_StandTypes = 0;
            m_MoveTypes = 0;
            m_PhysicsTypes = 0;
            m_Time = 0f;
            m_IsEnabled = false;
			m_NoProj = false;
        }

        public string m_LuaFuncName = string.Empty;
		public bool CheckPlayer(PlayerDisplay display, PlayerDisplay owner) {
			if (display == null || owner == null || !m_IsEnabled)
                return false;

            if ((m_StandTypes != 0 && ((byte)display.StateType & m_StandTypes) != 0) || 
                (m_PhysicsTypes != 0 && ((byte)display.PhysicsType & m_PhysicsTypes) != 0) ||
                (m_MoveTypes != 0 && (m_MoveTypes & (byte)display.MoveType) != 0))
                return false;

			if (owner.ShowType == DisplayType.Projectile && m_NoProj)
				return false;

            if (string.IsNullOrEmpty(m_LuaFuncName))
                return true;
            LuaTable luaPlayer = display.LuaPly;
            if (luaPlayer == null)
                return false;
            
            bool ret = luaPlayer.Invoke<LuaTable, bool>(m_LuaFuncName, luaPlayer);
            return ret;
        }
    }
}
