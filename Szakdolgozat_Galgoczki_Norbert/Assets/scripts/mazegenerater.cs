using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mazegenerater : MonoBehaviour
{
    private GameObject dogGO;
    private Dog dogscript;
    [SerializeField]private Transform playersTransforms;
    public string key;//where we start the generat;how deep is the FILO go;where is the player start ;wher the filo is step;
    [SerializeField]private Vector3 playerStarterpont;
    [SerializeField]private bool keyalreadygiven = false;
    private System.Random rnd = new System.Random();
    [SerializeField]private GameObject floor;
    [SerializeField]private GameObject roof;
    [SerializeField]private GameObject wall;
    [SerializeField]private GameObject floors;
    [SerializeField]private GameObject roofs;
    [SerializeField]private GameObject walls;
    [SerializeField]private bool[,,] basemaze;
    [SerializeField]private int[,] roomMap;
    //    _1_default
    // 0 |   | 2       its the cells wall numbers
    //   |_3_|
    // default pos of wall --
    [SerializeField]private int rows;
    [SerializeField]private int columns;
    [SerializeField]private float size = 6;//size of the maze's cells
    public short[,] generateLeft;
    private float fullcellsize = 7;//cell+wallssize
    private Vector3 starterpoint = new Vector3(20,0,20);//the mazes starter corner
    private bool generateDone = false;
    private int[] algorimusFilo;
    private int positionInTheFiloArray = 0;
    // Start is called before the first frame update
    void Start(){


        //dogGO = GameObject.Find("dog");
        //dogscript = dogGO.GetComponent<Dog>();


        rows    = rnd.Next(10,40);
        columns = rnd.Next(10,40);

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
                //Instantiate(roof,new Vector3(starterpoint.x+(i*fullcellsize)+fullcellsize/2,starterpoint.y+5.5f,starterpoint.z+(j*fullcellsize)-fullcellsize/2),Quaternion.identity,roofs.transform);
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
        //the first element of this list is the point where we start the maze generat and its the starter point of the player
        
        temporarystringholder2=temporarystringholder[3].Split(',');//4th part of the key and yet the last
        algorimusFilo =  new int[temporarystringholder2.Length];
        for (int i = 0; i < temporarystringholder2.Length; i++){
            algorimusFilo[i]=int.Parse(temporarystringholder2[i]);
        }
        //key read is done,we can start the algoritm

        playersTransforms.position = playerStarterpont;
        bool done=false;
        /*foreach (var a in algorimusFilo)
        {
            Debug.Log(a);
        }*/
        int prex=ListOfThePossiblaStarterPoints[0][0];
        int prey=ListOfThePossiblaStarterPoints[0][1];
        recursiveGenerate(ListOfThePossiblaStarterPoints[0][0],ListOfThePossiblaStarterPoints[0][1],ListOfThePossiblaStarterPoints[0][0],ListOfThePossiblaStarterPoints[0][1],deep);
        ListOfThePossiblaStarterPoints.Clear();
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < columns; j++){
            bool needPutInTheTable=false;
                if(generateLeft[i,j]==-1){//-1 is meaning the algoritm is now on this cells(the FILO part)
                    if(i==0){
                            if(generateLeft[i+1,j]==0){
                                needPutInTheTable=true;
                            }
                    }else{
                        if(i==rows-1){
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
                        if(j==(columns-1)){
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
            for (int i = 0; i < rows; i++){
                for (int j = 0; j < columns; j++){
                bool needPutInTheTable=false;
                    if(generateLeft[i,j]==-1){//-1 is meaning the algoritm is now on this cells(the FILO part)
                        if(i==0){
                                if(generateLeft[i+1,j]==0){
                                    done=false;
                                    needPutInTheTable=true;
                                }
                        }else{
                            if(i==rows-1){
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
                            if(j==(columns-1)){
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
    for (int i = 0; i < rows; i++){
        for (int j = 0; j < columns; j++){
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
            if(algorimusFilo[next]==0){//left
                if((prex!=x-1)&&(x-1>=0)&&(generateLeft[x-1,y]==0)){//its possible
                        basemaze[x,y,0]=false;//current cells left side
                        basemaze[x-1,y,2]=false;//next cells right side
                        generateLeft[x,y]=-1;//-1 => its mean we are now in this filo on there road;
                        movetonext=true;
                        recursiveGenerate(x,y,x-1,y,currentRecursivePoinLeft-1);
                        //Debug.Log("az "+x+" és "+y+" helyröl kitoroljűk a 0 falat:"+basemaze[x,y,0]);
                    }
                }
            if(algorimusFilo[next]==1){//up
                if((prey!=y-1)&&(y-1>=0)&&(generateLeft[x,y-1]==0)){//its possible,if its possitiv we cant go there//i dont know why i writed this comment
                    basemaze[x,y,1] = false;//current cells left side
                    basemaze[x,y-1,3] = false;//next cells right side
                    generateLeft[x,y]= -1;//-1 => its mean we are now in this filo on there road;
                    movetonext=true;
                    //Debug.Log("az "+x+" és "+y+" helyröl kitoroljűk a 1 falat:"+basemaze[x,y,1]);
                    recursiveGenerate(x,y,x,y-1,currentRecursivePoinLeft-1);
                }
            }
            if(algorimusFilo[next]==2){//right
                if((prex!=x+1)&&(x+1<rows)&&(generateLeft[x+1,y]==0)){//its possible
                    basemaze[x,y,2]=false;//current cells left side
                    basemaze[x+1,y,0]=false;//next cells right side
                    generateLeft[x,y]= -1;//-1 => its mean we are now in this filo on there road;
                    movetonext=true;
                    //Debug.Log("az "+x+" és "+y+" helyröl kitoroljűk a 2 falat:"+basemaze[x,y,2]);
                    recursiveGenerate(x,y,x+1,y,currentRecursivePoinLeft-1);
                }
            }
            if(algorimusFilo[next]==3){//down
                if((prey!=y+1)&&(y+1<columns)&&(generateLeft[x,y+1]==0)){//its possible
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
        int numbersOfRoomsCell = ((rows*columns)/5)-((rows*columns)%5);
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
        roomMap = new int[rows,columns];
        int roomnumber =1;
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < columns; j++){
                roomMap[i,j]=0;// no room here
            }
        }

        for(int a=BigRooms;a>0;a--){
            bool done=false;
            while(!done){
            int i = rnd.Next(0,rows-3);
            int j = rnd.Next(0,columns-3);//pick a random spot
            done=true;
            for (int ii = i; ii < i+4; ii++){
                for (int jj = j; jj < j+4; jj++){
                    if(roomMap[ii,jj]!=0){
                        done=false;
                    }
                }
            }
            if(done){
                for (int ii = i; ii < i+4; ii++){
                    for (int jj = j; jj < j+4; jj++){
                        //roomMap[ii,jj]=roomnumber;
                        roomMap[ii,jj]=3;
                    }
                }
                roomnumber++;
            }
            }
        }

        for(int b=NormalRoom;b>0;b--){
            bool done=false;
            while(!done){
            int i = rnd.Next(0,rows-2);
            int j = rnd.Next(0,columns-2);//pick a random spot
            done=true;
            for (int ii = i; ii < i+3; ii++){
                for (int jj = j; jj < j+3; jj++){
                    if(roomMap[ii,jj]!=0){
                        done=false;
                    }
                }
            }
            if(done){
                for (int ii = i; ii < i+3; ii++){
                    for (int jj = j; jj < j+3; jj++){
                        //roomMap[ii,jj]=roomnumber;
                        roomMap[ii,jj]=2;
                    
                    }
                }
                roomnumber++;
            }
            }
        }

        for(int c=SmallRoom;c>0;c--){
            bool done=false;
            while(!done){
            int i = rnd.Next(0,rows-1);
            int j = rnd.Next(0,columns-1);//pick a random spot
            done=true;
            for (int ii = i; ii < i+2; ii++){
                for (int jj = j; jj < j+2; jj++){
                    if(roomMap[ii,jj]!=0){
                        done=false;
                    }
                }
            }
            if(done){
                for (int ii = i; ii < i+2; ii++){
                    for (int jj = j; jj < j+2; jj++){
                        //roomMap[ii,jj]=roomnumber;
                        roomMap[ii,jj]=1;
                    }
                }
                roomnumber++;
            }
            }
        }
        string itemitem="";
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < columns; j++){
                itemitem += roomMap[i,j]+" ";
            }   
            itemitem+="\n";
        }
        Debug.Log(itemitem);
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