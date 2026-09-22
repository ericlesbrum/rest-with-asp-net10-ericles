using rest_with_asp_net10_ericles.Data.DTO.V2;

namespace rest_with_asp_net10_ericles.Files.Importers.Contract;

public interface IFileImporter
{
    Task<List<PersonDTO>> ImportFileAsync(Stream fileStream);
}
