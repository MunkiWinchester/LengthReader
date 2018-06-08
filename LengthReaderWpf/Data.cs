using System;
using System.IO;

namespace LengthReaderWpf
{
    public class Data
    {
        public Data(FileInfo fileInfo, int height, int width, TimeSpan duration, string basePath)
        {
            FileInfo = fileInfo;
            Height = height;
            Width = width;
            Duration = duration;
            ShortDirectoryName = fileInfo.DirectoryName?.Replace(basePath, "").Trim('\\');
        }

        public FileInfo FileInfo { get; set; }
        public string ShortDirectoryName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public TimeSpan Duration { get; set; }

        public override string ToString() =>
            $"{FileInfo.FullName} - {Width}x{Height} - {Duration:g}";
        }
    }
}