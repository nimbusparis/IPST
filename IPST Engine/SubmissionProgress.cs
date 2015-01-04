namespace IPST_Engine
{
    public class SubmissionProgress 
    {
        public SubmissionProgress(int current, int maximum)
        {
            Current = current;
            Maximum = maximum;
        }
        public virtual int Maximum { get; set; }
        public virtual int Current { get; set; }
    }
}