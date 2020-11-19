using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mazegenerater : MonoBehaviour
{
    public Transform playersTransforms;
    public string key;//where we start the generat;how deep is the FILO go;where is the player start ;wher the filo is step;
    public Vector3 playerStarterpont;
    public bool keyalreadygiven = false;
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
    private Vector3 starterpoint = new Vector3(20,0,20);//the mazes starter corner
    private bool generateDone = false;
    private short[,] generateLeft;
    // Start is called before the first frame update
    void Start(){
        rows    = rnd.Next(3,16);
        columns = rnd.Next(3,16);

        basemaze = new bool[rows,columns,4];
        generateLeft = new short[rows,columns];
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < columns; j++){
                basemaze[i,j,0]=true;// left
                basemaze[i,j,1]=true;// top
                basemaze[i,j,2]=true;// right
                basemaze[i,j,3]=true;// down
                generateLeft[i,j]=0;
            }
        }
        if(!keyalreadygiven){
            generatTheKey();
        }
        generateRoom();
        generate();
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
        berendezes();
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
        //temporary = Mathf.RoundToInt(Mathf.Pow((rows*columns),1f/2f));
        temporary = (Mathf.RoundToInt( Mathf.Pow( (rows*columns), 1f / 2f))+Mathf.RoundToInt( Mathf.Pow((rows+columns) , 1f / 2f)))/2;
        //temporary = Mathf.RoundToInt(Mathf.Pow((rows+columns),1f/2f));
        key += temporary.ToString()+";";
        //its will need in the last argument to the algoritm
        int range = (temporary*2)-1;
        //in the maze generator is the second step in the algoritm is
            //whene the algorimus start its a x and y
            //first i get the start is on the top or bottun 
            temporary = rnd.Next(2);
            if(temporary==0){//top or bot
                temporary = rnd.Next(2);//rows and colums i counting by 1 and it is counting by 0 so i need a -1 but random is generat to [0,max) and its int so its cast by down and its max-1 so i need only max in the next lines
                if(temporary==0){//top
                    temporary = rnd.Next(columns);
                    playerStarterponty= 0;
                    playerStarterpontx= temporary;
                }else{//bot
                    temporary = rnd.Next(columns);
                    playerStarterponty= rows-1;
                    playerStarterpontx= temporary;
                }
            }else{//left or right                
                temporary = rnd.Next(2);
                if(temporary==0){//left
                    temporary = rnd.Next(rows);
                    playerStarterponty=temporary;
                    playerStarterpontx=0;
                }else{//right
                    temporary = rnd.Next(rows);//its gives me 0-rows a number
                    playerStarterponty= temporary;
                    playerStarterpontx= columns-1;
                }
            }
            //im not sure itt good
            key +=playerStarterponty.ToString() +","+playerStarterpontx.ToString()+";";

        //where the FILO go
        for(int i=0; i< range; i++){
            temporary = rnd.Next(4);//all the sides left top right down
            key += temporary.ToString();
            key += ",";
        }
        key += "0;";
    }
    void generate(){
        //split the key component to generat easily
        string[] temporarystringholder = key.Split(';');
        string[] temporarystringholder2;
        int[] algoritmStarterPoints;//fill up in foreach
        temporarystringholder2=temporarystringholder[0].Split(',');
        algoritmStarterPoints = new int[temporarystringholder2.Length];
        for (int i = 0; i < temporarystringholder2.Length; i++){
            algoritmStarterPoints[i]=int.Parse(temporarystringholder2[i]);
        }
        int algorimusFiloDeepestPoint = int.Parse(temporarystringholder[1]);
        temporarystringholder2=temporarystringholder[2].Split(',');
        List<int[]> ListOfThePossiblaStarterPoints = new List<int[]>(); //its will be the list of the point where the algoritm possible start to grow the maze
        ListOfThePossiblaStarterPoints.Add(new int[2]{int.Parse(temporarystringholder2[0]),int.Parse(temporarystringholder2[1])});
        playerStarterpont = new Vector3(starterpoint.x+(ListOfThePossiblaStarterPoints[0][0]*fullcellsize)+fullcellsize/2,starterpoint.y+1.5f,starterpoint.z+(ListOfThePossiblaStarterPoints[0][1]*fullcellsize)-fullcellsize/2);
        int[] algorimusFilo;//fill up in foreach
        temporarystringholder2=temporarystringholder[3].Split(',');
        algorimusFilo =  new int[temporarystringholder2.Length];
        for (int i = 0; i < temporarystringholder2.Length; i++){
            algorimusFilo[i]=int.Parse(temporarystringholder2[i]);
        }
        //key read is done,we can start the algoritm

        Debug.Log(ListOfThePossiblaStarterPoints[0][0]+" and " + ListOfThePossiblaStarterPoints[0][1]);
        playersTransforms.position = playerStarterpont;
        bool done=false;
        while (!generateDone){//if a cell is done -> true
            
            

            done = true;
            //its done?
            for (int i = 0; i < rows; i++){
                for (int j = 0; j < columns; j++){//ne felejtsd el updatelni!!
                    if(generateLeft[i,j]==0 ||generateLeft[i,j]==-1){
                        done = false;
                    }
                }
            }
            generateDone = true;//its just temerary
        }
    }
    bool recursiveGenerate(int x,int y,int currentRecursivePoinLeft){//
        if(currentRecursivePoinLeft==0){
            return true;
        }
        return false;
    }

    void berendezes(){

    }

    void generateRoom(){

    }
}