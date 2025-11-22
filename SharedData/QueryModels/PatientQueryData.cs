using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.QueryModels
{
    public class PatientQueryData
    {
        private int? _take;
        private int? _pageIndex;

        public string? NameorPhoneNumber;
        public DateOnly? date;

        public int? Take
        {
            get { return _take; }
            set
            {
                if (value.HasValue && value.Value > 0 && value < 10)
                    _take = value;
                else
                    _take = 10;
            }
        }
        public int? PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (value.HasValue && value.Value >= 0)
                    _pageIndex = value;
                else
                    _pageIndex = 0;
            }
        }
    }
}
