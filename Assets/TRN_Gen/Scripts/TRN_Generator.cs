using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TRN_Erosion))]
public class TRN_Generator : MonoBehaviour
{
    [Header("Size")]

    public int width = 4000;
    [SerializeField] private float height = 250;

    [Header("Noise")]

    [SerializeField] private float scale = 5;
    [SerializeField] private bool rivers = true;
    [SerializeField] private float riverScale = 1;
    [SerializeField] private NoiseStructure[] noise;
    [Range(0, 1)]
    [SerializeField] private float beachHeight = 0.35f;

    public float seed = 0;

    [Header("Objects")]

    [SerializeField] private TRNTerrainObject[] prefabs;
    [SerializeField] private TRNDetaiTexture[] detailTextures;
    [HideInInspector] public bool spawnPrefabs = true;

    Terrain terrain;
    float[,] heightMap;

    public void Generate()
    {
        terrain = GetComponent<Terrain>();
        terrain.terrainData.size = new Vector3(width, height, width);
        terrain.drawHeightmap = true;

        int heightSize = terrain.terrainData.heightmapResolution;
        heightMap = new float[heightSize, heightSize];

        for (int x = 0; x < heightSize; x++)
        {
            for (int z = 0; z < heightSize; z++)
            {
                Vector2 uv = new Vector2(x + seed, z + seed) * width / (50000f * 500f);
                float noiseHeight = 0;
                float total = 0;

                List<float> noiseLayers = new List<float>();
                foreach (NoiseStructure structure in noise)
                {
                    noiseLayers.Add(TRN_Noise.LayeredNoise(structure.noiseType, uv, scale * structure.scale, structure.octaves, structure.persistence, structure.lacunarity));
                    total += structure.weight;
                }

                for (int i = 0; i < noiseLayers.Count; i++)
                {
                    float noiseLayer = noiseLayers[i];

                    foreach (NoiseFilter noiseFilter in noise[i].filters)
                    {
                        if (noiseFilter.filterType == FilterType.Canyon)
                        {
                            noiseLayer = TRN_Filters.Canyons(noiseLayer, noiseFilter.threshold, noiseFilter.power);
                        }
                        else if (noiseFilter.filterType == FilterType.Exponential)
                        {
                            noiseLayer = TRN_Filters.Exponential(noiseLayer, noiseFilter.exponent);
                        }
                        else if (noiseFilter.filterType == FilterType.Invert)
                        {
                            noiseLayer = TRN_Filters.Invert(noiseLayer);
                        }
                        else if (noiseFilter.filterType == FilterType.Ridge)
                        {
                            noiseLayer = TRN_Filters.Ridge(noiseLayer);
                        }
                        else if (noiseFilter.filterType == FilterType.Smoothstep)
                        {
                            noiseLayer = TRN_Filters.Smoothstep(noiseLayer, noiseFilter.step, noiseFilter.smoothness);
                        }
                    }

                    noiseHeight += noiseLayer * noise[i].weight;
                }

                float riverMap = rivers ? TRN_Noise.RiverMap(new Vector2(x, z), riverScale): 1f;
                heightMap[x, z] = (noiseHeight / total) * TRN_Filters.Falloff(uv, heightSize, new Vector2(x, z), beachHeight) * riverMap;
            }
        }

        terrain.terrainData.SetHeights(0, 0, heightMap);

        GetComponent<TRN_Erosion>().Run();
        Smooth();
        Smooth();
        Smooth();
        Smooth();

        if (spawnPrefabs)
        {
            SpawnDetailTextures();
            SpawnTrees();
        }

        terrain.terrainData.size = new Vector3(width - 1, height, width);
        terrain.terrainData.size = new Vector3(width, height, width);
    }

    public void Smooth()
    {
        terrain = GetComponent<Terrain>();
        int heightSize = terrain.terrainData.heightmapResolution;
        float[,] _heightMap = terrain.terrainData.GetHeights(0, 0, heightSize, heightSize);
        float[,] smoothedHeightmap = terrain.terrainData.GetHeights(0, 0, heightSize, heightSize);

        for (int x = 0; x < heightSize; x++)
        {
            for (int z = 0; z < heightSize; z++)
            {
                float total = 0;
                int smoothingWidth = 1;
                for (int a = -smoothingWidth; a <= smoothingWidth; a++)
                {
                    for (int b = -smoothingWidth; b <= smoothingWidth; b++)
                    {
                        int xPos = Mathf.Clamp(x + a, 0, heightSize - 1);
                        int zPos = Mathf.Clamp(z + b, 0, heightSize - 1);
                        total += _heightMap[xPos, zPos];
                    }
                }
                total /= ((smoothingWidth * 2) + 1) * ((smoothingWidth * 2) + 1);
                smoothedHeightmap[x, z] = Mathf.Lerp(total, _heightMap[x, z], 1 - Mathf.Pow(1 - _heightMap[x, z], 8));
            }
        }

        terrain.terrainData.SetHeights(0, 0, smoothedHeightmap);
    }

