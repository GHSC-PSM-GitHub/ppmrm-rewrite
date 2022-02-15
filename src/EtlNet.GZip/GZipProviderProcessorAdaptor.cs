using System;
using System.Threading;
using Paillave.Etl.Core;

namespace EtlNet.GZip
{
    public class GZipAdapterConnectionParameters
    {
        public string Password { get; set; }
    }
    public class GZipAdapterProcessorParameters
    {
        public string FileNamePattern { get; set; }
    }
    public class GZipProviderProcessorAdapter : ProviderProcessorAdapterBase<GZipAdapterConnectionParameters, object, GZipAdapterProcessorParameters>
    {
        public override string Description => "Handle gzip files";
        public override string Name => "GZip";
        protected override IFileValueProvider CreateProvider(string code, string name, string connectionName, GZipAdapterConnectionParameters connectionParameters, object inputParameters)
            => null;
        protected override IFileValueProcessor CreateProcessor(string code, string name, string connectionName, GZipAdapterConnectionParameters connectionParameters, GZipAdapterProcessorParameters outputParameters)
            => new GZipFileValueProcessor(code, name, connectionName, connectionParameters, outputParameters);
    }
    public class GZipFileValueProcessor : FileValueProcessorBase<GZipAdapterConnectionParameters, GZipAdapterProcessorParameters>
    {
        public GZipFileValueProcessor(string code, string name, string connectionName, GZipAdapterConnectionParameters connectionParameters, GZipAdapterProcessorParameters processorParameters)
            : base(code, name, connectionName, connectionParameters, processorParameters) { }
        public override ProcessImpact PerformanceImpact => ProcessImpact.Heavy;
        public override ProcessImpact MemoryFootPrint => ProcessImpact.Average;
        protected override void Process(IFileValue fileValue, GZipAdapterConnectionParameters connectionParameters, GZipAdapterProcessorParameters processorParameters, Action<IFileValue> push, CancellationToken cancellationToken, IDependencyResolver resolver, IInvoker invoker)
        {
            new UngzipFileProcessorValuesProvider(new UngzipFileProcessorParams
            {
                FileNamePattern = processorParameters.FileNamePattern,
                Password = connectionParameters.Password
            }).PushValues(fileValue, push, cancellationToken, resolver, invoker);
        }

        protected override void Test(GZipAdapterConnectionParameters connectionParameters, GZipAdapterProcessorParameters processorParameters) { }
    }
}