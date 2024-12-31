using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questions
{
    public class Question
    {
        public string Text { get; }
        public string Answer { get; }

        public Question(string t, string a)
        {
            Text = t;
            Answer = a;
        }
        public override string ToString()
        {
            return $"{Text}\n{Answer}";
        }
    }
}
