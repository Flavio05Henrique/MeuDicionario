using MediatR;
using MeuDicionarioV2.Core.Controllers;
using MeuDicionarioV2.Core.Paginator;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MeuDicionarioV2.Features.WordCtl
{
    [Route("/word")]
    public class WordController : MainController
    {
        private readonly IMediator _mediator;

        public WordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddWord.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add(AddWord.Command command)
        {
            var result = await _mediator.Send(command);
            return CustomResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _mediator.Send(new RemoveWord.Command
            {
                Id = id
            });

            return CustomResponse(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(QueryResponse<GetWords.Response>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWords(string? search, string? orderBy, int? page, int? pageSize, OrderDirection direction = OrderDirection.ASC)
        {
            var result = await _mediator.Send(new GetWords.Command
            {
                Search = search,
                Direction = direction,
                OrderBy = orderBy,
                Page = page,
                PageSize = pageSize
            });

            return CustomResponse(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(UpdateWord.Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateWord(UpdateWord.Command command)
        {
            var result = await _mediator.Send(command);
            return CustomResponse(result);
        }
    }
}
