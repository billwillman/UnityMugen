
/* MUGEN
SFF文件读取 SFF File Decode
Date:2019-8-9
Author:白绝 https://kfm.ink/
ReadMe：
该源码调用了FreeImage 该库是开源且免费的 在FI.cs中编写了一些预设函数
该源码在CSC.EXE编译方式下编写
该源码使用了指针 CSC.EXE编译参数中需要添加 /unsafe
该源码并没有编写异常处理 传入参数前自行进行有效性检验 或者自行添加异常处理代码
该源码推荐的编译参数 csc.exe sff.cs FI.cs /unsafe
*/

using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FreeImage;
using UnityEngine;

namespace FreeImage
{
	public enum FI_FORMAT
	{
		FIF_UNKNOWN = -1,
		FIF_BMP = 0,
		FIF_ICO = 1,
		FIF_JPEG = 2,
		FIF_JNG = 3,
		FIF_KOALA = 4,
		FIF_LBM = 5,
		FIF_IFF = FIF_LBM,
		FIF_MNG = 6,
		FIF_PBM = 7,
		FIF_PBMRAW = 8,
		FIF_PCD = 9,
		FIF_PCX = 10,
		FIF_PGM = 11,
		FIF_PGMRAW = 12,
		FIF_PNG = 13,
		FIF_PPM = 14,
		FIF_PPMRAW = 15,
		FIF_RAS = 16,
		FIF_TARGA = 17,
		FIF_TIFF = 18,
		FIF_WBMP = 19,
		FIF_PSD = 20,
		FIF_CUT = 21,
		FIF_XBM = 22,
		FIF_XPM = 23,
		FIF_DDS = 24,
		FIF_GIF = 25,
		FIF_HDR = 26
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RGBQUAD
	{
		public byte Blue;
		public byte Green;
		public byte Red;
		public byte Reserved;
	}

	unsafe public class FI
	{
		//创建一个位图
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_Allocate", SetLastError = true)]
		public static extern IntPtr Allocate(int width, int height, int bpp);

		//从内存载入一个位图
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_LoadFromMemory", SetLastError = true)]
		private static extern IntPtr LoadFromMemory(FI_FORMAT fif, int stream, int Flag);

		//创建流
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_OpenMemory", SetLastError = true)]
		public static extern int OpenMemory(int data, int size);

		//关闭流
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_CloseMemory", SetLastError = true)]
		public static extern void CloseMemory(int stream);

		//从流中获取数据
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_AcquireMemory", SetLastError = true)]
		public static extern bool AcquireMemory(int stream, ref int data, ref int size);

		//将位图保存到内存
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_SaveToMemory", SetLastError = true)]
		private static extern bool SaveToMemory(FI_FORMAT fif, IntPtr dib, int stream, int flag);

		//水平翻转图像
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_FlipHorizontal", SetLastError = true)]
		public static extern bool FlipHorizontal(IntPtr dib);

		//垂直翻转图像
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_FlipVertical", SetLastError = true)]
		public static extern bool FlipVertical(IntPtr dib);

		//获取图像宽度
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_GetWidth", SetLastError = true)]
		public static extern int GetWidth(IntPtr Dib);

		//获取图像高度
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_GetHeight", SetLastError = true)]
		public static extern int GetHeight(IntPtr Dib);

		//获取图像色表指针
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_GetPalette", SetLastError = true)]
		public static extern RGBQUAD* GetPalette(IntPtr Dib);

		//获取图像颜色数量
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_GetColorsUsed", SetLastError = true)]
		public static extern int GetColorsUsed(IntPtr Dib);

		//获取图像像素数据指针
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_GetBits", SetLastError = true)]
		public static extern IntPtr GetBits(IntPtr Dib);

		//释放图像
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_Unload", SetLastError = true)]
		public static extern void Free(IntPtr Dib);

		//从像素数据创建
		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_ConvertFromRawBits", SetLastError = true)]
		public static extern IntPtr ConvertFromRawBits(byte[] bits, int width, int height, int pitch, int bpp, int red_mask, int green_mask, int blue_mask, bool topdown);

