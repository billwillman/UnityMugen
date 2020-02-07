using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen;
using LuaInterface;

[RequireComponent(typeof(SndSound))]
[RequireComponent(typeof(ImageAnimation))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerStateMgr))]
[RequireComponent(typeof(SpriteMovement))]
[RequireComponent(typeof(PlayerAttribe))]
public class PlayerDisplay : BaseResLoader {

    private DefaultLoaderPlayer m_LoaderPlayer = null;
	private Material m_OrgSpMat = null;
	private Transform m_ClsnSpriteRoot = null;
    private Transform m_ClsnBoxRoot = null;
	private Rect[] m_DefaultClsn2 = null;
    private PlayerStateMgr m_StateMgr = null;
	private InputPlayerType m_PlayerType = InputPlayerType.none;
	private SpriteMovement m_Movement = null;
	private PlayerAttribe m_Attribe = null;
    private LuaTable m_LuaPlayer = null;
    // 是否是2P另外对着的
    private bool m_IsFlipX = false;

	private SndSound m_SndSound = null;



	[NoToLuaAttribute]
    public Vector2 m_OffsetPos = Vector2.zero;

	public float PosY
	{
		get
		{
			if (Mathf.Abs(m_OffsetPos.y) <= float.Epsilon)
				return 0;
			return m_OffsetPos.y;
		}

		set
		{
			m_OffsetPos.y = value;
		}
	}

	public float PosX
	{
		get
		{
			if (Mathf.Abs(m_OffsetPos.x) <= float.Epsilon)
				return 0;
			return m_OffsetPos.x;
		}
		set
		{
			m_OffsetPos.x = value;
		}
	}

    [NoToLuaAttribute]
    public string Call_LuaPly_GetAIName(string cmdName)
    {
        if (string.IsNullOrEmpty(cmdName) || m_LuaPlayer == null)
            return string.Empty;
        try
        {
			string ret = m_LuaPlayer.Invoke<LuaTable, string, string>("OnGetAICommandName", m_LuaPlayer, cmdName);
            return ret;
        } catch (System.Exception e)
        {
#if DEBUG
            Debug.LogError(e.ToString());
#endif
        }

        return string.Empty;
    }

	public static float _cVelPerUnit = 1f;
	public static float _cAPerUnit = 100f;

	[NoToLuaAttribute]
	public SndSound Sound
	{
		get
		{
			if (m_SndSound == null)
				m_SndSound = GetComponent<SndSound> ();
			return m_SndSound;
		}
	}

	public bool PlaySound(int group, int index)
	{
		var snd = this.Sound;
		if (snd == null)
			return false;
        if (m_LoaderPlayer == null)
            return false;
        var sndLoader = m_LoaderPlayer.SoundLoader;
        if (sndLoader == null)
            return false;
		if (!sndLoader.IsInited)
			sndLoader.Load (m_LoaderPlayer);
		try
		{
        	var clip = sndLoader.GetSoundClip(group, index);
        	return snd.PlaySound(clip);
		} catch {
			return true;
		}
	}

    public int SoundCount
    {
        get
        {
            if (m_LoaderPlayer == null)
                return 0;
            var sndLoader = m_LoaderPlayer.SoundLoader;
            if (sndLoader == null)
                return 0;
            return sndLoader.SoundCount;
        }
    }

    [NoToLua]
    // 判断角色声音是否加载了
    public bool IsSoundInited
    {
        get
        {
            if (m_LoaderPlayer == null)
                return false;
            var sndLoader = m_LoaderPlayer.SoundLoader;
            if (sndLoader == null)
                return false;
            return sndLoader.IsInited;
        }
    }

    [NoToLua]
    public bool LoadSounds()
    {
        if (m_LoaderPlayer == null)
            return false;
        return m_LoaderPlayer.LoadSounds();
    }

    [NoToLua]
    public Dictionary<KeyValuePair<int, int>, byte[]>.Enumerator GetSoundIter()
    {
        if (m_LoaderPlayer == null)
            return new Dictionary<KeyValuePair<int, int>, byte[]>.Enumerator();
        return m_LoaderPlayer.GetSoundIter();
    }

	private void AttachAttribeToSpriteMovement(bool isVaildX, bool isVaildY)
	{
		PlayerAttribe attribe = this.Attribe;
		if (attribe == null)
			return;
		SpriteMovement movement = this.Movement;
		if (movement == null)
			return;
		movement.StartVec = attribe.StateStartVec;

		if (isVaildX)
			movement.Vec.x = 0;
		if (isVaildY)
			movement.Vec.y = 0;
	}

