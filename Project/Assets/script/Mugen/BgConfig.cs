using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mugen
{
    public enum BgType
    {
        none,
        normal,
        anim
    }

    public enum TransType
    {
        none
    }

    public enum MaskType
    {
        none = 0,
        alpha = 1
    }

    public class IBg
    {
        public BgType bgType = BgType.none;
        public int layerno = -1;
        public string name;
		public int start_x;
		public int start_y;
    }

    public class BgStaticInfo : IBg
	{
		public int srpiteno_Group = -1;
		public int spriteno_Image = -1;
		public Vector2 delta;
        public MaskType mask;
		public Vector2 velocity;
		public int tile;
		public int tilespacing_x;
		public int tilespacing_y;
		public int zoffset;
		
        public TransType transType;
	}

    public class BgAniInfo : IBg
    {
        public int actionno = -1;
        public Vector2 delta;
        public TransType transType;
        public MaskType mask;
    }

    public class BgConfig
    {
        private List<IBg> m_BgList = null;

        private static int m_BgState = -2;

        public static int NewBgState()
        {
            int ret = m_BgState;
            --m_BgState;
            return ret;
        }

        public int BgCount
        {
            get
            {
                if (m_BgList == null)
                    return 0;
                return m_BgList.Count;
            }
        }

        public IBg GetBg(int index)
        {
            if (m_BgList == null)
                return null;
            if (index < 0 || index >= m_BgList.Count)
                return null;
            return m_BgList[index];
        }

        private static int OnBgSort(IBg b1, IBg b2)
        {
            return b1.layerno - b2.layerno;
        }

        public void SortBg()
        {
            if (m_BgList == null || m_BgList.Count <= 0)
                return;
            m_BgList.Sort(OnBgSort);
        }
 
        public bool LoadFromReader(ConfigReader reader)
        {
            if (reader == null)
                return false;

            for (int i = 0; i < reader.SectionCount; ++i)
            {
                var section = reader.GetSections(i);
                if (section == null)
                    continue;
                if (string.IsNullOrEmpty(section.Tile))
                    continue;
                if (!section.Tile.StartsWith(_cBG, System.StringComparison.CurrentCultureIgnoreCase))
                    continue;

                string name = section.Tile.Substring(_cBG.Length).Trim();
                BgStaticInfo staticInfo = null;
                BgAniInfo aniInfo = null;
                BgType bgType = BgType.none;
                for (int j = 0; j < section.ContentListCount; ++j)
                {
                    string key, value;
                    if (!section.GetKeyValue(j, out key, out value))
                        continue;
                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                        continue;
                    if (key.StartsWith("type", System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (string.Compare(value, "normal", true) == 0)
                        {
                            bgType = BgType.normal;
                            staticInfo = new BgStaticInfo();
                            staticInfo.name = name;
                            staticInfo.bgType = bgType;
                            if (m_BgList == null)
                                m_BgList = new List<IBg>();
                            m_BgList.Add(staticInfo);
                            continue;
                        }
                        else if (string.Compare(value, "anim", true) == 0)
                        {
                            bgType = BgType.anim;
                            aniInfo = new BgAniInfo();
                            aniInfo.name = name;
                            aniInfo.bgType = bgType;
                            if (m_BgList == null)
                                m_BgList = new List<IBg>();
                            m_BgList.Add(aniInfo);
                            continue;
                        }
                    }

                    if (bgType == BgType.none)
                        continue;
                    
                    if (bgType == BgType.normal)
                    {
                        string[] arr = null;
						if (string.Compare (key, "spriteno", true) == 0) {
							arr = value.Split (ConfigSection._cContentArrSplit, System.StringSplitOptions.RemoveEmptyEntries);
							if (arr != null && arr.Length >= 2) {
								staticInfo.srpiteno_Group = int.Parse (arr [0]);
								staticInfo.spriteno_Image = int.Parse (arr [1]);
							}
						} else if (string.Compare (key, "layerno", true) == 0) {
							staticInfo.layerno = int.Parse (value);
						} else if (string.Compare (key, "start", true) == 0) {
							arr = value.Split (ConfigSection._cContentArrSplit, System.StringSplitOptions.RemoveEmptyEntries);
							if (arr != null && arr.Length >= 2) {
								staticInfo.start_x = int.Parse (arr [0]);
								staticInfo.start_y = int.Parse (arr [1]);
							}
						} else if (string.Compare (key, "delta", true) == 0) {
							arr = value.Split (ConfigSection._cContentArrSplit, System.StringSplitOptions.RemoveEmptyEntries);
							if (arr != null && arr.Length >= 2) {
								float x1 = float.Parse (arr [0]);
								float y1 = float.Parse (arr [1]);
								staticInfo.delta = new Vector2 (x1, y1);
							}
						} else if (string.Compare (key, "trans", true) == 0) {
							if (string.Compare (value, "none", true) == 0) {
								staticInfo.transType = TransType.none;
							}
						} else if (string.Compare (key, "mask", true) == 0) {
							int i1 = int.Parse (value);
							staticInfo.mask = (MaskType)i1;
						} else if (string.Compare (key, "tile", true) == 0) {
							int i2 = int.Parse (value);
							staticInfo.tile = i2;
						}
                    }
                    else if (bgType == BgType.anim)
                    {
                        string[] arr = null;
                        if (string.Compare(key, "actionno", true) == 0)
                        {
                            aniInfo.actionno = int.Parse(value);
                        } else if (string.Compare(key, "layerno", true) == 0)
                        {
                            aniInfo.layerno = int.Parse(value);
                        } else if (string.Compare(key, "start", true) == 0)
                        {
                            arr = value.Split(ConfigSection._cContentArrSplit, System.StringSplitOptions.RemoveEmptyEntries);
                            if (arr != null && arr.Length >= 2)
                            {
                                aniInfo.start_x = int.Parse(arr[0]);
                                aniInfo.start_y = int.Parse(arr[1]);
                            }
                        } else if (string.Compare(key, "delta", true) == 0)
                        {
                            arr = value.Split(ConfigSection._cContentArrSplit, System.StringSplitOptions.RemoveEmptyEntries);
                            if (arr != null && arr.Length >= 2)
                            {
                                float x1 = float.Parse(arr[0]);
                                float y1 = float.Parse(arr[1]);
                                aniInfo.delta = new Vector2(x1, y1);
                            }
                        } else if (string.Compare(key, "trans", true) == 0)
                        {
                            if (string.Compare(value, "none", true) == 0)
                                aniInfo.transType = TransType.none;
                        } else if (string.Compare(key, "mask", true) == 0)
                        {
                            int i1 = int.Parse(value);
                            aniInfo.mask = (MaskType)i1;
                        }
                    }
                }
            }

            // 排序
            SortBg();
            return true;
        }

        private static readonly string _cBG = "BG ";
    }
}
