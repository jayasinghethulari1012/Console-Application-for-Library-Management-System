using System;
using System.Collections.Generic;
using System.Linq;

class User
{
    public string Name { get; }
    public int ID { get; }
    public string Contact { get; }

    public User(string name, int id, string contact)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ID = id;
        Contact = contact ?? throw new ArgumentNullException(nameof(contact));
    }

    public void DisplayDetails()
    {
        Console.WriteLine("User Details:");
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"ID: {ID}");
        Console.WriteLine($"Contact: {Contact}");
    }
}

class StaffMember : User
{
    public int PositionID { get; set; }

    public StaffMember(string name, int id, string contact)
        : base(name, id, contact)
    {
    }

    public void AssignPosition(int positionID)
    {
        PositionID = positionID;
    }
}

class Member : User
{
    public string Category { get; set; }

    public Member(string name, int id, string contact, string category)
        : base(name, id, contact)
    {
        Category = category ?? throw new ArgumentNullException(nameof(category));
    }

    public void AssignCategory(string category)
    {
        Category = category;
    }
}

class Book
{
    public string Title { get; }
    public string Author { get; }
    public string BookCategory { get; }

    public Book(string title, string author, string bookCategory)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Author = author ?? throw new ArgumentNullException(nameof(author));
        BookCategory = bookCategory ?? throw new ArgumentNullException(nameof(bookCategory));
    }

    public void DisplayBook(string Title, string Author, string BookCategory)
    {
        Console.WriteLine("Book Details:");
        Console.WriteLine($"Book Name: {Title}");
        Console.WriteLine($"Category: {BookCategory}");
        Console.WriteLine($"Author Name: {Author}");
    }
}

class Library
{
    private List<Book> books = new List<Book>();
    private List<Member> members = new List<Member>();
    private List<LendingTransaction> lendingTransactions = new List<LendingTransaction>();

    private string librarianUsername = "librarian";
    private string librarianPassword = "librarian123";
    private string userUsername = "user";
    private string userPassword = "user123";

    public void AddBook()
    {
        Console.Write("Enter the title of the book: ");
        string title = Console.ReadLine();
        Console.Write("Enter the author of the book: ");
        string author = Console.ReadLine();
        Console.Write("Enter the category of the book: ");
        string category = Console.ReadLine();

        Book book = new Book(title, author, category);
        books.Add(book);
    }

    public bool RemoveBook(string title)
    {
        Book bookToRemove = books.FirstOrDefault(b => b.Title == title);
        if (bookToRemove != null)
        {
            books.Remove(bookToRemove);
            return true;
        }
        return false;
    }

    public void DisplayBooks()
    {
        Console.WriteLine("Library Books:");
        foreach (var book in books)
        {
            Console.WriteLine($"Title: {book.Title}, Author: {book.Author}");
        }
    }

    public void RegisterMember()
    {
        Console.Write("Enter the name of the member: ");
        string name = Console.ReadLine();
        Console.Write("Enter the ID of the member: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Enter the contact of the member: ");
        string contact = Console.ReadLine();
        Console.Write("Enter the category of the member: ");
        string category = Console.ReadLine();

        Member member = new Member(name, id, contact, category);
        members.Add(member);
    }

    public bool RemoveMember(int memberId)
    {
        Member memberToRemove = members.FirstOrDefault(m => m.ID == memberId);
        if (memberToRemove != null)
        {
            members.Remove(memberToRemove);
            return true;
        }
        return false;
    }

    public void DisplayMembers()
    {
        Console.WriteLine("Registered Members:");
        foreach (var member in members)
        {
            Console.WriteLine($"Name: {member.Name}, ID: {member.ID}, Category: {member.Category}");
        }
    }

    public bool SearchBookInformation(string title)
    {
        Book bookToSearch = books.FirstOrDefault(b => b.Title == title);
        if (bookToSearch != null)
        {
            Console.WriteLine($"Book Information - Title: {bookToSearch.Title}, Author: {bookToSearch.Author}, Category: {bookToSearch.BookCategory}");
            return true;
        }
        Console.WriteLine("Book not found.");
        return false;
    }

    public bool SearchMemberInformation(int memberId)
    {
        Member memberToSearch = members.FirstOrDefault(m => m.ID == memberId);
        if (memberToSearch != null)
        {
            Console.WriteLine($"Member Information - Name: {memberToSearch.Name}, ID: {memberToSearch.ID}, Contact: {memberToSearch.Contact}, Category: {memberToSearch.Category}");
            return true;
        }
        Console.WriteLine("Member not found.");
        return false;
    }

