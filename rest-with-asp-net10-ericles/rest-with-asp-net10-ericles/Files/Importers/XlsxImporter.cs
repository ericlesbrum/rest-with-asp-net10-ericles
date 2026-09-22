using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Files.Importers.Contract;
using System.Globalization;

namespace rest_with_asp_net10_ericles.Files.Importers;

internal class XlsxImporter : IFileImporter
{
    public Task<List<PersonDTO>> ImportFileAsync(Stream fileStream)
    {
        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheets.First();

        var rows = worksheet.RowsUsed().Skip(1);

        var people = new List<PersonDTO>();

        foreach (var row in rows)
        {
            if (!row.Cell(1).IsEmpty())
            {
                var person = new PersonDTO
                {
                    FirstName = row.Cell(1).GetString(),
                    LastName = row.Cell(2).GetString(),
                    Address = row.Cell(3).GetString(),
                    Gender = row.Cell(4).GetString(),
                    Enabled = true
                };
                people.Add(person);
            }
        }
        return Task.FromResult(people);
    }
}