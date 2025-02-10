using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Abp.Runtime.Session;
using Abp.Extensions;
using Abp.Authorization;
using System.Security.Claims;
using System;
using Abp.Runtime.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthAttribute : Attribute, IAsyncAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private readonly string _expectedApiKey = "matrix-5dd9ae979c7d440e9e5602b25e08f65d";

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        await Task.CompletedTask;
        var request = context.HttpContext.Request;

        // 1) Check if the header is present and matches
        if (!request.Headers.TryGetValue(ApiKeyHeaderName, out var providedKey) ||
            !providedKey.Equals(_expectedApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // 2) If it matches, set up claims for user/tenant
        //    Let's say you want TenantId=2 and UserId=3:
        var claims = new List<Claim>
        {
            new Claim(AbpClaimTypes.TenantId, "2"),
            new Claim(AbpClaimTypes.UserId,   "3")
        };

        // 3) Create an identity & principal
        var identity = new ClaimsIdentity(claims, authenticationType: "ApiKey");
        var principal = new ClaimsPrincipal(identity);

        // 4) Overwrite HttpContext.User
        context.HttpContext.User = principal;

        // Done! ABP’s IAbpSession should now see TenantId=2, UserId=3 
        // because it reads from HttpContext.User.Claims by default.
        // You may need to ensure that your 'ICurrentPrincipalAccessor' 
        // syncs with HttpContext.User if you have a custom config.
    }
}