	private void AttachAttribeFromStateDef(CNSStateDef def)
	{
		if (def == null)
			return;
		/*
		 * 设置属性
		*/
		if (this.StateMgr.CurrentCnsDef == def)
			return;
		
		PlayerAttribe attribe = this.Attribe;
		if (attribe != null) {
			if (def.Type != Cns_Type.none)
				attribe.StandType = def.Type;
			attribe.Power += def.PowerAdd;
			//if (def.Ctrl != CNSStateDef._cNoVaildCtrl)
			attribe.Ctrl = def.Ctrl;
			// 状态开始的速度
			bool isVaildX = def.Velset_x != CNSStateDef._cNoVaildVelset;
			bool isVaildY = def.Velset_y != CNSStateDef._cNoVaildVelset;
			if (isVaildX)
				attribe.StateStartVec.x = def.Velset_x;
			if (isVaildY)
				attribe.StateStartVec.y = def.Velset_y;

			AttachAttribeToSpriteMovement (isVaildX, isVaildY);
		}
	}

	public bool PlayCnsAnimateByName(string stateDefName, bool isLoop = true)
	{
		var player = this.GPlayer;
		if (player == null)
			return false;
		if (player.CnsCfg == null || !player.CnsCfg.HasStateDef)
			return false;
		int stateDefId;
		if (!player.CnsCfg.GetCNSStateId(stateDefName, out stateDefId))
			return false;
		return PlayCnsAnimate(stateDefId, isLoop);
	}

	public bool PlayCnsAnimate(int stateDefId, bool isLoop = true)
	{
		var player = this.GPlayer;
		if (player == null)
			return false;
		if (player.CnsCfg == null || !player.CnsCfg.HasStateDef)
			return false;
		var def = player.CnsCfg.GetStateDef (stateDefId);
		if (def == null)
			return false;

		AttachAttribeFromStateDef (def);

		if (StateMgr.CurrentCnsDef != def) {
			// 重置动画属性
			ImageAni.ResetCns();
			def.ResetStatesPersistent ();
		}

		bool ret;
		if ((int)def.Anim != CNSStateDef._cNoVaildAnim) {
			ret = PlayAni (def.Anim, isLoop, false);
			if (ret) {
				this.StateMgr.CurrentCnsDef = def;
			}
		} else {
			this.StateMgr.CurrentCnsDef = def;
			ret = true;
		}
		return ret;
	}

	[NoToLuaAttribute]
	public bool HasCnsFiles
	{
		get
		{
			var player = this.GPlayer;
			if (player == null)
				return false;
			if (player.CnsCfg == null)
				return false;
			return player.CnsCfg.HasStateDef;
		}
	}

	public bool IsFlipX
	{
		get {
            return m_IsFlipX;
		}

        set
        {
            if (m_IsFlipX != value)
            {
                m_IsFlipX = value;
                var trans = this.CachedTransform;
                if (m_IsFlipX)
                {
                    Quaternion quat = new Quaternion();
                    quat.eulerAngles = new Vector3(0, 180, 0);
                    trans.localRotation = quat;
                }
                else
                {
                    Quaternion quat = Quaternion.identity;
                    trans.localRotation = quat;
                }
            }
        }
	}

	public void Trigger_SuperPause(int pauseTime, int moveTime)
	{
		
	}

	protected void AniCtlPauseTime(float pauseTime)
	{
		SpriteMovement mov = this.Movement;
		if (mov != null) {
			mov.AniCtlPause (pauseTime);
		}
	}

	protected SpriteMovement Movement
	{
		get
		{
			if (m_Movement == null)
				m_Movement = GetComponent<SpriteMovement> ();
			return m_Movement;
		}
	}

	public PlayerAttribe Attribe
	{
		get
		{
			if (m_Attribe == null)
				m_Attribe = GetComponent<PlayerAttribe> ();
			return m_Attribe;
		}
	}

	[NoToLuaAttribute]
	public InputPlayerType PlyType
	{
		get {
			return m_PlayerType;
		}
	}

	[NoToLuaAttribute]
    public DefaultLoaderPlayer LoaderPlayer
    {
        get
        {
            return m_LoaderPlayer;
        }
    }

	protected PlayerStateMgr StateMgr
	{
		get {
			if (m_StateMgr == null)
				m_StateMgr = GetComponent<PlayerStateMgr> ();
			return m_StateMgr;
		}
	}

