using UnityEngine;
using System.Collections;
using Mugen;

[RequireComponent(typeof(ImageAnimation))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerDisplay : BaseResLoader {

    private DefaultLoaderPlayer m_LoaderPlayer = null;
	private Material m_OrgSpMat = null;

    public DefaultLoaderPlayer LoaderPlayer
    {
        get
        {
            return m_LoaderPlayer;
        }
    }

    public GlobalPlayer GPlayer
    {
        get
        {
            if (m_LoaderPlayer == null)
                return null;
            if (!GlobalConfigMgr.GetInstance().HasLoadPlayer(m_LoaderPlayer))
                return null;
            GlobalPlayerLoaderResult result;
            GlobalPlayer ret = GlobalConfigMgr.GetInstance().LoadPlayer(m_LoaderPlayer, out result);
            return ret;
        }
    }

	void InitSpriteRender()
	{
		var sp = this.SpriteRender;
		if (sp != null) {
			LoadMaterial(ref m_OrgSpMat, AppConfig.GetInstance ().PalleetMatFileName);
			if (m_OrgSpMat != null) {
				Material mat = GameObject.Instantiate (m_OrgSpMat);
				AddOrSetInstanceMaterialMap (sp.GetInstanceID (), mat);
				sp.sharedMaterial = mat;
			}
		}
	}

	void Awake()
	{
		InitSpriteRender ();
	}

    public void Init(DefaultLoaderPlayer loaderPlayer)
    {
        if (m_LoaderPlayer != null)
            Clear();
        m_LoaderPlayer = loaderPlayer;
    }

    public PlayerState AnimationState
    {
        get
        {
            ImageAnimation ani = this.ImageAni;
            if (ani == null)
                return PlayerState.psNone;
            return ani.State;
        }
    }

    public bool PlayAni(PlayerState state, bool isLoop = true)
    {
        var playerName = this.PlayerName;
        if (string.IsNullOrEmpty(playerName))
            return false;
        var ani = this.ImageAni;
        if (ani == null)
            return false;
        return ani.PlayerPlayerAni(playerName, state, isLoop);
    }

    public string PlayerName
    {
        get
        {
            if (m_LoaderPlayer == null)
                return string.Empty;
            return m_LoaderPlayer.GetPlayerName();
        }
    }

    public void Clear(bool isResetLoaderPlayer = true)
    {
        if (m_ImgAni != null)
        {
            m_ImgAni.Clear();
        }

        if (isResetLoaderPlayer)
            m_LoaderPlayer = null;
    }

	public int ImageCurrentFrame
	{
		get {
			ImageAnimation ani = this.ImageAni;
			if (ani == null)
				return -1;
			return ani.CurFrame;
		}
	}

    public ImageAnimation ImageAni
    {
        get
        {
            if (m_ImgAni == null)
                m_ImgAni = GetComponent<ImageAnimation>();
            return m_ImgAni;
        }
    }

	public SpriteRenderer SpriteRender
	{
		get {
			if (m_SpriteRender == null)
				m_SpriteRender = GetComponent<SpriteRenderer> ();
			return m_SpriteRender;
		}
	}

	void UpdateRenderer(ImageFrame frame, ActionFlip flip)
	{
		SpriteRenderer r = this.SpriteRender;
		if (r == null)
			return;

		r.sprite = frame.Data;
		if (r.sprite != null)
		{
			Transform trans = r.transform;
			Quaternion quat = trans.localRotation;
			switch(flip)
			{
			case ActionFlip.afH:
				quat.eulerAngles += new Vector3(0, 180, 0);
				break;
			case ActionFlip.afV:
				quat.eulerAngles += new Vector3(180, 0, 0);
				break;
			case ActionFlip.afHV:
				quat.eulerAngles += new Vector3(180, 180, 0);
				break;
			default:
				quat.eulerAngles = Vector3.zero;
				break;
			}

			trans.localRotation = quat;
		}

		Material mat = r.sharedMaterial;
		if (mat != null) {
			//mat.SetTexture ("_PalletTex", frame.PalletTexture);
			mat.SetTexture ("_MainTex", frame.Data.texture);
		}
	}

	void OnImageAnimationFrame(ImageAnimation target)
	{
		if (target == null)
			return;
		SpriteRenderer r = this.SpriteRender;
		if (r == null)
			return;
		var frameList = target.GetImageFrameList ();
		if (frameList == null || frameList.Count <= 0)
			return;
		var aniNode = target.CurAniNode;
		if (aniNode.frameIndex < 0 || aniNode.frameIndex >= frameList.Count)
			return;
		var frame = frameList[aniNode.frameIndex];
		if (frame == null)
			return;
		UpdateRenderer(frame, aniNode.flipTag);
	}

    private ImageAnimation m_ImgAni = null;
	private SpriteRenderer m_SpriteRender = null;
}
