using Newtonsoft.Json.Linq;

namespace FinalProject_APIServer.Middelware
{
    public class MiddelWare
    {
        private readonly RequestDelegate _next;

        
        private static readonly List<string> validTokens = new List<string>
    {
        "TokenServer1",
        "TokenServer2"
    };

        public MiddelWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
           
            if (context.Request.Path.StartsWithSegments("http://localhost:5020/Login/"))
            {
                await _next(context);
                return;
            }

            if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
            {
                context.Request.EnableBuffering();
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

             
                var token = GetTokenFromBody(body);

                if (validTokens.Contains(token))
                {
                    await _next(context);
                    return;
                }
            }

           
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }

        
        private string GetTokenFromBody(string body)
        {
            try
            {
                var json = JObject.Parse(body);
                return json["token"]?.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}