using System;
using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T> {

	private static T m_Instance = null;

	public static T GetInstance()
	{
		if (m_Instance == null) {
			Type t = typeof(T);
			GameObject obj = new GameObject (t.Name, t);
			m_Instance = obj.GetComponent<T> ();
		}
		return m_Instance;
	}

	protected virtual void Awake()
	{
		if (m_Instance == null)
			m_Instance = this as T;
	}
}
