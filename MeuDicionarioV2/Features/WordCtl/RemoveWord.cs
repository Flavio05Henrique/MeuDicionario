using MediatR;
using MeuDicionariov2.Infra.Data;
using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MeuDicionarioV2.Features.WordCtl
{
    public class RemoveWord
    {
        public class Command : IRequest<Result>
        {
            public int Id { get; set; }
        }

        public class Handler : BaseHandler<Command, Result>
        {
            private readonly MyDictionaryDbContex _dbContext;

            public Handler(MyDictionaryDbContex dbContext)
            {
                _dbContext = dbContext;
            }

            public override async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var word = await _dbContext.Words
                    .FindAsync(request.Id, cancellationToken);

                if (word is null)
                {
                    AddErro("Tarefa não encontrada");
                    return Error(HttpStatusCode.NotFound);
                }

                await RemoveRelations(word, cancellationToken);

                _dbContext.Words.Remove(word);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Success();
            }
            async private Task RemoveRelations(Word word, CancellationToken cancellationToken)
            {
                var revisions = await _dbContext.RevisionLogs.Where(e => e.Word.Id == word.Id).ToArrayAsync(cancellationToken);
                if (revisions != null) _dbContext.RevisionLogs.RemoveRange(revisions);

                var texts = await _dbContext.TextWords.Where(e => e.Word.Id == word.Id).ToArrayAsync(cancellationToken);
                if (texts != null) _dbContext.TextWords.RemoveRange(texts);
            }
        }

        
    }
}
