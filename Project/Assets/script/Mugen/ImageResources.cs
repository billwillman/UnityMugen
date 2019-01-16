using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

namespace Mugen
{
	public class ImageFrame: DisposeObject
	{
		public ImageFrame(ImageLibrary parentLib, int image, Texture2D tex, float offsetX, 
			float offsetY, string name, Texture2D localPalletTex = null)
		{
			mParentLib = parentLib;
            m_Image = image;
			mLocalPalletTex = localPalletTex;
			SetTexture2D(tex, offsetX, offsetY, name);
		}

		public Sprite Data
		{
			get;
			protected set;
		}

		private void SetTexture2D(Texture2D tex,  float offsetX, float offsetY, string name)
		{
			if (Data != null)
			{
				if (Data.texture == tex)
					return;

				DestroyTexture();
				AppConfig.GetInstance().Loader.DestroyObject(Data);
				Data = null;
			}
			if (tex == null)
				return;

			Rect r = new Rect(0, 0, tex.width, tex.height);
			Vector2 offset = new Vector2(offsetX, offsetY);
			Data = Sprite.Create(tex, r, offset);  
			Data.name = name;
		}

		protected void DestroyTexture()
		{
			if (Data != null)
			{
				Texture2D tex = Data.texture;
				//	Data.texture = null;
				if (tex != null)
				{
					AppConfig.GetInstance().Loader.DestroyObject(tex);
				}
			}
		}

		protected override void OnFree(bool isManual)
		{
			DestroyTexture();
			AppConfig.GetInstance().Loader.DestroyObject(Data);
			Data = null;
		}

		public Texture2D LocalPalletTex
		{
			get
			{
				return mLocalPalletTex;
			}
		}

		public ImageLibrary ParentLib
		{
			get
			{
				return mParentLib;
			}
		}

        public int Image
        {
            get
            {
                return m_Image;
            }
        }

		private ImageLibrary mParentLib = null;
		private Texture2D mLocalPalletTex = null;
        private int m_Image = 0;
	}

	public struct ImageAnimateNode
	{
		public PlayerState Group;
		public int frameIndex;
		public float AniTick;
		public ActionFlip flipTag;
        public bool isLoopStart;
        public Rect[] localClsn2Arr;
        public Rect[] defaultClsn2Arr;
        public Rect[] localCls1Arr;
	}

	public class ImageLibrary: DisposeObject {
		public ImageLibrary(bool is32BitPallet = true)
		{
			mIs32BitPallet = is32BitPallet;
		}

		public bool Is32BitPallet
		{
			get
			{
				return mIs32BitPallet;
			}
		}

		public List<ImageFrame> GetImageFrameList(PlayerState state)
		{
			List<ImageFrame> ret;
			if (!mGroupImageMap.TryGetValue((int)state, out ret))
				return null;
			return ret;
		}

		public ImageFrame GetImageFrame(PlayerState state, int index)
		{
			List<ImageFrame> frameList = GetImageFrameList(state);
			if (frameList == null)
				return null;
			if (index < 0)
				return null;
            for (int i = 0; i < frameList.Count; ++i)
            {
                var frame = frameList[i];
                if (frame != null && frame.Image == index)
                    return frame;
            }
            return null;
		}

		public List<ImageAnimateNode> GetAnimationNodeList(PlayerState state)
		{
			List<ImageAnimateNode> ret;
			if (!mStateAniMap.TryGetValue((int)state, out ret))
				return null;
			return ret;
		}

		// 填充那些如果不存在則用orgState的第一幀填充
		private void FillNoAction(PlayerState targetState, PlayerState orgState, int step)
		{
			bool isFound = false;
			for (int i = 0; i <= 2; ++i)
			{
				PlayerState state = (PlayerState)((int)targetState + i * step);
				if (mStateAniMap.ContainsKey((int)state))
				{
					isFound = true;
					break;
				}
			}

			if (isFound)
				return;

			for (int i = 0; i <= 2; ++i)
			{
				PlayerState state = (PlayerState)((int)orgState + i * step);
				List<ImageAnimateNode> list;
				if (mStateAniMap.TryGetValue((int)state, out list) && list != null && list.Count > 0)
				{
					List<ImageAnimateNode> newList = new List<ImageAnimateNode>();
					newList.Add(list[0]);
					PlayerState newState = (PlayerState)((int)targetState + i * step);
					mStateAniMap.Add((int)newState, newList);
					break;
				}
			}
		}

