using System;
using System.IO;
using System.Linq;
using MediaToolkit;
using MediaToolkit.Model;

namespace LengthReaderWpf
{
    public static class Business
    {
        public static Data GetVideoInfo(FileInfo fileInfo, DirectoryInfo dir)
        {
            try
            {
                var inputFile = new MediaFile {Filename = fileInfo.FullName};
                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);
                }

                // FrameSize is returned as '1280x768' string.
                var size = inputFile.Metadata.VideoData.FrameSize.Split('x').Select(int.Parse).ToArray();

                return new Data(fileInfo, size[1], size[0], inputFile.Metadata.Duration, dir.FullName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"{fileInfo} - {ex}");
                return null;
            }
        }
    }
}