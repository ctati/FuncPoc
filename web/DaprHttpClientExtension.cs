using Dapr.Client;

public static class DaprHttpClientExtension
{
    public static IServiceCollection AddDaprHttpClient<TClient, TImplementation>(this IServiceCollection services, string appId = null, string daprEndpoint = null, string daprApiToken = null)
    { 
/*
        services.AddHttpClient<TClient, TImplementation>((client) => { 
            // ConfigureDapr(client, appId);
            if (appId is string)
            {
                try
                {
                    client.BaseAddress = new Uri($"http://{appId}");
                }
                catch (UriFormatException inner)
                {
                    throw new ArgumentException("The appId must be a valid hostname.", nameof(appId), inner);
                }
            }
        })
            .AddDaprMessageHandler(daprEndpoint, daprApiToken);
*/
        return services; 
    }

    private static IHttpClientBuilder AddDaprMessageHandler(this IHttpClientBuilder builder, string daprEndpoint = null, string daprApiToken = null)
    { 
        builder.AddHttpMessageHandler((provider) => {
            var handler = new InvocationHandler()
                {
                    InnerHandler = new HttpClientHandler(),
                    //DaprApiToken = daprApiToken
                };

            if (daprEndpoint is string)
            {
                // DaprEndpoint performs validation.
                handler.DaprEndpoint = daprEndpoint;
            }
            return handler;
        });

        return builder;
    }   

    private static void ConfigureDapr(HttpClient httpClient, string appId = null)
    {
        if (appId is string)
        {
            try
            {
                httpClient.BaseAddress = new Uri($"http://{appId}");
            }
            catch (UriFormatException inner)
            {
                throw new ArgumentException("The appId must be a valid hostname.", nameof(appId), inner);
            }
        }
    }
}