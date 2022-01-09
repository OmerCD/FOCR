using System.Drawing;

namespace FOCR;

public interface IImagePiece
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Name { get; set; }
    public IEnumerable<Color> ExceptionColors { get; set; }
    public byte PsmCode { get; set; }
}