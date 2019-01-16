using UnityEngine;
using System.Collections;
using Mugen;


[RequireComponent(typeof(ImageAnimation))]
[RequireComponent(typeof(SpriteRenderer))]
public class SceneLayerDisplay : BaseResLoader {
    public int layerno = -1;

    private SpriteRenderer m_SpriteRender = null;
    private ImageAnimation m_Anim = null;
	private Material m_OrgSpMat = null;

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

	private void UpdateImageFrame(ImageFrame frame)
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
			mat.SetTexture("_PalletTex", palletTex);
			mat.SetTexture ("_MainTex", frame.Data.texture);
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
			UpdateImageFrame (frame);
        }
    }
}
