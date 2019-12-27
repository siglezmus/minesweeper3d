using System;
using UnityEngine;

public partial class Board : MonoBehaviour
{
    public int rows = 10;
    public int cols = 10;
    public Tuple<int, int> playerOnePosition;
    public Tuple<int, int> playerTwoPosition;
    
    public GameObject tilePrefab;
    
    public Vector3 tileOffset = new Vector3(0.0f, 0, 0.0f);
    public Vector3 boardOffset = new Vector3(-5.0f, 0, -5.0f);
    
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;
    public bool currentPlayer;
    public float playerOneYShift = 0.6f;
    public float playerTwoYShift = 0.7f;

    public Player playerOne;
    public Player playerTwo;
    public Tile[,] tiles;

    private Vector2 mouseOver;

    public TextMesh[,] NumberTextMeshes;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseOver();
        if (Input.GetMouseButtonDown(0))
        {
            //if turn is valid king template and no other player
            //if there is stamina
            Debug.Log(mouseOver);

            int x = (int) mouseOver.x;
            int y = (int) mouseOver.y;

            //if (kingTemplateAndNoOtherPlayerCheck(x, y))
            //{
                Move(x, y);

            //}

        }

        
    }

    public bool kingTemplateAndNoOtherPlayerCheck(int targetX, int targetY)
    {
        int currentX = currentPlayer ? playerOnePosition.Item1 : playerTwoPosition.Item1;
        int currentY = currentPlayer ? playerOnePosition.Item2 : playerTwoPosition.Item2;

        Tuple<int, int> targetTuple = new Tuple<int, int>(targetX, targetY);

        Tuple<int,int> p1 = new Tuple<int, int>(currentX,currentY);

        int anotherPlayerX = currentPlayer ? playerTwoPosition.Item1 : playerOnePosition.Item1;
        int anotherPlayerY = currentPlayer ? playerTwoPosition.Item2 : playerOnePosition.Item2;

        Tuple<int, int> p2 = new Tuple<int, int>(anotherPlayerX, anotherPlayerY);

        if (Math.Abs(targetX - currentX) <= 1 && Math.Abs(targetY - currentY) <= 1 &&
            (targetX != anotherPlayerX && targetY != anotherPlayerY) &&
            (targetX != currentX && targetY != currentY))
            return true;
        else
            return false;
    }

    public void Move(int x, int y)
    {
        PlacePlayer(currentPlayer ? playerOne: playerTwo,x,y, currentPlayer ? playerOneYShift: playerTwoYShift);

        Tuple<int, int> temp = new Tuple<int, int>(x, y);

        if (currentPlayer)
            playerOnePosition = temp;
        else
            playerTwoPosition = temp;
    }

    private void Generate()
    {
        GenerateTiles();
        GeneratePlayers();

        GenerateMines();
        SetAllTextNumbers();
    }

    private void UpdateMouseOver()
    {
        //if it's my turn
        if (!Camera.main)
        {
            Debug.Log("Cannot find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f,
            LayerMask.GetMask("Board")))
        {
            mouseOver.x = (int)(hit.point.x + 5.0f) ;
            mouseOver.y = (int)(hit.point.z + 5.0f) ;

        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
    }

    private void GenerateTiles()
    {
        tiles = new Tile[rows, cols];
        NumberTextMeshes = new TextMesh[rows, cols];

        for (int i = 0; i < rows; i++)
        { 
            for (int j = 0; j < cols; j++)
            {
                GameObject go = Instantiate(tilePrefab) as GameObject;
                go.transform.SetParent(transform);
                
                NumberTextMeshes[i,j] = go.GetComponentInChildren<TextMesh>();

                Tile t = go.GetComponent<Tile>();
                tiles[i, j] = t;

                PlaceTile(t, i, j);
                t.row = i;
                t.col = j;
            }
        }

        //TryDelete(0,0);
    }

    private void PlaceTile(Tile t, int i, int j)
    {
        t.transform.position = (Vector3.right * i) + (Vector3.forward * j) - boardOffset;
    }

    private void PlacePlayer(Player p, int i, int j, float k)
    {
        p.transform.position = (Vector3.right * i) + (Vector3.forward * j) + (Vector3.up * k) - boardOffset;
    }

    private void PlaceGameObject(GameObject go, Vector3 position)
    {
        go.transform.position = position;
    }

    private bool IsTurnValid(bool currentPlayer)
    {
        return true;
    }

    private void TryDelete(int x, int y)
    {
        Destroy(tiles[x,y].gameObject);
        tiles[x, y] = null;
    }

    private void GeneratePlayers()
    {
        playerOne = new Player(this);
        playerTwo = new Player(this);
        playerOnePosition = new Tuple<int, int>(0, 0);
        playerTwoPosition = new Tuple<int, int>(rows - 1, cols - 1);
        bool currentPlayer = true;

        GameObject go = Instantiate(playerOnePrefab) as GameObject;
        go.transform.SetParent(transform);
        playerOne = go.GetComponent<Player>();
        PlacePlayer(playerOne, playerOnePosition.Item1, playerOnePosition.Item2, playerOneYShift);

        GameObject go1 = Instantiate(playerTwoPrefab) as GameObject;
        go1.transform.SetParent(transform);
        playerTwo = go1.GetComponent<Player>();
        PlacePlayer(playerTwo, playerTwoPosition.Item1, playerTwoPosition.Item2, playerTwoYShift);
    }
}
