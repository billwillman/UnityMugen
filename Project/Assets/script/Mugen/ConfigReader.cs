using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Mugen
{
	public interface IConfigPropertys
	{
		string ConfigName
		{
			get;
		}
	}

	public class ConfigSection
	{
		private static readonly string[] _cContentSplit = {"\n"};
		public static readonly string[] _cContentArrSplit = {","};
		private static readonly string _cConfigName = "ConfigName";
		public ConfigSection(string tile, string content)
		{
			mTile = tile;
			if (!string.IsNullOrEmpty(mTile))
			{
				if ((mTile[0] == '[') && (mTile[mTile.Length - 1] == ']'))
					mTile = mTile.Substring(1, mTile.Length - 2);
			}
			if (!string.IsNullOrEmpty(content))
			{
				string[] contentArr = content.Split(_cContentSplit, System.StringSplitOptions.RemoveEmptyEntries);
				if ((contentArr != null) && (contentArr.Length > 0))
				{
					ContentList.AddRange(contentArr);
					FormatContentList();
				}
			}
		}

		protected List<string> ContentList
		{
			get
			{
				if (mContentList == null)
					mContentList = new List<string>();
				return mContentList;
			}
		}

		protected void FormatContentList()
		{
			if (mContentList == null)
				return;
			for (int i = mContentList.Count - 1; i >= 0; --i)
			{
				string s = mContentList[i].Trim();
				if (s.StartsWith(";") || string.IsNullOrEmpty(s))
					mContentList.RemoveAt(i);
				else
				{
					int idx = s.IndexOf(";");
					if (idx >= 0)
					{
						s = s.Substring(0, idx);
						mContentList[i] = s.Trim();
					} else
						mContentList[i] = s;
				}
			}

			if (mContentList.Count <= 0)
				mContentList = null;
		}

		public bool GetKeyValue(int index, out string key, out int value)
		{
			value = 0;
			string v;
			bool ret = GetKeyValue(index, out key, out v);
			if (!ret)
				return ret;
			if (!int.TryParse(v, out value))
			{
				value = 0;
				return false;
			}

			return true;
		}

		public bool GetKeyValue(int index, out string key, out float value)
		{
			value = 0;
			string v;
			bool ret = GetKeyValue(index, out key, out v);
			if (!ret)
				return ret;
			if (!float.TryParse(v, out value))
			{
				value = 0;
				return false;
			}
			
			return true;
		}

		public bool GetKeyValue(int index, out string key, List<int> value)
		{
			key = string.Empty;
			if (value == null)
				return false;
			string s;
			bool ret = GetKeyValue(index, out key, out s);
			if (!ret)
				return ret;
			string[] s1 = s.Split(_cContentArrSplit, StringSplitOptions.RemoveEmptyEntries);
			if (s1 == null)
				return false;
			ret = false;
			for (int i = 0; i < s1.Length; ++i)
			{
				string item = s1[i].Trim();
				if (string.IsNullOrEmpty(item))
					continue;
				int d;
				if (!int.TryParse(item, out d))
					continue;

				value.Add(d);
				ret = true;
			}

			return ret;
		}

		public bool GetArray(int index, List<string> arr)
		{
			if (arr == null)
				return false;
			if (!CheckContentIndex(index))
				return false;
			string s = mContentList[index];
			if (string.IsNullOrEmpty(s))
				return false;
			string[] s1 = s.Split(_cContentArrSplit, StringSplitOptions.None);
			if (s1 == null)
				return false;

			arr.AddRange (s1);
			return true;
			/*
			bool ret = false;
			for (int i = 0; i < s1.Length; ++i)
			{
				string item = s1[i].Trim();
				if (string.IsNullOrEmpty (item)) {
					arr.Add (string.Empty);
					continue;
				}
				arr.Add(item);
				ret = true;
			}

			return ret;*/
		}

		public static string[] Split(string value)
		{
			if (string.IsNullOrEmpty (value))
				return null;
			string[] s1 = value.Split(_cContentArrSplit, StringSplitOptions.RemoveEmptyEntries);
			if (s1 == null || s1.Length <= 0)
				return null;
			for (int i = 0; i < s1.Length; ++i) {
				string s = s1 [i].Trim ();
				s1 [i] = s;
			}
			return s1;
		}

		public bool GetArray(int index, List<float> arr)
		{
			if (arr == null)
				return false;
			if (!CheckContentIndex(index))
				return false;
			string s = mContentList[index];
			if (string.IsNullOrEmpty(s))
				return false;
			string[] s1 = s.Split(_cContentArrSplit, StringSplitOptions.RemoveEmptyEntries);
			if (s1 == null)
				return false;
			bool ret = false;
			for (int i = 0; i < s1.Length; ++i)
			{
				string item = s1[i].Trim();
				if (string.IsNullOrEmpty(item))
					continue;
				float v;
				if (!float.TryParse(item, out v))
					return false;
				arr.Add(v);
				ret = true;
			}
			
			return ret;
		}

		public bool GetArray(int index, List<int> arr)
		{
			if (arr == null)
				return false;
			if (!CheckContentIndex(index))
				return false;
			string s = mContentList[index];
			if (string.IsNullOrEmpty(s))
				return false;
			string[] s1 = s.Split(_cContentArrSplit, StringSplitOptions.RemoveEmptyEntries);
			if (s1 == null)
				return false;
			bool ret = false;
			for (int i = 0; i < s1.Length; ++i)
			{
				string item = s1[i].Trim();
				if (string.IsNullOrEmpty(item))
					continue;
				int v;
				if (!int.TryParse(item, out v))
					return false;
				arr.Add(v);
				ret = true;
			}
			
			return ret;
		}

		public bool GetKeyValue(int index, out string key, List<float> value)
		{
			key = string.Empty;
			if (value == null)
				return false;
			string s;
			bool ret = GetKeyValue(index, out key, out s);
			if (!ret)
				return ret;
			string[] s1 = s.Split(_cContentArrSplit, StringSplitOptions.RemoveEmptyEntries);
			if (s1 == null)
				return false;
			ret = false;
			for (int i = 0; i < s1.Length; ++i)
			{
				string item = s1[i].Trim();
				if (string.IsNullOrEmpty(item))
					continue;

				float d;
				if (!float.TryParse(item, out d))
					continue;
				value.Add(d);
				ret = true;
			}
			
			return ret;
		}


		public bool GetKeyValue(int index, out string key, out string value)
		{
			bool ret = false;
			key = string.Empty;
			value = string.Empty;
			if (!CheckContentIndex(index))
				return ret;
			string s = mContentList[index];
			if (string.IsNullOrEmpty(s))
				return ret;
			int idx = s.IndexOf("=");
			if (idx < 0)
				return ret;
			key = s.Substring(0, idx).Trim();
			value = s.Substring(idx + 1, s.Length - idx - 1).Trim();
			ret = true;
			return ret;
		}

		public bool GetPropertysValues<T>(T obj) where T: IConfigPropertys
		{
			if ((obj == null) || (!HasContent))
				return false;

			Type tType = obj.GetType();
			var props = tType.GetProperties();
			if ((props == null) || (props.Length <= 0))
				return false;
			string name = obj.ConfigName;
			bool isEmptyName = string.IsNullOrEmpty(name);
			bool ret = false;
			for (int i = 0; i < props.Length; ++i)
			{
				PropertyInfo prop = props[i];
				bool isFound = false;
				if ((prop != null) && (!string.IsNullOrEmpty(prop.Name)) && prop.CanWrite)
				{
					if (string.Compare(prop.Name, _cConfigName) == 0)
						continue;
					string s;
					if (isEmptyName)
						s = prop.Name;
					else
						s = string.Format("{0}.{1}", name, prop.Name);
					for (int j = 0; j < mContentList.Count; ++j)
					{
						string s1 = mContentList[j];
						if ((s1.Length > s.Length + 1) && (s1.StartsWith(s, StringComparison.CurrentCultureIgnoreCase)))
						{
							string right = s1.Substring(s.Length, s1.Length - s.Length).Trim();
							if (right.StartsWith("="))
							{
								string value = right.Substring(1).Trim();
								if ((value.Length >= 2) && (value[0] == '\"') && (value[value.Length - 1] == '\"'))
								{
									value = value.Substring(1, value.Length - 2);
									if (string.IsNullOrEmpty(value))
										value = string.Empty;
								}
								isFound = true;
								if (prop.PropertyType != value.GetType ()) {
									if (prop.PropertyType == typeof(Int32)) {
										int iv = int.Parse (value);
										prop.SetValue (obj, iv, null);
									} else if (prop.PropertyType == typeof(float)) {
										float fv = float.Parse (value);
										prop.SetValue (obj, fv, null);
									}
									else
										prop.SetValue(obj, value, null);
								} else
									prop.SetValue(obj, value, null);
								break;
							}
						}
					}

					if (isFound)
						ret = true;
				}
			}

			return ret;
		}

		public bool HasContent
		{
			get
			{
				return (mContentList != null) && (mContentList.Count > 0);
			}
		}

		public string Tile
		{
			get
			{
				return mTile;
			}
		}

		public string GetContent(int index)
		{
			if (mContentList == null)
				return null;
			if ((index < 0) || (index >= mContentList.Count))
				return null;
			return mContentList[index];
		}

		public int ContentListCount
		{
			get
			{
				if (mContentList == null)
					return 0;
				return mContentList.Count;
			}
		}

		protected bool CheckContentIndex(int index)
		{
			return (index >= 0) && (mContentList != null) && (index < mContentList.Count);
		}

		private List<string> mContentList = null;
		private string mTile = string.Empty;
	}

	// Config Reader class
	public class ConfigReader
	{

		public void LoadString(string str)
		{
			if (string.IsNullOrEmpty(str))
				return;
			ConfigSection section = GetFirstSection(ref str);
			while (section != null)
			{
				SectionList.Add(section);
				section = GetFirstSection(ref str);
			}
		}

		public ConfigSection GetSection(string tile)
		{
			if (string.IsNullOrEmpty(tile) || (mSectionList == null) || (mSectionList.Count <= 0))
				return null;
			for (int i = 0; i < mSectionList.Count; ++i)
			{
				ConfigSection section = mSectionList[i];
				if (section != null)
				{
					if (string.IsNullOrEmpty(section.Tile))
						continue;
					if (string.Compare(section.Tile, tile) == 0)
					{
						return section;
					}
				}
			}

			return null;
		}

		private ConfigSection GetFirstSection(ref string str)
		{
			if (string.IsNullOrEmpty(str))
				return null;
			Match match = TileReg.Match(str);
			if ((match == null) || (match.Index < 0) || string.IsNullOrEmpty(match.Value))
				return null;
			string tile = match.Value.Trim();
			if ((!string.IsNullOrEmpty (tile))) {
				var idx = tile.IndexOf (';');
				if (idx >= 0)
					tile = tile.Substring (0, idx);
			}
			str = str.Substring(match.Index + match.Value.Length);
			match = TileReg.Match(str);
			if ((match == null) || (match.Index < 0) || string.IsNullOrEmpty(match.Value))
			{
				ConfigSection ret = new ConfigSection(tile, str);
				str = string.Empty;
				return ret;
			}

			string content = str.Substring(0, match.Index);
			str = str.Substring(match.Index);
			ConfigSection section = new ConfigSection(tile, content);
			return section;
		}

		protected List<ConfigSection> SectionList
		{
			get
			{
				if (mSectionList == null)
					mSectionList = new List<ConfigSection>();
				return mSectionList;
			}
		}

		protected Regex TileReg
		{
			get
			{
				if (mTileReg != null)
					return mTileReg;
				mTileReg = new Regex(@"\s\[.*\]", RegexOptions.IgnoreCase);
				return mTileReg;
			}
		}

		public int SectionCount
		{
			get
			{
				if (mSectionList == null)
					return 0;
				return mSectionList.Count;
			}
		}

		public ConfigSection GetSections(int index)
		{
			if ((index < 0) || (index >= SectionCount))
				return null;
			return mSectionList[index];
		}

		private List<ConfigSection> mSectionList = null;
		private Regex mTileReg = null;
	}
}

