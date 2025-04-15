using System;
using System.Collections.Generic;
using System.Linq;

namespace Studio
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? Rating { get; set; }
        public string Notes { get; set; }

        public Book(int id, string title, string author, DateTime startDate)
        {
            Id = id;
            Title = title;
            Author = author;
            StartDate = startDate;
            CompletionDate = null;
            Rating = null;
            Notes = string.Empty;
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
            Book book = _books.Find(b => b.Id == bookId);
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
            Book book = _books.Find(b => b.Id == bookId);
            if (book != null)
            {
                _books.Remove(book);
                return true;
            }
            return false;
        }

        // Metodo per segnare un libro come completato
        public bool CompleteBook(int bookId, DateTime completionDate, int rating = 0, string notes = "")
        {
            Book book = _books.Find(b => b.Id == bookId);
            if (book != null)
            {
                book.CompletionDate = completionDate;
                book.Rating = rating;
                book.Notes = notes;
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
                (b.Notes != null && b.Notes.ToLower().Contains(searchTerm))
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