using UnityEngine;
using System.Collections;

namespace XNode.Mugen {

    [CreateNodeMenu("AI/条件/游戏回合")]
    [System.Serializable]
    public class AI_Cond_RoundState : AI_BaseCondition {

        [SerializeField]
        public int value = 0;
        [SerializeField]
        public AI_Cond_Op op = AI_Cond_Op.Equal;

        public override string ToCondString(string luaPlayer) {
            var opStr = GetOpStr(op);
            string ret = string.Format("trigger:RoundState(){0}{1:D}", opStr, value);
            return ret;
        }
    }

}
