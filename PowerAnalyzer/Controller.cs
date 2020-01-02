using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using BetterConsoleTables;
using PowerAnalyzer.Model;

namespace PowerAnalyzer
{
    class Controller
    {
        private string _folderName;

        private List<Measurement> _measurements;

        public Controller()
        {
            _measurements = new List<Measurement>();
        }

        public async Task<Controller> Run()
        {
            await foreach (string line in ReadLinesFromCsvFilesAsync(_folderName))
            {
                _measurements.Add(ConvertToMeasurement(line));
            }

            return this;
        }

        private Measurement ConvertToMeasurement(string line)
        {
            string[] parts = line.Split(";");
            return new Measurement(DateTime.Parse(parts[0]), Double.Parse(parts[1]));
        }

        public Controller WithFolderAnalyser(string folderName)
        {
            _folderName = folderName ?? throw new ArgumentNullException(nameof(folderName));
            return this;
        }

        public void AnalyzeResults()
        {
            _measurements.Sort();
            
            Console.WriteLine($"Read {_measurements.Count} Measurements with a sum of {_measurements.Sum(_=>_.Value):F2} kWh.");

            Console.WriteLine();

            Table table = new Table {Config = TableConfiguration.Default()};
            Console.WriteLine(table
                .From(
                    _measurements
                        .GroupBy(measurement => measurement.IsDay)
                        .Select(group => 
                            new
                            {
                                Name = group.Key == true ? "Tag" : "Nacht",
                                kWh = group.Sum(_=>_.Value).ToString("F2")
                            })
                        .ToList()
                    )
                .ToString());
        }

        private async IAsyncEnumerable<string> ReadLinesFromCsvFilesAsync(string folderName)
        {
            var ext = new List<string> { ".csv" };
            var fileReadTasks =
                Directory
                    .EnumerateFiles(folderName, "*.*", SearchOption.AllDirectories)
                    .Where(s => ext.Contains(Path.GetExtension(s).ToLowerInvariant()))
                    .Select(fileName => File.ReadAllLinesAsync(fileName, Encoding.UTF8))
                    .ToArray();



            await Task.WhenAll(fileReadTasks);


            foreach (var valuesFromFile in fileReadTasks
                .Select(tasks => tasks.Result))
            {
                for (int i = 1; i < valuesFromFile.Length; i++)
                {
                    yield return valuesFromFile[i];
                }
            }

            //yield return null;
        }

    }
}
