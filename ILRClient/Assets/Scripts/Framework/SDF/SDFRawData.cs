using System.IO;
public class SDFRawData
{
    public int Width { get; private set; }
    public int Heigh { get; private set; }
    public FP Grain { get; private set; }
    public FP Scale { get; private set; }
    public TSVector2 Origin { get; private set; }
    private sbyte[] data;

    public sbyte this[int x, int y]
    {
        get
        {
            return data[x + y * Width];
        }
    }

    public FP this[int idx]
    {
        get
        {
            return data[idx]*Scale;
        }
    }

    public FP Get(GridPoint pt)
    {
        return this[pt.x + pt.y * Width];
    }

    public void Init(int width, int heigh, FP grain, TSVector2 origin, sbyte[] data)
    {
        Width = width;
        Heigh = heigh;
        Grain = grain;
        Origin = origin;
        this.data = new sbyte[Width * heigh];
        data.CopyTo(this.data, 0);
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Width);
        writer.Write(Heigh);
        writer.Write(Grain._serializedValue);
        writer.Write(Scale._serializedValue);
        writer.Write(Origin.x.RawValue);
        writer.Write(Origin.y.RawValue);
        writer.Write(data.Length);
        for (int i=0; i<data.Length; ++i)
        {
            writer.Write(data[i]);
        }
    }

    public void Read(BinaryReader reader)
    {
        Width = reader.ReadInt32();
        Heigh = reader.ReadInt32();
        Grain = reader.ReadInt64();
        Scale = reader.ReadInt64();
        Origin = new TSVector2(reader.ReadInt64(), reader.ReadInt64());
        int len = reader.ReadInt32();
        data = new sbyte[len];
        for (int i=0; i<len; ++i)
        {
            data[i] = reader.ReadSByte();
        }
    }

}
