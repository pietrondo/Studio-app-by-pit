using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Studio
{
    public class TaskDialog : Window
    {
        private System.Windows.Controls.TextBox _descriptionTextBox = null!;
        private DatePicker _dueDatePicker = null!;
        private System.Windows.Controls.ComboBox _priorityComboBox = null!;
        private bool _result = false;
        
        public string TaskDescription { get; private set; } = string.Empty;
        public DateTime DueDate { get; private set; } = DateTime.Now.AddDays(1);
        public Priority Priority { get; private set; } = Priority.Medium;
        
        public TaskDialog(string title)
        {
            Title = title;
            Width = 450;
            Height = 350;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(242, 242, 242));
            
            Content = CreateDialogContent();
        }
        
        public TaskDialog(string title, Task task) : this(title)
        {
            // Pre-popolamento dei campi con i dati della task esistente
            _descriptionTextBox.Text = task.Description;
            _dueDatePicker.SelectedDate = task.DueDate;
            _priorityComboBox.SelectedItem = task.Priority.ToString();
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
            
            // Descrizione
            StackPanel descriptionPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock descriptionLabel = new TextBlock { Text = "Descrizione:", Margin = new Thickness(0, 0, 0, 5) };
            _descriptionTextBox = new System.Windows.Controls.TextBox { MinHeight = 30 };
            
            descriptionPanel.Children.Add(descriptionLabel);
            descriptionPanel.Children.Add(_descriptionTextBox);
            grid.Children.Add(descriptionPanel);
            Grid.SetRow(descriptionPanel, 0);
            
            // Data di scadenza
            StackPanel dueDatePanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock dueDateLabel = new TextBlock { Text = "Data Scadenza:", Margin = new Thickness(0, 0, 0, 5) };
            _dueDatePicker = new DatePicker { MinHeight = 30, SelectedDate = DateTime.Now.AddDays(1) };
            
            dueDatePanel.Children.Add(dueDateLabel);
            dueDatePanel.Children.Add(_dueDatePicker);
            grid.Children.Add(dueDatePanel);
            Grid.SetRow(dueDatePanel, 2);
            
            // Priorità
            StackPanel priorityPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            TextBlock priorityLabel = new TextBlock { Text = "Priorità:", Margin = new Thickness(0, 0, 0, 5) };
            _priorityComboBox = new System.Windows.Controls.ComboBox { 
                MinHeight = 30, 
                MinWidth = 150, 
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left 
            };
            
            // Popola il ComboBox con i valori dell'enum Priority
            foreach (string priorityName in Enum.GetNames(typeof(Priority)))
            {
                _priorityComboBox.Items.Add(priorityName);
            }
            _priorityComboBox.SelectedIndex = 1; // Default: Medium
            
            priorityPanel.Children.Add(priorityLabel);
            priorityPanel.Children.Add(_priorityComboBox);
            grid.Children.Add(priorityPanel);
            Grid.SetRow(priorityPanel, 4);
            
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
            // Validazione
            if (string.IsNullOrWhiteSpace(_descriptionTextBox.Text))
            {
                System.Windows.MessageBox.Show("La descrizione non può essere vuota.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            // Salvataggio dei valori
            TaskDescription = _descriptionTextBox.Text.Trim();
            DueDate = _dueDatePicker.SelectedDate ?? DateTime.Now.AddDays(1);
            
            if (_priorityComboBox.SelectedItem != null)
            {
                Priority = (Priority)Enum.Parse(typeof(Priority), _priorityComboBox.SelectedItem.ToString() ?? "Medium");
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