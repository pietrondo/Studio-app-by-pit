using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Studio;

namespace Studio.ViewModels
{
    public class BookItemViewModel : INotifyPropertyChanged
    {
        private readonly Book _book;
        private readonly BookTracker _bookTracker;
        private bool _isCompleted;
        private int? _rating;
        private DateTime? _completionDate;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? BookUpdated;

        public string Title => _book.Title;
        public string Author => _book.Author;
        public DateTime StartDate => _book.StartDate;
        public DateTime? CompletionDate
        {
            get => _completionDate;
            set
            {
                if (_completionDate != value)
                {
                    _completionDate = value;
                    OnPropertyChanged();
                    UpdateBookCompletion();
                }
            }
        }
        public int? Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged();
                    UpdateBookCompletion();
                }
            }
        }
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                    UpdateBookCompletion();
                }
            }
        }
        public int Id => _book.Id;

        public BookItemViewModel(Book book, BookTracker bookTracker)
        {
            _book = book ?? throw new ArgumentNullException(nameof(book));
            _bookTracker = bookTracker ?? throw new ArgumentNullException(nameof(bookTracker));
            _isCompleted = book.CompletionDate.HasValue;
            _rating = book.Rating;
            _completionDate = book.CompletionDate;
        }

        private void UpdateBookCompletion()
        {
            _book.CompletionDate = this.IsCompleted ? (this.CompletionDate ?? DateTime.Now) : (DateTime?)null;
            _book.Rating = this.Rating;
            _bookTracker.UpdateBook(_book);
            BookUpdated?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
