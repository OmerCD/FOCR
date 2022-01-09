using System.Diagnostics;

namespace FOCR;

public interface IOCRService
{
    public Task<string> Read(string imagePath, byte psmCode = 6);
}

public class OCRService : IOCRService
{
    private const string TesseractCommand = "{0}.png - -l deu --psm {1}";

    public async Task<string> Read(string imagePath, byte psmCode = 6)
    {
        var name = Path.GetFileNameWithoutExtension(imagePath);
        var path = Path.GetDirectoryName(imagePath);
        var command = string.Format(TesseractCommand, name, psmCode);
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "tesseract",
                Arguments = command,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = path
            }
        };
        proc.Start();
        await proc.WaitForExitAsync();
        var line = await proc.StandardOutput.ReadToEndAsync();
        return line;
    }
}