using Microsoft.Maui.Controls;
using System.Linq;

namespace CalmSpace
{
    public partial class AppShell : Shell
    {
        public TabBar MainTabBarControl { get; private set; }
        public AppShell()
        {
            InitializeComponent();
            SetDefaultTab();
        }

        private void SetDefaultTab()
        {
            MainTabBarControl = this.Items[0] as TabBar;

            if (MainTabBarControl != null)
            {
                var homeTab = MainTabBarControl.Items.OfType<Tab>().FirstOrDefault(tab => tab.Title == "Home");
                if (homeTab != null)
                {
                    this.CurrentItem = homeTab;
                }
            }
        }
    }
}
