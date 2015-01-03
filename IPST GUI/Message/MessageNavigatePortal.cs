using IPST_GUI.View;
using IPST_GUI.ViewModel;

namespace IPST_GUI.Message
{
    public class MessageNavigatePortal : MessageNavigateView<PortalView>
    {
        private readonly PortalViewModel _viewModel;

        public PortalViewModel ViewModel
        {
            get { return _viewModel; }
        }
        public MessageNavigatePortal(PortalViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}