namespace CalmSpace.Views
{
    public partial class SettingsView : ContentView
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void CloseSettings(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }
    }
}
