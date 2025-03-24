using AutoMapper;
using FluentValidation;
using MeuDicionariov2.Infra.Data;
using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Enums;
using MeuDicionarioV2.Core.Messaging;
using MeuDicionarioV2.Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MeuDicionarioV2.Features.WordCtl
{
    public class UpdateWord
    {
        public class Command : BaseRequest<Result<Response>>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Meaning { get; set; }
            public bool IsRegular { get; set; }
            public List<CommandConjugationRequest>? Conjugations { get; set; }

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

        public class CommandConjugationRequest
        {
            public string ConjugationItSelf { get; set; }
            public ConjugationType ConjugationType { get; set; }
        }

        public class CommandConjugationResponse : CommandConjugationRequest
        {
            public int Id { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Meaning { get; set; }
            public DateTime CrationDate { get; set; }
            public WordType WordType { get; set; }
            public bool IsRegular { get; set; }
            public List<CommandConjugationResponse>? Conjugations { get; set; }
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

                var word = await _dbContext.Words
                    .Include(e => e.Conjugations)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);

                if (word is null)
                {
                    AddErro("Palavra não encontrada");
                    return Error<Response>(HttpStatusCode.NotFound);
                }

                word.Name = request.Name;
                word.Meaning = request.Meaning;
                word.IsRegular = request.IsRegular;
                if(!SetConjugations(request, word))
                {
                    return Error<Response>();
                }

                _dbContext.Words.Update(word);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<Response>(word);
                return Success(response);
            }

            public bool SetConjugations(Command request, Word word)
            {
                if (word.Conjugations.Count() < 1) return true;
                foreach(var item in request.Conjugations)
                {
                    var has = word.Conjugations.FindIndex(e => e.ConjugationType == item.ConjugationType);
                    if (has < 0)
                    {
                        AddErro("Tentativa de auterar uma conjução invalida.");
                        return false;
                    }
                    word.Conjugations[has].ConjugationItSelf = item.ConjugationItSelf;
                }
                return true;
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Word, Response>();
                CreateMap<Command, Word>(); ;
                CreateMap<CommandConjugationRequest, Conjugation>();
                CreateMap<Conjugation, CommandConjugationResponse>();
            }
        }
    }
}
