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
    }
}
