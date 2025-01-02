using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questions
{
    public class Run
    {
        public int Length { get; }
        public bool IsOver { get; private set; }
        private List<Question> questions;
        private Random r;

        public Run(List<Question> q)
        {
            questions = new List<Question>(q);
            Length = questions.Count;
            r = new Random();
            IsOver = questions.Count <= 0;
        }

        public Question Next()
        {
            int ind = r.Next(questions.Count);
            Question retval = questions[ind];
            questions.Remove(retval);
            if (questions.Count <= 0)
            {
                IsOver = true;
            }
            return retval;
        }
    }
}
