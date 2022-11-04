using System.Diagnostics;

namespace TaskManagerProject
{
    public class ProgramUI : IUserInterface
    {
        private string? input;

        public void SetWindowName()
        {
            Console.Title = "CONSOLE PROCESS MANAGER";
        }


        public void ShowWelcome()
        {
            Console.WriteLine();
            Console.WriteLine("WELCOME TO THE CONSOLE PROCESS MANAGER");
            Console.WriteLine("--------------------------------------");
        }


        public void AskMonitoringFrequency()
        {
            Console.Write("ENTER MONITORING FREQUENCY (MINUTES):");
        }


        public void AskMaxDuration()
        {
            Console.Write("ENTER MAX PROCESS LIFETIME (MINUTES):");
        }


        public string GetInput(Exception? ex = null)
        {
            if (ex != null)
            {
                Console.WriteLine("ERROR! " + ex.Message.ToUpper());
                Console.WriteLine("PLEASE TRY AGAIN");
            }
            input = Console.ReadLine();
            return input;
        }


        public void ShowStartMonitoring()
        {
            Console.WriteLine("-------------------");
            Console.WriteLine("STARTING MONITORING");
            Console.WriteLine();
        }


        public void ShowInputInstruction()
        {
            Console.Write("ENTER 'q' TO QUIT OR A PROCESS NUMBER TO CREATE NEW TIMER:");
        }


        public void ShowStack(List<Process> stack)
        {
            Console.WriteLine("RUNNING PROCESSES:");
            for(int i = 0; i < stack.Count; i++)
            {
                Console.WriteLine(i + ". " + stack[i].ProcessName);
            }
        }


        public void ShowTimeStack(List<ProcessTimer> timeStack)
        {
            Console.WriteLine("-------------------");
            Console.WriteLine("PROCESSES WITH TIMERS");
            for (int i = 0; i < timeStack.Count; i++)
            {
                ProcessTimer item = timeStack[i];
                Console.WriteLine(i + ". " + item.Process.ProcessName + " " + 
                     + item.MonitoringFrequency + " " + item.MaxLifetimeDuration);
            }
        }

        public void ShowException(Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("ERROR! " + ex.Message);
        }
    }
}
