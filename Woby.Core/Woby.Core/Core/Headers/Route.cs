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
        public string? DisplayName { get; }
        public RouteIntake Direction { get; }

        public Route(Uri uri, RouteIntake direction, string? displayName, string key, string body): base(key, body, HeaderType.Route)
        {
            Uri = uri;
            Direction = direction;
            DisplayName = displayName;
        }
    }
}
