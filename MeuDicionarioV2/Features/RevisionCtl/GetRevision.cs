using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using MeuDicionario.Model;
using MeuDicionariov2.Infra.Data;
using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Messaging;
using MeuDicionarioV2.Core.Paginator;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace MeuDicionarioV2.Features.RevisionCtl
{
    public class GetRevision
    {
        public class Command : QueryParams, IRequest<Result<QueryResponse<Response>>> { }

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Meaning { get; set; }
        }

        public class Handler : BaseHandler<Command, Result<QueryResponse<Response>>>
        {
            private readonly MyDictionaryDbContex _dbContext;
            private readonly IMapper _mapper;

            public Handler(MyDictionaryDbContex dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public override async Task<Result<QueryResponse<Response>>> Handle(Command request, CancellationToken cancellationToken)
            {
                var query = _dbContext.Revisions
                    .AsQueryable();

                query = query.ApplyPagination(request, out var count);

                var response = await query
                    .Select(e => e.Word)
                    .ProjectTo<Response>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Success(new QueryResponse<Response>
                {
                    Items =  response,
                    Count = count,
                    Page = request.Page.Value,
                    PageSize = request.PageSize
                });
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Revision, Response>();
                CreateMap<Word, Response>();
            }
        }
    }
}
