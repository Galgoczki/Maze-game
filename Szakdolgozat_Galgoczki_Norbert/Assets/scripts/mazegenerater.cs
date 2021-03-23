using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mazegenerater : MonoBehaviour
{
    private GameObject chair;
    private GameObject book;
    private GameObject bookshelf;
    private GameObject desk;
    private GameObject card;
    private GameObject torch;
    private GameObject table_candle;
    private GameObject enemyfolder;


    private GameObject dogGO;
    private Dog dogscript;
    [SerializeField]private Transform playersTransforms;
    public string key;//where we start the generat;how deep is the FILO go;where is the player start ;wher the filo is step;
    [SerializeField]private Vector3 playerStarterpont;
    [SerializeField]private bool keyalreadygiven = false;
    private System.Random rnd = new System.Random();
    private GameObject floor;
    private GameObject roof;
    private GameObject roof_room;
    private GameObject wall;
    private GameObject wall_whitout_door;
    private GameObject wall_whit_torch;
    private GameObject floors;
    private GameObject roofs;
    private GameObject walls;
    private GameObject door_frame;
    private GameObject door_inner;
    private bool[,,] basemaze;
    private bool[,,] doorMap;
    private int[,] roomMap;
    private float [,] roomsmid;
    //    _2_default
    // 3 |   | 1       its the cells wall numbers
    //   |_0_|
    // default pos of wall --
    //x_axis_size;z_axis_size
    [SerializeField]private int x_axis_size;
    [SerializeField]private int z_axis_size;
    [SerializeField]private float size = 6;//size of the maze's cells
    public short[,] generateLeft;
    private float fullcellsize = 7;//cell+wallssize
    private Vector3 starterpoint = new Vector3(20,0,20);//the mazes starter corner
    private bool generateDone = false;
    private int[] algorimusFilo;
    private int positionInTheFiloArray = 0;

    private int endi=0;
    private int endj=0;
    // Start is called before the first frame update
    void Start(){


        dogGO = GameObject.Find("Dog");
        floors = GameObject.Find("floors");
        roofs = GameObject.Find("roofs");
        walls = GameObject.Find("walls");
        enemyfolder = GameObject.Find("enemyfolder");
        chair = (GameObject)Resources.Load("prefab/chair", typeof(GameObject));
        book = (GameObject)Resources.Load("prefab/book", typeof(GameObject));
        bookshelf = (GameObject)Resources.Load("prefab/bookshelf", typeof(GameObject));
        desk = (GameObject)Resources.Load("prefab/desk", typeof(GameObject));
        card = (GameObject)Resources.Load("prefab/card", typeof(GameObject));
        wall_whitout_door = (GameObject)Resources.Load("prefab/wall_whitout_door", typeof(GameObject));
        wall_whit_torch = (GameObject)Resources.Load("prefab/wall_blender_torch", typeof(GameObject));
        wall = (GameObject)Resources.Load("prefab/wall_blender", typeof(GameObject));
        floor = (GameObject)Resources.Load("prefab/floor", typeof(GameObject));
        roof_room = (GameObject)Resources.Load("prefab/roof_room", typeof(GameObject));
        roof = (GameObject)Resources.Load("prefab/roof", typeof(GameObject));
        table_candle = (GameObject)Resources.Load("prefab/table_candle", typeof(GameObject));
        door_frame = (GameObject)Resources.Load("prefab/door_frame", typeof(GameObject));
        door_inner = (GameObject)Resources.Load("prefab/door", typeof(GameObject));
        
        //dogscript = dogGO.GetComponent<Dog>();


        x_axis_size = rnd.Next(10,40);
        z_axis_size = rnd.Next(10,40);

        basemaze = new bool[x_axis_size,z_axis_size,4];
        doorMap = new bool[x_axis_size,z_axis_size,4];
        endi=x_axis_size-1;
        endj=z_axis_size-1;
        generateLeft = new short[x_axis_size,z_axis_size];
        for (int i = 0; i < x_axis_size; i++){
            for (int j = 0; j < z_axis_size; j++){
                basemaze[i,j,0]=true;// down
                basemaze[i,j,1]=true;// right
                basemaze[i,j,2]=true;// top
                basemaze[i,j,3]=true;// left
                
                doorMap[i,j,0]=false;// down
                doorMap[i,j,1]=false;// right
                doorMap[i,j,2]=false;// top
                doorMap[i,j,3]=false;// left
                generateLeft[i,j]=0;
            }
        }
        if(!keyalreadygiven){
            generatTheKey();
        }

        generateRoom();
        int roomcounter=0;
        generate();

        Instantiate(dogGO,new Vector3(starterpoint.x+(0*fullcellsize)+fullcellsize/2,               starterpoint.y+1.5f,    starterpoint.z+((z_axis_size-1)*fullcellsize)-fullcellsize/2) ,Quaternion.identity,enemyfolder.transform);
        Instantiate(dogGO,new Vector3(starterpoint.x+((x_axis_size-1)*fullcellsize)+fullcellsize/2,     starterpoint.y+1.5f,    starterpoint.z+(0*fullcellsize)-fullcellsize/2)           ,Quaternion.identity,enemyfolder.transform);

        for (int i = 0; i < x_axis_size; i++){//x
            int offseti = i%2;
            for (int j = 0; j < z_axis_size; j++){//y
                int offsetj = j%2;
                //floor
                bool light= false;
                Instantiate(floor,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2,starterpoint.y-0.5f,starterpoint.z+(j*fullcellsize)-fullcellsize/2),Quaternion.identity,floors.transform);
                if(roomMap[i,j]!=0){//its a room
                    if((offseti==1 || offsetj==1 )&&(offseti!=offsetj)){
                        Instantiate(roof_room,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2,starterpoint.y+5.5f,starterpoint.z+(j*fullcellsize)-fullcellsize/2),Quaternion.identity,roofs.transform);
                    }else{
                        Instantiate(roof,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2,starterpoint.y+5.5f,starterpoint.z+(j*fullcellsize)-fullcellsize/2),Quaternion.identity,roofs.transform);
                    }
                   
                    if(basemaze[i,j,0]){// left - down
                        Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-3.25f       ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2)            ,Quaternion.Euler(-90f,270f,0f),walls.transform); 
                        if(!basemaze[i,j,1]){
                            Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-3.25f+1f       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-1.5f)            ,Quaternion.Euler(-90f,270f,0f),walls.transform); 
                        }else{
                            Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-3.25f+2f       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-1f)            ,Quaternion.Euler(-90f,-135f,0f),walls.transform);//-
                        }
                        if(basemaze[i,j,3]){
                            //HÁRMAS DOLGA AZ A SZEKRÉNY   capsry
                        }else{
                            Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-3.25f+1f       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+1.5f)            ,Quaternion.Euler(-90f,270f,0f),walls.transform); 
                        }
                        
                    }
                    if(basemaze[i,j,1]){// top - right
                        Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2             ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-3.25f)      ,Quaternion.Euler(-90f,180f,0f),walls.transform);
                        if(!basemaze[i,j,2]){
                            Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+1.5f       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-3.25f+1f)            ,Quaternion.Euler(-90f,180f,0f),walls.transform); 
                       
                        }else{
                            Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+1f       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-3.25f+2f)            ,Quaternion.Euler(-90f,135,0f),walls.transform); //-
                       
                        }
                        if(basemaze[i,j,0]){
                            //NULLLÁS DOLGA AZ A SZEKRÉNY   capsry
                        }else{
                           Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-1.5f       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-3.25f+1f)            ,Quaternion.Euler(-90f,180f,0f),walls.transform); 
                         
                        }
                        
                    }
                    if(basemaze[i,j,2]){// right -top
                        Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+3.25f       ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2)            ,Quaternion.Euler(-90f,90f,0f),walls.transform);
                        if(!basemaze[i,j,3]){
                            Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+3.25f-1f       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+1.5f)            ,Quaternion.Euler(-90f,90f,0f),walls.transform); 
                    
                        }else{
                            Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+3.25f-2F       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+1f)            ,Quaternion.Euler(-90f,45f,0f),walls.transform); //-
                    
                        }
                        if(basemaze[i,j,1]){
                            //EGYES DOLGA AZ A SZEKRÉNY   capsry
                        }else{
                           Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+3.25f-1f       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-1.5f)            ,Quaternion.Euler(-90f,90f,0f),walls.transform); 
                        
                        }
                        
                    }
                    if(basemaze[i,j,3]){// down - left
                        Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2             ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+3.25f)      ,Quaternion.Euler(-90f,180f,0f),walls.transform);
                        if(!basemaze[i,j,0]){
                            Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-1.5f       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+3.25f-1f)            ,Quaternion.Euler(-90f,0f,0f),walls.transform); 
                        
                        }else{
                            Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-1       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+3.25f-2.1f)            ,Quaternion.Euler(-90f,-45f,0f),walls.transform); //-
                        
                        }
                        if(basemaze[i,j,2]){
                            //KETTES DOLGA AZ A SZEKRÉNY   capsry
                        }else{
                          Instantiate(bookshelf,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+1.5f       ,starterpoint.y+2f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+3.25f-1f)            ,Quaternion.Euler(-90f,0f,0f),walls.transform); 
                     
                        }
                        
                    }
                    if(doorMap[i,j,0]){
                        Instantiate(wall_whitout_door,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-3.5f       ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2)            ,Quaternion.Euler(-90f,270f,0f),walls.transform); 
                        Instantiate(door_frame,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-3.5f       ,starterpoint.y+1.35f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2)            ,Quaternion.Euler(-90f,270f,0f),walls.transform); 
                        Instantiate(door_inner,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-3.6f       ,starterpoint.y+1.23f                ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-0.06f)            ,Quaternion.Euler(-90f,270f,0f),walls.transform); 
                        
                    }
                    if(doorMap[i,j,1]){
                        Instantiate(wall_whitout_door,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2             ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-3.5f)      ,Quaternion.Euler(-90f,180f,0f),walls.transform);
                        Instantiate(door_frame,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2             ,starterpoint.y+1.35f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-3.5f)      ,Quaternion.Euler(-90f,180f,0f),walls.transform);
                        Instantiate(door_inner,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+0.06f             ,starterpoint.y+1.23f               ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-3.66f)      ,Quaternion.Euler(-90f,180f,0f),walls.transform);
                    
                    }
                    if(doorMap[i,j,2]){
                        Instantiate(wall_whitout_door,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+3.5f       ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2)            ,Quaternion.Euler(-90f,90f,0f),walls.transform);
                        Instantiate(door_frame,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+3.5f       ,starterpoint.y+1.35f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2)            ,Quaternion.Euler(-90f,90f,0f),walls.transform);
                        Instantiate(door_inner,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+3.6f       ,starterpoint.y+1.23f                ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+0.06f)            ,Quaternion.Euler(-90f,90f,0f),walls.transform);
                    
                    }
                    if(doorMap[i,j,3]){
                        Instantiate(wall_whitout_door,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2             ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+3.5f)      ,Quaternion.Euler(-90f,0f,0f),walls.transform);
                        Instantiate(door_frame,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2             ,starterpoint.y+1.35f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+3.5f)      ,Quaternion.Euler(-90f,0f,0f),walls.transform);
                        Instantiate(door_inner,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-0.06f             ,starterpoint.y+1.23f               ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+3.6f)      ,Quaternion.Euler(-90f,0f,0f),walls.transform);
                    
                    }

                }else{//its a maze element
                    //roof
                    Instantiate(roof,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2,starterpoint.y+5.5f,starterpoint.z+(j*fullcellsize)-fullcellsize/2),Quaternion.identity,roofs.transform);
                    ///walls 
                    if(basemaze[i,j,0]){// left - down
                        if(!light){
                            Instantiate(wall_whit_torch,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-3.25f       ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2)            ,Quaternion.Euler(-90f,270f,0f),walls.transform); 
                            light=true;
                        }else{
                            Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2-3.25f       ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2)            ,Quaternion.Euler(-90f,270f,0f),walls.transform); 
                        }
                    }
                    if(basemaze[i,j,1]){// top - right
                        if(!light){
                           Instantiate(wall_whit_torch,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2             ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-3.25f)      ,Quaternion.Euler(-90f,180f,0f),walls.transform);
                            light=true;
                        }else{
                            Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2             ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2-3.25f)      ,Quaternion.Euler(-90f,180f,0f),walls.transform);
                        }
                    }
                    if(basemaze[i,j,2]&& !(endi==i&&endj==j)){// right -top
                        if(!light){
                            Instantiate(wall_whit_torch,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+3.25f       ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2)            ,Quaternion.Euler(-90f,90f,0f),walls.transform);
                            light=true;
                        }else{
                            Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2+3.25f       ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2)            ,Quaternion.Euler(-90f,90f,0f),walls.transform);
                        }
                    }
                    if(basemaze[i,j,3]&& !(endi==i&&endj==j)){// down - left
                        if(!light){
                            Instantiate(wall_whit_torch,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2             ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+3.25f)      ,Quaternion.Euler(-90f,0f,0f),walls.transform);
                            light=true;
                        }else{
                            Instantiate(wall,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2             ,starterpoint.y+2.5f        ,starterpoint.z+(j*fullcellsize)-fullcellsize/2+3.25f)      ,Quaternion.Euler(-90f,0f,0f),walls.transform);
                        }
                    }
                }

            }
        }
        for (int index = 0; index < (roomsmid.Length/2); index++){
            int temporary = rnd.Next(2);
            if(temporary==0){
                Instantiate(desk,new Vector3(starterpoint.x+(roomsmid[index,0]*fullcellsize)             ,starterpoint.y+0.35f        ,starterpoint.z+((roomsmid[index,1]-1)*fullcellsize))      ,Quaternion.Euler(-90f,0f,0f),walls.transform);
                Instantiate(table_candle,new Vector3(starterpoint.x+(roomsmid[index,0]*fullcellsize)+Random.Range(-0.8f, 0.8f)             ,starterpoint.y+1.08f        ,starterpoint.z+((roomsmid[index,1]-1)*fullcellsize)+Random.Range(-0.32f, 0.32f))      ,Quaternion.Euler(-90f,0f,0f),walls.transform);
                temporary = rnd.Next(2);
                if(temporary==0){
                    Instantiate(chair,new Vector3(starterpoint.x+(roomsmid[index,0]*fullcellsize)+0.4f             ,starterpoint.y+0.35f        ,starterpoint.z+((roomsmid[index,1]-1)*fullcellsize)+1.3f)      ,Quaternion.Euler(-90f,180f,0f),walls.transform);
                    
                }else{
                    Instantiate(chair,new Vector3(starterpoint.x+(roomsmid[index,0]*fullcellsize)+0.4f             ,starterpoint.y+0.35f        ,starterpoint.z+((roomsmid[index,1]-1)*fullcellsize)-1.3f)      ,Quaternion.Euler(-90f,0,0f),walls.transform);

                }
            }else{           
                Instantiate(desk,new Vector3(starterpoint.x+(roomsmid[index,0]*fullcellsize)                 ,starterpoint.y+0.35f               ,starterpoint.z+((roomsmid[index,1]-1)*fullcellsize))      ,Quaternion.Euler(-90f,90f,0f),walls.transform);   
                Instantiate(table_candle,new Vector3(starterpoint.x+(roomsmid[index,0]*fullcellsize)+Random.Range(-0.32f, 0.32f)                 ,starterpoint.y+1.08f               ,starterpoint.z+((roomsmid[index,1]-1)*fullcellsize)+Random.Range(-0.8f, 0.8f))      ,Quaternion.Euler(-90f,0f,0f),walls.transform);   
                temporary = rnd.Next(2);
                if(temporary==0){
                    Instantiate(chair,new Vector3(starterpoint.x+(roomsmid[index,0]*fullcellsize)+1.4f            ,starterpoint.y+0.35f          ,starterpoint.z+((roomsmid[index,1]-1)*fullcellsize)+0.5f)      ,Quaternion.Euler(-90f,270f,0f),walls.transform);
                     
                    
                }else{
                    Instantiate(chair,new Vector3(starterpoint.x+(roomsmid[index,0]*fullcellsize)-2f            ,starterpoint.y+0.35f          ,starterpoint.z+((roomsmid[index,1]-1)*fullcellsize)-0.5f)      ,Quaternion.Euler(-90f,90f,0f),walls.transform);
                        

                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void generatTheKey(){
        //in between the key component has a , or ; and its told us wich state we are im not good at english xd
        //key first 3 character is the witch starter point is we pick,the first is between 10-20 second is 1-13 and the 3th is fix 0
        int temporary = 0;
        int playerStarterpontx=0;
        int playerStarterponty=0;
        temporary = rnd.Next(10,21);
        key = temporary.ToString() +",";
        temporary = rnd.Next(1,14);
        key += temporary.ToString() + ",0;";
        //generat how deep is the FILO is go

        //here i have 2 idea with is flexiable to themaze size
        //temporary = Mathf.RoundToInt(Mathf.Pow((x_axis_size*z_axis_size),1f/2f));
        temporary = (Mathf.RoundToInt( Mathf.Pow( (x_axis_size*z_axis_size), 1f / 2f))+Mathf.RoundToInt( Mathf.Pow((x_axis_size+z_axis_size) , 1f / 2f)))/2;
         //temporary = Mathf.RoundToInt(Mathf.Pow((x_axis_size+z_axis_size),1f/2f));
        key += temporary.ToString()+";";
        //its will need in the last argument to the algoritm
        int range = (temporary*2)-1;
        //in the maze generator is the second step in the algoritm is
            //whene the algorimus start its a x and y
            //first i get the start is on the top or bottun 
            temporary = rnd.Next(2);
            if(temporary==0){//top or bot
                temporary = rnd.Next(2);//x_axis_size and colums i counting by 1 and it is counting by 0 so i need a -1 but random is generat to [0,max) and its int so its cast by down and its max-1 so i need only max in the next lines
                if(temporary==0){//top
                    temporary = rnd.Next(z_axis_size);
                    playerStarterponty= 0;
                    playerStarterpontx= temporary;
                }else{//bot
                    temporary = rnd.Next(z_axis_size);
                    playerStarterponty= x_axis_size-1;
                    playerStarterpontx= temporary;

                }
            }else{//left or right                
                temporary = rnd.Next(2);
                if(temporary==0){//left
                    temporary = rnd.Next(x_axis_size);
                    playerStarterponty=temporary;
                    playerStarterpontx=0;
                }else{//right
                    temporary = rnd.Next(x_axis_size);//its gives me 0-x_axis_size a number
                    playerStarterponty= temporary;
                    playerStarterpontx= z_axis_size-1;
                }
            }

            key +=playerStarterponty.ToString() +","+playerStarterpontx.ToString()+";"; //where the player is start

        //where the FILO go
        for(int i=0; i< range && i<11; i++){
            temporary = rnd.Next(4);//all the sides left top right down (0,1,2,3)->(left top right bot)
            key += temporary.ToString();
            key += ",";
        }
        key += "0,1,2,3";//+";"; ha bovitjuk
    }
    void generate(){
        //split the key component to generat easily
        string[] temporarystringholder = key.Split(';');// the stats of the allgoritm param
        string[] temporarystringholder2;

        int[] algoritmStarterPoints;//fill up in foreach

        temporarystringholder2=temporarystringholder[0].Split(',');//first part of the key
        algoritmStarterPoints = new int[temporarystringholder2.Length];
        for (int i = 0; i < temporarystringholder2.Length; i++){
            algoritmStarterPoints[i]=int.Parse(temporarystringholder2[i]);//where we go
        }
        int deep = int.Parse(temporarystringholder[1]);//secund part of the key(the fifos deepes point)
        temporarystringholder2=temporarystringholder[2].Split(',');//3th part of the key
        List<int[]> ListOfThePossiblaStarterPoints = new List<int[]>(); //its will be the list of the point where the algoritm possible start to grow the maze
        ListOfThePossiblaStarterPoints.Add(new int[2]{int.Parse(temporarystringholder2[0]),int.Parse(temporarystringholder2[1])});
        playerStarterpont = new Vector3(starterpoint.x+(ListOfThePossiblaStarterPoints[0][0]*fullcellsize)+fullcellsize/2,starterpoint.y+1.5f,starterpoint.z+(ListOfThePossiblaStarterPoints[0][1]*fullcellsize)-fullcellsize/2);
        playerStarterpont = new Vector3(starterpoint.x+(0*fullcellsize)+fullcellsize/2,starterpoint.y+1.5f,starterpoint.z+(0*fullcellsize)-fullcellsize/2);
        //the first element of this list is the point where we start the maze generat and its the starter point of the player
        
        temporarystringholder2=temporarystringholder[3].Split(',');//4th part of the key and yet the last
        algorimusFilo =  new int[temporarystringholder2.Length];
        for (int i = 0; i < temporarystringholder2.Length; i++){
            algorimusFilo[i]=int.Parse(temporarystringholder2[i]);
        }
        //key read is done,we can start the algoritm


        //karakter a jobb alsó sarokban kezd
        //playersTransforms.position = playerStarterpont;


        bool done=false;
        /*foreach (var a in algorimusFilo)
        {
            Debug.Log(a);
        }*/
        int prex=ListOfThePossiblaStarterPoints[0][0];
        int prey=ListOfThePossiblaStarterPoints[0][1];
        recursiveGenerate(ListOfThePossiblaStarterPoints[0][0],ListOfThePossiblaStarterPoints[0][1],ListOfThePossiblaStarterPoints[0][0],ListOfThePossiblaStarterPoints[0][1],deep);
        ListOfThePossiblaStarterPoints.Clear();
        for (int i = 0; i < x_axis_size; i++){
            for (int j = 0; j < z_axis_size; j++){
            bool needPutInTheTable=false;
                if(generateLeft[i,j]==-1){//-1 is meaning the algoritm is now on this cells(the FILO part)
                    if(i==0){
                            if(generateLeft[i+1,j]==0){
                                needPutInTheTable=true;
                            }
                    }else{
                        if(i==x_axis_size-1){
                            if(generateLeft[i-1,j]==0){
                                needPutInTheTable=true;
                            } 
                        }else{
                            if(generateLeft[i-1,j]==0||generateLeft[i+1,j]==0){
                                needPutInTheTable=true;
                            } 
                        }
                    }
                            
                    if(j==0){
                            if(generateLeft[i,j+1]==0){
                                needPutInTheTable=true;
                            }
                    }else{
                        if(j==(z_axis_size-1)){
                            if(generateLeft[i,j-1]==0){
                                needPutInTheTable=true;
                            }
                        }else{
                        if(generateLeft[i,j-1]==0||generateLeft[i,j+1]==0){
                            needPutInTheTable=true;
                        }

                    }
                }
                if(needPutInTheTable){
                    ListOfThePossiblaStarterPoints.Add(new int[2]{i,j});//have free cell around of this
                }else{
                    generateLeft[i,j]=-2;//not have
                }
                }
            }
        }
    /*foreach (var item in ListOfThePossiblaStarterPoints)
    {
        Debug.Log(item[0] + "és" +item[1]);
    }*/
    //--------------
        int asd=1;
        int szamlalo=0;
        while (!generateDone && ListOfThePossiblaStarterPoints.Count>0){//if a cell is done -> true
            szamlalo++;
            if(algoritmStarterPoints[asd]<ListOfThePossiblaStarterPoints.Count){
                recursiveGenerate(ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][0],ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][1],ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][0],ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][1],deep);
                
            }else{
                asd=(asd+1>2)?0:asd++;
                if(algoritmStarterPoints[asd]<ListOfThePossiblaStarterPoints.Count){
                    recursiveGenerate(ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][0],ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][1],ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][0],ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][1],deep);
                    
                }else{
                    asd=(asd+1>2)?0:asd++;
                    if(algoritmStarterPoints[asd]<ListOfThePossiblaStarterPoints.Count){
                        recursiveGenerate(ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][0],ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][1],ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][0],ListOfThePossiblaStarterPoints[algoritmStarterPoints[asd]][1],deep);
                        
                    }else{
                       recursiveGenerate(ListOfThePossiblaStarterPoints[ListOfThePossiblaStarterPoints.Count-1][0],ListOfThePossiblaStarterPoints[ListOfThePossiblaStarterPoints.Count-1][1],ListOfThePossiblaStarterPoints[ListOfThePossiblaStarterPoints.Count-1][0],ListOfThePossiblaStarterPoints[ListOfThePossiblaStarterPoints.Count-1][1],deep);
                     
                    }
                }
            }
            
            

            ListOfThePossiblaStarterPoints.Clear();
            done = true;
            //its done? and where we start next
            for (int i = 0; i < x_axis_size; i++){
                for (int j = 0; j < z_axis_size; j++){
                bool needPutInTheTable=false;
                    if(generateLeft[i,j]==-1){//-1 is meaning the algoritm is now on this cells(the FILO part)
                        if(i==0){
                                if(generateLeft[i+1,j]==0){
                                    done=false;
                                    needPutInTheTable=true;
                                }
                        }else{
                            if(i==x_axis_size-1){
                               if(generateLeft[i-1,j]==0){
                                    done=false;
                                    needPutInTheTable=true;
                                } 
                            }else{
                               if(generateLeft[i-1,j]==0||generateLeft[i+1,j]==0){
                                    done=false;
                                    needPutInTheTable=true;
                                } 
                            }
                        }
                                
                        if(j==0){
                                if(generateLeft[i,j+1]==0){
                                    done=false;
                                    needPutInTheTable=true;
                                }
                        }else{
                            if(j==(z_axis_size-1)){
                                if(generateLeft[i,j-1]==0){
                                    done=false;
                                    needPutInTheTable=true;
                                }
                            }else{
                            if(generateLeft[i,j-1]==0||generateLeft[i,j+1]==0){
                                done=false;
                                needPutInTheTable=true;
                            }

                        }
                    }
                    if(needPutInTheTable){
                        ListOfThePossiblaStarterPoints.Add(new int[2]{i,j});
                    }else{
                        generateLeft[i,j]=-2;
                    }
                    }
                }
            }
            generateDone=done;
            //Debug.Log(ListOfThePossiblaStarterPoints.Count);
        }
    //---------
    /*
    string itemitem="";
    for (int i = 0; i < x_axis_size; i++){
        for (int j = 0; j < z_axis_size; j++){
            itemitem += ((generateLeft[i,j]==-1)?"c":(generateLeft[i,j]==-2)?"v":"#") + "  ";
        }
        itemitem+="\n";
    }
    Debug.Log(itemitem);
    */
    //-----------
    }
    bool recursiveGenerate(int prex,int prey,int x,int y,int currentRecursivePoinLeft){//i didnt konw now why we give back bool
        if(currentRecursivePoinLeft==0/*|| (generateLeft[x-1,y]!=0 && generateLeft[x,y-1] !=0 && generateLeft[x,y+1]!= 0 && generateLeft[x+1,y]!=0)*/){//if we are cant move forward or we didnt have left stap point
            generateLeft[x,y]=-1;
            return true;
        }
        bool movetonext = false;
        for (int i = 0; i < 15 && !movetonext; i++){
            int next=nextstep();
            if(algorimusFilo[next]==3){//left
                if((prex!=x-1)&&(x-1>=0)&&(generateLeft[x-1,y]==0)){//its possible
                        basemaze[x,y,0]=false;//current cells left side
                        basemaze[x-1,y,2]=false;//next cells right side
                        generateLeft[x,y]=-1;//-1 => its mean we are now in this filo on there road;
                        movetonext=true;
                        recursiveGenerate(x,y,x-1,y,currentRecursivePoinLeft-1);
                        //Debug.Log("az "+x+" és "+y+" helyröl kitoroljűk a 0 falat:"+basemaze[x,y,0]);
                    }
                }
            if(algorimusFilo[next]==2){//up
                if((prey!=y-1)&&(y-1>=0)&&(generateLeft[x,y-1]==0)){//its possible,if its possitiv we cant go there//i dont know why i writed this comment
                    basemaze[x,y,1] = false;//current cells left side
                    basemaze[x,y-1,3] = false;//next cells right side
                    generateLeft[x,y]= -1;//-1 => its mean we are now in this filo on there road;
                    movetonext=true;
                    //Debug.Log("az "+x+" és "+y+" helyröl kitoroljűk a 1 falat:"+basemaze[x,y,1]);
                    recursiveGenerate(x,y,x,y-1,currentRecursivePoinLeft-1);
                }
            }
            if(algorimusFilo[next]==1){//right
                if((prex!=x+1)&&(x+1<x_axis_size)&&(generateLeft[x+1,y]==0)){//its possible
                    basemaze[x,y,2]=false;//current cells left side
                    basemaze[x+1,y,0]=false;//next cells right side
                    generateLeft[x,y]= -1;//-1 => its mean we are now in this filo on there road;
                    movetonext=true;
                    //Debug.Log("az "+x+" és "+y+" helyröl kitoroljűk a 2 falat:"+basemaze[x,y,2]);
                    recursiveGenerate(x,y,x+1,y,currentRecursivePoinLeft-1);
                }
            }
            if(algorimusFilo[next]==0){//down
                if((prey!=y+1)&&(y+1<z_axis_size)&&(generateLeft[x,y+1]==0)){//its possible
                    basemaze[x,y,3]=false;//current cells left side
                    basemaze[x,y+1,1]=false;//next cells right side
                    generateLeft[x,y]= -1;//-1 => its mean we are now in this filo on there road;
                    movetonext=true;
                    //Debug.Log("az "+x+" és "+y+" helyröl kitoroljűk a 3 falat:"+basemaze[x,y,3]);
                    recursiveGenerate(x,y,x,y+1,currentRecursivePoinLeft-1);
                }
            }
        }
        if(!movetonext){
            generateLeft[x,y]= -1;
            return true;
        }
        return false;
    }


    void generateRoom(){
        //int a=rnd.Next(2,5);
        int numbersOfRoomsCell = ((x_axis_size*z_axis_size)/5)-((x_axis_size*z_axis_size)%5);
        int BigRooms=0;//4x4
        int NormalRoom=0;//3x3
        int SmallRoom=0;//2x2
        while(numbersOfRoomsCell>=4){
            if(numbersOfRoomsCell>=(4*4)){
                if(rnd.Next(0,100)>=30){
                    BigRooms++;
                    numbersOfRoomsCell-=(4*4);
                }
            }
            if(numbersOfRoomsCell>=(3*3)){
                if(rnd.Next(0,100)>=60){
                    NormalRoom++;
                    numbersOfRoomsCell-=(4*4);
                }
            }
            if(numbersOfRoomsCell>=(2*2)){
                    SmallRoom++;
                    numbersOfRoomsCell-=(4*4);
            }
        }
        roomMap = new int[x_axis_size,z_axis_size];
        int roomnumber =1;
        for (int i = 0; i < x_axis_size; i++){
            for (int j = 0; j < z_axis_size; j++){
                roomMap[i,j]=0;// no room here
            }
        }
        roomsmid=new float[BigRooms+NormalRoom+SmallRoom,2];
        int roomsmidindex=0;
        for(int a=BigRooms;a>0;a--){
            bool done=false;
            while(!done){
            int i = rnd.Next(1,x_axis_size-4);
            int j = rnd.Next(1,z_axis_size-4);//pick a random spot
            done=true;
            for (int ii = i-1; ii < i+5; ii++){
                for (int jj = j-1; jj < j+5; jj++){
                    if(roomMap[ii,jj]!=0){
                        done=false;
                    }
                }
            }
            if(done){
                roomsmid[roomsmidindex,0]=i+2f;
                roomsmid[roomsmidindex,1]=j+2f;
                roomsmidindex++;
                for (int ii = i; ii < i+4; ii++){
                    for (int jj = j; jj < j+4; jj++){
                        roomMap[ii,jj]=roomnumber;
                        generateLeft[ii,jj]= 4;
                        //inner
                        if(ii!=i  &&  ii!=i+3  &&  jj!=j  &&  jj!=j+3){
                            basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        //border
                        if(ii == i    && !(jj==j  ||  jj==j+3)){
                            //basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(ii == i+3  && !(jj==j  ||  jj==j+3)){
                            basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            //basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(jj == j    && !(ii==i  ||  ii==i+3)){
                            basemaze[ii,jj,0]=false;
                            //basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(jj == j+3  && !(ii==i  ||  ii==i+3)){
                            basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            //basemaze[ii,jj,3]=false;
                        }
                        //corner
                        if(ii==i    &&  jj==j  ){
                            //basemaze[ii,jj,0]=false;
                            //basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(ii==i+3  &&  jj==j  ){
                            basemaze[ii,jj,0]=false;
                            //basemaze[ii,jj,1]=false;
                            //basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(ii==i    &&  jj==j+3){
                            //basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            //basemaze[ii,jj,3]=false;
                        }
                        if(ii==i+3  &&  jj==j+3){
                            basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            //basemaze[ii,jj,2]=false;
                            //basemaze[ii,jj,3]=false;
                        }
                    }
                }
                //door
                int num=rnd.Next(3);
                switch (rnd.Next(4)){
                    case 0://bot
                        basemaze[i,j+num,0]=false;
                        basemaze[i-1,j+num,2]=false;
                        doorMap[i,j+num,0]=true;
                        break;
                    case 1://right
                        basemaze[i+num,j,1]=false;
                        basemaze[i+num,j-1,3]=false;
                        doorMap[i+num,j,1]=true;
                        break;
                    case 2://top
                        basemaze[i+3,j+num,2]=false;
                        basemaze[i+3+1,j+num,0]=false;
                        doorMap[i+3,j+num,2]=true;
                        break;
                    case 3://left
                        basemaze[i+num,j+3,3]=false;
                        basemaze[i+num,j+3+1,1]=false;
                        doorMap[i+num,j+3,3]=true;
                        break;
                }
                roomnumber++;
            }
            }
        }

        for(int b=NormalRoom;b>0;b--){
            bool done=false;
            while(!done){
            int i = rnd.Next(1,x_axis_size-3);
            int j = rnd.Next(1,z_axis_size-3);//pick a random spot
            done=true;
            for (int ii = i-1; ii < i+4; ii++){
                for (int jj = j-1; jj < j+4; jj++){
                    if(roomMap[ii,jj]!=0){
                        done=false;
                    }
                }
            }
            if(done){
                roomsmid[roomsmidindex,0]=i+1.5f;
                roomsmid[roomsmidindex,1]=j+1.5f;
                roomsmidindex++;
                for (int ii = i; ii < i+3; ii++){
                    for (int jj = j; jj < j+3; jj++){
                        roomMap[ii,jj]=roomnumber;
                        generateLeft[ii,jj]= 3;
                        //inner
                        if(ii!=i  &&  ii!=i+2  &&  jj!=j  &&  jj!=j+2){
                            basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        //border
                        if(ii == i    && !(jj==j  ||  jj==j+2)){
                            //basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(ii == i+2  && !(jj==j  ||  jj==j+2)){
                            basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            //basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(jj == j    && !(ii==i  ||  ii==i+2)){
                            basemaze[ii,jj,0]=false;
                            //basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(jj == j+2  && !(ii==i  ||  ii==i+2)){
                            basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            //basemaze[ii,jj,3]=false;
                        }
                        //corner
                        if(ii==i    &&  jj==j  ){
                            //basemaze[ii,jj,0]=false;
                            //basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(ii==i+2  &&  jj==j  ){
                            basemaze[ii,jj,0]=false;
                            //basemaze[ii,jj,1]=false;
                            //basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(ii==i    &&  jj==j+2){
                            //basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            //basemaze[ii,jj,3]=false;
                        }
                        if(ii==i+2  &&  jj==j+2){
                            basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            //basemaze[ii,jj,2]=false;
                            //basemaze[ii,jj,3]=false;
                        }

                        
                    
                    }
                }
                //door
                int num=rnd.Next(2);
                switch (rnd.Next(4)){
                    case 0://bot
                        basemaze[i,j+num,0]=false;
                        basemaze[i-1,j+num,2]=false;
                        doorMap[i,j+num,0]=true;
                        break;
                    case 1://right
                        basemaze[i+num,j,1]=false;
                        basemaze[i+num,j-1,3]=false;
                        doorMap[i+num,j,1]=true;
                        break;
                    case 2://top
                        basemaze[i+2,j+num,2]=false;
                        basemaze[i+2+1,j+num,0]=false;
                        doorMap[i+2,j+num,2]=true;
                        break;
                    case 3://left
                        basemaze[i+num,j+2,3]=false;
                        basemaze[i+num,j+2+1,1]=false;
                        doorMap[i+num,j+2,3]=true;
                        break;
                }
                roomnumber++;
            }
            }
        }

        for(int c=SmallRoom;c>0;c--){
            bool done=false;
            while(!done){
            int i = rnd.Next(1,x_axis_size-2);
            int j = rnd.Next(1,z_axis_size-2);//pick a random spot
            done=true;
            for (int ii = i-1; ii < i+3; ii++){
                for (int jj = j-1; jj < j+3; jj++){
                    if(roomMap[ii,jj]!=0){
                        done=false;
                    }
                }
            }
            if(done){
                roomsmid[roomsmidindex,0]=i+1f;
                roomsmid[roomsmidindex,1]=j+1f;
                roomsmidindex++;
                for (int ii = i; ii < i+2; ii++){
                    for (int jj = j; jj < j+2; jj++){
                        roomMap[ii,jj]=roomnumber;
                        generateLeft[ii,jj]= 2;
                        //corner
                        if(ii==i    &&  jj==j  ){
                            //basemaze[ii,jj,0]=false;
                            //basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(ii==i+1  &&  jj==j  ){
                            basemaze[ii,jj,0]=false;
                            //basemaze[ii,jj,1]=false;
                            //basemaze[ii,jj,2]=false;
                            basemaze[ii,jj,3]=false;
                        }
                        if(ii==i    &&  jj==j+1){
                            //basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            basemaze[ii,jj,2]=false;
                            //basemaze[ii,jj,3]=false;
                        }
                        if(ii==i+1  &&  jj==j+1){
                            basemaze[ii,jj,0]=false;
                            basemaze[ii,jj,1]=false;
                            //basemaze[ii,jj,2]=false;
                            //basemaze[ii,jj,3]=false;
                        }
                    }
                }
                //door
                int num=rnd.Next(0,1);
                switch (rnd.Next(4)){
                    case 0://bot
                        basemaze[i,j+num,0]=false;
                        basemaze[i-1,j+num,2]=false;
                        doorMap[i,j+num,0]=true;
                        break;
                    case 1://right
                        basemaze[i+num,j,1]=false;
                        basemaze[i+num,j-1,3]=false;
                        doorMap[i+num,j,1]=true;
                        break;
                    case 2://top
                        basemaze[i+1,j+num,2]=false;
                        basemaze[i+1+1,j+num,0]=false;
                        doorMap[i+1,j+num,2]=true;
                        break;
                    case 3://left
                        basemaze[i+num,j+1,3]=false;
                        basemaze[i+num,j+1+1,1]=false;
                        doorMap[i+num,j+1,3]=true;
                        break;
                }
                roomnumber++;
            }
            }
        }
        string itemitem="";
        for (int i = 0; i < x_axis_size; i++){
            for (int j = 0; j < z_axis_size; j++){
                itemitem += roomMap[i,j]+" ";
            }   
            itemitem+="\n";
        }
        Debug.Log(itemitem);
        Debug.Log(roomsmidindex);
    }

    int nextstep(){
        positionInTheFiloArray += 1; //readable
        if(positionInTheFiloArray<algorimusFilo.Length){
            return positionInTheFiloArray;
        }
        positionInTheFiloArray=0;
        return positionInTheFiloArray;
    }
}