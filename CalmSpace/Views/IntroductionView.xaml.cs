namespace CalmSpace.Views
{
    public partial class IntroductionView : ContentView
    {
        public IntroductionView()
        {
            InitializeComponent();
        }

        private void CloseIntroduction(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }
    }
}
