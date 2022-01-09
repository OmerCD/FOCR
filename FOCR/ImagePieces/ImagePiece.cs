namespace FOCR;

public static class ImagePieceConstants
{
    public static readonly IEnumerable<IImagePiece> AllPieces = new List<IImagePiece>()
    {
        new DetailsPiece(),
        new TeamsPiece(),
        new TitlePiece()
    };
}