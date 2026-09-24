using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Data.DTO.V2;

namespace rest_with_asp_net10_ericles.Files.Exporters.Contract;

public interface IFileExporter
{
    FileContentResult ExportFile(List<PersonDTO> persons);
}
