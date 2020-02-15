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
	public MaskType m_MaskType = MaskType.alpha;
	public int m_Group = (int)PlayerState.psNone;
	private TransType m_TransType = TransType.none;
	public TransType m_CurTransType = TransType.none;

    private SpriteRenderer m_SpriteRender = null;
    private ImageAnimation m_Anim = null;
	private Material m_OrgSpMat = null;
    private int m_Image = -1;
    private bool m_IsPalletNull = true;
    private bool m_IsInited = false;
	#if UNITY_EDITOR
	public Vector2 m_CfgSize = Vector2.zero;
	public bool m_CfgFirstFrame = false;

	private void UpdateCfgSize()
	{
		if (!m_IsInited)
			return;
		
		var pt = this.CachedTransform.localPosition;
		var cam = AppConfig.GetInstance ().m_Camera;
		var sp = this.SpriteRender;
		if (sp != null && sp.sprite != null) {
			pt -= new Vector3 (sp.sprite.bounds.size.x/2.0f, sp.sprite.bounds.size.y/2.0f, 0);
		}
		//pt = cam.WorldToScreenPoint (pt);
		pt = pt * 100.0f;
		m_CfgSize = new Vector2 (pt.x, -pt.y) * PlayerDisplay._cScenePerUnit;
	}
	#endif

	public SceneLayerType m_SceneType = SceneLayerType.None;

	internal void AdjustPos()
	{
		//return;
		var sp = this.SpriteRender;
		if (sp != null && sp.sprite != null) {
			var trans = this.CachedTransform;
			/*
			var cam = AppConfig.GetInstance ().m_Camera;
			Vector3 offset = new Vector3 (sp.sprite.pivot.x, sp.sprite.pivot.y, 0);
			var orgPt = trans.position;
			orgPt = cam.WorldToScreenPoint (orgPt);
			var dst = orgPt + offset;
			trans.position = cam.ScreenToWorldPoint(dst);
			*/
			var orgPt = trans.localPosition;
			var dst = orgPt + new Vector3 (sp.sprite.bounds.size.x/2.0f, sp.sprite.bounds.size.y/2.0f, 0);
			trans.localPosition = dst;
		}
	}

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

	private void InitSpriteRender(TransType transType)
	{
		var sp = this.SpriteRender;
		if (sp != null) {
			if (m_OrgSpMat == null || m_CurTransType != transType) {
				m_CurTransType = transType;
				if (transType == TransType.Add) {
					LoadMaterial (ref m_OrgSpMat, AppConfig.GetInstance ().PalleetAddMatFileName);
				} else
					LoadMaterial (ref m_OrgSpMat, AppConfig.GetInstance ().PalleetMatFileName);
				if (m_OrgSpMat != null) {
					Material mat = GameObject.Instantiate (m_OrgSpMat);
					AddOrSetInstanceMaterialMap (sp.GetInstanceID (), mat);
					sp.sharedMaterial = mat;
				}
			}
		}
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

	private TransType GetTransType(ActionDrawMode drawMode)
	{
		if (drawMode == ActionDrawMode.adNone)
			return m_TransType;
		return (TransType)drawMode;
	}

	private void UpdateImageFrame(ImageFrame frame, ActionFlip flip, ActionDrawMode drawMode, bool isNoMask)
	{
		InitFrameInfo (frame);
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
			
		InitSpriteRender (GetTransType(drawMode));
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
				if (frame.LoadSceneExtLocalPalletTex(sceneFileName, m_Group))
                {
                    palletTex = frame.LocalPalletTex;
                   // int saveGroup = (int)ImageLibrary.SceneGroupToSaveGroup(group);
                  //  StageMgr.GetInstance().SetLastPalletLink(saveGroup, frame.Image);
					StageMgr.GetInstance().SetLastPalletLink(m_Group, frame.Image);
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
		#if UNITY_EDITOR
		if (m_SceneType == SceneLayerType.Animation)
		{
			if (m_CfgFirstFrame)
			{
				var ani = this.ImageAni;
				if (ani != null)
				{
					ani.ResetFirstFrame(false);

				}
			} else
			{
				var ani = this.ImageAni;
				if (ani != null && !ani.IsPlaying)
					ani.PlayerPlayerAni((PlayerState)m_Group, true);
			}
		}
		UpdateCfgSize();
		#endif
    }

	private void InitFrameInfo(ImageFrame frame)
	{
		if (frame == null)
			return;
		if (frame.Data != null && frame.Data.texture != null && frame.Data.texture.wrapMode != TextureWrapMode.Repeat) {
			frame.Data.texture.wrapMode = TextureWrapMode.Repeat;
		}
	}

	protected void RefreshCurFrame(ImageAnimation target)
	{
		if (target == null)
			return;
		SpriteRenderer r = this.SpriteRender;
		if (r == null)
			return;
		ActionFlip flip;
		ActionDrawMode drawMode;
		var frame = target.GetCurImageFrame(out flip, out drawMode);
		if (frame == null)
			return;
		UpdateImageFrame(frame, flip, drawMode, m_MaskType == MaskType.none);
	}

	void OnImageAnimationFrame(ImageAnimation target)
	{
		if (target == null)
			return;
		RefreshCurFrame(target);
	}

	public void InitAnimated(BgAniInfo anInfo)
	{
		if (anInfo == null)
			return;
		layerno = anInfo.layerno;
		var imageRes = StageMgr.GetInstance().ImageRes;
		if (imageRes != null && imageRes.LoadOk) {
			// 处理动画层
			m_Group = anInfo.actionno;
			m_MaskType = anInfo.mask;

			m_SceneType = SceneLayerType.Animation;
			InitSpriteRender (anInfo.transType);
			m_IsInited = true;

			if (StageMgr.GetInstance ().HasBeginAction (m_Group)) {
				// 初始化动画
				var ani = this.ImageAni;
				if (ani != null) {
					ani.Type = ImageAnimation.ImageAnimationType.Scene;
					if (ani.PlayerPlayerAni ((PlayerState)m_Group, true))
						RefreshCurFrame (ani);
				}
			}
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
			m_Group = bgInfo.srpiteno_Group;
			m_Image = bgInfo.spriteno_Image;
			m_MaskType = bgInfo.mask;
			m_SceneType = SceneLayerType.Static;
			m_TransType = bgInfo.transType;
			InitSpriteRender (bgInfo.transType);

			var frame = imageRes.GetImageFrame ((PlayerState)bgInfo.srpiteno_Group, bgInfo.spriteno_Image);
			UpdateImageFrame(frame, ActionFlip.afNone, ActionDrawMode.adNone, bgInfo.mask == MaskType.none);
            

            m_IsInited = true;
        }
    }
}
