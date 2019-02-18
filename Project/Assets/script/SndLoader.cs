using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Utils;

public struct SndFileHeader
{
    public string tile;
    public uint version;
    public uint soudNum;
    public uint firstSoundOffset;

    public bool LoadFromStream(Stream stream)
    {
        if (stream == null || stream.Length <= 0)
            return false;
        var mgr = FilePathMgr.Instance;
        tile = mgr.ReadString(stream, 12);
        version = (uint)mgr.ReadInt(stream);
        soudNum = (uint)mgr.ReadInt(stream);
        firstSoundOffset = (uint)mgr.ReadInt(stream);
        return true;
    }
}

public struct SndSubFileHeader
{
    public uint NextSoundOffset;
    public uint wavBuffSize;
    public int group;
    public int index;
    //public byte[] wavBuf;

    public bool LoadFromStream(Stream stream)
    {
        if (stream == null || stream.Length <= 0)
            return false;
        var mgr = FilePathMgr.Instance;
        NextSoundOffset = (uint)mgr.ReadInt(stream);
        wavBuffSize = (uint)mgr.ReadInt(stream);
        group = mgr.ReadInt(stream);
        index = mgr.ReadInt(stream);
        return true;
    }
}

public class SndLoader : MonoBehaviour {

    private Dictionary<KeyValuePair<int, int>, byte[]> m_SoundBufMap = null;
    private Dictionary<KeyValuePair<int, int>, AudioClip> m_SoundClipMap = null;
    private bool m_IsInited = false;
    public bool IsLoadedOk = false;

    public Dictionary<KeyValuePair<int, int>, byte[]>.Enumerator GetSoundIter()
    {
        if (m_SoundBufMap == null)
            return new Dictionary<KeyValuePair<int, int>, byte[]>.Enumerator();
        return m_SoundBufMap.GetEnumerator();
    }

    public int SoundCount
    {
        get
        {
            if (m_SoundBufMap == null)
                return 0;
            return m_SoundBufMap.Count;
        }
    }

    public bool IsInited
    {
        get
        {
            return m_IsInited;
        }
    }

    public AudioClip GetSoundClip(int group, int index)
    {
        KeyValuePair<int, int> key = new KeyValuePair<int, int>(group, index);
        AudioClip ret = null;
        if (m_SoundClipMap != null)
        {
            if (m_SoundClipMap.TryGetValue(key, out ret))
                return ret;
        }
        byte[] buf = GetSoundBuf(group, index);
        if (buf == null || buf.Length <= 0)
            return null;
        WAV wav = new WAV(buf);
        m_SoundBufMap[key] = null;

        string name = string.Format("{0:D}:{1:D}", group, index);
        ret = AudioClip.Create(name, wav.SampleCount, 1, wav.Frequency, false, false);
        ret.SetData(wav.LeftChannel, 0);
        if (m_SoundClipMap == null)
            m_SoundClipMap = new Dictionary<KeyValuePair<int, int>, AudioClip>();

        m_SoundClipMap[key] = ret;
        
        return ret;
    }

    public byte[] GetSoundBuf(int group, int index)
    {
        byte[] ret;
        if (m_SoundBufMap != null)
        {
            KeyValuePair<int, int> key = new KeyValuePair<int, int>(group, index);
            if (m_SoundBufMap.TryGetValue(key, out ret))
                return ret;
        }
        else
            ret = null;
        return ret;
    }

    public void Clear()
    {
        if (m_SoundBufMap != null)
        {
            m_SoundBufMap.Clear();
        }

        if (m_SoundClipMap != null)
        {
            var iter = m_SoundClipMap.GetEnumerator();
            while (iter.MoveNext())
            {
                var clip = iter.Current.Value;
                if (clip != null)
                    GameObject.Destroy(clip);
            }
            iter.Dispose();
            m_SoundClipMap.Clear();
        }
    }

    void OnDestroy()
    {
        if (!AppConfig.IsAppQuit)
            Clear();
    }

    private bool LoadFromBuffer(byte[] buffer)
    {
        if (buffer == null || buffer.Length <= 0)
            return false;

        MemoryStream stream = new MemoryStream(buffer);
        try
        {
            SndFileHeader header = new SndFileHeader();
            if (!header.LoadFromStream(stream))
                return false;
            stream.Seek(header.firstSoundOffset, SeekOrigin.Begin);
            for (int i = 0; i < header.soudNum; ++i)
            {
                SndSubFileHeader subHeader = new SndSubFileHeader();
                if (!subHeader.LoadFromStream(stream))
                    return false;

                if (subHeader.wavBuffSize > 0)
                {
                    byte[] buf = new byte[subHeader.wavBuffSize];
                    stream.Read(buf, 0, buf.Length);
                    if (m_SoundBufMap == null)
                        m_SoundBufMap = new Dictionary<KeyValuePair<int, int>, byte[]>();
                    KeyValuePair<int, int> key = new KeyValuePair<int, int>(subHeader.group, subHeader.index);
                    m_SoundBufMap[key] = buf;
                }

                stream.Seek(subHeader.NextSoundOffset, SeekOrigin.Begin);
            }

            return m_SoundBufMap != null && m_SoundBufMap.Count == header.soudNum;

        } finally
        {
            stream.Close();
            stream.Dispose();
        }
    }

    public bool Load(DefaultLoaderPlayer loaderPlayer)
    {
        if (m_IsInited)
            return true;

        if (loaderPlayer == null)
            return false;
        GlobalPlayerLoaderResult result;
        var player = GlobalConfigMgr.GetInstance().LoadPlayer(loaderPlayer, out result);
        if (player == null || player.PlayerCfg == null || !player.PlayerCfg.IsVaild || player.PlayerCfg.Files == null)
            return false;

        string soundName = player.PlayerCfg.Files.sound;
        if (string.IsNullOrEmpty(soundName))
            return false;
        soundName = GlobalConfigMgr.GetConfigFileNameNoExt(soundName);
        if (string.IsNullOrEmpty(soundName))
            return false;

        string playerName = loaderPlayer.GetPlayerName();
        if (string.IsNullOrEmpty(playerName))
            return false;
        string fileName = string.Format("{0}@{1}/{2}.snd.bytes", AppConfig.GetInstance().PlayerRootDir, playerName, soundName);
        byte[] buffer = AppConfig.GetInstance().Loader.LoadBytes(fileName);
        if (buffer == null || buffer.Length <= 0)
            return false;

        m_IsInited = true;
        IsLoadedOk = LoadFromBuffer(buffer);
        return IsLoadedOk;
    }
}
