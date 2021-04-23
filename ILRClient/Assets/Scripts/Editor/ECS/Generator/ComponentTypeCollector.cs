using System;
using System.Collections.Generic;
using System.Reflection;

public class ComponentTypeCollector
{
    private readonly List<Type> componentTypes = new List<Type>();
    private readonly Type baseType;
    private readonly string contextName;

    public ComponentTypeCollector(Type baseType, string contextName)
    {
        this.baseType = baseType;
        this.contextName = contextName;
    }

    public bool AddType(Type type)
    {
        if (baseType.IsAssignableFrom(type))
        {
            componentTypes.Add(type);
        }
        return false;
    }
    public void Gen(GeneratorFolder folder)
    {
        folder.AddFile($"{contextName}Components", GenComponentsFile());
        //自动System生成
        foreach (var type in componentTypes)
        {
            var cleanup = type.GetCustomAttribute<ECS.Core.CleanupAttribute>();
            if (cleanup == null)
                continue;
            CodeWriter writer = new CodeWriter();
            string className = $"{type.Name}CleanupSystem";
            writer.Write($"public class {className} : ECS.Core.ICleanupSystem");
            using(new CodeWriter.Scop(writer))
            {
                if (cleanup.Mode == ECS.Core.CleanupMode.DestroyEntity)
                {
                    GenDestroySystem(type, writer, className);
                }
                else
                {
                    GenRemoveSystem(type, writer, className);
                }
            }
            folder.AddFile(className, writer.ToString());
        }
    }

    private void GenDestroySystem(Type type, CodeWriter writer, string className)
    {
        writer.Write($"$private readonly ECS.Core.Group<{type.FullName}> group;").NewLine();
        writer.Write($"public {className}({contextName}Context context)");
        using(new CodeWriter.Scop(writer))
        {
            writer.Write($"group = context.CreatGroup<{type.FullName}>();");
        }
        writer.Write($"public void OnCleanup()");
        using (new CodeWriter.Scop(writer)) 
        {
            writer.Write($"if (group.Count > 0)");
            using (new CodeWriter.Scop(writer))
            {
                writer.Write($"while (group.TryGet(out ECS.Core.Entity entity, out _))");
                using (new CodeWriter.Scop(writer)) 
                {
                    writer.Write("entity.Destroy();");
                }
                writer.Write("group.Reset();");
            }
        }
    }

    private void GenRemoveSystem(Type type, CodeWriter writer, string className)
    {
        writer.Write($"$private readonly {className}Context context;").NewLine();
        writer.Write($"public {className}({contextName}Context context)");
        using (new CodeWriter.Scop(writer))
        {
            writer.Write("this.context = context;");
        }
        writer.Write($"public void OnCleanup()");
        using (new CodeWriter.Scop(writer))
        {
            writer.Write($"context.RemoveAll<{type.FullName}>();");
        }
    }

    private string GenComponentsFile()
    {
        CodeWriter writer = new CodeWriter();
        writer.Write($"public static partial class {contextName}Components");
        using(new CodeWriter.Scop(writer))
        {
            writer.Write($"static {contextName}Components()");
            using (new CodeWriter.Scop(writer))
            {
                writer.Write($"OnContextCreat = DoContentInit;").NewLine();
                writer.Write($"ComponentCount = {componentTypes.Count};").NewLine();
                writer.Write("InitComponentsIdentity();");
            }
            writer.Write($"static void InitComponentsIdentity()");
            using(new CodeWriter.Scop(writer))
            {
                for (int i=0; i<componentTypes.Count; ++i)
                {
                    var type = componentTypes[i];
                    if (type.GetCustomAttribute<ECS.Core.UniqueAttribute>() != null)
                    {
                        writer.Write($"ECS.Core.ComponentIdentity<{type.FullName}>.Unique = true;").NewLine();
                    }
                    writer.Write($"ECS.Core.ComponentIdentity<{type.FullName}>.Id = {i};");
                    if (i < componentTypes.Count - 1)
                    {
                        writer.NewLine();
                    }
                }
            }
            writer.Write($"static void DoContentInit({contextName}Context context)");
            using (new CodeWriter.Scop(writer))
            {
                for (int i = 0; i < componentTypes.Count; ++i)
                {
                    var type = componentTypes[i];
                    writer.Write($"context.InitComponentCollector<{type.FullName}>();");
                    if (i < componentTypes.Count - 1)
                    {
                        writer.NewLine();
                    }
                }
            }
        }
        return writer.ToString();
    }



}
