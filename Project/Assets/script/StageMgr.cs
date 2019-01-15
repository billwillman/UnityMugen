using UnityEngine;
using System.Collections;
using Mugen;

public class StageMgr : MonoSingleton<StageMgr> {

	public string DefaultSceneRoot= string.Empty;
	public string DefaultSceneName = string.Empty;
	private SceneConfig m_Config = null;

	public bool LoadOk = false;

	void Clear()
	{
		m_Config = null;
		LoadOk = false;
	}

	void LoadConfig(string fileName)
	{
		Clear ();
		if (string.IsNullOrEmpty (fileName))
			return;
		m_Config = new SceneConfig ();
		LoadOk = m_Config.LoadFromFile (fileName);
	}

    public void LoadScene(string root)
    {
        if (string.IsNullOrEmpty(root))
            return;
        string fileName = string.Format("{0}{1}.def.txt", AppConfig.GetInstance().SceneRootDir, root);
        LoadConfig(fileName);
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
	}

	void Start()
	{
		LoadDefaultScene ();
	}
}
