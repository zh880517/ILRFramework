using System.Collections.Generic;
using UnityEditor;
[FilePath("Assets/Editor/ECSConfig.asset", FilePathAttribute.Location.ProjectFolder)]
public class ECSGeneratorConfig : ScriptableSingleton<ECSGeneratorConfig>
{
    public List<ECSContextConfig> Contexts = new List<ECSContextConfig>();

}
