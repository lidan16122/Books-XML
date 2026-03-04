using System.Text;

namespace server.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"[Request] {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");

            if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
            {
                context.Request.EnableBuffering(); 
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                string body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; 
                if (!string.IsNullOrWhiteSpace(body))
                {
                    Console.WriteLine($"[Request Body] {body}");
                }
            }

            await _next(context);

            Console.WriteLine($"[Response] Status Code: {context.Response.StatusCode}");
        }
    }
}