    public void LendBook(int memberId, string bookTitle, DateTime dueDate)
    {
        Book bookToLend = books.FirstOrDefault(b => b.Title == bookTitle);
        Member memberLendingTo = members.FirstOrDefault(m => m.ID == memberId);
        if (bookToLend != null && memberLendingTo != null)
        {
            LendingTransaction transaction = new LendingTransaction(memberLendingTo, bookToLend, dueDate);
            lendingTransactions.Add(transaction);
            Console.WriteLine($"Book '{bookTitle}' successfully lent to '{memberLendingTo.Name}' with due date {dueDate.ToShortDateString()}");
        }
        else
        {
            Console.WriteLine("Failed to lend the book.");
        }
    }

    public void ReturnBook(int memberId, string bookTitle, DateTime returnDate)
    {
        LendingTransaction transaction = lendingTransactions.FirstOrDefault(t => t.Member.ID == memberId && t.Book.Title == bookTitle && !t.IsReturned);
        if (transaction != null)
        {
            transaction.ReturnBook(returnDate);
            Console.WriteLine($"Book '{bookTitle}' returned by '{transaction.Member.Name}' on {returnDate.ToShortDateString()}");
        }
        else
        {
            Console.WriteLine("Failed to return the book.");
        }
    }

    public void ViewLendingInformation()
    {
        Console.WriteLine("Lending Information:");
        foreach (var transaction in lendingTransactions)
        {
            Console.WriteLine($"Member: {transaction.Member.Name}, Book: {transaction.Book.Title}, Due Date: {transaction.DueDate.ToShortDateString()}, Returned: {transaction.IsReturned}");
        }
    }

    public void DisplayOverdueBooks(DateTime currentDate)
    {
        Console.WriteLine("Overdue Books:");
        foreach (var transaction in lendingTransactions)
        {
            if (!transaction.IsReturned && currentDate > transaction.DueDate)
            {
                Console.WriteLine($"Member: {transaction.Member.Name}, Book: {transaction.Book.Title}, Due Date: {transaction.DueDate.ToShortDateString()}");
            }
        }
    }

    public void CalculateFine(DateTime currentDate)
    {
        double totalFine = 0;
        foreach (var transaction in lendingTransactions)
        {
            if (!transaction.IsReturned && currentDate > transaction.DueDate)
            {
                TimeSpan overdueDuration = currentDate - transaction.DueDate;
                int daysOverdue = overdueDuration.Days;
                double fine = daysOverdue <= 7 ? daysOverdue * 50 : 7 * 50 + (daysOverdue - 7) * 100;
                totalFine += fine;
            }
        }
        Console.WriteLine($"Total Fine: Rs. {totalFine}");
    }

    internal bool Login(string username, string password, int userType)
    {
        // Replace this with your actual authentication logic
        // For example, you can check if the username and password match a predefined list of valid credentials.
        if ((userType == 1 && username == librarianUsername && password == librarianPassword) ||
            (userType == 2 && username == userUsername && password == userPassword))
        {
            return true; // Authentication successful
        }

        return false; // Authentication failed
    }

