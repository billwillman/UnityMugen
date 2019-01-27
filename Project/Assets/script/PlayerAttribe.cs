using UnityEngine;
using System.Collections;

public class PlayerAttribe : MonoBehaviour {
	// 生命值
	public int life = 1000;
	// 攻击力
	public int attack = 100;
	// 防御力
	public int defence = 100;
	// 倒地时增加防御力百分比
	public int fail__defence_up = 50;
	// 倒地后爬起需要时间
	public int liedown__time = 60;
	// 连击juggle点数
	public int airjuggle = 15;
	// 打击火花号
	public int sparkno = 2;
	// 防御火花号
	public int guard__sparkno = 40;
	// 设置为1时，KO会有回声
	public int ko__echo = 0;
	// 音量调节(负数声音减小)
	public int volume = 0;
	// 变量数量
	public int IntPersistIndex = 60;
	public int FloatPersistIndex = 40;

	public void Init()
	{
		ResetDatas ();
		if (IntPersistIndex <= 0) {
			m_IntVars = null;
		} else {
			if (m_IntVars == null || m_IntVars.Length != IntPersistIndex) {
				m_IntVars = new int[IntPersistIndex];
			}
		}

		if (FloatPersistIndex <= 0)
			m_FloatVars = null;
		else {
			if (m_FloatVars == null || m_FloatVars.Length != FloatPersistIndex) {
				m_FloatVars = new float[FloatPersistIndex];
			}
		}
	}

	public void Clear()
	{
		m_IntVars = null;
		m_FloatVars = null;
	}

	private void ResetDatas()
	{
		// 通过CNS重置
	}

	void OnApplicationQuit()
	{
		Clear ();
	}

	void OnDestroy()
	{
		if (!AppConfig.IsAppQuit)
			Clear ();
	}

	private int[] m_IntVars = null;
	private float[] m_FloatVars = null;
}
