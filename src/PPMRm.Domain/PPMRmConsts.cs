namespace PPMRm
{
    public static class PPMRmConsts
    {
        public const string DbTablePrefix = "App";

        public const string DbSchema = null;

        public static class Permissions
        {
            public const string Core = "Core";
            public const string DataProvider = "DataProvider";
            public const string DataReviewer = "DataReviewer";
        }

        public static class ProductTracerCategories
        {
            public const string ACTs = "ACTs";
            public const string mRDTs = "mRDTs";
            public const string SevereMalariaMeds = "Severe Malaria Meds";
            public const string SP = "SP";
            public const string OtherPharma = "Other Pharma";

            public static string[] All => new[] {ACTs, mRDTs, SevereMalariaMeds, SP, OtherPharma};
        }

        public static class DataFormats
        {
            public const string DateDisplayFormat = "yyyy-MM-dd";
        }
    }
}
