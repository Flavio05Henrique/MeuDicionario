using MeuDicionario.Infra.DALs;
using MeuDicionario.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeuDicionario.Controllers
{
    [ApiController]
    [Route("/revisao")]
    public class RevisionController : ControllerBase
    {
        private readonly RevisionDAL _revisionDAL;

        public RevisionController(RevisionDAL revisionDAL)
        {
            _revisionDAL = revisionDAL;
        }

        [HttpGet]
        public IActionResult List([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var contex = _revisionDAL.GetContex();

            var list = contex.Set<Revision>().Include(r => r.WordRef).ToList();
            if (list.Count() == 0) return NotFound("Sem registros");

            return Ok(list);
        }

        [HttpDelete]
        public IActionResult DeleteSome([FromBody] int[] list)
        {
            if (!list.Any()) return BadRequest("Nenhum item fornecido para exclusão.");

            foreach(var item in list)
            {
                var revisionFound = _revisionDAL.FindBy(e => e.Id == item);
                if (revisionFound != null)
                {
                    _revisionDAL.Remove(revisionFound);
                }
            }

            return NoContent();
        }
    }
}
