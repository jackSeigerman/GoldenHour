using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;

public class TRN_Filters
{
    public static float random(float2 uv)
    {
        float rnd = frac(sin(dot(uv.xy, float2(12.9898f, 78.233f))) * 43758.5453123f);
        return rnd;
    }

    public static float Smoothstep(float In, float Edge, float Smoothness)
    {
        return smoothstep(Edge, Edge + Smoothness, In);
    }

    public static float Ridge(float In)
    {
        In = In * 2;
        In -= 1;
        In = abs(In);
        return In;
    }

    public static float Exponential(float In, float power)
    {
        return pow(In, power);
    }

    public static float Invert(float In)
    {
        return 1 - In;
    }

    public static float Canyons(float In, float threshold, float power)
    {
        if(In <= threshold)
        {
            return pow(In, power);
        }
        else
        {
            return In;
        }
    }

    public static float Falloff(Vector2 uv, float width, Vector2 position, float beachHeight)
    {
        uv *= 10;

        float falloff = Vector2.Distance(new Vector2(width / 2, width / 2), new Vector2(position.x, position.y));
        falloff /= width;
        falloff = 1 - Mathf.Clamp(falloff, 0, 1);

        float mask = Smoothstep(falloff, 0.52f, 0f);
        float landmass = Smoothstep(falloff, 0.65f, 0f);
        float beaches = Smoothstep(falloff, 0.52f, 0.02f) * beachHeight;

        float cliffs = Mathf.PerlinNoise(uv.x, uv.y);
        cliffs = Smoothstep(cliffs, 0.5f, 0f) * mask;

        falloff = landmass + beaches + cliffs;

        return pow(Mathf.Clamp01(falloff), 2);
    }
}
