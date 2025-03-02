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

        public void Execute()
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

        async private void GetWordsForRevision()
        {
            var currentDate = DateTime.Now;

            var list = await _contex.Words.Where(e => EF.Functions.DateDiffDay(e.LastSeen, currentDate) > 3).ToListAsync();

            foreach (var i in list)
            {
                if (_contex.Revision.Any(e => e.WordRef.Id == i.Id)) return;

                var itemRevision = new Revision();
                itemRevision.SetAttributes(i);

                _contex.Revision.Add(itemRevision);
                _contex.SaveChanges();
            }
        }
    }
}
