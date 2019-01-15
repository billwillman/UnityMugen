using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;

// 动画驱动
[RequireComponent(typeof(Animation))]
public class ImageAnimation : MonoBehaviour {

    public void ResetState()
    {
        m_State = PlayerState.psNone;
        var ani = this.CacheAnimation;
        if (ani != null && ani.isPlaying)
            ani.Stop();
    }

    // 播放角色动画
    public bool PlayerPlayerAni(string playerName, PlayerState state, bool isLoop = true)
    {
        if (m_State == state)
            return true;

        ResetAnimation();
        if (string.IsNullOrEmpty(playerName))
            return false;

        PlayerDisplay displayer = GetComponent<PlayerDisplay>();
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
        }
        return ret;
    }

	public ImageFrame GetCurImageFrame(out ActionFlip flip)
	{
		flip = ActionFlip.afNone;
		var frameList = GetImageFrameList ();
		if (frameList == null || frameList.Count <= 0)
			return null;
		var aniNode = CurAniNode;
		if (aniNode.frameIndex < 0 || aniNode.frameIndex >= frameList.Count)
			return null;
		var frame = frameList[aniNode.frameIndex];
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

	public List<ImageFrame> GetImageFrameList()
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

		return imgLib.GetImageFrameList(this.m_State);
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
        DoChangeFrame();

        return true;
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

    private float CalcCurrentAnimationTime()
    {
        if (m_FrameList == null || m_FrameList.Count <= 0)
            return -1;
        int curFrame = m_CurFrame;
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

    public bool UpdateFrame(int frameIndex)
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
        if (oldFrame != m_CurFrame)
        {
            if (m_CurFrame >= 0 && m_CurFrame < m_FrameList.Count)
            {
                var node = m_FrameList[m_CurFrame];
                if (node.isLoopStart)
                {
                    var aniCtl = this.CacheAnimation;
                    if (aniCtl != null && aniCtl.isPlaying && aniCtl.clip != null)
                    {
                        if (m_LoopStart != m_CurFrame)
                        {
                            var info = aniCtl[_cPlayAnimationName];
                            if (info != null)
                            {
                                m_LoopStart = m_CurFrame;
                                m_LoopStartAniTime = info.time;
                            }

                        }
                    }
                }
            }

            DoChangeFrame();
        }
        return true;
    }

    public bool NextFrame()
    {
        return UpdateFrame(CurFrame + 1);
    }

	public bool PrevFrame()
	{
		return UpdateFrame(m_CurFrame - 1);
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
						if (c == mAniClip)
						{
							isFound = true;
							break;
						}
					}
				}

				if (isFound)
					m_Animation.RemoveClip(mAniClip);
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
    }

    public PlayerState State
    {
        get
        {
            return m_State;
        }
    }

    // 当前帧
    private static float _cImageAnimationScale = 0.03f;
    private int m_CurFrame = -1;
	private int m_LoopStart = -1;
    private float m_LoopStartAniTime = -1;
    private Animation m_Animation = null;
    private AnimationClip m_Clip = null;
    private GameObject m_GameObj = null;
    private PlayerState m_State = PlayerState.psNone;
    private List<ImageAnimateNode> m_FrameList = null;
    private bool m_IsLoop = false;
    // 动画文件名
    private static string _cPlayAnimationName = "Mugen Player";
}
