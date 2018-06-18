using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace EssentialsDemo
{
    public partial class MainPage : ContentPage
    {
        private const string LocationKey = "LocationKey";
        private bool loaded = false;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // we only want to do this on launch, not resume
            if (loaded)
                return;
            loaded = true;

            // load the last-saved location
            lblSaved.Text = "loading saved location...";
            var lastLocation = Preferences.Get(LocationKey, string.Empty);
            if (string.IsNullOrEmpty(lastLocation))
            {
                lastLocation = "no location saved.";
            }
            lblSaved.Text = lastLocation;

            // get the current location
            GetStreetAddressAsync();
        }

        private async Task GetStreetAddressAsync()
        {
            // fetching new co-ordinates
            lblCoords.Text = "fetching current co-ordinates...";
            Location location = null;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                location = await Geolocation.GetLocationAsync(request);

                lblCoords.Text = $"{location.Latitude}, {location.Longitude}";
            }
            catch (Exception ex)
            {
                lblCoords.Text = $"error: {ex.Message}";
            }

            // fetching street address
            lblLocation.Text = "fetching street address...";
            string address = null;
            if (location != null)
            {
                try
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location);
                    var place = placemarks?.FirstOrDefault();

                    address =
                        $"{place.SubThoroughfare} {place.Thoroughfare} {place.SubLocality}\n" +
                        $"{place.Locality}\n" +
                        $"{place.AdminArea}\n" +
                        $"{place.CountryName}\n" +
                        $"{place.PostalCode}";
                    lblLocation.Text = address;
                }
                catch (Exception ex)
                {
                    lblLocation.Text = $"error: {ex}";
                }
            }
            else
            {
                lblLocation.Text = "unable to get street address without a location.";
            }

            // saving the current street address
            if (!string.IsNullOrWhiteSpace(address))
            {
                Preferences.Set(LocationKey, address);
            }
        }

        private async void OnMoreClicked(object sender, EventArgs e)
        {
            const string ResetButton = "Reset preferences";
            const string RefreshButton = "Refresh location";

            var selected = await DisplayActionSheet("More Options", "Cancel", null, ResetButton, RefreshButton);

            switch (selected)
            {
                case ResetButton:
                    Preferences.Remove(LocationKey);
                    lblSaved.Text = "no location saved.";
                    break;

                case RefreshButton:
                    await GetStreetAddressAsync();
                    break;
            }
        }
    }
}
