using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveBehavior : StateMachineBehaviour
{
    #region variables
    [SerializeField] float moveSpeed;    
    [HideInInspector] public int timerValue;
    int x,y;
    public AnimationCurve moveAnimation;  
    Transform player;
    SceneChange sChange;
    GridTiles[,] grid;
    InGameUI UI;
    DoCoroutine doC;
    float lerper;
    int previousX;
    int previousY;
    bool flag;
    bool awake = true;
    public bool canMove;
    Vector3 startPos;
    SceneChange sceneChange;
    SkinnedMeshRenderer pSRend;
    #endregion


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lerper = 0;
        if (awake)
        {
            pSRend = FindObjectOfType<SkinnedMeshRenderer>();
            sceneChange = FindObjectOfType<SceneChange>();
            doC = animator.GetComponent<DoCoroutine>();
            UI = FindObjectOfType<InGameUI>();
            grid = FindObjectOfType<GridGenerator>().grid;
            player = FindObjectOfType<Player>().transform;
            sChange = FindObjectOfType<SceneChange>();
        }
        startPos = player.position;
        canMove = true;
        if (animator.GetBool("Rewind"))
        {
            x = (int)SwipeInput.rewindPos[SwipeInput.rewindPos.Count - 1].x;
            y = (int)SwipeInput.rewindPos[SwipeInput.rewindPos.Count - 1].y;

        }
        else
        {
            x = animator.GetInteger("TargetInfoX");
            y = animator.GetInteger("TargetInfoY");
        }

        previousX = animator.GetInteger("PreviousX");
        previousY = animator.GetInteger("PreviousY");

        flag = true;
               
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(canMove)
        {
            Move(animator,stateInfo);
        }

    }


    void Move(Animator anim, AnimatorStateInfo stateInfo)
    {
        if (anim.GetBool("Rewind") && flag == true)
        {
            
            flag = false;
            TileEffectOnMove(x, y, anim);
        }

        float distance = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(x, y));
        if (/*distance > 0*/ lerper < 1 && grid[x, y].walkable)
        {
            lerper += Time.deltaTime * moveSpeed;
            pSRend.transform.localPosition = new Vector3(0,moveAnimation.Evaluate(lerper) * 1, 0);

            if(lerper <= .5f)
            {
                pSRend.SetBlendShapeWeight(0,Mathf.Lerp(0,100,lerper));

            }
            else
            {
                pSRend.SetBlendShapeWeight(0, Mathf.Lerp(100, 0, lerper));
            }

            player.position = Vector3.Lerp(startPos, new Vector3(grid[x, y].transform.position.x, player.position.y, grid[x, y].transform.position.z), lerper);
            //Vector3 moveDir = (new Vector3(x, /*1.5f + grid[x, y].transform.position.y*/player.position.y, y) - player.position).normalized;
            
            //player.position += moveDir * moveSpeed * Time.deltaTime;
            if(lerper <.2f)
                player.LookAt(new Vector3(x, player.position.y/*1.5f + grid[x, y].transform.position.y*/, y));

            /*if (lerper >= 1)
            {
                lerper = 0;
                player.position = new Vector3(x, player.position.y*//*1.5f + grid[x, y].transform.position.y*//*, y);
                *//*if (anim.GetBool("Rewind"))
                    Debug.Log(SwipeInput.rewindPos[SwipeInput.rewindPos.Count-1]);*//*
            }*/
        }
        else
        {
            player.position = new Vector3(x, player.position.y , y);
            if (anim.GetBool("Rewind"))
            {
                UI.timerValue--;
            }
            else
            {
                UI.timerValue++;
            }

            if (!anim.GetBool("Rewind"))
            {
                TileEffectOnMove(x, y, anim);
            }

            if (anim.GetBool("Rewind"))
            {
                anim.SetBool("Rewind", false);
                SwipeInput.rewindPos.RemoveAt(SwipeInput.rewindPos.Count - 1);
            }

            if (stateInfo.IsName("Move"))
            {
                anim.SetTrigger("moveToTempo");
            }            
            else if (stateInfo.IsName("MoveOntoNormal"))
            {
                canMove = false;
                anim.SetBool("OntonormalTileMove", false);
            }
        }
    }

    void TileEffectOnMove(int x, int y, Animator anim)
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Character/Walk");
        //timerValue++;
        if (sceneChange.Hub)
        {
            if (grid[anim.GetInteger("PreviousX"), anim.GetInteger("PreviousY")].World > 0)
            {
                grid[anim.GetInteger("PreviousX"), anim.GetInteger("PreviousY")].gameObject.transform.Find("World/CanvasCam").gameObject.SetActive(false);
            }
            if (grid[x, y].World > 0)
            {
                grid[x, y].gameObject.transform.Find("World/CanvasCam").gameObject.SetActive(true);
            }
        }
        else if (grid[x, y].levelTransiIndex != 0)
        {
            if(grid[x, y].levelTransiIndex >= 1)
                sChange.StartCoroutine(sChange.Lerper(UI.startPosX, UI.endPosX));

            foreach (GridTiles tile in grid)
            {
                if(tile.levelTransiIndex != grid[x, y].levelTransiIndex)
                {
                    tile.levelTransiIndex = 100;
                    
                    doC.startClose(tile, grid[x,y].levelTransiIndex, grid[x,y].GetComponent<GridTiling>());
                }
            }
        }
            

        //sChange.startCoroutine(grid[x, y]);

        if (anim.GetBool("Rewind") && grid[previousX, previousY].teleporter != 0)
        {
            player.position = new Vector3(grid[previousX, previousY].TpTarget.transform.position.x, grid[previousX, previousY].TpTarget.transform.position.y + 1.5f, grid[previousX, previousY].TpTarget.transform.position.z);
        }
        else if (grid[x, y].teleporter != 0 && !anim.GetBool("Rewind"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/World/TP");
            player.position = new Vector3(grid[x, y].TpTarget.transform.position.x, grid[x, y].TpTarget.transform.position.y + 1.5f, grid[x, y].TpTarget.transform.position.z);
        }

        
        if (grid[x, y].key != 0)
            KeyBehavior(grid[x, y]);

        if (anim.GetBool("Rewind") && grid[previousX, previousY].crumble)
        {
            grid[previousX, previousY].crumbleBool = true;
            if (grid[previousX, previousY].crumbleUp)
                grid[previousX, previousY].crumbleUp = false;
            else if (!grid[previousX, previousY].crumbleUp)
                grid[previousX, previousY].crumbleUp = true;
        }
        else if (grid[x, y].crumble)
        {
            grid[x, y].crumbleBool = true;
            if (grid[x, y].crumbleUp)
                grid[x, y].crumbleUp = false;
            else if (!grid[x, y].crumbleUp)
                grid[x, y].crumbleUp = true;
        }

        if (anim.GetBool("Rewind"))
        { 
            if (grid[previousX, previousY].key !=0)
            {
                KeyBehavior(grid[previousX, previousY]);
            }
        }
    }

    void KeyBehavior(GridTiles tile)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/World/GetKey");
        //tile.transform.Find("Key").gameObject.SetActive(false);
        foreach (GridTiles t in grid)
        {
            if(t.door == tile.key && t.door > 0)
            {
               doC.startClose(t, t.levelTransiIndex, t.GetComponent<GridTiling>());
            }
        }
    }
}
