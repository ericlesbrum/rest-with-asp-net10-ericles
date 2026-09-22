using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Files.Importers.Contract;

namespace rest_with_asp_net10_ericles.Files.Importers;

internal class CsvFileImporter : IFileImporter
{
    public Task<List<PersonDTO>> ImportFileAsync(Stream fileStream)
    {
        throw new NotImplementedException();
    }
}