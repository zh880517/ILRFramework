
public class SDFData
{
    public int Width { get; private set; }
    public int Heigh { get; private set; }
    public FP Grain { get; private set; }
    public TSVector2 Origin { get; private set; }
    public sbyte[] Data { get; private set; }

    public sbyte this[int x, int y]
    {
        get
        {
            return Data[x + y * Width];
        }
    }

    public void Init(int width, int heigh, FP grain, TSVector2 origin, sbyte[] data)
    {
        Width = width;
        Heigh = heigh;
        Grain = grain;
        Origin = origin;
        Data = new sbyte[Width * heigh];
        data.CopyTo(Data, 0);
    }

    public sbyte Get(int x, int y)
    {
        return Data[x + y * Width];
    }

    public void Write(System.IO.BinaryWriter writer)
    {
        writer.Write(Width);
        writer.Write(Heigh);
        writer.Write(Grain._serializedValue);
        writer.Write(Origin.x.RawValue);
        writer.Write(Origin.y.RawValue);
        writer.Write(Data.Length);
        for (int i=0; i<Data.Length; ++i)
        {
            writer.Write(Data[i]);
        }
    }

    public void Read(System.IO.BinaryReader reader)
    {
        Width = reader.ReadInt32();
        Heigh = reader.ReadInt32();
        Grain = reader.ReadInt64();
        Origin = new TSVector2(reader.ReadInt64(), reader.ReadInt64());
        int len = reader.ReadInt32();
        Data = new sbyte[len];
        for (int i=0; i<len; ++i)
        {
            Data[i] = reader.ReadSByte();
        }
    }

}
