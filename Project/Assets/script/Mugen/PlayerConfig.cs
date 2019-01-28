
using System;

namespace Mugen
{
	public class PlayerFiles: IConfigPropertys
	{
		public string ConfigName
		{
			get
			{
				return string.Empty;
			}
		}

		public string cmd
		{
			get; protected set;
		}

		public string cns
		{
			get; protected set;
		}

		// state
		public string st
		{
			get; protected set;
		}

		public string st2
		{
			get; protected set;
		}

		public string st3
		{
			get; protected set;
		}

		public string st4
		{
			get; protected set;
		}

		public string anim
		{
			get; protected set;
		}

		public string sound
		{
			get; protected set;
		}

		public string stcommon
		{
			get; protected set;
		}

		public string sprite
		{
			get; protected set;
		}

		public string pal1
		{
			get; set;
		}

		public string pal2
		{
			get; set;
		}

		public string pal3
		{
			get; set;
		}

		public string pal4
		{
			get; set;
		}

		public string pal5
		{
			get; set;
		}

		public string pal6
		{
			get; set;
		}

		public string pal7
		{
			get; set;
		}

		public string pal8
		{
			get; set;
		}

		public string pal9
		{
			get; set;
		}

		public string pal10
		{
			get; set;
		}

		public string pal11
		{
			get; set;
		}

		public string pal12
		{
			get; set;
		}

        public bool HasPal
        {
            get
            {
                return (!string.IsNullOrEmpty(pal1)) || (!string.IsNullOrEmpty(pal2)) || (!string.IsNullOrEmpty(pal3))
                    || (!string.IsNullOrEmpty(pal4)) || (!string.IsNullOrEmpty(pal5)) || (!string.IsNullOrEmpty(pal6))
                    || (!string.IsNullOrEmpty(pal7)) || (!string.IsNullOrEmpty(pal8)) || (!string.IsNullOrEmpty(pal9))
                    || (!string.IsNullOrEmpty(pal10)) || (!string.IsNullOrEmpty(pal11)) || (!string.IsNullOrEmpty(pal12));
            }
        }

        public string ai
        {
            get; protected set;
        }
	}

	public class PlayerInfo: IConfigPropertys
	{
		public string ConfigName
		{
			get
			{
				return string.Empty;
			}
		}

		public string name {get; protected set;}
		public string displayname {get; protected set;}

        // 角色设计分辨率
        public int designWidth { get; set; }
        public int designHeight { get; set; }
	}

    public class PalletKeyMap: IConfigPropertys
    {
        public string ConfigName
        {
            get
            {
                return string.Empty;
            }
        }

        public int x
        {
            get;
            protected set;
        }

        public int y
        {
            get;
            protected set;
        }

        public int z
        {
            get;
            protected set;
        }

        public int a
        {
            get;
            protected set;
        }

        public int b
        {
            get;
            protected set;
        }

        public int c
        {
            get;
            protected set;
        }
    }

	public class PlayerConfig
	{
		public void LoadPlayer(string playerName)
		{
			if (string.IsNullOrEmpty(playerName))
				return;
			string fileName = string.Format("{0}@{1}/{2}.def.txt", AppConfig.GetInstance().PlayerRootDir, playerName, playerName);
			fileName = fileName.ToLower();
			string str = AppConfig.GetInstance().Loader.LoadText(fileName);
			LoadString(str);
		}

		public void LoadString(string str)
		{
			if (string.IsNullOrEmpty(str))
				return;
			ConfigReader reader = new ConfigReader();
			reader.LoadString(str);
			var section = reader.GetSection("Files");
			if (section == null)
				return;
			mPlayerFiles = new PlayerFiles();
			if (!section.GetPropertysValues(mPlayerFiles))
				mPlayerFiles = null;

			section = reader.GetSection("Info");
			mPlayerInfo = new PlayerInfo();
			if (section != null) {
				if (!section.GetPropertysValues (mPlayerInfo))
					mPlayerInfo = null;
                else
                {
                    for (int i = 0; i < section.ContentListCount; ++i)
                    {
                        string key, value;
                        if (section.GetKeyValue(i, out key, out value))
                        {
                            if (string.Compare(key, "localcoord", true) == 0)
                            {
                                string[] vals = ConfigSection.Split(value);
                                if (vals != null && vals.Length >= 2)
                                {
                                    mPlayerInfo.designWidth = int.Parse(vals[0]);
                                    mPlayerInfo.designHeight = int.Parse(vals[1]);
                                }
                            } else if (string.Compare(key, "pal.defaults", true) == 0)
                            {
                                if (mPlayerFiles.HasPal)
                                    continue;
                                string[] vals = ConfigSection.Split(value);
                                if (vals != null && vals.Length > 0)
                                {
                                    if (vals.Length >= 1)
                                        mPlayerFiles.pal1 = vals[0];
                                    if (vals.Length >= 2)
                                        mPlayerFiles.pal2 = vals[1];
                                    if (vals.Length >= 3)
                                        mPlayerFiles.pal3 = vals[2];
                                    if (vals.Length >= 4)
                                        mPlayerFiles.pal4 = vals[3];
                                    if (vals.Length >= 5)
                                        mPlayerFiles.pal5 = vals[4];
                                    if (vals.Length >= 6)
                                        mPlayerFiles.pal6 = vals[5];
                                    if (vals.Length >= 7)
                                        mPlayerFiles.pal7 = vals[6];
                                    if (vals.Length >= 8)
                                        mPlayerFiles.pal8 = vals[7];
                                    if (vals.Length >= 9)
                                        mPlayerFiles.pal9 = vals[8];
                                    if (vals.Length >= 10)
                                        mPlayerFiles.pal10 = vals[9];
                                    if (vals.Length >= 11)
                                        mPlayerFiles.pal11 = vals[10];
                                    if (vals.Length >= 12)
                                        mPlayerFiles.pal12 = vals[11];
                                }
                            }
                        }
                    }
                }
			}
            section = reader.GetSection("Palette Keymap");
            mKeyMap = new PalletKeyMap();
            if (section != null)
            {
                if (!section.GetPropertysValues(mKeyMap))
                    mKeyMap = null;
            }
		}

		public bool HasFilesConfig
		{
			get
			{
				return (mPlayerFiles != null) && (mPlayerInfo != null);
			}
		}

		public bool IsVaild
		{
			get
			{
				return HasFilesConfig;
			}
		}

		public PlayerFiles Files
		{
			get
			{
				return  mPlayerFiles;
			}
		}

		public PlayerInfo Info
		{
			get
			{
				return mPlayerInfo;
			}
		}

        public PalletKeyMap KeyMap
        {
            get
            {
                return mKeyMap;
            }
        }

		private PlayerFiles mPlayerFiles = null;
		private PlayerInfo mPlayerInfo = null;
        private PalletKeyMap mKeyMap = null;
	}
}
