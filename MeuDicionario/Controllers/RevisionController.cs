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
        private readonly WordDAL _wordDAL;
        public RevisionController(RevisionDAL revisionDAL, WordDAL wordDAL)
        {
            _revisionDAL = revisionDAL;
            _wordDAL = wordDAL;
        }

        [HttpGet]
        public IActionResult List([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var contex = _revisionDAL.GetContex();

            var list = contex.Set<Revision>().Include(r => r.WordRef).Take(50).ToList();
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
                    var wordFound = _wordDAL.FindBy(e => e.Id == revisionFound.WordRef.Id);
                    wordFound.LastSeen = DateTime.Now;
                    _wordDAL.Update(wordFound);

                    _revisionDAL.Remove(revisionFound);
                }
            }

            return NoContent();
        }
    }
}
