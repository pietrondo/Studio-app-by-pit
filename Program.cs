using System;
using System.Windows;

namespace Studio
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var application = new System.Windows.Application();
            application.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            application.Run();
        }
    }
}
