using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Mugen
{
	
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet=System.Runtime.InteropServices.CharSet.Ansi)]
	public struct SFFHEADER {
		
		/// unsigned char[11]
		[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=11)]
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
		[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=476)]
		public string BLANK;
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

	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet=System.Runtime.InteropServices.CharSet.Ansi)]
	public struct PCXHEADER {
		
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
		public byte reserved;
		
		/// unsigned char
		public byte NPlanes;
		
		/// unsigned char
		public byte bytesPerLine;
		
		/// unsigned char
		public byte palletInfo;
		
		/// unsigned char[58]
		[System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=58)]
		public string Filler;
	}

	public struct PCXDATA
	{
		public byte[] data;
		public Color32[] pallet;
		public Texture2D GetPalletTexture(bool is32Bit)
		{
			return SffFile.GeneratorPalletTexture(pallet, is32Bit);
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
					System.IO.FileStream stream = new System.IO.FileStream(fileName, System.IO.FileMode.CreateNew);
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

		public bool LoadChar(string charName, bool isLoadNormalPallet = true)
		{
			if (isLoadNormalPallet)
			{
				ChangeNormalPallet(charName, charName);
			}

			string fileName = string.Format("{0}{1}/{2}.sff.bytes", AppConfig.GetInstance().PlayerRootDir, charName, charName);
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
			SFFHEADER header = new SFFHEADER();
			int headerSize = Marshal.SizeOf(header);
			IntPtr headerBuffer = Marshal.AllocHGlobal(headerSize);
			try
			{
				Marshal.Copy(bytes, 0, headerBuffer, headerSize);
				header = (SFFHEADER)Marshal.PtrToStructure(headerBuffer, typeof(SFFHEADER));
			} finally
			{
				Marshal.FreeHGlobal(headerBuffer);
			}

			if (string.Compare(header.signature, _cElecbyteSpr, true) != 0)
				return false;

			if (!LoadSubFiles(header, bytes))
				return false;
		
			if (!LoadPcxs(header, bytes))
				return false;

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

			if ((source[offset++] == 12) && !subHeader.PalletSame && !HasNormalPallet && header.NPlanes <= 1)
			{
				// load pallet
				pcxData.pallet = new Color32[256];
				for (int i = 0; i < 256; ++i)
				{
					byte r = source[offset++];
					byte g = source[offset++];
					byte b = source[offset++];
					byte a;
					if (i == 0)
						a = 0;
					else
					{

						if ((r == pcxData.pallet[i - 1].r) && (g == pcxData.pallet[i - 1].g) && (b == pcxData.pallet[i - 1].b))
							a = 0;
						else
							a = 0xFF;
					}
					pcxData.pallet[i] = new Color32(r, g, b, a);
				}
			}

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
				if (mSubHeaders == null)
					return 0;
				return mSubHeaders.Count;
			}
		}

		public bool GetSubHeader(int group, int image, out SFFSUBHEADER header)
		{
			if ((mSubHeaders == null) || (group < 0) || (image < 0))
			{
				header = new SFFSUBHEADER();
				return false;
			}

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

		public bool GetSubHeader(int index, out SFFSUBHEADER header)
		{
			if ((mSubHeaders == null) || (index < 0) || (index >= mSubHeaders.Count))
			{
				header = new SFFSUBHEADER();
				return false;
			}

			header = mSubHeaders[index];

			if ((header.LenghtOfSubheader == 0) && (header.IndexOfPrevious > 0) && (header.IndexOfPrevious < mSubHeaders.Count))
			{
				header = mSubHeaders[header.IndexOfPrevious - 1];
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

		private bool mIsVaild = false;
		private List<SFFSUBHEADER> mSubHeaders = null;
		// key = group, image
		private Dictionary<KeyValuePair<uint, uint>, KeyValuePair<PCXHEADER, PCXDATA>> mPcxDataMap = new Dictionary<KeyValuePair<uint, uint>, KeyValuePair<PCXHEADER, PCXDATA>>();
	}
}