namespace FOCR;

public class ImageSeparator
{
    public string BasePath { get; init; }
    private readonly IImageProcessService _imageProcessService;

    public ImageSeparator(string basePath, IImageProcessService imageProcessService)
    {
        BasePath = basePath;
        _imageProcessService = imageProcessService;
    }

    public string Separate(string file)
    {
        var fullPath = Path.Combine(BasePath, file);
        var combine = Path.Combine(BasePath, Path.GetFileNameWithoutExtension(fullPath));
        if (!Directory.Exists(combine))
        {
            Directory.CreateDirectory(combine);
        }

        Parallel.ForEach(ImagePieceConstants.AllPieces, piece =>
        {
            var image = _imageProcessService.CensorTeamLogos(fullPath);
            image = _imageProcessService.CropImage(image, piece);
            image = _imageProcessService.DeColorExcept(image, piece);
            image.Save(Path.Combine(combine, piece.Name + ".png"));
            image.Dispose();
        });
        return combine;
    }
}