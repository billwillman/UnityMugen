using UnityEngine;
using System.Collections;

public enum ClsnType
{
    none = 0,
    def,
    attack
}


[RequireComponent(typeof(BoxCollider2D))]
public class Clsn : MonoBehaviour {
    private BoxCollider2D m_Box;
    private ClsnType m_Type = ClsnType.none;
    private float m_CreateTimer = 0f;

    public float CreateTimer
    { 
        get
        {
            return m_CreateTimer;
        }
    }

    public BoxCollider2D Box
    {
        get
        {
            if (m_Box == null)
            {
                m_Box = GetComponent<BoxCollider2D>();
                m_Box.isTrigger = true;
            }
            return m_Box;
        }
    }

    public ClsnType clsnType
    {
        get
        {
            return m_Type;
        }
    }

    public void Init(ClsnType type, float x, float y, float w, float h)
    {
        m_Type = type;
        m_CreateTimer = Time.realtimeSinceStartup;
        BoxCollider2D box = this.Box;
        float rw = w / 100f;
        if (rw < 0)
        {
            rw = -rw;
        }

        float rh = h / 100f;
        if (rh < 0)
        {
            rh = -rh;
        }


        box.offset = Vector2.zero;
        box.size = new Vector2(rw, rh);

        var trans = this.transform;
        Vector2 pos = new Vector2((x + w / 2f) / 100f, -(y + h / 2f) / 100f);
        trans.localPosition = pos;
        trans.localRotation = Quaternion.identity;
    }
}
