using System.IO;
using System.Windows;

namespace LengthReaderWpf
{
    /// <inheritdoc cref="MahApps.Metro.Controls.MetroWindow" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //VlcControl.MediaPlayer.VlcLibDirectory =
            //    new DirectoryInfo(@"c:\Program Files (x86)\VideoLAN\VLC\");
            //VlcControl.MediaPlayer.EndInit();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
                vm.LoadDataAsync();
        }
    }
}
