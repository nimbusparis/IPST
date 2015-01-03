using System.Collections.Generic;
using IPST_Engine;
using IPST_GUI.View;

namespace IPST_GUI.Message
{
    public class MessageNavigatePortals: MessageNavigateView<PortalsView>
    {
        private readonly IList<PortalSubmission> _portals;

        public IList<PortalSubmission> Portals
        {
            get { return _portals; }
        }

        public MessageNavigatePortals(IList<PortalSubmission> portals)
        {
            _portals = portals;
        }
    }
}
