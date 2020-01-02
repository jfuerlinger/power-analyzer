using System;
using System.Threading.Tasks;

namespace PowerAnalyzer
{
    class Program
    {
        private const string FolderName = @"C:\Users\fujo\OneDrive\02 Finanzen\04 Rechnungen\Strom";

        static async Task Main(string[] args)
        {
            Controller ctrl = new Controller();

            await ctrl
                .WithFolderAnalyser(FolderName)
                .Run();

            ctrl.AnalyzeResults();
        }
    }
}
