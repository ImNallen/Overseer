using Overseer.Web.Components.Toast;

namespace Overseer.Web.Services.Toast;

public class ToastService
{
    #pragma warning disable CA1003
    public event Func<string, string, Components.Toast.Toast.ToastType, Task>? OnShow;
#pragma warning restore CA1003

    public Task Show(string title, string message, Components.Toast.Toast.ToastType type) => OnShow?.Invoke(title, message, type) ?? Task.CompletedTask;
}
