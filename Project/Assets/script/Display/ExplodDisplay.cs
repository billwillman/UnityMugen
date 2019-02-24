using System;
using UnityEngine;

public enum ExplodPostType
{
	Screen = 0,
	p1,
	p2
}
	
public class ExplodDisplay: MonoBehaviour
{
	public ExplodPostType m_PostType = ExplodPostType.Screen;
	public int m_Anim = -1;
	public int m_Pal = -1;
	public Vector2 m_Pos = Vector2.zero;
	public int m_Sprpriority = 0;
	public int m_RemoveTime = 0;

	public int Id = -1;

	private ImageAnimation m_ImgAni = null;

	void Awake()
	{
		m_ImgAni = gameObject.AddComponent<ImageAnimation> ();
	}
}

