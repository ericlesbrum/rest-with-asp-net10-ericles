using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Files.Exporters.Contract;

namespace rest_with_asp_net10_ericles.Files.Exporters
{
    internal class XlsxExporter : IFileExporter
    {
        public FileContentResult ExportFile(List<PersonDTO> persons)
        {
            throw new NotImplementedException();
        }
    }
}