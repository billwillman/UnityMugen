using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;

// 动画驱动
[RequireComponent(typeof(Animation))]
public class ImageAnimation : MonoBehaviour {

    private void AniTimeUpdate()
    {
        if (m_AniTotalTime > 0)
        {
            m_AniUsedTime += Time.unscaledDeltaTime;
            CacheAnimation.SendMessage("OnImageAniTimeUpdate", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void LateUpdate()
    {
        AniTimeUpdate();
    }

    public void ResetState()
    {
        m_State = PlayerState.psNone;
        var ani = this.CacheAnimation;
        if (ani != null && ani.isPlaying)
            ani.Stop();
    }

    public bool HasImage(PlayerState group, int index)
    {
        if (group == PlayerState.psNone)
            return false;

        PlayerDisplay displayer = this.CacheDisplayer;
        if (displayer == null)
            return false;
        var loaderPlayer = displayer.LoaderPlayer;
        if (loaderPlayer == null)
            return false;
        var imgRes = loaderPlayer.ImageRes;
        if (imgRes == null)
            return false;
        var imgLib = imgRes.ImgLib;
        if (imgLib == null)
        {
            imgRes.Init();
            imgLib = imgRes.ImgLib;
            if (imgLib == null)
                return false;
        }

        var frame = imgLib.GetImageFrame(group, index);
        if (frame != null && frame.Data != null)
            return true;

        return false;
    }

    public bool HasImage(int group, int index)
    {
        return HasImage((PlayerState)group, index);
    }

    /*
    // 是否包含這個動作的資源
    public bool HasStateImage(PlayerState group, bool isCheckTex =false)
    {
        if (group == PlayerState.psNone)
            return false;

        PlayerDisplay displayer = this.CacheDisplayer;
        if (displayer == null)
            return false;
        var loaderPlayer = displayer.LoaderPlayer;
        if (loaderPlayer == null)
            return false;
        var imgRes = loaderPlayer.ImageRes;
        if (imgRes == null)
            return false;
        var imgLib = imgRes.ImgLib;
        if (imgLib == null)
        {
            imgRes.Init();
            imgLib = imgRes.ImgLib;
            if (imgLib == null)
                return false;
        }
        var list = imgLib.GetImageFrameList(group);
        bool ret = list != null && list.Count > 0;
        if (ret && isCheckTex)
        {
            ret = false;
            for (int i = 0; i < list.Count; ++i)
            {
                var frame = list[i];
                if (frame == null)
                    continue;
                if (frame.Data != null)
                {
                    ret = true;
                    break;
                }
            }
        }
        return ret;
    }
    */

	public void Pause(bool isPause)
	{
		bool isEnable = !isPause;
		var ani = this.CacheAnimation;
		if (ani != null && ani.enabled != isEnable)
			ani.enabled = isEnable;
		//var info = ani [_cPlayAnimationName];
	}

	public void SetLimitFrame(int startFrame = -1, int endFrame = -1, bool checkCurrentFrame = true)
	{
		m_LimitStartFrame = startFrame;
		m_LimitEndFrame = endFrame;
		if (checkCurrentFrame) {
			int oldFrame = m_CurFrame;
			bool isChanged = CheckCurrentFrameLimit ();
			if (isChanged && oldFrame != m_CurFrame) {
				var ani = this.CacheAnimation;
				if (ani != null) {
					Pause (true);
					if (ani.clip != null) {
						var info = ani [_cPlayAnimationName];
						if (info != null) {
							float t = CalcAnimationTime (m_CurFrame);
							info.time = t;
						}
					}
				}
				DoChangeFrame ();
			}
		}
	}

	public bool PlayerPlayerAni(PlayerState state, int startFrame, int endFrame, bool isLoop = true)
	{
		SetLimitFrame (startFrame, endFrame, false);
		if (m_State == state) {
			if (CacheAnimation.enabled && !CacheAnimation.isPlaying)
			{
				CacheAnimation.Play();
			}
			return true;
		}

		ResetAnimation();

		PlayerDisplay displayer = this.CacheDisplayer;
		if (displayer == null)
			return false;
		var loaderPlayer = displayer.LoaderPlayer;
		if (loaderPlayer == null)
			return false;

		var imgRes = loaderPlayer.ImageRes;
		if (imgRes == null)
			return false;
		var imgLib = imgRes.ImgLib;
		if (imgLib == null)
		{
			imgRes.Init();
			imgLib = imgRes.ImgLib;
			if (imgLib == null)
				return false;
		}


		m_FrameList = imgLib.GetAnimationNodeList(state);
		bool ret = DoInitAnimation();
		if (ret)
		{
			m_IsLoop = isLoop;
			m_State = state;

			CacheAnimation.Stop ();
			if(isLoop)
				CacheAnimation.wrapMode = WrapMode.Loop;
			else
				CacheAnimation.wrapMode = WrapMode.Once;
			CacheAnimation.Play();
		} else
		{
			m_IsLoop = isLoop;
			m_State = state;

			DoEndFrame();
		}

		return ret;
	}

    // 播放角色动画
    public bool PlayerPlayerAni(PlayerState state, bool isLoop = true)
    {
		return PlayerPlayerAni (state, -1, -1, isLoop);
    }

	public ImageFrame GetCurImageFrame(out ActionFlip flip)
	{
		flip = ActionFlip.afNone;
        var aniNode = CurAniNode;
        if (aniNode.frameGroup < 0 || aniNode.frameIndex < 0)
            return null;

        var frame = GetImageFrame(aniNode.frameGroup, aniNode.frameIndex);
        if (frame != null)
            flip = aniNode.flipTag;
        return frame;
	}

    private PlayerDisplay m_CacheDisplayer = null;
    protected PlayerDisplay CacheDisplayer
    {
        get
        {
            if (m_CacheDisplayer == null)
                m_CacheDisplayer = GetComponent<PlayerDisplay>();
            return m_CacheDisplayer;
        }
    }

    public ImageFrame GetImageFrame(int group, int image)
    {
        PlayerDisplay displayer = this.CacheDisplayer;
        if (displayer == null)
            return null;
        var loaderPlayer = displayer.LoaderPlayer;
        if (loaderPlayer == null)
            return null;
        var imgRes = loaderPlayer.ImageRes;
        if (imgRes == null)
            return null;
        var imgLib = imgRes.ImgLib;
        if (imgLib == null)
        {
            imgRes.Init();
            imgLib = imgRes.ImgLib;
            if (imgLib == null)
                return null;
        }

        return imgLib.GetImageFrame((PlayerState)group, image);
    }

    private bool DoInitAnimation()
    {
        if (m_FrameList == null || m_FrameList.Count <= 0)
        {
            ResetAnimation();
            return false;
        }

        InitAnimationClip();
        m_CurFrame = 0;
       // DoChangeFrame();

        return true;
    }

	public void ResetFirstFrame()
	{
		Animation ctl = this.CacheAnimation;
		ctl.Stop ();
		var info = ctl [_cPlayAnimationName];
		if (info != null) {
			info.time = 0f;
		}
		m_CurFrame = 0;
		DoChangeFrame();
		ctl.Play ();
	}

    public int AniNodeCount
    {
        get
        {
            if (m_FrameList != null)
                return m_FrameList.Count;
            return 0;
        }
    }

    public ImageAnimateNode CurAniNode
    {
        get
        {
            ImageAnimateNode ret = new ImageAnimateNode();
            ret.frameIndex = -1;
            if (m_FrameList == null)
                return ret;
            if (m_CurFrame < 0 || m_CurFrame >= m_FrameList.Count)
                return ret;
            return m_FrameList[m_CurFrame];
        }
    }

    public void RefreshCurrent()
    {
        DoChangeFrame();
    }

    void DoEndFrame()
    {
        string evtName = "OnImageAnimationEndFrame";
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            CacheGameObject.SendMessage(evtName, this, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
        }
#else
		CacheGameObject.SendMessage(evtName, this, SendMessageOptions.DontRequireReceiver);
#endif
    }

    void DoChangeFrame()
    {
        string evtName = "OnImageAnimationFrame";
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            CacheGameObject.SendMessage(evtName, this, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
        }
#else
		CacheGameObject.SendMessage(evtName, this, SendMessageOptions.DontRequireReceiver);
#endif
    }

    private void InitAnimationClip()
    {
        var clip = this.AniClip;
        clip.frameRate = 30;
#if UNITY_EDITOR
#else
		clip.events = null;
#endif
        Animation ctl = this.CacheAnimation;
        bool isFound = false;
        var iter = ctl.GetEnumerator();
        while (iter.MoveNext())
        {
            AnimationClip c = iter.Current as AnimationClip;
            if (c != null)
            {
                if (c == clip)
                {
                    isFound = true;
                    break;
                }
            }
        }

        ctl.clip = null;
        if (!isFound)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                ctl.AddClip(clip, clip.name);
            }
            else
            {
                AnimationClip[] clips = new AnimationClip[1];
                clips[0] = clip;
                UnityEditor.AnimationUtility.SetAnimationClips(ctl, clips);
            }
#else
			ctl.AddClip(clip, clip.name);
#endif
        }

        ctl.clip = clip;

        List<AnimationEvent> evtList = new List<AnimationEvent>();

        string nextFrameStr = "NextFrame";
        string endFrameStr = "EndFrame";
        string firstFrameStr = "StartFrame";
        if (m_FrameList.Count >= 2)
        {
            float sumTime = 0;

            AnimationEvent evt = new AnimationEvent();
            evt.functionName = firstFrameStr;
            evt.messageOptions = SendMessageOptions.DontRequireReceiver;
            evt.time = 0;
            evtList.Add(evt);
            for (int i = 0; i < m_FrameList.Count; ++i)
            {
                ImageAnimateNode frame = m_FrameList[i];
                float evtTime = frame.AniTick * _cImageAnimationScale;
                sumTime += evtTime;
                AnimationEvent aniEvt = new AnimationEvent();
                if (i == m_FrameList.Count - 1)
                    aniEvt.functionName = endFrameStr;
                else
                    aniEvt.functionName = nextFrameStr;
                aniEvt.messageOptions = SendMessageOptions.DontRequireReceiver;
                aniEvt.time = sumTime;
                evtList.Add(aniEvt);
            }

			if (!ctl.enabled)
				ctl.enabled = true;
            m_AniTotalTime = sumTime;
        }
        else
        {
            // 直接針添加StartFrame EndFrame
            AnimationEvent evt = new AnimationEvent();
            evt.functionName = firstFrameStr;
            evt.messageOptions = SendMessageOptions.DontRequireReceiver;
            evt.time = 0;
            evtList.Add(evt);

            evt.functionName = endFrameStr;
            evt.messageOptions = SendMessageOptions.DontRequireReceiver;
            //evt.time = 1.0f/clip.frameRate;
            evt.time = Time.fixedDeltaTime;
            //	evt.time = _cLimitFrameDeltaTime;
            evtList.Add(evt);
			if (ctl.enabled)
				ctl.enabled = false;

            m_AniTotalTime = Time.fixedDeltaTime;
        }
#if UNITY_EDITOR
        if (Application.isPlaying)
            clip.events = evtList.ToArray();
        else
            UnityEditor.AnimationUtility.SetAnimationEvents(clip, evtList.ToArray());
#else
		clip.events = evtList.ToArray();
#endif

    }

    public void StartFrame()
    {
        m_LoopStart = -1;
        m_LoopStartAniTime = -1;
    }

    public bool IsLoop
    {
        get
        {
            return m_IsLoop;
        }
    }

	private float CalcAnimationTime(int ff)
	{
		if (m_FrameList == null || m_FrameList.Count <= 0)
			return -1;
		int curFrame = ff;
		if (curFrame < 0)
			curFrame = 0;
		else if (curFrame >= m_FrameList.Count)
			curFrame = m_FrameList.Count - 1;
		float ret = 0;
		for (int i = 0; i <= curFrame; ++i)
		{
			ImageAnimateNode frame = m_FrameList[i];
			float evtTime = frame.AniTick * _cImageAnimationScale;
			ret += evtTime;
		}
		return 0;
	}

    private float CalcCurrentAnimationTime()
    {
		return CalcAnimationTime (m_CurFrame);
    }

	private bool CheckFrameLimit(ref int frameIndex)
	{
		if (m_LimitStartFrame >= 0 && m_LimitEndFrame >= 0 && m_LimitStartFrame > m_LimitEndFrame)
			return false;
		int curFrameCount = this.AniNodeCount;
		int limitStart = m_LimitStartFrame;
		if (limitStart >= curFrameCount)
			limitStart = curFrameCount - 1;
		int limitEnd = m_LimitEndFrame;
		if (limitEnd >= curFrameCount)
			limitEnd = curFrameCount - 1;
		bool ret = false;
		if (limitStart >= 0) {
			if (m_CurFrame < limitStart) {
				m_CurFrame = limitStart;
				ret = true;
			}
		}

		if (limitEnd >= 0) {
			if (m_CurFrame > limitEnd) {
				m_CurFrame = limitEnd;
				ret = true;
			}
		}

		return ret;
	}

	private bool CheckCurrentFrameLimit()
	{
		return CheckFrameLimit (ref m_CurFrame);
	}

    private bool UpdateFrame(int frameIndex)
    {
        if (m_FrameList == null || m_FrameList.Count <= 0)
            return false;
        if (frameIndex < 0)
        {
            if (IsLoop)
                frameIndex = m_FrameList.Count - 1;
            else
                frameIndex = 0;
        }
        else if (frameIndex >= m_FrameList.Count)
        {
            if (IsLoop)
                frameIndex = 0;
            else
                frameIndex = m_FrameList.Count - 1;
        }
        int oldFrame = m_CurFrame;
        m_CurFrame = frameIndex;
		bool isChanged = CheckCurrentFrameLimit ();
		if (oldFrame != m_CurFrame) {
			if (m_CurFrame >= 0 && m_CurFrame < m_FrameList.Count) {
				var node = m_FrameList [m_CurFrame];

				if (isChanged) {
					var aniCtl = this.CacheAnimation;
					if (aniCtl != null && aniCtl.isPlaying && aniCtl.clip != null) {
						var info = aniCtl [_cPlayAnimationName];
						if (info != null) {
							float t = CalcAnimationTime (m_CurFrame);
							info.time = t;
						}
					}
				}

				if (node.isLoopStart) {
					var aniCtl = this.CacheAnimation;
					if (aniCtl != null && aniCtl.isPlaying && aniCtl.clip != null) {
						if (m_LoopStart != m_CurFrame) {
							var info = aniCtl [_cPlayAnimationName];
							if (info != null) {
								m_LoopStart = m_CurFrame;
								m_LoopStartAniTime = info.time;
								CheckLoopStartLimit ();
							}

						}
					}
				}
			}

			DoChangeFrame ();
		} else {
			if (isChanged) {
				var aniCtl = this.CacheAnimation;
				if (aniCtl != null && aniCtl.isPlaying && aniCtl.clip != null) {
					var info = aniCtl [_cPlayAnimationName];
					if (info != null) {
						float t = CalcAnimationTime (m_CurFrame);
						info.time = t;
					}
				}
			}
		}
        return true;
    }

	private void CheckLoopStartLimit()
	{
		if (m_LimitEndFrame >= 0 && m_LimitStartFrame >= 0 && 
			m_LimitEndFrame < m_LimitStartFrame)
			return;

		int curFrameCount = this.AniNodeCount;
		int limitStart = m_LimitStartFrame;
		if (limitStart >= curFrameCount)
			limitStart = curFrameCount - 1;
		int limitEnd = m_LimitEndFrame;
		if (limitEnd >= curFrameCount)
			limitEnd = curFrameCount - 1;

		bool isChanged = false;
		if (limitStart >= 0) {
			if (m_LoopStart < limitStart) {
				m_LoopStart = limitStart;
				isChanged = true;
			}
		}

		if (limitEnd >= 0) {
			if (m_LoopStart > limitEnd) {
				m_LoopStart = limitEnd;
				isChanged = true;
			}
		}

		if (isChanged) {
			// 重新计算时间
			m_LoopStartAniTime = CalcAnimationTime(m_LoopStart);
		}
	}

	protected int LimitEndFrame
	{
		get {
			int limitEnd = m_LimitEndFrame;
			if (limitEnd < 0)
				return limitEnd;
			int curFrameCount = this.AniNodeCount;
			if (limitEnd >= curFrameCount)
				limitEnd = curFrameCount - 1;
			return limitEnd;
		}
	}

	protected int LimitStartFrame
	{
		get {
			int limitStart = m_LimitStartFrame;
			if (limitStart < 0)
				return limitStart;
			int curFrameCount = this.AniNodeCount;
			if (limitStart >= curFrameCount)
				limitStart = curFrameCount - 1;
			return limitStart;
		}
	}

	public bool NextFrame()
	{
		return NextFrame (true);
	}

	public bool NextFrame(bool checkLimitStop)
    {
		bool ret = UpdateFrame(CurFrame + 1);
		if (ret && checkLimitStop) {
			int limitEnd = this.LimitEndFrame;
			if (limitEnd >= 0 && limitEnd == this.m_CurFrame) {
				Pause (true);
			}
		}
		return ret;
    }

	public bool PrevFrame()
	{
		return PrevFrame (true);
	}

	public bool PrevFrame(bool checkLimitStop)
	{
		bool ret = UpdateFrame(m_CurFrame - 1);
		if (ret && checkLimitStop) {
			int limitStart = this.LimitStartFrame;
			if (limitStart >= 0 && limitStart == this.m_CurFrame) {
				Pause (true);
			}
		}
		return ret;
	}

	public void EndFrame()
	{
		if ((m_FrameList == null) || (m_FrameList.Count <= 0))
			return;

		DoEndFrame();
		if (IsLoop || m_LoopStart >= 0)
		{
			int loopStart = m_LoopStart; 
			if (loopStart < 0)
				loopStart = 0;
			if (UpdateFrame (loopStart)) {
				// 移动Animation
                if (m_LoopStart >= 0 && m_LoopStartAniTime >= 0)
                {
                    var aniCtl = this.CacheAnimation;
                    if (aniCtl != null)
                    {
                        var info = aniCtl[_cPlayAnimationName];
                        if (info != null)
                            info.time = m_LoopStartAniTime;
                    }
                }
			}
		}
	}

    public void Stop()
    {
        if (m_FrameList == null || m_FrameList.Count <= 0)
            return;
        CacheAnimation.Stop();
    }

    public bool HasAniData
    {
        get
        {
            return (m_FrameList != null) && (m_FrameList.Count > 0);
        }
    }

    public bool IsPlaying
    {
        get
        {
            if (m_FrameList == null || m_FrameList.Count <= 0)
                return false;
            return CacheAnimation.isPlaying;
        }
    }

    public Animation CacheAnimation
    {
        get
        {
            if (m_Animation == null)
                m_Animation = GetComponent<Animation>();
            return m_Animation;
        }
    }

    public bool SetManualFrame(int frame)
    {
        if (this.AniNodeCount <= 0)
            return false;

        var ani = this.CacheAnimation;
        if (ani != null && ani.enabled)
        {
            ani.enabled = false;
        }

        if (frame == CurFrame)
            return true;

        if (frame < 0)
            frame = 0;
        else if (frame >= AniNodeCount)
        {
            frame = AniNodeCount - 1;
        }

        bool ret = UpdateFrame(frame);
        if (ret)
        {
            if (ani != null)
            {
                var clip = ani[_cPlayAnimationName];
                if (clip != null)
                {
                    float time = CalcCurrentAnimationTime();
                    clip.time = time;
                }
            }
        }
        return ret;
    }

    // 单位：毫秒
    public int CurAniUsedTime
    {
        get
        {
            return (int)(m_AniUsedTime * 1000f);
        }
    }

    public int CurAniRetainTime
    {
        get
        {
            int ret = (int)((m_AniUsedTime - m_AniTotalTime) * 1000f);
            if (ret > 0)
                ret = 0;
            return ret;
        }
    }


    public int CurFrame
    {
        get
        {
            return m_CurFrame;
        }
    }

    protected GameObject CacheGameObject
    {
        get
        {
            if (m_GameObj == null)
                m_GameObj = this.gameObject;
            return m_GameObj;
        }
    }

    protected AnimationClip AniClip
    {
        get
        {
            if (m_Clip == null)
            {
                m_Clip = new AnimationClip();
                m_Clip.legacy = true;
                m_Clip.name = _cPlayAnimationName;
            }
            return m_Clip;
        }
    }

    public void Clear()
    {
        if (m_Clip != null)
        {
            if (m_Animation != null)
            {
#if UNITY_EDITOR
                UnityEditor.AnimationUtility.SetAnimationClips(m_Animation, null);
#else
				bool isFound = false;
				var iter = m_Animation.GetEnumerator();
				while (iter.MoveNext())
				{
					AnimationClip c = iter.Current as AnimationClip;
					if (c != null)
					{
						if (c == m_Clip)
						{
							isFound = true;
							break;
						}
					}
				}

				if (isFound)
					m_Animation.RemoveClip(m_Clip);
#endif
                m_Animation.clip = null;
            }

            AppConfig.GetInstance().Loader.DestroyObject(m_Clip);
            m_Clip = null;
        }

        m_State = PlayerState.psNone;
        m_FrameList = null;
        m_IsLoop = false;
    }

    void OnApplicationQuit()
    {
        Clear();
    }

    void OnDestroy()
    {
        if (!AppConfig.IsAppQuit)
        {
            Clear();
        }
    }

    private void ResetAnimation()
    {
        m_CurFrame = -1;
        m_AniUsedTime = 0;
        m_AniTotalTime = 0;
    }

    public PlayerState State
    {
        get
        {
            return m_State;
        }
    }

	public float GetMugenAnimateTime()
	{
		var ani = this.CacheAnimation;
		if (ani == null)
			return 0f;
		var info = ani[_cPlayAnimationName];
		if (info == null)
			return 0f;
		float time = info.time/_cImageAnimationScale;
		return time;
	}

    // 当前帧
    private static float _cImageAnimationScale = 0.015f;
    private int m_CurFrame = -1;
    // 当前动画状态已经使用的时间
    private float m_AniUsedTime = 0f;
    // 离当前动画总时间
    private float m_AniTotalTime = 0f;
	private int m_LoopStart = -1;
    private float m_LoopStartAniTime = -1;
    private Animation m_Animation = null;
    private AnimationClip m_Clip = null;
    private GameObject m_GameObj = null;
    private PlayerState m_State = PlayerState.psNone;
    private List<ImageAnimateNode> m_FrameList = null;
    private bool m_IsLoop = false;
	// 限定动画帧范围
	private int m_LimitStartFrame = -1;
	private int m_LimitEndFrame = -1;
    // 动画文件名
    private static string _cPlayAnimationName = "Mugen Player";
}
