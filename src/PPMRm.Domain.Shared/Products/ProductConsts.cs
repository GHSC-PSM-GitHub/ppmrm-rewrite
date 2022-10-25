using System;
using System.Collections.Generic;
using System.Text;

namespace PPMRm.Products
{
    public static class ProductConsts
    {
        public readonly static List<(string Id, string Name, TracerCategory Category)> PPMRmProducts = new List<(string Id, string Name, TracerCategory Category)>
        {
            ProductValues.ArtemetherLumefantrine20mg120mg6x1Blisters,
            ProductValues.ArtemetherLumefantrine20mg120mg6x2Blisters,
            ProductValues.ArtemetherLumefantrine20mg120mg6x3Blisters,
            ProductValues.ArtemetherLumefantrine20mg120mg6x4Blisters,
            ProductValues.ArtemetherLumefantrine80mg480mg6x1Blisters,

            ProductValues.ArtesunateAmodiaquine25mg67_5mgFDC3tabs,
            ProductValues.ArtesunateAmodiaquine50mg135mgFDC3tabs,
            ProductValues.ArtesunateAmodiaquine100mg270mgFDC3tabs,
            ProductValues.ArtesunateAmodiaquine100mg270mgFDC6tabs,

            ProductValues.ArtesunateSuppository50mg,
            ProductValues.ArtesunateSuppository100mg,
            ProductValues.ArtesunateSuppository200mg,

            ProductValues.ArtesunateInjectable30mg,
            ProductValues.ArtesunateInjectable60mg,
            ProductValues.ArtesunateInjectable120mg,

            ProductValues.ArtesunateMefloquine25mg50mgFDC3tabs,
            ProductValues.ArtesunateMefloquine25mg50mgFDC6tabs,
            ProductValues.ArtesunateMefloquine100mg200mgFDC3tabs,
            ProductValues.ArtesunateMefloquine100mg200mgFDC6tabs,

            ProductValues.DHAPPQ40mg320mg3tabs,
            ProductValues.DHAPPQ40mg320mg6tabs,
            ProductValues.DHAPPQ40mg320mg9tabs,
            ProductValues.DHAPPQ20mg160mg3tabs,

            ProductValues.PyronaridineArtesunate180_60mg_10x9Blisters,
            ProductValues.PyronaridineArtesunate60_20mg_30x3Sachets,

            ProductValues.Primaquine7_5mg,
            ProductValues.Primaquine15mg,

            ProductValues.SulphadoxinePyrimethamine,

            ProductValues.RapidDiagnosticTests,

            ProductValues.QuantitativeG6PDTest


        };
        public static class ProductValues
        {
            public static readonly (string Id, string Name, TracerCategory Category) ArtemetherLumefantrine80mg480mg6x1Blisters = ("AL6x1-80", "Artemether/Lumefantrine 80mg/480 mg 6x1 Blisters", TracerCategory.ACTs);
            public static readonly (string Id, string Name, TracerCategory Category) ArtemetherLumefantrine20mg120mg6x1Blisters = ("AL6x1-20", "Artemether/Lumefantrine 20mg/120mg 6x1 Blisters", TracerCategory.ACTs);
            public static readonly (string Id, string Name, TracerCategory Category) ArtemetherLumefantrine20mg120mg6x2Blisters = ("AL6x2-20", "Artemether/Lumefantrine 20mg/120mg 6x2 Blisters", TracerCategory.ACTs);
            public static readonly (string Id, string Name, TracerCategory Category) ArtemetherLumefantrine20mg120mg6x3Blisters = ("AL6x3-20", "Artemether/Lumefantrine 20mg/120mg 6x3 Blisters", TracerCategory.ACTs);
            public static readonly (string Id, string Name, TracerCategory Category) ArtemetherLumefantrine20mg120mg6x4Blisters = ("AL6x4-20", "Artemether/Lumefantrine 20mg/120mg 6x4 Blisters", TracerCategory.ACTs);

            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateAmodiaquine25mg67_5mgFDC3tabs = ("ASAQ3-25", "Artesunate/Amodiaquine 25mg/67.5mg FDC 3 tabs", TracerCategory.ArtesunateAmodiaquine);
            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateAmodiaquine50mg135mgFDC3tabs = ("ASAQ3-50", "Artesunate/Amodiaquine 50mg/135mg FDC 3 tabs", TracerCategory.ArtesunateAmodiaquine);
            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateAmodiaquine100mg270mgFDC3tabs = ("ASAQ3-100", "Artesunate/Amodiaquine 100mg/270mg FDC 3 tabs", TracerCategory.ArtesunateAmodiaquine);
            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateAmodiaquine100mg270mgFDC6tabs = ("ASAQ6-100", "Artesunate/Amodiaquine 100mg/270mg FDC 6 tabs", TracerCategory.ArtesunateAmodiaquine);

            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateSuppository50mg = ("AS-SUP-50", "Artesunate Suppository 50mg", TracerCategory.SevereMalaria);
            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateSuppository100mg = ("AS-SUP-100", "Artesunate Suppository 100mg", TracerCategory.SevereMalaria);
            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateSuppository200mg = ("AS-SUP-200", "Artesunate Suppository 200mg", TracerCategory.SevereMalaria);

            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateInjectable30mg = ("AS-INJ-30", "Artesunate Injectable 30mg", TracerCategory.SevereMalaria);
            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateInjectable60mg = ("AS-INJ-60", "Artesunate Injectable 60mg", TracerCategory.SevereMalaria);
            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateInjectable120mg = ("AS-INJ-120", "Artesunate Injectable 120mg", TracerCategory.SevereMalaria);


