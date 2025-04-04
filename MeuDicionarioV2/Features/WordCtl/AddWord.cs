﻿using AutoMapper;
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

                RuleFor(c => c.WordType)
                    .NotEmpty()
                    .WithMessage("O Tipo não pode ser vazio.");
            }
        }

        public class CommandConjugation
        {
            public string ConjugationItSelf { get; set; }
            public ConjugationType ConjugationType { get; set; }
        }

        public class CommandConjugationResponse
        {
            public string ConjugationItSelf { get; set; }
            public string ConjugationType { get; set; }
        }

        public class Response
        {
            public string Name { get; set; }
            public string Meaning { get; set; }
            public DateTime LastSeen { get; set; }
            public WordType WordType { get; set; }
            public List<CommandConjugationResponse> Conjugations { get; set; }
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
                            , cancellationToken);

                if (has)
                {
                    AddErro("A palavra já existe.");
                    return Error<Response>(HttpStatusCode.Conflict);
                }

                if (request.Conjugations is null) request.Conjugations = new List<CommandConjugation>();

                if (!ValidConjugations(request))
                {
                    return Error<Response>();
                }

                var word = new Word
                {
                    IsActive = true,
                    Name = StringFormat.ToUperFirstChar(request.Name),
                    Meaning = request.Meaning,
                    CrationDate = DateTime.Now,
                    WordType = request.WordType,
                    IsRegular = request.IsRegular,
                    Conjugations = _mapper.Map<List<Conjugation>>(request.Conjugations)
                };

                await _dbContext.Words.AddAsync(word, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<Response>(word);
                return Success(response);
            }

            public bool ValidConjugations(Command request)
            {
                try
                {
                    var wordTypeRulePath = "MeuDicionarioV2.Features.WordCtl.AddWord+" + request.WordType.ToString() + "Rule";
                    var rule = Type.GetType(wordTypeRulePath);
                    var wordTypeRule = Activator
                        .CreateInstance(rule);

                    var method = wordTypeRule.GetType().GetMethod("IsValid");

                    var wordTypeRuleResponse = method.Invoke(wordTypeRule, new object[] { request });

                    if (wordTypeRuleResponse.ToString().Count() > 0)
                    {
                        AddErro(wordTypeRuleResponse.ToString());
                        return false;
                    }
                } catch (Exception ex) 
                { 
                    if(request.Conjugations.Count() != 0)
                    {
                        AddErro("Esse tipo de palavra não se conjuga");
                        return false;
                    }
                }

                return true;
            }
        }


        public abstract class WordTypeRuleBase
        {
            protected abstract ConjugationType[] ValidConjugationTypes { get; }
            public abstract string IsValid(Command request);

            public bool IsDuplicate(Command request)
            {
                var conjugationTypes = request.Conjugations.Select(e => e.ConjugationType);

                var hasDuplicateTypes = conjugationTypes
                    .GroupBy(e => e).Any(i => i.Count() > 1);

                return hasDuplicateTypes;
            } 

            public bool HasAllConjugations(Command request)
            {
                var conjugationTypes = request.Conjugations.Select(e => e.ConjugationType);

                var hasAllValidConjugations =
                    ValidConjugationTypes.Any(e => !conjugationTypes.Contains(e));

                return hasAllValidConjugations;
            }

            public bool ExcededLimitConjugations(Command request)
            {
                return request.Conjugations.Count() > ValidConjugationTypes.Count();
            }
        }

        public class VerbRule : WordTypeRuleBase
        {
            protected override ConjugationType[] ValidConjugationTypes => new ConjugationType[]
            {
                    ConjugationType.ThirdPerson,
                    ConjugationType.Preterite,
                    ConjugationType.PresentContinuous,
                    ConjugationType.PaticiplePresent,
                    ConjugationType.PaticiplePass
            };

            public override string IsValid(Command request)
            {
                if (ExcededLimitConjugations(request))
                    return "Limite de conjugações para esse tipo excedido.";

                if (IsDuplicate(request))
                    return "Você está usando duas ou mais conjugações do mesmo tipo.";
                
                if (HasAllConjugations(request))
                    return "Conjugação(es) requeridas não atendidas.";

                return "";
            }
        }

        public class NounRule : WordTypeRuleBase
        {
            protected override ConjugationType[] ValidConjugationTypes => new ConjugationType[]
            {
                    ConjugationType.Plural
            };

            public override string IsValid(Command request)
            {
                if (ExcededLimitConjugations(request))
                    return "Limite de conjugações para esse tipo excedido.";

                if (HasAllConjugations(request))
                    return "Conjugação(es) requeridas não atendidas.";

                return "";
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, Word>();
                CreateMap<Word, Response>();
                CreateMap<CommandConjugation, Conjugation>();
                CreateMap<Conjugation, CommandConjugation>();
                CreateMap<Conjugation, CommandConjugationResponse>();
            }
        }

    }
}
