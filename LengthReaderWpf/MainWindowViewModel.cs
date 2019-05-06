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
        private string _path = @"D:\";
        private string _subMessage;

        public MainWindowViewModel()
        {
            DataList = new ObservableCollection<Data>();
        }

        public string Path
        {
            get => _path;
            set => SetField(ref _path, value);
        }

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

        public RelayCommand<object> PlayFileCommand =>
            new RelayCommand<object>(obj =>
            {
                if (obj is Data data)
                    Process.Start(data.FileInfo.FullName);
            });

        public DelegateCommand LoadFilesCommand => new DelegateCommand(LoadDataAsync);

        public async void LoadDataAsync()
        {
            IsLoading = true;
            DataList.Clear();
            await Task.Run(() =>
            {
                if (Directory.Exists(Path))
                {
					var files = Business.GetFileInfos(Path);
                    var count = 0;
                    foreach (var fileInfo in files)
                    {
                        var info = Business.GetVideoInfo(fileInfo);
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
					if (!DataList.Any())
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
						{
							DataList.Add(new Data(new FileInfo(@"D:\Dump\deadpool-2016.mp4"), 1920, 1080, new TimeSpan(1, 49, 0), @"D:\Dump\", tag: Business.ReadMetaDatas(@"D:\Dump\deadpool-2016.mp4")));
						}));
				}
            }).ContinueWith(t => { IsLoading = false; });
        }
    }
}