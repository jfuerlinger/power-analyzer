using System;
using System.Threading;
using System.Threading.Tasks;

namespace PowerAnalyzer
{
    class Program
    {
        private const string FolderName = @"C:\Users\fujo\OneDrive\02 Finanzen\04 Rechnungen\Strom";
        private const string OutputFileName = @"C:\Users\fujo\OneDrive\02 Finanzen\04 Rechnungen\Strom\2019_Summe.csv";

        static async Task Main(string[] args)
        {
            Controller ctrl = new Controller();

            ctrl
                .WithFolderAnalyser(FolderName, "Summe")
                .Run();

            ctrl.AnalyzeResults();


            CancellationTokenSource src = new CancellationTokenSource();
            ctrl.WriteAllMeasurementsToFileAsync(OutputFileName, src.Token);
        }
    }
}
