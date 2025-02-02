using MeuDicionario.Infra.DALs;
using MeuDicionario.Model.DTOs;
using MeuDicionario.Model;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace MeuDicionario.Controllers
{
    [ApiController]
    [Route("/texto")]
    public class TextController : ControllerBase
    {
        private readonly TextDAL _textDAL;
        private readonly WordDAL _wordDAL;
        private readonly TextWordDAL _textWordDAL;
        private readonly IMapper _mapper;

        public TextController(TextDAL textDAL, WordDAL wordDAL, TextWordDAL textWordDAL, IMapper mapper)
        {
            _textDAL = textDAL;
            _wordDAL = wordDAL;
            _textWordDAL = textWordDAL;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Create([FromBody] TextCreate textCreate)
        {
            var text = _mapper.Map<Text>(textCreate);

            if (string.IsNullOrWhiteSpace(text.Title)) return BadRequest("Sem titulo");
            if (string.IsNullOrWhiteSpace(text.TextItSelf)) return BadRequest("Sem texto");
            if (_textDAL.Has(e => e.Title.ToLower().Equals(text.Title.ToLower()))) 
                return Conflict("Titulo já registrado");

            text.SearchAllWordsInText();
            _textDAL.Add(text);
            text.SetRelationTextWord(_wordDAL, _textWordDAL);
            return CreatedAtAction(nameof(FindById), new { Id = text.Id }, text);
        }

        [HttpPost("{id}")]
        public IActionResult UpdateRelationTextWord(int id)
        {
            var text = _textDAL.FindBy(e => e.Id == id);
            if (text == null) return NotFound("Texto não consta");
            text.SearchAllWordsInText();
            text.SetRelationTextWord(_wordDAL, _textWordDAL);
            return Ok();
        }

        [HttpGet]
        public IActionResult List([FromQuery] int skip = 0, [FromQuery] int take = 3)
        {
            var list = _textDAL.ListOrderByLastFirst(skip, take, e => e.Id);
            if (list.Count() == 0) return NotFound("Sem registros de textos");
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult ListTextWord(int id)
        {
            var list = _textWordDAL.FindBySome(e => e.TextRef.Id == id).Select(e => e.WordRef);
            if (list.Count() == 0) return NotFound("Sem palavras relacionadas");
            return Ok(list);
        }

        public IActionResult FindById(int id)
        {
            var text = _textDAL.FindBy(e => e.Id == id);
            if (text == null) return NotFound("Texto não existe");
            return Ok(text);
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var search = _textDAL.FindBy(w => w.Id == id);
            if (search == null) return NotFound();
            search.ClearRelationTextWord(_textWordDAL, search.Id);
            _textDAL.Remove(search);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Change([FromBody] TextCreate text, int id)
        {
            var textFound = _textDAL.FindBy(e => e.Id == id);
            if (textFound == null) return NotFound("Texto não existe");
            textFound.SearchAllWordsInText();
            _textDAL.Update(_mapper.Map(text, textFound));
            return NoContent();
        }
    }
}
