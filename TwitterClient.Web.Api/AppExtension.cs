using Microsoft.Extensions.Options;
using TwitterClient.Application;

namespace TwitterClient.WebApi;

public static class AppExtension
{
    public static void SubscribeToTwitterStream(this IApplicationBuilder app)
    {
        var streamService = app.ApplicationServices.GetRequiredService<ITweetStreamService>();
        var settings = app.ApplicationServices.GetRequiredService<IOptions<AppSettings>>();
        streamService.StartStreaming(settings.Value.TwitterApi.BearerToken);
    }
}
