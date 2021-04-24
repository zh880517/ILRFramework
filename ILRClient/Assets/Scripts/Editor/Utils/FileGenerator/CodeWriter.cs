using System.Text;

public class CodeWriter
{
    private int tabCount = 0;

    public StringBuilder Stream { get; } = new StringBuilder();

    public struct Scop : System.IDisposable
    {
        private readonly CodeWriter writer;
        public Scop(CodeWriter writer)
        {
            writer.BeginScope();
            this.writer = writer;
        }

        public void Dispose()
        {
            writer.EndScope();
        }
    }

    public CodeWriter BeginScope()
    {
        NewLine();
        Stream.Append('{');
        tabCount += 1;
        NewLine();
        return this;
    }

    public CodeWriter EndScope()
    {
        tabCount -= 1;
        NewLine();
        Stream.Append('}');
        NewLine();
        return this;
    }

    public CodeWriter EmptyScop(bool emptyLine = true)
    {
        NewLine();
        Stream.Append('{');
        EmptyLine();
        Stream.Append('}');
        NewLine();
        if (emptyLine)
            Stream.Append('\n');
        return this;
    }

    public CodeWriter EmptyLine()
    {
        Stream.Append('\n');
        return this;
    }

    public CodeWriter NewLine()
    {
        Stream.Append('\n').Append(' ', tabCount*4);
        return this;
    }

    public CodeWriter Write(string val)
    {
        Stream.Append(val);
        return this;
    }

    public override string ToString()
    {
        return Stream.ToString();
    }
}
