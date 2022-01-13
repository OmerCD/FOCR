using FOCR;
using MongoORM4NetCore.Interfaces;

public class MatchModel : DbObjectSD
{
    public HalfModel FirstHalf { get; set; }
    public HalfModel SecondHalf { get; set; }
    public DateTime Date { get; set; }
}
public class HalfModel
{
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    public IEnumerable<StatisticLineModel> Summary { get; set; }
    public IEnumerable<StatisticLineModel> Possession { get; set; }
    public IEnumerable<StatisticLineModel> Shooting { get; set; }
    public IEnumerable<StatisticLineModel> Passing { get; set; }
    public IEnumerable<StatisticLineModel> Defending { get; set; }

    public void SetTeamNamesAndScores(string text)
    {
        text = text.ReplaceFaultyCharsForScoreAndTeamNames();

        var split = text.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var homeTeamInfo = split[0].Replace(" ", "");
        var awayTeamInfo = split[1].Replace(" ", "");

        var homeTeamInfoSpan = homeTeamInfo.AsSpan();
        var homeTeamSpan = homeTeamInfoSpan.Slice(0, homeTeamInfo.Length - 1);
        var homeTeamSpanScore = homeTeamInfoSpan.Slice(homeTeamInfo.Length - 1, 1);

        var awayTeamInfoSpan = awayTeamInfo.AsSpan();
        var awayTeamSpan = awayTeamInfoSpan.Slice(1, awayTeamInfo.Length - 1);
        var awayTeamSpanScore = awayTeamInfoSpan.Slice(0, 1);

        HomeTeam = homeTeamSpan.ToString();
        HomeScore = int.Parse(homeTeamSpanScore);

        AwayTeam = awayTeamSpan.ToString();
        AwayScore = int.Parse(awayTeamSpanScore);
    }

    public override string ToString() => @$"HomeTeam : {HomeTeam} 
                                            HomeScore : {HomeScore} 
                                            {Environment.NewLine} 
                                            AwayTeam: {AwayTeam}
                                            AwayScore: {AwayScore}";
}

public class StatisticLineModel
{
    public string Key { get; set; }
    public string Home { get; set; }
    public string Away { get; set; }
}