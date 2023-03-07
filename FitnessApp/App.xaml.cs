using FitnessApp.Tool;

namespace FitnessApp;

public partial class App : Application
{
	public App()
	{
		MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Created += async (s, e) =>
        {
            await DatabaseService.OpenConnectionAsync();
        };

        window.Destroying += async (s, e) =>
        {
            await DatabaseService.CloseConnectionAsync();
        };

        return window;
    }
}
