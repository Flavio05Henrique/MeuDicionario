using MediatR;
using MeuDicionariov2.Infra.Data;
using MeuDicionarioV2.Core.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MeuDicionarioV2.Features.RevisionCtl
{
    public class RemoveRangeRevision
    {
        public class Command : IRequest<Result>
        {
            public int[] list { get; set; }
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
                if(!request.list.Any())
                {
                    AddErro("Nenhum item fornecido para exclusão.");
                    return Error(HttpStatusCode.NotFound);
                }

                var revisions =  await _dbContext.RevisionLogs
                    .Where(e => request.list.Contains(e.Word.Id)).ToArrayAsync(cancellationToken);
                _dbContext.RevisionLogs.RemoveRange(revisions);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return Success();
            }
        }
    }
}
