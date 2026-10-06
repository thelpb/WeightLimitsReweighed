using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Spt.Tables;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WeightLimitsReweighed;

public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "thelpb.WeightLimitsReweighed";
    public string Name { get; init; } = "Weight Limits Reweighed";
    public string Author { get; init; } = "thelpb";
    public List<string>? Contributors { get; init; }
    public SemanticVersioning.Version Version { get; init; } = new("1.0.0");
    public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.0");
    public bool HasPrepatcher { get; init; }
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public string? Url { get; init; }
    public string License { get; init; } = "MIT";
}

public class WeightLimitsSettings
{
    public Stamina Stamina { get; set; } = new();
}

[Injectable(InjectionType.Singleton, int.MaxValue, TypePriority = 250000)]
public class EditDatabaseValues(GlobalTable globals, ModHelper modHelper)
    : IOnLoad
{
    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        WeightLimitsSettings config = JsonSerializer.Deserialize<WeightLimitsSettings>(File.ReadAllText(Path.Combine(modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly()), "config.json")))!;

        globals.Configuration.Stamina.BaseOverweightLimits = config.Stamina.BaseOverweightLimits;
        globals.Configuration.Stamina.SprintOverweightLimits = config.Stamina.SprintOverweightLimits;
        globals.Configuration.Stamina.WalkOverweightLimits = config.Stamina.WalkOverweightLimits;
        globals.Configuration.Stamina.WalkSpeedOverweightLimits = config.Stamina.WalkSpeedOverweightLimits;

        return Task.CompletedTask;
    }
}
