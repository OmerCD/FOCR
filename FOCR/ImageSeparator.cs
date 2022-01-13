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
            using var image = _imageProcessService.Builder
                        .CensorTeamLogos(fullPath)
                        .CropImage(piece)
                        .DeColorExcept(piece).Build();
            lock (BasePath)
            {
                image.Save(Path.Combine(combine, piece.Name + ".png"));
            }
        });

        return combine;
    }
}