		public static IntPtr LoadFromMemory(byte[] sprData,FI_FORMAT type)
		{
			//读图像数据并储存在非托管内存
			IntPtr sprPoint = Marshal.AllocHGlobal(sprData.Length);
			Marshal.Copy(sprData,0,sprPoint,sprData.Length);

			//从非托管内存提交给FreeImage
			int sprStream = FI.OpenMemory((int)sprPoint, sprData.Length);
			IntPtr dib = FI.LoadFromMemory(type, sprStream, 0);

			//释放
			Marshal.FreeHGlobal(sprPoint);
			FI.CloseMemory(sprStream);

			return dib;
		}

		public static bool SaveToMemory(IntPtr dib,ref byte[] sprData,FI_FORMAT type)
		{
			//保存图像到流
			int newSprStream = FI.OpenMemory(0,0);
			if(FI.SaveToMemory(type, dib, newSprStream, 0) == false)
			{
				return false;
			}

			//将非托管数据重新转为托管类型
			int newSprPoint = 0;
			int newLen = 0;
			FI.AcquireMemory(newSprStream, ref newSprPoint, ref newLen);
			sprData = new byte[newLen];
			Marshal.Copy((IntPtr)newSprPoint, sprData, 0, newLen);

			//释放
			FI.CloseMemory(newSprStream);

			return true;
		}

	}

}

namespace sff
{
	public class sffMsgV1
	{
		//SFFV1
		public int groupNum;//组的数量
		public int sprNum;//图像的数量
		public int sprOffset;//第一个图像子节点位置
		public int sprHeadSize;//子节点大小
		public byte firstSprPalType;//第一个图像的色表类型
	}
	public class sffMsgV2
	{
		//SFFV2
		public int sprOffset;//第一个图像子节点位置
		public int sprNum;//图像的数量
		public int palOffset;//第一个色表子节点的位置
		public int palNum;//色表的数量
		public int ldataOffset;
		public int ldataLen;
		public int tdataOffset;
		public int tdataLen;
	}
	public class sprMsgV1
	{
		public int pcxDataLen;
		public short x;
		public short y;
		public ushort group;
		public ushort index;
		public ushort linkIndex;
		public byte palType;
		public int offset;

	}
	public class sprMsgV2
	{
		public ushort group;
		public ushort index;
		public ushort width;
		public ushort height;
		public short x;
		public short y;
		public ushort linkIndex;
		public byte fmt;
		public byte depth;
		public int dataOffset;
		public int dataLen;
		public ushort palIndex;
		public ushort flags;

		public void AssignTo(ref sprMsgV2 other)
		{
			if (other == null)
				other = new sprMsgV2 ();
			other.group = group;
			other.index = index;
			other.width = width;
			other.height = height;
			other.x = x;
			other.y = y;
			other.linkIndex = linkIndex;
			other.fmt = fmt;
			other.depth = depth;
			other.dataOffset = dataOffset;
			other.dataLen = dataLen;
			other.palIndex = palIndex;
			other.flags = flags;
		}
	}
	public class palMsg
	{
		public ushort group;
		public ushort index;
		public ushort colorNum;
		public ushort linkIndex;
		public int ldataOffset;
		public int ldataLen;
	}




	class sffV2Decompress
	{
		private struct LZ5_CONTROL_PACKET
		{
			public byte[] flags;
			public void fromStream(BinaryReader br)
			{
				flags = new byte[8];
				byte abyte = br.ReadByte();
				flags[7] = (byte)(((int)abyte & 128) / 128);
				flags[6] = (byte)(((int)abyte & 64) / 64);
				flags[5] = (byte)(((int)abyte & 32) / 32);
				flags[4] = (byte)(((int)abyte & 16) / 16);
				flags[3] = (byte)(((int)abyte & 8) / 8);
				flags[2] = (byte)(((int)abyte & 4) / 4);
				flags[1] = (byte)(((int)abyte & 2) / 2);
				flags[0] = (byte)((int)abyte & 1);
			}
		}

