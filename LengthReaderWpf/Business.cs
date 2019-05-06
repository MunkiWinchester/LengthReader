using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaToolkit;
using MediaToolkit.Model;
using TagLib;
using File = System.IO.File;

namespace LengthReaderWpf
{
    public static class Business
    {
        public static Data GetVideoInfo(FileInfo fileInfo)
        {
            try
            {
                var inputFile = new MediaFile {Filename = fileInfo.FullName};
                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);
                }

				// FrameSize is returned as '1280x768' string.
				var size = new int[] { 0, 0 };
				try
				{
					size = inputFile.Metadata.VideoData.FrameSize.Split('x').Select(int.Parse).ToArray();
				}
				catch (Exception ex)
				{
					Console.WriteLine($@"{fileInfo} - {ex}");
				}

				return new Data(fileInfo, size[1], size[0], inputFile.Metadata?.Duration ?? new TimeSpan(0), fileInfo.FullName.Replace(fileInfo.Name, ""), tag: ReadMetaDatas(fileInfo.FullName));
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"{fileInfo} - {ex}");
                return null;
            }
        }

		/// <summary>
		/// Read metadata from a file info
		/// </summary>
		/// <param name="filePath">File info of the file, of which the metadata should be read</param>
		/// <returns>The baseinfotag</returns>
		public static Tag ReadMetaDatas(string filePath)
		{
			try
			{
				// Auslesen
				var file = TagLib.File.Create(filePath);
				var tag = file.TagTypes != TagTypes.Id3v2 ? file.Tag : file.GetTag(TagTypes.Id3v2);
				return tag;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public static List<FileInfo> GetFileInfos(string folder)
		{
			var dir = new DirectoryInfo(folder);
			var list = new List<FileInfo>();
			foreach (var item in dir.GetFiles())
			{
				if(!item.Attributes.HasFlag(FileAttributes.Hidden) &&
					new[] { ".mkv", ".avi", ".mp4", ".mpg", /*".ts",*/ ".m4v" }.Contains(item.Extension)
					/*&& item.Length >  52428800 /* 50 megabyte */)
					list.Add(item);
			}
			foreach (var subDir in dir.GetDirectories())
			{
				try
				{
					list.AddRange(GetFileInfos(subDir.FullName));
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
			return list;
		}
	}
}