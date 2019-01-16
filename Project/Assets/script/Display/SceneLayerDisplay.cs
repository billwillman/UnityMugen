using UnityEngine;
using System.Collections;


[RequireComponent(typeof(ImageAnimation))]
[RequireComponent(typeof(SpriteRenderer))]
public class SceneLayerDisplay : BaseResLoader {
    public int layerno = -1;

    private SpriteRenderer m_SpriteRender = null;
    private ImageAnimation m_Anim = null;

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

    public void Init()
    {
        var imageRes = StageMgr.GetInstance().ImageRes;
        if (imageRes != null && imageRes.LoadOk)
        {
            
        }
    }
}
