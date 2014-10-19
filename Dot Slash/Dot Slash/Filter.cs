namespace Dot_Slash
{
	/// <summary>
	/// An interface for all the filters that classify the image
	/// </summary>
	public interface Filter
	{
		void pump(ref AdvertDetails _advertDetails);
	}
}