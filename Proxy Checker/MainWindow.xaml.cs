using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        public static readonly DependencyProperty MessageListProperty = DependencyProperty.Register("MessageList", typeof(LinkedList<string>), typeof(MainWindow));

        public PulseButton PulseButton
        {
            get { return (PulseButton)GetValue(PulseButtonProperty); }
            set { SetValue(PulseButtonProperty, value); }
        }

        public LinkedList<string> MessageList
        {
            get { return (LinkedList<string>)GetValue(MessageListProperty); }
            set { SetValue(MessageListProperty, value); }
        }

        private PulseButtonColor pulseButtonColor { get; set; }

        public string ProxyFilePath { get; set; }

        private int MessageLimit { get; set; } = 3;

        public MainWindow()
        {
            InitializeComponent();

            MessageList = new LinkedList<string>();
            AddMessage("Test 1");
            AddMessage("Test 2");
            AddMessage("Test 3");
            AddMessage("Test 4");
            AddMessage("Test 5");

            pulseButtonColor = PulseButtonColor.Green;
            ProxyFilePath = AppDomain.CurrentDomain.BaseDirectory + "proxy.txt";
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
                    PulseButton = null;
                    break;
                case PulseButtonColor.Red:
                    PulseButton = new PulseButton
                    {
                        Color = "Red",

                        Scale = 1.5M,

                        Count = 4,

                        Width = 2,

                        Content = "Working",

                        FontSize = 10
                    };
                    break;
            }
        }

        private void AddMessage(string msg)
        {
            MessageList.AddFirst(msg);
            if (MessageList.Count > MessageLimit)
                MessageList.RemoveLast();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text|*.txt|All|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                ProxyFilePath = openFileDialog.FileName;
            }  
        }
    }
}
