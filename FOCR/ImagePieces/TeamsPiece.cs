using System.Drawing;
using static FOCR.ColorConstants;
namespace FOCR;

public class TeamsPiece : IImagePiece
{
    public int X { get; set; } = 500;
    public int Y { get; set; } = 50;
    public int Width { get; set; } = 1480;
    public int Height { get; set; } = 100;
    public string Name { get; set; } = "Teams";

    public IEnumerable<Color> ExceptionColors { get; set; } = new List<Color>()
    {
        Blue, White, Yellow 
    };

    public byte PsmCode { get; set; } = 11;
}