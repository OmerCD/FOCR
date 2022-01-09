using System.Drawing;

namespace FOCR;

public class TitlePiece : IImagePiece
{
    public int X { get; set; } = 430;
    public int Y { get; set; } = 230;
    public int Width { get; set; } = 1692;
    public int Height { get; set; } = 70;
    public string Name { get; set; } = "Title";
    public IEnumerable<Color> ExceptionColors { get; set; } = new List<Color>()
    {
        ColorConstants.Yellow 
    };

    public byte PsmCode { get; set; } = 6;
}