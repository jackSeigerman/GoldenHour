using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;

public class TRNGen_Heightmap : MonoBehaviour
{
    public static float random(float2 uv)
    {
        float rnd = frac(sin(dot(uv.xy, float2(12.9898f, 78.233f))) * 43758.5453123f);
        return rnd;
    }

    static float Smoothstep(float In, float Edge, float Smoothness)
    {
        return smoothstep(Edge, Edge + Smoothness, In);
    }

    static float Ridge(float In)
    {
        In = In * 2;
        In -= 1;
        In = abs(In);
        return In;
    }

    static float RiverMap(Vector2 uv, float scale)
    {
        uv *= scale;

        float largeNoise = Mathf.PerlinNoise(uv.x * 5, uv.y * 5);
        largeNoise = Ridge(largeNoise);

        float smallNoise = Mathf.PerlinNoise(uv.x * 30, uv.y * 30);
        smallNoise = Ridge(smallNoise);

        float blend = Smoothstep(largeNoise, 0.3f, 0.2f);
        largeNoise = Smoothstep(largeNoise, 0, 0.3f);
        smallNoise = Smoothstep(smallNoise, 0, 0.3f);

        float rivers = largeNoise * smallNoise;
        float noise = lerp(rivers, largeNoise, blend);

        return noise;
    }

    static float2 Falloff(Vector2 uv, float width, Vector2 position)
    {
        uv *= 20;

        float extraSmallNoise = Mathf.PerlinNoise(uv.x * 3, uv.y * 3) * 0.2f;
        float smallNoise = Mathf.PerlinNoise(uv.x, uv.y);
        float largeNoise = Mathf.PerlinNoise(uv.x / 2f, uv.y / 2f);
        float beachNoise = Mathf.PerlinNoise(uv.x * 100, uv.y * 100) + Mathf.PerlinNoise(uv.x * 50, uv.y * 50);
        beachNoise /= 2f;
        beachNoise *= 0.6f;
        beachNoise += 0.4f;

        float noise = largeNoise + smallNoise + extraSmallNoise;
        noise /= 2.2f;

        float cliffs = Smoothstep(noise, 0.5f, 0.05f);
        float falloff = Vector2.Distance(new Vector2(width / 2, width / 2), new Vector2(position.x, position.y));
        falloff /= width;
        falloff = 1 - Mathf.Clamp(falloff, 0, 1);

        //float blend = Smoothstep(falloff, 0.5f, 0.2f);
        float blend = Smoothstep(falloff, 0.6f, 0.05f);
        //falloff = Smoothstep(falloff, 0.5f, 0.1f);
        float beaches = Smoothstep(falloff, 0.5f, 0.2f);
        falloff = Smoothstep(falloff, 0.5f, 0.1f);

        float mask = falloff * cliffs;
        //mask = lerp(mask, blend, blend);
        mask += blend;
        mask = clamp(mask, 0, 1);

        return new float2(mask, beaches * beachNoise * 0.1f);
    }

    static float TerrainMap(Vector2 uv, float scale)
    {
        uv *= scale;

        float ridgeNoiseLarge = Mathf.PerlinNoise(uv.x * 10, uv.y * 10);
        float ridgeNoiseSmall = Mathf.PerlinNoise(uv.x * 20, uv.y * 20);

        float shapeNoiseExtraLarge = (ridgeNoiseLarge + (ridgeNoiseSmall * 0.5f)) / 1.5f;

        ridgeNoiseLarge = pow(clamp(1 - Ridge(ridgeNoiseLarge), 0, 1), 2);
        ridgeNoiseSmall = clamp(1 - Ridge(ridgeNoiseSmall), 0, 1);

        float ridgeNoise = ridgeNoiseLarge * ridgeNoiseSmall;

        float shapeNoiseLarge = Mathf.PerlinNoise(uv.x * 100, uv.y * 100) * 0.2f;
        float shapeNoiseSmall = Mathf.PerlinNoise(uv.x * 3000, uv.y * 3000) * 0.05f;

        float noise = ridgeNoise + shapeNoiseLarge + shapeNoiseSmall + shapeNoiseExtraLarge;

        return noise / 2.15f;
    }

    public static float NoiseMap(Vector2 uv, float scale, float riverScale, float terrainScale, float width, Vector2 position, float seed)
    {
        seed = random(new Vector2(seed, seed)) * 1000;
        uv += new Vector2(seed, seed);
        float rivers = RiverMap(uv, riverScale * scale);
        float mountains = TerrainMap(uv, terrainScale * scale);
        float2 mask = Falloff(uv * scale, width, position);

        //return mountains * rivers;
        return max(mountains * mask.x, mountains * mask.y) * rivers;
    }
}
