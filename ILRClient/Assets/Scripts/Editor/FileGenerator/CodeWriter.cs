using System.Text;

public class CodeWriter
{
    private int tabCount = 0;

    public StringBuilder Stream { get; } = new StringBuilder();

    public struct Scop : System.IDisposable
    {
        private readonly CodeWriter writer;
        private bool emptyLine;
        public Scop(CodeWriter writer, bool emptyLine = true)
        {
            writer.BeginScope();
            this.writer = writer;
            this.emptyLine = emptyLine;
        }

        public void Dispose()
        {
            writer.EndScope();
            if (emptyLine)
            {
                writer.EmptyLine();
            }
        }
    }

    public CodeWriter BeginScope()
    {
        NewLine();
        Stream.Append('{');
        tabCount += 4;
        NewLine();
        return this;
    }

    public CodeWriter EndScope()
    {
        tabCount -= 4;
        NewLine();
        Stream.Append('}');
        NewLine();
        return this;
    }

    public CodeWriter EmptyScop(bool emptyLine = true)
    {
        NewLine();
        Stream.Append('{');
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
