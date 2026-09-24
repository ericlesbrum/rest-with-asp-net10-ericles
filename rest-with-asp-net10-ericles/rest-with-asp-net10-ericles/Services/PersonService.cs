using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Files.Exporters.Factory;
using rest_with_asp_net10_ericles.Files.Importers.Factory;
using rest_with_asp_net10_ericles.Hypermedia.Utils;
using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Repositories.Interfaces;
using rest_with_asp_net10_ericles.Services.Interfaces;
using Serilog.Core;

namespace rest_with_asp_net10_ericles.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repositoryPerson;
        private readonly FileImporterFactory _fileImporterFactory;
        private readonly FileExporterFactory _fileExporterFactory;
        private readonly ILogger<PersonService> _logger;

        public PersonService(IPersonRepository personRepository, FileImporterFactory fileImporterFactory, FileExporterFactory fileExporterFactory,
            ILogger<PersonService> logger)
        {
            _repositoryPerson = personRepository;
            _fileImporterFactory = fileImporterFactory;
            _fileExporterFactory = fileExporterFactory;
            _logger = logger;
        }

        public PersonDTO Create(PersonDTO person)
        {
            var entity = person.Adapt<Person>();
            entity = _repositoryPerson.Create(entity);
            return entity.Adapt<PersonDTO>();
        }
        public bool Delete(long id)
        {
            return _repositoryPerson.Delete(id);
        }

        public List<PersonDTO> FindAll()
        {
            return _repositoryPerson.FindAll().Adapt<List<PersonDTO>>();
        }

        public PersonDTO FindById(long id)
        {
            return _repositoryPerson.FindById(id).Adapt<PersonDTO>();
        }

        public PersonDTO Update(PersonDTO person)
        {
            var entity = person.Adapt<Person>();
            entity = _repositoryPerson.Update(entity);
            return entity.Adapt<PersonDTO>();
        }

        public PersonDTO? Disable(long id)
        {
            var person = _repositoryPerson.Disable(id);
            return person?.Adapt<PersonDTO>();
        }

        public List<PersonDTO> FindByName(string firstName, string lastName)
        {
            return _repositoryPerson.FindByName(firstName, lastName).Adapt<List<PersonDTO>>();
        }

        public PagedSearchDTO<PersonDTO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var pageResult = _repositoryPerson.FindWithPagedSearch(name, sortDirection, pageSize, page);
            return pageResult.Adapt<PagedSearchDTO<PersonDTO>>();
        }

        public async Task<List<PersonDTO>> MassCreationAsync(IFormFile file)
        {
            if(file == null || file.Length == 0 )
            {
                _logger.LogError("File is null or empty.");
                throw new ArgumentException("File is null or empty.");
            }

            using var stream = file.OpenReadStream();
            var fileName = file.FileName;
            try
            {
                var importer = _fileImporterFactory.GetImporter(fileName);
                var persons = await importer.ImportFileAsync(stream);

                var entities = persons.Select(dto => _repositoryPerson.Create(dto.Adapt<Person>())).ToList();

                return entities.Adapt<List<PersonDTO>>();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during mass creation from file: {FileName}", file.FileName);
                throw;
            }
        }

        public FileContentResult ExportPage(int page, int pageSize, string sortDirection, string acceptHeader, string name)
        {
            _logger.LogInformation(
                "Exporting page: {page}, {pageSize}, {sortDirection}, {acceptHeader}, {name}",
                page, pageSize, sortDirection, acceptHeader, name);
            var content = FindWithPagedSearch(name, sortDirection, pageSize, page);

            try
            {
                var exporter = _fileExporterFactory.GetExporter(acceptHeader);
                var persons = content.List.Adapt<List<PersonDTO>>();
                return exporter.ExportFile(persons);
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "Unsupported export format requested: {AcceptHeader}", acceptHeader);
                throw;
            }

            
        }
    }
}