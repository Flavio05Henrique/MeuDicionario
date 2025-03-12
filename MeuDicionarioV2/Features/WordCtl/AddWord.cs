using AutoMapper;
using FluentValidation;
using MeuDicionariov2.Infra.Data;
using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Enums;
using MeuDicionarioV2.Core.Messaging;
using MeuDicionarioV2.Infra.Data.Entities;
using MeuDicionarioV2.Utilitys;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;


namespace MeuDicionarioV2.Features.WordCtl
{
    public class AddWord
    {
        public class Command : BaseRequest<Result<Response>>
        {
            public string Name { get; set; }
            public string Meaning { get; set; }
            public WordType WordType { get; set; }
            public bool IsRegular { get; set; }
            public List<CommandConjugation>? Conjugations { get; set; }

            public override bool IsValid()
            {
                ValidationResult = new CommandValidator().Validate(this);
                return ValidationResult.IsValid;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithMessage("O nome não pode estar vazio")
                    .Matches(@"^\S*$").WithMessage("O nome não pode conter espaços.")
                    .WithMessage("Não é uma palavra");

                RuleFor(c => c.Meaning)
                    .NotEmpty()
                    .WithMessage("O significado não pode ser vazio.");
            }
        }

        public class CommandConjugation
        {
            public string ConjugationItSelf { get; set; }
            public ConjugationType ConjugationType { get; set; }
        }

        public class Response
        {
            public string Name { get; set; }
            public string Meaning { get; set; }
            public DateTime LastSeen { get; set; }
            public WordType WordType { get; set; }
            public List<Conjugation> Conjugations { get; set; } 
        }

        public class Handler : BaseHandler<Command, Result<Response>>
        {
            private readonly MyDictionaryDbContex _dbContext;
            private readonly IMapper _mapper;

            public Handler(MyDictionaryDbContex dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public override async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!request.IsValid())
                {
                    AddErros(request.ValidationResult);
                    return Error<Response>();
                }

                var has = await _dbContext.Words
                    .AnyAsync(c => 
                        c.Name.Equals(request.Name) && c.WordType == request.WordType
                            ,cancellationToken);

                if (has)
                {
                    AddErro("A palavra já existe.");
                    return Error<Response>(HttpStatusCode.Conflict);
                }

                var word = new Word
                {
                    Name = StringFormat.ToUperFirstChar(request.Name),
                    Meaning = request.Meaning,
                    CrationDate = DateTime.Now,
                    LastSeen = DateTime.UtcNow,
                    WordType = request.WordType,
                    IsRegular = request.IsRegular,
                    Conjugations = _mapper.Map<List<Conjugation>>(request.Conjugations)
                };
                
                await _dbContext.Words.AddAsync(word, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<Response>(word);
                return Success(response);
            }

            public class MappingProfile : Profile
            {
                public MappingProfile()
                {
                    CreateMap<Command, Word>();
                    CreateMap<Word, Response>();
                    CreateMap<CommandConjugation, Conjugation>();
                }
            }
        }

    }
}
