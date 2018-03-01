using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridGenerator {
    private static int X_SIZE, Y_SIZE;

	public static Tile[,] tileMap = new Tile[,]{ };

	public static void GenerateTilemap (int size, int xUnits, int yUnits, float yOffset)
	{
        X_SIZE = xUnits;
        Y_SIZE = yUnits;

		tileMap = new Tile[xUnits, yUnits];
		int cnt=0;
		for(int y=0; y<yUnits; y++)
		{
			for (int x = 0; x < xUnits; x++) {

				Vector3 pos = new Vector3 ((x * size) - size / 2, yOffset, (y * size) - size / 2);
				bool n, s, e, w;
				n = Physics.Raycast (pos, Vector3.forward, size,10);
				s = Physics.Raycast (pos, Vector3.back, size,10);
				e = Physics.Raycast (pos, Vector3.right, size,10);
				w = Physics.Raycast (pos, Vector3.left, size,10);

				Tile tile = new Tile (cnt, pos, !n, !s, !e, !w);
				tileMap [x, y] = tile;
				cnt++;
			}
		}
	}

	public static Tile GetTile(int x, int y)
	{
        Tile tile = null;
        if (x < X_SIZE && x >= 0 && y < Y_SIZE && y >= 0) {
            tile = tileMap[x, y];
        }

		return tile;
	}


}


public class Tile {

	public int ID;
	public Vector3 pos;
	public bool pathNorth, pathSouth, pathEast, pathWest;

	public Tile (int _id, Vector3 _pos, bool _pathN, bool _pathS, bool _pathE, bool _pathW)
	{
		ID = _id;
		pos = _pos;
		pathNorth=_pathN;
		pathSouth=_pathS;
		pathEast=_pathE;
		pathWest=_pathW;
	}
}
