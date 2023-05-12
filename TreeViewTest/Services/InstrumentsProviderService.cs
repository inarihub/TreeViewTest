using System;
using System.IO;
using System.Threading.Tasks;

namespace TreeViewTest.Services;

public class InstrumentsProviderService
{
    readonly Uri _sourceUri;

    public InstrumentsProviderService()
    {
        _sourceUri = new("/filters.txt", UriKind.Relative);
    }

    public async Task<Stream> GetInstrumentsAsync(int simDelay)
    {
        await Task.Delay(simDelay); // simulation
        return App.GetContentStream(_sourceUri).Stream;
    }
}
