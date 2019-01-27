using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen
{
	public class CNSConfig
	{
		//public static readonly float _cPerUnit = 0.8f;

		public bool LoadPlayer(string playerName)
		{
			Reset();
			if (string.IsNullOrEmpty (playerName))
				return false;
			string fileName = string.Format("{0}@{1}/{2}.cns.txt", AppConfig.GetInstance().PlayerRootDir, playerName, playerName);
			return LoadFromFile(fileName);
		}

		public bool LoadFromFile(string fileName)
		{
			Reset();
			string str = AppConfig.GetInstance().Loader.LoadText(fileName);
			if (str == null || string.IsNullOrEmpty(str))
				return false;
			ConfigReader reader = new ConfigReader();
			reader.LoadString(str);

			ConfigSection section = reader.GetSection("Velocity");
			if (section != null) {
				if (!LoadVelocity (section))
					return false;
			}

			section = reader.GetSection("Size");
			if (section != null) {
				if (!LoadSize (section))
					return false;
			}

			return true;
		}

		private bool LoadSize(ConfigSection section)
		{
			if (section == null)
				return false;

			Dictionary<string, List<float>> keyValue = new Dictionary<string, List<float>>();
			for (int i = 0; i < section.ContentListCount; ++i)
			{
				string key;
				List<float> values = new List<float>();
				if (section.GetKeyValue(i, out key, values))
				{
					if (keyValue.ContainsKey(key))
						keyValue[key] = values;
					else
						keyValue.Add(key, values);
				}
			}

			GetKeyValue(keyValue, "xscale", out XScale);
			GetKeyValue(keyValue, "yscale", out YScale);
			GetKeyValue(keyValue, "ground.front", out GuardForwardNear);
			GetKeyValue(keyValue, "ground.back", out GuardBackNear);
			GetKeyValue(keyValue, "air.front", out AirForwardNear);
			GetKeyValue(keyValue, "air.back", out AirBackNear);

			return true;
		}

		private bool LoadVelocity(ConfigSection section)
		{
			if (section == null)
				return false;

			Dictionary<string, List<float>> keyValue = new Dictionary<string, List<float>>();
			for (int i = 0; i < section.ContentListCount; ++i)
			{
				string key;
				List<float> values = new List<float>();
				if (section.GetKeyValue(i, out key, values))
				{
					if (keyValue.ContainsKey(key))
						keyValue[key] = values;
					else
						keyValue.Add(key, values);
				}
			}

			GetKeyValue(keyValue, "walk.fwd", out ForwardWalkSpeed);
			GetKeyValue(keyValue, "walk.back", out BackWalkSpeed);
			GetKeyValue(keyValue, "run.fwd", out ForwardRunSpeed);
			GetKeyValue(keyValue, "run.fwd", out BackRunSpeed);

			return true;
		}

		private void GetKeyValue(Dictionary<string, List<float>> map, string key, out float value)
		{
			value = 0;
			if (map == null || string.IsNullOrEmpty(key))
				return;
			List<float> list;
			if (map.TryGetValue(key, out list))
			{
				value = list[0];
			}
		}

		private void GetKeyValue(Dictionary<string, List<float>> map, string key, out Vector2 value)
		{
			value = Vector2.zero;
			if (map == null || string.IsNullOrEmpty(key))
				return;
			List<float> list;
			if (map.TryGetValue(key, out list))
			{
				value.x = list[0];
				if (list.Count > 1)
					value.y = list[1];
			}
		}

		void Reset()
		{
			ForwardWalkSpeed = 0;
			BackWalkSpeed = 0;
			ForwardRunSpeed = Vector2.zero;
			BackRunSpeed = Vector2.zero;
			XScale = 1.0f;
			YScale = 1.0f;
		}

		public float ForwardWalkSpeed = 1.0f;
		public float BackWalkSpeed = 1.0f;
		public Vector2 ForwardRunSpeed = Vector2.one;
		public Vector2 BackRunSpeed = Vector2.one;
		public float XScale = 1.0f;
		public float YScale = 1.0f;

		// 地面前面离敌人距离
		public float GuardForwardNear;
		// 地面背面离敌人距离
		public float GuardBackNear;
		public float AirForwardNear;
		public float AirBackNear;
	}


}

