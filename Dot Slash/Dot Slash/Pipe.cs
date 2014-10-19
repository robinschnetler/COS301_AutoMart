using System;
using System.Collections.Generic;

namespace Dot_Slash
{
	/// <summary>
	/// This class acts as the Pipe object in the Pipe and Filters architecture.
	/// </summary>
	public class Pipe
	{
		/// <summary>
		/// The list of all the filters that this pipe must execute
		/// </summary>
		List<Filter> filtersList;
		/// <summary>
		/// The advertDetails object that is filled with relevant information as filters are executed
		/// </summary>
		AdvertDetails advertDetails;

		/// <summary>
		/// The constructor that creates the Pipe object
		/// </summary>
		/// <param name="_filters">A list of all the filters that this pipe must execute in their relevant order</param>
		/// <param name="_advertDetails">The advertDetails object (which initially has no information) which is subsequently filled with information as each filter gets executed</param>
		public Pipe(List<Filter> _filters, AdvertDetails _advertDetails)
		{
			filtersList = _filters;
			advertDetails = _advertDetails;
		}

		/// <summary>
		/// This method runs the advertDetails object through all of the filters and enters information into the advertDetails object as each filter is executed
		/// </summary>
		/// <returns>The advertDetails object with all the details of the image recorded</returns>
		public AdvertDetails flow()
		{
			foreach(Filter filter in filtersList)
			{
				filter.pump(ref advertDetails);
				//try
				//{ 
				//	filter.pump(ref advertDetails);
				//}
				//catch(Exception e)
				//{
				//	advertDetails.exception = e.Message;
				//	break;
				//}
			}
			return advertDetails;
		}
	}
}