		private bool LoadAir(string charName, AirConfig airCfg)
		{
			mStateAniMap.Clear();
			if (string.IsNullOrEmpty(charName) || airCfg == null || !airCfg.IsVaild)
				return false;
            for (int i = 0; i < airCfg.GetStateCount(); ++i)
            {
                int state = (int)airCfg.GetStateByIndex(i);
                if (state == (int)PlayerState.psNone)
                    continue;
                PlayerState action = (PlayerState)state;
                BeginAction beginAction = airCfg.GetBeginAction(action);
                if (beginAction == null || beginAction.ActionFrameListCount <= 0)
                    continue;

                List<ImageFrame> frameList = this.GetImageFrameList(action);
                if (frameList == null || frameList.Count <= 0)
                    continue;

                List<ImageAnimateNode> aniNodeList;
                if (!mStateAniMap.TryGetValue((int)action, out aniNodeList))
                {
                    aniNodeList = new List<ImageAnimateNode>();
                    mStateAniMap.Add((int)action, aniNodeList);
                }

                ActionFlip lastFlip = ActionFlip.afNone;
                for (int frame = 0; frame < beginAction.ActionFrameListCount; ++frame)
                {
                    ActionFrame actFrame;
                    if (beginAction.GetFrame(frame, out actFrame))
                    {
                        if (actFrame.Index >= 0)
                        {
                            int frameIndex = actFrame.Index;
                            if (frameIndex >= frameList.Count)
                                frameIndex = frameList.Count - 1;
                            ImageAnimateNode aniNode = new ImageAnimateNode();
                            aniNode.AniTick = actFrame.Tick;
                            //aniNode.flipTag = actFrame.Flip;
                            aniNode.flipTag = lastFlip;
                            lastFlip = actFrame.Flip;
                            aniNode.frameIndex = frameIndex;
                            aniNode.Group = action;
                            aniNode.isLoopStart = actFrame.IsLoopStart;
                            aniNode.defaultClsn2Arr = actFrame.defaultClsn2Arr;
                            aniNode.localCls1Arr = actFrame.localCls1Arr;
                            aniNode.localClsn2Arr = actFrame.localClsn2Arr;
                            aniNodeList.Add(aniNode);
                        }
                    }
                }
            }

			return true;
		}

        /// <summary>
        /// 加載圖片
        /// </summary>
        /// <param name="sf">sff文件舉兵</param>
        /// <param name="group">讀取的動畫Group組</param>
        /// <param name="charName">名稱</param>
        /// <param name="startLoadImage">動畫開始的位置</param>
        /// <param name="isAniLoad">是否是自動連續加載</param>
        /// <param name="useSaveGroup">是否使用saveGroup參數</param>
        /// <param name="saveGroup">保存到MAP的Group的KEY</param>
        private void LoadCharState(SffFile sf, PlayerState group, string charName, int startLoadImage = 0, 
            bool isAniLoad = true, bool useSaveGroup = false, 
            PlayerState saveGroup = PlayerState.psNone)
        {
           // if (group == PlayerState.psPlayerStateCount)
            //    return;

            if (!useSaveGroup || saveGroup == PlayerState.psNone)
                saveGroup = group;

            SFFSUBHEADER h;
            if (!sf.GetSubHeader((int)group, startLoadImage, out h))
                return;
            KeyValuePair<PCXHEADER, PCXDATA> d;
            if (!sf.GetPcxData((uint)group, (uint)startLoadImage, out d))
                return;
            float offX = ((float)(d.Key.x + h.x)) / d.Key.widht;//+ 1.0f;
            float offY = -((float)(d.Key.y + h.y)) / d.Key.height + 1.0f;

            Texture2D tex = sf.GetIndexTexture((uint)group, (uint)startLoadImage);
            while (tex != null)
            {
                ImageFrame frame = new ImageFrame(this, startLoadImage, tex, offX, offY, charName,
                    d.Value.GetPalletTexture(mIs32BitPallet));

                AddImageFrame(saveGroup, frame);

                // 只是單針加載，不是動畫加載
                if (!isAniLoad)
                    break;

                ++startLoadImage;
                if (!sf.GetSubHeader((int)group, startLoadImage, out h))
                    break;
                if (!sf.GetPcxData((uint)group, (uint)startLoadImage, out d))
                    break;
                offX = ((float)(d.Key.x + h.x)) / d.Key.widht;//+ 1.0f;
                offY = -((float)(d.Key.y + h.y)) / d.Key.height + 1.0f;
                tex = sf.GetIndexTexture((uint)group, (uint)startLoadImage);
            }
        }

