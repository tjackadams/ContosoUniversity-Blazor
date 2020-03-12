using BlazorPro.Spinkit;

namespace ContosoUniversity.Client.Components
{
    public class ContosoSpinLoader : SpinLoader
    {
        protected override void OnInitialized()
        {
            Center = true;
            Spinner = SpinnerType.CircleFade;
            Color = "#0078d4";

            base.OnInitialized();
        }
    }
}