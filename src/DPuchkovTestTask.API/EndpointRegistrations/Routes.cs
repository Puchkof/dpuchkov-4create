namespace DPuchkovTestTask.API.EndpointRegistrations;

public static class Routes
{
    public static string Root => "api/v{version:apiVersion}";

    public static string Files => $"{Root}/files";
    
    public static string UploadFile => $"{Files}/uploads";
    
    public static string Trials => $"{Root}/trials";

    public static string GetVersionedRoute(string route, int version) => 
        route.Replace("{version:apiVersion}",version.ToString());
}