	public float GetMugenAnimateTime()
	{
		var ani = this.ImageAni;
		if (ani == null)
			return 0f;
		return ani.GetMugenAnimateTime();
	}

	public bool ResetStateAndCtrlOne(int state = (int)PlayerState.psStand1)
	{
		bool ret = ChangeState(state, false);
		if (ret)
		{
			var attr = this.Attribe;
			if (attr != null)
				attr.Ctrl = 1;
		}
		return ret;
	}

	public bool ChangeState(int state, bool isCns = false)
	{
		return ChangeState ((PlayerState)state, isCns);
	}

	public bool ChangeState(PlayerState state, bool isCns = false)
    {
		var mgr = this.StateMgr;
		if (mgr == null)
            return false;
		bool ret = mgr.ChangeState(state, isCns);
        if (!isCns)
            SetAutoCnsState();
        return ret;
    }

	[NoToLuaAttribute]
    public void SetAutoCnsState()
    {
        var mgr = this.StateMgr;
        if (mgr == null)
            return;
        mgr.SetCnsState(mgr.CanUseCnsCtl);
    }

	[NoToLuaAttribute]
    public void SetCnsState(bool isUseCns)
    {
        var mgr = this.StateMgr;
        if (mgr == null)
            return;
        mgr.SetCnsState(isUseCns);
    }

	[NoToLuaAttribute]
    public GlobalPlayer GPlayer
    {
        get
        {
            if (m_LoaderPlayer == null)
                return null;
            if (!GlobalConfigMgr.GetInstance().HasLoadPlayer(m_LoaderPlayer))
                return null;
            GlobalPlayerLoaderResult result;
            GlobalPlayer ret = GlobalConfigMgr.GetInstance().LoadPlayer(m_LoaderPlayer, out result);
            return ret;
        }
    }

	void InitSpriteRender()
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

    private void DestroyLuaPlayer()
    {
        try
        {
            if (m_LuaPlayer != null)
            {
                m_LuaPlayer.Call<LuaTable>("OnDestroy", m_LuaPlayer);
                m_LuaPlayer.Dispose();
                m_LuaPlayer = null;
            }
        } catch (System.Exception e)
        {
#if DEBUG
            Debug.LogError(e.ToString());
#endif
        }
    }

    private void CreateLuaPlayer()
    {
        DestroyLuaPlayer();
        if (m_LoaderPlayer != null)
        {
            GlobalPlayer player = m_LoaderPlayer.GetGlobalPayer();
            if (player != null && player.LuaCfg != null)
            {
                // 创建LUA对象
                m_LuaPlayer = player.LuaCfg.NewLuaPlayer(this);
            }
        }
    }

    [NoToLuaAttribute]
    public LuaTable LuaPly
    {
        get
        {
            return m_LuaPlayer;
        }
    }

	[NoToLuaAttribute]
	public void Init(DefaultLoaderPlayer loaderPlayer, InputPlayerType playerType, bool Shader_RGB_Zero_Alpha_One = true)
    {
        if (m_LoaderPlayer != null)
            Clear();
        m_LoaderPlayer = loaderPlayer;
		m_PlayerType = playerType;
        CreateLuaPlayer();

		var sp = this.SpriteRender;
		if (sp != null && sp.sharedMaterial != null) {
			var mat = sp.sharedMaterial;
			if (Shader_RGB_Zero_Alpha_One) {
				if (!mat.IsKeywordEnabled ("_RGB_A"))
					mat.EnableKeyword ("_RGB_A");
				if (mat.IsKeywordEnabled ("_NO_RGB_A"))
					mat.DisableKeyword ("_NO_RGB_A");
			} else {
				if (mat.IsKeywordEnabled ("_RGB_A"))
					mat.DisableKeyword ("_RGB_A");
				if (!mat.IsKeywordEnabled ("_NO_RGB_A"))
					mat.EnableKeyword ("_NO_RGB_A");
			}
		}

		PlayerControls.GetInstance().SwitchPlayer(playerType, this);
    }

	public int Stateno
	{
		get {
			return (int)this.AnimationState;
		}
	}

	public int PrevStateNo
	{
		get
		{
			return (int)this.PrevAnimationState;
		}
	}


	[NoToLuaAttribute]
    public PlayerState AnimationState
    {
        get
        {
            ImageAnimation ani = this.ImageAni;
            if (ani == null)
                return PlayerState.psNone;
            return ani.State;
        }
    }

