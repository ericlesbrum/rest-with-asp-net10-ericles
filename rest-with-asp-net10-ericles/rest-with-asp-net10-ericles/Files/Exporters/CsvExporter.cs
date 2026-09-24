using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Files.Exporters.Contract;
using System.Globalization;
using System.Text;

namespace rest_with_asp_net10_ericles.Files.Exporters
{
    internal class CsvExporter : IFileExporter
    {
        public FileContentResult ExportFile(List<PersonDTO> persons)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true);

            using var csv = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            });

            csv.WriteRecord(persons);
            streamWriter.Flush();

            var filesBytes = memoryStream.ToArray();

            return new FileContentResult(filesBytes, MediaTypes.ApplicationCsv)
            {
                FileDownloadName = $"people_exported_{DateTime.UtcNow:ssmmHHddMMyyyy}.csv"
            };
        }
    }
}