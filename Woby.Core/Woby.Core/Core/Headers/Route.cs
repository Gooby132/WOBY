using FluentResults;

namespace Woby.Core.Core.Headers
{
    /// <summary>
    /// Class representing a route from / to / between (proxy) 
    /// thus storing the direction intended:
    ///     * from - recepient that sent message
    ///     * to - recepient that meant to receive the message
    ///     * proxy - additional header representing where the message came from
    /// </summary>
    public class Route : HeaderBase
    {

        public Uri Uri { get; }
        public RouteIntake Direction { get; }

        public Route(Uri uri, RouteIntake direction, string key, string body): base(key, body, HeaderTypes.Route)
        {
            Uri = uri;
            Direction = direction;
        }
    }
}