	[NoToLuaAttribute]
	public PlayerState PrevAnimationState
	{
		get
		{
			ImageAnimation ani = this.ImageAni;
			if (ani == null)
				return PlayerState.psNone;
			return ani.PrevState;
		}
	}

	[NoToLuaAttribute]
	public bool CanInputKey()
	{
		/*
		var stateMgr = this.StateMgr;
		if (stateMgr == null)
			return false;
		if (stateMgr.CurState == (PlayerState)200)
			return false;
		return true;
		*/
		return true;
	}

	public bool HasAniGroup(int state)
	{
		return HasAniGroup ((PlayerState)state);
	}

	public bool HasAniGroup(PlayerState state)
	{
		return HasBeginActionSrpiteData(state, false);	
	}

	[NoToLuaAttribute]
    public bool HasBeginActionSrpiteData(PlayerState state, bool isCheckTex = false)
    {
        if (state == PlayerState.psNone)
            return false;
        var player = this.GPlayer;
        if (player == null || player.AirCfg == null || !player.AirCfg.IsVaild)
            return false;
        var ani = this.ImageAni;
        if (ani == null)
            return false;

        var beginAction = player.AirCfg.GetBeginAction(state);
        if (beginAction == null)
            return false;
        if (!isCheckTex)
            return true;
        for (int i = 0; i < beginAction.ActionFrameListCount; ++i)
        {
            ActionFrame frame;
            if (beginAction.GetFrame(i, out frame))
            {
                if (ani.HasImage(frame.Group, frame.Index))
                    return true;
            }
        }

        return false;
    }

	[NoToLuaAttribute]
	public void StopAni()
	{
		var ani = this.ImageAni;
		if (ani == null)
			return;
		ani.Stop ();
	}

	[NoToLuaAttribute]
	public void ResetFirstFrame()
	{
		var ani = this.ImageAni;
		if (ani == null)
			return;
		ani.ResetFirstFrame ();
	}

	public bool PlayAni(int state, bool isLoop = true, bool isClearTempCnsDef = true)
	{
		return PlayAni ((PlayerState)state, isLoop, isClearTempCnsDef);
	}

	public bool PlayAni(PlayerState state, bool isLoop = true, bool isClearTempCnsDef = true)
    {
		if (isClearTempCnsDef) {
			var stateMgr = this.StateMgr;
			if (stateMgr != null) {
				stateMgr.ClearCurrentCnsDef ();
			}
		}


        var playerName = this.PlayerName;
        if (string.IsNullOrEmpty(playerName))
            return false;
        var ani = this.ImageAni;
        if (ani == null)
            return false;
        bool ret = ani.PlayerPlayerAni(state, isLoop);
		m_DefaultClsn2 = null;
        if (ret)
        {

			CallCnsTriggerEvent (CnsStateTriggerType.Anim);

            RefreshCurFrame(this.ImageAni);
        } else
        {
            ani.ResetState();
			UpdateRenderer(null, ActionFlip.afNone, this.ImageAni);
        }
        return ret;
    }

	public bool IsCommandInputKeyOk(string cmdName)
	{
		if (string.IsNullOrEmpty(cmdName))
			return false;
		GlobalPlayer ply = this.GPlayer;
		if (ply == null || ply.CmdCfg == null)
			return false;
		Cmd_Command cmd = ply.CmdCfg.GetCommand(cmdName);
		if (cmd == null)
			return false;
		bool ret = PlayerControls.GetInstance ().InputCtl.CheckPlayerCmdCommandInputOk (this.PlyType, cmd);
		return ret;
	}

#if UNITY_EDITOR
	[NoToLuaAttribute]
	public bool IsCmdEditorActive(string cmdName)
	{
		if (string.IsNullOrEmpty(cmdName))
			return false;
		GlobalPlayer ply = this.GPlayer;
		if (ply == null || ply.CmdCfg == null)
			return false;
		if (ply == null || ply.CmdCfg == null)
			return false;
		var ccmd = ply.CmdCfg.GetCommand(cmdName);
		if (ccmd == null)
			return false;
		return ccmd.isEditorActive;
	}

	[NoToLuaAttribute]
	public void SetCmdEditorActive(bool isActive, string cmdName)
	{
		if (string.IsNullOrEmpty(cmdName))
			return;
		GlobalPlayer ply = this.GPlayer;
		if (ply == null || ply.CmdCfg == null)
			return;
		var ccmd = ply.CmdCfg.GetCommand(cmdName);
		if (ccmd == null)
			return;
		ccmd.isEditorActive = isActive;
	}
#endif

