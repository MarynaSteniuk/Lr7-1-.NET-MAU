namespace Astronomy.Pages;

public partial class SunrisePage : ContentPage
{
    public SunrisePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var sunData = await GetSunriseSunsetData();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            InitializeUI(sunData.Item1, sunData.Item2, sunData.Item3);
        });
    }

    private async Task<(DateTime, DateTime, TimeSpan)> GetSunriseSunsetData()
    {
        var latLongData = await new LatLongService().GetLatLong();
        var sunData = await new SunriseService().GetSunriseSunsetTimes(latLongData.Latitude, latLongData.Longitude);

        var riseTime = sunData.Sunrise.ToLocalTime();
        var setTime = sunData.Sunset.ToLocalTime();
        var span = setTime.TimeOfDay - riseTime.TimeOfDay;

        return (riseTime, setTime, span);
    }

    void InitializeUI(DateTime riseTime, DateTime setTime, TimeSpan span)
    {
        
        lblDate.Text = DateTime.Today.ToString("D");
        lblSunrise.Text = riseTime.ToString("h:mm tt");
        lblSunset.Text = setTime.ToString("h:mm tt");
        lblDaylight.Text = $"{span.Hours} hours, {span.Minutes} minutes";
    }
}