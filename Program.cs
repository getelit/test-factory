using System.Data.SQLite;

// -----------------------------------------------------------

static void FirstTask()
{
    Console.Write("Enter size array: ");
    int.TryParse(Console.ReadLine(), out int arraySize);

    int[] numbers = GenerateRandomArray(arraySize);

    Console.Write("Original: ");
    PrintArray(numbers);

    int[] modified = ModifyArray(numbers, out int max);

    Console.Write("\nModified: ");
    PrintArray(modified);

    Console.WriteLine("\nMax odd on even positions: " + max);

    if (!HasEvenOnOddPositions(modified))
    {
        Console.WriteLine("Array has not even elements on odd positions");
    }

}

static int[] GenerateRandomArray(int size)
{
    Random random = new Random();
    int[] array = new int[size];

    for (int i = 0; i < size; i++)
    {
        // between 1 and 100
        array[i] = random.Next(1, 101);
    }

    return array;
}

static void PrintArray(int[] array)
{
    foreach (var number in array)
    {
        Console.Write(number + "\t");
    }
}

static int[] ModifyArray(int[] numbers, out int max)
{
    int[] modified = new int[numbers.Length];
    max = 0;

    for (int i = 1; i <= numbers.Length; i++)
    {
        if (( i % 2 != 0) && (numbers[i-1] % 2 == 0) )
        {
            modified[i-1] = numbers[i-1] * 2;
        }
        else
        {
            modified[i-1] = numbers[i-1];
            if ( (i % 2 == 0) && (numbers[i-1] % 2 != 0) && (numbers[i-1] > max) )
            {
                max = numbers[i-1];
            }
        }
    }

    return modified;
}

static bool HasEvenOnOddPositions(int[] array)
{
    for (int i = 1; i <= array.Length; i += 2)
    {
        if (array[i-1] % 2 == 0)
        {
            return true;
        }
    }

    return false;
}

//FirstTask();

//-----------------------------------------------------------

static void SecondTask()
{
    SQLiteConnection connection;
    connection = CreateConnection();
    //CreateTables(connection);
    //InsertData(connection);
    ReadData(connection);
}

static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection;
    string connectionString = "Data Source=library.db;Version=3;";
    connection = new SQLiteConnection(connectionString);

    try
    {
        connection.Open();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    return connection;
}

static void CreateTables(SQLiteConnection connection)
{
    SQLiteCommand command;
    string createSQL = "CREATE TABLE IF NOT EXISTS book_authors(" +
        "book_id integer, " +
        "author_id integer, " +
        "CONSTRAINT pk_author_book primary key(book_id, author_id) " +
        ");" +

        "CREATE TABLE IF NOT EXISTS books(" +
        "book_id integer primary key, book_title varchar(100) " +
        "); " +

        "CREATE TABLE IF NOT EXISTS authors(" +
        "author_id integer primary key,     " +
        "first_name varchar(50), " +
        "middle_name varchar(50), " +
        "last_name varchar(50) " +
        ");";

    command = connection.CreateCommand();
    command.CommandText = createSQL;
    command.ExecuteNonQuery();
}

static void InsertData(SQLiteConnection connection)
{
    SQLiteCommand sqliteCommand;
    sqliteCommand = connection.CreateCommand();
    sqliteCommand.CommandText = "INSERT INTO books (book_id, book_title) " +
        "VALUES \r\n  " +
        "(10001, 'To Kill a Mockingbird')," +
        "\r\n  (10002, 'The Great Gatsby')," +
        "\r\n  (10003, 'Pride and Prejudice')," +
        "\r\n  (10004, '1984')," +
        "\r\n  (10005, 'The Catcher in the Rye');" +
        "\r\n  \r\n" +
        "INSERT INTO authors (author_id, first_name, last_name) " +
        "VALUES\r\n  " +
        "(9001, 'Harper', 'Lee')," +
        "\r\n  (9002, 'F. Scott', 'Fitzgerald'),    " +
        "\r\n  (9003, 'Jane', 'Austen')," +
        "\r\n  (9004, 'George', 'Orwell')," +
        "\r\n  (9005, 'J.D.', 'Salinger')," +
        "\r\n  (9006, 'Leo', 'Tolstoy')," +
        "\r\n  (9007, 'Mark', 'Twain')," +
        "\r\n  (9008, 'Charles', 'Dickens');" +
        "\r\n  \r\n" +
        "INSERT INTO book_authors (book_id, author_id) " +
        "VALUES\r\n  (10001, 9001), " +
        "\r\n  (10002, 9002)," +
        "\r\n  (10003, 9003), " +
        "\r\n  (10004, 9004)," +
        "\r\n  (10005, 9005), " +
        "\r\n  (10003, 9001)," +
        "\r\n  (10003, 9004)," +
        "\r\n  (10001, 9002)," +
        "\r\n  (10001, 9003)," +
        "\r\n  (10003, 9002);";
    sqliteCommand.ExecuteNonQuery();
}

static void ReadData(SQLiteConnection connection)
{
    SQLiteDataReader sqliteReader;
    SQLiteCommand sqliteCommand;
    sqliteCommand = connection.CreateCommand();
    sqliteCommand.CommandText = "SELECT b.book_title, COUNT(ba.author_id) AS authors_count " +
                                "FROM books b " +
                                "LEFT JOIN book_authors ba ON b.book_id = ba.book_id " +
                                "GROUP BY b.book_id, b.book_title " +
                                "HAVING COUNT(ba.author_id) >= 3;";
    sqliteReader = sqliteCommand.ExecuteReader();
    while (sqliteReader.Read())
    {
        string book_title = sqliteReader["book_title"].ToString();
        string authors_amount = sqliteReader["authors_count"].ToString();

        Console.WriteLine($"\"{book_title}\" {authors_amount}");
    }
    connection.Close();
}

//SecondTask();

// -----------------------------------------------------------