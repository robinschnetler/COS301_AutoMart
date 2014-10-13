namespace Dot_Slash
{
    /// <summary>
    /// Interfaace for different filters used in analysing advert images.
    /// </summary>
	public interface Filter
	{
        /// <summary>
        /// Method used to perform tests on AdvertDetails object.
        /// </summary>
        /// <param name="_advertDetails"></param>AdvertDetails object containg all the information about the advert image.
		void pump(ref AdvertDetails _advertDetails);
	}
}

