using MediatR;
using MeuDicionariov2.Infra.Data;
using MeuDicionarioV2.Core.Messaging;
using System.Net;

namespace MeuDicionarioV2.Features.TextCtl
{
    public class RemoveText
    {
        public class Command : IRequest<Result>
        {
            public int Id { get; set; }
        }

        public class Handler : BaseHandler<Command, Result>
        {
            private readonly MyDictionaryDbContex _dbContex;

            public Handler(MyDictionaryDbContex dbContex)
            {
                _dbContex = dbContex;
            }

            public override async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var text = await _dbContex.Texts.FindAsync(request.Id, cancellationToken);

                if(text is null)
                {
                    AddErro("Texto não econtrado");
                    return Error(HttpStatusCode.NotFound);
                }

                await TextFunctions.ClearRelationTextWord(_dbContex, text, cancellationToken);

                _dbContex.Texts.Remove(text);
                await _dbContex.SaveChangesAsync(cancellationToken);

                return Success();
            }
        }
    }
}
