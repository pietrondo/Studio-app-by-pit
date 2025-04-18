using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Studio
{
    public class BookCompletionDialog : Window
    {
        private DatePicker _completionDatePicker = null!;
        private System.Windows.Controls.TextBox _reviewTextBox = null!;
        private System.Windows.Controls.Primitives.RepeatButton _ratingUpButton = null!;
        private System.Windows.Controls.Primitives.RepeatButton _ratingDownButton = null!;
        private System.Windows.Controls.TextBox _ratingTextBox = null!;
        private int? _rating = 1;  // Initialize with default value
        private bool _result = false;
        
        public DateTime CompletionDate { get; private set; } = DateTime.Now;
        public int? Rating => _rating;
        public string Review { get; private set; } = string.Empty;
        
        public BookCompletionDialog(string bookTitle)
        {
            Title = $"Completa Libro - {bookTitle}";
            Width = 450;
            Height = 400;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(242, 242, 242));
            
            Content = CreateDialogContent();
        }
        
        public new bool? ShowDialog()
        {
            base.ShowDialog();
            return _result;
        }
        
        private UIElement CreateDialogContent()
        {
            // Creazione del layout del dialogo
            Grid mainGrid = new Grid();
            mainGrid.Margin = new Thickness(20);
            
            // Definizione delle righe
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            
            // Panel principale
            Grid grid = new Grid();
            
            // Definizione delle righe
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10) }); // Spaziatura
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10) }); // Spaziatura
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            
            // Data di completamento
            StackPanel completionDatePanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock completionDateLabel = new TextBlock { Text = "Data Completamento:", Margin = new Thickness(0, 0, 0, 5) };
            _completionDatePicker = new DatePicker { MinHeight = 30, SelectedDate = DateTime.Now };
            
            completionDatePanel.Children.Add(completionDateLabel);
            completionDatePanel.Children.Add(_completionDatePicker);
            grid.Children.Add(completionDatePanel);
            Grid.SetRow(completionDatePanel, 0);
            
            // Valutazione
            StackPanel ratingPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock ratingLabel = new TextBlock { Text = "Valutazione (1-5):", Margin = new Thickness(0, 0, 0, 5) };
            
            // Create a numeric spinner control
            Grid ratingGrid = new Grid();
            ratingGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
            ratingGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            
            _ratingTextBox = new System.Windows.Controls.TextBox 
            { 
                MinHeight = 30,
                Width = 40,
                Text = "1",
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center
            };
            
            StackPanel spinnerPanel = new StackPanel { Orientation = System.Windows.Controls.Orientation.Vertical };
            _ratingUpButton = new System.Windows.Controls.Primitives.RepeatButton 
            { 
                Content = "▲",
                Height = 15
            };
            _ratingDownButton = new System.Windows.Controls.Primitives.RepeatButton 
            { 
                Content = "▼",
                Height = 15
            };
            
            _ratingUpButton.Click += (s, e) => {
                if (int.TryParse(_ratingTextBox.Text, out int currentValue) && currentValue < 5)
                {
                    _ratingTextBox.Text = (currentValue + 1).ToString();
                    _rating = currentValue + 1;
                }
            };
            
            _ratingDownButton.Click += (s, e) => {
                if (int.TryParse(_ratingTextBox.Text, out int currentValue) && currentValue > 1)
                {
                    _ratingTextBox.Text = (currentValue - 1).ToString();
                    _rating = currentValue - 1;
                }
            };
            
            _ratingTextBox.TextChanged += (s, e) => {
                if (int.TryParse(_ratingTextBox.Text, out int value))
                {
                    if (value < 1) { _ratingTextBox.Text = "1"; _rating = 1; }
                    else if (value > 5) { _ratingTextBox.Text = "5"; _rating = 5; }
                    else { _rating = value; }
                }
            };
            
            spinnerPanel.Children.Add(_ratingUpButton);
            spinnerPanel.Children.Add(_ratingDownButton);
            
            Grid.SetColumn(_ratingTextBox, 0);
            Grid.SetColumn(spinnerPanel, 1);
            
            ratingGrid.Children.Add(_ratingTextBox);
            ratingGrid.Children.Add(spinnerPanel);
            
            ratingPanel.Children.Add(ratingLabel);
            ratingPanel.Children.Add(ratingGrid);
            grid.Children.Add(ratingPanel);
            Grid.SetRow(ratingPanel, 2);
            
            // Recensione
            StackPanel reviewPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock reviewLabel = new TextBlock { Text = "Recensione:", Margin = new Thickness(0, 0, 0, 5) };
            _reviewTextBox = new System.Windows.Controls.TextBox { 
                MinHeight = 100, 
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            
            reviewPanel.Children.Add(reviewLabel);
            reviewPanel.Children.Add(_reviewTextBox);
            grid.Children.Add(reviewPanel);
            Grid.SetRow(reviewPanel, 4);
            
            // Aggiungi il grid al mainGrid
            mainGrid.Children.Add(grid);
            Grid.SetRow(grid, 0);
            
            // Pulsanti
            StackPanel buttonPanel = new StackPanel { 
                Orientation = System.Windows.Controls.Orientation.Horizontal, 
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                Margin = new Thickness(0, 15, 0, 0)
            };
            
            System.Windows.Controls.Button saveButton = new System.Windows.Controls.Button
            {
                Content = "Salva",
                Margin = new Thickness(5, 0, 0, 0),
                MinWidth = 80,
                MinHeight = 30,
                Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 120, 215)),
                Foreground = System.Windows.Media.Brushes.White,
                BorderThickness = new Thickness(0)
            };
            
            System.Windows.Controls.Button cancelButton = new System.Windows.Controls.Button
            {
                Content = "Annulla",
                Margin = new Thickness(5, 0, 0, 0),
                MinWidth = 80,
                MinHeight = 30
            };
            
            // Aggiungi gli eventi ai pulsanti
            saveButton.Click += SaveButton_Click;
            cancelButton.Click += CancelButton_Click;
            
            buttonPanel.Children.Add(saveButton);
            buttonPanel.Children.Add(cancelButton);
            mainGrid.Children.Add(buttonPanel);
            Grid.SetRow(buttonPanel, 1);
            
            return mainGrid;
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validazione e salvataggio dei valori
            CompletionDate = _completionDatePicker.SelectedDate ?? DateTime.Now;
            Review = _reviewTextBox.Text?.Trim() ?? string.Empty;
            
            // Rating is already validated and stored in _rating
            
            _result = true;
            Close();
        }
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _result = false;
            Close();
        }
    }
}