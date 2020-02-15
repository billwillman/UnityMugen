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
/*
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FreeImage;
namespace sff
{
	class sffMsgV1
	{
		//SFFV1
		public int groupNum;//组的数量
		public int sprNum;//图像的数量
		public int sprOffset;//第一个图像子节点位置
		public int sprHeadSize;//子节点大小
		public byte firstSprPalType;//第一个图像的色表类型
	}
	class sffMsgV2
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
	class sprMsgV1
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
	class sprMsgV2
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
	}
	class palMsg
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
	unsafe class sffReader
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

		//销毁
		public void close()
		{
			br.Close();
			ms.Close();
			fs.Close();
			ver = -1;


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
		public sprMsgV2 getSprMsgV2(int index)
		{
			sprMsgV2 spr = new sprMsgV2();
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
		public byte[] getSprDataV1(int index, FI_FORMAT type)
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
						return getSprDataV1(spr.linkIndex,type);
					}

					sprData = br.ReadBytes(spr.pcxDataLen);
					IntPtr dib = FI.LoadFromMemory(sprData, FI_FORMAT.FIF_PCX);
					if (spr.palType == 1)
					{
						RGBQUAD *pal = FI.GetPalette(dib);
						for(int n = 0; n < 256; n++)
						{
							pal[n].Red = palData[n * 3];
							pal[n].Green = palData[n * 3 + 1];
							pal[n].Blue = palData[n * 3 + 2];
						}
					}
					FI.SaveToMemory(dib, ref sprData, type);

					//释放图像流
					FI.Free(dib);
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
					return getSprDataV1(i, type);
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
		public byte[] getSprDataV2(int index, FI_FORMAT type)
		{
			byte[] sprData,palData;
			int sprSize;
			IntPtr dib;
			sprMsgV2 spr = getSprMsgV2(index);
			if(spr.dataLen == 0)
			{
				//链接型图像
				return getSprDataV2(spr.linkIndex, type);
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
				palData = getPalData(spr.palIndex);
				break;

			case 2:
				sprData = sffV2Decompress.unRle8(sprData);
				palData = getPalData(spr.palIndex);
				break;

			case 4:
				sprData = sffV2Decompress.unLz5(sprData);
				palData = getPalData(spr.palIndex);
				break;

			case 10:
				//压缩算法为PNG算法时读取的数据默认都是一个完整的PNG文件 可以直接载入
				palData = getPalData(spr.palIndex);
				dib = FI.LoadFromMemory(sprData, FI_FORMAT.FIF_PNG);
				//PNG8调色板校正
				RGBQUAD *pale = FI.GetPalette(dib);
				for(int n = 0; n < 256; n++)
				{
					pale[n].Red = palData[n * 4];
					pale[n].Green = palData[n * 4 + 1];
					pale[n].Blue = palData[n * 4 + 2];
				}
				FI.SaveToMemory(dib, ref sprData, type);
				return sprData;

			case 11:
			case 12:
				return sprData;

			default:	
				sprData = new byte[0];
				return sprData;
			}
			//对于无压缩、Rle8和Lz5压缩的图像读取的数据是原始的像素数据 不能直接载入
			dib = FI.ConvertFromRawBits(sprData, spr.width, spr.height, spr.width, 8, 0, 0, 0, true);
			RGBQUAD *pal = FI.GetPalette(dib);
			int colorNum = palData.Length / 4;
			for(int n = 0; n < colorNum; n++)
			{
				pal[n].Red = palData[n * 4];
				pal[n].Green = palData[n * 4 + 1];
				pal[n].Blue = palData[n * 4 + 2];
			}
			FI.SaveToMemory(dib, ref sprData, type);
			FI.Free(dib);
			return sprData;
		}


		//SFFV2 从组和编码获取图像数据
		public byte[] getSprDataV2(int group, int index, FI_FORMAT type)
		{
			sprMsgV2 spr;
			for(int i = 0; i < msgV2.sprNum; i++)
			{
				spr = getSprMsgV2(i);
				if(spr.group == group && spr.index == index)
				{
					//转移事件
					return getSprDataV2(i,type);
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
				byte[] spr = sf.getSprDataV1(i, FI_FORMAT.FIF_PNG);
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
*/