using AutoMapper;
using MeuDicionario.Infra;
using MeuDicionario.Model;
using MeuDicionario.Model.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MeuDicionario.Controllers
{
    [ApiController]
    [Route("/palavra")]
    public class WordController : ControllerBase
    {
        private readonly WordDAL _wordDAL;
        private readonly IMapper _mapper;

        public WordController(WordDAL wordDAL, IMapper mapper)
        {
            _wordDAL = wordDAL;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Create([FromBody]WordCreate wordCreate)
        {
            if (wordCreate.Name.Contains(" ")) return BadRequest("Não é uma palavra");
            if (_wordDAL.Has(e => e.Name.Equals(wordCreate.Name))) return Conflict("Palavra já existe");

            var word = _mapper.Map<Word>(wordCreate);

            _wordDAL.Add(word);
            return CreatedAtAction(nameof(FindOne), new {Id = word.Id}, word);
        }

        [HttpDelete("{id}")] 
        public IActionResult Remove(int id)
        {
            var busca = _wordDAL.FindBy(w => w.Id == id);
            if (busca == null) return NotFound();
            _wordDAL.Remove(busca);
            return Ok(busca);
        }

        [HttpGet("{id}")]
        public IActionResult FindOne(int id)
        {
            var word = _wordDAL.FindBy(e => e.Id == id);
            if (word == null) return NotFound("Palavra não existe");
            return Ok(word);
        }

        [HttpGet]
        public IActionResult List([FromQuery]int skip = 0, [FromQuery]int take = 50)
        {
            var list = _wordDAL.GetSome(skip, take);
            return Ok(list);
        }

        [HttpPut("{id}")]
        public IActionResult Change([FromBody] WordCreate word, int id)
        {
            var wordFound = _wordDAL.FindBy(e => e.Id == id);
            if (wordFound == null) return NotFound("Palavra não existe");

            _wordDAL.Update(_mapper.Map(word, wordFound));
            return NoContent();
        }

    }
}
