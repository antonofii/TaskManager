using System.Text.Json;

namespace TaskManagerProject
{
    public class LogDataManager
    {
        public async void AddLog(ProcessLog processLog, Action<Exception> onLogFailed)
        {
            try
            {
                string fileName = "Log-" + Guid.NewGuid() + ".txt";
                string jsonString = JsonSerializer.Serialize(processLog);
                await File.WriteAllTextAsync(fileName, jsonString);
            }
            catch (Exception ex)
            {
                onLogFailed(ex);
            }
        }
    }
}
