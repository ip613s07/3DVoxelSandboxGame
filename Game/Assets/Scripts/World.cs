﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

	[SerializeField]
	private Player player = null;

	[SerializeField]
	private Vector3 spawnPosition = Vector3.zero;

	[SerializeField]
	private WorldAttributes worldAttributes = null;
	[SerializeField]
	private BlocksAttributes blocksAttributes = null;
	[SerializeField]
	private ItemsAttributes itemsList = null;

	public WorldAttributes WorldAttributes { get => worldAttributes; }
	public BlocksAttributes BlocksAttributes { get => blocksAttributes; }
	public ItemsAttributes ItemsAttributes { get => itemsList; }

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

	public void GenerateWorld()
	{

		generator.GenerateWorld(this);

		player.Spawn(spawnPosition);

	}

	public void CreateChunk(Vector2Int coord)
	{

		if (Chunks[coord.x, coord.y] == null)
		{

			Chunks[coord.x, coord.y] = new Chunk(new Vector2Int(coord.x, coord.y), this);

		}
		else
		{

			Chunks[coord.x, coord.y].Clear();

		}

	}

	public bool IsChunkInWorld(Vector2 pos)
	{

		if (pos.x < WorldAttributes.WorldSizeInChunks
			&& pos.y < WorldAttributes.WorldSizeInChunks && pos.x >= 0 && pos.y >= 0)
		{

			return true;

		}

		return false;

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

	public bool IsVoxelSolid(Vector3 pos)
	{

		Vector2Int ChunkCoord = GetChunkCoord(pos);
		Vector2Int InChunkCoord = GetInChunkCoord(pos);

		return blocksAttributes.Blocktypes[Chunks[ChunkCoord.x, ChunkCoord.y].Voxels[InChunkCoord.x, Mathf.FloorToInt(pos.y), InChunkCoord.y]].IsSolid;

	}

	public bool IsVoxelSolid(float x, float y, float z)
	{

		int xChunk, zChunk;
		int xInChunk, zInChunk;

		if (x >= WorldAttributes.WorldSizeInBlocks && y >= WorldAttributes.ChunkHeight && z >= WorldAttributes.WorldSizeInBlocks && x < 0 && y < 0 && z < 0)
		{

			return true;

		}

		GetChunkCoord(x, z, out xChunk, out zChunk);
		GetInChunkCoord(x, z, out xInChunk, out zInChunk);

		return blocksAttributes.Blocktypes[Chunks[xChunk, zChunk].Voxels[xInChunk, Mathf.FloorToInt(y), zInChunk]].IsSolid;

	}

	public Vector2Int GetChunkCoord(Vector3 pos)
	{

		int x = Mathf.FloorToInt(pos.x / WorldAttributes.ChunkWidth);
		int z = Mathf.FloorToInt(pos.z / WorldAttributes.ChunkWidth);

		return new Vector2Int(x, z);

	}

	public void GetChunkCoord(float x, float z, out int xChunk, out int zChunk)
	{

		xChunk = Mathf.FloorToInt(x / WorldAttributes.ChunkWidth);
		zChunk = Mathf.FloorToInt(z / WorldAttributes.ChunkWidth);

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

	public void GetInChunkCoord(float x, float z, out int xInChunk, out int zInChunk)
	{

		xInChunk = Mathf.FloorToInt(x % WorldAttributes.ChunkWidth);
		zInChunk = Mathf.FloorToInt(z % WorldAttributes.ChunkWidth);

	}

	public Vector2Int GetInChunkCoord(Vector2 pos)
	{

		int _x = Mathf.FloorToInt(pos.x % WorldAttributes.ChunkWidth);
		int _z = Mathf.FloorToInt(pos.y % WorldAttributes.ChunkWidth);

		return new Vector2Int(_x, _z);

	}

	public int GetBlockID(Vector3 pos)
	{

		Vector2Int ChunkCoord = GetChunkCoord(pos);
		Vector2Int InChunkCoord = GetInChunkCoord(pos);

		return Chunks[ChunkCoord.x, ChunkCoord.y].Voxels[InChunkCoord.x, Mathf.FloorToInt(pos.y), InChunkCoord.y];

	}

	public void EditVoxel(Vector3 pos, byte newID)
	{

		if (!IsVoxelInWorld(pos))
		{

			return;

		}

		Vector2Int ChunkCoord = GetChunkCoord(pos);

		Chunks[ChunkCoord.x, ChunkCoord.y].EditVoxel(pos, newID);

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
