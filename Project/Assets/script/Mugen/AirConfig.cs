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
	}

	public class BeginAction
	{
		private static readonly string _cClsn2Default = "Clsn2Default:";

		public BeginAction(PlayerState state, ConfigSection section)
		{
			this.State = state;
			if (section != null)
			{
				string str = section.GetContent(0);
				Rect[] clsn2DefaultArr = null;
				if (!string.IsNullOrEmpty(str))
				{
					int aniStartIdx = 0;
					if (str.StartsWith(_cClsn2Default))
					{
						string defClsStr = str.Substring(_cClsn2Default.Length).Trim();
						int defClsCnt;
						if (!int.TryParse(defClsStr, out defClsCnt))
							return;

						// default clsn2
						if (defClsCnt > 0)
						{
							clsn2DefaultArr = new Rect[defClsCnt];
							for (int i = 1; i <= defClsCnt; ++i)
							{
								string key;
								string value;
								if (section.GetKeyValue(i, out key, out value))
								{
									// 未完

								}
							}
						}

						aniStartIdx = defClsCnt + 1;
					}

					List<string> arr = new List<string>();
					for (int i = aniStartIdx; i < section.ContentListCount; ++i)
					{
						arr.Clear();
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
								ActionFrameList.Add(frame);
							}
						}
					}
				}
			}
		}

		// def
		protected List<Rect> Clsn2List
		{
			get
			{
				if (mClsn2List == null)
					mClsn2List = new List<Rect>();
				return mClsn2List;
			}
		}

		// attack
		protected List<Rect> Clsn1Rect
		{
			get
			{
				if (mClsn1List == null)
					mClsn1List = new List<Rect>();
				return mClsn1List;
			}
		}

		// def
		public bool HasClsn2
		{
			get
			{
				return (mClsn2List != null) && (mClsn2List.Count > 0);
			}
		}

		// attack
		public bool HasClsn1
		{
			get
			{
				return (mClsn1List != null) && (mClsn1List.Count > 0);
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

		// attack
		private List<Rect> mClsn1List = null;
		// def
		private List<Rect> mClsn2List = null;

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

		private string mPlayerName = string.Empty;
		private static readonly string _cBeginAction = "Begin Action";
		private Dictionary<PlayerState, BeginAction> mBeginActionMap = new Dictionary<PlayerState, BeginAction>();
		private bool mIsVaild = false;
	}
}