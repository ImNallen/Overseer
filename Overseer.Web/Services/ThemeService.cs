using Microsoft.JSInterop;

namespace Overseer.Web.Services;

public class ThemeService
{
    private readonly IJSRuntime _jsRuntime;

    public ThemeService(IJSRuntime jsRuntime) => _jsRuntime = jsRuntime;

#pragma warning disable CA1003 // Use generic event handler instances
    public event Action? OnThemeChanged;
#pragma warning restore CA1003 // Use generic event handler instances

    public async Task<string> GetThemeAsync() => await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme") ?? "light";

    public async Task SetThemeAsync(string theme)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", theme);
        await _jsRuntime.InvokeVoidAsync("document.documentElement.setAttribute", "data-theme", theme);
        OnThemeChanged?.Invoke();
    }

    public async Task ToggleThemeAsync()
    {
        string currentTheme = await GetThemeAsync();
        string newTheme = currentTheme == "light" ? "dark" : "light";
        await SetThemeAsync(newTheme);
    }
}
