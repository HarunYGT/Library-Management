using System;
using System.Collections.Generic;
using System.IO;

namespace LibraryApp
{
    class Program
    {
        static void Main()
        {
            Library library = new Library();
            Book book1 = new Book() { Title = "The Da Vinci Code", Author = "Dan Brown", ISBN = 15355493, CopyNum = 5, BorrowCopyNum = 0 };
            Book book2 = new Book() { Title = "Harry Potter and the Prisoner of Azkaban", Author = "J. K. Rowling", ISBN = 19475826, CopyNum = 10, BorrowCopyNum = 0 };
            Book book3 = new Book() { Title = "Heidi", Author = "Johanna Spyri", ISBN = 87531942, CopyNum = 8, BorrowCopyNum = 0 };
            Book book4 = new Book() { Title = "The Hobbit", Author = "J. R. R. Tolkien", ISBN = 63794581, CopyNum = 3, BorrowCopyNum = 0 };
            if (!File.Exists("library.txt")) 
            {
                library.SaveTheFile();
            }
            library.Books.Add(book1);
            library.Books.Add(book2);
            library.Books.Add(book3);
            library.Books.Add(book4);
           
            while (true)
            {
                Console.Write("Welcome The Kultur Library.!");
                Console.WriteLine("Please Select What Do You Want?");
                Console.WriteLine("1. Add a new book");
                Console.WriteLine("2. Show all books");
                Console.WriteLine("3. Search a book");
                Console.WriteLine("4. Borrow a book");
                Console.WriteLine("5. Return a book");
                Console.WriteLine("6. Show Expired Books");
                Console.WriteLine("0. Exit The Menu");
                string choose = Console.ReadLine();

                switch (choose)
                {
                    case "1":
                        library.AddBook();
                        break;
                    case "2":
                        library.ShowAllBooks();
                        break;
                    case "3":
                        library.SearchBook();
                        break;
                    case "4":
                        library.BorrowBook();
                        break;
                    case "5":
                        library.ReturnBook();
                        break;
                    case "6":
                        library.ExpiredBooks();
                        break;
                    case "0":
                        return;
                }
                Console.Clear();
                library.SaveTheFile();
            }
        }
    }
    class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int ISBN { get; set; }
        public int CopyNum { get; set; }
        public int BorrowCopyNum { get; set; }
        public int BorrowNum { get; set; }
        public DateTime borrowDate;
        public bool isBorrowed;
    }
    class Library
    {
        public List<Book> Books = new List<Book>();

        public void SaveTheFile()
        {
            if (!File.Exists("library.txt")) 
            {
                using (StreamWriter lib = new StreamWriter("library.txt"))
                {
                    foreach (var str in Books)
                    {
                        lib.WriteLine("Book Title :" + str.Title + "\nBook Author : " + str.Author + "\nBook ISBN: " + str.ISBN + "\nNumber of coppies: " + str.CopyNum + "\nNumber borrowed :" + str.BorrowCopyNum);
                    }
                }
            }
        }

        public void AddBook()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Please Enter Book Title: ");
                string title = Console.ReadLine();
                Console.WriteLine("Please Enter Book Author: ");
                string author = Console.ReadLine();
                Console.WriteLine("Please Enter ISBN: ");
                int isbn = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Please Enter Copy Number: ");
                int copyNum = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                Book newBook = new Book()
                {
                    Title = title,
                    Author = author,
                    ISBN = isbn,
                    CopyNum = copyNum,
                    BorrowCopyNum = 0,
                };
                Books.Add(newBook);
                using (StreamWriter lib = new StreamWriter("library.txt", true))
                {
                    lib.WriteLine("Book Title :" + newBook.Title + "\nBook Author : " + newBook.Author +
                        "\nBook ISBN: " + newBook.ISBN + "\nNumber of coppies: " + newBook.CopyNum + "\nNumber borrowed :" + newBook.BorrowCopyNum);
                    lib.Close();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Please insert an integer for ISBN.");
                throw;
            }
        }
        public void ShowAllBooks()
        {
            Console.Clear();
            Console.WriteLine("In Library: ");
            using (StreamReader lib = new StreamReader("library.txt"))
            {
                string line;

                while ((line = lib.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
            Console.WriteLine("Press any key to return to the main menu");
            Console.ReadLine();
        }
        public void SearchBook()
        {
            Console.Clear();
            Console.Write("Please enter the title or author you want to search : ");
            string entry = Console.ReadLine();
            foreach (var book in Books)
            {
                if (entry == book.Author || entry == book.Title)
                {
                    Console.WriteLine("Book Title : " + book.Title);
                    Console.WriteLine("Book Author : " + book.Author);
                    Console.WriteLine("Book ISBN : " + book.ISBN);
                    Console.WriteLine("Number of coppies: " + book.CopyNum);
                    Console.WriteLine("Number borrowed : " + book.BorrowCopyNum);
                }
                else
                {
                    Console.WriteLine("No Results Found.");
                }
            }
        }
        public void BorrowBook()
        {
            Console.Clear();
            Console.Write("Please enter the title of the book you want to give: ");
            string entry = Console.ReadLine();
            foreach (var book in Books)
            {
                if (entry == book.Title)
                {
                    book.CopyNum--;
                    book.BorrowCopyNum++;
                    var rand = new Random();
                    book.BorrowNum = rand.Next(1001, 10000);
                    book.isBorrowed = true;
                    book.borrowDate = DateTime.Today;
                    Console.WriteLine("Borrow Date: " + DateTime.Today.ToString() + "\nDeadline : " + DateTime.Today.AddDays(30).ToString()); ;
                    Console.WriteLine("Borrow Book Return Code : " + book.BorrowNum + "\nPlease save this code somewhere.");
                    Console.Read();
                }
            }
        }
        public void ReturnBook()
        {
            try
            {
                Console.Clear();
                Console.Write("Please enter the borrow number of the book you want to return: ");
                int entry = Convert.ToInt32(Console.ReadLine());
                foreach (var book in Books)
                {
                    if (entry == book.BorrowNum)
                    {
                        book.CopyNum++;
                        book.BorrowCopyNum--;
                    }
                    else
                    {
                        Console.WriteLine("No results found this number.");
                        Console.ReadLine();
                        break;
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Please insert an integer.");
                Console.ReadLine();
            }

        }
        public void ExpiredBooks()
        {
            foreach (var book in Books)
            {
                if (book.isBorrowed && book.borrowDate == DateTime.Today.AddDays(-30))
                {
                    Console.WriteLine("Expired books:" + book.Title + "\nBook Borrow date: " + book.borrowDate + "\nBook Deadline: " + book.borrowDate.AddDays(30));
                    Console.ReadLine();
                    return;
                }
                else
                {
                    Console.WriteLine("No expired books.");
                    Console.ReadLine();
                    return;
                }

            }
        }
    }
}