	public LuaCnsConfig LuaCfg
	{
		get
		{
			GlobalPlayer ply = this.GPlayer;
			if (ply == null)
				return null;
			return ply.LuaCfg;
		}
	}

	private string GetDefaultCnsName()
	{
		var attr = this.Attribe;
		if (attr.Ctrl == 0)
			return string.Empty;

		if (attr.StandType == Cns_Type.S)
			return "0";
		return string.Empty;
	}

	[NoToLuaAttribute]
	public bool RunAutoCmd()
	{
		GlobalPlayer ply = this.GPlayer;
		if (ply == null || ply.CmdCfg == null)
			return false;

		CNSConfig cnsCfg = ply.CnsCfg;
		if (cnsCfg == null)
		{
			if (ply.LuaCfg != null)
			{
				cnsCfg = ply.LuaCfg.CnsCfg;
				if (cnsCfg == null)
					return false;
			}
			else
				return false;
		}

		bool mustCheckTrigger;
		AI_Command aiCmd = ply.CmdCfg.GetAutoCheckAICommand(this, out mustCheckTrigger);
		if (aiCmd == null)
		{
			// 判断stand以及需要Ctrl = 1
			string defCnsName = GetDefaultCnsName();
			if (!string.IsNullOrEmpty(defCnsName))
			{
				return PlayCnsAnimateByName(defCnsName, true);
			}
			return false;
		}
		string cmdName = aiCmd.name;
		if (mustCheckTrigger && !aiCmd.CanTrigger(this, cmdName))
			return false;

		
		int id;
		if (!cnsCfg.GetCNSStateId(aiCmd.value, out id))
		{
			if (!int.TryParse(aiCmd.value, out id))
				return false;
			return this.PlayAni((PlayerState)id, aiCmd.AniLoop);
		}

		return this.PlayCnsAnimate(id, aiCmd.AniLoop);
		//return ChangeState((PlayerState)id, true);
	}

	/*
	public bool RunCmd(string cmdName)
    {
        if (string.IsNullOrEmpty(cmdName))
            return false;
        GlobalPlayer ply = this.GPlayer;
        if (ply == null || ply.CmdCfg == null)
            return false;
        Cmd_Command cmd = ply.CmdCfg.GetCommand(cmdName);
        if (cmd == null)
            return false;

		bool mustCheckTrigger;
		AI_Command aiCmd = ply.CmdCfg.GetAICommand (cmd, this, out mustCheckTrigger);
		if (aiCmd == null || (mustCheckTrigger && !aiCmd.CanTrigger(this, cmdName)))
			return false;

        CNSConfig cnsCfg = ply.CnsCfg;
        if (cnsCfg == null)
        {
            if (ply.LuaCfg != null)
            {
                cnsCfg = ply.LuaCfg.CnsCfg;
                if (cnsCfg == null)
                    return false;
            } else
                return false;
        }
        int id;
		if (!cnsCfg.GetCNSStateId(aiCmd.value, out id))
		{
			if (!int.TryParse(aiCmd.value, out id))
				return false;
			return this.PlayAni((PlayerState)id, aiCmd.AniLoop);
		}
		return this.PlayCnsAnimate(id, aiCmd.AniLoop);
	}
	*/

	[NoToLuaAttribute]
    public string PlayerName
    {
        get
        {
            if (m_LoaderPlayer == null)
                return string.Empty;
            return m_LoaderPlayer.GetPlayerName();
        }
    }

	[NoToLuaAttribute]
    public void Clear(bool isResetLoaderPlayer = true)
    {
		m_DefaultClsn2 = null;
		DestroyAllClsn ();
        DestroyLuaPlayer();
        if (m_ImgAni != null)
        {
            m_ImgAni.Clear();
        }
			
        if (isResetLoaderPlayer)
            m_LoaderPlayer = null;
    }

	public int ImageCurrentFrame
	{
		get {
			ImageAnimation ani = this.ImageAni;
			if (ani == null)
				return -1;
			return ani.CurFrame;
		}
	}

	public int AnimElemTime(int frameNo)
	{
		if (frameNo < 0)
			return -1;
		ImageAnimation ani = this.ImageAni;
		if (ani == null)
			return -1;
		if (ani.CurFrame != frameNo)
			return -1;
		return ani.AnimElemTime;
	}

	[NoToLuaAttribute]
    public ImageAnimation ImageAni
    {
        get
        {
            if (m_ImgAni == null)
                m_ImgAni = GetComponent<ImageAnimation>();
            return m_ImgAni;
        }
    }

