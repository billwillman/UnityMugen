using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SndSound: MonoBehaviour
{
    private AudioSource m_AudioSource = null;

    protected AudioSource Source
    {
        get
        {
            if (m_AudioSource == null)
                m_AudioSource = GetComponent<AudioSource>();
            return m_AudioSource;
        }
    }

	public bool PlaySound(AudioClip clip)
	{
        if (clip == null || clip.length <= 0)
            return false;
        var source = this.Source;
        if (source == null)
            return false;
        source.PlayOneShot(clip);
		return true;
	}
}

