using System;
using System.IO;
using TagLib;

namespace LengthReaderWpf
{
    public class Data
    {
        public Data(FileInfo fileInfo, int height, int width, TimeSpan duration, string basePath, string imagePath = "", Tag tag = null)
        {
            FileInfo = fileInfo;
            Height = height;
            Width = width;
            Duration = duration;
            ShortDirectoryName = fileInfo.DirectoryName?.Replace(basePath, "").Trim('\\');
			ImagePath = imagePath;
			Tag = tag;
        }

        public FileInfo FileInfo { get; set; }
        public string ShortDirectoryName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public TimeSpan Duration { get; set; }
		public string ImagePath { get; set; } = "https://image.tmdb.org/t/p/w600_and_h900_bestv2/inVq3FRqcYIRl2la8iZikYYxFNR.jpg";
		public Tag Tag { get; set; }


		public override string ToString() =>
            $"{FileInfo.FullName} - {Width}x{Height} - {Duration:g}";
        }
    }