		private struct LZ5_RLE_PACKET
		{
			public byte color;
			public int numtimes;		
			public void fromStream(BinaryReader br)
			{
				byte byte1 = br.ReadByte();
				byte byte2;
				numtimes = ((int)byte1 & 224) >> 5;
				if(numtimes == 0)
				{
					byte2 = br.ReadByte();
					numtimes = (int)byte2;
					numtimes += 8;
				}
				color = (byte)((int)byte1 & 31);
			}	
		}
		private struct LZ5_LZ_PACKET
		{
			public int len;
			public int offset;
			public byte recycled;
			public byte recycled_bits_filled;		
			public void fromStream(BinaryReader br)
			{
				byte byte1 = br.ReadByte();
				byte byte2,byte3,tmp;
				len = (int)byte1 & 63;
				if(len == 0)
				{
					byte2 = br.ReadByte();
					byte3 = br.ReadByte();
					offset = ((int)byte1 & 192) * 4 + (int)byte2 + 1;
					len = (int)byte3 + 3;
				}
				else
				{
					len += 1;
					tmp = (byte)((int)byte1 & 192);
					if(recycled_bits_filled == 2)
					{
						tmp >>= 2;
					}
					if(recycled_bits_filled == 4)
					{
						tmp >>= 4;
					}
					if(recycled_bits_filled == 6)
					{
						tmp >>= 6;
					}
					recycled += tmp;
					recycled_bits_filled += 2;
					if(recycled_bits_filled < 8)
					{
						byte2 = br.ReadByte();
						offset = byte2;
					}
					if(recycled_bits_filled == 8)
					{
						offset = recycled;
						recycled = 0;
						recycled_bits_filled = 0;
					}
					offset += 1;

				}

			}
		}
		public static byte[] unRle8(byte[] src)
		{
			List<byte> ret = new List<byte>();
			MemoryStream ms = new MemoryStream(src);
			BinaryReader br = new BinaryReader(ms);
			byte ch,color;
			int len;
			ms.Seek(0, SeekOrigin.Begin);
			while(ms.Position != ms.Length)
			{
				ch = br.ReadByte();
				if(((int)ch & 192) == 64)
				{
					color = br.ReadByte();
					len = (int)ch & 63;
					for(int i = 0; i < len; i++)
					{
						ret.Add(color);
					}
				}
				else
				{
					ret.Add(ch);
				}
			}
			br.Close();
			ms.Dispose();
			return ret.ToArray();
		}

		public static byte[] unLz5(byte[] src)
		{
			List<byte> ret = new List<byte>();
			MemoryStream ms = new MemoryStream(src);
			BinaryReader br = new BinaryReader(ms);
			LZ5_CONTROL_PACKET ctrl = new LZ5_CONTROL_PACKET();
			LZ5_RLE_PACKET rle = new LZ5_RLE_PACKET();
			LZ5_LZ_PACKET lz = new LZ5_LZ_PACKET();
			List<byte> tmp = new List<Byte>();
			ms.Seek(0, SeekOrigin.Begin);
			while(ms.Position != ms.Length)
			{
				ctrl.fromStream(br);
				for(int i = 0; i < 8; i++)
				{
					if(ms.Position == ms.Length)
					{
						break;
					}
					if(ctrl.flags[i] == 0)
					{
						rle.fromStream(br);
						for(int n = 0; n < rle.numtimes; n++)
						{
							ret.Add(rle.color);
						}
					}
					else if(ctrl.flags[i] == 1)
					{
						lz.fromStream(br);
						tmp.Clear();
						for(int n = 0; n < lz.len; n++)
						{
							int offset = ret.Count - lz.offset + n;
							if(offset < ret.Count)
							{
								tmp.Add(ret[offset]);
							}
							else
							{
								break;
							}
						}
						if(tmp.Count < lz.len && tmp.Count != 0)
						{
							int count = tmp.Count;
							int len = (lz.len - count) / count;
							List<byte> tmp2 = tmp.GetRange(0, tmp.Count);
							for(int n = 0; n < len; n++)
							{
								tmp.AddRange(tmp2);
							}
							tmp.AddRange(tmp2.GetRange(0, (lz.len - count) % count));
						}
						ret.AddRange(tmp);
					}

				}

			}
			br.Close();
			ms.Close();
			return ret.ToArray();
		}

	}

	public unsafe class sffReader
	{
		private FileStream fs;
		private MemoryStream ms;
		private BinaryReader br;
		private sffMsgV1 msgV1 = new sffMsgV1();
		private sffMsgV2 msgV2 = new sffMsgV2();
		//直接从外部访问ver变量获取版本号
		public int ver; 

