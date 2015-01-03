using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using IPST_GUI.Message;
using IPST_GUI.ViewModel;

namespace IPST_GUI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Messenger.Default.Register<MessageNavigatePortals>(this, NotificationMessageReceived);
            Messenger.Default.Register<MessageNavigateCharts>(this, NotificationDisplayCharts);

        }

        private void NotificationDisplayCharts(MessageNavigateCharts obj)
        {
            var chartView = new ChartView();
            chartView.ShowDialog();
        }

        private void NotificationMessageReceived(MessageNavigatePortals obj)
        {
            var portalsView = new PortalsView(obj.Portals);
            portalsView.ShowDialog();
        }
    }
}