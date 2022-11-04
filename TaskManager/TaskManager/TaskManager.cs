using System.Diagnostics;

namespace TaskManagerProject
{
    public class TaskManager
    {
        private int updateFrequency;
        private int processMaxLifetime;
        private ProgramUI programUI = new();
        private Exception wrongInputException;
        private int processMonitoringFrequency;
        private List<Process> processes = new();
        private LogDataManager logDataManager = new();
        private System.Timers.Timer updateTimer = new();
        private List<ProcessTimer> processTimers = new();


        public void StartProgram()
        {
            InitializeException();
            programUI.SetWindowName();
            programUI.ShowWelcome();
            programUI.AskMonitoringFrequency();
            SetValueFromString(programUI.GetInput(), ref updateFrequency, true);
            programUI.ShowStartMonitoring();
            UpdateStack();
            SetAutoUpdate();
            ShowAllTables(false);
            HandleInput(programUI.GetInput());
        }


        private void UpdateStack()
        {
            List<Process> rawProcessStack = Process.GetProcesses().ToList();
            foreach (Process process in rawProcessStack.ToList())
            {
                if (processTimers.Find(x => x.Process.Id == process.Id) != null)
                {
                    rawProcessStack.Remove(process);
                }
            }
            processes = rawProcessStack;
        }


        private void SetAutoUpdate()
        {
            updateTimer.Interval = updateFrequency * 60000;
            updateTimer.Start();
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += (object? sender, System.Timers.ElapsedEventArgs e) => UpdateTables();
        }


        private void UpdateTables()
        {
            UpdateStack();
            ShowAllTables(true);
        }


        private void ShowAllTables(bool clearConsole)
        {
            if (clearConsole)
                Console.Clear();

            programUI.ShowStack(processes);
            programUI.ShowTimeStack(processTimers);
            programUI.ShowInputInstruction();
        }


        private void HandleInput(string input)
        {
            if (input == "q")
            {
                Environment.Exit(0);
            }
            else
            {
                try
                {
                    int processIndex = Convert.ToInt16(input);
                    if (processes.Count > 0 &&
                        processIndex > processes.Count - 1)
                    {
                        throw wrongInputException;
                    }
                    if (input == "0")
                    {
                        throw wrongInputException;
                    }
                    CreateNewProcessTimer(processIndex);
                }
                catch (Exception ex)
                {
                    HandleInput(programUI.GetInput(ex));
                }
            }
        }


        private void CreateNewProcessTimer(int processIndex)
        {
            programUI.AskMonitoringFrequency();
            SetValueFromString(programUI.GetInput(), ref processMonitoringFrequency, true);
            programUI.AskMaxDuration();
            SetValueFromString(programUI.GetInput(), ref processMaxLifetime, true);
            ProcessTimer processTimer = new(processes[processIndex], processMaxLifetime,
                processMonitoringFrequency, OnProcessKillFailed, OnProcessKilled);
            processTimers.Add(processTimer);
            ShowAllTables(true);
            HandleInput(programUI.GetInput());
        }


        private void SetValueFromString(string input, ref int value, bool greaterThanZero = false)
        {
            try
            {
                value = Convert.ToInt32(input);
                if (greaterThanZero && value <= 0)
                {
                    throw wrongInputException;
                }
            }
            catch (Exception ex)
            {
                SetValueFromString(programUI.GetInput(ex), ref value, greaterThanZero);
            }
        }


        private void OnProcessKillFailed(Process process, Exception ex)
        {
            OnProcessKilled(process, false);
            logDataManager.AddLog(new ProcessLog(process.ProcessName, DateTime.Now, ProcessState.FailedToTerminate), OnLogFailed);
        }


        private void OnProcessKilled(Process process, bool logRequired)
        {
            ProcessTimer? processTimer = processTimers.Find(x => x.Process == process);
            if (processTimer != null)
            {
                processTimers.Remove(processTimer);
            }

            if (logRequired)
                logDataManager.AddLog(new ProcessLog(process.ProcessName, DateTime.Now, ProcessState.Terminated), OnLogFailed);

            UpdateTables();
        }


        private void OnLogFailed(Exception ex)
        {
            programUI.ShowException(ex);
        }


        private void InitializeException()
        {
            wrongInputException = new Exception("Wrong value!");
        }
    }
}
