namespace App.Application.Contracts.Infrastructure.AI;

/// <summary>
/// FACTORY FOR RESOLVING AI ANALYSIS PROVIDERS BY NAME.
/// </summary>
public interface IAIProviderFactory
{
    IAIAnalysisProvider GetProvider(string providerName);
    IReadOnlyList<string> AvailableProviders { get; }
}
