using MeuDicionario.Model;
using Microsoft.EntityFrameworkCore;

namespace MeuDicionario.Infra
{
    public class SearchWordsForRevision
    {
        private MyDictionaryContex _contex;

        public SearchWordsForRevision(MyDictionaryContex contex)
        {
            _contex = contex;
        }

        public async void Execute()
        {
            //if(_contex.RevisionLogs.Count() > 0)
            //{
            //    var differenceBetweenTodayLastDate =
            //        Math.Abs((_contex.RevisionLogs.OrderByDescending(e => e.Id).First().Date - DateTime.Now).Days);

            //    if (differenceBetweenTodayLastDate == 0) return;
            //}

            UpDateLastRevision();
            GetWordsForRevision();
        }

        private void UpDateLastRevision()
        {
            if (_contex.RevisionLogs.Count() >= 10)
            {
                _contex.RevisionLogs.Remove(_contex.RevisionLogs.First());
            }

            var currentDate = DateTime.Now;
            _contex.RevisionLogs.Add(new RevisionLog(currentDate));
            _contex.SaveChanges();
        }

         private void GetWordsForRevision()
        {
            var currentDate = DateTime.Now;

            var list =  _contex.Words.Where(e => EF.Functions.DateDiffDay(e.LastSeen, currentDate) > 3).ToList();

            if (list is null) return;

            foreach (var word in list)
            {
                //Console.WriteLine(i.Name + " " + list.Count);
                if (_contex.RevisionV3.Any(e => e.WordRef.Id == word.Id)) continue;

                var itemRevision = new RevisionV3(word);

                _contex.RevisionV3.Add(itemRevision);
            }
            _contex.SaveChanges();
        }
    }
}
