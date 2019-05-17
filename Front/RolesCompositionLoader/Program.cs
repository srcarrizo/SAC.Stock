namespace SAC.Stock.Front.RolesCompositionLoader
{
    using System;
    public class Program
    {
        public static void Main(string[] args)
        {
            ContainerConfig.Initialize();
            SACStockRolesCompositionLoader.Run();
            Console.WriteLine(@"Pulse una tecla para terminar.");
            Console.ReadKey();
        }
    }
}
