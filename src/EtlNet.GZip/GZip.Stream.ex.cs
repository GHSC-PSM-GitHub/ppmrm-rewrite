using System;

using Paillave.Etl.Core;
namespace EtlNet.GZip
{
    public static class ZipEx
    {
        public static IStream<IFileValue> CrossApplyGZipFiles(this IStream<IFileValue> stream, string name, string pattern = "*", string password = null, bool noParallelisation = false)
                => stream.CrossApply<IFileValue, IFileValue>(name, new UngzipFileProcessorValuesProvider(new UngzipFileProcessorParams
                {
                    FileNamePattern = pattern,
                    Password = password
                }), noParallelisation);
    }
}
