using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepAssignement : MonoBehaviour
{
    [TextArea]
    [SerializeField] string Notes = "Comment Here.";
    #region variables
    int startPosX, startPosY, row, columns;
    GridTiles[,] grid;
    [SerializeField] GridGenerator gridGenerator;
    [SerializeField] Transform player;
    #endregion

    private void Awake()
    {
        gridGenerator = GetComponent<GridGenerator>();
        player = FindObjectOfType<Player>().transform;        
        grid = gridGenerator.grid;
        row = gridGenerator.raws;
        columns = gridGenerator.columns;
    }
    
    public void Initialisation()
    {
        startPosX = (int)player.position.x;
        startPosY = (int)player.position.z;
        
        foreach (GridTiles obj in grid)
        {
            obj.step = -1;
            if (!obj.walkable)
            {
                obj.step = -2;
            }
        }

        grid[startPosX, startPosY].step = 0;

        AssignationChecker();
    }

    void AssignationChecker()
    {
        for(int i = 1; i< row*columns; i++) 
        {
            foreach(GridTiles obj in grid)
            {
                if(obj.step == i-1)
                {
                    TestFourDirection((int)obj.transform.position.x, (int)obj.transform.position.z, (int)obj.height,i);                  
                }
            }
        }
    }

    void TestFourDirection(int x, int y,int height, int step)
    {
        if(TestDirection(x, y, height, -1, 1))       
            SetVisited(x, y+1, step);
         
        if(TestDirection(x, y,height, -1, 2))        
            SetVisited(x+1, y, step);  
        
        if(TestDirection(x, y,height, -1, 3))        
            SetVisited(x, y-1, step); 
        
        if(TestDirection(x, y,height, -1, 4))        
            SetVisited(x-1, y, step);       
    }

    public bool TestDirection(int x, int y,int height, int step, int direction)
    {
        switch (direction)
        {
            case 1:
                if(y+1<row && grid[x,y+1] && grid[x,y+1].step == step && grid[x,y+1].height <= grid[x,y].height+1 && grid[x, y + 1].height >= grid[x, y].height - 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }     
            case 2:
                if(x+1<columns && grid[x+1,y] && grid[x+1,y].step == step && grid[x+1, y].height <= grid[x, y].height + 1 && grid[x+1, y].height >= grid[x, y].height - 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }     
            case 3:
                if(y-1>-1 && grid[x,y-1] && grid[x,y-1].step == step && grid[x, y - 1].height <= grid[x, y].height + 1 && grid[x, y - 1].height >= grid[x, y].height - 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }     
            case 4:
                if(x-1>-1 && grid[x-1,y] && grid[x-1,y].step == step && grid[x-1, y].height <= grid[x, y].height + 1 && grid[x-1, y].height >= grid[x, y].height - 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    void SetVisited(int x, int y, int step)
    {
        if(grid[x, y])
        {
            grid[x,y].step = step;
        }
    }
}