    public void SpawnTrees()
    {
        terrain = GetComponent<Terrain>();

        List<TreeInstance> trees = new List<TreeInstance>();

        TreePrototype[] prototypes = new TreePrototype[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            TreePrototype proto = new TreePrototype();
            proto.prefab = prefabs[i].prefab;
            prototypes[i] = proto;
        }
        terrain.terrainData.treePrototypes = prototypes;

        for (int i = 0; i < prefabs.Length; i++)
        {
            TreeInstance tree = new TreeInstance();
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < width; z++)
                {
                    if (x % prefabs[i].spacing == 0 && z % prefabs[i].spacing == 0)
                    {
                        float xOffset = (TRNGen_Heightmap.random(new Vector2(z + seed * i, x + seed * i)) * 2) - 1;
                        float zOffset = (TRNGen_Heightmap.random(new Vector2(x + seed * i, z + seed * i)) * 2) - 1;

                        Vector2 offset = (new Vector2(xOffset, zOffset) * (prefabs[i].spacing / 2));

                        float _x = (float)(x + offset.x) / width;
                        float _z = (float)(z + offset.y) / width;

                        float _y = terrain.terrainData.GetHeight((int)(_x * terrain.terrainData.heightmapResolution), (int)(_z * terrain.terrainData.heightmapResolution)) / (float)height;
                        float grad = terrain.terrainData.GetSteepness(_x, _z);

                        tree.position = new Vector3(_x, _y, _z);
                        tree.heightScale = (TRNGen_Heightmap.random(new Vector2(z + seed, x + seed)) * (prefabs[i].sizeRange.y - prefabs[i].sizeRange.x)) + prefabs[i].sizeRange.x;
                        tree.widthScale = tree.heightScale;

                        tree.prototypeIndex = i;

                        if (_y > prefabs[i].heightRange.x / height && _y < prefabs[i].heightRange.y / height && grad < 20)
                        {
                            trees.Add(tree);
                        }
                    }
                }
            }
        }

        terrain.terrainData.treeInstances = trees.ToArray();

        terrain.terrainData.size = new Vector3(width - 1, height, width);
        terrain.terrainData.size = new Vector3(width, height, width);
    }

    public Vector3 DetailToWorld(int x, int y)
    {
        //XZ world position
        return new Vector3(
            terrain.GetPosition().x + (((float)x / (float)terrain.terrainData.detailWidth) * (terrain.terrainData.size.x)),
            0f,
            terrain.GetPosition().z + (((float)y / (float)terrain.terrainData.detailHeight) * (terrain.terrainData.size.z))
            );
    }

    public Vector2 GetNormalizedPosition(Vector3 worldPosition)
    {
        Vector3 localPos = terrain.transform.InverseTransformPoint(worldPosition);

        //Position relative to terrain as 0-1 value
        return new Vector2(
            localPos.x / terrain.terrainData.size.x,
            localPos.z / terrain.terrainData.size.z);
    }

    public void SampleHeight(Vector2 position, out float height, out float worldHeight, out float normalizedHeight)
    {
        height = terrain.terrainData.GetHeight(
            Mathf.CeilToInt(position.x * terrain.terrainData.heightmapTexture.width),
            Mathf.CeilToInt(position.y * terrain.terrainData.heightmapTexture.height)
            );

        worldHeight = height + terrain.transform.position.y;
        //Normalized height value (0-1)
        normalizedHeight = height / terrain.terrainData.size.y;
    }

    public void SpawnDetailTextures()
    {
        terrain = GetComponent<Terrain>();

        //Initialize Details

        List<DetailPrototype> prototypes = new List<DetailPrototype>();
        for (int i = 0; i < detailTextures.Length; i++)
        {
            DetailPrototype proto = new DetailPrototype();
            proto.usePrototypeMesh = false;
            proto.renderMode = DetailRenderMode.Grass;
            proto.healthyColor = Color.white;
            proto.dryColor = Color.white;
            proto.prototypeTexture = detailTextures[i].texture;
            proto.minWidth = detailTextures[i].sizeRange.x;
            proto.maxWidth = detailTextures[i].sizeRange.y;
            proto.minHeight = detailTextures[i].sizeRange.x;
            proto.maxHeight = detailTextures[i].sizeRange.y;
            //proto.noiseSeed = Random.Range(0, 5000);
            prototypes.Add(proto);
        }
        terrain.terrainData.detailPrototypes = prototypes.ToArray();

        //Spawn Details

        List<int[,]> detailMaps = new List<int[,]>();

        for (int i = 0; i < detailTextures.Length; i++)
        {
            int[,] map = new int[terrain.terrainData.detailWidth, terrain.terrainData.detailWidth];
            detailMaps.Add(map);
        }

        for (int x = 0; x < terrain.terrainData.detailWidth; x++)
        {
            for (int z = 0; z < terrain.terrainData.detailWidth; z++)
            {
                Vector3 wPos = DetailToWorld(z, x);
                Vector2 normPos = GetNormalizedPosition(wPos);
                SampleHeight(normPos, out _, out wPos.y, out _);

                float grad = terrain.terrainData.GetSteepness(normPos.x, normPos.y);

                for (int i = 0; i < detailTextures.Length; i++)
                {
                    float spacing = detailTextures[i].spacing * 3;
                    int xPos = Mathf.Clamp(x + (int)Random.Range(-spacing, spacing), 1, terrain.terrainData.detailWidth - 1);
                    int zPos = Mathf.Clamp(z + (int)Random.Range(-spacing, spacing), 1, terrain.terrainData.detailWidth - 1);
                    if (wPos.y > detailTextures[i].heightRange.x && wPos.y < detailTextures[i].heightRange.y && grad < 20 && (x % detailTextures[i].spacing == 0 && z % detailTextures[i].spacing == 0))
                    {
                        detailMaps[i][xPos, zPos] = width / 1000;
                    }
                    else
                    {
                        detailMaps[i][xPos, zPos] = 0;
                    }
                }
            }
        }

        //for (int i = 0; i < detailTextures.Length; i++)
        //{
        //    int[,] map = new int[terrain.terrainData.detailWidth, terrain.terrainData.detailWidth];

        //    for (int x = 0; x < terrain.terrainData.detailWidth; x++)
        //    {
        //        for (int z = 0; z < terrain.terrainData.detailWidth; z++)
        //        {
        //            Vector3 wPos = DetailToWorld(z, x);
        //            Vector2 normPos = GetNormalizedPosition(wPos);
        //            SampleHeight(normPos, out _, out wPos.y, out _);

        //            float grad = terrain.terrainData.GetSteepness(normPos.x, normPos.y);

        //            float spacing = (float)detailTextures[i].spacing / 2;
        //            int xPos = Mathf.Clamp(x + (int)Random.Range(-spacing, spacing), 0, terrain.terrainData.detailWidth);
        //            int zPos = Mathf.Clamp(z + (int)Random.Range(-spacing, spacing), 0, terrain.terrainData.detailWidth);
        //            if (wPos.y > detailTextures[i].heightRange.x && wPos.y < detailTextures[i].heightRange.y && grad < 20 && (xPos % detailTextures[i].spacing == 0 && zPos % detailTextures[i].spacing == 0))
        //            {
        //                map[x, z] = width / 1000;
        //            }
        //            else
        //            {
        //                map[x, z] = 0;
        //            }
        //        }
        //    }
        //    detailMaps.Add(map);
        //}

        for (int i = 0; i < detailMaps.Count; i++)
        {
            terrain.terrainData.SetDetailLayer(0, 0, i, detailMaps[i]);
        }

        terrain.terrainData.size = new Vector3(width - 1, height, width);
        terrain.terrainData.size = new Vector3(width, height, width);
    }
}

