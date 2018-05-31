using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfUtility.Services;

namespace LengthReaderWpf
{
    public class MainWindowViewModel : ObservableObject
    {
        private ObservableCollection<Data> _dataList;
        private bool _isLoading;
        private bool _isPlayingVideo;
        private string _subMessage;

        public ObservableCollection<Data> DataList
        {
            get => _dataList;
            set => SetField(ref _dataList, value);
        }

        public string SubMessage
        {
            get => _subMessage;
            set => SetField(ref _subMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetField(ref _isLoading, value);
        }

        public MainWindowViewModel()
        {
            DataList = new ObservableCollection<Data>();
        }

        public RelayCommand<object> PlayFileCommand => new RelayCommand<object>(obj =>
        {
            if (obj is Data data)
                Process.Start(data.FileInfo.FullName);
        });

        public bool IsPlayingVideo
        {
            get => _isPlayingVideo;
            set => SetField(ref _isPlayingVideo, value);
        }

        public async void LoadDataAsync()
        {
            IsLoading = true;
            await Task.Run(() =>
            {
                var dir = new DirectoryInfo(@"D:\Users\eikes\Videos");
                var files = dir.GetFiles("*.*", SearchOption.AllDirectories).ToList()
                    .Where(f => new[] {".mkv", ".avi", ".mp4", ".mpg", ".ts"}.Contains(f.Extension)).ToList();
                var count = 0;
                foreach (var fileInfo in files)
                {
                    var info = Business.GetVideoInfo(fileInfo, dir);
                    if (info != null)
                    {
                        Console.WriteLine(info);
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                        {
                            SubMessage = $"{++count} of {files.Count} loaded..";
                            DataList.Add(info);
                        }));
                    }
                }
            }).ContinueWith(t => { IsLoading = false; });
        }
    }
}