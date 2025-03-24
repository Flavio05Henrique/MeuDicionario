using AutoMapper;
using FluentValidation;
using MeuDicionariov2.Infra.Data;
using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Messaging;
using MeuDicionarioV2.Utilitys;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MeuDicionarioV2.Features.TextCtl
{
    public class AddText
    {
        public class Command : BaseRequest<Result<Response>>
        {
            public string Title { get; set; }
            public string TextItSelf { get; set; }

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
                RuleFor(c => c.Title)
                    .NotEmpty()
                    .WithMessage("O titulo não pode estar vazio")
                    .Length(0, 500)
                    .WithMessage("Titulo de 0 a 500 caracteres");

                RuleFor(c => c.TextItSelf)
                    .NotEmpty()
                    .WithMessage("O texto não pode ser vazio.");
            }


        }
        public class Response
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string TextItSelf { get; set; }
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

                var hasText = await _dbContext.Texts
                    .AnyAsync(c => c.Title.Equals(request.Title), cancellationToken);

                if (hasText)
                {
                    AddErro("Texto com esse titulo já existe.");
                    return Error<Response>(HttpStatusCode.Conflict);
                }

                var text = new Text
                {
                    Title = StringFormat.ToUperFirstChar(request.Title),
                    TextItSelf = request.TextItSelf,
                    WordsInText = TextFunctions.SearchAllWordsInText(request.TextItSelf)
                };

                await _dbContext.Texts.AddAsync(text, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                await TextFunctions.SetRelationTextWord(_dbContext, text, cancellationToken);

                var response = _mapper.Map<Response>(text);
                return Success(response);
            }

            public class MappingProfile : Profile
            {
                public MappingProfile()
                {
                    CreateMap<Command, Word>();
                    CreateMap<Text, Response>();
                }
            }
        }
    }
}