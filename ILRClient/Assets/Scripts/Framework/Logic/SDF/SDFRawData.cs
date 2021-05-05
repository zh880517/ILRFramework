using System.IO;
using UnityEngine;

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
            if (x < 0 || x >= Width || y < 0 || y >= Heigh)
                return sbyte.MinValue;
            return data[x + y * Width];
        }
    }

    public FP this[int idx]
    {
        get
        {
            if (idx < 0 || idx >= data.Length)
                return sbyte.MinValue * Scale;
            return data[idx]*Scale;
        }
    }

    public FP Sample(TSVector2 pos)
    {
        pos /= Grain;
        int x = (int)FP.Floor(pos.x);
        int y = (int)FP.Floor(pos.y);
        int idx = x + y * Width;
        FP rx = pos.x - x;
        FP ry = pos.y - y;
        //2 3
        //0 1
        FP v0 = this[idx];
        FP v1 = this[idx + 1];
        FP v2 = this[idx + Width];
        FP v3 = this[idx + Width + 1];

        return (v0 * (1 - rx) + v1 * rx) * (1 - ry) + (v2 * (1 - rx) + v3 * rx) * ry;
    }

    public FP Get(Vector2Int pt)
    {
        return this[pt.x + pt.y * Width];
    }

    public void Init(int width, int heigh, FP grain, FP scale, TSVector2 origin, sbyte[] data)
    {
        Width = width;
        Heigh = heigh;
        Grain = grain;
        Scale = scale;
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
