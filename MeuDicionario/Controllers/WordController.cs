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
        private readonly MyDictionaryContex _dbContex;
        private readonly IMapper _mapper;

        public WordController(MyDictionaryContex dbContex, IMapper mapper)
        {
            _dbContex = dbContex;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Create([FromBody]WordCreate wordCreate)
        {
            if (wordCreate.Name.Contains(" ")) return BadRequest("Não é uma palavra");
            var word = _mapper.Map<Word>(wordCreate);
            if (_dbContex.Words.Any(e => e.Name.Equals(word.Name))) return Conflict("Palavra já existe");

            _dbContex.Words.Add(word);
            _dbContex.SaveChanges();

            return CreatedAtAction(nameof(FindById), new {Id = word.Id}, word);
        }

        [HttpDelete("{id}")] 
        public IActionResult Remove(int id)
        {
            var searchWord = _dbContex.Words.FirstOrDefault(e => e.Id == id);
            if (searchWord == null) return NotFound();

            ClearWordRelactions(searchWord);

            _dbContex.Words.Remove(searchWord);
            _dbContex.SaveChanges();
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult FindById(int id)
        {
            var word = _dbContex.Words.FirstOrDefault(e => e.Id == id);
            if (word == null) return NotFound("Palavra não existe");
            return Ok(word);
        }

        [HttpGet("search")]
        public IActionResult FindByWord([FromQuery]string word)
        {
            var w = new Word(word, "");
            var wordSearch = _dbContex.Words.FirstOrDefault(e => e.Name.Equals(w.Name));
            if (wordSearch == null) return NotFound("Palavra não existe");
            Console.WriteLine(wordSearch);
            return Ok(wordSearch);  
        }

        [HttpGet]
        public IActionResult List([FromQuery]int skip = 0, [FromQuery]int take = 3)
        {
            var list = _dbContex.Words.OrderByDescending(e => e.Id).Skip(skip).Take(take);
            if (list.Count() == 0) return NotFound("Sem registros");
            return Ok(list);
        }

        [HttpPut("{id}")]
        public IActionResult Change([FromBody] WordCreate word, int id)
        {
            var wordFound = _dbContex.Words.FirstOrDefault(e => e.Id == id);
            if (wordFound == null) return NotFound("Palavra não existe");

            _dbContex.Words.Update(_mapper.Map(word, wordFound));
            return NoContent();
        }

        public void ClearWordRelactions(Word word)
        {
            var searchRevision = _dbContex.RevisionV3.FirstOrDefault(e => e.WordRef.Id == word.Id);
            if (searchRevision != null)
            {
                _dbContex.RevisionV3.Remove(searchRevision);
            }
            var searchWordInText = _dbContex.TextWords.Where(e => e.TextRef.Id == word.Id);
            if(searchWordInText.Count() > 0)
            {
                _dbContex.TextWords.RemoveRange(searchWordInText);
            }
            _dbContex.SaveChanges();
        }

    }
}
