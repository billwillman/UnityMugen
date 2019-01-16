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

	public class BgStaticInfo
	{
		public int srpiteno_Group = -1;
		public int spriteno_Image = -1;
		public int start_x;
		public int start_y;
		public Vector2 delta;
        public MaskType mask;
		public Vector2 velocity;
		public int tile_x;
		public int tile_y;
		public int tilespacing_x;
		public int tilespacing_y;
		public int zoffset;
		public int layerno = -1;
        public TransType transType;
	}

    public class BgAniInfo
    {
        public int actionno = -1;
        public int layerno = -1;
        public int start_x;
        public int start_y;
        public Vector2 delta;
        public TransType transType;
        public MaskType mask;
    }

    public class BgConfig
    {
        private List<BgStaticInfo> m_StaticInfoList = null;
        private List<BgAniInfo> m_AniInfoList = null;  
 
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
                            if (m_StaticInfoList == null)
                                m_StaticInfoList = new List<BgStaticInfo>();
                            m_StaticInfoList.Add(staticInfo);
                            continue;
                        }
                        else if (string.Compare(value, "anim", true) == 0)
                        {
                            bgType = BgType.anim;
                            aniInfo = new BgAniInfo();
                            if (m_AniInfoList == null)
                                m_AniInfoList = new List<BgAniInfo>();
                            m_AniInfoList.Add(aniInfo);
                            continue;
                        }
                    }

                    if (bgType == BgType.none)
                        continue;
                    
                    if (bgType == BgType.normal)
                    {
                        string[] arr = null;
                        if (string.Compare(key, "spriteno", true) == 0)
                        {
                            arr = value.Split(ConfigSection._cContentArrSplit, System.StringSplitOptions.RemoveEmptyEntries);
                            if (arr != null && arr.Length >= 2)
                            {
                                staticInfo.srpiteno_Group = int.Parse(arr[0]);
                                staticInfo.spriteno_Image = int.Parse(arr[1]);
                            }
                        } else if (string.Compare(key, "layerno", true) == 0)
                        {
                            staticInfo.layerno = int.Parse(value);
                        } else if (string.Compare(key, "start", true) == 0)
                        {
                            arr = value.Split(ConfigSection._cContentArrSplit, System.StringSplitOptions.RemoveEmptyEntries);
                            if (arr != null && arr.Length >= 2)
                            {
                                staticInfo.start_x = int.Parse(arr[0]);
                                staticInfo.start_y = int.Parse(arr[1]);
                            }
                        } else if (string.Compare(key, "delta", true) == 0)
                        {
                            arr = value.Split(ConfigSection._cContentArrSplit, System.StringSplitOptions.RemoveEmptyEntries);
                            if (arr != null && arr.Length >= 2)
                            {
                                float x1 = float.Parse(arr[0]);
                                float y1 = float.Parse(arr[1]);
                                staticInfo.delta = new Vector2(x1, y1);
                            }
                        }
                        else if (string.Compare(key, "trans", true) == 0)
                        {
                            if (string.Compare(value, "none", true) == 0)
                            {
                                staticInfo.transType = TransType.none;
                            }
                        } else if (string.Compare(key, "mask", true) == 0)
                        {
                            int i1 = int.Parse(value);
                            staticInfo.mask = (MaskType)i1;
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

            return true;
        }

        private static readonly string _cBG = "BG";
    }
}
