using System;
using System.ComponentModel; // Aggiunto per CancelEventArgs
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Forms; // Aggiunto per NotifyIcon e ContextMenuStrip
using System.Drawing; // Aggiunto per Icon

namespace Studio
{
    public partial class MainWindow : Window
    {
        private TaskManager _taskManager;
        private ExamManager _examManager;
        private BookTracker _bookTracker;
        
        // Risorse per i temi
        private SolidColorBrush _lightBackground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(242, 242, 242));
        private SolidColorBrush _darkBackground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(45, 45, 48));
        private SolidColorBrush _lightText = new SolidColorBrush(System.Windows.Media.Color.FromRgb(51, 51, 51));
        private SolidColorBrush _darkText = new SolidColorBrush(System.Windows.Media.Color.FromRgb(240, 240, 240));
        private SolidColorBrush _alternateRowLightColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(249, 249, 249));
        private SolidColorBrush _alternateRowDarkColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(60, 60, 63));
        private SolidColorBrush _secondaryBackgroundLight = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        private SolidColorBrush _secondaryBackgroundDark = new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 30, 33));

        private NotifyIcon _notifyIcon;
        private bool _isExplicitClose = false; // Flag per gestire la chiusura effettiva

        public MainWindow()
        {
            InitializeComponent();
            
            // Inizializzazione dei manager
            _taskManager = new TaskManager();
            _examManager = new ExamManager();
            _bookTracker = new BookTracker();

            InitializeNotifyIcon();
        }

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "Studio Manager"; // Testo tooltip
            // Assicurati di avere un file 'app_icon.ico' nelle risorse del progetto
            // e che sia impostato come 'Resource' o 'Content' (Copy if newer)
            try
            {
                // Prova a caricare l'icona dalle risorse incorporate o da un file
                // Cambia il percorso se necessario
                _notifyIcon.Icon = new System.Drawing.Icon("app_icon.ico"); 
            }
            catch (Exception ex)
            {
                // Fallback o gestione errore se l'icona non viene trovata
                System.Diagnostics.Debug.WriteLine($"Icona non trovata: {ex.Message}");
                // Potresti usare un'icona di sistema come fallback, ma richiede più codice P/Invoke
            }

            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            // Creazione del menu contestuale per la tray icon
            var contextMenu = new ContextMenuStrip();
            var showMenuItem = new ToolStripMenuItem("Mostra");
            showMenuItem.Click += ShowMenuItem_Click;
            var exitMenuItem = new ToolStripMenuItem("Esci");
            exitMenuItem.Click += ExitMenuItem_Click;

            contextMenu.Items.Add(showMenuItem);
            contextMenu.Items.Add(new ToolStripSeparator()); // Separatore
            contextMenu.Items.Add(exitMenuItem);

            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Carica i dati iniziali
            RefreshTaskData();
            RefreshExamData();
            RefreshBookData();
        }

        // Gestione eventi NotifyIcon
        private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
        {
            ShowMainWindow();
        }

        private void ShowMenuItem_Click(object? sender, EventArgs e)
        {
            ShowMainWindow();
        }

        private void ExitMenuItem_Click(object? sender, EventArgs e)
        {
            _isExplicitClose = true; // Imposta il flag per permettere la chiusura
            System.Windows.Application.Current.Shutdown(); // Chiude l'applicazione
        }

        // Gestione comportamento finestra
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
                // Opzionale: mostra una notifica balloon quando minimizzato nella tray
                // _notifyIcon.ShowBalloonTip(1000, "Studio Manager", "L'applicazione è ancora attiva qui.", ToolTipIcon.Info);
            }
            base.OnStateChanged(e);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Se la chiusura non è esplicita (dal menu tray), annulla la chiusura
            // e nascondi la finestra invece di chiudere l'app.
            if (!_isExplicitClose)
            {
                e.Cancel = true;
                this.Hide();
                // Opzionale: mostra una notifica balloon
                // _notifyIcon.ShowBalloonTip(1000, "Studio Manager", "L'applicazione è stata minimizzata nella tray.", ToolTipIcon.Info);
            }
            else
            {
                // Pulizia NotifyIcon prima di chiudere
                if (_notifyIcon != null)
                {
                    _notifyIcon.Visible = false;
                    _notifyIcon.Dispose();
                    _notifyIcon = null!;
                }
            }
        }

        private void ShowMainWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate(); // Porta la finestra in primo piano
        }

        #region Gestione Temi
        
        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            // Imposta il tema scuro
            ApplyDarkTheme();
        }
        
        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            // Imposta il tema chiaro
            ApplyLightTheme();
        }
        
        private void ApplyDarkTheme()
        {
            // Cambia i colori principali
            Background = _darkBackground;
            Resources["PrimaryBackgroundColor"] = _darkBackground;
            Resources["SecondaryBackgroundColor"] = _secondaryBackgroundDark;
            Resources["TextColor"] = _darkText;
            Resources["AlternateRowColor"] = _alternateRowDarkColor;
            Resources["BorderColor"] = new SolidColorBrush(System.Windows.Media.Color.FromRgb(70, 70, 73));
            
            // Aggiorna lo stato dell'elemento ThemeToggle
            ThemeToggle.Content = "Tema Chiaro";
            
            // Forza l'aggiornamento delle DataGrid
            TaskDataGrid.ItemsSource = null;
            ExamDataGrid.ItemsSource = null;
            BookDataGrid.ItemsSource = null;
            
            RefreshTaskData();
            RefreshExamData();
            RefreshBookData();
        }
        
        private void ApplyLightTheme()
        {
            // Cambia i colori principali
            Background = _lightBackground;
            Resources["PrimaryBackgroundColor"] = _lightBackground;
            Resources["SecondaryBackgroundColor"] = _secondaryBackgroundLight;
            Resources["TextColor"] = _lightText;
            Resources["AlternateRowColor"] = _alternateRowLightColor;
            Resources["BorderColor"] = new SolidColorBrush(System.Windows.Media.Color.FromRgb(221, 221, 221));
            
            // Aggiorna lo stato dell'elemento ThemeToggle
            ThemeToggle.Content = "Tema Scuro";
            
            // Forza l'aggiornamento delle DataGrid
            TaskDataGrid.ItemsSource = null;
            ExamDataGrid.ItemsSource = null;
            BookDataGrid.ItemsSource = null;
            
            RefreshTaskData();
            RefreshExamData();
            RefreshBookData();
        }
        
        #endregion
        
        #region Notifiche
        
        private void NotificationButton_Click(object sender, RoutedEventArgs e)
        {
            // Mostra una notifica di esempio
            ShowNotification("Studio Manager", "Benvenuto nell'app modernizzata!", 
                "La nuova interfaccia è stata progettata per assomigliare alle app di notifica di Windows.");
        }
        
        private void ShowNotification(string title, string message, string? details = null)
        {
            NotificationWindow notification = new NotificationWindow(title, message, details);
            notification.Owner = this;
            notification.Show();
        }
        
        #endregion
        
        #region Gestione Task
        
        private void RefreshTaskData()
        {
            TaskDataGrid.ItemsSource = null;
            TaskDataGrid.ItemsSource = _taskManager.GetAllTasks();
        }
        
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Crea un form di dialogo moderno per aggiungere una task
            var dialog = new TaskDialog("Aggiungi Task");
            dialog.Owner = this;
            
            if (dialog.ShowDialog() == true)
            {
                // Aggiungi la task e aggiorna la DataGrid
                _taskManager.AddTask(dialog.TaskDescription, dialog.DueDate, dialog.Priority);
                RefreshTaskData();
                
                // Mostra notifica di conferma
                ShowNotification("Studio Manager", "Task aggiunta con successo!");
            }
        }
        
        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = TaskDataGrid.SelectedItem as Task;
            if (selectedTask != null)
            {
                // Crea un form di dialogo moderno per modificare una task
                var dialog = new TaskDialog("Modifica Task", selectedTask);
                dialog.Owner = this;
                
                if (dialog.ShowDialog() == true)
                {
                    // Modifica la task e aggiorna la DataGrid
                    _taskManager.EditTask(selectedTask.Id, dialog.TaskDescription, dialog.DueDate, dialog.Priority);
                    RefreshTaskData();
                    
                    // Mostra notifica di conferma
                    ShowNotification("Studio Manager", "Task modificata con successo!");
                }
            }
            else
            {
                ShowWarningMessage("Seleziona prima una task da modificare");
            }
        }
        
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = TaskDataGrid.SelectedItem as Task;
            if (selectedTask != null)
            {
                var result = System.Windows.MessageBox.Show(
                    "Sei sicuro di voler eliminare questa task?", 
                    "Conferma eliminazione", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question);
                    
                if (result == MessageBoxResult.Yes)
                {
                    _taskManager.RemoveTask(selectedTask.Id);
                    RefreshTaskData();
                    
                    // Mostra notifica di conferma
                    ShowNotification("Studio Manager", "Task eliminata con successo!");
                }
            }
            else
            {
                ShowWarningMessage("Seleziona prima una task da eliminare");
            }
        }
        
        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = TaskDataGrid.SelectedItem as Task;
            if (selectedTask != null)
            {
                _taskManager.CompleteTask(selectedTask.Id);
                RefreshTaskData();
                
                // Mostra notifica di conferma
                ShowNotification("Studio Manager", "Task completata con successo!");
            }
            else
            {
                ShowWarningMessage("Seleziona prima una task da completare");
            }
        }
        
        #endregion
        
        #region Gestione Esami
        
        private void RefreshExamData()
        {
            ExamDataGrid.ItemsSource = null;
            ExamDataGrid.ItemsSource = _examManager.GetAllExams();
        }
        
        private void AddExamButton_Click(object sender, RoutedEventArgs e)
        {
            // Crea un form di dialogo moderno per aggiungere un esame
            var dialog = new ExamDialog("Aggiungi Esame");
            dialog.Owner = this;
            
            if (dialog.ShowDialog() == true)
            {
                // Aggiungi l'esame e aggiorna la DataGrid
                _examManager.AddExam(dialog.Subject, dialog.ExamDate, dialog.Location);
                RefreshExamData();
                
                // Mostra notifica di conferma
                ShowNotification("Studio Manager", "Esame aggiunto con successo!");
            }
        }
        
        private void EditExamButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedExam = ExamDataGrid.SelectedItem as Exam;
            if (selectedExam != null)
            {
                // Crea un form di dialogo moderno per modificare un esame
                var dialog = new ExamDialog("Modifica Esame", selectedExam);
                dialog.Owner = this;
                
                if (dialog.ShowDialog() == true)
                {
                    // Modifica l'esame e aggiorna la DataGrid
                    _examManager.EditExam(selectedExam.Id, dialog.Subject, dialog.ExamDate, dialog.Location);
                    RefreshExamData();
                    
                    // Mostra notifica di conferma
                    ShowNotification("Studio Manager", "Esame modificato con successo!");
                }
            }
            else
            {
                ShowWarningMessage("Seleziona prima un esame da modificare");
            }
        }
        
        private void DeleteExamButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedExam = ExamDataGrid.SelectedItem as Exam;
            if (selectedExam != null)
            {
                var result = System.Windows.MessageBox.Show(
                    "Sei sicuro di voler eliminare questo esame?", 
                    "Conferma eliminazione", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question);
                    
                if (result == MessageBoxResult.Yes)
                {
                    _examManager.RemoveExam(selectedExam.Id);
                    RefreshExamData();
                    
                    // Mostra notifica di conferma
                    ShowNotification("Studio Manager", "Esame eliminato con successo!");
                }
            }
            else
            {
                ShowWarningMessage("Seleziona prima un esame da eliminare");
            }
        }
        
        private void RecordResultButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedExam = ExamDataGrid.SelectedItem as Exam;
            if (selectedExam != null)
            {
                // Crea un form di dialogo moderno per registrare il risultato di un esame
                var dialog = new ExamResultDialog(selectedExam.Subject);
                dialog.Owner = this;
                
                if (dialog.ShowDialog() == true)
                {
                    // Registra il risultato dell'esame e aggiorna la DataGrid
                    _examManager.RecordExamResult(selectedExam.Id, dialog.IsPassed, (int?)dialog.Score);
                    RefreshExamData();
                    
                    // Mostra notifica di conferma
                    ShowNotification("Studio Manager", "Risultato esame registrato con successo!");
                }
            }
            else
            {
                ShowWarningMessage("Seleziona prima un esame per registrare il risultato");
            }
        }
        
        #endregion
        
        #region Gestione Libri
        
        private void RefreshBookData()
        {
            BookDataGrid.ItemsSource = null;
            BookDataGrid.ItemsSource = _bookTracker.GetAllBooks();
        }
        
        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            // Crea un form di dialogo moderno per aggiungere un libro
            var dialog = new BookDialog("Aggiungi Libro");
            dialog.Owner = this;
            
            if (dialog.ShowDialog() == true)
            {
                // Aggiungi il libro e aggiorna la DataGrid
                _bookTracker.AddBook(dialog.Title, dialog.Author, dialog.StartDate);
                RefreshBookData();
                
                // Mostra notifica di conferma
                ShowNotification("Studio Manager", "Libro aggiunto con successo!");
            }
        }
        
        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BookDataGrid.SelectedItem as Book;
            if (selectedBook != null)
            {
                // Crea un form di dialogo moderno per modificare un libro
                var dialog = new BookDialog("Modifica Libro", selectedBook);
                dialog.Owner = this;
                
                if (dialog.ShowDialog() == true)
                {
                    // Modifica il libro e aggiorna la DataGrid
                    _bookTracker.EditBook(selectedBook.Id, dialog.Title, dialog.Author, dialog.StartDate);
                    RefreshBookData();
                    
                    // Mostra notifica di conferma
                    ShowNotification("Studio Manager", "Libro modificato con successo!");
                }
            }
            else
            {
                ShowWarningMessage("Seleziona prima un libro da modificare");
            }
        }
        
        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BookDataGrid.SelectedItem as Book;
            if (selectedBook != null)
            {
                var result = System.Windows.MessageBox.Show(
                    "Sei sicuro di voler eliminare questo libro?", 
                    "Conferma eliminazione", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question);
                    
                if (result == MessageBoxResult.Yes)
                {
                    _bookTracker.RemoveBook(selectedBook.Id);
                    RefreshBookData();
                    
                    // Mostra notifica di conferma
                    ShowNotification("Studio Manager", "Libro eliminato con successo!");
                }
            }
            else
            {
                ShowWarningMessage("Seleziona prima un libro da eliminare");
            }
        }
        
        private void CompleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BookDataGrid.SelectedItem as Book;
            if (selectedBook != null)
            {
                // Crea un form di dialogo moderno per completare un libro
                var dialog = new BookCompletionDialog(selectedBook.Title);
                dialog.Owner = this;
                
                if (dialog.ShowDialog() == true)
                {
                    // Segna il libro come completato e aggiorna la DataGrid
                    _bookTracker.CompleteBook(
                        selectedBook.Id, 
                        dialog.CompletionDate,
                        dialog.Rating,
                        dialog.Review
                    );
                    RefreshBookData();
                    
                    // Mostra notifica di conferma
                    ShowNotification("Studio Manager", "Libro completato con successo!");
                }
            }
            else
            {
                ShowWarningMessage("Seleziona prima un libro da completare");
            }
        }
        
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                RefreshBookData();
            }
            else
            {
                BookDataGrid.ItemsSource = _bookTracker.SearchBooks(searchTerm);
            }
        }
        
        #endregion
        
        #region Utility
        
        private void ShowWarningMessage(string message)
        {
            System.Windows.MessageBox.Show(
                message, 
                "Attenzione", 
                MessageBoxButton.OK, 
                MessageBoxImage.Warning);
        }

        private void ShowError(string message)
        {
            System.Windows.MessageBox.Show(message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void UpdateBook(Book book, BookCompletionDialog dialog)
        {
            book.CompletionDate = dialog.CompletionDate;
            book.Rating = dialog.Rating;  // Both are now int?, no conversion needed
            book.Review = dialog.Review;  // Using Review property
        }
        
        #endregion
    }
}