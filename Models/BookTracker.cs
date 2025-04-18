using System;
using System.Collections.Generic;
using System.Linq;

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

        public BookTracker()
        {
            _books = new List<Book>();
            _nextId = 1;
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
    }
}