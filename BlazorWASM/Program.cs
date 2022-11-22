using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorWASM;
using BlazorWASM.Auth;
using BlazorWASM.Auth.HTTP;
using HttpClients.ClientInterfaces;
using HttpClients.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
        new HttpClient { BaseAddress = new Uri("https://localhost:7031") }
); // webapi url

builder.Services.AddScoped<IPostService, PostHttpClient>();
builder.Services.AddScoped<IUserService, UserHttpClient>();
builder.Services.AddScoped<IAuthService, JwtAuthService >();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
builder.Services.AddScoped<IAuthorizationPolicyProvider, DefaultAuthorizationPolicyProvider>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();