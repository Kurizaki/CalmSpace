namespace CalmSpace.Views
{
    public partial class FavouritesView : ContentView
    {
        public FavouritesView()
        {
            InitializeComponent();
        }

        private void CloseFavourites(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }
    }
}
