namespace FOCR;

public class MatchModelExtractor
{
    private readonly IImageProcessService _imageProcessService;
    private readonly IOCRService _ocrService;

    public MatchModelExtractor(IImageProcessService imageProcessService, IOCRService ocrService)
    {
        _imageProcessService = imageProcessService;
        _ocrService = ocrService;
    }

    private string GetJson(string text)
    {
        string[] lines = text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var jsonLines = string.Join(',', lines.Select(GetJsonLine));
        return $"[{jsonLines}]";
    }

    private static string GetJsonLine(string text)
    {
        var homeTeamIndex = text.IndexOf(' ');
        var awayTeamIndex = text.LastIndexOf(' ');
        var homeValue = text.Substring(0, homeTeamIndex);
        var awayValue = text.Substring(awayTeamIndex + 1, text.Length - awayTeamIndex - 1);
        var key = text.Substring(homeTeamIndex + 1, awayTeamIndex - homeTeamIndex - 1);
        return "{\"key\":\"" + key + "\",\"home\":\"" + homeValue.ReplaceFaultyChars() + "\",\"away\":\"" + awayValue.ReplaceFaultyChars() + "\"}";
    }

    private string GetPossessionJson(string text)
    {
        string[] lines = text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var list = new List<string>();
        for (var i = 0; i < lines.Length; i++)
        {
            var values = lines[i].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var jsonValue = "{\"key\":\"" + ((i + 1) * 15)
                + "\",\"home\":\"" + values[0].ReplaceFaultyChars()
                + "\",\"away\":\"" + values[1].ReplaceFaultyChars()
                + "\"}";

            list.Add(jsonValue);
        }

        return "[" + string.Join(',', list) + "]";
    }


    private static IEnumerable<StatisticLineModel> GetPossessionModels(string text)
    {
        string[] lines = text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        for (var i = 0; i < lines.Length; i++)
        {
            var values = lines[i].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var minute = (i + 1) * 15;
            yield return new StatisticLineModel()
            {
                Key = minute.ToString(),
                Home = values[0].ReplaceFaultyChars(),
                Away = values[1].ReplaceFaultyChars()
            };
        }
    }

    private static StatisticLineModel GetModel(string text)
    {
        var homeTeamIndex = text.IndexOf(' ');
        var awayTeamIndex = text.LastIndexOf(' ');
        var homeValue = text.Substring(0, homeTeamIndex).ReplaceFaultyChars();
        var awayValue = text.Substring(awayTeamIndex + 1, text.Length - awayTeamIndex - 1).ReplaceFaultyChars();
        var key = text.Substring(homeTeamIndex + 1, awayTeamIndex - homeTeamIndex - 1);
        return new StatisticLineModel
        {
            Key = key,
            Home = homeValue,
            Away = awayValue
        };
    }

    private static IEnumerable<StatisticLineModel> GetModels(string text)
    {
        string[] lines = text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return lines.Where(x=>!x.Contains('%')).Select(GetModel);
    }

    public async Task<HalfModel?> CreateMatchModel(string imagesPath)
    {
        var files = Directory.GetFiles(imagesPath);
        if (!files.Any())
        {
            return null;
        }

        var separator = new ImageSeparator(imagesPath, _imageProcessService);
        var matchModel = new HalfModel();
        string newPath = String.Empty;
        Parallel.ForEach(files, async image =>
        {
            newPath = separator.Separate(Path.GetFileName(image));
            var details = await _ocrService.Read(Path.Combine(newPath, $"Details.png"));
            var title = (await _ocrService.Read(Path.Combine(newPath, $"Title.png"))).Trim().Replace(" ", "").ToLower();
            switch (title)
            {
                case "summary":
                    matchModel.Summary = GetModels(details).ToArray();
                    break;
                case "possession":
                    matchModel.Possession = GetPossessionModels(details.Replace("%", "")).ToArray();
                    break;
                case "shooting":
                    matchModel.Shooting = GetModels(details).ToArray();
                    break;
                case "passing":
                    matchModel.Passing = GetModels(details).ToArray();
                    break;
                case "defending":
                    matchModel.Defending = GetModels(details).ToArray();
                    break;
            }
        });

        var teamInfos = await _ocrService.Read(Path.Combine(newPath, $"Teams.png"));
        matchModel.SetTeamNamesAndScores(teamInfos);

        Console.WriteLine(teamInfos.Replace("\r\n", " "));
        Console.WriteLine(newPath);
        return matchModel;
    }
}