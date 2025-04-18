using System;
using System.Windows;
using System.Windows.Controls;

namespace Studio
{
    public class ExamDialog : Window
    {
        private System.Windows.Controls.TextBox _subjectTextBox = null!;
        private DatePicker _examDatePicker = null!;
        private System.Windows.Controls.TextBox _locationTextBox = null!;
        private bool _result = false;
        
        public string Subject { get; private set; } = string.Empty;
        public DateTime ExamDate { get; private set; } = DateTime.Now;
        public string Location { get; private set; } = string.Empty;
        
        public ExamDialog(string title)
        {
            Title = title;
            Width = 450;
            Height = 350;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(242, 242, 242));
            
            Content = CreateDialogContent();
        }
        
        public ExamDialog(string title, Exam exam) : this(title)
        {
            // Pre-popolamento dei campi con i dati dell'esame esistente
            _subjectTextBox.Text = exam.Subject;
            _examDatePicker.SelectedDate = exam.ExamDate;
            _locationTextBox.Text = exam.Location;
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
            
            // Materia
            StackPanel subjectPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock subjectLabel = new TextBlock { Text = "Materia:", Margin = new Thickness(0, 0, 0, 5) };
            _subjectTextBox = new System.Windows.Controls.TextBox { MinHeight = 30 };
            
            subjectPanel.Children.Add(subjectLabel);
            subjectPanel.Children.Add(_subjectTextBox);
            grid.Children.Add(subjectPanel);
            Grid.SetRow(subjectPanel, 0);
            
            // Data dell'esame
            StackPanel examDatePanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock examDateLabel = new TextBlock { Text = "Data Esame:", Margin = new Thickness(0, 0, 0, 5) };
            _examDatePicker = new DatePicker { MinHeight = 30, SelectedDate = DateTime.Now };
            
            examDatePanel.Children.Add(examDateLabel);
            examDatePanel.Children.Add(_examDatePicker);
            grid.Children.Add(examDatePanel);
            Grid.SetRow(examDatePanel, 2);
            
            // Luogo
            StackPanel locationPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock locationLabel = new TextBlock { Text = "Luogo:", Margin = new Thickness(0, 0, 0, 5) };
            _locationTextBox = new System.Windows.Controls.TextBox { MinHeight = 30 };
            
            locationPanel.Children.Add(locationLabel);
            locationPanel.Children.Add(_locationTextBox);
            grid.Children.Add(locationPanel);
            Grid.SetRow(locationPanel, 4);
            
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
            if (string.IsNullOrWhiteSpace(_subjectTextBox.Text))
            {
                System.Windows.MessageBox.Show("La materia non può essere vuota.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            // Salvataggio dei valori
            Subject = _subjectTextBox.Text.Trim();
            ExamDate = _examDatePicker.SelectedDate ?? DateTime.Now;
            Location = _locationTextBox.Text?.Trim() ?? string.Empty;
            
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