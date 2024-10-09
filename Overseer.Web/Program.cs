using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Overseer.Web;
using Overseer.Web.Clients;
using Overseer.Web.Services;
using Overseer.Web.Services.Auth;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddTransient<AuthenticationHandler>();

builder.Services
    .AddRefitClient<IUsersClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri($"http://localhost:5000"))
    .AddHttpMessageHandler<AuthenticationHandler>();

builder.Services.AddOptions();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthorizationStateProvider>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, RoleAuthorizationPolicyProvider>();

builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<ToastService>();

await builder.Build().RunAsync();