	[NoToLuaAttribute]
	public SpriteRenderer SpriteRender
	{
		get {
			if (m_SpriteRender == null)
				m_SpriteRender = GetComponent<SpriteRenderer> ();
			return m_SpriteRender;
		}
	}

	// 创建爆炸效果
	public bool Trigger_CreateExplode()
	{
		return true;
	}

	public int Trigger_Time()
	{
		/*
		var imgAni = this.ImageAni;
		if (imgAni == null)
			return -1;
		return ImageAni.CurFrame;
		*/
		var imgAni = this.ImageAni;
		if (imgAni == null)
			return -1;
		return imgAni.CurAniUsedTime;
	}

	public int Trigger_AnimTime()
	{
		/*
		var imgAni = this.ImageAni;
		if (imgAni == null)
			return -9999;
		int ret = (imgAni.CurFrame + 1) - imgAni.AniNodeCount;
		return ret;
		*/
		var imgAni = this.ImageAni;
		if (imgAni == null)
			return -9999;
		return imgAni.CurAniRetainTime;
	}

	public void SetVelSetX(float x)
	{
		var movement = this.Movement;
		if (movement == null)
			return;
		movement.Vec.x = x;
	}

	public void SetVelSetY(float y)
	{
		var movement = this.Movement;
		if (movement == null)
			return;
		movement.Vec.x = y;
	}

	public void SetVelSet(float x, float y)
	{
		var movement = this.Movement;
		if (movement == null)
			return;
		movement.Vec = new Vector2 (x, y);
	}

	public void PosAdd(float x, float y)
	{
		float dir;
		if (IsFlipX)
			dir = -1;
		else
			dir = 1;
		m_OffsetPos += new Vector2(x * dir, y);
	}

	public void VelAdd(float x, float y)
	{
		var movement = this.Movement;
		if (movement == null)
			return;
		Vector2 v = movement.Vec;
		v += new Vector2(x, y);
		movement.Vec = v;
	}

	public float VelX {
		get {
			var movement = this.Movement;
			if (movement == null)
				return 0;
			return movement.Vec.x;
		}
	}

	public float VelY {
		get {
			var movement = this.Movement;
			if (movement == null)
				return 0;
			return movement.Vec.y;
		}
	}

	public void VelMul(float x, float y)
	{
		var movement = this.Movement;
		if (movement == null)
			return;
		Vector2 v = movement.Vec;
		Vector2 mulVec = new Vector2 (x, y);
		v.x *= mulVec.x;
		v.y *= mulVec.y;
		movement.Vec = v;
	}

    private void UpdateClsnRootOffsetPos(Vector2 localPos)
    {
        if (m_ClsnSpriteRoot != null)
            m_ClsnSpriteRoot.localPosition = -localPos;
        if (m_ClsnBoxRoot != null)
            m_ClsnBoxRoot.localPosition = -localPos;
    }

	void UpdateRenderer(ImageFrame frame, ActionFlip flip, ImageAnimation imageAni)
	{
		SpriteRenderer r = this.SpriteRender;
		if (r == null)
			return;
        if (frame == null)
        {
            r.sprite = null;
            Material m1 = r.sharedMaterial;
            if (m1 != null)
            {
                m1.SetTexture("_PalletTex", null);
                m1.SetTexture("_MainTex", null);
            }
            return;
        }

		r.sprite = frame.Data;
		if (r.sprite != null)
		{
            Transform trans = r.transform;
            /*
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
            */

            switch (flip)
            {
                case ActionFlip.afH:
                    r.flipX = true;
                    break;
                case ActionFlip.afV:
                    r.flipY = true;
                    break;
                case ActionFlip.afHV:
                    r.flipX = true;
                    r.flipY = true;
                    break;
                default:
                    r.flipX = false;
                    r.flipY = false;
                    break;
            }

            Vector2 frameOffset = frame.OffsetPos;
            if (IsFlipX)
                frameOffset.x = -frameOffset.x;
            trans.localPosition = frameOffset + m_OffsetPos;
            UpdateClsnRootOffsetPos(frame.OffsetPos);
		}

		Material mat = r.sharedMaterial;
		if (mat != null) {
			//mat.SetTexture ("_PalletTex", frame.PalletTexture);
            var palletTex = frame.LocalPalletTex;
            if (palletTex == null)
            {
                // 取公共的Pallet
                if (string.IsNullOrEmpty(m_PalletName))
                {
                    this.PalletName = GetFirstVaildPalletName();
                }
                palletTex = GetPalletTexture();
            }
            mat.SetTexture("_PalletTex", palletTex);
			mat.SetTexture ("_MainTex", frame.Data.texture);
		}
			
		if (imageAni != null) {
			var aniNode = imageAni.CurAniNode;
			if (aniNode.defaultClsn2Arr != null)
				m_DefaultClsn2 = aniNode.defaultClsn2Arr;
		}

		DestroyAllClsn ();
        if (imageAni != null)
        {
            var aniNode = imageAni.CurAniNode;
            if (aniNode.localClsn2Arr != null)
                CreateClsn(aniNode.localClsn2Arr, true, m_ShowClsnDebug);
            else
                CreateClsn(m_DefaultClsn2, true, m_ShowClsnDebug);

            CreateClsn(aniNode.localCls1Arr, false, m_ShowClsnDebug);
        }
	}

