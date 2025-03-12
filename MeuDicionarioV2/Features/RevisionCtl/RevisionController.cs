using MediatR;
using MeuDicionarioV2.Core.Controllers;
using MeuDicionarioV2.Core.Paginator;
using MeuDicionarioV2.Features.WordCtl;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MeuDicionarioV2.Features.RevisionCtl
{
    [Route("/revision")]
    public class RevisionController : MainController
    {
        private readonly IMediator _mediator;

        public RevisionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(QueryResponse<GetWords.Response>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRevisions(int? page = 1, int? pageSize = 15)
        {
            var result = await _mediator.Send(new GetRevision.Command
            {
                Page = page,
                PageSize = pageSize
            });

            return CustomResponse(result);
        }

        [HttpDelete()]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Remove([FromBody] int[] list)
        {
            var result = await _mediator.Send(new RemoveRangeRevision.Command
            {
                list = list
            });

            return CustomResponse(result);
        }
    }
}
