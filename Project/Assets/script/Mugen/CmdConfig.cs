using System;

namespace Mugen
{
	public class CmdConfig
	{

		public bool LoadFromFile(string fileName)
		{
			if (string.IsNullOrEmpty (fileName))
				return false;
			string str = AppConfig.GetInstance ().Loader.LoadText (fileName);
			return LoadFromStr (str);
		}

		public bool LoadFromStr(string str)
		{
			if (string.IsNullOrEmpty(str))
				return false;
			ConfigReader reader = new ConfigReader ();
			reader.LoadString (str);
			return LoadFromReader (reader);
		}

		public bool LoadFromReader(ConfigReader reader)
		{
			if (reader == null)
				return false;
			return true;
		}
	}
}

