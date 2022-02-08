using Dapr.Client;

public static class DaprHttpClientExtension
{
    public static IServiceCollection AddDaprSingleton<TClient, TImplementation>(this IServiceCollection services, string appId, string? daprEndpoint = null, string? daprApiToken = null) where TClient : class where TImplementation : class, TClient
    { 
        if (string.IsNullOrEmpty(appId)) throw new ArgumentNullException(nameof(appId));

        services.AddSingleton<TClient, TImplementation>(_ => (TImplementation?)Activator.CreateInstance(typeof(TImplementation), new object[] {DaprClient.CreateInvokeHttpClient(appId, daprEndpoint, daprApiToken)}) ?? throw new InvalidOperationException("can't create an instance of type by passing a Dapr HttpClient isntance: " + typeof(TImplementation).FullName));
        return services; 
    }
}