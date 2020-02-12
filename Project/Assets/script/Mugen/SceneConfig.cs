using System;
using System.Collections;
using System.Collections.Generic;

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

		public string author
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

    public class BgDef : IConfigPropertys
    {
        public string ConfigName
        {
            get
            {
                return string.Empty;
            }
        }

        public string spr
        {
            get;
            protected set;
        }

        public int debugbg
        {
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

		public int p3startx {
			get;
			protected set;
		}

		public int p3starty {
			get;
			protected set;
		}

		public int p3startz {
			get;
			protected set;
		}

		public int p3facing {
			get;
			protected set;
		}

		public int p4startx {
			get;
			protected set;
		}

		public int p4starty {
			get;
			protected set;
		}

		public int p4startz {
			get;
			protected set;
		}

		public int p4facing {
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
        private AirConfig m_AirConfig = null;
        private BgDef m_BgDef = null;
        private BgConfig m_BgCfg = null;

        public BgConfig BgCfg
        {
            get
            {
                return m_BgCfg;
            }
        }

        public StageInfo Info
        {
            get
            {
                return m_Info;
            }
        }

        public BgDef bgDef
        {
            get
            {
                return m_BgDef;
            }
        }

        public AirConfig AirCfg
        {
            get
            {
                return m_AirConfig;
            }
        }

        public StageScaling Scaling
        {
            get
            {
                return m_Scaling;
            }
        }

        public StagePlayerInfo Players
        {
            get
            {
                return m_Players;
            }
        }

        public StageCamera Camera
        {
            get
            {
                return m_Cam;
            }
        }

		public bool IsVaild
		{
			get
			{
                return m_Info != null && m_Cam != null && m_Players != null && m_Scaling != null && m_AirConfig != null && m_BgDef != null && m_BgCfg != null;
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

		/*
		private static readonly string _cBeginActionTitle = "Begin action";
		private void LoadBeginActions(ConfigReader reader)
		{
			if (m_BeginActionMap != null)
				m_BeginActionMap.Clear ();
			for (int i = 0; i < reader.SectionCount; ++i) {
				var section = reader.GetSections (i);
				if (section != null && !string.IsNullOrEmpty(section.Tile)) {
					if (section.Tile.StartsWith (_cBeginActionTitle, StringComparison.CurrentCultureIgnoreCase)) {
						string actionnoStr = section.Tile.Substring (_cBeginActionTitle.Length);
						if (!string.IsNullOrEmpty (actionnoStr)) {
							actionnoStr = actionnoStr.Trim ();
							if (!string.IsNullOrEmpty (actionnoStr)) {
								int actionno;
								if (int.TryParse (actionnoStr, out actionno)) {
									BeginAction action = new BeginAction (section);
									this.AddBeginAction (actionno, action);
								}
							}
						}
					}
				}
			}	
		}
		*/

		private bool LoadFromStr(string str)
		{
			Clear ();
			if (string.IsNullOrEmpty (str))
				return false;
			ConfigReader reader = new ConfigReader ();
			reader.LoadString (str);

            // 1
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

            // 2
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

            // 3
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

            // 4
			section = reader.GetSection ("Scaling");
            if (section != null)
            {
                m_Scaling = new StageScaling();
                if (!section.GetPropertysValues(m_Scaling))
                {
                    Clear();
                    return false;
                }
            } else
            {
                m_Scaling = new StageScaling();
            }

            // 5
            m_AirConfig = new AirConfig(reader);
            if (!m_AirConfig.IsVaild)
            {
                Clear();
                return false;
            }

            // 6
            section = reader.GetSection("BGdef");
            if (section == null)
            {
                Clear();
                return false;
            }
            m_BgDef = new BgDef();
            if (!section.GetPropertysValues(m_BgDef))
            {
                Clear();
                return false;
            }
            
            // 7.
            m_BgCfg = new BgConfig();
            if (!m_BgCfg.LoadFromReader(reader))
            {
                Clear();
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
            m_AirConfig = null;
            m_BgDef = null;
            m_BgCfg = null;
		}
	}
}
