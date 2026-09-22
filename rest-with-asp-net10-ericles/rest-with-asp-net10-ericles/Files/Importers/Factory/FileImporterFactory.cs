using rest_with_asp_net10_ericles.Files.Importers.Contract;

namespace rest_with_asp_net10_ericles.Files.Importers.Factory;

public class FileImporterFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FileImporterFactory> _logger;

    public IFileImporter GetImporter(string fileName)
    {
        if (fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation(
                    "Selected CSV file importer for file: {FileName}", fileName);
            return _serviceProvider.GetRequiredService<CsvFileImporter>();
        }
        else if (fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation(
                    "Selected Excel file importer for file: {FileName}", fileName);

            return _serviceProvider.GetRequiredService<XlsxImporter>();
        }
        else
        {
            _logger.LogError("Unsupported file format: {FileName}", fileName);
            throw new NotSupportedException($"The file format of '{fileName}' is not supported.");
        }
    }
}
