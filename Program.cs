using System.Collections.Frozen;
using System.Text.Json;
// Global Variables

string input = "";
string? usn = null;
string? workspace = null;
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

static string Paint(string text, int? fg = null, int? bg = null, bool bold = false, bool underline = false)
{
    var codes = new List<string>();

    if (bold) codes.Add("1");
    if (underline) codes.Add("4");
    if (fg.HasValue) codes.Add(fg.Value.ToString());
    if (bg.HasValue) codes.Add(bg.Value.ToString());

    if (codes.Count == 0)
        return text;

    return $"\x1b[{string.Join(";", codes)}m{text}\x1b[0m";
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
    Console.Write($"|{Paint(usn ?? "guest", fg = 36)}|>> ({Paint(workspace ?? "no-workspace", fg = 31)}) ");

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
                    if (usn is not null && Users.ContainsKey(usn))
                    {
                        int clearance = Users.GetValueOrDefault(usn, 0);
                        Console.WriteLine($"You are logged in as {usn}, with a clearance of {clearance}.");
                    }
                    else
                    {
                        Console.WriteLine("You are not logged in or you are not a valid user.");
                    }
                    break;
                }
            default:
                Console.WriteLine($"{command} unknown");
                break;
        }
    }
    else continue;
}
