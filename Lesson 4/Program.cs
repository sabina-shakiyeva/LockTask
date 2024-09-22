using Bogus;
using Lesson_4;
using System.Text.Json;

namespace Lesson4;
class Program
{
    static List<User> _users = [];
    static object obj = new object();
    static void Main(string[] args)
    {
        

        for (int i = 0; i < 5; i++)
        {
            var faker = new Faker<User>();
            var users = faker.RuleFor(u => u.Name, f => f.Person.FirstName)
                .RuleFor(u => u.Surname, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.DateOfBirth, f => f.Person.DateOfBirth)
                .Generate(50);


            var json = JsonSerializer.Serialize(users);
            File.WriteAllText($"{i}.json", json);
        }
        string[] Files = { "0.json", "1.json", "2.json", "3.json", "4.json" };

        Console.WriteLine("1)Single or 2)Multiple threads?:");
        

        short choice;
        choice=short.Parse(Console.ReadLine());
       
        if (choice == 1) {
            foreach (var file in Files)
            {
                ReadFile(file);
            }

        }
        else if (choice == 2)
        {
            foreach(var file in Files)
            {
                Thread thread = new Thread(() => ReadFile(file));
                thread.Start();
                thread.Join();
            }
        }

        for (int i = 0; i < _users.Count; i++)
        {
            Console.WriteLine($"{i + 1}) {_users[i].Name} {_users[i].Surname}, {_users[i].Email}, {_users[i].DateOfBirth}");
        }

    }

    static void ReadFile(string file)
    {
        var json=File.ReadAllText(file);
        var users=JsonSerializer.Deserialize<List<User>>(json);

        lock (obj)
        {
            _users.AddRange(users);
        }
    }




    
}