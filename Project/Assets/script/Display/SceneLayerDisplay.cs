using UnityEngine;
using System.Collections;
using Mugen;

public enum SceneLayerType
{
	None,
	Static,
	Animation,
	Parallax
}


[RequireComponent(typeof(ImageAnimation))]
[RequireComponent(typeof(SpriteRenderer))]
public class SceneLayerDisplay : BaseResLoader {
    public int layerno = -1;

    private SpriteRenderer m_SpriteRender = null;
    private ImageAnimation m_Anim = null;
	private Material m_OrgSpMat = null;
    private int m_Group = (int)PlayerState.psNone;
    private int m_Image = -1;
    private bool m_IsPalletNull = true;
    private bool m_IsInited = false;

	public SceneLayerType m_SceneType = SceneLayerType.None;

    public SpriteRenderer SpriteRender
    {
        get
        {
            if (m_SpriteRender == null)
                m_SpriteRender = GetComponent<SpriteRenderer>();
            return m_SpriteRender;
        }
    }

    public ImageAnimation ImageAni
    {
        get
        {
            if (m_Anim == null)
                m_Anim = GetComponent<ImageAnimation>();
            return m_Anim;
        }
    }

	private void InitSpriteRender()
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

	private void CheckMaskKey(bool isNoMask)
	{
		SpriteRenderer r = this.SpriteRender;
		if (r == null)
			return;
		var m1 = r.sharedMaterial;
		if (m1 != null) {
			if (isNoMask) {
				if (m1.IsKeywordEnabled ("_RGB_A"))
					m1.DisableKeyword ("_RGB_A");
				if (!m1.IsKeywordEnabled ("_NO_RGB_A"))
					m1.EnableKeyword ("_NO_RGB_A");
			} else {
				if (!m1.IsKeywordEnabled ("_RGB_A"))
					m1.EnableKeyword ("_RGB_A");
				if (m1.IsKeywordEnabled ("_NO_RGB_A"))
					m1.DisableKeyword ("_NO_RGB_A");
			}
		}
	}

	private void UpdateImageFrame(int group, ImageFrame frame, bool isNoMask)
	{
		SpriteRenderer r = this.SpriteRender;
		if (r == null)
			return;

		if (frame == null) {
			r.sprite = null;
			var m1 = r.sharedMaterial;
			if (m1 != null) {
				m1.SetTexture ("_PalletTex", null);
				m1.SetTexture ("_MainTex", null);
                m_IsPalletNull = true;
			}
			return;
		}

		var flip = ActionFlip.afNone;
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
			var palletTex = frame.LocalPalletTex;
            if (palletTex == null)
            {
                string sceneFileName = StageMgr.GetInstance().LoadedSceneFileName;
                // 再尝试额外加载一个文件
                if (frame.LoadSceneExtLocalPalletTex(sceneFileName, group))
                {
                    palletTex = frame.LocalPalletTex;
                   // int saveGroup = (int)ImageLibrary.SceneGroupToSaveGroup(group);
                  //  StageMgr.GetInstance().SetLastPalletLink(saveGroup, frame.Image);
                    StageMgr.GetInstance().SetLastPalletLink(group, frame.Image);
                } else
                {
                    StageMgr.GetInstance().LinkImageFramePalletLastLink(frame);
                    palletTex = frame.LocalPalletTex;
                }
            }
			mat.SetTexture("_PalletTex", palletTex);
			mat.SetTexture ("_MainTex", frame.Data.texture);
            m_IsPalletNull = palletTex == null;
			CheckMaskKey (isNoMask);
		}
	}

    void RefreshPallet()
    {
        SpriteRenderer r = this.SpriteRender;
        if (r == null)
            return;
        Material mat = r.sharedMaterial;
        if (mat == null)
            return;

        var imageRes = StageMgr.GetInstance().ImageRes;
        if (imageRes != null && imageRes.LoadOk)
        {
            var frame = imageRes.GetImageFrame((PlayerState)m_Group, m_Image);
            if (frame != null)
            {
                StageMgr.GetInstance().LinkImageFramePalletLastLink(frame);
                var palletTex = frame.LocalPalletTex;
                mat.SetTexture("_PalletTex", palletTex);
                m_IsPalletNull = palletTex == null;
            }
        }
    }

    void Update()
    {
        if (m_IsInited && m_IsPalletNull)
        {
            RefreshPallet();
        }
    }

	private void InitFrameInfo(ImageFrame frame)
	{
		if (frame == null)
			return;
		if (frame.Data != null && frame.Data.texture != null && frame.Data.texture.wrapMode != TextureWrapMode.Repeat) {
			frame.Data.texture.wrapMode = TextureWrapMode.Repeat;
		}
	}

	public void InitStatic(BgStaticInfo bgInfo)
    {
		if (bgInfo == null)
			return;
		layerno = bgInfo.layerno;

        var imageRes = StageMgr.GetInstance().ImageRes;
        if (imageRes != null && imageRes.LoadOk)
        {
			var frame = imageRes.GetImageFrame ((PlayerState)bgInfo.srpiteno_Group, bgInfo.spriteno_Image);
			InitFrameInfo (frame);
			UpdateImageFrame(bgInfo.srpiteno_Group, frame, bgInfo.mask == MaskType.none);
            m_Group = bgInfo.srpiteno_Group;
            m_Image = bgInfo.spriteno_Image;
			m_SceneType = SceneLayerType.Static;
            m_IsInited = true;
        }
    }
}
