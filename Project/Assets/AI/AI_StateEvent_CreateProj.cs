using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/创建飞行物")]
	[Serializable]
	public class AI_StateEvent_CreateProj: AI_CreateStateEvent
	{
		[SerializeField] public int projid = -1;
		[SerializeField] public int projanim = CNSStateDef._cNoVaildAnim;
		[SerializeField] public int projhitanim = CNSStateDef._cNoVaildAnim;
		[SerializeField] public int projremove = 1;
		[SerializeField] public int projcancelanim = CNSStateDef._cNoVaildAnim;
		[SerializeField] public int projremanim = CNSStateDef._cNoVaildAnim;
		[SerializeField] public float projremovetime = 100;
		[SerializeField] public int offset_x = 0;
		[SerializeField] public int offset_y = 0;
		[SerializeField] public float velocity_x = 0;
		[SerializeField] public float velocity_y = 0;
		[SerializeField] public ExplodPosType Postype = ExplodPosType.p1;
		[SerializeField] public int projshadow = 0;
		[SerializeField] public int projpriority = 0;
		[SerializeField] public int projsprpriority = 0;

		protected override string GetDoStr(bool hasCond)
		{
			string pre = string.Empty;
			if (hasCond)
				pre = "\t\t\t\t\t\t\t\t";
			else
				pre = "\t\t\t\t\t\t";

			string ret = "local proj = trigger:CreateProj(luaPlayer)\n\r";
			if (projid >= 0)
				ret += string.Format ("{0}proj.projid = {1:D}\n\r", pre, projid);

			if (VaildAnim(projanim))
				ret += string.Format ("{0}proj.projanim = {1:D}\n\r", pre, projanim);

			if (VaildAnim(projhitanim))
				ret += string.Format ("{0}proj.projhitanim = {1:D}\n\r", pre, projhitanim);

			if (VaildAnim(projremanim))
				ret += string.Format ("{0}proj.projremanim = {1:D}\n\r", pre, projremanim);

			if (VaildAnim(projcancelanim))
				ret += string.Format ("{0}proj.projcancelanim = {1:D}\n\r", pre, projcancelanim);

			if (offset_x != 0)
				ret += string.Format ("{0}proj.offset_x = {1:D}\n\r", pre, offset_x);

			if (offset_y != 0)
				ret += string.Format ("{0}proj.offset_y = {1:D}\n\r", pre, offset_y);

			if (VaildStr(velocity_x, false) != "nil")
				ret += string.Format ("{0}proj.velocity_x = {1}\n\r", pre, velocity_x.ToString());

			if (VaildStr(velocity_y, false) != "nil")
				ret += string.Format ("{0}proj.velocity_y = {1}\n\r", pre, velocity_y.ToString());

			if (Postype != ExplodPosType.p1)
				ret += string.Format ("{0}proj.Postype = {1}.{2}\n\r", pre, Postype.GetType().FullName, Postype.ToString());

			if (projshadow != 0)
				ret += string.Format ("{0}proj.projshadow = {1:D}\n\r", pre, projshadow);

			if (projpriority != 0)
				ret += string.Format ("{0}proj.projpriority = {1:D}\n\r", pre, projpriority);

			if (projsprpriority != 0)
				ret += string.Format ("{0}proj.projsprpriority = {1:D}\n\r", pre, projsprpriority);

			if (projremovetime >= 0)
				ret += string.Format ("{0}proj.projremovetime = {1}\n\r", pre, projremovetime.ToString());

			ret += string.Format ("{0}proj.projremove = {1:D}\n\r", pre, projremove);
			ret += pre + "proj:Apply()\n\r";

			return ret;
		}

	}
}
