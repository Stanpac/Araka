using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopCycle : MonoBehaviour
{
    #region Variables

    [Header("Input Values")]
    [Space]
    public int tempoValue;

    [Header("Components")]
    [Space]
    GridTiles[,] grid;
    StepAssignement sAss;
    PlayerMovement pMov;
    Reset reset;
    Transform Player;

    [Header("Hidden Values")]
    [Space]
    int resetTimer;
    int inverseResetTimer;
    int resetStart;
    [HideInInspector] public int tempoIndexValue;
    bool flag = true;
    int maxIndexValue = 0;
    #endregion

    private void Awake()
    {
        reset = GetComponent<Reset>();
        resetStart = reset.resetTimerValue;
        sAss = GetComponent<StepAssignement>();
        pMov = GetComponent<PlayerMovement>();
        
        grid = GetComponent<GridGenerator>().grid;
        Player = FindObjectOfType<Player>().transform;
        foreach(GridTiles obj in grid)
        {
            if(obj.tempoTile > maxIndexValue)
            {
                maxIndexValue = obj.tempoTile;
            }
        }

    }

    private void Update()
    {
        inverseResetTimerValueSet();

        if (maxIndexValue > 1)
        {
            tempoTileCycleIncr();
        }

    }

    void inverseResetTimerValueSet()
    {
        resetTimer = reset.resetTimer;
        inverseResetTimer = resetStart - resetTimer;
    }

    void tempoTileCycleIncr()
    {
        if (flag)
        {
            if (inverseResetTimer % tempoValue >= tempoValue - 1)
            {
                tempoIndexValue++;
                pMov.highlightedTiles.Clear();
                pMov.currentPathIndex = 0;
                StartCoroutine(InvokeIni());
                flag = false;
            }
        }

        if (inverseResetTimer % tempoValue < tempoValue - 1)
        {
            flag = true;
        }
        tempoIndexValue %= maxIndexValue;
    }


    //Rubberband fix to change later
    IEnumerator InvokeIni()
    {
        yield return new WaitForSeconds(.1f);
        sAss.Initialisation();
        Player.position = new Vector3(Player.position.x, grid[(int)Player.position.x, (int)Player.position.z].transform.position.y + 1.5f, Player.position.z);
    }

}