	private void CreateClsn(Rect[] r, bool isCls2, bool showSprite)
	{
		if (r == null || r.Length <= 0)
			return;
		string name;
		if (isCls2)
			name = "clsn2";
		else
			name = "clsn1";
		var mgr = GlobalConfigMgr.GetInstance ();
		for (int i = 0; i < r.Length; ++i) {
			Rect s = r [i];
            if (showSprite)
            {
                if (m_ClsnSpriteRoot == null)
                {
                    GameObject obj = new GameObject("clsn");
                    m_ClsnSpriteRoot = obj.transform;
                    m_ClsnSpriteRoot.SetParent(this.CachedTransform, false);
                    m_ClsnSpriteRoot.localPosition = Vector3.zero;
                    m_ClsnSpriteRoot.localRotation = Quaternion.identity;
                    m_ClsnSpriteRoot.localScale = Vector3.one;
                }
                mgr.CreateClsnSprite(name, m_ClsnSpriteRoot, s.min.x, s.min.y, s.width, s.height, isCls2);
            }

            if (m_LoaderPlayer != null)
            {
                if (m_ClsnBoxRoot == null)
                {
                    GameObject obj = new GameObject("box");
                    m_ClsnBoxRoot = obj.transform;
                    m_ClsnBoxRoot.SetParent(this.CachedTransform, false);
                    m_ClsnBoxRoot.localPosition = Vector3.zero;
                    m_ClsnBoxRoot.localRotation = Quaternion.identity;
                    m_ClsnBoxRoot.localScale = Vector3.one;
                }
                mgr.CreateClsnBox(m_LoaderPlayer.PlayerType, name, m_ClsnBoxRoot, s.min.x, s.min.y, s.width, s.height, isCls2);
            }
		}
	}

	[NoToLuaAttribute]
	public void DestroyAllClsn()
	{
		if (m_ClsnSpriteRoot != null && m_ClsnSpriteRoot.childCount > 0) {
			for (int i = m_ClsnSpriteRoot.childCount - 1; i >= 0; --i) {
				var trans = m_ClsnSpriteRoot.GetChild (i);
				if (trans == null)
					continue;
				var sp = trans.GetComponent<SpriteRenderer> ();
				if (sp == null)
					continue;
				GlobalConfigMgr.GetInstance ().DestroyClsn (sp);
			}
		}

        if (m_ClsnBoxRoot != null && m_ClsnBoxRoot.childCount > 0)
        {
            for (int i = m_ClsnBoxRoot.childCount - 1; i >= 0; --i)
            {
                var trans = m_ClsnBoxRoot.GetChild(i);
                if (trans == null)
                    continue;
                var box = trans.GetComponent<Clsn>();
                if (box == null)
                    continue;
                GlobalConfigMgr.GetInstance().DestroyBoxCollider(box);
            }
        }
	}

	void OnDestroy()
	{
		if (!AppConfig.IsAppQuit) {
			m_DefaultClsn2 = null;
			DestroyAllClsn ();
            DestroyLuaPlayer();
		}
	}

    private void RefreshCurPallet()
    {
        SpriteRenderer r = this.SpriteRender;
        if (r == null)
            return;
        Material m1 = r.sharedMaterial;
        if (m1 == null)
            return;
        var ani = this.ImageAni;
        if (ani == null)
            return;
        ActionFlip flip;
        var frame = ani.GetCurImageFrame(out flip);
        if (frame == null)
            return;
        var palletTex = frame.LocalPalletTex;
        if (palletTex == null)
        {
            // 取公共的Pallet
            if (string.IsNullOrEmpty(m_PalletName))
            {
                this.PalletName = GetFirstVaildPalletName();
            }
            palletTex = GetPalletTexture();
            // 只能刷新用公用調色版的
            m1.SetTexture("_PalletTex", palletTex);
        }
    }

