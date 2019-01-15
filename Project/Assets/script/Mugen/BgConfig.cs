using UnityEngine;
using System.Collections;

namespace Mugen
{
    public enum BgType
    {
        none,
        normal,
        anim
    }

    public enum TransType
    {
        none
    }

    public class BgInfo
    {
        public string name = string.Empty;
        public BgType type = BgType.none;
        public TransType trans = TransType.none;
        public Vector2 delta;
        public int mask = 1;
        public int layerno = 0;
    }

    public class BgStaticInfo : BgInfo
    {
        public int tile = 0;
    }

    public class BgConfig
    {

    }
}
