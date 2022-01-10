using System.Drawing;
using System.Drawing.Imaging;

namespace FOCR;

public interface IBuild
{

}
public interface IHaveBuilder<T> where T : IBuild
{
    T Builder { get; }
}
public interface IImageProcessService : IHaveBuilder<ImageProcessBuilder>
{
    Bitmap CropImage(string imagePath, IImagePiece imagePiece);
    Bitmap CropImage(string imagePath, int x, int y, int width, int height);
    Bitmap DeColorExcept(string imagePath, IEnumerable<Color> colors);
    Bitmap DeColorExcept(string imagePath, IImagePiece imagePiece);
    Bitmap DeColorExcept(Bitmap image, IEnumerable<Color> colors);
    Bitmap DeColorExcept(Bitmap image, IImagePiece imagePiece);
    Bitmap CensorTeamLogos(Bitmap image);
    Bitmap CensorTeamLogos(string imagePath);
    Bitmap CropImage(Bitmap image, int x, int y, int width, int height);
    Bitmap CropImage(Bitmap image, IImagePiece imagePiece);

}

public class ImageProcessService : IImageProcessService
{
    public ImageProcessBuilder Builder => new ImageProcessBuilder();

    public Bitmap CropImage(Bitmap image, int x, int y, int width, int height)
    {
        var croppedImage = new Bitmap(width, height);
        using var g = Graphics.FromImage(croppedImage);
        g.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height),
            GraphicsUnit.Pixel);
        return croppedImage;
    }
    public Bitmap CropImage(Bitmap image, IImagePiece imagePiece)
    {
        return CropImage(image, imagePiece.X, imagePiece.Y, imagePiece.Width, imagePiece.Height);
    }

    public Bitmap CropImage(string imagePath, IImagePiece imagePiece)
    {
        return CropImage(imagePath, imagePiece.X, imagePiece.Y, imagePiece.Width, imagePiece.Height);
    }

    public Bitmap CropImage(string imagePath, int x, int y, int width, int height)
    {
        using var image = Image.FromFile(imagePath);
        return CropImage((Bitmap)image, x, y, width, height);
    }

    public Bitmap DeColorExcept(string imagePath, IEnumerable<Color> colors)
    {
        Bitmap myBitmap = new Bitmap(imagePath);
        return DeColorExcept(myBitmap, colors);
    }

    public Bitmap DeColorExcept(Bitmap image, IEnumerable<Color> colors)
    {
        var enumerable = colors as Color[] ?? colors.ToArray();
        for (int i = 0; i < image.Width; i++)
        {
            for (int j = 0; j < image.Height; j++)
            {
                Color c = image.GetPixel(i, j);
                var exceptionColor = enumerable.Any(x =>
                    CheckColorWithTolerance(x.R, c.R) && CheckColorWithTolerance(x.G, c.G) &&
                    CheckColorWithTolerance(x.B, c.B));
                if (!exceptionColor)
                {
                    image.SetPixel(i, j, ColorConstants.FullRed);
                }
                else
                {
                    image.SetPixel(i, j, ColorConstants.FullBlack);
                }
            }
        }

        return image;
    }

    public Bitmap DeColorExcept(string imagePath, IImagePiece imagePiece)
    {
        return DeColorExcept(imagePath, imagePiece.ExceptionColors);
    }

    public Bitmap DeColorExcept(Bitmap image, IImagePiece imagePiece)
    {
        return DeColorExcept(image, imagePiece.ExceptionColors);
    }

    public Bitmap CensorTeamLogos(Bitmap image)
    {
        using var g = Graphics.FromImage(image);
        var brush = new SolidBrush(ColorConstants.FullBlack);
        var rect = new Rectangle(1046, 54, 110, 110);

        g.FillRectangle(brush, rect);
        rect.X = 1410;
        g.FillRectangle(brush, rect);
        return image;
    }
    public Bitmap CensorTeamLogos(string imagePath)
    {
        using var image = Image.FromFile(imagePath);

        return CensorTeamLogos(new Bitmap(image));
    }
    private static bool CheckColorWithTolerance(byte a, byte b, byte tolerance = 80)
    {
        var b1 = b - tolerance;
        var b2 = b + tolerance;
        if (b1 < 0)
        {
            b1 = 0;
        }

        if (b2 > 255)
        {
            b2 = 255;
        }

        return a >= b1 && a <= b2;
    }
}


public class ImageProcessBuilder : IBuild
{
    Bitmap _image = null;
    IImageProcessService _imageProcessService;
    public ImageProcessBuilder()
    {
        _imageProcessService = new ImageProcessService();
    }
    public ImageProcessBuilder CensorTeamLogos(string imagePath)
    {

        _image = _imageProcessService.CensorTeamLogos(imagePath);
        return this;
    }

    public ImageProcessBuilder CropImage(IImagePiece imagePiece)
    {

        _image = _imageProcessService.CropImage(_image, imagePiece);
        return this;
    }

    public ImageProcessBuilder DeColorExcept(IImagePiece imagePiece)
    {
        _image = _imageProcessService.DeColorExcept(_image, imagePiece);
        return this;
    }

    public Bitmap ProcessAll(string imagePath, IImagePiece imagePiece)
    {
        return CensorTeamLogos(imagePath).CropImage(imagePiece).DeColorExcept(imagePiece).Build();
    }

    public Bitmap Build()
    {
        return _image.Clone(new Rectangle(0, 0, _image.Width, _image.Height), _image.PixelFormat);
    }

    public static implicit operator Bitmap(ImageProcessBuilder ipb)
    {
        return ipb.Build();
    }
}