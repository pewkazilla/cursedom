using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player instance;
    private void Awake()
    {
        instance = this;
    }
    public enum CardinalDirection
    {
        North,
        East,
        South,
        West
    }
    public CardinalDirection Direction;

    public bool PlayerInControl = true;

    public float rotSpeed, moveSpeed, completionArc, completionBuffer, offTimer;

	public int gridSize = 2, xCnt = 20, yCnt = 20;
	private int xPos, yPos, facingAngle;
	private bool keyF,keyB,keyL,keyR, canRotate, canMove;
	private Vector3 wantedRot;
	private Tile wantedTile;

	public int startTileX,startTileY;

    // Use this for initialization
	void Start () { 
		xPos = startTileX;
		yPos = startTileY;
		GridGenerator.GenerateTilemap (gridSize, xCnt, yCnt, transform.position.y);
		wantedTile = GridGenerator.GetTile (startTileX, startTileY);

	}
		   
    // Update is called once per frame
    void LateUpdate()
    {
        if (PlayerInControl)
        {
            if (!canRotate && Vector3.Distance(transform.eulerAngles, wantedRot) < completionArc)
                canRotate = true;

            if (!canMove && Vector3.Distance(transform.position, wantedTile.pos) < completionBuffer)
                canMove = true;

            float x = InputManager.XAxis,
                  y = InputManager.YAxis;
            int motion = 0;
            int rotate = 0;
            if (y == 1 && keyF == false)
            {
                motion = 1;
                keyF = true;
            }
            else if (y == -1 && keyB == false)
            {
                motion = -1;
                keyB = true;
            }
            else
            {
                if (x == 1 && keyR == false)
                {
                    rotate = 1;
                    keyR = true;
                }
                else if (x == -1 && keyL == false)
                {
                    rotate = -1;
                    keyL = true;
                }
            }
            if (y == 0 && keyF == true)
            {
                keyF = false;
            }

            else if (y == 0 && keyB == true)
            {
                keyB = false;
            }

            else if (y == 0 && keyR == true)
            {
                keyR = false;
            }

            else if (y == 0 && keyL == true)
            {
                keyL = false;
            }

            // choose a new tile to move towards
            if (canMove && motion != 0)
            {
                int directionToMove = facingAngle;
                // if we moved backwards, add 2 modulo 4 to reverse direction
                if (motion == -1)
                {
                    directionToMove += 2;
                    directionToMove = directionToMove % 4;
                }
                int targetX = xPos,
                    targetY = yPos;
                bool canMoveInDirection = true;
                switch (directionToMove)
                {
                    case 0:
                        targetY += 1;
                        canMoveInDirection = wantedTile.pathNorth;
                        break;
                    case 1:
                        targetX += 1;
                        canMoveInDirection = wantedTile.pathEast;
                        break;
                    case 2:
                        targetY -= 1;
                        canMoveInDirection = wantedTile.pathSouth;
                        break;
                    case 3:
                        targetX -= 1;
                        canMoveInDirection = wantedTile.pathWest;
                        break;
                }
                if (canMoveInDirection)
                {
                    Tile target = GridGenerator.GetTile(targetX, targetY);
                    if (target != null)
                    {
                        wantedTile = target;
                        xPos = targetX;
                        yPos = targetY;
                    }
                    else
                    {
                        Debug.Log("Null!");
                    }
                }
                canMove = false;
            }
			/*
			if (!canRotate && canMove) {
				offTimer += Time.deltaTime;
				if (offTimer > 3)
					canRotate = true;
			}

			*/
            if (canRotate && rotate != 0)
            {
                wantedRot = new Vector3(0, wantedRot.y + (90 * rotate), 0);
                if(Mathf.Sign(rotate) == 1)
                {
                    Direction = (CardinalDirection)(((int)Direction + 1)%4);
                }
                else
                {
                    Direction = (CardinalDirection)(((int)Direction - 1 + 4) % 4);
                }

                if (wantedRot.y > 360)
                    wantedRot.y -= 360;
                if (wantedRot.y < 0)
                    wantedRot.y += 360;
                facingAngle = Mathf.RoundToInt((wantedRot.y / 90)) % 4;
                canRotate = false;
				offTimer = 0f;
            }

            transform.position = Vector3.Lerp(transform.position, wantedTile.pos, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, wantedRot.y, Time.deltaTime * rotSpeed), 0));


        }
    }

	public void OnDrawGizmos()
	{
		
		foreach (Tile tile in GridGenerator.tileMap) {
			Gizmos.color = wantedTile == tile ? Color.yellow : Color.white;
			Gizmos.DrawSphere (tile.pos, 0.3f);

			Gizmos.color = tile.pathNorth ? Color.cyan : Color.magenta;
			Gizmos.DrawLine(tile.pos, tile.pos + Vector3.forward); 
			Gizmos.color = tile.pathSouth ? Color.cyan : Color.magenta;
			Gizmos.DrawLine(tile.pos, tile.pos + Vector3.back);
			Gizmos.color = tile.pathEast ? Color.cyan : Color.magenta;
			Gizmos.DrawLine(tile.pos, tile.pos + Vector3.right);
			Gizmos.color = tile.pathWest ? Color.cyan : Color.magenta;
			Gizmos.DrawLine(tile.pos, tile.pos + Vector3.left);
		}
	}

	public void RefreshMap()
	{
		GridGenerator.GenerateTilemap (gridSize, xCnt, yCnt, transform.position.y);
	}

	public void SetTargetNode(int x, int y)
	{
		
		Tile target = GridGenerator.GetTile(x, y);
		if (target != null)
		{
			wantedTile = target;
			yPos = y;
			xPos = x;
		}
	}

	public void SetTargetNodeClosest()
	{
		Tile target=wantedTile;
		float dist = 99999;
		int tx=0, ty=0;
		Tile[,] nodes = GridGenerator.tileMap;
		for(int x = 0; x < xCnt; x++){
			for(int y = 0; y < yCnt; y++){
				if (Vector3.Distance (transform.position, nodes[x,y].pos) < dist) {
					dist = Vector3.Distance (transform.position, nodes[x,y].pos);
					target = GridGenerator.GetTile (x, y);
					tx = x;
					ty = y;
				}
			}
		}
		if (target != null)
		{
			wantedTile = target;
			yPos = ty;
			xPos = tx;
		}
	}
}
