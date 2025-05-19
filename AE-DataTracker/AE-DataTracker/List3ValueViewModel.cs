using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE_DataTracker
{
    public class List3ValueViewModel
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public List3ValueViewModel(string val1, string val2, string val3)
        {
            Value1 = val1;
            Value2 = val2;
            Value3 = val3;
        }

        public bool Compare(List3ValueViewModel other)
        {
            return string.Compare(Value3, other.Value3) > 0;
        }
    }
}
