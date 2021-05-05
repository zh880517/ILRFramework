using System.Collections;
using UnityEngine;

namespace SharpRecast
{
    public class SDFGenerate
    {
        private static readonly Vector2Int inside = Vector2Int.zero;
        private static readonly Vector2Int empty = new Vector2Int(999, 999);
        public class Grid
        {
            public Vector2Int[,] Data;
            public int Width;
            public int Heigh;

            public Grid(int width, int heigh)
            {
                Width = width;
                Heigh = heigh;
                Data = new Vector2Int[width, heigh];
            }

            Vector2Int Get(int x, int y)
            {
                if (x < 0 || y < 0 || x >= Width || y >= Heigh)
                    return empty;
                return Data[x, y];
            }

            public Vector2Int Compare(Vector2Int p, int x, int y, int offsetX, int offsetY)
            {
                var other = Get(x + offsetX, y + offsetY);
                other.x += offsetX;
                other.y += offsetY;
                if (other.sqrMagnitude < p.sqrMagnitude)
                    return other;
                return p;
            }

            public void Cacl()
            {
                for (int j = 0; j < Heigh; ++j)
                {
                    for (int i = 0; i < Width; ++i)
                    {
                        var p = Data[i, j];
                        p = Compare(p, i, j, -1, 0);
                        p = Compare(p, i, j, 0, -1);
                        p = Compare(p, i, j, -1, -1);
                        p = Compare(p, i, j, 1, -1);
                        Data[i, j] = p;
                    }
                    for (int i = Width - 1; i >= 0; --i)
                    {
                        var p = Data[i, j];
                        p = Compare(p, i, j, 1, 0);
                        Data[i, j] = p;
                    }
                }

                for (int j = Heigh - 1; j >= 0; --j)
                {
                    for (int i = Width - 1; i >= 0; --i)
                    {
                        var p = Data[i, j];
                        p = Compare(p, i, j, 1, 0);
                        p = Compare(p, i, j, 0, 1);
                        p = Compare(p, i, j, -1, 1);
                        p = Compare(p, i, j, 1, 1);
                        Data[i, j] = p;
                    }
                    for (int i = 0; i < Width; ++i)
                    {
                        var p = Data[i, j];
                        p = Compare(p, i, j, -1, 0);
                        Data[i, j] = p;
                    }
                }
            }

        }

        public static void Gen(BitArray bitArray, int width, int heigh, float cellSize, float scale, sbyte[] result)
        {
            Grid grid1 = new Grid(width, heigh);
            Grid grid2 = new Grid(width, heigh);
            for (int i=0; i<bitArray.Length; ++i)
            {
                int x = i % width;
                int y = i / width;
                if (bitArray.Get(i))
                {
                    grid1.Data[x, y] = inside;
                    grid2.Data[x, y] = empty;
                }
                else
                {
                    grid2.Data[x, y] = inside;
                    grid1.Data[x, y] = empty;
                }
            }
            grid1.Cacl();
            grid2.Cacl();
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < heigh; ++j)
                {
                    float val = grid2.Data[i, j].magnitude - grid1.Data[i, j].magnitude;
                    val *= cellSize;
                    val /= scale;
                    result[i + j * width] = (sbyte)Mathf.Clamp((int)val, sbyte.MinValue, sbyte.MaxValue);
                }
            }
        }

        private Vector2Int[,] grid1;
        private Vector2Int[,] grid2;
        private int width;
        private int heigh;
        public SDFGenerate(BitArray bitArray, int width, int heigh)
        {
            this.width = width;
            this.heigh = heigh;
            grid1 = new Vector2Int[width, heigh];
            grid2 = new Vector2Int[width, heigh];
            for (int i=0; i<width; ++i)
            {
                for (int j=0; j<heigh; ++j)
                {
                    if (bitArray.Get(i+j*width))
                    {
                        grid1[i, j] = inside;
                        grid2[i, j] = empty;
                    }
                    else
                    {
                        grid1[i, j] = empty;
                        grid2[i, j] = inside;
                    }
                }
            }
            Gen(grid1);
            Gen(grid2);
        }

        private Vector2Int Get(Vector2Int[,] grid, int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= heigh)
                return empty;
            return grid[x, y];
        }

        private Vector2Int Compare(Vector2Int[,] grid, Vector2Int p, int x, int y, int offsetX, int offsetY)
        {
            var other = Get(grid,x + offsetX, y + offsetY);
            other.x += offsetX;
            other.y += offsetY;
            if (other.sqrMagnitude < p.sqrMagnitude)
                return other;
            return p;
        }

        private void Gen(Vector2Int[,] grid)
        {
            for (int j = 0; j < heigh; ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    var p = grid[i, j];
                    p = Compare(grid, p, i, j, -1, 0);
                    p = Compare(grid, p, i, j, 0, -1);
                    p = Compare(grid, p, i, j, -1, -1);
                    p = Compare(grid, p, i, j, 1, -1);
                    grid[i, j] = p;
                }
                for (int i=width -1; i>=0; --i)
                {
                    var p = grid[i, j];
                    p = Compare(grid, p, i, j, 1, 0);
                    grid[i, j] = p;
                }
            }

            for (int j = heigh-1; j >=0; --j)
            {
                for (int i = width - 1; i >= 0; --i)
                {
                    var p = grid[i, j];
                    p = Compare(grid, p, i, j, 1, 0);
                    p = Compare(grid, p, i, j, 0, 1);
                    p = Compare(grid, p, i, j, -1, 1);
                    p = Compare(grid, p, i, j, 1, 1);
                    grid[i, j] = p;
                }
                for (int i = 0; i < width; ++i)
                {
                    var p = grid[i, j];
                    p = Compare(grid, p, i, j, -1, 0);
                    grid[i, j] = p;
                }
            }
        }

        public Texture2D ToTexture()
        {
            float[,] sdData = new float[width,heigh];
            float min = float.MaxValue, max = float.MinValue;
            for (int i=0; i<width; ++i)
            {
                for (int j=0; j<heigh; ++j)
                {
                    float val = grid2[i, j].magnitude - grid1[i, j].magnitude;
                    sdData[i, j] = val;
                    if (val < min)
                        min = val;
                    if (val > max)
                        max = val;
                }
            }
            Texture2D texture = new Texture2D(width, heigh);
            float range = max - min;
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < heigh; ++j)
                {
                    float val = sdData[i, j];
                    val = (val - min) / range;
                    texture.SetPixel(i, j, Color.white * val);
                    //texture.SetPixel(i, j, val > 0 ? Color.white : Color.black);
                }
            }
            return texture;
        }
    }

}