        public static PlayerState SceneGroupToSaveGroup(int group)
        {
            return (PlayerState)(-(group + 1));
        }

        public bool LoadScene(string fileName, BgConfig config)
        {
            if (string.IsNullOrEmpty(fileName) || config == null)
                return false;
            SffFile sf = new SffFile();
            if (!sf.LoadFromFileName(fileName))
                return false;
            /* 处理场景 */

            for (int i = 0; i < config.BgCount; ++i)
            {
                var bg = config.GetBg(i);
                if (bg != null)
                {
                    if (bg.bgType == BgType.normal)
                    {
                        var staticBg = bg as BgStaticInfo;
                        PlayerState saveGroup = SceneGroupToSaveGroup(staticBg.srpiteno_Group);
                        PlayerState group = (PlayerState)(staticBg.srpiteno_Group);
                        if (!HasLoadImageFrame(saveGroup, staticBg.spriteno_Image))
                            LoadCharState(sf, group, bg.name, staticBg.spriteno_Image, false, true, saveGroup);
                    }
                }
            }

            return true;
        }

		public bool LoadChar(string charName, AirConfig airCfg = null, string customSpriteName = "")
		{
			ClearAll ();
			if (string.IsNullOrEmpty (charName))
				return false;

			SffFile sf = new SffFile();
			if (!sf.LoadChar (charName, customSpriteName, false)) {
				ClearAll ();
				return false;
			}

            if (airCfg == null)
            {
                foreach (PlayerState group in PlayerStateEnumValues.GetValues())
                {
                    LoadCharState(sf, group, charName);
                }
            } else
            {
                for (int i = 0; i < airCfg.GetStateCount(); ++i)
                {
                    var key = airCfg.GetStateByIndex(i);
                    var value = airCfg.GetBeginAction(key);
                    if (value != null)
                    {
                        for (int j = 0; j < value.ActionFrameListCount; ++j)
                        {
                            ActionFrame frame;
                            if (value.GetFrame(j, out frame))
                            {
                                LoadCharState(sf, key, charName, frame.Index, false);
                            }
                        }
                    }
                }
            }

			if (airCfg != null && !LoadAir (charName, airCfg)) {
				ClearAll ();
				return false;
			}

			return true;
		}

		protected override void OnFree(bool isManual)
		{
			ClearAll();
		}

		public void ClearAll()
		{
			mStateAniMap.Clear();
			DestroyPallets();
			DestroyGroupImages();
		}

		protected void DestroyPallets()
		{
			var iter = mPalletMap.GetEnumerator();
			while (iter.MoveNext())
			{
				AppConfig.GetInstance().Loader.DestroyObject(iter.Current.Value);
			}
			iter.Dispose();
			mPalletMap.Clear();
		}

		public Dictionary<string, Texture2D>.Enumerator GetPalletMapIter()
		{
			return mPalletMap.GetEnumerator();
		}

		protected void DestroyGroupImages()
		{
			var iter = mGroupImageMap.GetEnumerator();
			while (iter.MoveNext())
			{
				if (iter.Current.Value != null)
				{
					for (int i = 0; i < iter.Current.Value.Count; ++i)
					{
						ImageFrame frame = iter.Current.Value[i];
						if (frame != null)
							frame.Dispose();
					}
					iter.Current.Value.Clear();
				}
			}
			iter.Dispose();
			mGroupImageMap.Clear();
		}

