using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ChaosGameScreensaver
{
    static class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Console.WriteLine(RegistryStorage.GetData().Count);

            if (RegistryStorage.GetData().Count == 0)
            {
                List<RenderData> list = new List<RenderData>();
                list.Add(new RenderData(3, 0.5));
                list.Add(new RenderData(4, 0.25));
                list.Add(new RenderData(5, 0.33));
                list.Add(new RenderData(5, 0.395));
                list.Add(new RenderData(6, 0.33));
                list.Add(new RenderData(20, 0.1));
                list.Add(new RenderData(100, 0.01));

                RegistryStorage.SaveData(list);
            }


            if (args.Length > 0)
            {
                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = null;

                // Handle cases where arguments are separated by colon.
                if (firstArgument.Length > 2)
                {
                    secondArgument = firstArgument.Substring(3).Trim();
                    firstArgument = firstArgument.Substring(0, 2);
                }
                else if (args.Length > 1)
                    secondArgument = args[1];



                if (firstArgument == "/c")           // Configuration mode
                {
                    ConfigurationForm config = new ConfigurationForm();
                    Application.Run(config);
                }
                else if (firstArgument == "/p")      // Preview mode
                {
                    IntPtr previewWindowHandle = new IntPtr(long.Parse(secondArgument));
                    Application.Run(new ScreenSaverForm(previewWindowHandle));

                }
                else if (firstArgument == "/s")      // Full-screen mode
                {
                    ShowScreenSaver();
                    Application.Run();
                }
                else    // Undefined argument
                {
                    MessageBox.Show("Sorry, but the command line argument \"" + firstArgument +
                        "\" is not valid.", "ScreenSaver",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else    // No arguments - treat like /c
            {
                ConfigurationForm config = new ConfigurationForm();
                Application.Run(config);
            }
        }


        static void ShowScreenSaver()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                
                ScreenSaverForm screensaver = new ScreenSaverForm(screen.Bounds);
                screensaver.Show();
            }
        }


    }
}
