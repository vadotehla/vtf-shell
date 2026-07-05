using System.Collections.Frozen;
using System.Text.Json;
// Global Variables

string input = "";
string username = null;
bool cont = true;
List<string> Builtins = new(["exit", "type"]);
FrozenDictionary<string, int> Users = JsonSerializer.Deserialize<Dictionary<string, int>>("""
    {
        "u1t": 10,
        "luka": 10
    }
""")!.ToFrozenDictionary();

static string[] ParseInput(string inp)
{
    return inp.Split(" ");
}

/* Commands to be implemented:
    - exit | quit (exits)
    - type (checks location)
    - open (prints a file's content)
    - list (lists files in the working directory)
    - whoami (prints current user and information about them)
    - login (changes changes current user)
*/
while (cont)
{
    Console.Write($"|{username ?? "guest"}|>> (no-workspace) ");

    input = Console.ReadLine();
    string[] Parsed = ParseInput(input.ToLower());

    if (Parsed.Length > 0)
    {
        string command = Parsed.ElementAt(0);
        switch (command)
        {
            case "quit":
            case "exit":
                cont = false;
                break;
            case "type":
                {
                    if (Parsed.Length > 1)
                    {
                        string arg = Parsed.ElementAt(1);
                        if (Builtins.Contains(arg))
                        {
                            Console.WriteLine($"{arg} is builtin");
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"{arg} unknown");
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("type expected 1 argument (command)");
                        break;
                    }
                }
            case "whoami":
                {
                    Console.Write("|admin.bot|>> ");
                    if (typeof(username) == "string" && Users.Contains(username))
                    {
                        int clearance = Users.GetValueOrDefault(username, 0);
                        Console.WriteLine($"You are logged in as {username}, with a clearance of {clearance}.");
                    }
                    else
                    {
                        Console.WriteLine("You are not logged in or you are not a valid user.");
                    }
                }
            default:
                Console.WriteLine($"{command} unknown");
                break;
        }
    }
    else continue;
}
