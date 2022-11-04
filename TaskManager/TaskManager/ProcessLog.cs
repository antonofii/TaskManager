
namespace TaskManagerProject
{
    [Serializable]
    public class ProcessLog
    {
        public string processName { get; set; }
        public DateTime logDateTime { get; set; }
        public ProcessState result { get; set; }

        public ProcessLog(string processName, DateTime logDateTime, ProcessState result)
        {
            this.processName = processName;
            this.logDateTime = logDateTime;
            this.result = result;
        }
    }
}
