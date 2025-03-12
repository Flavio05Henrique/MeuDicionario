using MeuDicionario.Infra;
using MeuDicionario.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeuDicionario.Controllers
{
    [ApiController]
    [Route("/revisao")]
    public class RevisionController : ControllerBase
    {

        private readonly MyDictionaryContex _dbContex;

        public RevisionController(MyDictionaryContex dbContex)
        {
            _dbContex = dbContex;
        }

        [HttpGet]
        public IActionResult List([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var list = _dbContex.RevisionV3.Include(r => r.WordRef).Take(50).ToList();
            if (list.Count() == 0) return NotFound("Sem registros");

            return Ok(list);
        }

        [HttpDelete]
        public IActionResult DeleteSome([FromBody] int[] list)
        {
            if (!list.Any()) return BadRequest("Nenhum item fornecido para exclusão.");

            foreach(var item in list)
            {
                var revisionFound = _dbContex.RevisionV3.FirstOrDefault(e => e.Id == item);
                if (revisionFound != null)
                {
                    var wordFound = _dbContex.Words.FirstOrDefault(e => e.Id == revisionFound.WordRef.Id);
                    wordFound.LastSeen = DateTime.Now;
                    _dbContex.Words.Update(wordFound);

                    _dbContex.RevisionV3.Remove(revisionFound);
                }
            }

            _dbContex.SaveChanges();

            return NoContent();
        }
    }
}
