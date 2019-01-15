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

	public struct BgStaticInfo
	{
		public int srpiteno_Group;
		public int spriteno_Image;
		public int start_x;
		public int start_y;
		public Vector2 delta;
		public int mask;
		public Vector2 velocity;
		public int tile_x;
		public int tile_y;
		public int tilespacing_x;
		public int tilespacing_y;
		public int zoffset;
		public int layerno;
	}

    public class BgConfig
    {

    }
}
