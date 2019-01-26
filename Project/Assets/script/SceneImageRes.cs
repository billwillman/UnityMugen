using UnityEngine;
using System.Collections;
using Mugen;

public class SceneImageRes : MonoBehaviour {
    private ImageLibrary m_ImgLib = null;

    public bool Is32BitPallet = true;

    void OnApplicationQuit()
    {
        Clear();
    }

    public bool LoadOk = false;

    public bool LoadScene(string fileName, BgConfig config)
    {
        Clear();
        if (string.IsNullOrEmpty(fileName) || config == null)
            return false;

        m_ImgLib = new ImageLibrary(Is32BitPallet);
        if (!m_ImgLib.LoadScene(fileName, config))
            return false;
        LoadOk = true;
        return true;
    }

	public ImageFrame GetImageFrame(PlayerState group, int image)
	{
		if (m_ImgLib == null)
			return null;
		PlayerState saveGroup = ImageLibrary.SceneGroupToSaveGroup (group);
		return m_ImgLib.GetImageFrame (saveGroup, (int)group, image);
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
