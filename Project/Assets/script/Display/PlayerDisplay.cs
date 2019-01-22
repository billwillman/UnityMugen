using UnityEngine;
using System.Collections;
using Mugen;

[RequireComponent(typeof(ImageAnimation))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerStateMgr))]
public class PlayerDisplay : BaseResLoader {

    private DefaultLoaderPlayer m_LoaderPlayer = null;
	private Material m_OrgSpMat = null;
	private Transform m_ClsnSpriteRoot = null;
	private Rect[] m_DefaultClsn2 = null;
    private PlayerStateMgr m_StateMgr = null;

    public DefaultLoaderPlayer LoaderPlayer
    {
        get
        {
            return m_LoaderPlayer;
        }
    }

    public bool ChangeState(PlayerState state)
    {
        if (m_StateMgr == null)
            return false;
        return m_StateMgr.ChangeState(state);
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
        PlayerControls.GetInstance().SwitchPlayer(loaderPlayer.PlayerType, this);
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

	public bool CanInputKey()
	{
		return true;
	}

    public bool HasStateImage(PlayerState state, bool isCheckTex = false)
    {
        var ani = this.ImageAni;
        if (ani == null)
            return false;
        bool ret = ani.HasStateImage(state, isCheckTex);
        return ret;
    }

	public void StopAni()
	{
		var ani = this.ImageAni;
		if (ani == null)
			return;
		ani.Stop ();
	}

	public void ResetFirstFrame()
	{
		var ani = this.ImageAni;
		if (ani == null)
			return;
		ani.ResetFirstFrame ();
	}

    public bool PlayAni(PlayerState state, bool isLoop = true)
    {
        var playerName = this.PlayerName;
        if (string.IsNullOrEmpty(playerName))
            return false;
        var ani = this.ImageAni;
        if (ani == null)
            return false;
        bool ret = ani.PlayerPlayerAni(state, isLoop);
        if (ret)
        {
            RefreshCurFrame(this.ImageAni);
        } else
        {
            ani.ResetState();
			UpdateRenderer(null, ActionFlip.afNone, this.ImageAni);
        }
        return ret;
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
		m_DefaultClsn2 = null;
		DestroyAllClsn ();
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

	void UpdateRenderer(ImageFrame frame, ActionFlip flip, ImageAnimation imageAni)
	{
		SpriteRenderer r = this.SpriteRender;
		if (r == null)
			return;
        if (frame == null)
        {
            r.sprite = null;
            Material m1 = r.sharedMaterial;
            if (m1 != null)
            {
                m1.SetTexture("_PalletTex", null);
                m1.SetTexture("_MainTex", null);
            }
            return;
        }

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
            var palletTex = frame.LocalPalletTex;
            if (palletTex == null)
            {
                // 取公共的Pallet
                if (string.IsNullOrEmpty(m_PalletName))
                {
                    this.PalletName = GetFirstVaildPalletName();
                }
                palletTex = GetPalletTexture();
            }
            mat.SetTexture("_PalletTex", palletTex);
			mat.SetTexture ("_MainTex", frame.Data.texture);
		}
			
		if (imageAni != null) {
			var aniNode = imageAni.CurAniNode;
			if (aniNode.defaultClsn2Arr != null)
				m_DefaultClsn2 = aniNode.defaultClsn2Arr;
		}

		DestroyAllClsn ();
		if (m_ShowClsnDebug) {
			if (imageAni != null) {
				var aniNode = imageAni.CurAniNode;
				if (aniNode.localClsn2Arr != null)
					CreateClsn (aniNode.localClsn2Arr, true);
				else
					CreateClsn (m_DefaultClsn2, true);

				CreateClsn (aniNode.localCls1Arr, false);
			}
		}
	}

	private void CreateClsn(Rect[] r, bool isCls2)
	{
		if (r == null || r.Length <= 0)
			return;
		string name;
		if (isCls2)
			name = "clsn2";
		else
			name = "clsn1";
		var mgr = GlobalConfigMgr.GetInstance ();
		for (int i = 0; i < r.Length; ++i) {
			Rect s = r [i];
			if (m_ClsnSpriteRoot == null) {
				GameObject obj = new GameObject (name);
				m_ClsnSpriteRoot = obj.transform;
				m_ClsnSpriteRoot.SetParent (this.CachedTransform, false);
				m_ClsnSpriteRoot.localPosition = Vector3.zero;
				m_ClsnSpriteRoot.localRotation = Quaternion.identity;
				m_ClsnSpriteRoot.localScale = Vector3.one;
			}
			mgr.CreateClsnSprite (name, m_ClsnSpriteRoot, s.min.x, s.min.y, s.width, s.height, isCls2);
		}
	}

	public void DestroyAllClsn()
	{
		if (m_ClsnSpriteRoot != null && m_ClsnSpriteRoot.childCount > 0) {
			for (int i = m_ClsnSpriteRoot.childCount - 1; i >= 0; --i) {
				var trans = m_ClsnSpriteRoot.GetChild (i);
				if (trans == null)
					continue;
				var sp = trans.GetComponent<SpriteRenderer> ();
				if (sp == null)
					continue;
				GlobalConfigMgr.GetInstance ().DestroyClsn (sp);
			}
		}
	}

	void OnDestroy()
	{
		if (!AppConfig.IsAppQuit) {
			m_DefaultClsn2 = null;
			DestroyAllClsn ();
		}
	}

    private void RefreshCurPallet()
    {
        SpriteRenderer r = this.SpriteRender;
        if (r == null)
            return;
        Material m1 = r.sharedMaterial;
        if (m1 == null)
            return;
        var ani = this.ImageAni;
        if (ani == null)
            return;
        ActionFlip flip;
        var frame = ani.GetCurImageFrame(out flip);
        if (frame == null)
            return;
        var palletTex = frame.LocalPalletTex;
        if (palletTex == null)
        {
            // 取公共的Pallet
            if (string.IsNullOrEmpty(m_PalletName))
            {
                this.PalletName = GetFirstVaildPalletName();
            }
            palletTex = GetPalletTexture();
            // 只能刷新用公用調色版的
            m1.SetTexture("_PalletTex", palletTex);
        }
    }

    protected Texture2D GetPalletTexture()
    {
        if (string.IsNullOrEmpty(m_PalletName) || m_LoaderPlayer == null)
        {
            return null;
        }
        var imgRes = m_LoaderPlayer.ImageRes;
        if (imgRes == null)
            return null;
        var imglib = imgRes.ImgLib;
        if (imglib == null)
            return null;
        return imglib.GetPalletTexture(this.PlayerName, m_PalletName);
    }

    // 取第一个有效的PalletName
    private string GetFirstVaildPalletName()
    {
        GlobalPlayer player = this.GPlayer;
        if (player == null || player.PlayerCfg == null || !player.PlayerCfg.IsVaild
            || player.PlayerCfg.Files == null)
            return string.Empty;
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal1))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal1);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal2))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal2);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal3))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal3);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal4))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal4);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal5))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal5);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal6))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal6);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal7))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal7);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal8))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal8);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal9))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal9);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal10))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal10);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal11))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal11);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal12))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal12);
        return string.Empty;
    }

    protected void RefreshCurFrame(ImageAnimation target)
    {
        SpriteRenderer r = this.SpriteRender;
        if (r == null)
            return;
        ActionFlip flip;
        var frame = target.GetCurImageFrame(out flip);
        if (frame == null)
            return;
		UpdateRenderer(frame, flip, target);
    }

	void OnImageAnimationFrame(ImageAnimation target)
	{
		if (target == null)
			return;
        RefreshCurFrame(target);
	}

    // 调色板名字更换
    private void OnPalletNameChanged()
    {
        RefreshCurPallet();
    }

    public string PalletName
    {
        get
        {
            return m_PalletName;
        }

        set
        {
            if (string.Compare(m_PalletName, value, true) == 0)
                return;
            m_PalletName = value;
            OnPalletNameChanged();
        }
    }

	public void ShowClsn(bool isShow)
	{
		if (m_ShowClsnDebug == isShow)
			return;
		m_ShowClsnDebug = isShow;
		OnShowClsnDebugChanged ();
	}

	private void OnShowClsnDebugChanged()
	{}

    private ImageAnimation m_ImgAni = null;
	private SpriteRenderer m_SpriteRender = null;
    private string m_PalletName = string.Empty;
	private bool m_ShowClsnDebug = false;
}
