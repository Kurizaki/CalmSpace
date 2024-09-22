using Microsoft.Maui.Controls;

namespace CalmSpace.Helpers
{
    public static class SwipeHandler
    {
        public static void OnSwiped(Shell shell, SwipedEventArgs e)
        {
            if (shell?.CurrentItem?.CurrentItem == null) return;

            var currentShellItem = shell.CurrentItem;
            var currentShellSection = currentShellItem.CurrentItem;
            var currentIndex = currentShellItem.Items.IndexOf(currentShellSection);

            if (currentIndex == -1) return;

            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    NavigateToNextSection(shell, currentShellItem, currentIndex);
                    break;

                case SwipeDirection.Right:
                    NavigateToPreviousSection(shell, currentShellItem, currentIndex);
                    break;
            }
        }

        private static void NavigateToNextSection(Shell shell, ShellItem currentShellItem, int currentIndex)
        {
            if (currentIndex < currentShellItem.Items.Count - 1)
            {
                shell.CurrentItem.CurrentItem = currentShellItem.Items[currentIndex + 1];
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
        }

        private static void NavigateToPreviousSection(Shell shell, ShellItem currentShellItem, int currentIndex)
        {
            if (currentIndex > 0)
            {
                shell.CurrentItem.CurrentItem = currentShellItem.Items[currentIndex - 1];
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
        }
    }
}
