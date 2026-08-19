using System.Text;

namespace UserManagementApi.Middleware;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;

    public BasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Missing Authorization Header");
            return;
        }

        try
        {
            var authHeader =
                context.Request.Headers["Authorization"].ToString();

            if (!authHeader.StartsWith("Basic "))
            {
                throw new Exception("Invalid authentication scheme.");
            }

            var encodedCredentials =
                authHeader["Basic ".Length..].Trim();

            var credentials =
                Encoding.UTF8.GetString(
                    Convert.FromBase64String(encodedCredentials));

            var values = credentials.Split(':');

            var username = values[0];
            var password = values[1];

            // Demo credentials
            if (username != "admin" || password != "password")
            {
                context.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsync("Invalid credentials");
                return;
            }

            await _next(context);
        }
        catch
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsync("Authentication failed");
        }
    }
}