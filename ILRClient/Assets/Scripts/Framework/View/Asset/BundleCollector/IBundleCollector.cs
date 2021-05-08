using System.Collections.Generic;

public interface IBundleCollector
{
    void Create(string name, Dictionary<string, IBundle> bundles);
}
