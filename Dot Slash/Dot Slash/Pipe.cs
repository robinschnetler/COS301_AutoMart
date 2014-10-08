using System;
using System.Collections.Generic;

namespace Dot_Slash
{
    public class Pipe
    {
        List<Filter> filtersList;
        AdvertDetails advertDetails;
        public Pipe(List<Filter> _filters, AdvertDetails _advertDetails)
        {
            filtersList = _filters;
            advertDetails = _advertDetails;
        }

        public AdvertDetails flow()
        {
		foreach(Filter filter in filtersList)
		{
			try
			{ 
				filter.pump(ref advertDetails);
			}
			catch(Exception e)
			{
				advertDetails.exception = e.Message;
				break;
			}
		}
		return advertDetails;
        }
    }
}
