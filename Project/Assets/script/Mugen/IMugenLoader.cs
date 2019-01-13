namespace Mugen
{
	public interface IMugenLoader
	{
		string LoadText(string fileName);
		byte[] LoadBytes (string fileName);
		void DestroyObject(UnityEngine.Object obj);
	}
}