using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Studio
{
    public class ExamResultDialog : Window
    {
        private System.Windows.Controls.CheckBox _isPassedCheckBox = null!;
        private System.Windows.Controls.TextBox _scoreTextBox = null!;
        private bool _result = false;
        
        public bool IsPassed { get; private set; } = false;
        public double Score { get; private set; } = 0;
        
        public ExamResultDialog(string examSubject)
        {
            Title = $"Registra Risultato - {examSubject}";
            Width = 450;
            Height = 300;
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
            
            // Esame superato
            _isPassedCheckBox = new System.Windows.Controls.CheckBox { 
                Content = "Esame Superato", 
                Margin = new Thickness(0, 0, 0, 5),
                VerticalAlignment = VerticalAlignment.Center
            };
            
            grid.Children.Add(_isPassedCheckBox);
            Grid.SetRow(_isPassedCheckBox, 0);
            
            // Voto
            StackPanel scorePanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock scoreLabel = new TextBlock { Text = "Voto (0-30):", Margin = new Thickness(0, 0, 0, 5) };
            
            // Sostituiamo NumberBox con un TextBox semplice
            _scoreTextBox = new System.Windows.Controls.TextBox { 
                MinHeight = 30,
                Width = 150,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left
            };
            
            scorePanel.Children.Add(scoreLabel);
            scorePanel.Children.Add(_scoreTextBox);
            grid.Children.Add(scorePanel);
            Grid.SetRow(scorePanel, 2);
            
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
            IsPassed = _isPassedCheckBox.IsChecked ?? false;
            
            if (!double.TryParse(_scoreTextBox.Text, out double score))
            {
                System.Windows.MessageBox.Show("Inserisci un voto valido.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (score < 0 || score > 30)
            {
                System.Windows.MessageBox.Show("Il voto deve essere compreso tra 0 e 30.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            Score = score;
            
            // Coerenza tra IsPassed e Score
            if (IsPassed && Score < 18)
            {
                var result = System.Windows.MessageBox.Show(
                    "Hai segnato l'esame come superato ma il voto è inferiore a 18. Sei sicuro?",
                    "Attenzione",
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Warning);
                
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            else if (!IsPassed && Score >= 18)
            {
                var result = System.Windows.MessageBox.Show(
                    "Hai segnato l'esame come non superato ma il voto è maggiore o uguale a 18. Sei sicuro?",
                    "Attenzione",
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Warning);
                
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            
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