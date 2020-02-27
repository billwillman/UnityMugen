using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;

namespace Mugen {
    public class CnsNotHit {
        public bool m_IsEnabled = false;
        public Cns_Type m_Type = Cns_Type.none;
        public Cns_MoveType m_MoveType = Cns_MoveType.none;
        public Cns_PhysicsType m_PhysicsType = Cns_PhysicsType.none;
        public float m_Time = 0f;

        public void Update(float deltaTime) {
            if (m_IsEnabled) {
                m_Time -= deltaTime;
                if (m_Time <= 0) {
                    Reset();
                }
            }
        }

        public void Reset() {
            m_Type = Cns_Type.none;
            m_MoveType = Cns_MoveType.none;
            m_PhysicsType = Cns_PhysicsType.none;
            m_Time = 0f;
            m_IsEnabled = false;
        }

        public string m_LuaFuncName = string.Empty;
        public bool CheckPlayer(PlayerDisplay display) {
            if (display == null || !m_IsEnabled)
                return false;

            if ((m_Type != Cns_Type.none && display.StateType != m_Type) || (m_PhysicsType != Cns_PhysicsType.none && display.PhysicsType != m_PhysicsType) ||
                (m_MoveType != Cns_MoveType.none && m_MoveType != display.MoveType))
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
