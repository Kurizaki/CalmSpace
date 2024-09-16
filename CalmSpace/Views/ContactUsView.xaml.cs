namespace CalmSpace.Views
{
    public partial class ContactUsView : ContentView
    {
        public ContactUsView()
        {
            InitializeComponent();
        }

        private void CloseContactUs(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }

        private async void OnFacebookClicked(object sender, EventArgs e)
        {
            var uri = new Uri("https://www.facebook.com/YourPage");
            await Launcher.OpenAsync(uri);
        }
        private async void OnTwitterClicked(object sender, EventArgs e)
        {
            var uri = new Uri("https://www.facebook.com/YourPage");
            await Launcher.OpenAsync(uri);
        }
        private async void OnInstagramClicked(object sender, EventArgs e)
        {
            var uri = new Uri("https://www.facebook.com/YourPage");
            await Launcher.OpenAsync(uri);
        }
    }
}
