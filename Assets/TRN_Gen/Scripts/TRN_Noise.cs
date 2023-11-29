using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;

public class TRN_Noise : MonoBehaviour
{
    public static float LayeredNoise(NoiseType noiseType, Vector2 uv, float scale, int octaves, float persistence = 2, float lacunarity = 0.5f)
    {
        uv *= scale;
        float Noise = 0;
        float strength = 1;
        float total = 0;
        for (int i = 0; i < octaves; i++)
        {
            if(noiseType == NoiseType.Perlin)
            {
                Noise += Mathf.PerlinNoise(uv.x, uv.y) * strength;
            }
            else if (noiseType == NoiseType.Voronoi)
            {
                Noise += Voronoi_F1F2(new float2(uv.x, uv.y)).x * strength;
            }
            else if (noiseType == NoiseType.Edges)
            {
                Noise += Voronoi_Edges(new float2(uv.x, uv.y)) * strength;
            }
            else if (noiseType == NoiseType.Chebyshev)
            {
                Noise += Voronoi_ManhattanChebyshev(new float2(uv.x, uv.y)).y * strength;
            }
            else if (noiseType == NoiseType.Manhattan)
            {
                Noise += Voronoi_ManhattanChebyshev(new float2(uv.x, uv.y)).x * strength;
            }
            uv *= lacunarity;
            total += strength;
            strength *= persistence;
        }
        return Noise / total;
    }

    static float2 RandomVector(float2 UV, float offset)
    {
        float2x2 m = float2x2(15.27f, 47.63f, 99.41f, 89.98f);
        UV = frac(sin(mul(UV, m)));
        return float2(sin(UV.y * + offset) * 0.5f + 0.5f, cos(UV.x * offset) * 0.5f + 0.5f);
    }

    public static float RiverMap(Vector2 uv, float scale)
    {
        uv *= scale;
        uv /= 1500f;

        float largeNoise = Mathf.PerlinNoise(uv.x * 5, uv.y * 5);
        largeNoise = TRN_Filters.Ridge(largeNoise);

        float smallNoise = Mathf.PerlinNoise(uv.x * 30, uv.y * 30);
        smallNoise = TRN_Filters.Ridge(smallNoise);

        float blend = TRN_Filters.Smoothstep(largeNoise, 0.3f, 0.2f);
        largeNoise = TRN_Filters.Smoothstep(largeNoise, 0, 0.3f);
        smallNoise = TRN_Filters.Smoothstep(smallNoise, 0, 0.3f);

        float rivers = largeNoise * smallNoise;
        float noise = lerp(rivers, largeNoise, blend);

        return largeNoise;
    }

    public static float2 Voronoi_F1F2(float2 UV, float AngleOffset = 15)
    {
        float2 g = floor(UV);
        float2 f = frac(UV);
        float2 res = float2(8f, 8f);

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                float2 lattice = float2(x, y);
                float2 offset = RandomVector(lattice + g, AngleOffset);
                float dist = distance(lattice + offset, f);

                if (dist < res.x)
                {
                    res.y = res.x;
                    res.x = dist;
                }
                else if (dist < res.y)
                {
                    res.y = dist;
                }
            }
        }

        return res;
    }

    public static float2 Voronoi_ManhattanChebyshev(float2 UV, float AngleOffset = 15)
    {
        float2 g = floor(UV);
        float2 f = frac(UV);
        float2 res = float2(8f, 8f);

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                float2 lattice = float2(x, y);
                float2 offset = RandomVector(lattice + g, AngleOffset);
                float2 manhattan = abs((lattice + offset) - f);
                float manhattanDist = (manhattan.x + manhattan.y) / sqrt(2);

                if (manhattanDist < res.x)
                {
                    res.y = res.x;
                    res.x = manhattanDist;
                }
                else if (manhattanDist < res.y)
                {
                    res.y = manhattanDist;
                }
            }
        }

        res.y = res.y - res.x;

        return res;
    }

    public static float Voronoi_Edges(float2 UV, float AngleOffset = 15)
    {
        float2 g = floor(UV);
        float2 f = frac(UV);
        float2 res = new float2(8f, 8f);
        float2 ml = float2(0, 0);
        float2 mv = float2(0, 0);

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                float2 lattice = float2(x, y);
                float2 offset = RandomVector(g + lattice, AngleOffset);
                float2 v = lattice + offset - f;
                float d = dot(v, v);

                if (d < res.x)
                {
                    res.x = d;
                    res.y = offset.x;
                    mv = v;
                    ml = lattice;
                }
            }
        }

        res = new float2(8f, 8f);
        for (int y = -2; y <= 2; y++)
        {
            for (int x = -2; x <= 2; x++)
            {
                float2 lattice = ml + float2(x, y);
                float2 offset = RandomVector(g + lattice, AngleOffset);
                float2 v = lattice + offset - f;

                float2 cellDifference = abs(ml - lattice);
                if (cellDifference.x + cellDifference.y > 0.1)
                {
                    float d = dot(0.5f * (mv + v), normalize(v - mv));
                    res.x = min(res.x, d);
                }
            }
        }

        return res.x;
    }
}