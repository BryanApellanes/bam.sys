using Bam.CommandLine;
using Bam.Net;
using Bam.Net.Configuration;
using Bam.Net.CoreServices;
using Bam.Net.Logging;

namespace Bam.Sys.Console
{
    public interface IBamContext
    {
        IApplicationNameProvider ApplicationNameProvider { get; }
        IConfigurationProvider ConfigurationProvider { get; }
        ILogger Logger { get; }
        IMenuManager MenuManager { get; }
        ServiceRegistry ServiceRegistry { get; }

        event ConsoleArgsParsedDelegate ArgsParsed;
        event ConsoleArgsParsedDelegate ArgsParsedError;

        void AddValidArgument(string name, string? description = null);
        void AddValidArgument(string name, bool allowNull, bool addAcronym = false, string? description = null, string? valueExample = null);
    }
}