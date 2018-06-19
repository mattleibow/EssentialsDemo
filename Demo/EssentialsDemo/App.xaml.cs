using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace EssentialsDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // the Bing maps API key for UWP
            Geocoding.MapKey = "XlsUne0USC9xTQ0EOIZC~ofT1GRucPY1M2GlkHKDkZw~AoiP7td1fhMcxPuhcLLXiovBTLtZuiJTEDmMoAEzHdTk_J6-k91o-TBuuCaks0Ou";

            MainPage = new NavigationPage(new MainPage());
        }
    }
}
