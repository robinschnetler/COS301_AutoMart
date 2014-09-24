using System.Collections.Generic;

namespace Dot_Slash
{
    class Pipe
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
                filter.pump(ref advertDetails);
            }
            return advertDetails;
        }
    }
}
