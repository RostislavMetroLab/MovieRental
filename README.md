# MovieRental Exercise

> The app is throwing an error when we start, please help us. Also, tell us what caused the issue.

The error occurs because you cannot inject a Scoped service into a Singleton service directly because the Scoped service is created per request, but Singleton is created once for the application's lifetime.


> The rental class has a method to save, but it is not async, can you make it async and explain to us what is the difference?

The thread calling SaveChanges() is blocked until the database operation completes. This can make your application less responsive, especially under high load or slow database responses.
The thread calling SaveChangesAsync() can continue executing other work or return to the thread pool while waiting for the database operation to complete.


> No exceptions are being caught in this api, how would you deal with these exceptions?

For catching unhandled exceptions at the application boundary, I would log them, and return a consistent error response to the client. Something like this:


```
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
            await context.Response.WriteAsync(result);
        }
    }
}
```