    protected Texture2D GetPalletTexture()
    {
        if (string.IsNullOrEmpty(m_PalletName) || m_LoaderPlayer == null)
        {
            return null;
        }
        var imgRes = m_LoaderPlayer.ImageRes;
        if (imgRes == null)
            return null;
        var imglib = imgRes.ImgLib;
        if (imglib == null)
            return null;
        return imglib.GetPalletTexture(this.PlayerName, m_PalletName);
    }

    // 取第一个有效的PalletName
    private string GetFirstVaildPalletName()
    {
        GlobalPlayer player = this.GPlayer;
        if (player == null || player.PlayerCfg == null || !player.PlayerCfg.IsVaild
            || player.PlayerCfg.Files == null)
            return string.Empty;
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal1))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal1);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal2))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal2);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal3))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal3);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal4))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal4);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal5))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal5);
        if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal6))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal6);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal7))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal7);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal8))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal8);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal9))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal9);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal10))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal10);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal11))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal11);
		if (!string.IsNullOrEmpty(player.PlayerCfg.Files.pal12))
			return GlobalConfigMgr.GetConfigFileNameNoExt(player.PlayerCfg.Files.pal12);
        return string.Empty;
    }

	private PlayerPartMgr m_PartMgr = null;
	private bool m_IsInitedPartMgr = false;

	void InitPartMgr()
	{
		if (m_IsInitedPartMgr)
			return;
		m_IsInitedPartMgr = true;
		m_PartMgr = GetComponent<PlayerPartMgr> ();
	}

	protected void SendPartMgrFrame(ImageAnimation target)
	{
		InitPartMgr ();
		if (m_PartMgr == null)
			return;
		m_PartMgr.OnUpdateFrame (target);
	}

	

    protected void RefreshCurFrame(ImageAnimation target)
    {
        SpriteRenderer r = this.SpriteRender;
        if (r == null)
            return;
        ActionFlip flip;
        var frame = target.GetCurImageFrame(out flip);
        if (frame == null)
            return;
		UpdateRenderer(frame, flip, target);

		CallCnsTriggerEvent (CnsStateTriggerType.AnimElem);

		SendPartMgrFrame (target);
    }

	[NoToLua]
	public void CallCnsTriggerEvent(params CnsStateTriggerType[] triggerTypes)
	{
		if (triggerTypes == null || triggerTypes.Length <= 0)
			return;
		
		var stateMgr = this.StateMgr;
		if (stateMgr != null) {
			var def = stateMgr.CurrentCnsDef;
			if (def != null) {
				for (int i = 0; i < triggerTypes.Length; ++i) {
					var triggerType = triggerTypes [i];
					def.OnStateEvent (this, triggerType);
				}
			}
		}
	}

	void OnImageAniTimeUpdate(ImageAnimation target)
	{
		CallCnsTriggerEvent(CnsStateTriggerType.AnimTime);
	}

	void OnImageAnimationFrame(ImageAnimation target)
	{
		if (target == null)
			return;
        RefreshCurFrame(target);
	}

	void OnImageAnimationEndFrame(ImageAnimation target)
	{
		var mgr = this.StateMgr;
		if (mgr != null) {
			mgr.CurStateOnAnimateEndFrame ();
		}
	}

    // 调色板名字更换
    private void OnPalletNameChanged()
    {
        RefreshCurPallet();
    }

	[NoToLuaAttribute]
    public string PalletName
    {
        get
        {
            return m_PalletName;
        }

        set
        {
            if (string.Compare(m_PalletName, value, true) == 0)
                return;
            m_PalletName = value;
            OnPalletNameChanged();
        }
    }

	[NoToLuaAttribute]
	public void ShowClsn(bool isShow)
	{
		if (m_ShowClsnDebug == isShow)
			return;
		m_ShowClsnDebug = isShow;
		OnShowClsnDebugChanged ();
	}

	private void OnShowClsnDebugChanged()
	{}

    // 创建飞行道具

    private ImageAnimation m_ImgAni = null;
	private SpriteRenderer m_SpriteRender = null;
    private string m_PalletName = string.Empty;
	private bool m_ShowClsnDebug = false;
}
