using System;
using UnityEngine;
using Mugen;

[RequireComponent(typeof(InputControl))]
public class PlayerControls: MonoSingleton<PlayerControls>
{
    public PlayerDisplay m_1P = null;
    public PlayerDisplay m_2P = null;
    public PlayerDisplay m_3P = null;
    public PlayerDisplay m_4P = null;
	public PlayerDisplay m_5P = null;
	public PlayerDisplay m_6P = null;
	public PlayerDisplay m_7P = null;
	public PlayerDisplay m_8P = null;
	public PlayerDisplay m_9P = null;
	public PlayerDisplay m_10P = null;
	public PlayerDisplay m_11P = null;
	public PlayerDisplay m_12P = null;


    private InputControl m_InputCtl = null;

    public InputControl InputCtl
    {
        get
        {
            if (m_InputCtl == null)
                m_InputCtl = GetComponent<InputControl>();
            return m_InputCtl;
        }
    }

	public PlayerDisplay GetPlayer(int playerType)
	{
		return GetPlayer ((InputPlayerType)playerType);
	}

	public PlayerDisplay GetPlayer(InputPlayerType playerType)
	{
		PlayerDisplay ret = null;
		switch (playerType)
		{
		case InputPlayerType._1p:
			ret = m_1P;
			break;
		case InputPlayerType._2p:
			ret = m_2P;
			break;
		case InputPlayerType._3p:
			ret = m_3P;
			break;
		case InputPlayerType._4p:
			ret = m_4P;
			break;
		case InputPlayerType._5p:
			ret = m_5P;
			break;
		case InputPlayerType._6p:
			ret = m_6P;
			break;
		case InputPlayerType._7p:
			ret = m_7P;
			break;
		case InputPlayerType._8p:
			ret = m_8P;
			break;
		case InputPlayerType._9p:
			ret = m_9P;
			break;
		case InputPlayerType._10p:
			ret = m_10P;
			break;
		case InputPlayerType._11p:
			ret = m_11P;
			break;
		case InputPlayerType._12p:
			ret = m_12P;
			break;
		}
		return ret;
	}

    public void SwitchPlayer(InputPlayerType playerType, PlayerDisplay display)
    {
		float x = 0f;
        switch (playerType)
        {
            case InputPlayerType._1p:
                m_1P = display;
				x  = 100.0f;
                break;
		case InputPlayerType._2p:
				m_2P = display;
				x = ((float)Screen.width) - 100.0f;
				if (m_2P != null)
					m_2P.IsFlipX = true;
                break;
			case InputPlayerType._3p:
				m_3P = display;
				x = 0f;
                break;
            case InputPlayerType._4p:
                m_4P = display;
				x = 0f;
                break;
		case InputPlayerType._5p:
			m_5P = display;
			x = 0f;
			break;
		case InputPlayerType._6p:
			m_6P = display;
			x = 0f;
			break;
		case InputPlayerType._7p:
			m_7P = display;
			x = 0f;
			break;
		case InputPlayerType._8p:
			m_8P = display;
			x = 0f;
			break;
		case InputPlayerType._9p:
			m_9P = display;
			x = 0f;
			break;
		case InputPlayerType._10p:
			m_10P = display;
			x = 0f;
			break;
		case InputPlayerType._11p:
			m_11P = display;
			x = 0f;
			break;
		case InputPlayerType._12p:
			m_12P = display;
			x = 0f;
			break;
        }

		var cam = AppConfig.GetInstance ().m_Camera;
		if (display != null && cam != null) {
			var stage = StageMgr.GetInstance ();
			Vector2 stayPos;
			if (stage.GetStayPos (playerType, out stayPos)) {
				var pt = cam.ScreenToWorldPoint (new Vector3 (stayPos.x, stayPos.y, 0));
				display.m_OffsetPos.x = pt.x;
				//display.m_OffsetPos.y = pt.y - 0.5f;
			} else {
				var pt = cam.ScreenToWorldPoint (new Vector3 (x, 0, 0)); 
				display.m_OffsetPos.x = pt.x;
			}
			display.InternalUpdatePos ();
		}

		if (display != null) {
			display.SwitchPallet ((int)playerType - 1);
		}

        if (display != null && display.AnimationState == PlayerState.psNone)
        {
            if (display.LuaPly == null)
				display.PlayAni(PlayerState.psStand1);
        }


    }
}