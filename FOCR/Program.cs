using FOCR;


var mainDirectory = @"C:\Program Files (x86)\Steam\userdata\182075523\760\remote\1506830\screenshots\";
var dateDirectories = Directory.GetDirectories(mainDirectory).Where(x => x.Contains("202"));


var basePath = @"C:\Program Files (x86)\Steam\userdata\182075523\760\remote\1506830\screenshots\2022-01-02\3\1\";
var imageProcessService = new ImageProcessService();
var ocrReader = new OCRService();
var extractor = new MatchModelExtractor(imageProcessService, ocrReader);


foreach (var dateDirectory in dateDirectories)
{


    var matchDirectories = Directory.GetDirectories(dateDirectory);
    foreach (var matchDirectory in matchDirectories)
    {
        MatchModel? firstHalf = new();
        MatchModel? secondHalf = new();

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
    }
}