using Microsoft.AspNetCore.Mvc;

namespace rest_with_asp_net10_ericles.Hypermedia.Utils;

public static class URLHelper
{
    private static readonly object _lock = new object();
    public static string BuildBaseURL(this IUrlHelper urlHelper, string routeName, string path)
    {
        lock (_lock)
        {
            var url = urlHelper.Link(routeName, new { controller = path }) ?? string.Empty;
            if (url == null)
                return string.Empty;
            url = url.Replace("%2F", "/").TrimEnd('/');
            return url;
        }
    }
}
