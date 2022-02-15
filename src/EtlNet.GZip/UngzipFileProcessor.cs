using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;
using Microsoft.Extensions.FileSystemGlobbing;
using Paillave.Etl.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace EtlNet.GZip
{
    public class UngzipFileProcessorParams
    {
        public string Password { get; set; }
        public string FileNamePattern { get; set; }
    }
    public class UngzippedFileValueMetadata : FileValueMetadataBase, IFileValueWithDestinationMetadata
    {
        public string ParentFileName { get; set; }
        public IFileValueMetadata ParentFileMetadata { get; set; }
        public Dictionary<string, IEnumerable<Destination>> Destinations { get; set; }
    }

    public class UngzipFileProcessor : FileValueProcessorBase<object, UngzipFileProcessorParams>
    {
        public UngzipFileProcessor(string code, string name, string connectionName, object connectionParameters, UngzipFileProcessorParams processorParameters) : base(code, name, connectionName, connectionParameters, processorParameters)
        {
        }
        public override ProcessImpact PerformanceImpact => ProcessImpact.Heavy;

        public override ProcessImpact MemoryFootPrint => ProcessImpact.Average;

        protected override void Process(IFileValue fileValue, object connectionParameters, UngzipFileProcessorParams processorParameters, Action<IFileValue> push, CancellationToken cancellationToken, IDependencyResolver resolver, IInvoker invoker)
        {
            var destinations = (fileValue.Metadata as IFileValueWithDestinationMetadata)?.Destinations;
            if (cancellationToken.IsCancellationRequested) return;
            
            using (Stream gzipStream = new GZipInputStream(fileValue.GetContent()))
            {
                using (var tarInputStream = new TarInputStream(gzipStream, null))
                {
                    var searchPattern = string.IsNullOrEmpty(processorParameters.FileNamePattern) ? "*" : processorParameters.FileNamePattern;
                    var matcher = new Matcher().AddInclude(searchPattern);
                    var tarEntries = new List<TarEntry>();
                    var fileNames = new HashSet<string>();
                    var files = new Dictionary<string, Stream>();
                    TarEntry tarEntry;
                    while((tarEntry = tarInputStream.GetNextEntry()) != null)
                    {
                        if (tarEntry.IsDirectory) continue;
                        if (matcher.Match(Path.GetFileName(tarEntry.Name)).HasMatches)
                        {
                            fileNames.Add(tarEntry.Name);
                            MemoryStream outputStream = new MemoryStream();
                            tarInputStream.CopyEntryContents(outputStream);
                            outputStream.Seek(0, SeekOrigin.Begin);
                            files.Add(tarEntry.Name, outputStream);
                        }
                    }
                    foreach (var f in files)
                    {
                        push(new UngzippedFileValue<UngzippedFileValueMetadata>(f.Value, f.Key, new UngzippedFileValueMetadata
                        {
                            ParentFileName = fileValue.Name,
                            ParentFileMetadata = fileValue.Metadata,
                            Destinations = destinations
                        }, fileValue, fileNames, f.Key));
                    }

                }
            }
            
        }

        protected override void Test(object connectionParameters, UngzipFileProcessorParams processorParameters) { }
    }
}