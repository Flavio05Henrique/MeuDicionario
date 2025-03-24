using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using MeuDicionariov2.Infra.Data;
using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Messaging;
using MeuDicionarioV2.Core.Paginator;
using MeuDicionarioV2.Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeuDicionarioV2.Features.TextCtl
{
    public class GetTexts
    {
        public class Command : QueryParams, IRequest<Result<QueryResponse<Response>>>
        {
            public string? Search { get; set; }
            public bool WithWords { get; set; }
        }

        public class Response
        {
            public int Id {  get; set; }
            public string Title { get; set; }
            public string TextItSelf { get; set; }
            public List<Word> Words { get; set; }
        }

        public class Handler : BaseHandler<Command, Result<QueryResponse<Response>>>
        {
            private readonly MyDictionaryDbContex _dbContex;
            private readonly IMapper _mapper;

            public Handler(MyDictionaryDbContex dbContex, IMapper mapper)
            {
                _dbContex = dbContex;
                _mapper = mapper;
            }

            public override async Task<Result<QueryResponse<Response>>> Handle(Command request, CancellationToken cancellationToken)
            {
                var query = _dbContex.Texts.AsQueryable();

                if (!string.IsNullOrEmpty(request.Search))
                {
                    query = query
                        .Where(c => c.Title.ToLower().Equals(request.Search.ToLower()));
                }

                query = query.ApplyPagination(request, out var count);

                var response =  await query
                    .ProjectTo<Response>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                if(request.WithWords)
                {
                    foreach(var item in response)
                    {
                        item.Words = await _dbContex.TextWords
                            .Where(e => e.TextId == item.Id)
                            .Include(e => e.Word)
                            .ThenInclude(e => e.Conjugations)
                            .Select(e => e.Word)
                            .ToListAsync(cancellationToken);
                    }
                }

                return Success(new QueryResponse<Response>
                {
                    Items = response,
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
                CreateMap<Text, Response>();
            }
        }

    }
}
