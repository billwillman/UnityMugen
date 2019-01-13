using System;
using System.Collections;

namespace Mugen
{
	public class StageInfo: IConfigPropertys
	{
		public string ConfigName
		{
			get
			{
				return string.Empty;
			}
		}

		public string name
		{
			get;
			protected set;
		}
	}

	public class StageCamera: IConfigPropertys
	{
		public string ConfigName
		{
			get
			{
				return string.Empty;
			}
		}

		public int startx {
			get;
			protected set;
		}

		public int starty {
			get;
			protected set;
		}

		public int boundleft {
			get;
			protected set;
		}

		public int boundright {
			get;
			protected set;
		}

		public int boundhigh {
			get;
			protected set;
		}

		public int boundlow {
			get;
			protected set;
		}

		public float verticalfollow {
			get;
			protected set;
		}

		public int floortension {
			get;
			protected set;
		}

		public int tension {
			get;
			protected set;
		}
	}

	public class StagePlayerInfo: IConfigPropertys
	{
		public string ConfigName
		{
			get
			{
				return string.Empty;
			}
		}

		public int p1startx {
			get;
			protected set;
		}

		public int p1starty {
			get;
			protected set;
		}

		public int p1startz {
			get;
			protected set;
		}

		public int p1facing {
			get;
			protected set;
		}

		public int p2startx {
			get;
			protected set;
		}

		public int p2starty {
			get;
			protected set;
		}

		public int p2startz {
			get;
			protected set;
		}

		public int p2facing {
			get;
			protected set;
		}

		public int leftbound {
			get;
			protected set;
		}

		public int rightbound {
			get;
			protected set;
		}

		public int topbound {
			get;
			protected set;
		}

		public int botbound {
			get;
			protected set;
		}
	}

	public class StageScaling: IConfigPropertys
	{
		public string ConfigName
		{
			get
			{
				return string.Empty;
			}
		}

		public int topz {
			get;
			protected set;
		}

		public int botz {
			get;
			protected set;
		}

		public float topscale {
			get;
			protected set;
		}

		public float botscale {
			get;
			protected set;
		}
	}

	// 场景配置
	public class SceneConfig
	{
		private StageInfo m_Info = null;
		private StageCamera m_Cam = null;
		private StagePlayerInfo m_Players = null;
		private StageScaling m_Scaling = null;

		public bool IsVaild
		{
			get
			{
				return m_Info != null && m_Cam != null && m_Players != null && m_Scaling != null;
			}
		}

		public bool LoadFromFile(string fileName)
		{
			Clear ();
			if (string.IsNullOrEmpty (fileName))
				return false;
			string str = AppConfig.GetInstance ().Loader.LoadText (fileName);
			return LoadFromStr(str);
		}

		private bool LoadFromStr(string str)
		{
			Clear ();
			if (string.IsNullOrEmpty (str))
				return false;
			ConfigReader reader = new ConfigReader ();
			reader.LoadString (str);
			var section = reader.GetSection ("Info");
			if (section != null) {
				m_Info = new StageInfo ();
				if (!section.GetPropertysValues (m_Info)) {
					Clear ();
					return false;
				}
			} else {
				Clear ();
				return false;
			}

			section = reader.GetSection ("Camera");
			if (section == null) {
				Clear ();
				return false;
			}
			m_Cam = new StageCamera ();
			if (!section.GetPropertysValues (m_Cam)) {
				Clear ();
				return false;
			}

			section = reader.GetSection ("PlayerInfo");
			if (section == null) {
				Clear ();
				return false;
			}
			m_Players = new StagePlayerInfo ();
			if (!section.GetPropertysValues (m_Players)) {
				Clear ();
				return false;
			}

			section = reader.GetSection ("Scaling");
			if (section == null) {
				Clear ();
				return false;
			}
			m_Scaling = new StageScaling ();
			if (!section.GetPropertysValues (m_Scaling)) {
				Clear ();
				return false;
			}

			return IsVaild;
		}

		private void Clear()
		{
			m_Info = null;
			m_Cam = null;
			m_Players = null;
			m_Scaling = null;
		}
	}
}
