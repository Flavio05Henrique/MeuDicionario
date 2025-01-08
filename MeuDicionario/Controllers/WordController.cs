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
            var word = _mapper.Map<Word>(wordCreate);
            if (_wordDAL.Has(e => e.Name.Equals(word.Name))) return Conflict("Palavra já existe");


            _wordDAL.Add(word);
            return CreatedAtAction(nameof(FindById), new {Id = word.Id}, word);
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
        public IActionResult FindById(int id)
        {
            var word = _wordDAL.FindBy(e => e.Id == id);
            if (word == null) return NotFound("Palavra não existe");
            return Ok(word);
        }

        [HttpGet("search")]
        public IActionResult FindByWord([FromQuery]string word)
        {
            var w = new Word(word, "");
            var wordSearch = _wordDAL.FindBy(e => e.Name.Equals(w.Name));
            if (wordSearch == null) return NotFound("Palavra não existe");
            Console.WriteLine(wordSearch);
            return Ok(wordSearch);  
        }

        [HttpGet]
        public IActionResult List([FromQuery]int skip = 0, [FromQuery]int take = 3)
        {
            var list = _wordDAL.GetSome(skip, take, e => e.Id);
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
