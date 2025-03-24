using MediatR;
using MeuDicionarioV2.Core.Controllers;
using MeuDicionarioV2.Core.Paginator;
using MeuDicionarioV2.Features.WordCtl;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MeuDicionarioV2.Features.TextCtl
{
    [Route("/texts")]
    public class TextContoller : MainController
    {
        private readonly IMediator _mediator;

        public TextContoller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddText.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add(AddText.Command command)
        {
            var result = await _mediator.Send(command);
            return CustomResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _mediator.Send(new RemoveText.Command
            {
                Id = id
            });

            return CustomResponse(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(QueryResponse<GetTexts.Response>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWords(
            bool withWords,string? search, string? orderBy, int? page, int? pageSize, 
            OrderDirection direction = OrderDirection.ASC)
        {
            var result = await _mediator.Send(new GetTexts.Command
            {
                Search = search,
                Direction = direction,
                OrderBy = orderBy,
                Page = page,
                PageSize = pageSize,
                WithWords = withWords
            });

            return CustomResponse(result);
        }
    }
}
