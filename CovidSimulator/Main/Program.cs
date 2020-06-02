using Controller;
using Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using View;

namespace Main
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static string filePath = "Load100.txt";
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoggerSetup();

            EntityLoader loader = new EntityLoader();
            loader.LoadFromFile(filePath);

            List<DisplayableElement> displayableGrid = EntityDisplayConverter.ToDisplayableElements(loader.grid.SelectMany(node => node).ToList<IPosition>());
            List<DisplayableElement> displayableRooms = EntityDisplayConverter.ToDisplayableElements(loader.rooms.ToList<IPosition>());
            SimulationForm simulationForm = new SimulationForm(displayableGrid, displayableRooms);

            SimulationController controller = new SimulationController(simulationForm, loader.grid, loader.movableEntities, loader.rooms);

            Console.WriteLine("Starting Setup:");
            Console.WriteLine("Seed file: {0}", filePath);
            Console.WriteLine("Simulation with {0} people", loader.movableEntities.Count);

            // close event from the GUI
            simulationForm.onCloseEvent += (object sender, EventArgs e) => controller.Dispose();
            // close event from the console
            AppDomain.CurrentDomain.ProcessExit += new EventHandler((object sender, EventArgs e) => controller.Dispose());

            Thread controllerThread = new Thread(controller.start);
            simulationForm.onShowEvent += new EventHandler((object sender, EventArgs e) => controllerThread.Start());

            Application.Run(simulationForm);
        }

        private static void LoggerSetup()
        {
            //Logger configuration
            var config = new NLog.Config.LoggingConfiguration();
            // Where to lo to: File & Console
            var logFile = new NLog.Targets.FileTarget("logFile") { FileName = "log_file.txt" };
            var logConsole = new NLog.Targets.ConsoleTarget("logConsole");
            // Logging rules (where to log what)
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);
            config.AddRule(LogLevel.Warn, LogLevel.Fatal, logConsole);

            LogManager.Configuration = config;
        }

    }
}
