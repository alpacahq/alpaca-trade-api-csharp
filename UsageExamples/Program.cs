using System;
using System.Threading.Tasks;

namespace UsageExamples
{
    internal static class Program
    {
        public static async Task Main()
        {
            try
            {
                var algorithm = new MeanReversionBrokerage();
                await algorithm.Run();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }

            Console.Read();
        }
    }
}
