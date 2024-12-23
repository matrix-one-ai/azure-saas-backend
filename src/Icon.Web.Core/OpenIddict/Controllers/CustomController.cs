// using System;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Abp.AspNetCore.OpenIddict.Claims;
// using Abp.AspNetCore.OpenIddict.Controllers;
// using Abp.Authorization;
// using Abp.Authorization.Users;
// using Icon.Authorization.Roles;
// using Icon.Authorization.Users;
// using Icon.MultiTenancy;
// using Microsoft.AspNetCore;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;
// using OpenIddict.Abstractions;
// using OpenIddict.Server.AspNetCore;
// using static OpenIddict.Abstractions.OpenIddictConstants;

// namespace Icon.Web.OpenIddict.Controllers
// {
//     public partial class TokenController : TokenController<Tenant, Role, User>
//     {
//         private readonly IOpenIddictApplicationManager _applicationManager;

//         public TokenController(
//             AbpSignInManager<Tenant, Role, User> signInManager,
//             AbpUserManager<Role, User> userManager,
//             IOpenIddictApplicationManager applicationManager,
//             IOpenIddictAuthorizationManager authorizationManager,
//             IOpenIddictScopeManager scopeManager,
//             IOpenIddictTokenManager tokenManager,
//             AbpOpenIddictClaimsPrincipalManager openIddictClaimsPrincipalManager)
//             : base(
//                 signInManager,
//                 userManager,
//                 applicationManager,
//                 authorizationManager,
//                 scopeManager,
//                 tokenManager,
//                 openIddictClaimsPrincipalManager)
//         {
//             _applicationManager = applicationManager;
//         }

//         [HttpPost("~/connect/token")]
//         public override async Task<IActionResult> HandleAsync()
//         {
//             var request = HttpContext.GetOpenIddictServerRequest();

//             if (request == null)
//             {
//                 throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");
//             }

//             if (request.IsClientCredentialsGrantType())
//             {
//                 // Handle client_credentials grant type
//                 return await HandleClientCredentialsGrantAsync(request);
//             }

//             // For other grant types, fall back to base implementation
//             return await base.HandleAsync();
//         }

//         private async Task<IActionResult> HandleClientCredentialsGrantAsync(OpenIddictRequest request)
//         {
//             // Validate the client application
//             var application = await _applicationManager.FindByClientIdAsync(request.ClientId);
//             if (application == null)
//             {
//                 return BadRequest(new OpenIddictResponse
//                 {
//                     Error = Errors.InvalidClient,
//                     ErrorDescription = "The client application was not found."
//                 });
//             }

//             // Validate the client secret
//             if (!await _applicationManager.ValidateClientSecretAsync(application, request.ClientSecret))
//             {
//                 return BadRequest(new OpenIddictResponse
//                 {
//                     Error = Errors.InvalidClient,
//                     ErrorDescription = "The client credentials are invalid."
//                 });
//             }

//             // Create a new ClaimsIdentity
//             var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType);

//             // Add claims
//             identity.AddClaim(Claims.Subject, request.ClientId, Destinations.AccessToken);
//             identity.AddClaim(Claims.Name, (await _applicationManager.GetDisplayNameAsync(application)) ?? request.ClientId, Destinations.AccessToken);

//             // Create a ClaimsPrincipal
//             var principal = new ClaimsPrincipal(identity);

//             // Set scopes and resources
//             principal.SetScopes(request.GetScopes());
//             principal.SetResources("default-api"); // Replace with your API resource(s)

//             // Set claim destinations
//             foreach (var claim in principal.Claims)
//             {
//                 claim.SetDestinations(Destinations.AccessToken);
//             }

//             // Sign in the client application
//             return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
//         }
//     }
// }
