using UnityEngine;
using System.Collections;
using Mugen;

public class DefaultLoader : MonoBehaviour, IMugenLoader {

	void Awake()
	{
		ResourceMgr.Instance.LoadConfigs (null);
		AppConfig.GetInstance ().Loader = this;
	}

	public string LoadText(string fileName)
	{
		return ResourceMgr.Instance.LoadText (fileName);
	}

	public byte[] LoadBytes (string fileName)
	{
		return ResourceMgr.Instance.LoadBytes (fileName);
	}

	public void DestroyObject(UnityEngine.Object obj)
	{
		ResourceMgr.Instance.DestroyObject (obj);
	}
}
