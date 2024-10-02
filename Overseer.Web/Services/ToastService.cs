using Overseer.Web.Components;

namespace Overseer.Web.Services;

public class ToastService
{
    #pragma warning disable CA1003
    public event Func<string, string, Toast.ToastType, Task>? OnShow;
#pragma warning restore CA1003

    public Task Show(string title, string message, Toast.ToastType type) => OnShow?.Invoke(title, message, type) ?? Task.CompletedTask;
}
