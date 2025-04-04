﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using MeuDicionariov2.Infra.Data;
using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Enums;
using MeuDicionarioV2.Core.Messaging;
using MeuDicionarioV2.Core.Paginator;
using MeuDicionarioV2.Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeuDicionarioV2.Features.WordCtl
{
    public class GetWords
    {
        public class Command : QueryParams, IRequest<Result<QueryResponse<Response>>>
        {
            public string? Search { get; set; }
        }

        public class CommandConjugation
        {
            public int Id { get; set; }
            public string ConjugationItSelf { get; set; }
            public ConjugationType ConjugationType { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Meaning { get; set; }
            public DateTime CrationDate { get; set; }
            public WordType WordType { get; set; }
            public bool IsRegular { get; set; }
            public List<CommandConjugation>? Conjugations { get; set; }
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
                var query = _dbContext.Words
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Search))
                {
                    query = query
                        .Where(c => c.Name.ToLower().Equals(request.Search.ToLower()));
                }

                query = query.ApplyPagination(request, out var count);

                var response = query
                    .ProjectTo<Response>(_mapper.ConfigurationProvider);

                return Success(new QueryResponse<Response>
                {
                    Items = await response.ToListAsync(cancellationToken),
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
                CreateMap<Word, Response>();
                CreateMap<Conjugation, CommandConjugation>();
            }
        }
    }
}
