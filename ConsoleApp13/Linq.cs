using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp13
{
    public class Program
    {
        //public static void Main()
        //{
        //    List<Author> authors = new List<Author> { 
        //        new Author {AuthorId = 1, FirstName = "Jožo", LastName = "Jožový", Age = 22},
        //        new Author {AuthorId = 2, FirstName = "Anton", LastName = "Fico", Age = 54},
        //        new Author {AuthorId = 3, FirstName = "Silvester", LastName = "Zidane", Age = 22},
        //        new Author {AuthorId = 4, FirstName = "Ferenc", LastName = "Suchý", Age = 42}
        //    };

        //    List<Publisher> publishers = new List<Publisher> {
        //        new Publisher {PublisherId = 1, Name = "Ja soms ani vizualne"},
        //        new Publisher {PublisherId = 2, Name = "Do psej matere"}
        //    };

        //    List<Book> books = new List<Book> { 
        //        new Book {BookId = 1, Title = "Levi kral", AuthorId = 1, PublisherId = 1, PublishedYear = 2000},
        //        new Book {BookId = 2, Title = "Hary Pota 2", AuthorId = 1, PublisherId = 2, PublishedYear = 2011},
        //        new Book {BookId = 3, Title = "Sam doma 3", AuthorId = 4, PublisherId = 3, PublishedYear = 1999},
        //        new Book {BookId = 4, Title = "Prci, prci, prcicky 2", AuthorId = 3, PublisherId = 2, PublishedYear = 2010}
        //    };

        //    PerformVariousOperations(authors, publishers, books);


        //}

        public static void PerformVariousOperations(List<Author> authors, List<Publisher> publishers, List<Book> books)
        {
            //Basic Queries 
            GetAllBooks(books);
            GetBookByYear(books, 2000);
            GetOrderBooksByYear(books);
            GetBooksWithAuthors(books, authors);
            GetLatestPublishedBooks(books);
            DistincsPublishers(books, publishers);
            AverageYear(books);
        }

        private static void DistincsPublishers(List<Book> books, List<Publisher> publishers)
        {
            var distincsPublishers = (from book in books select book.PublisherId).Distinct();

            var distinctPublisherName = from publisher in publishers where distincsPublishers.Contains(publisher.PublisherId) select publisher.Name;

            Console.WriteLine("\nDistinct Publishers of Books: ");
            foreach (var publisherName in distinctPublisherName)
            {
                Console.WriteLine(publisherName);
            }
        }

        private static void GetLatestPublishedBooks(List<Book> books)
        {
            var latestBook = (from book in books orderby book.PublishedYear descending select book).FirstOrDefault();

            Console.WriteLine("\nLatest published books: ");
            if (latestBook != null)
            {
                Console.WriteLine($"Title: {latestBook.Title}, Year: {latestBook.PublishedYear}");
            }
        }

        public static void GetAllBooks(List<Book> books)
        {
            var retrieveAllBooks = from book in books select book;

            Console.WriteLine("All books: ");
            foreach(var book in retrieveAllBooks)
            {
                Console.WriteLine($"Title: {book.Title}, Year: {book.PublishedYear}");
            }

            Console.WriteLine(new string('-', 50));
        }

        public static void GetBookByYear(List<Book> books, int year)
        {
            var thresholdYear = from book in books where book.PublishedYear > 2000 select book;

            Console.WriteLine("\nSelect All books after threshold value: ");
            foreach (var book in thresholdYear)
            {
                Console.WriteLine($"Title: {book.Title}, Year: {book.PublishedYear}");
            }
            Console.WriteLine(new string('-', 50));
        }

        public static void GetOrderBooksByYear(List<Book> books)
        {
            var orderAllBooks = from book in books orderby book.PublishedYear select book;

            Console.WriteLine("\nOrdered Books by Year: ");
            foreach (var book in orderAllBooks)
            {
                Console.WriteLine($"Title: {book.Title}, Year: {book.PublishedYear}");
            }
            Console.WriteLine(new string('-', 50));
        }

        public static void GetBooksWithAuthors(List<Book> books, List<Author> authors)
        {
            var booksWithAuthors = from book in books
                                   join author in authors on book.AuthorId equals author.AuthorId
                                   select new { book.Title, AuthorName = author.FirstName, book.PublishedYear };

            Console.WriteLine("\nBooks with authors: ");
            foreach(var item in booksWithAuthors)
            {
                Console.WriteLine($"Title: {item.Title}, Author: {item.AuthorName}, Year: {item.PublishedYear}");
            }
            Console.WriteLine(new string('-', 50));
        }

        public static void AverageYear(List<Book> books) {

            var averageYear = books.Average(b => b.PublishedYear);

            Console.WriteLine($"\nAverage Year is: {averageYear}");
        }





    }

    public class Author
    {
        public int AuthorId { get; set; }
        public string  FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

    }

    public class Publisher
    {
        public int PublisherId { get; set; }
        public string Name { get; set; }
    }

    public class Book
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public int PublisherId { get; set; }
        public int PublishedYear { get; set; }
    }

    
}
