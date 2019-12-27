using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Board : MonoBehaviour
{
    public int rows = 10;
    public int cols = 10;
    public Tuple<int, int> playerOnePosition;
    public Tuple<int, int> playerTwoPosition;
    public GameObject tilePrefab;
    public Vector3 tileOffset = new Vector3(0.0f, 0, 0.0f);
    public Vector3 boardOffset = new Vector3(-5.0f, 0, -5.0f);
    public GameObject numberPrefab;
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;
    public bool currentPlayer;
    public float playerOneYShift = 0.6f;
    public float playerTwoYShift = 0.7f;

    public Player playerOne;
    public Player playerTwo;
    public Tile[,] tiles;

    private Vector2 mouseOver;



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
            Move(currentPlayer, (int)mouseOver.x, (int)mouseOver.y);

        }

        //Debug.Log(mouseOver);
    }

    public void Move(bool current, int x, int y)
    {
        PlacePlayer(current? playerOne: playerTwo,x,y, current? playerOneYShift: playerTwoYShift);
    }

    private void Generate()
    {
        GenerateTiles();
        GeneratePlayers();
        //FillTilesWithValues();
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

        for (int i = 0; i < rows; i++)
        { 
            for (int j = 0; j < cols; j++)
            {
                GameObject go = Instantiate(tilePrefab) as GameObject;
                go.transform.SetParent(transform);
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

    private void FillTilesWithValues()
    {

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
        playerOne = new Player();
        playerTwo = new Player();
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
