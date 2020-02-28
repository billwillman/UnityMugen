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
        [SerializeField] public bool Type_A = false;
        [SerializeField] public bool Type_C = false;
        [SerializeField] public bool Type_L = false;
        [SerializeField] public bool Type_S = false;

        [SerializeField] public bool MoveType_A = false;
        [SerializeField] public bool MoveType_I = false;
        [SerializeField] public bool MoveType_H = false;

        [SerializeField] public bool PhysicsType_S = false;
        [SerializeField] public bool PhysicsType_C = false;
        [SerializeField] public bool PhysicsType_A = false;
        [SerializeField] public bool PhysicsType_N = false;

		[SerializeField] public bool NoProj = false;

		[SerializeField] public int durTime = 1;

        protected override string GetDoStr(bool hasCond) {
            byte stand = 0;
            stand = (byte)(Type_A ? (stand | (1 << ((int)Cns_Type.A - 1))) : stand);
            stand = (byte)(Type_C ? (stand | (1 << ((int)Cns_Type.C - 1))) : stand);
            stand = (byte)(Type_L ? (stand | (1 << ((int)Cns_Type.L - 1))) : stand);
            stand = (byte)(Type_S ? (stand | (1 << ((int)Cns_Type.S - 1))) : stand);

            byte moveTypes = 0;
            moveTypes = (byte)(MoveType_A ? (moveTypes | (1 << ((int)Cns_MoveType.A - 1))) : moveTypes);
            moveTypes = (byte)(MoveType_I ? (moveTypes | (1 << ((int)Cns_MoveType.I - 1))) : moveTypes);
            moveTypes = (byte)(MoveType_H ? (moveTypes | (1 << ((int)Cns_MoveType.H - 1))) : moveTypes);

            byte physicTypes = 0;
            physicTypes = (byte)(PhysicsType_S ? (physicTypes | (1 << ((int)Cns_PhysicsType.S - 1))) : physicTypes);
            physicTypes = (byte)(PhysicsType_C ? (physicTypes | (1 << ((int)Cns_PhysicsType.C - 1))) : physicTypes);
            physicTypes = (byte)(PhysicsType_A ? (physicTypes | (1 << ((int)Cns_PhysicsType.A - 1))) : physicTypes);
            physicTypes = (byte)(PhysicsType_N ? (physicTypes | (1 << ((int)Cns_PhysicsType.N - 1))) : physicTypes);

			if (stand == 0 && moveTypes == 0 && physicTypes == 0 && !NoProj)
                return string.Empty;

			string script = scriptFuncName;
			if (string.IsNullOrEmpty(scriptFuncName))
				script = "\"\"";
			
			string ret = string.Format("trigger:CreateNotHit(luaPlayer, {0:D}, {1:D}, {2:D}, {3:D}, {4}, {5})",
                                durTime,
                                stand,
                                moveTypes,
                                physicTypes,
								NoProj.ToString().ToLower(),
								script
                         );
            return ret;
        }
    }
}
