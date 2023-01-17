namespace MagicVilla_VillaApi.Logging
{
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.BackgroundColor= ConsoleColor.Red;
                Console.WriteLine("Error Message" + message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (type == "warning")
            {
                Console.BackgroundColor= ConsoleColor.Green;
                Console.WriteLine("WARNING" + message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
