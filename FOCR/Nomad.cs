namespace FOCR;


public static class Nomad
{

    public static string ReplaceFaultyChars(this string text)
    {
        return text.Replace('a', '0').Replace('O', '0').Replace('o', '0');
    }

    public static string ReplaceFaultyCharsForScoreAndTeamNames(this string text)
    {
        var replaceMap = File.ReadAllLines("MapList.txt");
        Parallel.ForEach(replaceMap, line =>
        {
            var split = line.Replace("\t", string.Empty).Split("-", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            string oldText = split[0];
            string newText = split[1];
            text = text.Replace(oldText, newText);
        });

        return text;

    }

    public static MatchModel SetTeamNamesAndScores(this MatchModel model, string text)
    {
        text = text.ReplaceFaultyCharsForScoreAndTeamNames();

        var split = text.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var homeTeamInfo = split[0].Replace(" ", "");
        var awayTeamInfo = split[1].Replace(" ", "");

        var homeTeam = homeTeamInfo.Substring(0, homeTeamInfo.Length - 1);
        var homeTeamScore = homeTeamInfo.Substring(homeTeamInfo.Length - 1, 1);

        var awayTeam = awayTeamInfo.Substring(1, awayTeamInfo.Length - 1);
        var awayTeamScore = awayTeamInfo.Substring(0, 1);

        model.HomeTeam = homeTeam;
        model.HomeScore = int.Parse(homeTeamScore);

        model.AwayTeam = awayTeam;
        model.AwayScore = int.Parse(awayTeamScore);

        return model;
    }

}