		//以文件首为参考点 移动读写位置
		private void seek(int offset)
		{
			if(ms != null)
			{
				ms.Seek(offset,SeekOrigin.Begin);
			}
			else
			{
				fs.Seek(offset,SeekOrigin.Begin);
			}
		}

		//自定义参考点 移动读写位置
		private void seek(int offset, SeekOrigin orgin)
		{
			if(ms != null)
			{
				ms.Seek(offset,orgin);
			}
			else
			{
				fs.Seek(offset,orgin);
			}
		}

		//构造器
		public sffReader(string fn)
		{
			//从文件读取
			fs = new FileStream(fn,FileMode.Open);
			br = new BinaryReader(fs);
			ver = getVer();
			getSffMsg();
		}
		public sffReader(byte[] data)
		{
			//从内存读取
			ms = new MemoryStream(data);
			br = new BinaryReader(ms);
			ver = getVer();
			getSffMsg();
		}

		//获取版本号 1或者2
		private int getVer()
		{

			seek(15);
			return (int)br.ReadByte();
		}

		//获取SFF文件信息
		private void getSffMsg()
		{
			if(ver == 1)
			{
				seek(16);
				msgV1.groupNum = br.ReadInt32();
				msgV1.sprNum = br.ReadInt32();
				msgV1.sprOffset = br.ReadInt32();
				msgV1.sprHeadSize = br.ReadInt32();//32
				msgV1.firstSprPalType = br.ReadByte();//0

			}
			else if(ver == 2)
			{
				seek(36);
				msgV2.sprOffset = br.ReadInt32();
				msgV2.sprNum = br.ReadInt32();
				msgV2.palOffset = br.ReadInt32();
				msgV2.palNum = br.ReadInt32();
				msgV2.ldataOffset = br.ReadInt32();
				msgV2.ldataLen = br.ReadInt32();
				msgV2.tdataOffset = br.ReadInt32();
				msgV2.tdataLen = br.ReadInt32();

			}
		}

		//SFFV1 获取指定图像信息
		public sprMsgV1 getSprMsgV1(int index)
		{
			int offset = msgV1.sprOffset;
			sprMsgV1 spr = new sprMsgV1();
			for(int i = 0; i < msgV1.sprNum ; i++)
			{
				seek(offset);
				if(i == index)
				{
					spr.offset = br.ReadInt32();
					spr.pcxDataLen = br.ReadInt32();
					spr.x = br.ReadInt16();
					spr.y = br.ReadInt16();
					spr.group = br.ReadUInt16();
					spr.index = br.ReadUInt16();
					spr.linkIndex = br.ReadUInt16();
					spr.palType = br.ReadByte();
					offset = spr.offset;
				}
				else
				{
					offset = br.ReadInt32();
				}
			}
			return spr;
		}

		//SFFV2 获取指定图像信息
		public sprMsgV2 getSprMsgV2(int index, sprMsgV2 sp = null)
		{
			sprMsgV2 spr;
			if (sp == null)
				spr = new sprMsgV2 ();
			else
				spr = sp;
			seek(msgV2.sprOffset + 28 * index);
			spr.group = br.ReadUInt16();
			spr.index = br.ReadUInt16();
			spr.width = br.ReadUInt16();
			spr.height = br.ReadUInt16();
			spr.x = br.ReadInt16();
			spr.y = br.ReadInt16();
			spr.linkIndex = br.ReadUInt16();
			spr.fmt = br.ReadByte();
			spr.depth = br.ReadByte();
			spr.dataOffset = br.ReadInt32();
			spr.dataLen = br.ReadInt32();
			spr.palIndex = br.ReadUInt16();
			spr.flags = br.ReadUInt16();
			return spr;
		}

