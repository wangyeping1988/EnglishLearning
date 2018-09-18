using JasonWang.Dal.EntityFrameworkDal;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class WordManager : COMManager<Word>
    {
        public WordManager()
            : base(true)
        {
        }

        public static WordManager Instance = new WordManager();
    }
}
