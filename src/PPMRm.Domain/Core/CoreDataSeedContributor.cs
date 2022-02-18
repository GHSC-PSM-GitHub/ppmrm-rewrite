using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using System.Linq;

namespace PPMRm.Core
{
    public class CoreDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private IRepository<Country, string> CountryRepository { get; }
        private IRepository<Program, int> ProgramRepository { get; }
        private IRepository<Period, int> PeriodRepository { get; }

        public CoreDataSeedContributor(IRepository<Country, string> countryRepository, IRepository<Program, int> programRepository, IRepository<Period, int> periodRepository)
        {
            CountryRepository = countryRepository;
            ProgramRepository = programRepository;
            PeriodRepository = periodRepository;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            if(await CountryRepository.GetCountAsync() <= 0)
            {
                var countries = List.Where(c => c.IsDeleted == false);
                await CountryRepository.InsertManyAsync(countries);
            }

            if (await ProgramRepository.GetCountAsync() <= 0)
            {
                var programs = new List<Program>
                {
                    new Program(Programs.MDG3, "3MDG"),
                    new Program(Programs.CAPMalaria, "CAP-Malaria"){ IsDeleted = true },
                    new Program(Programs.CambodiaMEP, "Cambodia Malaria Elimination Program- CMEP"),
                    new Program(Programs.NationalMalariaProgram, "National Malaria Program"),
                    new Program(Programs.PNFP, "PNFP Sector"),
                    new Program(Programs.PartnersWorkingBeyondFPH, "Partners Working Beyond Formal Public Health"),
                    new Program(Programs.PSI, "Population Services International (PSI)"),
                    new Program(Programs.PublicSector, "Public Sector"),
                    new Program(Programs.SaveTheChildren, "Save The Children")
                };
                await ProgramRepository.InsertManyAsync(programs);
            }

            if (await PeriodRepository.GetCountAsync() <= 0)
            {
                var periods = new List<Tuple<int, int>>();
                foreach (var year in Enumerable.Range(2021,5))
                {
                    foreach (var month in Enumerable.Range(1,12))
                    {
                        periods.Add(new Tuple<int, int>(year, month));
                    }
                }
                await PeriodRepository.InsertManyAsync(periods.Select(p => new Period(p.Item1, p.Item2)));
            }
        }

