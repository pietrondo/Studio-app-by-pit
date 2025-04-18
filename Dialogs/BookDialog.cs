using System;
using System.Windows;
using System.Windows.Controls;

namespace Studio
{
    public class BookDialog : Window
    {
        private System.Windows.Controls.TextBox _titleTextBox = null!;
        private System.Windows.Controls.TextBox _authorTextBox = null!;
        private DatePicker _startDatePicker = null!;
        private bool _result = false;
        
        public new string Title { get; private set; } = string.Empty;  // Added 'new' keyword to resolve warning
        public string Author { get; private set; } = string.Empty;
        public DateTime StartDate { get; private set; } = DateTime.Now;
        
        public BookDialog(string title)
        {
            Title = title;
            Width = 450;
            Height = 350;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(242, 242, 242));
            
            Content = CreateDialogContent();
        }
        
        public BookDialog(string title, Book book) : this(title)
        {
            // Pre-popolamento dei campi con i dati del libro esistente
            _titleTextBox.Text = book.Title;
            _authorTextBox.Text = book.Author;
            _startDatePicker.SelectedDate = book.StartDate;
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
            
            // Titolo
            StackPanel titlePanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock titleLabel = new TextBlock { Text = "Titolo:", Margin = new Thickness(0, 0, 0, 5) };
            _titleTextBox = new System.Windows.Controls.TextBox { MinHeight = 30 };
            
            titlePanel.Children.Add(titleLabel);
            titlePanel.Children.Add(_titleTextBox);
            grid.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, 0);
            
            // Autore
            StackPanel authorPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock authorLabel = new TextBlock { Text = "Autore:", Margin = new Thickness(0, 0, 0, 5) };
            _authorTextBox = new System.Windows.Controls.TextBox { MinHeight = 30 };
            
            authorPanel.Children.Add(authorLabel);
            authorPanel.Children.Add(_authorTextBox);
            grid.Children.Add(authorPanel);
            Grid.SetRow(authorPanel, 2);
            
            // Data di inizio lettura
            StackPanel startDatePanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock startDateLabel = new TextBlock { Text = "Data Inizio Lettura:", Margin = new Thickness(0, 0, 0, 5) };
            _startDatePicker = new DatePicker { MinHeight = 30, SelectedDate = DateTime.Now };
            
            startDatePanel.Children.Add(startDateLabel);
            startDatePanel.Children.Add(_startDatePicker);
            grid.Children.Add(startDatePanel);
            Grid.SetRow(startDatePanel, 4);
            
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
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 120, 215)),
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
            // Validazione
            if (string.IsNullOrWhiteSpace(_titleTextBox.Text))
            {
                System.Windows.MessageBox.Show("Il titolo non può essere vuoto.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            // Salvataggio dei valori
            Title = _titleTextBox.Text.Trim();
            Author = _authorTextBox.Text?.Trim() ?? string.Empty;
            StartDate = _startDatePicker.SelectedDate ?? DateTime.Now;
            
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