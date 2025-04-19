using System;
using System.Collections.Generic;
using System.Linq;
using System.IO; // Aggiunto per I/O file
using System.Text.Json; // Aggiunto per JSON

namespace Studio
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? Rating { get; set; }
        public string Review { get; set; } = string.Empty;

        public Book(int id, string title, string author, DateTime startDate)
        {
            Id = id;
            Title = title;
            Author = author;
            StartDate = startDate;
            CompletionDate = null;
            Rating = null;
            Review = string.Empty;
        }
    }

    public class BookTracker
    {
        private List<Book> _books;
        private int _nextId;
        private const string FilePath = "books.json"; // Percorso file dati

        public BookTracker()
        {
            _books = new List<Book>();
            _nextId = 1;
            LoadData(); // Carica i dati all'inizializzazione
        }

        // Metodo per aggiungere un libro
        public void AddBook(string title, string author, DateTime startDate)
        {
            Book newBook = new Book(_nextId, title, author, startDate);
            _books.Add(newBook);
            _nextId++;
        }

        // Metodo per modificare un libro
        public bool EditBook(int bookId, string newTitle, string newAuthor, DateTime newStartDate)
        {
            Book? book = _books.Find(b => b.Id == bookId);
            if (book != null)
            {
                book.Title = newTitle;
                book.Author = newAuthor;
                book.StartDate = newStartDate;
                return true;
            }
            return false;
        }

        // Metodo per rimuovere un libro
        public bool RemoveBook(int bookId)
        {
            Book? book = _books.Find(b => b.Id == bookId);
            if (book != null)
            {
                _books.Remove(book);
                return true;
            }
            return false;
        }

        // Metodo per segnare un libro come completato
        public bool CompleteBook(int bookId, DateTime completionDate, int? rating = null, string review = "")
        {
            Book? book = _books.Find(b => b.Id == bookId);
            if (book != null)
            {
                book.CompletionDate = completionDate;
                book.Rating = rating;  // No conversion needed since both are int?
                book.Review = review;
                return true;
            }
            return false;
        }

        // Metodo per cercare libri
        public List<Book> SearchBooks(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return _books.FindAll(b => 
                b.Title.ToLower().Contains(searchTerm) || 
                b.Author.ToLower().Contains(searchTerm) || 
                b.Review.ToLower().Contains(searchTerm)
            );
        }

        // Metodo per ottenere tutti i libri
        public List<Book> GetAllBooks()
        {
            return _books;
        }

        // Metodo per ottenere i libri completati
        public List<Book> GetCompletedBooks()
        {
            return _books.FindAll(b => b.CompletionDate.HasValue);
        }

        // Metodo per ottenere i libri in lettura
        public List<Book> GetCurrentlyReadingBooks()
        {
            return _books.FindAll(b => !b.CompletionDate.HasValue);
        }

        // Aggiorna un libro esistente (sovrascrive i dati del libro con lo stesso Id)
        public bool UpdateBook(Book updatedBook)
        {
            var idx = _books.FindIndex(b => b.Id == updatedBook.Id);
            if (idx >= 0)
            {
                _books[idx] = updatedBook;
                SaveData();
                return true;
            }
            return false;
        }

        // Metodo per salvare i dati su file JSON
        public void SaveData()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_books, options);
                File.WriteAllText(FilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il salvataggio dei libri: {ex.Message}");
            }
        }

        // Metodo per caricare i dati da file JSON
        private void LoadData()
        {
            if (!File.Exists(FilePath))
            {
                _books = new List<Book>();
                _nextId = 1;
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(FilePath);
                var loadedBooks = JsonSerializer.Deserialize<List<Book>>(jsonString);

                if (loadedBooks != null)
                {
                    _books = loadedBooks;
                    _nextId = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
                }
                 else
                {
                     _books = new List<Book>();
                     _nextId = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il caricamento dei libri: {ex.Message}");
                _books = new List<Book>();
                _nextId = 1;
            }
        }
    }
}