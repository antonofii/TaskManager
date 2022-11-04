using System.Diagnostics;

namespace TaskManagerProject
{
    public enum ProcessState
    {
        Terminated,
        FailedToTerminate
    }

    public class ProcessTimer
    {
        private Process process;
        private TimeSpan processLifetime;
        private float maxLifetimeDuration;
        private float monitoringFrequency;
        private Action<Process, bool> onProcessKill;
        private Action<Process, Exception> onKillFailed;
        private System.Timers.Timer monitoringTimer = new();

        public Process Process
        {
            get { return process; }
        }

        public float MaxLifetimeDuration
        {
            get { return maxLifetimeDuration; }
        }

        public float MonitoringFrequency
        {
            get { return monitoringFrequency; }
        }

        public ProcessTimer(Process process, float maxLifetimeDuration, float monitoringFrequency,
            Action<Process, Exception> onKillFailed, Action<Process, bool> onProcessKill)
        {
            this.process = process;
            this.maxLifetimeDuration = maxLifetimeDuration;
            this.monitoringFrequency = monitoringFrequency;
            this.onKillFailed = onKillFailed;
            this.onProcessKill = onProcessKill;
            InitializeMonitoring();
        }


        private void StopMonitoring() => monitoringTimer.Stop();


        private void InitializeMonitoring()
        {
            monitoringTimer.Interval = monitoringFrequency * 60000;
            monitoringTimer.AutoReset = true;
            monitoringTimer.Elapsed += OnProcessTimerElapsed;
            monitoringTimer.Start();
        }


        private void OnProcessTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                processLifetime = DateTime.Now.Subtract(process.StartTime);
                if (processLifetime.TotalMinutes >= maxLifetimeDuration)
                {
                    process.Kill();
                    onProcessKill?.Invoke(process, true);
                    StopMonitoring();
                }
            }
            catch (Exception ex)
            {
                onKillFailed?.Invoke(process, ex);
                StopMonitoring();
            }
        }
    }
}