		//SFFV1 从索引获取指定图像数据
		public byte[] getSprDataV1(int index, out byte[] pp, FI_FORMAT type)
		{
			int offset = msgV1.sprOffset;
			sprMsgV1 spr = new sprMsgV1();
			byte[] facePalData = new byte[0];
			byte[] palData = new byte[0];
			byte[] sprData = new byte[0];
			for(int i = 0; i < msgV1.sprNum; i++)
			{
				seek(offset);
				offset = br.ReadInt32();
				spr.pcxDataLen = br.ReadInt32();
				spr.x = br.ReadInt16();
				spr.y = br.ReadInt16();
				spr.group = br.ReadUInt16();
				spr.index = br.ReadUInt16();
				spr.linkIndex = br.ReadUInt16();
				spr.palType = br.ReadByte();
				if(i == 0)
				{
					//第一张图强制为独立色表
					spr.palType = 0;
				}
				seek(13,SeekOrigin.Current);
				if((spr.group == 0 && spr.index ==0 || spr.group == 9000 && spr.index == 0) && facePalData.Length != 0)
				{
					//0,0和9000,0强制为独立色表，且使用第一张色表
					palData = facePalData;
				}
				if(i == index)
				{
					if(spr.pcxDataLen == 0)
					{
						//链接型图像
						return getSprDataV1(spr.linkIndex, out pp, type);
					}

					sprData = br.ReadBytes(spr.pcxDataLen);
					if (type != FI_FORMAT.FIF_UNKNOWN)
					{
						IntPtr dib = FI.LoadFromMemory(sprData, FI_FORMAT.FIF_PCX);
						if (spr.palType == 1)
						{
							RGBQUAD* pal = FI.GetPalette(dib);
							for (int n = 0; n < 256; n++)
							{
								pal[n].Red = palData[n * 3];
								pal[n].Green = palData[n * 3 + 1];
								pal[n].Blue = palData[n * 3 + 2];
							}
						}
						FI.SaveToMemory(dib, ref sprData, type);


						//释放图像流
						FI.Free(dib);
					}

					pp = palData;
					return sprData;
				}
				else
				{
					if (spr.palType == 0)
					{
						//读取PCX尾部色表数据
						seek(spr.pcxDataLen - 768, SeekOrigin.Current);
						palData = br.ReadBytes(768);
						if(i == 0)
						{
							//保留第一张图的色表
							facePalData = palData;
						}

					}
				}
			}

			pp = null;
			return null;
		}

		//SFFV1 从组和编号获取图像数据
		public byte[] getSprDataV1(int group, int index, FI_FORMAT type)
		{
			int offset = msgV1.sprOffset;
			for(int i = 0; i < msgV1.sprNum; i++)
			{
				seek(offset);
				offset = br.ReadInt32();
				seek(8, SeekOrigin.Current);
				if (br.ReadUInt16() == group && br.ReadUInt16() == index)
				{
					//转移事件
					byte[] pal;
					return getSprDataV1(i, out pal, type);
				}

			}
			return null;
		}

		//SFFV2 获取色表信息
		public palMsg getPalMsg(int index)
		{
			palMsg pal = new palMsg();
			seek(44);
			int palOffset = br.ReadInt32();
			seek(palOffset + index * 16);
			pal.group = br.ReadUInt16();
			pal.index = br.ReadUInt16();
			pal.colorNum = br.ReadUInt16();
			pal.linkIndex = br.ReadUInt16();
			pal.ldataOffset = br.ReadInt32();
			pal.ldataLen = br.ReadInt32();
			return pal;
		}


		//SFFV2 获取色表数据
		private byte[] getPalData(int index)
		{
			palMsg pal = getPalMsg(index);
			seek(msgV2.ldataOffset + pal.ldataOffset);
			return br.ReadBytes(pal.ldataLen);
		}

