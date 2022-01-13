using System.Collections.Concurrent;
using System.Globalization;
using FOCR;
using MongoORM4NetCore;

MongoDbConnection.InitializeAndStartConnection(
    "mongodb+srv://dbusr:9YpBXL6L8zuTez48@cluster0.zvps8.mongodb.net/FOCR?retryWrites=true&w=majority", "FOCR");
var matchCrud = new Crud<MatchModel>();

var mainDirectory = @"C:\Program Files (x86)\Steam\userdata\182075523\760\remote\1506830\screenshots\";
var dateDirectories = Directory.GetDirectories(mainDirectory).Where(x => x.Contains("202"));


var basePath = @"C:\Program Files (x86)\Steam\userdata\182075523\760\remote\1506830\screenshots\2022-01-02\3\1\";
var imageProcessService = new ImageProcessService();
var ocrReader = new OCRService();
var extractor = new MatchModelExtractor(imageProcessService, ocrReader);

var matches = new ConcurrentDictionary<DateTime, ICollection<MatchModel>>();
Parallel.ForEach(dateDirectories, dateDirectory =>
{
    var matchDirectories = Directory.GetDirectories(dateDirectory);
    var date = DateTime.ParseExact(Path.GetFileName(dateDirectory), "yyyy-MM-dd", CultureInfo.InvariantCulture);
    matches.TryAdd(date, new List<MatchModel>());
    Parallel.ForEach(matchDirectories, async (matchDirectory) =>
    {
        HalfModel? firstHalf = new();
        HalfModel? secondHalf = new();

        var firstHalfPath = Path.Combine(matchDirectory, "1");
        var secondHalfPath = Path.Combine(matchDirectory, "2");
        if (!Directory.Exists(firstHalfPath))
        {
            firstHalf = null;
            secondHalf = await extractor.CreateMatchModel(matchDirectory);
        }
        else
        {
            firstHalf = await extractor.CreateMatchModel(firstHalfPath);
            if (Directory.Exists(secondHalfPath))
            {
                secondHalf = await extractor.CreateMatchModel(secondHalfPath);
            }
        }

        matches.GetOrAdd(date, new List<MatchModel>()).Add(new MatchModel
        {
            FirstHalf = firstHalf,
            SecondHalf = secondHalf,
            Date = date
        });
    });
    if (matches.TryGetValue(date, out var matchModels))
    {
        matchCrud.InsertMany(matchModels.ToArray());
        Console.WriteLine($"{date} - {matchModels.Count}");
    }
});