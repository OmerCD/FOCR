using System.Drawing;

namespace FOCR;

public class DetailsPiece : IImagePiece
{
    public int X { get; set; } = 850;
    public int Y { get; set; } = 320;
    public int Width { get; set; } = 852;
    public int Height { get; set; } = 978;
    public string Name { get; set; } = "Details";
    public IEnumerable<Color> ExceptionColors { get; set; } = new List<Color>()
    {
        ColorConstants.White
    };

    public byte PsmCode { get; set; } = 6;
}