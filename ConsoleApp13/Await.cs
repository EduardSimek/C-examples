using DocumentFormat.OpenXml.Bibliography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp13
{
    public class MainProgram
    {
        public static readonly string JSONPosts = "https://jsonplaceholder.typicode.com/posts";
        public static readonly string JSONComments = "https://jsonplaceholder.typicode.com/comments";

        //public static async Task Main()
        //{

        //    try {
        //        List<Post> posts = await FetchingPosts.FetchPostsAsync<Post>(JSONPosts);
        //        List<Comment> comments = await FetchingPosts.FetchPostsAsync<Comment>(JSONComments);
        //        LinqOperations.PerformLinq(posts, comments);
        //    }
        //    catch (Exception ex) {
        //        HandleError.ErrorCheck(ex);
        //    }
        //}
    }

    public class HandleError
    {
        public static void ErrorCheck(Exception ex)
        {
            Console.WriteLine($"An error occured: {ex.Message}");
        }
    }



    public class FetchingPosts
    {
        public static async Task<List<T>> FetchPostsAsync<T>(string url)
        {
            using HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<T>>(responseBody);
        }

    }


    public class LinqOperations
    {
        public static void PerformLinq(List<Post> posts, List<Comment> comments)
        {
            // Query 1: Get all posts with userId = 1
            var userPosts = from post in posts where post.UserId == 1 select post;
            Console.WriteLine("Posts By User 1: ");
            foreach (var post in userPosts)
            {
                Console.WriteLine($"Title: {post.Title}");
            }


            //Query 2: Count posts by each user
            var postCounts = from post in posts
                             group post by post.UserId into userGroup
                             select new { UserId = userGroup.Key, Count = userGroup.Count() };
            Console.WriteLine("\nNumber of posts By Each User: ");
            foreach (var post in postCounts)
            {
                Console.WriteLine($"User: {post.UserId}: {post.Count} posts");
            }

            // Query 3: Find the longest post title
            var longestTitle = posts.OrderByDescending(p => p.Title.Length).FirstOrDefault();
            Console.WriteLine($"\nLongest Post Title: {longestTitle?.Title}");

            // Query 4: Group posts by title length
            var postsByTitleLength = from post in posts
                                     group post by post.Title.Length into lengthGroup
                                     orderby lengthGroup.Key
                                     select new { Length = lengthGroup.Key, Posts = lengthGroup };

            Console.WriteLine("\nPosts Grouped by Title Length:");
            foreach (var group in postsByTitleLength)
            {
                Console.WriteLine($"Title Length: {group.Length}");
                foreach (var post in group.Posts)
                {
                    Console.WriteLine($"  Title: {post.Title}");
                }
            }

            //Query 5: Get all posts with more than 5 comments 
            var popularPosts = from post in posts
                               join comment in comments on post.Id equals comment.PostId into postComments
                               where postComments.Count() > 5
                               select new { post.Title, CommentCount = postComments.Count() };
            Console.WriteLine("Posts with More Than 5 Comments: ");
            foreach (var item in popularPosts)
            {
                Console.WriteLine($"Title: {item.Title}, Comments: {item.CommentCount}");
            }

            // Query 2: Average number of comments per post
            var averageComments = comments.GroupBy(c => c.PostId)
                                          .Select(g => new { PostId = g.Key, CommentCount = g.Count() })
                                          .Average(g => g.CommentCount);

            Console.WriteLine($"\nAverage Number of Comments per Post: {averageComments:F2}");

            // Query 3: Find the post with the highest number of comments
            var mostCommentedPost = (from post in posts
                                     join comment in comments on post.Id equals comment.PostId into postComments
                                     orderby postComments.Count() descending
                                     select new { post.Title, CommentCount = postComments.Count() }).FirstOrDefault();

            Console.WriteLine($"\nMost Commented Post: {mostCommentedPost?.Title}, Comments: {mostCommentedPost?.CommentCount}");

            // Query 4: Group comments by post and select posts with specific keywords
            var keywordPosts = from post in posts
                               join comment in comments on post.Id equals comment.PostId into postComments
                               where post.Title.Contains("dolor") || post.Title.Contains("qui")
                               select new { post.Title, Comments = postComments };

            Console.WriteLine("\nPosts with Titles Containing 'dolor' or 'qui':");
            foreach (var post in keywordPosts)
            {
                Console.WriteLine($"Title: {post.Title}, Number of Comments: {post.Comments.Count()}");
            }

            // Query 5: Select distinct email addresses from the comments
            var distinctEmails = comments.Select(c => c.Email).Distinct();

            Console.WriteLine("\nDistinct Emails from Comments:");
            foreach (var email in distinctEmails)
            {
                Console.WriteLine(email);
            }
        }

      
    }

    public class Post
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<Comment> Comments { get; internal set; }
    }

    public class Comment
    {
        public int PostId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }
}
