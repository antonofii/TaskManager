using System.Diagnostics;

namespace TaskManagerProject
{
    public interface IUserInterface
    {
        public void SetWindowName();
        public void ShowWelcome();
        public void AskMonitoringFrequency();
        public void AskMaxDuration();
        public void ShowException(Exception ex);
        public string GetInput(Exception? ex = null);
        public void ShowStartMonitoring();
        public void ShowInputInstruction();
        public void ShowStack(List<Process> stack);
        public void ShowTimeStack(List<ProcessTimer> timeStack);
    }
}