		//SFFV2 从索引获取图像数据
		public byte[] getSprDataV2(int index, out byte[] pp, FI_FORMAT type, bool loadPal = true)
		{
			byte[] sprData;
			byte[] palData = null;
			int sprSize;
			IntPtr dib;
			sprMsgV2 spr = getSprMsgV2(index);
			if(spr.dataLen == 0)
			{
				//链接型图像
				return getSprDataV2(spr.linkIndex, out pp, type);
			}
			seek((spr.flags == 1 ? msgV2.tdataOffset : msgV2.ldataOffset) + spr.dataOffset);

			//压缩算法(fmt)常量声明这里省略了，直接使用值
			//0 无压缩
			//2 Rle8压缩
			//3 Rle5压缩 几乎不会用到 直接省略
			//4 Lz5压缩
			//10 PNG8
			//11 PNG24
			//12 PNG32
			if(spr.fmt == 0)
			{
				sprData = br.ReadBytes(spr.dataLen);
			}
			else
			{
				sprSize = br.ReadInt32();//解压后的校验长度，这里省略了校验
				sprData = br.ReadBytes(spr.dataLen - 4);
			}
			switch(spr.fmt)
			{
			case 0:
				if (loadPal)
					palData = getPalData(spr.palIndex);
				pp = palData;
				break;

			case 2:
				sprData = sffV2Decompress.unRle8(sprData);
				if (loadPal)
					palData = getPalData(spr.palIndex);
				pp = palData;
				break;

			case 4:
				sprData = sffV2Decompress.unLz5(sprData);
				if (loadPal)
					palData = getPalData(spr.palIndex);
				pp = palData;
				break;

			case 10:
				//压缩算法为PNG算法时读取的数据默认都是一个完整的PNG文件 可以直接载入
				palData = getPalData (spr.palIndex);
				dib = FI.LoadFromMemory (sprData, FI_FORMAT.FIF_PNG);
				//PNG8调色板校正
				RGBQUAD* pale = FI.GetPalette (dib);
				for (int n = 0; n < 256; n++) {
					pale [n].Red = palData [n * 4];
					pale [n].Green = palData [n * 4 + 1];
					pale [n].Blue = palData [n * 4 + 2];
				}
				FI.SaveToMemory (dib, ref sprData, type);
				pp = palData;
				return sprData;

			case 11:
			case 12:
				pp = null;
				return sprData;

			default:	
				sprData = new byte[0];
				pp = new byte[0];
				return sprData;
			}

			if (type != FI_FORMAT.FIF_UNKNOWN) {
				//对于无压缩、Rle8和Lz5压缩的图像读取的数据是原始的像素数据 不能直接载入
				dib = FI.ConvertFromRawBits (sprData, spr.width, spr.height, spr.width, 8, 0, 0, 0, true);
				RGBQUAD* pal = FI.GetPalette (dib);
				int colorNum = palData.Length / 4;
				for (int n = 0; n < colorNum; n++) {
					pal [n].Red = palData [n * 4];
					pal [n].Green = palData [n * 4 + 1];
					pal [n].Blue = palData [n * 4 + 2];
				}
				FI.SaveToMemory (dib, ref sprData, type);
				FI.Free (dib);
				return sprData;
			} else {
				return sprData;
			}
		}

		public delegate void TOnRawForeachV2(sffReader reader, sprMsgV2 spr, int linkGoup, int linkIndex, int linkPalGroup, int linkPalIndex, byte[] rawData);
		public delegate void TOnRawForeachV1(sprMsgV1 spr, int linkGoup, int linkIndex, byte[] rawData, byte[] pal);

		public bool RawForeachV1(TOnRawForeachV1 OnRawForeachV1)
		{
			if (ver != 1 || OnRawForeachV1 == null)
				return false;

			for (int i = 0; i < getSprNum(); ++i)
			{
				sprMsgV1 spr = getSprMsgV1(i);
				if (spr.pcxDataLen == 0)
				{
					sprMsgV1 linkSpr = spr;
					while (linkSpr.pcxDataLen == 0)
					{
						linkSpr = getSprMsgV1(linkSpr.linkIndex);
					}

					OnRawForeachV1(spr, linkSpr.group, linkSpr.index, null, null);
				} else
				{
					byte[] pal;
					byte[] colors = getSprDataV1(i, out pal, FI_FORMAT.FIF_UNKNOWN);
					if (colors == null || colors.Length <= 0)
						return false;
					OnRawForeachV1(spr, -1, -1, colors, pal);
				}
			}

			return true;
		}


		private Dictionary<KeyValuePair<ushort, ushort>, byte[]> m_PalMap = null;
		protected Dictionary<KeyValuePair<ushort, ushort>, byte[]> PalMap
		{
			get {
				if (m_PalMap == null)
					m_PalMap = new Dictionary<KeyValuePair<ushort, ushort>, byte[]> ();
				return m_PalMap;
			}
		}

