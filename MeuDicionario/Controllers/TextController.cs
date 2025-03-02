using MeuDicionario.Model.DTOs;
using MeuDicionario.Model;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MeuDicionario.Infra;

namespace MeuDicionario.Controllers
{
    [ApiController]
    [Route("/texto")]
    public class TextController : ControllerBase
    {
        private readonly MyDictionaryContex _dbContex;
        private readonly IMapper _mapper;

        public TextController(MyDictionaryContex dbContex, IMapper mapper)
        {
            _dbContex = dbContex;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Create([FromBody] TextCreate textCreate)
        {
            var text = _mapper.Map<Text>(textCreate);

            if (string.IsNullOrWhiteSpace(text.Title)) return BadRequest("Sem titulo");
            if (string.IsNullOrWhiteSpace(text.TextItSelf)) return BadRequest("Sem texto");
            if (_dbContex.Texts.Any(e => e.Title.ToLower().Equals(text.Title.ToLower())))
                return Conflict("Titulo já registrado");

            text.SearchAllWordsInText();
            _dbContex.Texts.Add(text);
            text.SetRelationTextWord(_dbContex);
            _dbContex.SaveChanges();
            return CreatedAtAction(nameof(FindById), new { Id = text.Id }, text);
        }

        [HttpPost("{id}")]
        public IActionResult UpdateRelationTextWord(int id)
        {
            var text = _dbContex.Texts.FirstOrDefault(e => e.Id == id);
            if (text == null) return NotFound("Texto não consta");
            text.SetRelationTextWord(_dbContex);
            return Ok();
        }

        [HttpGet]
        public IActionResult List([FromQuery] int skip = 0, [FromQuery] int take = 3)
        {
            var list = _dbContex.Texts.OrderByDescending(e => e.Id).Skip(skip).Take(take);
            if (list.Count() == 0) return NotFound("Sem registros de textos");
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult ListTextWord(int id)
        {
            var list = _dbContex.TextWords.Where(e => e.TextRef.Id == id).Select(e => e.WordRef);
            if (list.Count() == 0) return NotFound("Sem palavras relacionadas");
            return Ok(list);
        }

        public IActionResult FindById(int id)
        {
            var text = _dbContex.Texts.FirstOrDefault(e => e.Id == id);
            if (text == null) return NotFound("Texto não existe");
            return Ok(text);
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var search = _dbContex.Texts.FirstOrDefault(w => w.Id == id);
            if (search == null) return NotFound();
            search.ClearRelationTextWord(_dbContex, search.Id);
            _dbContex.Texts.Remove(search);
            _dbContex.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Change([FromBody] TextCreate text, int id)
        {
            var textFound = _dbContex.Texts.FirstOrDefault(e => e.Id == id);
            if (textFound == null) return NotFound("Texto não existe");
            textFound.SearchAllWordsInText();
            _dbContex.Texts.Update(_mapper.Map(text, textFound));
            _dbContex.SaveChanges();
            return NoContent();
        }
    }
}
