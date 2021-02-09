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

        public static readonly DependencyProperty PulseButtonProperty = DependencyProperty.Register("PulseButton", typeof(PulseButton), typeof(MainWindow));


        public PulseButton PulseButton
        {
            get { return (PulseButton)GetValue(PulseButtonProperty); }
            set { SetValue(PulseButtonProperty, value); }
        }

        private PulseButtonColor pulseButtonColor { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            PulseButton = new PulseButton
            {
                Color = "LightGreen",

                Scale = 2,

                Count = 6,

                Width = 3,

                Content = "Start"
            };

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
            (sender as NMT.Wpf.Controls.PulseButton).GetBindingExpression(ContentProperty).UpdateTarget();
        }

        private void ChangePulseButton(PulseButtonColor pbc)
        {
            pulseButtonColor = pbc;
            switch (pulseButtonColor)
            {
                case PulseButtonColor.Green:
                    PulseButton = new PulseButton
                    {
                        Color = "LightGreen",

                        Scale = 2,

                        Count = 6,

                        Width = 3,

                        Content = "Start"
                    };
                    break;
                case PulseButtonColor.Red:
                    PulseButton = new PulseButton
                    {
                        Color = "Red",

                        Scale = 1.5M,

                        Count = 4,

                        Width = 2,

                        Content = "Working"
                    };
                    break;
            }
        }
    }
}