		public byte[] GetPal(ushort group, ushort index)
		{
			if (m_PalMap == null)
				return null;
			KeyValuePair<ushort, ushort> key = new KeyValuePair<ushort, ushort> (group, index);
			byte[] ret;
			if (m_PalMap.TryGetValue (key, out ret))
				return ret;
			return  null;
		}

		private bool IsHasPalMap(ushort group, ushort index)
		{
			if (m_PalMap == null)
				return false;
			KeyValuePair<ushort, ushort> key = new KeyValuePair<ushort, ushort> (group, index);
			return m_PalMap.ContainsKey (key);
		}
		public bool RawForeachV2(TOnRawForeachV2 OnRawForeachV2)
		{
			if (ver != 2 || OnRawForeachV2 == null)
				return false;
			sprMsgV2 spr = null;
			sprMsgV2 linkSpr = null;
			for (int i = 0; i < getSprNum (); ++i) {
				spr = getSprMsgV2 (i, spr);
				if (spr.dataLen == 0) {
					// 链接类型
					spr.AssignTo(ref linkSpr);
					while (linkSpr.dataLen == 0) {
						linkSpr = getSprMsgV2 (linkSpr.linkIndex, linkSpr);
					}
					// 暂时不做PNG类型的支持
					if (linkSpr.fmt == 10 || linkSpr.fmt == 11 || linkSpr.fmt == 12)
						return false;
					OnRawForeachV2 (this, spr, linkSpr.group, linkSpr.index, -1, -1, null); 
				} else {
					// 暂时不做PNG类型的支持
					if (spr.fmt == 10 || spr.fmt == 11 || spr.fmt == 12)
						return false;

					spr.AssignTo(ref linkSpr);
					while (true)
					{
						int palGroup = linkSpr.group;
						int palIndex = linkSpr.index;
						linkSpr = getSprMsgV2 (linkSpr.palIndex, linkSpr);
						if (linkSpr.group == palGroup && linkSpr.index == palIndex)
							break;
					}

					byte[] pal;
					bool isHasPal = IsHasPalMap (linkSpr.group, linkSpr.index);

					byte[] colors = getSprDataV2 (i, out pal, FI_FORMAT.FIF_UNKNOWN, !isHasPal); 
					if (colors == null || colors.Length <= 0)
						return false;
					

					if (!isHasPal && pal != null && pal.Length > 0) {
						KeyValuePair<ushort, ushort> key = new KeyValuePair<ushort, ushort> (linkSpr.group, linkSpr.index);
						PalMap.Add (key, pal);
					}

					int linkPalGroup;
					int linkPalIndex;
					if (linkSpr.group == spr.group && linkSpr.index == spr.index) {
						linkPalGroup = -1;
						linkPalIndex = -1;
					} else
					{
						linkPalGroup = linkSpr.group;
						linkPalIndex = linkSpr.index;
					}
					OnRawForeachV2 (this, spr, -1, -1, linkPalGroup, linkPalIndex, colors);
				}
			}
			return true;
		}


		//SFFV2 从组和编码获取图像数据
		public byte[] getSprDataV2(int group, int index, FI_FORMAT type)
		{
			sprMsgV2 spr;
			byte[] pal;
			for(int i = 0; i < msgV2.sprNum; i++)
			{
				spr = getSprMsgV2(i);
				if(spr.group == group && spr.index == index)
				{
					//转移事件
					return getSprDataV2(i, out pal, type);
				}
			}

			return null;

		}

		//获取图像数量
		public int getSprNum()
		{
			if(ver == 1)
			{
				return msgV1.sprNum;
			}
			else if(ver == 2)
			{
				return msgV2.sprNum;
			}
			return -1;

		}

	}


	class test
	{
		//此处仅供测试
		static void Main(string[] args)
		{
			//示例：输出所有图像到当前目录的out文件夹
			sffReader sf = new sffReader("kfm.sff");
			for(int i = 0; i < sf.getSprNum(); i++)
			{
				byte[] pal;
				byte[] spr = sf.getSprDataV1(i, out pal, FI_FORMAT.FIF_PNG);
				FileStream fs = new FileStream(@"out\" + i.ToString() + ".png",FileMode.Create);
				BinaryWriter bw = new BinaryWriter(fs);
				bw.Write(spr);
				bw.Close();
				fs.Close();
			}
			Console.ReadLine();//等待输入
		}
	}
}