    internal void PerformLibrarianAction(int actionChoice)
    {
        switch (actionChoice)
        {
            case 1:
                AddBook();
                break;
            case 2:
                Console.Write("Enter the title of the book to remove: ");
                string bookTitleToRemove = Console.ReadLine();
                if (RemoveBook(bookTitleToRemove))
                {
                    Console.WriteLine($"{bookTitleToRemove} removed from the library.");
                }
                else
                {
                    Console.WriteLine($"{bookTitleToRemove} not found in the library.");
                }
                break;
            case 3:
                DisplayBooks();
                break;
            case 4:
                RegisterMember();
                break;
            case 5:
                Console.Write("Enter the ID of the member to remove: ");
                int memberIdToRemove;
                if (int.TryParse(Console.ReadLine(), out memberIdToRemove))
                {
                    if (RemoveMember(memberIdToRemove))
                    {
                        Console.WriteLine($"Member with ID {memberIdToRemove} removed.");
                    }
                    else
                    {
                        Console.WriteLine($"Member with ID {memberIdToRemove} not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input for member ID.");
                }
                break;
            case 6:
                DisplayMembers();
                break;
            case 7:
                Console.Write("Enter the title of the book to search: ");
                string bookTitleToSearch = Console.ReadLine();
                SearchBookInformation(bookTitleToSearch);
                break;
            case 8:
                Console.Write("Enter the ID of the member to search: ");
                int memberIdToSearch;
                if (int.TryParse(Console.ReadLine(), out memberIdToSearch))
                {
                    SearchMemberInformation(memberIdToSearch);
                }
                else
                {
                    Console.WriteLine("Invalid input for member ID.");
                }
                break;
            case 9:
                Console.Write("Enter the member ID: ");
                int memberIdToLend;
                if (int.TryParse(Console.ReadLine(), out memberIdToLend))
                {
                    Console.Write("Enter the title of the book to lend: ");
                    string bookTitleToLend = Console.ReadLine();
                    Console.Write("Enter the due date (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
                    {
                        LendBook(memberIdToLend, bookTitleToLend, dueDate);
                    }
                    else
                    {
                        Console.WriteLine("Invalid due date format.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input for member ID.");
                }
                break;
            case 10:
                Console.Write("Enter the member ID: ");
                int memberIdToReturn;
                if (int.TryParse(Console.ReadLine(), out memberIdToReturn))
                {
                    Console.Write("Enter the title of the book to return: ");
                    string bookTitleToReturn = Console.ReadLine();
                    Console.Write("Enter the return date (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime returnDate))
                    {
                        ReturnBook(memberIdToReturn, bookTitleToReturn, returnDate);
                    }
                    else
                    {
                        Console.WriteLine("Invalid return date format.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input for member ID.");
                }
                break;
            case 11:
                ViewLendingInformation();
                break;
            case 12:
                DisplayOverdueBooks(DateTime.Now);
                break;
            case 13:
                CalculateFine(DateTime.Now);
                break;
            case 14:
                Console.WriteLine("Exiting the Library Management System.");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice. Please select a valid option.");
                break;
        }
    }
}

class LendingTransaction
{
    public Member Member { get; }
    public Book Book { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnDate { get; private set; }
    public bool IsReturned => ReturnDate.HasValue;

    public LendingTransaction(Member member, Book book, DateTime dueDate)
    {
        Member = member;
        Book = book;
        DueDate = dueDate;
    }

    public void ReturnBook(DateTime returnDate)
    {
        ReturnDate = returnDate;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create a library
        Library library = new Library();

        while (true)
        {
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Book List");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Register");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    // Implement logic to display the list of books
                    break;

                case 2:
                    Console.WriteLine("Login as:");
                    Console.WriteLine("1. Librarian");
                    Console.WriteLine("2. User");
                    Console.Write("Enter who you are: ");

                    int userType = int.Parse(Console.ReadLine());

                    Console.Write("Enter Username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter Password: ");
                    string password = Console.ReadLine();

                    if (library.Login(username, password, userType))
                    {
                        if (userType == 1)
                        {
                            Console.WriteLine("\nLibrary Management System Menu:");
                            Console.WriteLine("1. Add Book");
                            Console.WriteLine("2. Remove Book");
                            Console.WriteLine("3. Display Books");
                            Console.WriteLine("4. Register Member");
                            Console.WriteLine("5. Remove Member");
                            Console.WriteLine("6. Display Members");
                            Console.WriteLine("7. Search Book Information");
                            Console.WriteLine("8. Search Member Information");
                            Console.WriteLine("9. Lend Book");
                            Console.WriteLine("10. Return Book");
                            Console.WriteLine("11. View Lending Information");
                            Console.WriteLine("12. Display Overdue Books");
                            Console.WriteLine("13. Calculate Fine");
                            Console.WriteLine("14. Exit");
                            Console.WriteLine("Logged in as Librarian.");
                            Console.Write("Enter your choice: ");

                            int actionChoice = int.Parse(Console.ReadLine());
                            library.PerformLibrarianAction(actionChoice);
                        }
                        else if (userType == 2)
                        {
                            Console.WriteLine("Logged in as User.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid user type.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Login failed. Please try again.");
                    }
                    break;

                case 3:
                    // Implement logic for user registration
                    library.RegisterMember();
                    break;

                case 4:
                    Console.WriteLine("Exiting the application.");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}




