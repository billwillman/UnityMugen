using UnityEngine;
using System;
using System.Collections;
using Mugen;
using NsLib.ResMgr;
using Utils;
using LuaInterface;

public class AppConfig : MonoSingleton<AppConfig> {
	public int FPS = 30;
	public int ScreenSleepTime = 30;
	public int ResolutionWidth = 1280;
	public int ResolutionHeight = 720;
	public string PlayerRootDir = "resources/mugen/char/";
	public string SceneRootDir = "resources/mugen/scene/";
	public string PalleetMatFileName = "resources/mugen/@mat/palleetmaterial.mat";
	public bool IsUsePhysixUpdate = true;
	public Camera m_Camera = null;

	public IMugenLoader Loader = null;

    public static bool IsAppQuit = false;

	private LuaLoader m_LuaLoader = null;
	private LuaState m_LuaState = null;
	private LuaLooper m_LuaLoop = null;

	public float DeltaTime {
		get {
			if (IsUsePhysixUpdate)
				return Time.fixedDeltaTime;
			else
				return Time.deltaTime;
		}
	}

	public float DeltaTick
	{
		get {
			return DeltaTime / ImageAnimation._cImageAnimationScale;
		}
	}

	public LuaLoader LuaLoader
	{
		get
		{
			return m_LuaLoader;
		}
	}

	private void UnInitLua()
	{
		if (m_LuaLoop != null)
		{
			m_LuaLoop.Destroy();
			m_LuaLoop = null;
		}

		if (m_LuaState != null)
		{
			m_LuaState.Dispose();
			m_LuaState = null;
		}

		if (m_LuaLoader != null)
		{
			m_LuaLoader.Dispose();
			m_LuaLoader = null;
		}
	}

	public T DoFile<T>(string fileName) where T: class
	{
		if (string.IsNullOrEmpty(fileName))
			return null;
        T ret = default(T);
		try
		{
			if (m_LuaState != null)
				ret = m_LuaState.DoFile<T>(fileName);
			else
                return null; 
		} catch(System.Exception e)
		{
			#if DEBUG
			Debug.LogError(e.ToString());
			#endif
            return null;
		}
		return ret;
	}

	public void LuaGC()
	{
		if (m_LuaState != null)
			m_LuaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
	}

	void OnDestroy()
	{
		if (IsAppQuit)
			return;
		UnInitLua();
	}

    void OnApplicationQuit()
    {
        IsAppQuit = true;
		UnInitLua();
    }

	// 初始化环境
	private void InitLuaEnv()
	{
		m_LuaState = new LuaState();
		OpenLibs();
		m_LuaLoader = new LuaLoader();
		m_LuaState.LuaSetTop(0);
		LuaBinder.Bind(m_LuaState);
		DelegateFactory.Init();
		LuaCoroutine.Register(m_LuaState, this);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LuaOpen_Socket_Core(IntPtr L)
	{        
		return LuaDLL.luaopen_socket_core(L);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LuaOpen_Mime_Core(IntPtr L)
	{
		return LuaDLL.luaopen_mime_core(L);
	}

	protected void OpenLuaSocket()
	{
		LuaConst.openLuaSocket = true;

		m_LuaState.BeginPreLoad();
		m_LuaState.RegFunction("socket.core", LuaOpen_Socket_Core);
		m_LuaState.RegFunction("mime.core", LuaOpen_Mime_Core);                
		m_LuaState.EndPreLoad();                     
	}

	/// <summary>
	/// 注册的C库
	/// </summary>
	void OpenLibs()
	{
		if (m_LuaState != null)
		{
			m_LuaState.OpenLibs(LuaDLL.luaopen_pb);      
			m_LuaState.OpenLibs(LuaDLL.luaopen_sproto_core);
			m_LuaState.OpenLibs(LuaDLL.luaopen_protobuf_c);
			m_LuaState.OpenLibs(LuaDLL.luaopen_lpeg);
			m_LuaState.OpenLibs(LuaDLL.luaopen_bit);
			m_LuaState.OpenLibs(LuaDLL.luaopen_socket_core);
		}
		OpenCJson();

		if (LuaConst.openLuaSocket) {
			OpenLuaSocket ();
		}
	}

	//cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
	protected void OpenCJson()
	{
		if (m_LuaState != null)
		{
			m_LuaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
			m_LuaState.OpenLibs(LuaDLL.luaopen_cjson);
			m_LuaState.LuaSetField(-2, "cjson");

			m_LuaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
			m_LuaState.LuaSetField(-2, "cjson.safe");   
		}
	}

	private void InitDefaultLoader()
	{
		new GameObject("DefaultLoader", typeof(DefaultLoader));
	}

	protected override void Awake()
	{
		base.Awake();

		if (m_Camera == null)
			m_Camera = Camera.main;

		//Time.timeScale = 1.5f;
		Time.fixedDeltaTime = ImageAnimation._cImageAnimationScale;

		InitDefaultLoader ();
		InitLuaEnv();
		InitStart();
	}

	void UpdateTimerMgr()
	{
		TimerMgr.Instance.UnScaleTick(Time.unscaledDeltaTime);
		TimerMgr.Instance.ScaleTick(Time.deltaTime);
	}

	void Update()
	{
		UpdateTimerMgr();
	}
	
	private void InitStart()
	{
		// InitLuaPath
		if (m_LuaState != null)
		{
			m_LuaState.Start();
		}

		if (m_LuaLoop == null)
		{
			m_LuaLoop = gameObject.AddComponent<LuaLooper>();
			m_LuaLoop.luaState = m_LuaState;
		}

		if (m_LuaState != null) {
			m_LuaState.Require ("Const");
		}
        /*
		m_LuaState.DoString("testVar = 123", "@AppConst.cs");
		m_LuaState.DoFile("test.lua");
		var func = m_LuaState.GetFunction ("CallTest");
		int top = func.BeginPCall ();
		func.PCall ();
		top = func.GetLuaState ().LuaGetTop ();
		var obj = func.CheckObject (typeof(TestClass));
		func.EndPCall ();*/
	}
}