public enum FilterType
{
    Ridge,
    Exponential,
    Invert,
    Smoothstep,
    Canyon
}

public enum NoiseType
{
    Perlin,
    Voronoi,
    Edges,
    Chebyshev,
    Manhattan,
}

[System.Serializable]
public class NoiseStructure
{
    [Header("Noise")]
    public NoiseType noiseType;
    public float scale = 1;
    public int octaves = 5;
    public float persistence = 0.5f;
    public float lacunarity = 2;
    [Range(0, 1)]
    public float weight = 1;
    [Header("Filters")]
    public NoiseFilter[] filters;
}

[System.Serializable]
public class NoiseFilter
{
    public FilterType filterType;

    [Header("Exponential Settings")]
    public float exponent;

    [Header("Smoothstep Settings")]
    [Range(0, 1)]
    public float step;
    [Range(0, 2)]
    public float smoothness;

    [Header("Canyon Settings")]
    [Range(0, 1)]
    public float threshold;
    public float power;
}

[System.Serializable]
public struct TRNTerrainObject
{
    public GameObject prefab;
    public int spacing;
    public Vector2 sizeRange;
    public Vector2 heightRange;
}

[System.Serializable]
public struct TRNDetaiTexture
{
    public Texture2D texture;
    public int spacing;
    public Vector2 sizeRange;
    public Vector2 heightRange;
}
