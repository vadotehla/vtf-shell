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
