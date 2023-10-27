using AutoMapper;
using Grpc.Core;
using GrpcService.Protos;
using Services.Interfaces;

namespace GrpcService.Services
{
    public class PeopleService : GrpcService.Protos.PeopleService.PeopleServiceBase
    {
        private IPeopleService _service;
        private IMapper _mapper;

        public PeopleService(IPeopleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public override async Task<Person> Create(Person request, ServerCallContext context)
        {
            var person = _mapper.Map<Models.Person>(request);
            person = await _service.CreateAsync(person);

            return _mapper.Map<Person>(person);
        }

        public override async Task<People> Read(Protos.Void request, ServerCallContext context)
        {
            var people = await _service.ReadAsync();
            var result = new People();
            result.Collection.AddRange(_mapper.Map<IEnumerable<Person>>(people));
            return result;
        }


        public override async Task<Person> ReadById(Id request, ServerCallContext context)
        {
            var person = await _service.ReadAsync(request.Value);

            return _mapper.Map<Person>(person);
        }
    }
}
