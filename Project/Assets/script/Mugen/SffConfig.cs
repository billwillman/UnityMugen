using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Utils;

namespace Mugen
{
	
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet=System.Runtime.InteropServices.CharSet.Ansi)]
	public struct SFFHEADER {
		
		/// unsigned char[11]
		[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=12)]
		public string signature;
		
		/// unsigned char
		public byte verhi;
		
		/// unsigned char
		public byte verlo;
		
		/// unsigned char
		public byte verhi2;
		
		/// unsigned char
		public byte verlo2;
		
		/// unsigned int
		public uint NumberOfGroups;
		
		/// unsigned int
		public uint NumberOfImage;
		
		/// unsigned int
		public uint SubHeaderFileOffset;
		
		/// unsigned int
		public uint SizeOfSubheader;
		
		/// unsigned char
		public byte PaletteType;
		
		/// unsigned char[476]
		//[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=476)]
		//public string BLANK;
	}

    // 2.0 文件头
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct SFFHEADERv2
    {
        /// unsigned char[11]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 12)]
        public string signature;

        /// unsigned char
        public byte verhi;

        /// unsigned char
        public byte verlo;

        /// unsigned char
        public byte verhi2;

        /// unsigned char
        public byte verlo2;

        public uint reserved1;
        public uint reserved2;

        // compatVerLoad
        public byte compatverlo3;
        public byte compatverlo1;
        public byte compatverlo2;
        public byte compatverhi;

        public uint reserved3;
        public uint reserved4;

        public uint offsetSubFile;
        public uint totalImage;

        public uint offsetPaletteFile;
        public uint totalPalette;

        public uint offsetLData;
        public uint sizeLData;

        public uint offsetTData;
        public uint sizeTData;

        public uint reserved5;
        public uint reserved6;

        /// unsigned char[436]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=436)]
        public string comments;

		public static SFFHEADERv2 LoadFromStream(Stream stream)
		{
			SFFHEADERv2 ret = new SFFHEADERv2 ();
			if (stream == null)
				return ret;
			var mgr = FilePathMgr.GetInstance ();

			ret.signature = mgr.ReadString (stream, 12, System.Text.Encoding.UTF8);
			ret.verhi = (byte)stream.ReadByte ();
			ret.verlo = (byte)stream.ReadByte ();
			ret.verhi2 = (byte)stream.ReadByte ();
			ret.verlo2 = (byte)stream.ReadByte ();
			ret.reserved1 = (uint)mgr.ReadInt (stream);
			ret.reserved2 = (uint)mgr.ReadInt (stream);
			ret.compatverlo3 = (byte)stream.ReadByte ();
			ret.compatverlo1 = (byte)stream.ReadByte ();
			ret.compatverlo2 = (byte)stream.ReadByte ();
			ret.compatverhi = (byte)stream.ReadByte ();
			ret.reserved3 = (uint)mgr.ReadInt (stream);
			ret.reserved4 = (uint)mgr.ReadInt (stream);
			ret.offsetSubFile = (uint)mgr.ReadInt (stream);
			ret.totalImage = (uint)mgr.ReadInt (stream);
			ret.offsetPaletteFile = (uint)mgr.ReadInt (stream);
			ret.totalPalette = (uint)mgr.ReadInt (stream);
			ret.offsetLData = (uint)mgr.ReadInt (stream);
			ret.sizeLData = (uint)mgr.ReadInt (stream);
			ret.offsetTData = (uint)mgr.ReadInt (stream);
			ret.sizeTData = (uint)mgr.ReadInt (stream);
			ret.reserved5 = (uint)mgr.ReadInt (stream);
			ret.reserved6 = (uint)mgr.ReadInt (stream);
			ret.comments = mgr.ReadString (stream, 436, System.Text.Encoding.UTF8);

			return ret;
		}
    }

	
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet=System.Runtime.InteropServices.CharSet.Ansi)]
	public struct SFFSUBHEADER {
		
		/// unsigned int
		public uint NextSubheaderFileOffset;
		
		/// unsigned int
		public uint LenghtOfSubheader;
		
		/// short
		public short x;
		
		/// short
		public short y;
		
		/// short
		public short GroubNumber;
		
		/// short
		public short ImageNumber;
		
		/// short
		public short IndexOfPrevious;
		
		/// boolean
		[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool PalletSame;
		
		/// unsigned char[13]
		[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=13)]
		public string BALNK;
	}

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct SFFSUBHEADERv2
    {
        /// short
        public short GroubNumber;
        /// short
        public short ImageNumber;

        public short width;
        public short height;
        public short x;
        public short y;

        public short IndexOfPrevious;

        public byte fmt;
        public byte coldepth;
        public uint offsetData;
        public uint subfileLength; //(0: linked)
        public short palletteIndex;

        //flags
        //0    unset: literal (use ldata); set: translate (use tdata; decompress on load)
        //1-15 unused
        public short flags;

		public static SFFSUBHEADERv2 LoadFromStream(Stream stream)
		{
			SFFSUBHEADERv2 subHeader = new SFFSUBHEADERv2 ();
			if (stream == null)
				return subHeader;
			var filePathMgr = FilePathMgr.GetInstance ();
			subHeader.GroubNumber = filePathMgr.ReadShort(stream);
			subHeader.ImageNumber = filePathMgr.ReadShort(stream);
			subHeader.width = filePathMgr.ReadShort(stream);
			subHeader.height = filePathMgr.ReadShort(stream);
			subHeader.x = filePathMgr.ReadShort(stream);
			subHeader.y = filePathMgr.ReadShort(stream);
			subHeader.IndexOfPrevious = filePathMgr.ReadShort(stream);
			subHeader.fmt = (byte)stream.ReadByte();
			subHeader.coldepth = (byte)stream.ReadByte();
			subHeader.offsetData = (uint)filePathMgr.ReadInt(stream);
			subHeader.subfileLength = (uint)filePathMgr.ReadInt(stream);
			subHeader.palletteIndex = filePathMgr.ReadShort(stream);
			subHeader.flags = filePathMgr.ReadShort(stream);
			return subHeader;
		}
    }

    public enum PcxCompressType
    {
        raw = 0,
        notused = 1,
        RLE8 = 2,
        RLE5 = 3,
        LZ5 = 4
    }

	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet=System.Runtime.InteropServices.CharSet.Ansi)]
	public struct PCXHEADER {

		public static PCXHEADER LoadFromStream(Stream stream)
		{
			PCXHEADER header = new PCXHEADER ();
			if (stream == null) {
				return header;
			}
			var mgr = FilePathMgr.GetInstance ();
			header.Manufacturer = (byte)stream.ReadByte ();
			header.Version = (byte)stream.ReadByte ();
			header.Encoding = (byte)stream.ReadByte ();
			header.BPP = (byte)stream.ReadByte ();
			header.x = (ushort)mgr.ReadShort (stream);
			header.y = (ushort)mgr.ReadShort (stream);
			header.widht = (ushort)mgr.ReadShort (stream);
			header.height = (ushort)mgr.ReadShort (stream);
			header.HRES = (ushort)mgr.ReadShort (stream);
			header.VRES = (ushort)mgr.ReadShort (stream);
			header.ColorMap = mgr.ReadString (stream, 48, System.Text.Encoding.ASCII);
			header.reserved1 = (byte)stream.ReadByte ();
			header.NPlanes = (byte)stream.ReadByte ();
			header.bytesPerLine = (byte)stream.ReadByte ();
			header.palletInfo = (byte)stream.ReadByte ();
			header.HorzScreenSize = (ushort)mgr.ReadShort (stream);
			header.VertScreenSize = (ushort)mgr.ReadShort (stream);
			header.Reserved2 = mgr.ReadString(stream, 54, System.Text.Encoding.ASCII);
			return header;
		}
		
		/// unsigned char
		public byte Manufacturer;
		
		/// unsigned char
		public byte Version;
		
		/// unsigned char
		public byte Encoding;
		
		/// unsigned char
		public byte BPP;
		
		/// unsigned short
		public ushort x;
		
		/// unsigned short
		public ushort y;
		
		/// unsigned short
		public ushort widht;
		
		/// unsigned short
		public ushort height;
		
		/// unsigned short
		public ushort HRES;
		
		/// unsigned short
		public ushort VRES;
		
		/// unsigned char[48]
		[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=48)]
		public string ColorMap;
		
		/// unsigned char
		public byte reserved1;
		
		/// unsigned char
		public byte NPlanes;
		
		/// unsigned char
		public byte bytesPerLine;
		
		/// unsigned char
		public byte palletInfo;

		public ushort HorzScreenSize;
		public ushort VertScreenSize;
		
		/// unsigned char[58]
		[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=54)]
		public string Reserved2;
	}

	public struct PCXDATA
	{
		public byte[] data;
		public Color32[] pallet;
		public Texture2D GetPalletTexture(bool is32Bit)
		{
			return SffFile.GeneratorPalletTexture(pallet, is32Bit);
		}
		public KeyValuePair<short, short> palletLink;
		public bool IsVaildPalletLink
		{
			get
			{
				return palletLink.Key >= 0 && palletLink.Value >= 0;
			}
		}
	}

	
	public class SffFile
	{
		private static readonly string _cElecbyteSpr = "ElecbyteSpr";

		private Color32[] mNormalPallet = null;

		// Load Normal Pallet
		private bool LoadActToSff(byte[] actSource)
		{
			mNormalPallet = null;
			if ((actSource == null) || (actSource.Length <= 0))
				return false;
			int n = (int)(actSource.Length/3);
			if (n <= 0)
				return false;
			mNormalPallet = new Color32[n];
			int offset = 0;
			for (int j = n - 1; j >= 0; --j)
			{
				byte r = actSource[offset++];
				byte g = actSource[offset++];
				byte b = actSource[offset++];
				byte a;
				if (j == 0)
					a = 0;
				else
				{

					if ((r == mNormalPallet[j - 1].r) && (g == mNormalPallet[j - 1].g) && (b == mNormalPallet[j - 1].b))
						a = 0;
					else
						a = 0xFF;
				}
				mNormalPallet[j] = new Color32(r, g, b, a);
			}

			return true;
		}

		private byte GetPalletAlpha(byte r, byte g, byte b, Color32[] pallet)
		{
			if ((pallet == null) || (pallet.Length <= 0))
				return 0;
			byte a;
			if ((r == pallet[0].r) && (g == pallet[0].g) && (b == pallet[0].b))
				a = 0;
			else
				a = 0xFF;
			return a;
		}

		protected bool HasNormalPallet
		{
			get
			{
				return (mNormalPallet != null);
			}
		}

		public Texture2D GetNormalPalletTexture(bool is32Bit)
		{
			if (!HasNormalPallet)
				return null;
			Texture2D ret = GeneratorPalletTexture(mNormalPallet, is32Bit);
			return ret;
		}

		private void SaveTexture(string fileName, Texture2D texture)
		{
			if (string.IsNullOrEmpty(fileName) || (texture == null))
				return;

			if (texture != null)
			{
				byte[] b = texture.EncodeToPNG();
				if (b != null)
				{
					System.IO.FileStream stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
					stream.Write(b, 0, b.Length);
					stream.Close();
					stream.Dispose();
				}
			}
		}

		public static Texture2D GeneratorActTexture(string playerName, string palletName, bool is32Bit)
		{
			string fileName = string.Format("{0}@{1}/{2}.act.bytes", AppConfig.GetInstance().PlayerRootDir, playerName, palletName);
			byte[] bytes = AppConfig.GetInstance().Loader.LoadBytes(fileName);
			return GeneratorActTexture(bytes, is32Bit);
		}

		public static Texture2D GeneratorActTexture(byte[] actSource, bool is32Bit)
		{
			if ((actSource == null) || (actSource.Length <= 0))
				return null;
			int n = (int)(actSource.Length/3);
			if (n <= 0)
				return null;

			Color32[] pallet = new Color32[n];
			int offset = 0;
			for (int j = n - 1; j >= 0; --j)
			{
				byte r = actSource[offset++];
				byte g = actSource[offset++];
				byte b = actSource[offset++];
				byte a;
				if (j == 0)
					a = 0;
				else
				{
					
					if ((r == pallet[j - 1].r) && (g == pallet[j - 1].g) && (b == pallet[j - 1].b))
						a = 0;
					else
						a = 0xFF;
				}
				pallet[j] = new Color32(r, g, b, a);
			}

			return GeneratorPalletTexture(pallet, is32Bit);
		}

		public static Texture2D GeneratorPalletTexture(Color32[] pallet, bool is32Bit)
		{
			if ((pallet == null) || (pallet.Length <= 0))
				return null;
			TextureFormat fmt;
			if (is32Bit)
				fmt = TextureFormat.ARGB32;
			else
				fmt = TextureFormat.ARGB4444;
			Texture2D ret = new Texture2D(pallet.Length, 1, fmt, false, false);
			ret.filterMode = FilterMode.Point;
			ret.wrapMode = TextureWrapMode.Clamp;

			byte[] raw;
			if (is32Bit)
				raw = new byte[pallet.Length  * 4];
			else
				raw = new byte[pallet.Length * 2];
			
			for (int idx = 0; idx < pallet.Length; ++idx)
			{
				Color32 color = pallet[idx];
				if (is32Bit)
				{
					int rawIdx = idx * 4;
					raw[rawIdx++] = color.a;
					raw[rawIdx++] = color.r;
					raw[rawIdx++] = color.g;
					raw[rawIdx++] = color.b;
				} else
				{
					int rawIdx = idx * 2;
					byte v = (byte)(((color.b & 0xF0) >> 4) & ((color.g & 0xF0)));
					raw[rawIdx++] = v;
					v = (byte)(((color.r & 0xF0) >> 4) & ((color.a & 0xF0)));
					raw[rawIdx++] = v;
				}
			}
			
			ret.LoadRawTextureData(raw);
			ret.Apply();

			return ret;
		}

		public bool ChangeNormalPallet(string playerName, string name)
		{
			mNormalPallet = null;
			if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(name))
				return false;
			string fileName = string.Format("{0}@{1}/{2}.act.bytes", AppConfig.GetInstance().PlayerRootDir, playerName, name);
			byte[] bytes = AppConfig.GetInstance().Loader.LoadBytes(fileName);
			return LoadActToSff(bytes);
		}

		public bool LoadFromFileName(string fileName)
		{
			if (string.IsNullOrEmpty (fileName))
				return false;
			byte[] bytes = AppConfig.GetInstance ().Loader.LoadBytes (fileName);
			return Load(bytes);
		}

		public bool LoadChar(string charName, string customSpriteName = "", bool isLoadNormalPallet = true)
		{
			if (isLoadNormalPallet)
			{
				ChangeNormalPallet(charName, charName);
			}
			if (string.IsNullOrEmpty (customSpriteName))
				customSpriteName = charName;
			string fileName = string.Format("{0}@{1}/{2}.sff.bytes", AppConfig.GetInstance().PlayerRootDir, charName, customSpriteName);
			byte[] bytes = AppConfig.GetInstance().Loader.LoadBytes(fileName);
			return Load(bytes);
		}

		private bool Load(byte[] bytes)
		{
			mPcxDataMap.Clear();
			if (mSubHeaders != null)
				mSubHeaders.Clear();
			if ((bytes == null) || (bytes.Length <= 0))
			{
				mIsVaild = false;
				return false;
			}

            // 判断是否是v2的文件
            if (bytes.Length < 16)
            {
                mIsVaild = false;
                return false;
            }

            byte v1 = bytes[15];
            byte v2;
            byte v3;
            byte v4;
            if (v1 == 2)
            {
				
                SFFHEADERv2 header = new SFFHEADERv2();
                int headerSize = Marshal.SizeOf(header);
                IntPtr headerBuffer = Marshal.AllocHGlobal(headerSize);
                try
                {
                    Marshal.Copy(bytes, 0, headerBuffer, headerSize);
                    header = (SFFHEADERv2)Marshal.PtrToStructure(headerBuffer, typeof(SFFHEADERv2));
                }
                finally
                {
                    Marshal.FreeHGlobal(headerBuffer);
                }
				MemoryStream stream = new MemoryStream (bytes);
				try
				{
				//	SFFHEADERv2 header = SFFHEADERv2.LoadFromStream (stream);

					int comp = string.Compare(header.signature, _cElecbyteSpr, true);
					if (comp != 0)
                    	return false;

                	v1 = header.verlo2;
                	v2 = header.verlo;
                	v3 = header.verhi2;
                	v4 = header.verhi;

					if (!LoadSubFilesV2(header, stream))
                    	return false;
				} finally {
					stream.Close ();
					stream.Dispose ();
				}
            }
            else if (v1 == 1)
            {

                SFFHEADER header = new SFFHEADER();
                int headerSize = Marshal.SizeOf(header);
                IntPtr headerBuffer = Marshal.AllocHGlobal(headerSize);
                try
                {
                    Marshal.Copy(bytes, 0, headerBuffer, headerSize);
                    header = (SFFHEADER)Marshal.PtrToStructure(headerBuffer, typeof(SFFHEADER));
                }
                finally
                {
                    Marshal.FreeHGlobal(headerBuffer);
                }

                if (string.Compare(header.signature, _cElecbyteSpr, true) != 0)
                    return false;

                v1 = header.verlo2;
                v2 = header.verlo;
                v3 = header.verhi2;
                v4 = header.verhi;

                if (v1 > 1)
                {
                    Debug.LogErrorFormat("sff file not supoort v{0:D}.{1:D}.{2:D}.{3:D}", v1, v2, v3, v4);
                    return false;
                }

                if (!LoadSubFiles(header, bytes))
                    return false;

                if (!LoadPcxs(header, bytes))
                    return false;
            } else
            {
                Debug.LogErrorFormat("sff file not supoort v{0:D}", v1);
                return false;
            }

            if (v1 > 1)
            {
                Debug.LogErrorFormat("sff file not supoort v{0:D}.{1:D}.{2:D}.{3:D}", v1, v2, v3, v4);
                return false;
            }

			mIsVaild = true;
			return true;
		}

		private byte[] DecodePcxData(int offset, PCXHEADER header, byte[] source)
		{
			byte[] ret = null;

			if ((offset < 0) || (offset >= source.Length))
				return ret;

			//int nTotalyByte = (int)(header.bytesPerLine * header.NPlanes);
			int bpp = (int)(header.NPlanes * 8);
			if (bpp > 8)
				return ret; // not support

			try
			{
			int width = header.widht;
			if (width < header.bytesPerLine * header.NPlanes)
				width = header.bytesPerLine * header.NPlanes;

			int size = 0;
			int Pos = 0;
			ret = new byte[header.widht * header.NPlanes * header.height + 1];
			for (int y = 0; y < header.height; ++y)
			{
				int x = 0;
				while (x < width)
				{
					byte byData = source[offset + Pos++];
					if ((byData & 0xC0) == 0xC0)
					{
						size = byData & 0x3F;
						byData = source[offset + Pos++];
					} else
					{
						size = 1;
					}

					while (size-- > 0)
					{
						if (x <= header.widht)
							ret[x + (y * header.widht)] = byData;
						//this it to Skip blank data on PCX image wich are on the right side
						// TODO:OK? Skip two bytes
						if ((x == width) && (width != header.widht))
						{
							int nHowManyBlank = width - (int)header.widht;
							for (int i = 0; i < nHowManyBlank; ++i)
								Pos += 2;
						}


						x++;
					}
				}
			}

				// H changed
				byte[] temp = new byte[header.widht];
				for (int y = 0; y < (int)header.height/2; ++y)
				{
					int x = ((int)header.height - 1 - y);
					int s = y * header.widht;
					int d = x * header.widht;
					Buffer.BlockCopy(ret, d, temp, 0, header.widht);
					Buffer.BlockCopy(ret, s, ret, d, header.widht);
					Buffer.BlockCopy(temp, 0, ret, s, header.widht);
				}
			} catch(Exception e)
			{
				Debug.LogError(e.Message);
				return null;
			}



			return ret;
		}

		public Texture2D GetIndexTexture(uint group, uint image)
		{
			KeyValuePair<PCXHEADER, PCXDATA> data;
			if (!GetPcxData(group, image, out data))
				return null;
			if ((data.Value.data == null) || (data.Value.data.Length <= 0))
				return null;
			/*Color32[] curPattle;

			if (data.Value.pallet == null)
				curPattle = mNormalPallet;
			else
				curPattle = data.Value.pallet;
			
			if (curPattle == null)
				return null;*/

			Texture2D ret = new Texture2D(data.Key.widht, data.Key.height, TextureFormat.Alpha8, false, false);
			ret.filterMode = FilterMode.Point;
			ret.wrapMode = TextureWrapMode.Clamp;

			ret.LoadRawTextureData(data.Value.data);
			ret.Apply();
	
			return ret;
		}

		public Texture2D GetTexture(uint group, uint image, bool is32Bit)
		{
			KeyValuePair<PCXHEADER, PCXDATA> data;
			if (!GetPcxData(group, image, out data))
				return null;
			if ((data.Value.data == null) || (data.Value.data.Length <= 0))
				return null;
			Color32[] curPattle;
			if (data.Value.pallet == null)
				curPattle = mNormalPallet;
			else
				curPattle = data.Value.pallet;

			if (curPattle == null)
				return null;
			TextureFormat fmt;
			if (is32Bit)
				fmt = TextureFormat.ARGB32;
			else
				fmt = TextureFormat.ARGB4444;
			Texture2D ret = new Texture2D(data.Key.widht, data.Key.height, fmt, false);

			byte[] raw;
			if (is32Bit)
				raw = new byte[data.Key.widht * data.Key.height * 4];
			else
				raw = new byte[data.Key.widht * data.Key.height * 2];

			for (int r = data.Key.height - 1; r >= 0 ; --r)
			{
				for (int c = 0; c < data.Key.widht; ++c)
				{
					int idx = r * data.Key.widht + c;
					if (idx >= data.Value.data.Length)
					{
						Debug.LogError("Pcx Data error");
						break;
					}
					int palletIdx = data.Value.data[idx];
					if (palletIdx >= curPattle.Length)
					{
						Debug.LogError("palletIdx is error");
						continue;
					}
					Color32 color = curPattle[palletIdx];
					// change height
					//idx = (data.Key.height - 1 - r) * data.Key.widht + c;
					if (is32Bit)
					{
						int rawIdx = idx * 4;
						raw[rawIdx++] = color.a;
						raw[rawIdx++] = color.r;
						raw[rawIdx++] = color.g;
						raw[rawIdx++] = color.b;
					} else
					{
						int rawIdx = idx * 2;
						byte v = (byte)(((color.b & 0xF0) >> 4) & ((color.g & 0xF0)));
						raw[rawIdx++] = v;
						v = (byte)(((color.r & 0xF0) >> 4) & ((color.a & 0xF0)));
						raw[rawIdx++] = v;
					}
				}
			}

			ret.LoadRawTextureData(raw);
			ret.Apply();
			return ret;
		}


		private KeyValuePair<short, short> m_currentLink = new KeyValuePair<short, short> (-1, -1);
		private bool LoadPcx(int offset, SFFSUBHEADER subHeader, byte[] source, out KeyValuePair<PCXHEADER, PCXDATA> dataPair)
		{
			if ((offset < 0) || (offset >= source.Length))
			{
				dataPair = new KeyValuePair<PCXHEADER, PCXDATA>();
				return false;
			}

			PCXHEADER header = new PCXHEADER();
			int bufSize = Marshal.SizeOf(header);
			if (offset + bufSize > source.Length)
			{
				dataPair = new KeyValuePair<PCXHEADER, PCXDATA>();
				return false;
			}
			IntPtr buf = Marshal.AllocHGlobal(bufSize);
			try
			{
				Marshal.Copy(source, offset, buf, bufSize);
				header = (PCXHEADER)Marshal.PtrToStructure(buf, typeof(PCXHEADER));
			} finally
			{
				Marshal.FreeHGlobal(buf);
			}

			if (header.BPP == 0)
			{
				dataPair = new KeyValuePair<PCXHEADER, PCXDATA>();
				return true;
			}

			offset += bufSize;

			int pcxBufSz = (int)subHeader.LenghtOfSubheader - 127;
			if (pcxBufSz <= 0)
			{
				dataPair = new KeyValuePair<PCXHEADER, PCXDATA>();
				return true;
			}

			if (offset + pcxBufSz > source.Length)
			{
				dataPair = new KeyValuePair<PCXHEADER, PCXDATA>();
				return true;
			}
			byte[] pcxBuf = new byte[pcxBufSz];
			Buffer.BlockCopy(source, offset, pcxBuf, 0, pcxBufSz); 
			offset += pcxBufSz;

			if (offset >= source.Length)
			{
				dataPair = new KeyValuePair<PCXHEADER, PCXDATA>();
				return false;
			}

			header.widht = (ushort)(header.widht - header.x + 1);
			header.height = (ushort)(header.height - header.y + 1);

			//byte[] dst = DecodePcxData(offset, header, pcxBuf);
			byte[] dst = DecodePcxData(0, header, pcxBuf);
			pcxBuf = null;
			if ((dst == null) || (dst.Length <= 0))
			{
				// empty
				dataPair = new KeyValuePair<PCXHEADER, PCXDATA>();
				return true;
			}


			PCXDATA pcxData = new PCXDATA();
			pcxData.data = dst;
			pcxData.pallet = null;

			offset -= 768;
			//eat empty 8bit
			offset++;

			byte s = source [offset++];
			if ((s == 12) && !subHeader.PalletSame && !HasNormalPallet && header.NPlanes <= 1) 
           // if (!subHeader.PalletSame && !HasNormalPallet && header.NPlanes <= 1)
            {			
				// load pallet
				pcxData.pallet = new Color32[256];
				for (int i = 0; i < 256; ++i) {
					byte r = source [offset++];
					byte g = source [offset++];
					byte b = source [offset++];
					byte a;
					if (i == 0)
						a = 0;
					else {

						if ((r == pcxData.pallet [i - 1].r) && (g == pcxData.pallet [i - 1].g) && (b == pcxData.pallet [i - 1].b))
							a = 0;
						else
							a = 0xFF;
					}
					pcxData.pallet [i] = new Color32 (r, g, b, a);
				}
				m_currentLink = new KeyValuePair<short, short>(subHeader.GroubNumber, subHeader.ImageNumber);
			} else
				pcxData.palletLink = m_currentLink;

			dataPair = new KeyValuePair<PCXHEADER, PCXDATA>(header, pcxData);

			return true;
		}

        private bool LoadPcxs(SFFHEADER sffHeader, byte[] source)
		{
			if ((source == null) || (source.Length <= 0))
				return false;
			if ((mSubHeaders == null) || (mSubHeaders.Count <= 0))
				return true;

			bool ret = true;
			int offset = (int)sffHeader.SubHeaderFileOffset;
			for (int i = 0; i < mSubHeaders.Count; ++i)
			{
				SFFSUBHEADER header;
				if (!GetSubHeader(i, out header))
				{
					ret = false;
					break;
				}

				KeyValuePair<uint, uint> key = new KeyValuePair<uint, uint>((uint)header.GroubNumber, (uint)header.ImageNumber);
				if (mPcxDataMap.ContainsKey(key))
				{
					offset = (int)header.NextSubheaderFileOffset;
					if (offset == 0 || offset >= source.Length)
						break;
					continue;
				}

                /*
                // 檢查indexPrevious
                if (header.LenghtOfSubheader == 0 && header.IndexOfPrevious != 0)
                {
                    offset = (int)header.NextSubheaderFileOffset;
                    if (offset == 0 || offset >= source.Length)
                        break;
                    continue;
                }
                 * */

				offset += Marshal.SizeOf(header);
				KeyValuePair<PCXHEADER, PCXDATA> value;
				if (!LoadPcx(offset, header, source, out value))
				{
					Debug.LogErrorFormat("LoadPcxs: index = {0} error", i);
					ret = false;
					break;
				}


				mPcxDataMap.Add(key, value);

				offset = (int)header.NextSubheaderFileOffset;
				if (offset == 0 || offset >= source.Length)
					break;
			}

			return ret;
		}

		private bool LoadSubFilesV2(SFFHEADERv2 header, Stream stream)
		{
			if (stream == null || header.offsetSubFile == 0)
				return false;
			int offset = (int)header.offsetSubFile;
			if (offset < 0 || offset >= stream.Length)
				return false;
			int cnt = (int)header.totalImage;
			if (cnt < 0)
				return false;

			if (stream.Seek (offset, SeekOrigin.Begin) != offset)
				return false;
			var filePathMgr = FilePathMgr.GetInstance ();
			for (int i = 0; i < cnt; ++i) {
				SFFSUBHEADERv2 subHeader = SFFSUBHEADERv2.LoadFromStream (stream);
				SubHeadersV2.Add (subHeader);
			}

			if (mSubHeadersV2 != null) {
				for (int i = 0; i < mSubHeadersV2.Count; ++i) {
					var subHeader = mSubHeadersV2 [i];
					KeyValuePair<uint, uint> key = new KeyValuePair<uint, uint> ((uint)subHeader.GroubNumber, (uint)subHeader.ImageNumber);
					if (mPcxDataMap.ContainsKey (key))
						continue;
					// 读取pcx
					if (subHeader.subfileLength == 0 && subHeader.IndexOfPrevious != 0) {
						// LINK模式
					} else {
						if (subHeader.subfileLength == 0)
							continue;
						int off = (int)subHeader.offsetData;
						if (off > 0) {
							stream.Seek (off, SeekOrigin.Begin);

							byte[] buffer = new byte[subHeader.subfileLength];
							stream.Read (buffer, 0, buffer.Length);
							buffer = DeCompressBuffer (buffer, (PcxCompressType)subHeader.fmt);
						}
					}
				}
			}
			return true;
		}

		private byte[] DeCompressBuffer(byte[] buffer, PcxCompressType compressType)
		{
			if (buffer == null || buffer.Length <= 0)
				return buffer;
			if (compressType == PcxCompressType.notused || compressType == PcxCompressType.raw)
				return buffer;

			// 未完
			
			return buffer;
		}

		private bool LoadSubFiles(SFFHEADER header, byte[] source)
		{
			if ((header.NumberOfGroups == 0) && (header.NumberOfImage == 0))
				return true;
	
			if ((header.SubHeaderFileOffset == 0))
				return false;
			if (!LoadSubFiles((int)header.SubHeaderFileOffset, source))
				return false;
			return (int)header.NumberOfImage == mSubHeaders.Count;
		}

		private bool LoadSubFiles(int offset, byte[] source)
		{
			if (offset < 0)
				return false;
			SFFSUBHEADER header = new SFFSUBHEADER();
			int headerSize = Marshal.SizeOf(header);
			if (headerSize + offset > source.Length)
			{
				// File is Eof
				return true;
			}
			IntPtr headerBuf = Marshal.AllocHGlobal(headerSize);
			try
			{
				Marshal.Copy(source, offset, headerBuf, headerSize);
				header = (SFFSUBHEADER)Marshal.PtrToStructure(headerBuf, typeof(SFFSUBHEADER));
			} finally
			{
				Marshal.FreeHGlobal(headerBuf);
			}

			SubHeaders.Add(header);

			// load pcx
			/*
			KeyValuePair<PCXHEADER, PCXDATA> pcxData;
			if (!LoadPcx(offset, header, source, out pcxData))
				return false;
			KeyValuePair<short, short> key = new KeyValuePair<short, short>(header.GroubNumber, header.ImageNumber);
			*/
			if (header.NextSubheaderFileOffset != 0)
			{
				if (header.NextSubheaderFileOffset >= source.Length)
					return true;
				if (!LoadSubFiles((int)header.NextSubheaderFileOffset, source))
					return false;
			}

			return true;
		}

		public bool IsLoadVaild
		{
			get
			{
				return mIsVaild;
			}
		}

		public int SubHeaderCount
		{
			get
			{
				if (mSubHeaders == null && mSubHeadersV2 == null)
					return 0;
				if (mSubHeaders != null)
				 return mSubHeaders.Count;
				if (mSubHeadersV2 != null)
					return mSubHeadersV2.Count;
				return 0;
			}
		}

		public bool GetSubHeader(int group, int image, out SFFSUBHEADER header)
		{
			if ((mSubHeaders == null) || (group < 0) || (image < 0))
			{
				header = new SFFSUBHEADER();
				return false;
			}

			if (mSubHeaders != null)
			for (int i = 0; i < mSubHeaders.Count; ++i)
			{
				SFFSUBHEADER sub = mSubHeaders[i];

				if ((sub.LenghtOfSubheader == 0) && (sub.IndexOfPrevious > 0) && (sub.IndexOfPrevious <= mSubHeaders.Count))
				{
					sub = mSubHeaders[sub.IndexOfPrevious - 1];
				}

				int g = (int)sub.GroubNumber;
				int img = (int)sub.ImageNumber;

				if ((g == group) && (img == image))
				{
					header = sub;
					return true;
				}
			}

			header = new SFFSUBHEADER();
			return false;
		}

		public bool GetSubHeaderV2(int group, int image, out SFFSUBHEADERv2 header)
		{
			if ((mSubHeadersV2 == null) || (group < 0) || (image < 0))
			{
				header = new SFFSUBHEADERv2();
				return false;
			}

			if (mSubHeadersV2 != null)
				for (int i = 0; i < mSubHeadersV2.Count; ++i)
				{
					SFFSUBHEADERv2 sub = mSubHeadersV2[i];

					if ((sub.subfileLength == 0) && (sub.IndexOfPrevious > 0) && (sub.IndexOfPrevious <= mSubHeadersV2.Count))
					{
						sub = mSubHeadersV2[sub.IndexOfPrevious - 1];
					}

					int g = (int)sub.GroubNumber;
					int img = (int)sub.ImageNumber;

					if ((g == group) && (img == image))
					{
						header = sub;
						return true;
					}
				}

			header = new SFFSUBHEADERv2();
			return false;
		}

		public bool IsSFFHeaderV2
		{
			get {
				return mSubHeaders == null && mSubHeadersV2 != null;
			}
		}

		public bool GetSubHeader(int index, out SFFSUBHEADER header)
		{
			if ((mSubHeaders == null) || (index < 0) || (index >= mSubHeaders.Count))
			{
				header = new SFFSUBHEADER();
				return false;
			}

			header = mSubHeaders[index];

           // while (true)
            {
                if ((header.LenghtOfSubheader == 0) && (header.IndexOfPrevious > 0) && (header.IndexOfPrevious < mSubHeaders.Count))
                {
                    header = mSubHeaders[header.IndexOfPrevious - 1];
                }
              //  else
               //     break;
            }
			return true;
		}

		public bool GetSubHeaderV2(int index, out SFFSUBHEADERv2 header)
		{
			if ((mSubHeadersV2 == null) || (index < 0) || (index >= mSubHeadersV2.Count))
			{
				header = new SFFSUBHEADERv2();
				return false;
			}

			header = mSubHeadersV2[index];

			// while (true)
			{
				if ((header.subfileLength == 0) && (header.IndexOfPrevious > 0) && (header.IndexOfPrevious < mSubHeadersV2.Count))
				{
					header = mSubHeadersV2[header.IndexOfPrevious - 1];
				}
				//  else
				//     break;
			}
			return true;
		}

		public bool GetPcxData(uint group, uint image, out KeyValuePair<PCXHEADER, PCXDATA> data)
		{
			KeyValuePair<uint, uint> key = new KeyValuePair<uint, uint>(group, image);
			if (!mPcxDataMap.TryGetValue(key, out data))
				return false;
			return true;
		}

		public bool GetPcxData(uint group, uint image, out PCXDATA data)
		{
			KeyValuePair<PCXHEADER, PCXDATA> pair;
			if (!GetPcxData(group, image, out pair))
			{
				data = new PCXDATA();
				return false;
			}

			data = pair.Value;
			return true;
		}

		protected List<SFFSUBHEADER> SubHeaders
		{
			get
			{
				if (mSubHeaders == null)
					mSubHeaders = new List<SFFSUBHEADER>();
				return mSubHeaders;
			}
		}

        protected List<SFFSUBHEADERv2> SubHeadersV2
        {
            get
            {
                if (mSubHeadersV2 == null)
                    mSubHeadersV2 = new List<SFFSUBHEADERv2>();
                return mSubHeadersV2;
            }
        }

		private bool mIsVaild = false;
		private List<SFFSUBHEADER> mSubHeaders = null;
        private List<SFFSUBHEADERv2> mSubHeadersV2 = null;
		// key = group, image
		private Dictionary<KeyValuePair<uint, uint>, KeyValuePair<PCXHEADER, PCXDATA>> mPcxDataMap = new Dictionary<KeyValuePair<uint, uint>, KeyValuePair<PCXHEADER, PCXDATA>>();
	}
}