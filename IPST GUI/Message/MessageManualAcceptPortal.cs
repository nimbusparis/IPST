using IPST_Engine;
using IPST_GUI.View;

namespace IPST_GUI.Message
{
    public class MessageManualAcceptPortal : MessageNavigateView<ManualAcceptView>
    {
        private readonly PortalSubmission _portalSubmission;

        public PortalSubmission PortalSubmission
        {
            get { return _portalSubmission; }
        }

        public MessageManualAcceptPortal(PortalSubmission portalSubmission)
        {
            _portalSubmission = portalSubmission;
        }
    }
}