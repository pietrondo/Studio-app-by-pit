using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Studio
{
    public class NotificationWindow : Window
    {
        private DispatcherTimer _timer;
        
        public NotificationWindow(string title, string message, string? details = null)
        {
            // Configura la finestra come finestra di notifica
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;
            Topmost = true;
            Width = 350;
            Height = details != null ? 150 : 100;
            
            // Posiziona nell'angolo in basso a destra
            WindowStartupLocation = WindowStartupLocation.Manual;
            
            // Sfondo con effetto acrilico/traslucido
            Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(45, 137, 239));
            
            // Bordi arrotondati
            BorderThickness = new Thickness(0);
            
            // Ombra (effetto con il bordo)
            BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 0, 0, 0));
            
            // Crea il contenuto
            Grid mainGrid = new Grid();
            mainGrid.Margin = new Thickness(15);
            
            // Definizione delle righe
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            
            if (details != null)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            
            // Titolo della notifica
            TextBlock titleBlock = new TextBlock
            {
                Text = title,
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Foreground = System.Windows.Media.Brushes.White,
                Margin = new Thickness(0, 0, 0, 5)
            };
            mainGrid.Children.Add(titleBlock);
            Grid.SetRow(titleBlock, 0);
            
            // Messaggio della notifica
            TextBlock messageBlock = new TextBlock
            {
                Text = message,
                FontSize = 14,
                Foreground = System.Windows.Media.Brushes.White,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 5)
            };
            mainGrid.Children.Add(messageBlock);
            Grid.SetRow(messageBlock, 1);
            
            // Dettagli aggiuntivi (opzionali)
            if (details != null)
            {
                TextBlock detailsBlock = new TextBlock
                {
                    Text = details,
                    FontSize = 12,
                    Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(230, 255, 255, 255)),
                    TextWrapping = TextWrapping.Wrap
                };
                mainGrid.Children.Add(detailsBlock);
                Grid.SetRow(detailsBlock, 2);
            }
            
            Content = mainGrid;
            
            // Imposta il posizionamento nell'angolo in basso a destra
            PositionWindow();
            
            // Aggiungi effetto di fade in
            Opacity = 0;
            
            // Timer per la chiusura automatica
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(4);
            _timer.Tick += (sender, e) => CloseNotification();
            
            // Gestione degli eventi
            Loaded += NotificationWindow_Loaded;
            MouseEnter += NotificationWindow_MouseEnter;
            MouseLeave += NotificationWindow_MouseLeave;
            MouseLeftButtonUp += NotificationWindow_MouseLeftButtonUp;
        }
        
        private void PositionWindow()
        {
            // Posiziona nell'angolo in basso a destra dello schermo
            var workingArea = SystemParameters.WorkArea;
            Left = workingArea.Right - Width - 10;
            Top = workingArea.Bottom - Height - 10;
        }
        
        private void NotificationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Animazione di fade-in
            BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300)));
            
            // Avvia il timer per la chiusura automatica
            _timer.Start();
        }
        
        private void NotificationWindow_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Quando il mouse è sopra la notifica, ferma il timer
            _timer.Stop();
        }
        
        private void NotificationWindow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Quando il mouse esce dalla notifica, riavvia il timer
            _timer.Start();
        }
        
        private void NotificationWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Chiudi la notifica al clic
            CloseNotification();
        }
        
        private void CloseNotification()
        {
            // Ferma il timer
            _timer.Stop();
            
            // Animazione di fade-out
            var animation = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
            animation.Completed += (s, e) => Close();
            BeginAnimation(OpacityProperty, animation);
        }
    }
}