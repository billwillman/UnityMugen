using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using XNode;
using System;

namespace XNode.Mugen
{
	[CreateNodeMenu("AI/创建StateEvent/创建爆炸")]
	[Serializable]
	public class AI_StateEvent_CreateExplod: AI_CreateStateEvent
	{
		[SerializeField] public int anim = CNSStateDef._cNoVaildAnim;
		[SerializeField] public int id = -1;
		[SerializeField] public int pos_x = 0;
		[SerializeField] public int pos_y = 0;
		[SerializeField] public ExplodPosType explodPosType = ExplodPosType.p1;
		[SerializeField] public int bindtime = -1;
		[SerializeField] public int removetime = -2;
		[SerializeField] public int sprpriority = 0;
		[SerializeField] public int removeongethit = 0;
		[SerializeField] public int ignorehitpause = 1;
		[SerializeField] public bool isChangeStateRemove = true;
		[SerializeField] public bool IsUseParentUpdate = true;
		[SerializeField] public Vector2 scale = Vector2.one;

		protected override string GetDoStr(bool hasCond)
		{
			string pre = string.Empty;
			if (hasCond)
				pre = "\t\t\t\t\t\t\t\t";
			else
				pre = "\t\t\t\t\t\t";
			string ret = "local explod = trigger:CreateExplod(luaPlayer)\n\r";
			if (VaildAnim (anim))
				ret += string.Format ("{0}explod.anim = {1:D}\n\r", pre, anim);
			else
				return string.Empty;

			if (id >= 0)
				ret += string.Format ("{0}explod.ID = {1:D}\n\r", pre, id);
			if (pos_x != 0)
				ret += string.Format ("{0}explod.pos_x = {1:D}\n\r", pre, pos_x);
			if (pos_y != 0)
				ret += string.Format ("{0}explod.pos_y = {1:D}\n\r", pre, pos_y);

			ret += string.Format ("{0}explod.postype = {1}.{2}\n\r", pre, explodPosType.GetType ().FullName, explodPosType.ToString ());

			if (bindtime > 0)
				ret += string.Format ("{0}explod.bindtime = {1:D} * bindTimePer\n\r", pre, bindtime);


			ret += string.Format ("{0}explod.removetime = {1:D}\n\r", pre, removetime);

			if (sprpriority != 0)
				ret += string.Format ("{0}explod.sprpriority = {1:D}\n\r", pre, sprpriority);

			ret += string.Format ("{0}explod.removeongethit = {1:D}\n\r", pre, removeongethit);
			ret += string.Format ("{0}explod.ignorehitpause = {1:D}\n\r", pre, ignorehitpause);
			ret += string.Format ("{0}explod.isChangeStateRemove = {1}\n\r", pre, isChangeStateRemove.ToString().ToLower());
			ret += string.Format ("{0}explod.IsUseParentUpdate = {1}\n\r", pre, IsUseParentUpdate.ToString().ToLower());

			if (Mathf.Abs ((Vector2.one - scale).magnitude) > float.Epsilon) {
				ret += string.Format ("{0}explod.scale = Vector2.New({1}f, {2}f)", pre, scale.x.ToString (), scale.y.ToString ());
			}

			ret += pre + "explod:Apply()\n\r";

			return ret;
		}
	}
}
