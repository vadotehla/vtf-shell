using System.Collections.Frozen;
using System.Security.Authentication;
using System.Text.Json;

{
    Console.Title = "VTF International, Secure Interface";
    Console.Clear();
}

// Global Variables

string input = "";
string? usn = Environment.UserName;
string? workspace = null;
bool cont = true;
List<string> Builtins = new(["exit", "type", "clear"]);
List<string> ServerContacts = new(["whoami", "login", "open", "list", "move"]);
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

{
    if (usn is not null && Users.ContainsKey(usn))
        workspace = "home";
}

/* Commands to be implemented:
    - exit | quit (exits)
    - type (checks location)
    - clear (clears screen)
    - open (prints a file's content)
    - list (lists files in the working directory)
    - whoami (prints current user and information about them)
    - login (changes changes current user)
    - move (changes to a different working directory)
*/
while (cont)
{
    Console.Write($"|{Paint(usn ?? "guest", fg: 36)}|>> ({Paint(workspace ?? "no-workspace", fg: 31)}) ");

    input = Console.ReadLine() ?? "";
    string[] Parsed = ParseInput(input.ToLower() ?? "");

    if (Parsed.Length > 0)
    {
        string command = Parsed.ElementAt(0);
        switch (command)
        {
            case "quit":
            case "exit":
                cont = false;
                break;
            case "clear":
                Console.Clear();
                break;
            case "type":
                {
                    if (Parsed.Length > 1)
                    {
                        string arg = Parsed.ElementAt(1);
                        if (Builtins.Contains(arg))
                        {
                            Console.WriteLine($"|{Paint("server", fg: 30, bg: 47)}|>> {Paint(arg, bg: 41, fg: 37)} is builtin");
                        }
                        else if (ServerContacts.Contains(arg))
                        {
                            Console.WriteLine($"|{Paint("server", fg: 30, bg: 47)}|>> {Paint(arg, bg: 41, fg: 37)} is contact");
                        }
                        else
                        {
                            Console.Error.WriteLine($"|{Paint("server", fg: 30, bg: 47)}|>> ERROR: {Paint(arg, bg: 41, fg: 37)} is {Paint("unknown", fg: 31)}");
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine($"|{Paint("server", fg: 30, bg: 47)}|>> ERROR: {Paint(command, bg: 41, fg: 37)}type expected 1 argument (command)");
                    }
                    break;
                }
            case "whoami":
                {
                    Console.Write($"|{Paint("admin.bot", fg: 31, bold: true)}|>> ");
                    if (usn is not null && Users.ContainsKey(usn))
                    {
                        int clearance = Users.GetValueOrDefault(usn, 0);
                        Console.WriteLine($"You are logged in as {Paint(usn, fg: 36)}, with a clearance of {Paint(clearance.ToString(), fg: 34)}.");
                    }
                    else
                    {
                        if (usn is null)
                        {
                            Console.WriteLine("You are not logged in.");
                        }
                        else
                        {
                            Console.WriteLine("You are not a valid user.");
                        }
                    }
                    break;
                }
            case "login":
                {
                    Console.Write($"|{Paint("admin.bot", fg: 31, bold: true)}|>> enter username: ");
                    string inp = Console.ReadLine() ?? "guest";

                    usn = inp;
                    if (Users.ContainsKey(usn ?? "guest"))
                    {
                        workspace = "home";
                    }
                    else
                    {
                        workspace = "no-workspace";
                    }
                    break;
                }
            case "open":
            case "list":
                Console.WriteLine($"|{Paint("admin.bot", fg: 31, bold: true)}|>> unimplemented");
                break;
            case "move":
                {
                    if (Users.ContainsKey(usn ?? "guest"))
                    {
                        if (Parsed.Length > 1) workspace = Parsed.ElementAt(1);
                        else workspace = "home";
                    }
                    else workspace = "no-workspace";
                }
                break;
            default:
                Console.Error.WriteLine($"|{Paint("server", fg: 30, bg: 47)}|>> ERROR: {Paint(command, bg: 41, fg: 37)} is {Paint("unknown", fg: 31)}");
                break;
        }
    }
    else continue;
}
