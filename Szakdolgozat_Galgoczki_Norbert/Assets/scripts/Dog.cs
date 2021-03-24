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
    private int dogi;
    private int dogj;
    private int sizex;
    private int sizez;
    private List<Vector2> mypath;
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

    public void setMaze(bool[,,] basemaze ,int x_axis_size ,int z_axis_size, int i, int j, int myi, int myj){
        maze=basemaze;
        playeri=i;
        playerj=j;
        dogi=myi;
        dogj=myj;
        sizex=x_axis_size;
        sizez=z_axis_size;
        aStarMap = new int[x_axis_size,z_axis_size];
        boneMap = new bool[x_axis_size,z_axis_size];
        for (int i_tomb_init = 0; i_tomb_init < x_axis_size; i_tomb_init++){
            for (int j_tomb_init = 0; j_tomb_init < z_axis_size; j_tomb_init++){
                aStarMap[i,j]=0;
                boneMap[i,j]=false;
            }
        }
        aStarMap[dogi,dogj]=(int)Mathf.Floor(Mathf.Sqrt(Mathf.Pow(playeri-dogi,2)+Mathf.Pow(playerj-dogi,2)));
        Debug.Log("booo már tudok tájékozodni");
    }

    private void player_moved(){
        if(dogi!=-1 && dogj!=-1){
            aStarGenerat();
            mypath = optimalpath();
        }
    }

    private List<Vector2> optimalpath(){
        //todo:játékostol a kutyáig majd valahogy megfordit és ezen halad a kutya mikor követi a játékost
        return new List<Vector2>();
    }

    private void aStarGenerat(){
        bool vege = false;
        while(!vege){
            vege=true;
            for (int i = 0; i < sizex; i++){
                for (int j = 0; j < sizez; j++){
                    if(aStarMap[i,j]==0){
                        vege=false;
                    }else{
                        if(maze[i,j,0]  &&  aStarMap[i-1,j  ]==0){
                            aStarMap[i-1,j  ]  =  aStarMap[i,j]+1+(int)Mathf.Floor(Mathf.Sqrt(Mathf.Pow(playeri-i,2)+Mathf.Pow(playerj-j,2)));//a^ˇ=b^2+c^2 
                        }
                        if(maze[i,j,1]  &&  aStarMap[i  ,j-1]==0){
                            aStarMap[i  ,j-1]  =  aStarMap[i,j]+1+(int)Mathf.Floor(Mathf.Sqrt(Mathf.Pow(playeri-i,2)+Mathf.Pow(playerj-j,2)));
                        }
                        if(maze[i,j,2]  &&  aStarMap[i+1,j  ]==0){
                            aStarMap[i+1,j  ]  =  aStarMap[i,j]+1+(int)Mathf.Floor(Mathf.Sqrt(Mathf.Pow(playeri-i,2)+Mathf.Pow(playerj-j,2)));
                        }
                        if(maze[i,j,3]  &&  aStarMap[i  ,j+1]==0){
                            aStarMap[i  ,j+1]  =  aStarMap[i,j]+1+(int)Mathf.Floor(Mathf.Sqrt(Mathf.Pow(playeri-i,2)+Mathf.Pow(playerj-j,2)));
                        }

                    }
                }
            }
        }   
    }
    public void setPlayerPosition(int i,int j){
        playeri=i;
        playerj=j;
        Debug.Log("kutya tudja hol vagy:"+playeri+playerj);
        player_moved();
    }


    public void stop_wait_a_minute(){
        Debug.Log("Fill my cup, put some liquor in it \nTake a sip, sign a check\n...");
        //ideiglenes ez az egész fügvény
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void say_hi(int a){
        Debug.Log("hi im a sphere...i mean dog. my mane is" + a);
    }
}
