using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    private bool[,,] maze;
    private bool[,] doorMap;
    private Vector3 mazeZeroZero;
    private Vector3 target;
    private Vector3 dogposition;
    private bool[,] boneMap;
    private int[,] aiMap;
    private int[,] preMap;//not optimal if i use a row+1 long array its tottaly could be better  
    private GameObject player;
    private float wait;
    private int playeri;
    private int playerj;
    public int dogi;
    public int dogj;
    private int sizex;
    private int sizez;
    private List<Vector2> mypath;
    private float fullcellsize = 7;
    private int faceing=0;//testing the muvement
    private int turning=0;
    private Transform turningVector;
    private Transform targetTurningVector;
    private float timeToTarget=2f;
    private float turningTime=1f;
    private float turningspeed=0.5f;
    private bool animeting = true;
    private Animator animator;
    private bool bloodeye=false;
    private Light[] eyes; 
    private SphereCollider kill_zone;
    
    /*
                            dogposition.eulerAngles.y
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
        Transform temperary = this.gameObject.transform.GetChild(0);
        animator = temperary.gameObject.GetComponent<Animator>();
        wait= 10f;
        eyes = GetComponentsInChildren<Light>();
        kill_zone = GetComponent<SphereCollider>();
    }

    private bool looking(int i,int j,int faceing){
        int index=0;
        //Debug.Log("keres és itt vagy"+playeri+" "+playerj);
        switch (faceing){   
            case 0:
                //dogi-=1;
                while(true){
                    if(i-index==playeri &&j==playerj){//player itt van akkor erre amúgy tovább elöre nézünk
                        //bloodeye=true;
                        return true;
                    }

                    if(!maze[i-index,j,faceing]){//nincs fal
                        index++;
                    }else{//nem lehet tovább menni akkor kilépünk
                        return false;
                    }
                }
                break;
            case 1:
                //dogj-=1;
                while(true){
                    if(i==playeri &&j-index==playerj){//player itt van akkor erre amúgy tovább elöre nézünk
                        //bloodeye=true;
                        return true;

                    }

                    if(!maze[i,j-index,faceing]){//nincs fal
                        index++;
                    }else{//nem lehet tovább menni akkor kilépünk
                        return false;
                    }
                    
                }
                break;
            case 2:
                //dogi+=1;
                while(true){
                    if(i+index==playeri &&j==playerj){//player itt van akkor erre amúgy tovább elöre nézünk
                        //bloodeye=true;
                        return true;

                    }

                    if(!maze[i+index,j,faceing]){//nincs fal
                        index++;
                    }else{//nem lehet tovább menni akkor kilépünk
                        return false;
                    }
                    
                }
                break;
            case 3:
                //dogj+=1;
                while(true){
                    if(i==playeri &&j+index==playerj){//player itt van akkor erre amúgy tovább elöre nézünk
                        //bloodeye=true;
                        return true;
                        
                    }

                    if(!maze[i,j+index,faceing]){//nincs fal
                        index++;
                    }else{//nem lehet tovább menni akkor kilépünk
                        return false;
                    }

                }
                break;
            default:
                break;
        }
        return false;
    }
    // Update is called once per frame
    void Update(){ 
        //Debug.DrawLine(transform.position,transform.position+transform.forward,Color.red,0.3f);
        if(bloodeye){
            eyes[0].enabled=true;
            eyes[1].enabled=true;
            Global_options_handler.lightoff=true;
        }else{
            eyes[0].enabled=false;
            eyes[1].enabled=false;
            Global_options_handler.lightoff=false;
        }

        if(maze!=null&& wait<=0){ 
            if(!animeting){
                animeting=true;
                animator.enabled = true;
            }
            wait = 0;
            if(turningTime<1){

                turningTime += Time.deltaTime/turningspeed;
                transform.rotation = Quaternion.Lerp(turningVector.rotation, Quaternion.Euler (turningVector.rotation.x, turningVector.rotation.y-turning, turningVector.rotation.z) , turningTime);
                kill_zone.radius = 4f;

            }else{
                kill_zone.radius = 3f;
                turningVector=transform;
                if(timeToTarget<1){
                    if(looking(dogi,dogj,(faceing+2)%4)){
                        bloodeye=true;
                        timeToTarget += Time.deltaTime/0.5f;
                    }else{
                        bloodeye=false;
                        timeToTarget += Time.deltaTime/7;
                    }
                    
                    transform.position = Vector3.Lerp(dogposition, target, timeToTarget);
                }
                if(timeToTarget>=1){//update the target
                    bloodeye=false;
                    int optimalface=4;
                    //int optimalface_index=10;
                    bool best_right =false;
                    for (int index = 0; index < 4; index++){
                        if(!maze[dogi,dogj,((faceing+1+index)%4)]){//mindig jobbra szabály/right-hand rule
                                if(!best_right){
                                    optimalface=((faceing+1+index)%4);
                                    best_right=true;
                                    turning=((faceing+2+index)%4)*90;
                                }
                                if(looking(dogi,dogj,((faceing+1+index)%4))){
                                    bloodeye=true;
                                    optimalface=((faceing+1+index)%4);
                                    best_right=true;
                                    turning=((faceing+2+index)%4)*90;
                                    break;
                                }

                        }
                    }
                    faceing=optimalface;
                    switch (faceing){   
                        case 0:
                            dogposition = mazeZeroZero +new Vector3(dogi*fullcellsize,0,dogj*fullcellsize);
                            dogi-=1;
                            target = mazeZeroZero +new Vector3(dogi*fullcellsize,0,dogj*fullcellsize);
                            timeToTarget=0;
                            break;
                        case 1:
                            dogposition = mazeZeroZero +new Vector3(dogi*fullcellsize,0,dogj*fullcellsize);
                            dogj-=1;
                            target = mazeZeroZero +new Vector3(dogi*fullcellsize,0,dogj*fullcellsize);
                            timeToTarget=0;
                            break;
                        case 2:
                            dogposition = mazeZeroZero +new Vector3(dogi*fullcellsize,0,dogj*fullcellsize);
                            dogi+=1;
                            target = mazeZeroZero +new Vector3(dogi*fullcellsize,0,dogj*fullcellsize);
                            timeToTarget=0;
                            break;
                        case 3:
                            dogposition = mazeZeroZero +new Vector3(dogi*fullcellsize,0,dogj*fullcellsize);
                            dogj+=1;
                            target = mazeZeroZero +new Vector3(dogi*fullcellsize,0,dogj*fullcellsize);
                            timeToTarget=0;
                            break;
                        default:
                            break;
                    }
                    faceing=(faceing+2)%4;
                    turningTime=(bloodeye?0.5f:0);
                }
            }
        }else{
            if(animeting){
                animeting=false;
                animator.enabled = false;
            }
            wait-=Time.deltaTime;
        }
    }

    public void setMaze(bool[,,] basemaze ,int x_axis_size ,int z_axis_size, int i, int j, int myi, int myj,Vector3 mapzero){
        maze=basemaze;
        mazeZeroZero=mapzero;//its the positin where the first cell of the maze have and its midel
        playeri=i;
        playerj=j;
        dogi=myi;
        dogj=myj;
        sizex=x_axis_size;
        sizez=z_axis_size;
        aiMap = new int[x_axis_size,z_axis_size];
        preMap = new int[x_axis_size,z_axis_size];
        boneMap = new bool[x_axis_size,z_axis_size];
        for (int i_tomb_init = 0; i_tomb_init < x_axis_size; i_tomb_init++){
            for (int j_tomb_init = 0; j_tomb_init < z_axis_size; j_tomb_init++){
                aiMap[i,j]=0;
                preMap[i,j]=0;
                boneMap[i,j]=false;
            }
        }
        aiMap[dogi,dogj]=(int)Mathf.Floor(Mathf.Sqrt(Mathf.Pow(playeri-dogi,2)+Mathf.Pow(playerj-dogi,2)));
        preMap[dogi,dogj]=(int)Mathf.Floor(Mathf.Sqrt(Mathf.Pow(playeri-dogi,2)+Mathf.Pow(playerj-dogi,2)));
        //Debug.Log("booo már tudok tájékozodni");
    }

    private void player_moved(){
        if(dogi!=-1 && dogj!=-1){
            aiMapGenerat();
            mypath = optimalpath();
        }
    }

    private List<Vector2> optimalpath(){
        //todo:játékostol a kutyáig majd valahogy megfordit és ezen halad a kutya mikor követi a játékost

        return new List<Vector2>();
    }
    private List<Vector2> rekurziv_way_from_the_player_to_the_dog(int pre_i,int pre_j,int now_i,int now_j){
        //int best_way;
        if(maze[now_i,now_j,0]  &&  pre_i!=now_i-1 &&pre_j!=now_j){
           
        }
        if(maze[now_i,now_j,1]  &&  pre_i!=now_i &&pre_j!=now_j-1){
            
        }
        if(maze[now_i,now_j,2]  &&  pre_i!=now_i+1 &&pre_j!=now_j){
            
        }
        if(maze[now_i,now_j,3]  &&  pre_i!=now_i &&pre_j!=now_j+1){
            
        }
        return new List<Vector2>();
    }
    private void aiMapGenerat(){
        bool nosolution = false;
        bool vege = false;
        while(!vege&&!nosolution){
            vege=true;
            nosolution=true;
            for (int i = 0; i < sizex; i++){
                for (int j = 0; j < sizez; j++){
                    if(aiMap[i,j]==0){
                        vege=false;
                    }else{
                        if(i>0 && maze[i,j,0]  &&  preMap[i-1,j  ]==0){
                            aiMap[i-1,j  ]  =  preMap[i,j]+1+(int)Mathf.Floor(Mathf.Sqrt(Mathf.Pow(playeri-i,2)+Mathf.Pow(playerj-j,2)));//a^ˇ=b^2+c^2 
                            nosolution=false;
                        }
                        if(j>0&& maze[i,j,1]  &&  preMap[i  ,j-1]==0){
                            aiMap[i  ,j-1]  =  preMap[i,j]+1+(int)Mathf.Floor(Mathf.Sqrt(Mathf.Pow(playeri-i,2)+Mathf.Pow(playerj-j,2)));
                            nosolution=false;
                        }
                        if(i<sizex-1 &&maze[i,j,2]  &&  preMap[i+1,j  ]==0){
                            aiMap[i+1,j  ]  =  preMap[i,j]+1+(int)Mathf.Floor(Mathf.Sqrt(Mathf.Pow(playeri-i,2)+Mathf.Pow(playerj-j,2)));
                            nosolution=false;
                        }
                        if(j<sizez-1 &&maze[i,j,3]  &&  preMap[i  ,j+1]==0){
                            aiMap[i  ,j+1]  =  preMap[i,j]+1+(int)Mathf.Floor(Mathf.Sqrt(Mathf.Pow(playeri-i,2)+Mathf.Pow(playerj-j,2)));
                            nosolution=false;
                        }
                    }
                }
            }
        } 
        for (int i = 0; i < sizex; i++){
            for (int j = 0; j < sizez; j++){
                preMap[i,j]=aiMap[i,j];
            }
        } 
    }
    public void setPlayerPosition(int i,int j){
        playeri=i;
        playerj=j;
        //Debug.Log("kutya tudja hol vagy:"+playeri+playerj);
        //player_moved();
    }


    public void stop_wait_a_minute(){
        //Debug.Log("Fill my cup, put some liquor in it \nTake a sip, sign a check\n...");//music reference
        //ideiglenes ez az egész fügvény
        //Destroy(this.gameObject);
        //this.gameObject.SetActive(false);
        wait=15f;
        bloodeye=false;
    }

    public void say_hi(int a){
        //Debug.Log("hi im a sphere...i mean dog. my mane is" + a);
    }
    public bool isDangerous(){
        return (wait<=0);
    }
}
