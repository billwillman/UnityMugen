using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen {

    [CreateNodeMenu("AI/创建StateEvent/创建NotHit")]
    [Serializable]
    public class AI_StateEvent_NotHit : AI_CreateStateEvent {
        [SerializeField] public string scriptFuncName = string.Empty;
        [SerializeField] public Cns_Type standType = Cns_Type.none;
        [SerializeField] public Cns_MoveType moveType = Cns_MoveType.none;
        [SerializeField] public Cns_PhysicsType physicsType = Cns_PhysicsType.none;
        [NonSerialized] public float durTime = 1f;

        protected override string GetDoStr(bool hasCond) {
            if (standType == Cns_Type.none && moveType == Cns_MoveType.none && physicsType == Cns_PhysicsType.none && string.IsNullOrEmpty(scriptFuncName))
                return string.Empty;

            string ret = string.Format("trigger:CreateNotHit(luaPlayer, %s, %s.%s, %s.%s, %s.%s, %s)",
                                durTime.ToString(),
                                standType.GetType().FullName, standType.ToString(),
                                moveType.GetType().FullName, moveType.ToString(),
                                physicsType.GetType().FullName, physicsType.ToString(),
                                scriptFuncName
                         );
            return ret;
        }
    }
}
