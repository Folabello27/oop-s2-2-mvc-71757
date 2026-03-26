using Serilog.Core;
using Serilog.Events;

namespace oop_s2_2_mvc_71757.Logging;

public sealed class UserNameEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserNameEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var userName = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true
            ? _httpContextAccessor.HttpContext?.User?.Identity?.Name
            : "Anonymous";

        var property = propertyFactory.CreateProperty("UserName", userName ?? "Anonymous");
        logEvent.AddOrUpdateProperty(property);
    }
}
