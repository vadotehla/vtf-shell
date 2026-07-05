// Global Variables

string input = "";
bool cont = true;
List<string> Builtins = new(["exit", "type"]);

static string[] ParseInput(string inp)
{
    return inp.Split(" ");
}

/* Commands to be implemented:
    - exit | quit (exits)
    - type (checks location)
    - open (opens a file)
    - list (lists files in the working directory)
    - 
*/
while (cont)
{
    Console.Write("~ ");

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
            default:
                Console.WriteLine($"{command} unknown");
                break;
        }
    }
    else continue;
}
