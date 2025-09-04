namespace Tasheer.Shared.Helper;

public static class FileHandler
{
    readonly static byte[] _pNG = [137, 80, 78, 71, 13, 10, 26, 10];
    readonly static byte[] _jPG = [255, 216, 255, 224, 0, 16, 74, 70, 73, 70, 0, 1];
    readonly static byte[] _jPEG = [0xFF, 0xD8, 0xFF];
    readonly static byte[] _wEBP = Encoding.ASCII.GetBytes("RIFF\x0B\x04WEBPVP8");
    readonly static byte[] _pDF = [0x25, 0x50, 0x44, 0x46, 0x2D];
    readonly static byte[] _hEIC = [0x66, 0x74, 0x79, 0x70, 0x68, 0x65, 0x69, 0x63];

    public static byte[] GetFileAsArray(IFormFile file,
                                        bool pngEncoder = true,
                                        short encoderQuality = 90,
                                        bool allowPdf = false,
                                        bool resize = false,
                                        int newWidth = 0,
                                        int newHeight = 0)
    {
        try
        {
            // open form file for read
            using var stream = file.OpenReadStream();
            using var ms = new MemoryStream();

            // copy the file into memory
            file.CopyTo(ms);

            // convert stream into bytes array
            var buffer = ms.ToArray();

            // check if the file is pdf, then return the file as is
            if (allowPdf && IsPdf(buffer)) return buffer;

            // validate file format
            if (!ValidImageFormat(buffer))
                throw new InvalidDataException("Invalid image format");

            // normalize image
            using var image = new MagickImage(buffer) ??
                throw new InvalidDataException("Invalid image");

            // validate resize dimension
            if (resize && (newWidth <= 0 || newHeight <= 0))
                throw new ArgumentException("Invalid resize info");

            // resize the image
            if (resize)
                image.Resize(Convert.ToUInt32(newWidth), Convert.ToUInt32(newHeight));

            // check image format
            MagickFormat[] standardEncoders = [MagickFormat.Jpg, MagickFormat.Jpeg, MagickFormat.Png];
            if (!standardEncoders.Contains(image.Format))
            {
                image.Format = pngEncoder ? MagickFormat.Png : MagickFormat.Jpeg;
                image.Quality = Convert.ToUInt32(encoderQuality);
            }

            // get the image data
            return image.ToByteArray();
        }
        catch { }

        return null;
    }

    public static bool CheckHeicExtension(string path)
    {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return ext == ".heic";
    }

    public static bool ValidImageFormat(this IFormFile file)
    {
        using var stream = file.OpenReadStream();
        using var ms = new MemoryStream();

        // copy the file into memory
        file.CopyTo(ms);

        // convert stream into bytes array
        var buffer = ms.ToArray();

        return ValidImageFormat(buffer);
    }

    #region [ private ]

    static bool ValidImageFormat(byte[] data)
    {
        var jpegSignature = data
            .Take(_jPEG.Length)
            .SequenceEqual(_jPEG);

        var jpgSignature = data
            .Take(_jPG.Length)
            .SequenceEqual(_jPG);

        var pngSignature = data
            .Take(_pNG.Length)
            .SequenceEqual(_pNG);

        var webpSignature = data
           .Take(_wEBP.Length)
           .SequenceEqual(_wEBP);

        var heicSignature = true;

        // Skip the first 4 bytes (size field) and check the next 8 bytes
        for (int i = 0; i < _hEIC.Length; i++)
        {
            if (data[i + 4] != _hEIC[i])
            {
                heicSignature = false;
                break;
            }
        }

        // success?
        return jpegSignature || pngSignature ||
            webpSignature || jpgSignature || heicSignature;
    }

    static bool IsPdf(byte[] data)
        => data.Take(_pDF.Length).SequenceEqual(_pDF);

    #endregion
}