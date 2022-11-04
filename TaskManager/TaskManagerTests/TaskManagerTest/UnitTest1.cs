using System.Diagnostics;
using TaskManagerProject;

namespace TaskManagerTest
{
    public class Tests
    {
        [Test]
        public void Process_Timer_Values_Fill_In()
        {
            int maxLifetimeDuration = 1;
            int monitoringFrequency = 1;
            Process process = new Process();
            ProcessTimer processTimer = new ProcessTimer(process, maxLifetimeDuration, monitoringFrequency, null, null);
            Assert.That(processTimer.Process, Is.EqualTo(process));
            Assert.That(processTimer.MonitoringFrequency, Is.EqualTo(monitoringFrequency));
            Assert.That(processTimer.MaxLifetimeDuration, Is.EqualTo(maxLifetimeDuration));
        }

        [Test]
        public void ProcessLog_Fills_In()
        {
            string processName = "testName";
            ProcessState result = ProcessState.Terminated;
            DateTime dateTime = DateTime.Now;
            ProcessLog processLog = new ProcessLog(processName, dateTime, result);
            Assert.That(processLog.result, Is.EqualTo(result));
            Assert.That(processLog.processName, Is.EqualTo(processName));
            Assert.That(processLog.logDateTime, Is.EqualTo(dateTime));
        }
    }
}