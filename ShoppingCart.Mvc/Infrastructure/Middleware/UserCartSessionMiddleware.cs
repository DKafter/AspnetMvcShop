using Newtonsoft.Json;

namespace ShoppingCart.Mvc.Infrastructure.Middleware;

public class UserCartSessionMiddleware
{
    public static readonly string UserSessionKey = "UserID";
    private readonly RequestDelegate _next;

    public UserCartSessionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sessionValue = context.Session.GetString(UserSessionKey);
        Guid jsonDeserialize =
            JsonConvert.DeserializeObject<Guid>(sessionValue ?? JsonConvert.SerializeObject(Guid.Empty));
        if (jsonDeserialize.Equals(Guid.Empty) ||
            string.IsNullOrEmpty(sessionValue))
        {
            Guid userGuid = Guid.NewGuid();
            string jsonSerialize = JsonConvert.SerializeObject(userGuid);
            context.Session.SetString(UserSessionKey, jsonSerialize);
        }

        await _next(context);
    }
}