public class MatchModel
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
}

public class StatisticLineModel
{
    public string Key { get; set; }
    public string Home { get; set; }
    public string Away { get; set; }
}