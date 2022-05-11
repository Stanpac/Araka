using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTiling : MonoBehaviour
{
    [SerializeField] public bool refreshRend = false;
    [SerializeField] public bool refreshRendTempo = false;
    GridTiles[,] grid;
    GridGenerator gridG;
    TileVariables t;
    GridTiles tile;
    GameObject TempoTile;
    public MeshRenderer[] tempoTilesMats = new MeshRenderer[6];

    public Material mat4D;
    public Material mat3D;
    public Material mat2DO;
    public Material mat2DA;
    public Material mat1D;
    public Material mat0D;
    public Material TmatR;
    public Material TmatB1;
    public Material TmatB2;
    public Material TmatG1;
    public Material TmatG2;
    public Material TmatG3;
    public Material Cmat;
    public GameObject colonne1, colonne2, colonne3, colonne4;
    public Material Orange, Purple, Green;
    float lerpMatTimer;
    public MeshRenderer decorMesh;
    public Mesh mesh1D;
    public Mesh mesh2DA;
    public Mesh normalMesh;
    MeshRenderer mesh;


    private void Awake()
    {
        TempoTile = transform.Find("TempoTile").gameObject;
        gridG = FindObjectOfType<GridGenerator>();
        t = FindObjectOfType<TileVariables>();
        grid = gridG.grid;
        tile = GetComponent<GridTiles>();
        mesh = transform.Find("Renderer").GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        TempoDecorMaterial();
        if (/*tile.tempoTile != 0 || */tile.crumble)
        {
            TempoTile.SetActive(true);
        }
        else
        {
            TempoTile.SetActive(false);
        }
        if (tile.walkable  && tile.open && tile.tempoTile == 0)
            SetDirectionalMaterial();
        //transform.Find("Renderer").GetComponent<MeshFilter>().mesh = meshF;
    }

    private void OnDrawGizmos()
    {
        TempoTile = transform.Find("TempoTile").gameObject;
        tile = GetComponent<GridTiles>();
        t = FindObjectOfType<TileVariables>();


        if (tile.tempoTile != 0 || tile.crumble)
        {
            TempoTile.SetActive(true);
        }
        else 
        {
            TempoTile.SetActive(false);
        }

        if (tile.walkable && refreshRend)
        {
            gridG = FindObjectOfType<GridGenerator>();
            gridG.generateGrid();
            grid = gridG.grid;
            mesh = transform.Find("Renderer").GetComponent<MeshRenderer>();
            //meshF = transform.Find("Renderer").GetComponent<MeshFilter>().mesh;
            if(tile.tempoTile == 0)
                SetDirectionalMaterial();

            TempoDecorMaterial();
            //transform.Find("Renderer").GetComponent<MeshFilter>().mesh = meshF;
        }
        else
            refreshRend = false;

        if (tile.walkable && (tile.tempoTile != 0 || tile.crumble)  && refreshRendTempo)
        {
            //mesh = transform.Find("Renderer").GetComponent<MeshRenderer>();
            TempoTileMaterial();
            TempoDecorMaterial();
        }
        else
            refreshRendTempo = false;
        //mesh.transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        //TempoTileMaterial();
    }


    public void TempoDecorMaterial()
    {
        if (tile.tempoTile == 1)
        {
            decorMesh.material = Orange;
        }

        if (tile.tempoTile == 2)
        {
            decorMesh.material = Purple;
        }

        if (tile.tempoTile == 3)
        {
            decorMesh.material = Green;
        }
    }

    void MaterialLerping(Material previousMat, Material mat)
    {
        print(1);
        lerpMatTimer += Time.deltaTime * 2;
        mesh.material.Lerp(previousMat, mat, lerpMatTimer);
        if(lerpMatTimer < 1)
        {
            MaterialLerping(previousMat, mat);
        }
        else
        {
            lerpMatTimer = 0;
        }

    }


    public void TempoTileMaterial()
    {
        if (tile.tempoTile != 0 || tile.crumble)
        {
            if (tile.crumble)
            {
                foreach (MeshRenderer m in tempoTilesMats)
                {
                    m.material = Cmat;
                    //m.material.Lerp(m.material,Cmat,Time.deltaTime);
                    SetCubeSize();
                    AllColonneActivate();
                    refreshRendTempo = false;
                }
            }

            if (tile.tempoTile == 1)
            {
                //mesh.material = TmatR;
                Material curM = mesh.material;
                MaterialLerping(TmatR, TmatR);
                // mesh.material.Lerp(mesh.material,TmatR,Time.deltaTime*2);
                SetCubeSize();
                AllColonneActivate();
                refreshRendTempo = false;
            }


            if (tile.tempoTile == 2)
            {
                if (t.blueTimer == 0 || t.blueTimer == 2)
                {
                    //MaterialLerping(TmatB1, TmatB2);
                    mesh.material = TmatB2;
                    //mesh.material.Lerp(mesh.material, TmatB2, Time.deltaTime * 2);
                    SetCubeSize();
                    AllColonneActivate();
                    refreshRendTempo = false;
                    /*foreach (MeshRenderer m in tempoTilesMats)
                    {
                        m.material = TmatB2;
                        refreshRendTempo = false;
                    }*/
                }
                else if (t.blueTimer == 1)
                {
                    //MaterialLerping(TmatB2, TmatB1);
                    mesh.material = TmatB1;
                    //mesh.material.Lerp(mesh.material, TmatB1, Time.deltaTime * 2);
                    SetCubeSize();
                    AllColonneActivate();
                    refreshRendTempo = false;
                    /* foreach (MeshRenderer m in tempoTilesMats)
                     {
                         m.material = TmatB1;
                         refreshRendTempo = false;
                     }*/
                }
                refreshRendTempo = false;
            }

            if (tile.tempoTile == 3)
            {
                if (t.greenTimer == 0 || t.greenTimer == 3)
                {
                    //MaterialLerping(TmatG2, TmatG3);
                    mesh.material = TmatG3;
                    //mesh.material.Lerp(mesh.material, TmatG3, Time.deltaTime * 2);
                    SetCubeSize();
                    AllColonneActivate();
                    refreshRendTempo = false;
                    /*foreach (MeshRenderer m in tempoTilesMats)
                    {
                        m.material = TmatG3;
                        refreshRendTempo = false;
                    }*/
                }
                else if ((t.greenTimer == 1 && t.greenFlag) || (t.greenTimer == 2 && !t.greenFlag))
                {
                    //MaterialLerping(TmatG1, TmatG2);
                    //mesh.material.Lerp(mesh.material, TmatG2, Time.deltaTime * 2);
                    mesh.material = TmatG2;
                    SetCubeSize();
                    AllColonneActivate();
                    refreshRendTempo = false;
                    /*foreach (MeshRenderer m in tempoTilesMats)
                    {
                        m.material = TmatG1;
                        refreshRendTempo = false;
                    }*/
                }
                else if ((t.greenTimer == 2 && t.greenFlag) || (t.greenTimer == 1 && !t.greenFlag))
                {
                    //MaterialLerping(TmatG3, TmatG1);
                    //mesh.material.Lerp(mesh.material, TmatG1, Time.deltaTime * 2);
                    mesh.material = TmatG1;
                    SetCubeSize();
                    AllColonneActivate();
                    refreshRendTempo = false;
                    /* foreach (MeshRenderer m in tempoTilesMats)
                     {
                         m.material = TmatG2;
                         refreshRendTempo = false;
                     }*/
                }
            }
        }
    }

    public void SetDirectionalMaterial()
    {
        if (tile.tempoTile != 0)
            refreshRend = false;
        //4 directions
        if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
            gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
            gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
            gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat4D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 90,0);
            SetCubeSize();
            AllColonneActivate();

        }

        //3 directions
        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 0, 0);
            SetCubeSize();
            AllColonneActivate();

        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 180, 0);
            SetCubeSize();
            AllColonneActivate();
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 90, 0);
            SetCubeSize();
            AllColonneActivate();
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat3D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, -90, 0);
            SetCubeSize();
            AllColonneActivate();
        }

        //2 directions oppos�es

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DO;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 180, 0);
            SetCubeSize();
            AllColonneActivate();
        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DO;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, -90, 0);
            SetCubeSize();
            AllColonneActivate();
        }

        //2 directions adjacentes

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 0, 0);
            SetCubeSize();
            AllColonneActivate();


        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 90, 0);
            SetCubeSize();
            AllColonneActivate();
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 180, 0);
            SetCubeSize();
            AllColonneActivate();
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat2DA;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, -90, 0);
            SetCubeSize();
            AllColonneActivate();
        }

        //1 direction

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 0, 0);
            SetCubeSize();
            AllColonneActivate();

        }

        else if (gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            //meshF = mesh1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 90, 0);
            SetCubeSize();
            AllColonneActivate();



        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, 180, 0);           
            SetCubeSize();
            AllColonneActivate();
        }

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                  gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat1D;
            refreshRend = false;
            mesh.transform.rotation = Quaternion.identity;
            mesh.transform.Rotate(-90, -90, 0);
            SetCubeSize();
            AllColonneActivate();


        }

        //0 direction

        else if (!gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 1) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 2) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 3) &&
                 !gridG.TestDirection((int)transform.position.x, (int)transform.position.z, 4))
        {
            mesh.material = mat0D;
            refreshRend = false;
            Quaternion rot = mesh.transform.rotation ;
            rot.eulerAngles = new Vector3(-90,0,0);
            mesh.transform.rotation = rot;
            SetCubeSize();
            AllColonneActivate();
        }
    }

    void ColonneActive(GameObject colonne)
    {
        if(colonne.transform.localScale.y == 0)
            colonne.SetActive(false);
        else
            colonne.SetActive(true);
    }

    public void AllColonneActivate()
    {
        ColonneActive(colonne1);
        ColonneActive(colonne2);
        ColonneActive(colonne3);
        ColonneActive(colonne4);
    }

    void SetSingleCubeSize(Transform colonne, float hDiff1, float hDiff2)
    {
        colonne.localPosition = new Vector3(colonne.localPosition.x, .5f, colonne.localPosition.z);

        if(hDiff1 > hDiff2)
        {
            colonne.localScale = new Vector3(colonne.localScale.x, hDiff2 * 0.4f, colonne.localScale.z);
            colonne.localPosition -=  new Vector3(0, hDiff2 * 0.2f, 0); 
        }
        else
        {
            colonne.localScale = new Vector3(colonne.localScale.x, hDiff1 * 0.4f, colonne.localScale.z);
            colonne.localPosition -= new Vector3(0, hDiff1 * 0.2f, 0);
        }
    }

    public void SetCubeSize()
    {
        SetSingleCubeSize(colonne1.transform, tile.HeightDiffU, tile.HeightDiffR);
        SetSingleCubeSize(colonne2.transform, tile.HeightDiffD, tile.HeightDiffR);
        SetSingleCubeSize(colonne3.transform, tile.HeightDiffU, tile.HeightDiffL);
        SetSingleCubeSize(colonne4.transform, tile.HeightDiffD, tile.HeightDiffL);
    }


    /*print(grid[(int)transform.position.x, (int)transform.position.z].HeightDiffR);
print(grid[(int)transform.position.x, (int)transform.position.z].HeightDiffL);
print(grid[(int)transform.position.x, (int)transform.position.z].HeightDiffU);
print(grid[(int)transform.position.x, (int)transform.position.z].HeightDiffD);*/
}
