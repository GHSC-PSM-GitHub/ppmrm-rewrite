using System;
using System.Collections.Generic;
using System.Text;

namespace PPMRm.PeriodReports
{
    public static class PeriodReportConsts
    {
        public static class DataValidation
        {
            public const int CSUpdatesMaxLength = 10000;
            public const int ActionRecommendedMaxLength = 2000;

        }

        public static class DataFormatting
        {
            public const string DateFormat = "dd MMM yyyy";
            public const string SOH_AMC_Format = "N0";
            public const string MOS_Format = "N1";
        }

        public static class MOS
        {
            public const int Min = 6;
            public const int Max = 12;
        }
    }
}
