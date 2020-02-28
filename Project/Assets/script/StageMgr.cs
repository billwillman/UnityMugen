using UnityEngine;
using System.Collections;
using Mugen;

[RequireComponent(typeof(SceneImageRes), typeof(AudioSource), typeof(BaseResLoader))]
public class StageMgr : MonoSingleton<StageMgr> {

	public string DefaultSceneRoot= string.Empty;
	public string DefaultSceneName = string.Empty;
    // 摩擦因子
    public float u = 0.005f;
	private SceneConfig m_Config = null;
    private SceneImageRes m_ImageRes = null;
    private string m_LoadedSceneName = string.Empty;
    private string m_LoadedSceneFileName = string.Empty;
    private int m_LastPalletGroupLink = -1;
    private int m_LastpalletImageLink = -1;
	private AudioSource m_Audio = null;
	private BaseResLoader m_Loader = null;
    private AudioClip m_BgClip = null;

	protected BaseResLoader Loader
	{
		get
		{
			if (m_Loader == null)
				m_Loader = GetComponent<BaseResLoader> ();
			return m_Loader;
		}
	}

	public bool GetStayPos(InputPlayerType playerType, out Vector2 ret)
	{
		if (playerType == InputPlayerType.none || m_Config == null || m_Config.Players == null) {
			ret = Vector2.zero;
			return false;
		}
		float h = ((float)Screen.height)/2.0f;
		float w = ((float)Screen.width)/2.0f;
		var players = m_Config.Players;
		switch (playerType) {
		case InputPlayerType._1p:
			ret = new Vector2 (-players.p1startx, h-players.p1starty);
			return true;
		case InputPlayerType._2p:
			ret = new Vector2 (-players.p2startx, h-players.p2starty);
			return true;
		case InputPlayerType._3p:
			ret = new Vector2 (-players.p3startx, h-players.p3starty);
			return true;
		case InputPlayerType._4p:
			ret = new Vector2 (-players.p4startx, -players.p4starty);
			return true;
		}
		ret = Vector2.zero;
		return false;
	}

	public bool LoadOk = false;

    public void SetLastPalletLink(int palletGroupLink, int palletImageLink)
    {
        m_LastPalletGroupLink = palletGroupLink;
        m_LastpalletImageLink = palletImageLink;
    }

    public void LinkImageFramePalletLastLink(ImageFrame frame)
    {
        if (frame == null)
            return;
        frame._SetLocalPalletLink(m_LastPalletGroupLink, m_LastpalletImageLink);
    }

	public void LinkImageFramePalletFirstLink(ImageFrame frame)
	{
		if (frame == null)
			return;
		frame._SetLocalPalletLink (m_LastPalletGroupLink, 0);
	}

    public string LoadedSceneFileName
    {
        get
        {
            return m_LoadedSceneFileName;
        }
    }

	public void Clear()
	{
		ClearAudio ();
        DestroyScene();

		m_Config = null;
        if (m_ImageRes != null)
        {
            m_ImageRes.Clear();
        }
		LoadOk = false;
        m_LoadedSceneName = string.Empty;
        m_LoadedSceneFileName = string.Empty;
        m_LastPalletGroupLink = -1;
        m_LastpalletImageLink = -1;
	}

    public SceneImageRes ImageRes
    {
        get
        {
            if (m_ImageRes == null)
                m_ImageRes = GetComponent<SceneImageRes>();
            return m_ImageRes;
        }
    }

	public AudioSource Audio
	{
		get {
			if (m_Audio == null)
				m_Audio = GetComponent<AudioSource> ();
			return m_Audio;
		}
	}

	public bool HasBeginAction(int actionno)
	{
		return GetBeginAction (actionno) != null;
	}

	public BeginAction GetBeginAction(int actionno)
	{
		if (m_Config == null || m_Config.AirCfg == null)
			return null;
		return m_Config.AirCfg.GetBeginAction((PlayerState)actionno);
	}

	void LoadConfig(string fileName)
	{
		Clear ();
		if (string.IsNullOrEmpty (fileName))
			return;
		m_Config = new SceneConfig ();
		LoadOk = m_Config.LoadFromFile (fileName);
	}

    private void CreateSceneLayer(IBg bg)
    {
		if (bg == null || bg.bgType == BgType.none)
            return;
	//	if (bg.name != "10" && bg.name != "20")
	//		return;
        GameObject obj = new GameObject(bg.name, typeof(SceneLayerDisplay));
        var trans = obj.transform;
        trans.SetParent(this.transform, false);

		//var cam = AppConfig.GetInstance ().m_Camera;
		Vector3 pt = new Vector3 (bg.start_x, -bg.start_y, 0)/PlayerDisplay._cScenePerUnit;
		//pt = cam.ScreenToWorldPoint (pt);
		pt = pt / 100.0f;
		Vector3 p = trans.localPosition;
		p.x = pt.x;
		p.y = pt.y;
		float offz = ((float)bg.GenId) / 100.0f;
		p.z = bg.layerno > 0 ? -8.9f + offz: 9.9f - offz;
		trans.localPosition = p;
		
        trans.localScale = Vector3.one;
        trans.localRotation = Quaternion.identity;

        var dislpay = obj.GetComponent<SceneLayerDisplay>();
        dislpay.layerno = bg.layerno;
		if (bg.bgType == BgType.normal) {
			dislpay.InitStatic (bg as BgStaticInfo);
			dislpay.AdjustPos ();
		} else if (bg.bgType == BgType.anim) {
			dislpay.InitAnimated (bg as BgAniInfo);
			dislpay.AdjustPos ();
		}
    }

