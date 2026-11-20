namespace NttBankApi.Api.Endpoints.Extensions;

internal static class LocationBuilder
{
    public static Uri ForNamedRoute(
        LinkGenerator linkGenerator,
        HttpContext context,
        string routeName,
        object id)
    {
        ArgumentNullException.ThrowIfNull(linkGenerator);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrWhiteSpace(routeName);
        ArgumentNullException.ThrowIfNull(id);

        string? location = linkGenerator
            .GetUriByName(context, routeName, new { id });

        if (location is null)
        {
            throw new InvalidOperationException(
                $"Route '{routeName}' could not be resolved.");
        }

        return new Uri(location);
    }
}
