using MediatR;
using MeuDicionarioV2.Core.Controllers;
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
    }
}
