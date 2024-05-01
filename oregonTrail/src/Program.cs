

using System;
using System.Threading;

namespace OregonTrailDotNet
{
    
    internal static class Program
    {
        
        public static int Main()
        {
            // Create console with title, no cursor, make CTRL-C act as input.
            Console.Title = "Oregon Trail Clone";
            Console.WriteLine("Starting...");
            Console.CursorVisible = false;
            Console.CancelKeyPress += Console_CancelKeyPress;

            
            Console.OutputEncoding = System.Text.Encoding.Unicode;

           
            GameSimulationApp.Create();

            
            GameSimulationApp.Instance.SceneGraph.ScreenBufferDirtyEvent += Simulation_ScreenBufferDirtyEvent;

         
            while (GameSimulationApp.Instance != null)
            {
              
                GameSimulationApp.Instance.OnTick(true);

                
                if (Console.KeyAvailable)
                {
                    
                    var key = Console.ReadKey(true);

                   
                    switch (key.Key)
                    {
                        case ConsoleKey.Enter:
                            GameSimulationApp.Instance.InputManager.SendInputBufferAsCommand();
                            break;
                        case ConsoleKey.Backspace:
                            GameSimulationApp.Instance.InputManager.RemoveLastCharOfInputBuffer();
                            break;
                        default:
                            GameSimulationApp.Instance.InputManager.AddCharToInputBuffer(key.KeyChar);
                            break;
                    }
                }

            
                Thread.Sleep(1);
            }

           
            Console.Clear();
            Console.WriteLine("Goodbye!");
            Console.WriteLine("Press ANY KEY to close this window...");
            Console.ReadKey();
            return 0;
        }

       
        private static void Simulation_ScreenBufferDirtyEvent(string tuiContent)
        {
            var tuiContentSplit = tuiContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (var index = 0; index < Console.WindowHeight - 1; index++)
            {
                Console.CursorLeft = 0;
                Console.SetCursorPosition(0, index);

                var emptyStringData = new string(' ', Console.WindowWidth);

                if (tuiContentSplit.Length > index)
                {
                    emptyStringData = tuiContentSplit[index].PadRight(Console.WindowWidth);
                }

                Console.Write(emptyStringData);
            }
        }

        
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
        
            GameSimulationApp.Instance.Destroy();

          
            e.Cancel = true;
        }
    }
}