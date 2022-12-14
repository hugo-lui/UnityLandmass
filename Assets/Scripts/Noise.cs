using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
		float[,] noiseMap = new float[mapWidth, mapHeight];
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffset = new Vector2[octaves];
        for(int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffset[i] = new Vector2(offsetX, offsetY);
        }
		if(scale <= 0)
        {
			scale = 0.0000001f;
		}
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

		for(int i = 0; i < mapWidth; i++)
        {
			for(int j = 0; j < mapHeight; j++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                for(int k = 0; k < octaves; k++)
                {
                    float sampleI = (i - halfWidth) / scale * frequency + octaveOffset[k].x;
                    float sampleJ = (j - halfHeight) / scale * frequency + octaveOffset[k].y;
                    float perlinValue = Mathf.PerlinNoise(sampleI, sampleJ) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if(noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if(noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[i, j] = noiseHeight;				
			}
		}
        for(int i = 0; i < mapWidth; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                noiseMap[i, j] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[i, j]);
            }
        }
		return noiseMap;
	}
}