        public static readonly Country[] List = new[]
        {
            new Country("Afghanistan", "AF", "AFG", "004"){ IsDeleted = true },
            new Country("Albania", "AL", "ALB", "008"){ IsDeleted = true },
            new Country("Algeria", "DZ", "DZA", "012"){ IsDeleted = true },
            new Country("American Samoa", "AS", "ASM", "016"){ IsDeleted = true },
            new Country("Andorra", "AD", "AND", "020"){ IsDeleted = true },
            new Country("Angola", "AO", "AGO", "024"),
            new Country("Anguilla", "AI", "AIA", "660"){ IsDeleted = true },
            new Country("Antarctica", "AQ", "ATA", "010"){ IsDeleted = true },
            new Country("Antigua and Barbuda", "AG", "ATG", "028"){ IsDeleted = true },
            new Country("Argentina", "AR", "ARG", "032"){ IsDeleted = true },
            new Country("Armenia", "AM", "ARM", "051"){ IsDeleted = true },
            new Country("Aruba", "AW", "ABW", "533"){ IsDeleted = true },
            new Country("Australia", "AU", "AUS", "036"){ IsDeleted = true },
            new Country("Austria", "AT", "AUT", "040"){ IsDeleted = true },
            new Country("Azerbaijan", "AZ", "AZE", "031"){ IsDeleted = true },
            new Country("Bahamas", "BS", "BHS", "044"){ IsDeleted = true },
            new Country("Bahrain", "BH", "BHR", "048"){ IsDeleted = true },
            new Country("Bangladesh", "BD", "BGD", "050"){ IsDeleted = true },
            new Country("Barbados", "BB", "BRB", "052"){ IsDeleted = true },
            new Country("Belarus", "BY", "BLR", "112"){ IsDeleted = true },
            new Country("Belgium", "BE", "BEL", "056"){ IsDeleted = true },
            new Country("Belize", "BZ", "BLZ", "084"){ IsDeleted = true },
            new Country("Benin", "BJ", "BEN", "204"),
            new Country("Bermuda", "BM", "BMU", "060"){ IsDeleted = true },
            new Country("Bhutan", "BT", "BTN", "064"){ IsDeleted = true },
            new Country("Bolivia, Plurinational State of", "BO", "BOL", "068"){ IsDeleted = true },
            new Country("Bonaire, Sint Eustatius and Saba", "BQ", "BES", "535"){ IsDeleted = true },
            new Country("Bosnia and Herzegovina", "BA", "BIH", "070"){ IsDeleted = true },
            new Country("Botswana", "BW", "BWA", "072"){ IsDeleted = true },
            new Country("Bouvet Island", "BV", "BVT", "074"){ IsDeleted = true },
            new Country("Brazil", "BR", "BRA", "076"){ IsDeleted = true },
            new Country("British Indian Ocean Territory", "IO", "IOT", "086"){ IsDeleted = true },
            new Country("Brunei Darussalam", "BN", "BRN", "096"){ IsDeleted = true },
            new Country("Bulgaria", "BG", "BGR", "100"){ IsDeleted = true },
            new Country("Burkina Faso", "BF", "BFA", "854"),
            new Country("Burundi", "BI", "BDI", "108"),
            new Country("Cabo Verde", "CV", "CPV", "132"){ IsDeleted = true },
            new Country("Cambodia", "KH", "KHM", "116"),
            new Country("Cameroon", "CM", "CMR", "120"),
            new Country("Canada", "CA", "CAN", "124"){ IsDeleted = true },
            new Country("Cayman Islands", "KY", "CYM", "136"){ IsDeleted = true },
            new Country("Central African Republic", "CF", "CAF", "140"){ IsDeleted = true },
            new Country("Chad", "TD", "TCD", "148"){ IsDeleted = true },
            new Country("Chile", "CL", "CHL", "152"){ IsDeleted = true },
            new Country("China", "CN", "CHN", "156"){ IsDeleted = true },
            new Country("Christmas Island", "CX", "CXR", "162"){ IsDeleted = true },
            new Country("Cocos (Keeling) Islands", "CC", "CCK", "166"){ IsDeleted = true },
            new Country("Colombia", "CO", "COL", "170"){ IsDeleted = true },
            new Country("Comoros", "KM", "COM", "174"){ IsDeleted = true },
            new Country("Congo", "CG", "COG", "178"){ IsDeleted = true },
            new Country("Congo DRC", "CD", "COD", "180"),
            new Country("Cook Islands", "CK", "COK", "184"){ IsDeleted = true },
            new Country("Costa Rica", "CR", "CRI", "188"){ IsDeleted = true },
            new Country("Côte d'Ivoire", "CI", "CIV", "384"),
            new Country("Croatia", "HR", "HRV", "191"){ IsDeleted = true },
            new Country("Cuba", "CU", "CUB", "192"){ IsDeleted = true },
            new Country("Curaçao", "CW", "CUW", "531"){ IsDeleted = true },
            new Country("Cyprus", "CY", "CYP", "196"){ IsDeleted = true },
            new Country("Czechia", "CZ", "CZE", "203"){ IsDeleted = true },
            new Country("Denmark", "DK", "DNK", "208"){ IsDeleted = true },
            new Country("Djibouti", "DJ", "DJI", "262"){ IsDeleted = true },
            new Country("Dominica", "DM", "DMA", "212"){ IsDeleted = true },
            new Country("Dominican Republic", "DO", "DOM", "214"){ IsDeleted = true },
            new Country("Ecuador", "EC", "ECU", "218"){ IsDeleted = true },
            new Country("Egypt", "EG", "EGY", "818"){ IsDeleted = true },
            new Country("El Salvador", "SV", "SLV", "222"){ IsDeleted = true },
            new Country("Equatorial Guinea", "GQ", "GNQ", "226"){ IsDeleted = true },
            new Country("Eritrea", "ER", "ERI", "232"){ IsDeleted = true },
            new Country("Estonia", "EE", "EST", "233"){ IsDeleted = true },
            new Country("Eswatini", "SZ", "SWZ", "748"){ IsDeleted = true },
            new Country("Ethiopia", "ET", "ETH", "231"),
            new Country("Falkland Islands (Malvinas)", "FK", "FLK", "238"){ IsDeleted = true },
            new Country("Faroe Islands", "FO", "FRO", "234"){ IsDeleted = true },
            new Country("Fiji", "FJ", "FJI", "242"){ IsDeleted = true },
            new Country("Finland", "FI", "FIN", "246"){ IsDeleted = true },
            new Country("France", "FR", "FRA", "250"){ IsDeleted = true },
            new Country("French Guiana", "GF", "GUF", "254"){ IsDeleted = true },
            new Country("French Polynesia", "PF", "PYF", "258"){ IsDeleted = true },
            new Country("French Southern Territories", "TF", "ATF", "260"){ IsDeleted = true },
            new Country("Gabon", "GA", "GAB", "266"){ IsDeleted = true },
            new Country("Gambia", "GM", "GMB", "270"){ IsDeleted = true },
            new Country("Georgia", "GE", "GEO", "268"){ IsDeleted = true },
            new Country("Germany", "DE", "DEU", "276"){ IsDeleted = true },
            new Country("Ghana", "GH", "GHA", "288"),
            new Country("Gibraltar", "GI", "GIB", "292"){ IsDeleted = true },
            new Country("Greece", "GR", "GRC", "300"){ IsDeleted = true },
            new Country("Greenland", "GL", "GRL", "304"){ IsDeleted = true },
            new Country("Grenada", "GD", "GRD", "308"){ IsDeleted = true },
            new Country("Guadeloupe", "GP", "GLP", "312"){ IsDeleted = true },
            new Country("Guam", "GU", "GUM", "316"){ IsDeleted = true },
            new Country("Guatemala", "GT", "GTM", "320"){ IsDeleted = true },
            new Country("Guernsey", "GG", "GGY", "831"){ IsDeleted = true },
            new Country("Guinea", "GN", "GIN", "324"),
            new Country("Guinea-Bissau", "GW", "GNB", "624"){ IsDeleted = true },
            new Country("Guyana", "GY", "GUY", "328"){ IsDeleted = true },
            new Country("Haiti", "HT", "HTI", "332"){ IsDeleted = true },
            new Country("Heard Island and McDonald Islands", "HM", "HMD", "334"){ IsDeleted = true },
            new Country("Holy See", "VA", "VAT", "336"){ IsDeleted = true },
            new Country("Honduras", "HN", "HND", "340"){ IsDeleted = true },
            new Country("Hong Kong", "HK", "HKG", "344"){ IsDeleted = true },
            new Country("Hungary", "HU", "HUN", "348"){ IsDeleted = true },
            new Country("Iceland", "IS", "ISL", "352"){ IsDeleted = true },
            new Country("India", "IN", "IND", "356"){ IsDeleted = true },
            new Country("Indonesia", "ID", "IDN", "360"){ IsDeleted = true },
            new Country("Iran, Islamic Republic of", "IR", "IRN", "364"){ IsDeleted = true },
            new Country("Iraq", "IQ", "IRQ", "368"){ IsDeleted = true },
            new Country("Ireland", "IE", "IRL", "372"){ IsDeleted = true },
            new Country("Isle of Man", "IM", "IMN", "833"){ IsDeleted = true },
            new Country("Israel", "IL", "ISR", "376"){ IsDeleted = true },
            new Country("Italy", "IT", "ITA", "380"){ IsDeleted = true },
            new Country("Jamaica", "JM", "JAM", "388"){ IsDeleted = true },
            new Country("Japan", "JP", "JPN", "392"){ IsDeleted = true },
            new Country("Jersey", "JE", "JEY", "832"){ IsDeleted = true },
            new Country("Jordan", "JO", "JOR", "400"){ IsDeleted = true },
            new Country("Kazakhstan", "KZ", "KAZ", "398"){ IsDeleted = true },
            new Country("Kenya", "KE", "KEN", "404"),
            new Country("Kiribati", "KI", "KIR", "296"){ IsDeleted = true },
            new Country("Korea, Democratic People's Republic of", "KP", "PRK", "408"){ IsDeleted = true },
            new Country("Korea, Republic of", "KR", "KOR", "410"){ IsDeleted = true },
            new Country("Kuwait", "KW", "KWT", "414"){ IsDeleted = true },
            new Country("Kyrgyzstan", "KG", "KGZ", "417"){ IsDeleted = true },
            new Country("Laos", "LA", "LAO", "418"),
            new Country("Latvia", "LV", "LVA", "428"){ IsDeleted = true },
            new Country("Lebanon", "LB", "LBN", "422"){ IsDeleted = true },
            new Country("Lesotho", "LS", "LSO", "426"){ IsDeleted = true },
            new Country("Liberia", "LR", "LBR", "430"),
            new Country("Libya", "LY", "LBY", "434"){ IsDeleted = true },
            new Country("Liechtenstein", "LI", "LIE", "438"){ IsDeleted = true },
            new Country("Lithuania", "LT", "LTU", "440"){ IsDeleted = true },
            new Country("Luxembourg", "LU", "LUX", "442"){ IsDeleted = true },
            new Country("Macao", "MO", "MAC", "446"){ IsDeleted = true },
            new Country("Madagascar", "MG", "MDG", "450"),
            new Country("Malawi", "MW", "MWI", "454"),
            new Country("Malaysia", "MY", "MYS", "458"){ IsDeleted = true },
            new Country("Maldives", "MV", "MDV", "462"){ IsDeleted = true },
            new Country("Mali", "ML", "MLI", "466"),
            new Country("Malta", "MT", "MLT", "470"){ IsDeleted = true },
            new Country("Marshall Islands", "MH", "MHL", "584"){ IsDeleted = true },
            new Country("Martinique", "MQ", "MTQ", "474"){ IsDeleted = true },
            new Country("Mauritania", "MR", "MRT", "478"){ IsDeleted = true },
            new Country("Mauritius", "MU", "MUS", "480"){ IsDeleted = true },
            new Country("Mayotte", "YT", "MYT", "175"){ IsDeleted = true },
            new Country("Mexico", "MX", "MEX", "484"){ IsDeleted = true },
            new Country("Micronesia, Federated States of", "FM", "FSM", "583"){ IsDeleted = true },
            new Country("Moldova, Republic of", "MD", "MDA", "498"){ IsDeleted = true },
            new Country("Monaco", "MC", "MCO", "492"){ IsDeleted = true },
            new Country("Mongolia", "MN", "MNG", "496"){ IsDeleted = true },
            new Country("Montenegro", "ME", "MNE", "499"){ IsDeleted = true },
            new Country("Montserrat", "MS", "MSR", "500"){ IsDeleted = true },
            new Country("Morocco", "MA", "MAR", "504"){ IsDeleted = true },
            new Country("Mozambique", "MZ", "MOZ", "508"),
            new Country("Myanmar", "MM", "MMR", "104"),
            new Country("Namibia", "NA", "NAM", "516"){ IsDeleted = true },
            new Country("Nauru", "NR", "NRU", "520"){ IsDeleted = true },
            new Country("Nepal", "NP", "NPL", "524"){ IsDeleted = true },
            new Country("Netherlands", "NL", "NLD", "528"){ IsDeleted = true },
            new Country("New Caledonia", "NC", "NCL", "540"){ IsDeleted = true },
            new Country("New Zealand", "NZ", "NZL", "554"){ IsDeleted = true },
            new Country("Nicaragua", "NI", "NIC", "558"){ IsDeleted = true },
            new Country("Niger", "NE", "NER", "562"),
            new Country("Nigeria", "NG", "NGA", "566"),
            new Country("Niue", "NU", "NIU", "570"){ IsDeleted = true },
            new Country("Norfolk Island", "NF", "NFK", "574"){ IsDeleted = true },
            new Country("Northern Mariana Islands", "MP", "MNP", "580"){ IsDeleted = true },
            new Country("North Macedonia", "MK", "MKD", "807"){ IsDeleted = true },
            new Country("Norway", "NO", "NOR", "578"){ IsDeleted = true },
            new Country("Oman", "OM", "OMN", "512"){ IsDeleted = true },
            new Country("Pakistan", "PK", "PAK", "586"){ IsDeleted = true },
            new Country("Palau", "PW", "PLW", "585"){ IsDeleted = true },
            new Country("Palestine, State of", "PS", "PSE", "275"){ IsDeleted = true },
            new Country("Panama", "PA", "PAN", "591"){ IsDeleted = true },
            new Country("Papua New Guinea", "PG", "PNG", "598"){ IsDeleted = true },
            new Country("Paraguay", "PY", "PRY", "600"){ IsDeleted = true },
            new Country("Peru", "PE", "PER", "604"){ IsDeleted = true },
            new Country("Philippines", "PH", "PHL", "608"){ IsDeleted = true },
            new Country("Pitcairn", "PN", "PCN", "612"){ IsDeleted = true },
            new Country("Poland", "PL", "POL", "616"){ IsDeleted = true },
            new Country("Portugal", "PT", "PRT", "620"){ IsDeleted = true },
            new Country("Puerto Rico", "PR", "PRI", "630"){ IsDeleted = true },
            new Country("Qatar", "QA", "QAT", "634"){ IsDeleted = true },
            new Country("Réunion", "RE", "REU", "638"){ IsDeleted = true },
            new Country("Romania", "RO", "ROU", "642"){ IsDeleted = true },
            new Country("Russian Federation", "RU", "RUS", "643"){ IsDeleted = true },
            new Country("Rwanda", "RW", "RWA", "646"),
            new Country("Saint Barthélemy", "BL", "BLM", "652"){ IsDeleted = true },
            new Country("Saint Helena, Ascension and Tristan da Cunha", "SH", "SHN", "654"){ IsDeleted = true },
            new Country("Saint Kitts and Nevis", "KN", "KNA", "659"){ IsDeleted = true },
            new Country("Saint Lucia", "LC", "LCA", "662"){ IsDeleted = true },
            new Country("Saint Martin (French part)", "MF", "MAF", "663"){ IsDeleted = true },
            new Country("Saint Pierre and Miquelon", "PM", "SPM", "666"){ IsDeleted = true },
            new Country("Saint Vincent and the Grenadines", "VC", "VCT", "670"){ IsDeleted = true },
            new Country("Samoa", "WS", "WSM", "882"){ IsDeleted = true },
            new Country("San Marino", "SM", "SMR", "674"){ IsDeleted = true },
            new Country("Sao Tome and Principe", "ST", "STP", "678"){ IsDeleted = true },
            new Country("Saudi Arabia", "SA", "SAU", "682"){ IsDeleted = true },
            new Country("Senegal", "SN", "SEN", "686"),
            new Country("Serbia", "RS", "SRB", "688"){ IsDeleted = true },
            new Country("Seychelles", "SC", "SYC", "690"){ IsDeleted = true },
            new Country("Sierra Leone", "SL", "SLE", "694"),
            new Country("Singapore", "SG", "SGP", "702"){ IsDeleted = true },
            new Country("Sint Maarten (Dutch part)", "SX", "SXM", "534"){ IsDeleted = true },
            new Country("Slovakia", "SK", "SVK", "703"){ IsDeleted = true },
            new Country("Slovenia", "SI", "SVN", "705"){ IsDeleted = true },
            new Country("Solomon Islands", "SB", "SLB", "090"){ IsDeleted = true },
            new Country("Somalia", "SO", "SOM", "706"){ IsDeleted = true },
            new Country("South Africa", "ZA", "ZAF", "710"){ IsDeleted = true },
            new Country("South Georgia and the South Sandwich Islands", "GS", "SGS", "239"){ IsDeleted = true },
            new Country("South Sudan", "SS", "SSD", "728"),
            new Country("Spain", "ES", "ESP", "724"){ IsDeleted = true },
            new Country("Sri Lanka", "LK", "LKA", "144"){ IsDeleted = true },
            new Country("Sudan", "SD", "SDN", "729"){ IsDeleted = true },
            new Country("Suriname", "SR", "SUR", "740"){ IsDeleted = true },
            new Country("Svalbard and Jan Mayen", "SJ", "SJM", "744"){ IsDeleted = true },
            new Country("Sweden", "SE", "SWE", "752"){ IsDeleted = true },
            new Country("Switzerland", "CH", "CHE", "756"){ IsDeleted = true },
            new Country("Syrian Arab Republic", "SY", "SYR", "760"){ IsDeleted = true },
            new Country("Taiwan, Province of China", "TW", "TWN", "158"){ IsDeleted = true },
            new Country("Tajikistan", "TJ", "TJK", "762"){ IsDeleted = true },
            new Country("Tanzania", "TZ", "TZA", "834"),
            new Country("Thailand", "TH", "THA", "764"),
            new Country("Timor-Leste", "TL", "TLS", "626"){ IsDeleted = true },
            new Country("Togo", "TG", "TGO", "768"){ IsDeleted = true },
            new Country("Tokelau", "TK", "TKL", "772"){ IsDeleted = true },
            new Country("Tonga", "TO", "TON", "776"){ IsDeleted = true },
            new Country("Trinidad and Tobago", "TT", "TTO", "780"){ IsDeleted = true },
            new Country("Tunisia", "TN", "TUN", "788"){ IsDeleted = true },
            new Country("Turkey", "TR", "TUR", "792"){ IsDeleted = true },
            new Country("Turkmenistan", "TM", "TKM", "795"){ IsDeleted = true },
            new Country("Turks and Caicos Islands", "TC", "TCA", "796"){ IsDeleted = true },
            new Country("Tuvalu", "TV", "TUV", "798"){ IsDeleted = true },
            new Country("Uganda", "UG", "UGA", "800"),
            new Country("Ukraine", "UA", "UKR", "804"){ IsDeleted = true },
            new Country("United Arab Emirates", "AE", "ARE", "784"){ IsDeleted = true },
            new Country("United Kingdom of Great Britain and Northern Ireland", "GB", "GBR", "826"){ IsDeleted = true },
            new Country("United States", "US", "USA", "840"){ IsDeleted = true },
            new Country("United States Minor Outlying Islands", "UM", "UMI", "581"){ IsDeleted = true },
            new Country("Uruguay", "UY", "URY", "858"){ IsDeleted = true },
            new Country("Uzbekistan", "UZ", "UZB", "860"){ IsDeleted = true },
            new Country("Vanuatu", "VU", "VUT", "548"){ IsDeleted = true },
            new Country("Venezuela, Bolivarian Republic of", "VE", "VEN", "862"){ IsDeleted = true },
            new Country("Viet Nam", "VN", "VNM", "704"){ IsDeleted = true },
            new Country("Virgin Islands, British", "VG", "VGB", "092"){ IsDeleted = true },
            new Country("Virgin Islands, U.S.", "VI", "VIR", "850"){ IsDeleted = true },
            new Country("Wallis and Futuna", "WF", "WLF", "876"){ IsDeleted = true },
            new Country("Western Sahara", "EH", "ESH", "732"){ IsDeleted = true },
            new Country("Yemen", "YE", "YEM", "887"){ IsDeleted = true },
            new Country("Zambia", "ZM", "ZMB", "894"),
            new Country("Zimbabwe", "ZW", "ZWE", "716"),
            new Country("Åland Islands", "AX", "ALA", "248"){ IsDeleted = true }
        };
    }

}
