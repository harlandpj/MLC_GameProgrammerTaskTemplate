using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; }

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    //public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;
    
    public SmartPawn smartPawnPrefab;
    public King smartKingPrefab;

    private Quaternion whiteOrientation = Quaternion.Euler(0, 270, 0);
    private Quaternion blackOrientation = Quaternion.Euler(0, 90, 0);

    public Chessman[,] Chessmans { get; set; }
    private Chessman selectedChessman;

    public bool isWhiteTurn = true;

    private Material previousMat;
    public Material selectedMat;

    public int[] EnPassantMove { set; get; }

    [SerializeField] bool doBuildPieces = true;
    [SerializeField] SmartPawnView smartPawnView;
    [SerializeField] SmartPawnBuilder smartPawnBuilder;
    [SerializeField] SmartPawnCombatResolver smartPawnCombatResolver;
    [SerializeField] GameObject battleOverlay;

    // Use this for initialization
    async void Start()
    {
        Instance = this;
        battleOverlay.SetActive(false);
        SpawnAllChessmans();

        if(doBuildPieces)
        {
            Debug.Log("Building pawns...");
            int buildCount = 0;
            var allTasks = new List<Task>();
            foreach(var chessman in Chessmans)
            {
                if (chessman is SmartPawn) // TODO: can we multi-thread with GPT4ALL??
                {
                    allTasks.Add(smartPawnBuilder.BuildPawn(chessman as SmartPawn));
    /*                await smartPawnBuilder.BuildPawn(chessman as SmartPawn);
                    Debug.Log($"Built pawn {buildCount++}.");
    */            
                }
            }

            await Task.WhenAll(allTasks.ToArray());
        }

        EnPassantMove = new int[2] { -1, -1 };
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelection();
        if(selectionX >= 0
            && selectionY >= 0
            && selectionX < 8
            && selectionY < 8)
        {
            HoverChessman(selectionX, selectionY);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedChessman == null)
                {
                    // Select the chessman
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    // Move the chessman
                    MoveChessman(selectionX, selectionY);
                }
            }
        }

        if (Input.GetKey("escape"))
            Application.Quit();
    }

    private void HoverChessman(int x, int y)
    {
        smartPawnView.selectedSmartPawn = null;

        if (Chessmans[x, y] == null) return;

        if (Chessmans[x, y] is SmartPawn)
            smartPawnView.selectedSmartPawn = Chessmans[x, y] as SmartPawn;
    }

    private void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null) return;

        if (Chessmans[x, y].isWhite != isWhiteTurn) return;

        bool hasAtLeastOneMove = false;

        allowedMoves = Chessmans[x, y].PossibleMoves();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    hasAtLeastOneMove = true;
                    i = 8;
                    break;
                }
            }
        }

        if (!hasAtLeastOneMove)
            return;

        selectedChessman = Chessmans[x, y];
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;

        BoardHighlights.Instance.HighLightAllowedMoves(allowedMoves);
    }

    private void CalculateVictory(
        Chessman attackFrom,
        Chessman attackTo)
    {

        IEnumerator DelayCalc(
            Chessman attackFrom,
            Chessman attackTo)
        {
            battleOverlay.SetActive(true);
            battleOverlay.GetComponentInChildren<TMP_Text>().text = string.Format(
                SmartPawnCombatResolver.BATTLE_PROMPT_TEMPLATE,
                (attackFrom as SmartPawn).characterName,
                (attackTo as SmartPawn).characterName,
                (attackFrom as SmartPawn).characterWeapon,
                (attackTo as SmartPawn).characterWeapon) + ". \n\nProcessing...";
            yield return new WaitForSeconds(0.2f);
            smartPawnCombatResolver.ResolveBattle(
                attackFrom as SmartPawn,
                attackTo as SmartPawn,
                OnBattleResolved);
            battleOverlay.SetActive(false);
        }

        StartCoroutine(DelayCalc(attackFrom, attackTo));
    }

    private void OnBattleResolved(SmartPawnCombatResolver.BattleResult result)
    {
        if (result.result == SmartPawnCombatResolver.BattleResultValue.DefenderDies)
        {
            var x = result.defender.CurrentX;
            var y = result.defender.CurrentY;
            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;

            activeChessman.Remove(result.defender.gameObject);
            Destroy(result.defender.gameObject);
        }

        ResetSelection();
    }

    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            Chessman c = Chessmans[x, y];

            if (c != null && c.isWhite != isWhiteTurn)
            {
                // Capture a piece

                if (c.GetType() == typeof(King))
                {
                    // End the game
                    EndGame();
                    return;
                }

                CalculateVictory(selectedChessman, c);
            }
            else
            {
                Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
                selectedChessman.transform.position = GetTileCenter(x, y);
                selectedChessman.SetPosition(x, y);
                Chessmans[x, y] = selectedChessman;
                ResetSelection();
            }

            isWhiteTurn = !isWhiteTurn;
        }
        else
        {
            ResetSelection();
        }
    }

    void ResetSelection()
    {
        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;

        BoardHighlights.Instance.HideHighlights();
        selectedChessman = null;
    }

    private void UpdateSelection()
    {
        if (!Camera.main) return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnChessman(GameObject prefab, int x, int y, bool isWhite)
    {
        Vector3 position = GetTileCenter(x, y);
        GameObject go;

        if (isWhite)
        {
            go = Instantiate(prefab, position, whiteOrientation);
            go.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            go.GetComponentInChildren<Chessman>().isWhite = true;
        }
        else
        {
            go = Instantiate(prefab, position, blackOrientation);
            go.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
            go.GetComponentInChildren<Chessman>().isWhite = false;
        }

        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);

        var hoverable = go.AddComponent<Hoverable>();
        hoverable.OnHoverEnter.AddListener(smartPawnView.OnHoverEnter);
        hoverable.OnHoverExit.AddListener(smartPawnView.OnHoverExit);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;

        return origin;
    }

    private void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

        foreach (var team in new bool[] { true, false } )
        {
            int offset = team ? 0 : 5;

            for(int i = 1; i < 2; ++i)
            {
                for(int j = 3; j < 5; ++j)
                {
                    //if(i != (team ? 0 : 1) || j != 3)
                    SpawnChessman(smartPawnPrefab.gameObject, j, offset + i, team);
                }
            }

            // King
            SpawnChessman(smartKingPrefab.gameObject, 3, team ? 0 : 7, team);
        }
    }

    private void EndGame()
    {
        if (isWhiteTurn)
            Debug.Log("White wins");
        else
            Debug.Log("Black wins");

        foreach (GameObject go in activeChessman)
        {
            Destroy(go);
        }

        isWhiteTurn = true;
        BoardHighlights.Instance.HideHighlights();
        SpawnAllChessmans();
    }
}


