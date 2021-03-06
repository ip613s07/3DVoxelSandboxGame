﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

	[SerializeField]
	private WorldAttributes worldAttributes;
	[SerializeField]
	private BlocksAttributes blocksAttributes;

	public WorldAttributes WorldAttributes { get => worldAttributes; }

	public BlocksAttributes BlocksAttributes { get => blocksAttributes; }
    public Chunk[,] Chunks { get; private set; }

	public int[,] Bioms;

	private IWorldGenerator generator;

    private void Start()
	{

		Chunks = new Chunk[WorldAttributes.WorldSizeInChunks, WorldAttributes.WorldSizeInChunks];

		Bioms = new int[WorldAttributes.WorldSizeInBlocks, WorldAttributes.WorldSizeInBlocks];

		generator = new VoronoiPerlinNoiseGenerator();

		GenerateWorld(); 

	}

	private void Update()
	{		

	}
	
	public void GenerateWorldFullRandom()
	{

		generator = new FullRandom();

		GenerateWorld();

	}

	public void GenerateWorldRandomHills()
	{

		generator = new RandomHills();

		GenerateWorld();

	}

	public void GenerateWorldPerlinNoise()
	{

		generator = new PerlinNoiseGenerator();

		GenerateWorld();

	}

	internal void GenerateWorldVoronoiPerlinNoise()
	{

		generator = new VoronoiPerlinNoiseGenerator();

		GenerateWorld();

	}

	internal void GenerateWorldVoronoiDiamondSquare()
	{

		generator = new VoronoiDiamondSquareGenerator();

		GenerateWorld();

	}

	private void GenerateWorld()
	{

		generator.GenerateWorld(this);

	}

	public void CreateChunk(Vector2Int coord)
	{

		if (Chunks[coord.x, coord.y] == null) {

			Chunks[coord.x, coord.y] = new Chunk(new Vector2Int(coord.x, coord.y), this, WorldAttributes, BlocksAttributes);

		}
		else
		{

			Chunks[coord.x, coord.y].Clear();

		}

	}

	public bool IsVoxelInWorld(Vector3 pos)
	{

		if (pos.x < WorldAttributes.WorldSizeInBlocks 
			&& pos.z < WorldAttributes.WorldSizeInBlocks 
			&& pos.y < WorldAttributes.ChunkHeight && pos.x >= 0 && pos.y >= 0 && pos.z >= 0)
		{

			return true;

		}

		return false;

	}

	public bool IsVoxelInWorld(Vector2 pos)
	{

		if (pos.x < WorldAttributes.WorldSizeInBlocks && pos.y < WorldAttributes.WorldSizeInBlocks && pos.x >= 0 && pos.y >= 0)
		{

			return true;

		}

		return false;

	}    

    public int GetTopSoilBlockHeight(Vector2 pos)
	{

		Vector2Int ChunkCoord = GetChunkCoord(pos);
		Vector2Int InChunkCoord = GetInChunkCoord(pos);

		int y;

		for (y = WorldAttributes.ChunkHeight - 1; y > 0; --y)
		{

			if (Chunks[ChunkCoord.x, ChunkCoord.y].Voxels[InChunkCoord.x, y, InChunkCoord.y] != 0 && Chunks[ChunkCoord.x, ChunkCoord.y].Voxels[InChunkCoord.x, y, InChunkCoord.y] != 9)
			{

				break;

			}

		}

		return y;

	}

	public int GetTopNonAirBlockHeight(Vector2 pos)
	{

		Vector2Int ChunkCoord = GetChunkCoord(pos);
		Vector2Int InChunkCoord = GetInChunkCoord(pos);

		int y;

		for (y = WorldAttributes.ChunkHeight - 1; y > 0; --y)
		{

			if (Chunks[ChunkCoord.x, ChunkCoord.y].Voxels[InChunkCoord.x, y, InChunkCoord.y] != 0)
			{

				break;

			}

		}

		return y;

	}

	public Vector2Int GetChunkCoord(Vector3 pos)
	{

		int x = Mathf.FloorToInt(pos.x / WorldAttributes.ChunkWidth);
		int z = Mathf.FloorToInt(pos.z / WorldAttributes.ChunkWidth);

		return new Vector2Int(x, z);

	}

	public Vector2Int GetChunkCoord(Vector2 pos)
	{

		int _x = Mathf.FloorToInt(pos.x / WorldAttributes.ChunkWidth);
		int _z = Mathf.FloorToInt(pos.y / WorldAttributes.ChunkWidth);

		return new Vector2Int(_x, _z);

	}

	public Vector2Int GetInChunkCoord(Vector3 pos)
	{

		int x = Mathf.FloorToInt(pos.x % WorldAttributes.ChunkWidth);
		int z = Mathf.FloorToInt(pos.z % WorldAttributes.ChunkWidth);

		return new Vector2Int(x, z);

	}

	public Vector2Int GetInChunkCoord(Vector2 pos)
	{

		int _x = Mathf.FloorToInt(pos.x % WorldAttributes.ChunkWidth);
		int _z = Mathf.FloorToInt(pos.y % WorldAttributes.ChunkWidth);

		return new Vector2Int(_x, _z);

	}

	public void SetVoxel(Vector3 pos, byte id)
	{

		Vector2Int ChunkCoord = GetChunkCoord(pos);
		Vector2Int InChunkCoord = GetInChunkCoord(pos);

		Chunks[ChunkCoord.x, ChunkCoord.y].Voxels[InChunkCoord.x, Mathf.FloorToInt(pos.y), InChunkCoord.y] = 7;

	}

	internal void UpdateChunks()
	{

		for (int x = 0; x < WorldAttributes.WorldSizeInChunks; ++x)
		{

			for (int z = 0; z < WorldAttributes.WorldSizeInChunks; ++z)
			{

				Chunks[x, z].Update();

			}

		}

	}

}
