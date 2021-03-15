using Microsoft.Extensions.Logging;

namespace BookClub.Data
{
    public class DataEvents
    {
        public static EventId GatMany = new EventId(10001, "GetManyFromProc");
    }
}
