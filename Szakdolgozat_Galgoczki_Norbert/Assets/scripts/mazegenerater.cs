using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mazegenerater : MonoBehaviour
{
    public System.Random rnd = new System.Random();
    public GameObject floor;
    public GameObject roof;
    public GameObject wall;
    public GameObject floors;
    public GameObject roofs;
    public GameObject walls;
    public bool[,,] basemaze;
    //    _1_default
    // 0 |   | 2       its the cells wall numbers
    //   |_3_|
    // default pos of wall --
    public int rows;
    public int columns;
    public float size = 6;//size of the maze's cells
    private float fullcellsize = 7;//cell+wallssize
    private Vector3 starterpoint = new Vector3(20,0,20);
    // Start is called before the first frame update
    void Start()
    {
        rows    = rnd.Next(3,15);
        columns = rnd.Next(3,15);

        basemaze = new bool[rows,columns,4];
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < columns; j++){
                basemaze[i,j,0]=true;// left
                basemaze[i,j,1]=true;// top
                basemaze[i,j,2]=true;// right
                basemaze[i,j,3]=true;// down
            }
        }

        //basemaze = generat();

        for (int i = 0; i < rows; i++){//x
            for (int j = 0; j < columns; j++){//y
                //floor
                Instantiate(floor,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2,starterpoint.y-0.5f,starterpoint.z+(j*fullcellsize)-fullcellsize/2),Quaternion.identity,floors.transform);
                //roof
                Instantiate(roof,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2,starterpoint.y+5.5f,starterpoint.z+(j*fullcellsize)-fullcellsize/2),Quaternion.identity,roofs.transform);
                //walls 
                if(basemaze[i,j,0]){// left
                    Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-3.25f,starterpoint.y+2.5f,starterpoint.z+(j*fullcellsize)-fullcellsize/2),Quaternion.Euler(0f,270f,0f),walls.transform); 
                }
                if(basemaze[i,j,1]){// top
                    Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2,starterpoint.y+2.5f,starterpoint.z+(j*fullcellsize)-fullcellsize/2-3.25f),Quaternion.Euler(0f,0f,0f),walls.transform);
                }
                if(basemaze[i,j,2]){// right
                    Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+3.25f,starterpoint.y+2.5f,starterpoint.z+(j*fullcellsize)-fullcellsize/2),Quaternion.Euler(0f,90f,0f),walls.transform);
                }
                if(basemaze[i,j,3]){// down
                    Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2,starterpoint.y+2.5f,starterpoint.z+(j*fullcellsize)-fullcellsize/2+3.25f),Quaternion.Euler(0f,180f,0f),walls.transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
