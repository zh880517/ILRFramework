public static class ContextGenertor
{
    
    public static string Gen(string name)
    {
        CodeWriter writer = new CodeWriter(true);
        writer.Write($"public interface I{name}Component : ECS.Core.IComponent");
        writer.EmptyScop();

        writer.Write($"public class {name}Entity : ECS.Core.TEntity<I{name}Component>");
        using(new CodeWriter.Scop(writer))
        {
            writer.Write($"public {name}Entity(ECS.Core.Context context, int id) : base(context, id)");
            writer.EmptyScop(false);
        }

        writer.Write($"public static partial class {name}Components");
        using (new CodeWriter.Scop(writer))
        {
            writer.Write($"public static System.Action<{name}Context> OnContextCreat;").NewLine();
            writer.Write($"public static int ComponentCount {{ get; private set; }}").NewLine();
        }

        writer.Write($"public class {name}Context : ECS.Core.TContext<{name}Entity>");
        using (new CodeWriter.Scop(writer))
        {
            //Ctor
            writer.Write($"protected {name}Context(int componentTypeCount) : base(componentTypeCount, CreatFunc)");
            writer.EmptyScop();

            //CreatFunc
            writer.Write($"private static {name}Entity CreatFunc(ECS.Core.Context context, int id)");
            using (new CodeWriter.Scop(writer))
            {
                writer.Write($"return new {name}Entity(context, id);");
            }

            //Creat
            writer.Write($"public static {name}Context Creat()");
            using (new CodeWriter.Scop(writer))
            {
                writer.Write($"var contxt = new {name}Context({name}Components.ComponentCount);").NewLine();
                writer.Write($"{name}Components.OnContextCreat(contxt);").NewLine();
                writer.Write("return contxt;");
            }
        }


        return writer.ToString();
    }
}
