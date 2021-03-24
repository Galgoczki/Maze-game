using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    private bool[,,] maze;
    private bool[,] boneMap;
    private int[,] aStarMap;
    private GameObject player;
    private float wait;
    private int playeri;
    private int playerj;
    /*
    / end->|---|
    /      |   | x tendely i
    /      |---|<start
    /        z tendely     j
    /--------------
    /         2
    /      |----|
    /     3|cell| 1
    /      |----|
    /         0
    /   true -> fal van
    /   false -> nincs fal-> ha false akkor mehet arra a kutyus
    */
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");

        wait= 100f;
    }

    // Update is called once per frame
    void Update()
    {   
        if(wait>0)
        wait -= Time.deltaTime;
        if(wait<=0){


        }
    }

    int move(){//0 1 2 3
        return 0;
    }

    public void setMaze(bool[,,] basemaze,int x_axis_size,int z_axis_size){
        maze=basemaze;
        aStarMap = new int[x_axis_size,z_axis_size];
        boneMap = new bool[x_axis_size,z_axis_size];
        for (int i = 0; i < x_axis_size; i++){
            for (int j = 0; j < z_axis_size; j++){
                aStarMap[i,j]=0;// down
                aStarMap[i,j]=0;// right
                aStarMap[i,j]=0;// top
                aStarMap[i,j]=0;// left
                
                boneMap[i,j]=false;// down
                boneMap[i,j]=false;// right
                boneMap[i,j]=false;// top
                boneMap[i,j]=false;// left
            }
        }
        Debug.Log("booo már tudok tájékozodni");
    }
    public void setPlayerPosition(int i,int j){
        playeri=i;
        playerj=j;
    }


    public void stop_wait_a_minute(){
        Debug.Log("Fill my cup, put some liquor in it \nTake a sip, sign a check\n...");
        Destroy(this.gameObject);
    }
    public void say_hi(int a){
        Debug.Log("hi im a sphere...i mean dog. my mane is" + a);
    }
}