            public static readonly(string Id, string Name, TracerCategory Category) ArtesunateMefloquine25mg50mgFDC3tabs = ("ASMQ3-25", "Artesunate/Mefloquine 25mg/50mg FDC 3 tabs", TracerCategory.ACTs);
            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateMefloquine25mg50mgFDC6tabs = ("ASMQ6-25", "Artesunate/Mefloquine 25mg/50mg FDC 6 tabs", TracerCategory.ACTs);
            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateMefloquine100mg200mgFDC3tabs = ("ASMQ3-100", "Artesunate/Mefloquine 100mg/200mg FDC 3 tabs", TracerCategory.ACTs);
            public static readonly (string Id, string Name, TracerCategory Category) ArtesunateMefloquine100mg200mgFDC6tabs = ("ASMQ6-100", "Artesunate/Mefloquine 100mg/200mg FDC 6 tabs", TracerCategory.ACTs);

            public static readonly (string Id, string Name, TracerCategory Category) DHAPPQ40mg320mg3tabs = ("DHAPPQ3-40", "DHA-PPQ 40mg/320mg-3 tabs", TracerCategory.ACTs);
            public static readonly (string Id, string Name, TracerCategory Category) DHAPPQ40mg320mg6tabs = ("DHAPPQ6-40", "DHA-PPQ 40mg/320mg-6 tabs", TracerCategory.ACTs);
            public static readonly (string Id, string Name, TracerCategory Category) DHAPPQ40mg320mg9tabs = ("DHAPPQ9-40", "DHA-PPQ 40mg/320mg-9 tabs", TracerCategory.ACTs);
            public static readonly (string Id, string Name, TracerCategory Category) DHAPPQ20mg160mg3tabs = ("DHAPPQ3-20", "DHA-PPQ 20mg/160mg-3 tabs", TracerCategory.ACTs);


            public static readonly (string Id, string Name, TracerCategory Category) PyronaridineArtesunate180_60mg_10x9Blisters = ("PYAS-10X9-180", "Pyronaridine/Artesunate 180/60 mg Film-Coated Tablet, 10x9 Blister Pack Tablets", TracerCategory.ACTs);
            public static readonly (string Id, string Name, TracerCategory Category) PyronaridineArtesunate60_20mg_30x3Sachets = ("PYAS-30X3-60", "Pyronaridine/Artesunate 60/20 mg Granules for Suspension, 30X3 Sachets", TracerCategory.ACTs);

            public static readonly (string Id, string Name, TracerCategory Category) Primaquine7_5mg = ("PQ-7_5", "Primaquine 7.5mg", TracerCategory.OtherPharma);
            public static readonly (string Id, string Name, TracerCategory Category) Primaquine15mg = ("PQ-15", "Primaquine 15mg", TracerCategory.OtherPharma);

            public static readonly (string Id, string Name, TracerCategory Category) SulphadoxinePyrimethamine = ("SP", "Sulphadoxine-Pyrimethamine", TracerCategory.SulphadoxinePyrimethamine);

            public static readonly (string Id, string Name, TracerCategory Category) RapidDiagnosticTests = ("RDT", "Rapid Diagnostic Tests", TracerCategory.RapidDiagnosticTest);

            public static readonly (string Id, string Name, TracerCategory Category) QuantitativeG6PDTest = ("QGT", "Quantitative G6PD Test", TracerCategory.DiagnosticTest);


            /*
             * Pyronaridine/Artesunate 180/60 mg Film-Coated Tablet, 10 x 9 Blister Pack Tablets - 106286ABC0NYP - PYAS-10X9-180 
             * Pyronaridine/Artesunate 60/20 mg Granules for Suspension, 30 X 3 Sachets - 106284DEW0NXP - PYAS-30X3-60
             * Artesunate/Amodiaquine 25mg/67.5mg FDC 3 tabs
             * Artesunate/Amodiaquine 50mg/135mg FDC 3 tabs
             * Artesunate/Amodiaquine 100mg/270mg FDC 3 tabs
             * Artesunate/Amodiaquine 100mg/270mg FDC 6 tabs
             * 
             * 	Artesunate suppository 50mg
             * Artesunate suppository 100mg
             * Artesunate suppository 200mg
             * 
             * Artesunate injectable 30mg
             * Artesunate Injectable 60mg
             * Artesunate Injectable 120mg
             * 
             * Artesunate/Mefloquine 25mg/50mg FDC 3 tabs
             * Artesunate/Mefloquine 25mg/50mg FDC 6 tabs	
             * Artesunate/Mefloquine 100mg/200mg FDC 3 tabs
             * Artesunate/Mefloquine 100mg/200mg FDC 6 tabs
             * 
             * DHA-PPQ 40mg/320mg-3 tabs
             * DHA-PPQ 40mg/320mg-6 tabs
             * DHA-PPQ 40mg/320mg-9 tabs
             * DHA-PPQ 20mg/160mg-3 tabs
             * 
             * 	Sulphadoxine-Pyrimethamine
             * 	
             * 	Rapid Diagnostic Tests
             * 
             * Primaquine 15mg
             * 	Primaquine 7.5mg tablet
             * 	
             * 
             * 	Artesunate/Amodiaquine 25mg/67.5mg FDC 3 tabs
             *  Artesunate/Amodiaquine 50mg/135mg FDC 3 tabs
             *  Artesunate/Amodiaquine 100mg/270mg FDC 3 tabs
             *  Artesunate/Amodiaquine 100mg/270mg FDC 6 tabs
             */
        }
    }
}
