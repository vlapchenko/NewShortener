using System;
using NFX.ApplicationModel;
using NFX.Wave;


namespace NewShortener
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var app = new ServiceBaseApplication(args, null))
                using (var server = new WaveServer())
                {
                    server.Configure(null);
                    server.Start();
                    Console.WriteLine("server started...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Critical error:");
                Console.WriteLine(ex.Message);
                Environment.ExitCode = -1;
                Console.ReadLine();
            }
        }
    }
}
