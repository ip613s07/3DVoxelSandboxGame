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

	private IWorldGenerator generator;

    private void Start()
	{

		Chunks = new Chunk[WorldAttributes.WorldSizeInChunks, WorldAttributes.WorldSizeInChunks];

		generator = new FullRandom();

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

		generator = new PerlinNoise();

		GenerateWorld();

	}

	private void GenerateWorld()
	{

		generator.GenerateWorld(this);

	}

	public void CreateChunk(ChunkCoord coord)
	{

		if (Chunks[coord.x, coord.z] == null) {

			Chunks[coord.x, coord.z] = new Chunk(new ChunkCoord(coord.x, coord.z), this, WorldAttributes, BlocksAttributes);

		}
		else
		{

			Chunks[coord.x, coord.z].Clear();

		}

	}

	public bool IsVoxelInWorld(int x, int y, int z)
	{

		if (x < WorldAttributes.WorldSizeInBlocks && z < WorldAttributes.WorldSizeInBlocks && y < WorldAttributes.ChunkHeight)
		{

			return true;

		}

		return false;

	}

	public bool IsVoxelInWorld(int x, int z)
	{

		if (x < WorldAttributes.WorldSizeInBlocks && z < WorldAttributes.WorldSizeInBlocks)
		{

			return true;

		}

		return false;

	}
	
	public ChunkCoord GetChunkCoord(Vector3 pos)
	{

		int x = Mathf.FloorToInt(pos.x / WorldAttributes.ChunkWidth);
		int z = Mathf.FloorToInt(pos.z / WorldAttributes.ChunkWidth);

		return new ChunkCoord(x, z);

	}

	public ChunkCoord GetChunkCoord(Vector2 pos)
	{

		int _x = Mathf.FloorToInt(pos.x / WorldAttributes.ChunkWidth);
		int _z = Mathf.FloorToInt(pos.y / WorldAttributes.ChunkWidth);

		return new ChunkCoord(_x, _z);

	}

}

public class ChunkCoord
{

	public int x;
	public int z;

	public ChunkCoord(int _x, int _z)
	{

		x = _x;
		z = _z;

	}

	public bool Equals(ChunkCoord other)
	{

		if (other == null)
			return false;
		else if (other.x == x && other.z == z)
			return true;
		else
			return false;

	}

}
