using System.Windows;

namespace Proxy_Checker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum PulseButtonColor
        {
            Green,
            Red
        }

        private PulseButtonColor pulseButtonColor;

        public MainWindow()
        {
            InitializeComponent();
            itemsControl.Items.Add("Test 1");
            itemsControl.Items.Add("Test 2");
            itemsControl.Items.Add("Test 3");

            pulseButtonColor = PulseButtonColor.Green;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void PulseButton_Click(object sender, RoutedEventArgs e)
        {
            if (pulseButtonColor == PulseButtonColor.Green)
                ChangePulseButton(PulseButtonColor.Red);
            else
                ChangePulseButton(PulseButtonColor.Green);
        }

        private void ChangePulseButton(PulseButtonColor pbc)
        {
            switch (pulseButtonColor)
            {
                case PulseButtonColor.Green:

                    break;
                case PulseButtonColor.Red:
                    break;
            }
        }
    }
}
