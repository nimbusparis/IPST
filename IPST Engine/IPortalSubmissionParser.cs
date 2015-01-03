using Google.Apis.Gmail.v1.Data;

namespace IPST_Engine
{
    public interface IPortalSubmissionParser
    {
        PortalSubmission ParseMessage(Message message);
    }
}