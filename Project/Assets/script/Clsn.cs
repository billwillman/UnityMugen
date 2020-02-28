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
    private bool m_IsVisible = false;
    private InputPlayerType m_PlayerType = InputPlayerType.none;

    public InputPlayerType PlayerType {
        get {
            return m_PlayerType;
        }
    }

    public ClsnType type {
        get {
            return m_Type;
        }
    }

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

    // 回池
    public void OnInPool() {
        m_IsVisible = false;
    }

    private void OnClsnCollision(Clsn other) {
        if (other == null || m_IsVisible)
            return;
        if ((PlayerType == other.PlayerType) || (type == other.type))
            return;
        if (type == ClsnType.attack) {

            PlayerDisplay owner = PlayerControls.GetInstance().GetPlayer(PlayerType);
            PlayerDisplay target = PlayerControls.GetInstance().GetPlayer(other.PlayerType);
            
            if (owner != target && owner != null && target != null) {
                // 能否被攻击
                if (target.OnAttacked()) {
                    owner.OnAttack(target);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null) {
            Clsn other = collision.GetComponent<Clsn>();
            OnClsnCollision(other);
        }
    }

    public ClsnType clsnType
    {
        get
        {
            return m_Type;
        }
    }

    public void Init(InputPlayerType playerType, ClsnType type, float x, float y, float w, float h)
    {
        m_Type = type;
        m_PlayerType = playerType;
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

        m_IsVisible = true;
    }
}
