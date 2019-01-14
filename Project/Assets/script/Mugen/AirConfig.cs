using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Mugen
{
	public enum ActionFlip
	{
		afNone, 
		afH,
		afV,
		afHV
	}

	public struct ActionFrame
	{
		public int Group;
		public int Index;
		public int Tick;
		public ActionFlip Flip;
        public bool IsLoopStart;
        // 防御盒
        public Rect[] localClsn2Arr;
        public Rect[] defaultClsn2Arr;
        public Rect[] localCls1Arr;
	}

	public class BeginAction
	{
		private static readonly string _cClsn2Default = "Clsn2Default:";
        private static readonly string _cClsn2 = "Clsn2:";
        private static readonly string _cClsn1 = "Clsn1:";

        private static readonly string _cClsn2DKeyName = "Clsn2";
        private static readonly string _cClsn1DKeyName = "Clsn1";

        private bool ReadClsn(out Rect[] clsn2DArr, ConfigSection section, ref int aniStartIdx, string clsnName, string clsnKeyName)
        {
            clsn2DArr = null;
            if (string.IsNullOrEmpty(clsnName))
                return false;
            string str = section.GetContent(aniStartIdx);
            if (string.IsNullOrEmpty(str))
            {
                clsn2DArr = null;
                return false;
            }

            bool ret = false;
            if (str.StartsWith(clsnName))
            {
                string defClsStr = str.Substring(clsnName.Length).Trim();
                int defClsCnt;
                if (!int.TryParse(defClsStr, out defClsCnt))
                {
                    clsn2DArr = null;
                    return false;
                }
                if (defClsCnt > 0)
                {
                    clsn2DArr = new Rect[defClsCnt];
                    for (int i = aniStartIdx + 1; i <= aniStartIdx + defClsCnt; ++i)
                    {
                        string key;
                        string value;
                        if (section.GetKeyValue(i, out key, out value))
                        {
                            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                            {
                                int idx = key.IndexOf(clsnKeyName, StringComparison.CurrentCultureIgnoreCase);
                                if (idx >= 0)
                                {
                                    key = key.Substring(idx + clsnKeyName.Length);
                                    int startIdx = key.IndexOf("[");
                                    int endIdx = key.IndexOf("]");
                                    if (startIdx >= 0 && endIdx >= 0 && endIdx > startIdx + 1)
                                    {
                                        string idxStr = key.Substring(startIdx + 1, endIdx - startIdx - 1);
                                        if (!string.IsNullOrEmpty(idxStr))
                                        {
                                            idxStr = idxStr.Trim();
                                            if (!string.IsNullOrEmpty(idxStr))
                                            {
                                                int index = int.Parse(idxStr);
                                                if (index >= 0 && index < clsn2DArr.Length)
                                                {
                                                    string[] values = value.Split(ConfigSection._cContentArrSplit, StringSplitOptions.RemoveEmptyEntries);
                                                    if (values != null && values.Length > 0)
                                                    {
                                                        Rect r = new Rect();
                                                        if (values.Length >= 4)
                                                        {
                                                            string v = values[0].Trim();
                                                            int left = int.Parse(v);
                                                            v = values[1].Trim();
                                                            int top = int.Parse(v);
                                                            v = values[2].Trim();
                                                            int right = int.Parse(v);
                                                            v = values[3].Trim();
                                                            int bottom = int.Parse(v);
                                                            r.min = new Vector2(left, top);
                                                            r.max = new Vector2(right, bottom);
                                                        }
                                                        clsn2DArr[index] = r;
                                                        ret = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    aniStartIdx += defClsCnt + 1;
                }
            }

            if (!ret)
            {
                clsn2DArr = null;
            }

            return ret;
        }

		public BeginAction(PlayerState state, ConfigSection section)
		{
			this.State = state;
			if (section != null)
			{
                int aniStartIdx = 0;
                Rect[] clsn2DefaultArr = null;

                ReadClsn(out clsn2DefaultArr, section, ref aniStartIdx, _cClsn2Default, _cClsn2DKeyName);

                
                string str = section.GetContent(aniStartIdx);
                Rect[] frameClsn2 = null;
                Rect[] frameClsn1 = null;
				if (!string.IsNullOrEmpty(str))
				{
					List<string> arr = new List<string>();
                    bool isLoopStart = false;
                    int i = aniStartIdx;
                    while (i < section.ContentListCount)
					{
						arr.Clear();

                        string line = section.GetContent(i);
                        if (string.IsNullOrEmpty(line))
                        {
                            ++i;
                            continue;
                        }

                        if (line.StartsWith("Loopstart", StringComparison.CurrentCultureIgnoreCase))
                        {
                            isLoopStart = true;
                            ++i;
                            continue;
                        }

                        // 说明需要设置默认clsDefault
                        if (line.StartsWith(_cClsn2Default))
                        {
                            aniStartIdx = i;
                            if (ReadClsn(out clsn2DefaultArr, section, ref aniStartIdx, _cClsn2Default, _cClsn2DKeyName))
                                i = aniStartIdx;
                            else
                                ++i;
                            continue;
                        }

                        if (line.StartsWith(_cClsn2))
                        {
                            aniStartIdx = i;
                            if (ReadClsn(out frameClsn2, section, ref aniStartIdx, _cClsn2, _cClsn2DKeyName))
                                i = aniStartIdx;
                            else
                                ++i;
                            continue;
                        }

                        if (line.StartsWith(_cClsn1))
                        {
                            aniStartIdx = i;
                            if (ReadClsn(out frameClsn1, section, ref aniStartIdx, _cClsn1, _cClsn1DKeyName))
                                i = aniStartIdx;
                            else
                                ++i;
                            continue;
                        }

						if (section.GetArray(i, arr))
						{
							if (arr.Count >= 5)
							{
								int ImageGroup = int.Parse(arr[0].Trim());
								int ImageIndex = int.Parse(arr[1].Trim());
								int Tick = int.Parse(arr[4].Trim());
								bool hasFlip = arr.Count >= 6;
								ActionFlip flipMode = ActionFlip.afNone;
								if (hasFlip)
								{
									string flipStr = arr[5].Trim();
									if (string.Compare(flipStr, "H", true) == 0)
										flipMode = ActionFlip.afH;
									else if (string.Compare(flipStr, "V", true) == 0)
										flipMode = ActionFlip.afV;
									else if (string.Compare(flipStr, "HV", true) == 0 ||
									         string.Compare(flipStr, "VH", true) == 0)
										flipMode = ActionFlip.afHV;
								}

								ActionFrame frame = new ActionFrame();
								frame.Group = ImageGroup;
								frame.Index = ImageIndex;
								frame.Tick = Tick;
								frame.Flip = flipMode;
                                if (aniStartIdx > 0 && clsn2DefaultArr != null)
                                {
                                    frame.defaultClsn2Arr = clsn2DefaultArr;
                                    clsn2DefaultArr = null;
                                }
                                if (frameClsn2 != null)
                                {
                                    frame.localClsn2Arr = frameClsn2;
                                    frameClsn2 = null;
                                }
                                if (frameClsn1 != null)
                                {
                                    frame.localCls1Arr = frameClsn1;
                                    frameClsn1 = null;
                                }
                                if (isLoopStart)
                                {
                                    frame.IsLoopStart = isLoopStart;
                                    isLoopStart = false;
                                }
								ActionFrameList.Add(frame);
							}
						}
                        ++i;
					}
				}
			}
		}

		public PlayerState State
		{
			get;
			protected set;
		}

		public int ActionFrameListCount
		{
			get
			{
				if (mActionFrameList == null)
					return 0;
				return mActionFrameList.Count;
			}
		}

		public bool GetFrame(int index, out ActionFrame frame)
		{
			frame = new ActionFrame();
			if (mActionFrameList == null)
				return false;
			if ((index < 0) || (index >= mActionFrameList.Count))
				return false;
			frame = mActionFrameList[index];
			return true;
		}

		protected List<ActionFrame> ActionFrameList
		{
			get
			{
				if (mActionFrameList == null)
					mActionFrameList = new List<ActionFrame>();
				return mActionFrameList;
			}
		}

		private List<ActionFrame> mActionFrameList = null;
	}

	public class AirConfig
	{
		public AirConfig(string playerName, string customName = "")
		{
			mIsVaild = LoadPlayer(playerName, customName);
		}

		public string PlayerName
		{
			get
			{
				return mPlayerName;
			}
		}

		public bool IsVaild
		{
			get
			{
				return mIsVaild;
			}
		}

		protected bool LoadPlayer(string playerName, string customName = "")
		{
			mPlayerName = playerName;
			if (string.IsNullOrEmpty(mPlayerName))
				return false;

			ConfigReader reader = new ConfigReader();
			if (string.IsNullOrEmpty (customName))
				customName = playerName;
			string fileName = string.Format("{0}@{1}/{2}.air.txt", AppConfig.GetInstance().PlayerRootDir, playerName, customName);
			string str = AppConfig.GetInstance().Loader.LoadText(fileName);
			if (string.IsNullOrEmpty(str))
				return false;
			reader.LoadString(str);

			// load Begin Action
			for (int i = 0; i < reader.SectionCount; ++i)
			{
				ConfigSection section = reader.GetSections(i);
				if (section == null)
					continue;
				string tile = section.Tile;
				if (tile.StartsWith(_cBeginAction))
				{
					string stateStr = tile.Substring(_cBeginAction.Length).Trim();
					int state;
					if (int.TryParse(stateStr, out state) && (state >= 0) && (state < (int)PlayerState.psPlayerStateCount))
					{
						PlayerState playerState = (PlayerState)state;
						BeginAction action = new BeginAction(playerState, section);
						AddOrSetBeginAction(playerState, action);
					}
				}
			}

			return true;
		}

		protected void AddOrSetBeginAction(PlayerState state, BeginAction action)
		{
			if (action == null)
				return;

			if (mBeginActionMap.ContainsKey(state))
			{
				mBeginActionMap[state] = action;
			} else
			{
				mBeginActionMap.Add(state, action);
			}
		}

		public BeginAction GetBeginAction(PlayerState state)
		{
			BeginAction ret;
			if (!mBeginActionMap.TryGetValue(state, out ret))
				ret = null;
			return ret;
		}

        public Dictionary<PlayerState, BeginAction>.Enumerator GetStateIter()
        {
            return mBeginActionMap.GetEnumerator();
        }

		private string mPlayerName = string.Empty;
		private static readonly string _cBeginAction = "Begin Action";
		private Dictionary<PlayerState, BeginAction> mBeginActionMap = new Dictionary<PlayerState, BeginAction>();
		private bool mIsVaild = false;
	}
}