using System.CommandLine;
using System.Net;
using System.IO;

public class Cli {
    private string[] CommandLineArguments { get; }
    private int Day { get; set; }
    private char Part { get; set; }
    private const string _aocUrl = "https://adventofcode.com/2025/day";

    public Cli(string[] args)
    {
        CommandLineArguments = args;
    }

    private void ReadArguments()
    {
        RootCommand root = new("2025 AOC");
        Option<int> dayOption = new("--day", "-d")
        {
            Description = "The day to solve a problem for."
        };
        root.Add(dayOption);

        Option<string> partOption = new("--part", "-p")
        {
            Description = "Whether to solve part A or B.",
        };
        root.Add(partOption);

        ParseResult result = root.Parse(CommandLineArguments);
        
        Day = result.GetValue(dayOption);
        Part = result.GetValue(partOption)![0];
    }

    private async Task GetInput()
    {
        var baseFileName = $"{Day:00}/{Part}.txt";
        var inputFileName = Path.Combine(AppContext.BaseDirectory, "input", baseFileName);

        if (File.Exists(inputFileName))
        {
            Console.WriteLine($"Input file {baseFileName} for exercise {Day}{Part} already exists, skipping");
            return;
        }

        string aocSession = Environment.GetEnvironmentVariable("AOC_2025_SESSION")!;
        HttpClientHandler handler = new()
        {
            CookieContainer = new(),
            UseCookies = true,
        };
        handler.CookieContainer.Add(new Cookie("session", aocSession, "/", ".adventofcode.com"));
        HttpClient client = new(handler);
        string requestUri = $"{_aocUrl}/{Day}/input";
        try {
            string result = await client.GetStringAsync(requestUri);
            SaveResult(result);
        } catch (Exception e) {
            Console.WriteLine("Weeeh :( " + e.Message);
        }
    }

    private void SaveResult(string result)
    {
        // Make sure directory exists
        var dirName = Path.Combine(AppContext.BaseDirectory, $"./input/{Day:00}");
        var dir = Directory.CreateDirectory(dirName);

        // Create file for input ("a.txt" or "b.txt")
        var fileName = Path.Combine(dir.FullName, Part + ".txt");
        var file = File.Create(fileName);
        
        // Write to the file
        using StreamWriter writer = new(file);
        writer.Write(result);

        // Tell user it's done
        Console.WriteLine($"File {fileName} created.");
    }

    public async Task SetupDay()
    {
        ReadArguments();
        await GetInput();
    }

    public void Solve()
    {
        ISolver solver = Day switch
        {
            1 => new Day1Solver(),
            _ => throw new NotImplementedException()
        };

        solver.SolvePart(Part);
    }
}