	private void InitCamera()
	{
		var cam = AppConfig.GetInstance ().m_Camera;
		if (cam != null) {
			if (!cam.orthographic)
				cam.orthographic = true;
			var worldPt = cam.ViewportToWorldPoint(Vector3.zero);
			var trans = cam.transform;
			var pt = trans.position;
			pt.y -= worldPt.y;
			trans.position = pt;
		}
	}

    // 創建場景
    private void CreateScene()
    {
        SceneImageRes imgRes = this.ImageRes;
        if (imgRes == null || m_Config == null || !m_Config.IsVaild)
            return;
		var bgCfg = m_Config.BgCfg;
        if (bgCfg == null)
            return;
		InitCamera ();
        for (int i = 0; i < bgCfg.BgCount; ++i)
        {
            IBg bg = bgCfg.GetBg(i);
            if (bg == null)
                continue;
            CreateSceneLayer(bg);
        }

		var cam = AppConfig.GetInstance().m_Camera;
		if (cam != null)
		{
			var pt = cam.ScreenToWorldPoint (Vector3.zero);
			pt.y = 0;
			pt.z = 0;
			this.transform.position = pt;
		}
    }

	private void ClearAudio()
	{
		var audio = this.Audio;
		if (audio != null) {
			if (audio.isPlaying)
				audio.Stop ();
			audio.clip = null;
		}
		var loader = this.Loader;
		if (loader != null) {
			loader.ClearAudioClip (ref m_BgClip);
			m_BgClip = null;
		}
	}

	private void PlayCurrSceneAudio()
	{
		ClearAudio ();
		if (m_Config == null || m_Config.Music == null || string.IsNullOrEmpty(m_LoadedSceneFileName))
			return;
		
		var audio = this.Audio;
		var loader = this.Loader;
		if (!string.IsNullOrEmpty (m_Config.Music.bgmusic)) {
			if (audio != null && loader != null) {
				string fileName = string.Format ("{0}/{1}", System.IO.Path.GetDirectoryName (m_LoadedSceneFileName), m_Config.Music.bgmusic);
				if (loader.LoadAudioClip (ref m_BgClip, fileName)) {
					if (audio.isPlaying)
						audio.Stop ();
					if (m_BgClip != null) {
						audio.clip = m_BgClip;
						audio.Play ();
					}
				} else {
					Debug.LogErrorFormat ("[Bg Audio] Scene BgAudio not found: {0}", fileName);
				}
			}
		}
	}

    // 进入当前场景
    public bool EnterCurrentScene()
    {
        SceneImageRes imgRes = this.ImageRes;
        if (imgRes == null || m_Config == null || !m_Config.IsVaild)
            return false;
        var bgCfg = m_Config.BgCfg;
        if (bgCfg == null)
            return false;
        var bgDef = m_Config.bgDef;
        if (bgDef == null || string.IsNullOrEmpty(bgDef.spr))
            return false;
        var name = bgDef.spr;
		name = GlobalConfigMgr.GetConfigFileNameNoExt(name);
        if (string.IsNullOrEmpty(name))
            return false;
        string sceneRoot;
        if (string.IsNullOrEmpty(DefaultSceneRoot))
        {
            sceneRoot = string.Format("{0}@{1}/{2}", AppConfig.GetInstance().SceneRootDir, DefaultSceneName, name);
            
        }
        else
        {
            sceneRoot = string.Format("{0}{1}/@{2}/{3}", AppConfig.GetInstance().SceneRootDir, DefaultSceneRoot, DefaultSceneName, name);
        }
        string fileName = string.Format("{0}.sff.bytes", sceneRoot);
        if (!imgRes.LoadScene(fileName, m_Config))
        {
            Debug.LogErrorFormat("[sff] {0} is not found~!!!", fileName);
            return false;
        }

        m_LoadedSceneName = this.DefaultSceneName;
        m_LoadedSceneFileName = sceneRoot;
        // 創建場景
        CreateScene();
		AppConfig.GetInstance ().StartFollow ();
		PlayCurrSceneAudio ();

        return true;
    }

    public void LoadScene(string root)
    {
        if (string.IsNullOrEmpty(root))
            return;
        string fileName = string.Format("{0}{1}.def.txt", AppConfig.GetInstance().SceneRootDir, root);
        LoadConfig(fileName);
    }

    public void LoadDefaultScene(string root, string sceneName)
    {
        DefaultSceneRoot = root;
        DefaultSceneName = sceneName;
        LoadDefaultScene();
    }

    void DestroyScene()
    {
        // 刪除所有場景子節點
        this.transform.DestroyChildren();
    }

	void LoadDefaultScene()
	{
        if (string.Compare(m_LoadedSceneName, DefaultSceneName, true) == 0)
            return;
		Clear ();
		if (string.IsNullOrEmpty (DefaultSceneName))
			return;
		string root;
		if (string.IsNullOrEmpty (DefaultSceneRoot))
			root = string.Format ("@{0}/{0}", DefaultSceneName);
		else
			root = string.Format ("{0}/@{1}/{1}", DefaultSceneRoot, DefaultSceneName);
        LoadScene(root);
        // 如果成功則進入場景
        if (LoadOk)
        {
            EnterCurrentScene();
        }
	}

	void Start()
	{
		LoadDefaultScene ();
	}
}
