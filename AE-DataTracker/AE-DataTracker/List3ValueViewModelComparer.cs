using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE_DataTracker
{
    public class List3ValueViewModelComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var item1 = x as List3ValueViewModel;
            var item2 = y as List3ValueViewModel;

            if (double.TryParse(item1?.Value3, out double d1) &&
                double.TryParse(item2?.Value3, out double d2))
            {
                return d1.CompareTo(d2);
            }

            return string.Compare(item1?.Value3, item2?.Value3, StringComparison.Ordinal);
        }
    }
}
