using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;

public class SceneImageRes : MonoBehaviour {
    private ImageLibrary m_ImgLib = null;

    public bool Is32BitPallet = true;

    void OnApplicationQuit()
    {
        Clear();
    }

    public bool LoadOk = false;

	public bool LoadScene(string fileName, SceneConfig config)
    {
        Clear();
		if (string.IsNullOrEmpty(fileName) || config == null || config.BgCfg == null)
            return false;

		m_ImgLib = new ImageLibrary(Is32BitPallet, true);
		if (!m_ImgLib.LoadScene(fileName, config))
            return false;
        LoadOk = true;
        return true;
    }

	public ImageFrame GetImageFrame(PlayerState group, int image)
	{
		if (m_ImgLib == null)
			return null;
		//PlayerState saveGroup = ImageLibrary.SceneGroupToSaveGroup (group);
		//return m_ImgLib.GetImageFrame (saveGroup, image);
        return m_ImgLib.GetImageFrame(group, image);
	}

	public List<ImageAnimateNode> GetAnimationNodeList(PlayerState group)
	{
		if (m_ImgLib == null)
			return null;
		return m_ImgLib.GetAnimationNodeList (group);
	}

    void OnDestroy()
    {
        if (!AppConfig.IsAppQuit)
            Clear();
    }

    public void Clear()
    {
        LoadOk = false;
        if (m_ImgLib != null)
        {
            m_ImgLib.Dispose();
            m_ImgLib = null;
        }
    }
}
