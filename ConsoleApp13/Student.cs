//using System;
//using System.Net.Http;
//using System.Runtime.CompilerServices;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;
//using DocumentFormat.OpenXml.Drawing.Charts;
//using Newtonsoft.Json;

//public class AsynchProgrMainMethod
//{
//    //public static async Task Main(string[] args)
//    //{
//    //    Console.WriteLine("Titles of fetched posts: ");
//    //    General.DataFetched += OnDataFetched;       //subscribe to the event

//    //    //await FetchAllData.FetchAllDataAsync();     //retrieve all data from JSON file
//    //   await FetchById.FetchBySpecificId();        //retrieve only certain data from JSON file
//    //}

//    private static void OnDataFetched(object sender, DataFetchedEventArgs e)
//    {
        
//        var titles = e.Data.Select(d => d.Title).ToList();
//        titles.ForEach(title => Console.WriteLine(title));  
//    }
 
//}

//public class HandleError
//{
//    public static void ErrorHandling(Exception ex)
//    {
//        Console.Write($"An error occured: {ex.Message}");
//    }
//}

//public class FetchAllData
//{
//    public static readonly HttpClient client = new HttpClient();
//    public static async Task FetchAllDataAsync()
//    {
//        string url = "https://jsonplaceholder.typicode.com/posts";


//        try
//        {
//            string data = await General.FetchDataAsync(url);
//            Console.WriteLine(data);
//        }
//        catch (Exception ex)
//        {
//            HandleError.ErrorHandling(ex);
//        }

//    }

//}

//public class FetchById
//{
//    public static readonly HttpClient client = new HttpClient();
//    public static async Task FetchBySpecificId()
//    {

//        string[] urls = new string[] {
//            "https://jsonplaceholder.typicode.com/posts/1",
//            "https://jsonplaceholder.typicode.com/posts/2",
//            "https://jsonplaceholder.typicode.com/posts/3"
//        };

//        try
//        {
//            Task<string>[] fetchTasks = urls.Select(url => General.FetchDataAsync(url)).ToArray();

//            string[] results = await Task.WhenAll(fetchTasks);

//            foreach (var result in results)
//            {
//                Console.WriteLine(result);
//                Console.WriteLine(new string('.', 50));
//            }
//        }

//        catch (Exception ex)
//        {
//            HandleError.ErrorHandling(ex);
//        }
//    }

//}

//public class General
//{
//    public static readonly HttpClient client = new HttpClient();

//    public delegate void DataFetchedEventHandler(object sender, DataFetchedEventArgs e);
//    public static event DataFetchedEventHandler DataFetched;    

//    public static async Task <string> FetchDataAsync(string url)
//    {
//        HttpResponseMessage response = await client.GetAsync(url);
//        response.EnsureSuccessStatusCode();

//        string responseBody = await response.Content.ReadAsStringAsync();

//        var post = JsonConvert.DeserializeObject<Post>(responseBody);

//        OnDataFetched(new List<Post> { post });

//        return responseBody;
//    }

//    protected static void OnDataFetched(List<Post> data)
//    {
//        DataFetched?.Invoke(null, new DataFetchedEventArgs { Data = data });
//    }
//}



//public class DataFetchedEventArgs : EventArgs
//{
//    public required List<Post> Data { get; set; }
//}

//public class Post
//{
//    public int Id { get; set; }
//    public string Title { get; set; }
//    public string Body { get; set; }
//}