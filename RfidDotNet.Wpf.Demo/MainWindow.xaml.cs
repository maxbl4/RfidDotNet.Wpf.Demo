using maxbl4.Infrastructure.Extensions.DisposableExt;
using maxbl4.RfidDotNet;
using maxbl4.RfidDotNet.AlienTech.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RfidDotNet.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UniversalTagStreamFactory factory;
        private IUniversalTagStream stream;

        public MainWindow()
        {
            InitializeComponent();
            factory = new UniversalTagStreamFactory();
            factory.UseAlienProtocol();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (stream != null) stream.DisposeSafe();

            stream = factory.CreateStream("protocol=Alien;Network=127.0.0.1:20023;RfPower=200;AntennaConfiguration=Antenna1");
            stream.Connected.Subscribe(c => Dispatcher.BeginInvoke(new Action(() => { connectionStatus.Text = $"{DateTime.Now}> Connected: {c}"; })));
            stream.Errors.Subscribe(err => Dispatcher.BeginInvoke(new Action(() => { errors.Text = $"{DateTime.Now}> {err}"; })));
            stream.Tags.Subscribe(tag => Dispatcher.BeginInvoke(new Action(() => { tags.Text = tag.TagId; })));
            await stream.Start();
        }
    }
}
