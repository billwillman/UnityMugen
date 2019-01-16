using UnityEngine;
using System.Collections;
using Mugen;

[RequireComponent(typeof(SceneImageRes))]
public class StageMgr : MonoSingleton<StageMgr> {

	public string DefaultSceneRoot= string.Empty;
	public string DefaultSceneName = string.Empty;
	private SceneConfig m_Config = null;
    private SceneImageRes m_ImageRes = null;

	public bool LoadOk = false;

	public void Clear()
	{
        DestroyScene();

		m_Config = null;
        if (m_ImageRes != null)
        {
            m_ImageRes.Clear();
        }
		LoadOk = false;
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
        if (bg == null)
            return;
        GameObject obj = new GameObject(bg.name, typeof(SceneLayerDisplay));
        var trans = obj.transform;
        trans.SetParent(this.transform, false);
        trans.localPosition = Vector3.zero;
        trans.localScale = Vector3.one;
        trans.localRotation = Quaternion.identity;

        var dislpay = obj.GetComponent<SceneLayerDisplay>();
        dislpay.layerno = bg.layerno;
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
        for (int i = 0; i < bgCfg.BgCount; ++i)
        {
            IBg bg = bgCfg.GetBg(i);
            if (bg == null)
                continue;
            CreateSceneLayer(bg);
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
        name = System.IO.Path.GetFileNameWithoutExtension(name);
        if (string.IsNullOrEmpty(name))
            return false;
        string fileName;
        if (string.IsNullOrEmpty (DefaultSceneRoot))
            fileName = string.Format("{0}@{1}/{2}.sff.bytes", AppConfig.GetInstance().SceneRootDir, DefaultSceneName, name);
        else
            fileName = string.Format("{0}{1}/@{2}/{3}.sff.bytes", AppConfig.GetInstance().SceneRootDir, DefaultSceneRoot, DefaultSceneName, name);
        if (!imgRes.LoadScene(fileName, bgCfg))
            return false;

        // 創建場景
        CreateScene();

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
		Clear ();
		if (string.IsNullOrEmpty (DefaultSceneRoot) || string.IsNullOrEmpty (DefaultSceneName))
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
