using System;
using System.Linq;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Paillave.Etl.Core;
using System.Threading;
using Microsoft.Extensions.FileSystemGlobbing;
using ICSharpCode.SharpZipLib.GZip;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Tar;

namespace EtlNet.GZip
{
    public class UngzipFileProcessorValuesProvider : ValuesProviderBase<IFileValue, IFileValue>
    {
        private UngzipFileProcessorParams _args;
        public UngzipFileProcessorValuesProvider(UngzipFileProcessorParams args)
            => _args = args;
        public override ProcessImpact PerformanceImpact => ProcessImpact.Average;
        public override ProcessImpact MemoryFootPrint => ProcessImpact.Average;
        public override void PushValues(IFileValue fileValue, Action<IFileValue> push, CancellationToken cancellationToken, IDependencyResolver resolver, IInvoker invoker)
        {
            var destinations = (fileValue.Metadata as IFileValueWithDestinationMetadata)?.Destinations;
            if (cancellationToken.IsCancellationRequested) return;
            //using (var zf = new ZipFile(fileValue.GetContent()))
            //{
            //    var searchPattern = string.IsNullOrEmpty(_args.FileNamePattern) ? "*" : _args.FileNamePattern;
            //    var matcher = new Matcher().AddInclude(searchPattern);

            //    if (!String.IsNullOrEmpty(_args.Password))
            //        zf.Password = _args.Password;
            //    var fileNames = zf.OfType<ZipEntry>().Where(i => i.IsFile && matcher.Match(Path.GetFileName(i.Name)).HasMatches).Select(i => i.Name).ToHashSet();
            //    foreach (ZipEntry zipEntry in zf)
            //    {
            //        if (cancellationToken.IsCancellationRequested) break;
            //        if (zipEntry.IsFile && matcher.Match(Path.GetFileName(zipEntry.Name)).HasMatches)
            //        {
            //            MemoryStream outputStream = new MemoryStream();
            //            using (var zipStream = zf.GetInputStream(zipEntry))
            //                zipStream.CopyTo(outputStream, 4096);
            //            outputStream.Seek(0, SeekOrigin.Begin);
            //            push(new UngzippedFileValue<UngzippedFileValueMetadata>(outputStream, zipEntry.Name, new UngzippedFileValueMetadata
            //            {
            //                ParentFileName = fileValue.Name,
            //                ParentFileMetadata = fileValue.Metadata,
            //                Destinations = destinations
            //            }, fileValue, fileNames, zipEntry.Name));
            //        }
            //    }
            //}


            using (Stream gzipStream = new GZipInputStream(fileValue.GetContent()))
            {
                using (var tarInputStream = new TarInputStream(gzipStream, null))
                {
                    var searchPattern = string.IsNullOrEmpty(_args.FileNamePattern) ? "*" : _args.FileNamePattern;
                    var matcher = new Matcher().AddInclude(searchPattern);
                    var tarEntries = new List<TarEntry>();
                    var fileNames = new HashSet<string>();
                    var files = new Dictionary<string, Stream>();
                    TarEntry tarEntry;
                    while ((tarEntry = tarInputStream.GetNextEntry()) != null)
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
    }
}