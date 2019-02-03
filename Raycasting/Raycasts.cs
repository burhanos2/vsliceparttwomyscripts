using UnityEngine;

public enum DirectionOfWall { Left, Right, Nothing };

public class Raycasts : MonoBehaviour
{
    private LayerMask groundMask;
    private LayerMask wallMask;
    private Renderer rend;
    private Vector3 vectorGroundOffset1;
    private Vector3 vectorGroundOffset2;
    private Vector3 vectorWallOffset = new Vector3(0, 0, 0);

    public bool Grounded {   get { return DoGroundCheck(); }   }

    private DirectionOfWall directionOfWall = DirectionOfWall.Nothing;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        groundMask = LayerMask.GetMask("Ground");
        wallMask = LayerMask.GetMask("Wall");
        vectorGroundOffset1 = new Vector3(rend.bounds.min.x + 0,0,0);
        vectorGroundOffset2 = new Vector3(rend.bounds.max.x + 0,0,0);
    }

    public bool DoGroundCheck()
    {
        float rayCastLength = 1f;
        // ground check

        if (Physics2D.Raycast(transform.localPosition + vectorGroundOffset1, -transform.up, rayCastLength, groundMask))
        { return true; }
        if (Physics2D.Raycast(transform.localPosition + vectorGroundOffset2, -transform.up, rayCastLength, groundMask))
        { return true; }
        else { return false; }
    }

    public DirectionOfWall DoWallCheck()
    {
        float rayCastLength = 0.7f;

        // left wall check
        if (Physics2D.Raycast(transform.localPosition + vectorWallOffset, -transform.right, rayCastLength, wallMask))
        {
            directionOfWall = DirectionOfWall.Left;
            return directionOfWall;
        }
        // right wall check
        if (Physics2D.Raycast(transform.localPosition + vectorWallOffset, transform.right, rayCastLength, wallMask))
        {
            directionOfWall = DirectionOfWall.Right;
            return directionOfWall;
        }
        else {
            directionOfWall = DirectionOfWall.Nothing;
            return directionOfWall;
        }
    }
}
