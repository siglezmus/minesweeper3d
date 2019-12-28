using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public partial class Board : MonoBehaviour
{
    public int rows = 10;
    public int cols = 10;
    public Tuple<int, int> playerOnePosition;
    public Tuple<int, int> playerTwoPosition;

    public GameObject tilePrefab;
    public GameObject textHolderPrefab;

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

    Stopwatch stopwatch = new Stopwatch();

    public TextMesh[,] NumberTextMeshes;

    public TextMesh playerOneHealth;
    public TextMesh playerTwoHealth;

    public TextMesh playerOneStamina;
    public TextMesh playerTwoStamina;

    public TextMesh playerOneCoins;
    public TextMesh playerTwoCoins;

    public TextMesh playerOneTypeOfMoving;
    public TextMesh playerTwoTypeOfMoving;

    public TextMesh CurrentPlayMesh;
    public TextMesh TurnTimeLeft;
    public TextMesh Winner;
    public bool kostyl = true;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    void DisplayStat()
    {
        playerOneHealth.text = "HP1:" + playerOne.currentHealth;
        playerOneHealth.transform.position = new Vector3(-8.0f, 0, 5.0f);
        playerOneCoins.text = "Coins1:" + playerOne.coins;
        playerOneCoins.transform.position = new Vector3(-8.0f, 0, 4.5f);
        playerOneStamina.text = "Stamina1:" + playerOne.currentStamina;
        playerOneStamina.transform.position = new Vector3(-8.0f, 0, 4.0f);
        playerOneTypeOfMoving.text = "Type1:" + (playerOne.currentMovingType ? "Safe" : "Unsafe");
        playerOneTypeOfMoving.transform.position = new Vector3(-8.0f, 0, 3.5f);

        playerTwoHealth.text = "HP2:" + playerTwo.currentHealth;
        playerTwoHealth.transform.position = new Vector3(-8.0f, 0, 3.0f);
        playerTwoCoins.text = "Coins2:" + playerTwo.coins;
        playerTwoCoins.transform.position = new Vector3(-8.0f, 0, 2.5f);
        playerTwoStamina.text = "Stamina2:" + playerTwo.currentStamina;
        playerTwoStamina.transform.position = new Vector3(-8.0f, 0, 2.0f);
        playerTwoTypeOfMoving.text = "Type2:" + (playerTwo.currentMovingType ? "Safe" : "Unsafe");
        playerTwoTypeOfMoving.transform.position = new Vector3(-8.0f, 0, 1.5f);

        CurrentPlayMesh.text = "Current player:" + (currentPlayer ? "P1" : "P2");
        CurrentPlayMesh.transform.position = new Vector3(-8.0f, 0, 1.0f);
        TurnTimeLeft.text = "Time left:" + (90 - stopwatch.Elapsed.Minutes*60 - stopwatch.Elapsed.Seconds);
        TurnTimeLeft.transform.position = new Vector3(-8.0f, 0, 0.5f);
    }

    // Update is called once per frame

    public void RecountTiles(int x, int y)
    {
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i != x && j != y && x < 10 && x >=0 && y < 10 && x >=0)
                    tiles[i, j].number -= 1;
            }
        }
    }
    void Update()
    {
        DisplayStat();

        if (GameIsNotEnded())
        {
            stopwatch.Start();
            SetAllTextNumbers();
            UpdateMouseOver();
            if (Input.GetMouseButtonDown(0))
            {
                //if turn is valid king template and no other player
                //if there is stamina
                //Debug.Log(mouseOver);

                int x = (int)mouseOver.x;
                int y = (int)mouseOver.y;

                if (IsTurnValid(x, y))
                {
                    Move(x, y);
                    /*
                     *             if (tiles[x, y].mine)
                RecountTiles(x, y);
                     */
                }

            }

            if (Input.GetKeyDown(KeyCode.E) || stopwatch.Elapsed.Minutes * 60 + stopwatch.Elapsed.Seconds >= 90)
            {
                EndTurn();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Player p = currentPlayer ? playerOne : playerTwo;
                p.SwitchMovingType();
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                Player p = currentPlayer ? playerOne : playerTwo;
                p.UseAbility1(this);
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                Player p = currentPlayer ? playerOne : playerTwo;
                p.UseAbility2(this);
            }

        }
        else
        {
            //how to do this once
            
            //Console.ReadKey();

            if (kostyl)
            {
                DisplayWinner();
                stopwatch.Reset();
                stopwatch.Stop();
                Thread.Sleep(5000);
                kostyl = false;
                Reset();
            }

            
            //here to wait a little
            
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

    }

    public void DisplayWinner()
    {
        GameObject go = Instantiate(textHolderPrefab) as GameObject;
        go.transform.SetParent(transform);
        Winner = go.GetComponentInChildren<TextMesh>();

        int w = -1;
        String temp;

        if (playerOne.currentHealth == 0)
            w = 2;
        else if (playerTwo.currentHealth == 0)
            w = 1;
        else if(playerOne.coins > playerTwo.coins)
            w = 1;
        else if (playerTwo.coins > playerOne.coins)
            w = 2;
        else
            w = 0;

        if (w == 1)
        {
            temp = "Player one has won";
            Winner.text = temp;
            Winner.transform.position = new Vector3(-1.0f, 5, 0.5f);
        }

        
        else if (w == 2)
        {
            temp = "Player two has won";
            Winner.text = temp;
            Winner.transform.position = new Vector3(-1.0f, 5, 0.5f);
        }
        else if (w == 0)
        {
            temp = "Draw";
            Winner.text = temp;
            Winner.transform.position = new Vector3(-1.0f, 5, 0.5f);
        }

    }


    public void Reset()
    {
        playerOne.Reset();
        playerTwo.Reset();
        playerOnePosition = new Tuple<int, int>(0,0);
        playerTwoPosition = new Tuple<int, int>(rows-1, cols-1);
        PlacePlayer(playerOne, playerOnePosition.Item1, playerOnePosition.Item2, playerOneYShift);
        PlacePlayer(playerTwo, playerTwoPosition.Item1, playerTwoPosition.Item2, playerTwoYShift);

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tiles[i, j].number = 0;
                tiles[i, j].visible = false;
                tiles[i, j].coins = 0;
            }
        }

        GenerateMines();
        FillTiles();
        SetAllTextNumbers();
        Destroy(Winner.gameObject);
        
    }
    public bool GameIsNotEnded()
    {
        //if both players are alive 
        //if there are unopened tiles
        if (playerOne.currentHealth > 0 && playerTwo.currentHealth >= 0 && IsThereAreUnopenedTile())
            return true;
        else
            return false;
    }

    public bool IsThereAreUnopenedTile()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (!tiles[i, j].visible)
                    return true;
            }
        }

        return false;
    }

    //kingTemplateAndNoOtherPlayerCheck
    public bool IsTurnValid(int targetX, int targetY)
    {
        int currentX = currentPlayer ? playerOnePosition.Item1 : playerTwoPosition.Item1;
        int currentY = currentPlayer ? playerOnePosition.Item2 : playerTwoPosition.Item2;

        int anotherPlayerX = currentPlayer ? playerTwoPosition.Item1 : playerOnePosition.Item1;
        int anotherPlayerY = currentPlayer ? playerTwoPosition.Item2 : playerOnePosition.Item2;

        if (Math.Abs(targetX - currentX) <= 1 && Math.Abs(targetY - currentY) <= 1 &&
            (targetX != anotherPlayerX || targetY != anotherPlayerY) &&
            (targetX != currentX || targetY != currentY))
            return true;
        else
            return false;
    }

    public void Move(int x, int y)
    {
        Player p = currentPlayer ? playerOne : playerTwo;

        if (p.IsThereEnoughStaminaToMove())
        {
            tiles[x,y].Open(p);

            PlacePlayer(p, x, y, currentPlayer ? playerOneYShift : playerTwoYShift);

            Tuple<int, int> temp = new Tuple<int, int>(x, y);

            if (currentPlayer)
                playerOnePosition = temp;
            else
                playerTwoPosition = temp;

            //Debug.Log(p.currentStamina);
            //Debug.Log(p.coins);
            

        }

    }

    private void EndTurn()
    {
        Player p = currentPlayer ? playerOne : playerTwo;
        p.EndTurnReset();
        currentPlayer = !currentPlayer;
        stopwatch.Reset();
    }

    private void Generate()
    {
        GenerateStat();
        GenerateTiles();
        GeneratePlayers();
        GenerateMines();
        FillTiles();
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

    }

    private void GenerateStat()
    {

        GameObject go1 = Instantiate(textHolderPrefab) as GameObject;
        go1.transform.SetParent(transform);
        GameObject go2 = Instantiate(textHolderPrefab) as GameObject;
        go2.transform.SetParent(transform);
        GameObject go3 = Instantiate(textHolderPrefab) as GameObject;
        go3.transform.SetParent(transform);
        GameObject go4 = Instantiate(textHolderPrefab) as GameObject;
        go4.transform.SetParent(transform);
        GameObject go5 = Instantiate(textHolderPrefab) as GameObject;
        go5.transform.SetParent(transform);
        GameObject go6 = Instantiate(textHolderPrefab) as GameObject;
        go6.transform.SetParent(transform);
        GameObject go7 = Instantiate(textHolderPrefab) as GameObject;
        go7.transform.SetParent(transform);
        GameObject go8 = Instantiate(textHolderPrefab) as GameObject;
        go8.transform.SetParent(transform);
        GameObject go9 = Instantiate(textHolderPrefab) as GameObject;
        go9.transform.SetParent(transform);
        GameObject go10 = Instantiate(textHolderPrefab) as GameObject;
        go10.transform.SetParent(transform);

        playerOneHealth = go1.GetComponentInChildren<TextMesh>();
        playerTwoHealth = go2.GetComponentInChildren<TextMesh>();

        playerOneStamina = go3.GetComponentInChildren<TextMesh>();
        playerTwoStamina = go4.GetComponentInChildren<TextMesh>();
    
        playerOneCoins = go5.GetComponentInChildren<TextMesh>();
        playerTwoCoins = go6.GetComponentInChildren<TextMesh>();

        playerOneTypeOfMoving = go7.GetComponentInChildren<TextMesh>();
        playerTwoTypeOfMoving = go8.GetComponentInChildren<TextMesh>();

        CurrentPlayMesh = go9.GetComponentInChildren<TextMesh>();
        TurnTimeLeft = go10.GetComponent<TextMesh>();

    }

    private void FillTiles()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {

                if (Equals(playerOnePosition, new Tuple<int, int>(i, j))
                    || Equals(playerTwoPosition, new Tuple<int, int>(i, j)))
                {
                    tiles[i, j].coins = 0;
                    tiles[i, j].visible = true;
                }
                else
                {
                    Random r = new Random();
                    tiles[i, j].coins = r.Next(1,15);
                    tiles[i, j].visible = false;
                }

                
            }
        }
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

    private void GeneratePlayers()
    {
        //playerOne = gameObject.GetComponent<Player>();
        //playerTwo = gameObject.GetComponent<Player>();
        playerOne = gameObject.AddComponent<Player>();
        playerTwo = gameObject.AddComponent<Player>();
        //playerOne = new Player();
        //playerTwo = new Player();
        playerOnePosition = new Tuple<int, int>(0, 0);
        playerTwoPosition = new Tuple<int, int>(rows - 1, cols - 1);
        currentPlayer = true;

        GameObject go = Instantiate(playerOnePrefab) as GameObject;
        go.transform.SetParent(transform);
        playerOne = go.GetComponent<Player>();
        PlacePlayer(playerOne, playerOnePosition.Item1, playerOnePosition.Item2, playerOneYShift);

        GameObject go1 = Instantiate(playerTwoPrefab) as GameObject;
        go1.transform.SetParent(transform);
        playerTwo = go1.GetComponent<Player>();
        PlacePlayer(playerTwo, playerTwoPosition.Item1, playerTwoPosition.Item2, playerTwoYShift);
    }

    public void swapPlayers()
    {
        Tuple<int, int> temp = playerOnePosition;
        playerOnePosition = playerTwoPosition;
        playerTwoPosition = temp;
    }
}