        protected bool HasLoadImageFrame(PlayerState state, int image)
        {
            if (image < 0 || state == PlayerState.psNone)
                return false;
            List<ImageFrame> frameList;
            int key = (int)state;
            if (!mGroupImageMap.TryGetValue((int)state, out frameList))
            {
                return false;
            }
            for (int i = 0; i < frameList.Count; ++i)
            {
                var frame = frameList[i];
                if (frame != null && frame.Image == image)
                {
                    return true;
                }
            }
            return false;
        }

		protected bool AddImageFrame(PlayerState state, ImageFrame frame)
		{
			if (frame == null)
				return false;
			List<ImageFrame> frameList;
            int key = (int)state;
            if (!mGroupImageMap.TryGetValue((int)state, out frameList))
			{
				frameList = new List<ImageFrame>();
                mGroupImageMap.Add(key, frameList);
			}

			frameList.Add(frame);
			return true;
		}

		public string GeneratorPalletFileName(string playerName, string palletName)
		{
			palletName = System.IO.Path.GetFileNameWithoutExtension(palletName);
			string fileName = string.Format("{0}@{1}/{2}.act.bytes", AppConfig.GetInstance().PlayerRootDir, playerName, palletName);
			return fileName;
		}

        /*
		private bool AddPalletTexture(string playerName, string palletName)
		{
			string fileName = GeneratorPalletFileName(playerName, palletName);
			return AddPalletTexture(palletName, fileName, mIs32BitPallet);
		}
         */

        public Texture2D GetPalletTexture(string playerName, string palletName)
        {
            Texture2D ret;
            if (mPalletMap.TryGetValue(palletName, out ret))
                return ret;
            string fileName = GeneratorPalletFileName(playerName, palletName);
            return GetPalletTexture(palletName, fileName, mIs32BitPallet);
        }

        private Texture2D GetPalletTexture(string palletName, string fileName, bool is32Bit)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(palletName))
                return null;
            Texture2D ret;
            if (mPalletMap.TryGetValue(palletName, out ret) && ret != null)
                return ret;
            if (!AddPalletTexture(palletName, fileName, is32Bit))
                return null;
            if (!mPalletMap.TryGetValue(palletName, out ret))
                ret = null;
            return ret;
        }

		private bool AddPalletTexture(string palletName, string fileName, bool is32Bit)
		{
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(palletName))
				return false;
			if (mPalletMap.ContainsKey(palletName))
				return true;
			byte[] source = AppConfig.GetInstance().Loader.LoadBytes(fileName);
			Texture2D tex = SffFile.GeneratorActTexture(source, is32Bit);
			if (tex == null)
				return false;
			mPalletMap.Add(palletName, tex);
			return true;
		}

		public bool AddPalletTexture(string playerName, string palletName, bool is32Bit, out string key)
		{
			key = string.Empty;
			if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(palletName))
				return false;
			string fileName = GeneratorPalletFileName(playerName, palletName);
			bool ret = AddPalletTexture(palletName, fileName, is32Bit);
			if (ret)
				key = palletName;
			return ret;
		}

		public Dictionary<int, List<ImageFrame>>.Enumerator GetGroupMapIter()
		{
			return mGroupImageMap.GetEnumerator();
		}

		public Dictionary<int, List<ImageAnimateNode>>.Enumerator GetAniMapIter()
		{
			return mStateAniMap.GetEnumerator();
		}

		// fileName, palletTexture
		private Dictionary<string, Texture2D> mPalletMap = new Dictionary<string, Texture2D>();
		// groupNumber, ImageFrame List
		private Dictionary<int, List<ImageFrame>> mGroupImageMap = new Dictionary<int, List<ImageFrame>>();
		private bool mIs32BitPallet = true;
		private Dictionary<int, List<ImageAnimateNode>> mStateAniMap = new Dictionary<int, List<ImageAnimateNode>>();
	}
}
