using Microsoft.Maui.Controls;

namespace CalmSpace.Helpers
{
    public static class SwipeHandler
    {
        public static void OnSwiped(Shell shell, SwipedEventArgs e)
        {
            if (shell == null) return;

            var currentShellItem = shell.CurrentItem;
            if (currentShellItem == null) return;

            var currentShellSection = currentShellItem.CurrentItem;
            if (currentShellSection == null) return;

            var currentIndex = currentShellItem.Items.IndexOf(currentShellSection);

            if (currentIndex == -1) return;

            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    if (currentIndex < currentShellItem.Items.Count - 1)
                    {
                        var nextShellSection = currentShellItem.Items[currentIndex + 1];
                        shell.CurrentItem.CurrentItem = nextShellSection;
                    }
                    else
                    {
                        var firstShellItem = shell.Items.FirstOrDefault();
                        if (firstShellItem != null)
                        {
                            shell.CurrentItem = firstShellItem;
                            shell.CurrentItem.CurrentItem = firstShellItem.Items.FirstOrDefault();
                        }
                    }
                    break;

                case SwipeDirection.Right:
                    if (currentIndex > 0)
                    {
                        var previousShellSection = currentShellItem.Items[currentIndex - 1];
                        shell.CurrentItem.CurrentItem = previousShellSection;
                    }
                    else
                    {
                        var lastShellItem = shell.Items.LastOrDefault();
                        if (lastShellItem != null)
                        {
                            shell.CurrentItem = lastShellItem;
                            shell.CurrentItem.CurrentItem = lastShellItem.Items.LastOrDefault();
                        }
                    }
                    break;
            }
        }
    }
}
