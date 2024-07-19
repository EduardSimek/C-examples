


using Newtonsoft.Json;

public class Post2
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Body { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public string Phone { get; set; }
    public string Website { get; set; }
    public Company Company { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public string Suite { get; set; }
    public string City { get; set; }
    public string Zipcode { get; set; }
    public Geo Geo { get; set; }
}

public class Geo
{
    public string Lat { get; set; }
    public string Lng { get; set; }
}

public class Company
{
    public string Name { get; set; }
    public string CatchPhrase { get; set; }
    public string Bs { get; set; }
}

public class Todo
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public bool Completed { get; set; }
}

public delegate void DataFetchedEventHandler(object sender, DataFetchedEventArgs e);
public class DataService
{
    private readonly HttpClient _httpClient;
    private readonly string JSON = "https://jsonplaceholder.typicode.com/";
    public event DataFetchedEventHandler DataFetched;

    public DataService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(JSON) };
    }

    public async Task<List<Post2>> GetPostsAsync()
    {
        var response = await _httpClient.GetStringAsync("posts");
        return JsonConvert.DeserializeObject<List<Post2>>(response);
    }

    public async Task<List<Comment>> GetCommentsAsync()
    {
        var response = await _httpClient.GetStringAsync("comments");
        return JsonConvert.DeserializeObject<List<Comment>>(response);
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var response = await _httpClient.GetStringAsync("users");
        return JsonConvert.DeserializeObject<List<User>>(response);
    }

    public async Task<List<Todo>> GetTodoAsync()
    {
        var response = await _httpClient.GetStringAsync("todos");
        return JsonConvert.DeserializeObject<List<Todo>>(response);
    }

    public async Task<List<Post2>> SearchPostsAsync(string searchTerm) {
        var posts = await GetPostsAsync();
        return posts.Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    public async Task<List<User>> SearchUsersAsync(string searchTerm)
    {
        var users = await GetUsersAsync();
        return users.Where(u => u.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task FetchDataAsync()
    {
        var posts = await GetPostsAsync();
        OnDataFetched(new DataFetchedEventArgs(posts, "Posts"));

        var comments = await GetCommentsAsync();
        OnDataFetched(new DataFetchedEventArgs(comments, "Comments"));

        var users = await GetUsersAsync();
        OnDataFetched(new DataFetchedEventArgs(users, "Users"));

        var todos = await GetTodoAsync();
        OnDataFetched(new DataFetchedEventArgs(todos, "Todos"));
    }

    protected virtual void OnDataFetched(DataFetchedEventArgs e)
    {
        DataFetched?.Invoke(this, e);
    }
}

public class DataFetchedEventArgs : EventArgs
{
    public object Data { get;  }
    public string DataType { get; }

    public DataFetchedEventArgs(object data, string dataType)
    {
        Data = data;
        DataType = dataType;
    }
}

public class DataController
{
    private readonly DataService _dataService;

    public DataController(DataService dataService)
    {
        _dataService = dataService;
        _dataService.DataFetched += OnDataFetched;
    }

    private void OnDataFetched(object sender, DataFetchedEventArgs e)
    {
        Console.WriteLine($"\nData Fetched: {e.DataType}");
        if (e.DataType == "Posts")
        {
            var posts = e.Data as List<Post2>;
            foreach (var post in posts)
            {
                Console.WriteLine($"Post: {post.Title} - {post.Body}");
            }
        }

        else if (e.DataType == "Users") {
            var users = e.Data as List<User>;
            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.Username} - {user.Address}");
            }
        }

        else if (e.DataType == "Comments") {
            var comments = e.Data as List<Comment>;
            foreach (var comment in comments)
            {
                Console.WriteLine($"Comment: {comment.Name} - {comment.Body}");
            }
        }

        else if (e.DataType == "Todos"){
            var todos = e.Data as List<Todo>;
            foreach (var todo in todos)
            {
                Console.WriteLine($"Todo: {todo.Id} - {todo.Title} - {todo.UserId}");
            }
        }
    }

    public async Task RunAsync()
    {
        await _dataService.FetchDataAsync();
    }
}

public class MainProgram
{
    public static async Task Main()
    {
        DataService dataService = new DataService();
        DataController dataController = new DataController(dataService);

        while (true)
        {
            Console.WriteLine("1. Fetch Data");
            Console.WriteLine("2. Search Posts");
            Console.WriteLine("3. Search Users");
            Console.WriteLine("4. Exit");

            var choice = Console.ReadLine();
            switch(choice)
            {
                case "1":
                    await dataController.RunAsync();
                    break;

                case "2":
                    Console.Write("\nEnter search term: ");
                    var postSearchTerm = Console.ReadLine();
                    var foundPosts = await dataService.SearchPostsAsync(postSearchTerm);
                    foreach (var post in foundPosts)
                    {
                        Console.WriteLine($"\nPosts: {post.Title} - {post.Body}");
                    }
                    break;

                case "3":
                    Console.Write("\nEnter search term: ");
                    var userSearchTerm = Console.ReadLine();
                    var foundUsers = await dataService.SearchUsersAsync(userSearchTerm);
                    foreach (var user in foundUsers)
                    {
                        Console.WriteLine($"\nUsers: {user.Name} - {user.Address}");
                    }
                    break;
            }
        }
    }
}