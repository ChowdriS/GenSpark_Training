namespace May20_Tasks
{
    public class InstagramPosts 
    {
        public string? captions { get; set; }
        public int likes { get; set; }
        public InstagramPosts(string? captions, int likes) 
        {
            this.captions = captions;
            this.likes = likes;
        }
    }


    public class UserInput
    {
        public int getUserInt()
        {
            int num;
            while(!int.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Invalid Input! Please Enter Int value...");
            }
            return num;
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            UserInput input = new UserInput();
            Console.WriteLine("Instagram Posts!!");
            Console.Write("No of Users : ");
            int users = input.getUserInt();

            InstagramPosts[][] Igusers = new InstagramPosts[users][];

            for (int i = 0; i < users; i++) 
            {
                Console.Write($"No of posts for User {i+1} : ");
                int posts = input.getUserInt();
                Igusers[i] = new InstagramPosts[posts];

                for(int j = 0; j < posts; j++)
                {
                    Console.WriteLine($"Please Enter the Details for Post {j+1}");
                    Console.Write("Enter the Caption : ");
                    string? caption = Console.ReadLine();
                    Console.Write("No of Likes : ");
                    int likes = input.getUserInt();

                    Igusers[i][j] = new InstagramPosts(caption,likes);
                }
            }
            Console.WriteLine("--------------------------------------");
            for (int i = 0; i < users; i++)
            {
                Console.WriteLine($"Details of User {i+1}");
                int posts = Igusers[i].Length;

                for(int j = 0; j < posts; j++)
                {
                    Console.WriteLine($"Detail of Post {j + 1} => Caption : {Igusers[i][j].captions} | Likes : {Igusers[i][j].likes} ");
                }

            }

            Console.WriteLine("Thank you!!");

        }
    }
}
