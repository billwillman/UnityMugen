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
        bool ret = ani.PlayerPlayerAni(playerName, state, isLoop);
        if (ret)
        {
            RefreshCurFrame(this.ImageAni);
        } else
        {
            UpdateRenderer(null, ActionFlip.afNone);
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
            return System.IO.Path.GetFileNameWithoutExtension(player.PlayerCfg.Files.pal1);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal2))
            return System.IO.Path.GetFileNameWithoutExtension(player.PlayerCfg.Files.pal2);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal3))
            return System.IO.Path.GetFileNameWithoutExtension(player.PlayerCfg.Files.pal3);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal4))
            return System.IO.Path.GetFileNameWithoutExtension(player.PlayerCfg.Files.pal4);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal5))
            return System.IO.Path.GetFileNameWithoutExtension(player.PlayerCfg.Files.pal5);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal6))
            return System.IO.Path.GetFileNameWithoutExtension(player.PlayerCfg.Files.pal6);
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
        UpdateRenderer(frame, flip);
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

    private ImageAnimation m_ImgAni = null;
	private SpriteRenderer m_SpriteRender = null;
    private string m_PalletName = string.Empty;
}
