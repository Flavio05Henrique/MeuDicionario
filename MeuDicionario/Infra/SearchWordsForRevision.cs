using MeuDicionario.Infra.DALs;
using MeuDicionario.Model;

namespace MeuDicionario.Infra
{
    public class SearchWordsForRevision
    {
        private RevisionDAL _revionContex;
        private RevisionLogDAL _revionLogContex;
        private WordDAL _wordDAL;

        public SearchWordsForRevision(RevisionDAL revionContex, RevisionLogDAL revionLogContex, WordDAL wordDAL)
        {
            _revionContex = revionContex;
            _revionLogContex = revionLogContex;
            _wordDAL = wordDAL;
        }

        public void Execute()
        {
            if(_revionLogContex.GetCount() > 0)
            {
                var differenceBetweenTodayLastDate =
                    Math.Abs((_revionLogContex.GetLast(e => e.Id).Date - DateTime.Now).Days);

                if (differenceBetweenTodayLastDate == 0) return;
            }

            UpDateLastRevision();
            GetWordsForRevision();
        }

        private void UpDateLastRevision()
        {
            if (_revionLogContex.GetCount() >= 10)
            {
                _revionLogContex.Remove(_revionLogContex.GetFirst());
            }

            var currentDate = DateTime.Now;
            _revionLogContex.Add(new RevisionLog(currentDate));
        }

        private void GetWordsForRevision()
        {
            var currentDate = DateTime.Now;

            var list = _wordDAL.FindBySome(e => Math.Abs((e.LastSeen - currentDate).Days) > 7);

            foreach (var i in list)
            {
                if (_revionContex.Has(e => e.WordRef.Id == i.Id)) return;

                var itemRevision = new Revision();
                itemRevision.SetAttributes(i);

                _revionContex.Add(itemRevision);
            }
        }
    }
}
