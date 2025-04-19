using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input; // Per ICommand (se necessario in futuro)
using Studio; // Assicurati che il namespace dei modelli sia corretto

namespace Studio.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly TaskManager _taskManager;
        private readonly ExamManager _examManager;
        private readonly BookTracker _bookTracker;

        public ObservableCollection<TaskItemViewModel> UpcomingTasks { get; } = new();
        public ObservableCollection<ExamItemViewModel> NextExams { get; } = new(); // Da implementare ExamItemViewModel
        public ObservableCollection<BookItemViewModel> CurrentBooks { get; } = new(); // Da implementare BookItemViewModel

        // Evento per notificare i cambiamenti delle proprietà alla UI
        public event PropertyChangedEventHandler? PropertyChanged;

        public DashboardViewModel(TaskManager taskManager, ExamManager examManager, BookTracker bookTracker)
        {
            _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            _examManager = examManager ?? throw new ArgumentNullException(nameof(examManager));
            _bookTracker = bookTracker ?? throw new ArgumentNullException(nameof(bookTracker));

            // Inizialmente potresti voler caricare i dati qui,
            // ma è meglio farlo in un metodo separato chiamato dopo l'inizializzazione della UI (es. Window_Loaded)
            // LoadData(); 
        }

        // Metodo per caricare/aggiornare tutti i dati della dashboard
        public void LoadData()
        {
            LoadUpcomingTasks();
            LoadNextExams();
            LoadCurrentBooks();
            // Notifica che le collection potrebbero essere cambiate (anche se ObservableCollection lo fa per gli elementi)
            OnPropertyChanged(nameof(UpcomingTasks));
            OnPropertyChanged(nameof(NextExams));
            OnPropertyChanged(nameof(CurrentBooks));
        }

        private void LoadUpcomingTasks()
        {
            UpcomingTasks.Clear();
            try
            {
                var tasks = _taskManager.GetAllTasks()
                    .Where(t => !t.IsCompleted && t.DueDate >= DateTime.Today && t.DueDate <= DateTime.Today.AddDays(7)) // Task futuri entro 7 giorni
                    .OrderBy(t => t.DueDate);

                foreach (var task in tasks)
                {
                    // Passa una callback o il manager stesso per gestire l'aggiornamento
                    var taskVM = new TaskItemViewModel(task, _taskManager); 
                    taskVM.TaskUpdated += (s, e) => LoadUpcomingTasks(); // Ricarica la lista se un task viene aggiornato
                    UpcomingTasks.Add(taskVM);
                }
            }
            catch (Exception ex)
            {
                // Gestire l'eccezione (es. log, messaggio all'utente)
                Console.WriteLine($"Errore durante il caricamento dei task: {ex.Message}");
            }
        }

        private void LoadNextExams()
        {
            NextExams.Clear();
            try
            {
                 var exams = _examManager.GetAllExams()
                     .Where(e => e.ExamDate >= DateTime.Today) // Esami futuri
                     .OrderBy(e => e.ExamDate)
                     .Take(3); // Limita ai prossimi 3

                 foreach (var exam in exams)
                 {
                     var examVM = new ExamItemViewModel(exam, _examManager);
                     examVM.ExamUpdated += (s, e) => LoadNextExams();
                     NextExams.Add(examVM);
                 }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il caricamento degli esami: {ex.Message}");
            }
        }

        private void LoadCurrentBooks()
        {
            CurrentBooks.Clear();
            try
            {
                 var books = _bookTracker.GetAllBooks()
                     .Where(b => !b.CompletionDate.HasValue) // Libri non completati
                     .OrderBy(b => b.Title);

                 foreach (var book in books)
                 {
                     var bookVM = new BookItemViewModel(book, _bookTracker);
                     bookVM.BookUpdated += (s, e) => LoadCurrentBooks();
                     CurrentBooks.Add(bookVM);
                 }
            }
             catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il caricamento dei libri: {ex.Message}");
            }
        }

        // Metodo helper per invocare PropertyChanged
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}