using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Text text;
    private Text end_text;
    private Text text_csont;
    private Text text_cd;
    private float usedelay=0f;
    private float usedelay_max = 5f;
    private int bone_number=0;

    //scenne reset
    private GameObject floors;
    private GameObject roofs;
    private GameObject walls;
    private GameObject end_room_folder;
    private mazegenerater mazegenerator;

    private Ray ray;
    private Transform temporary;
    private RaycastHit hit;
    private Collider pre;
    private Door doorscript;
    private Chest chestscript;
    private Dog dogscript;
    private Transform kamera;
    private Transform playerBody;
    private Transform groundCheck;
    private float groundDistance = 0.4f;
    private LayerMask groundMask;
    private bool isGrounded;
    private CharacterController controller;
    private float vertikalisNezes = 0f;
    private float egerErzekenyseg = 10f;
    private float egerX;
    private float egerY;
    private float moveX;
    private float moveZ;
    private float speed = 10f;
    private float gravitysize = -13f;//default -10f
    private float jumpHight = 4.5f;
    private Vector3 move;
    private Vector3 movegravity;
    private GameObject end;

    
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//the cursor is dont go out off the screen
        Cursor.visible = false;

        Transform temperary = this.gameObject.transform.GetChild(3);
        animator = temperary.gameObject.GetComponent<Animator>();

        text = GameObject.Find("UIszoveg").GetComponent<Text>();//
        end_text = GameObject.Find("UIendszoveg").GetComponent<Text>();//
        text_csont = GameObject.Find("UIcsontszamlalo").GetComponent<Text>();//
        text_cd = GameObject.Find("UIcdszoveg").GetComponent<Text>();//
        end=(GameObject)Resources.Load("prefab/end_room", typeof(GameObject));
        //alap irányitáshoz a bekért public változok lekérése
        kamera = this.transform.GetChild(0);
        playerBody = this.transform;
        groundCheck = this.transform.GetChild(1);
        controller = this.transform.GetComponent<CharacterController>();
        //
        end_room_folder = GameObject.Find("end_room_folder");
        floors = GameObject.Find("floors");
        roofs = GameObject.Find("roofs");
        walls = GameObject.Find("walls");
        mazegenerator = GameObject.Find("Maze").GetComponent<mazegenerater>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //isGrounded= Physics.CheckSphere(,groundDistance,groundMask);
        if (Physics.Raycast(groundCheck.position,Vector3.down, 0.2f)){
            isGrounded=true;
            animator.ResetTrigger("jump_trigger");
        }else{
            isGrounded=false;
        }

        if(isGrounded && movegravity.y < 0){//fall
            movegravity.y=-1f;
        }

        //input kezelés
        egerX = Input.GetAxis("Mouse X") * egerErzekenyseg;//unity give -1,1 * mouse sens * time to run the game same on a old pc and a new one
        egerY = Input.GetAxis("Mouse Y") * egerErzekenyseg;//* Time.deltaTime
        moveX = Input.GetAxis("Horizontal") * Time.deltaTime;
        moveZ = Input.GetAxis("Vertical") * Time.deltaTime;
        

        vertikalisNezes -= egerY;
        vertikalisNezes = Mathf.Clamp(vertikalisNezes, -90f, 90f);//dont spin the hade up or down

        kamera.transform.localRotation = Quaternion.Euler(vertikalisNezes,0f,0f);//set the kamera

        playerBody.Rotate(Vector3.up * egerX);
        
        move = transform.right * moveX*0.5f + transform.forward * moveZ;//its a vector to the distance wher we want to go
        if(move!=new Vector3(0,0,0)){
            animator.SetTrigger("run_trigger");
        }
        
        if(Input.GetButton("Jump")&& isGrounded){
            movegravity.y= jumpHight;
            // animation
            animator.ResetTrigger("run_trigger");
            animator.SetTrigger("jump_trigger");

        }

        controller.Move(move * speed);
        movegravity.y += gravitysize * Time.deltaTime;

        controller.Move(movegravity*Time.deltaTime);

        //----------------
        //text.text="Hello"+movegravity.y;
        //if(raycastpoint!=null){
        //    Debug.Log("talált valamilyen gyereket "+ kamera.transform.eulerAngles.x+" "+ raycastpoint.transform.eulerAngles.y+" "+ kamera.transform.eulerAngles.z+"\n");
        //}
        
        //delay állitás
        usedelay-=Time.deltaTime;
        if(usedelay<0)usedelay=0f;


        if (Physics.Raycast(kamera.position,kamera.forward, out hit, 3)){
            //Debug.DrawLine(this.transform.position, hit.point,Color.red,2);
            //Debug.Log(hit.collider.name);
            temporary = hit.collider.transform;
            doorscript = temporary.GetComponent<Door>();
            chestscript = temporary.GetComponent<Chest>();
            dogscript = temporary.GetComponent<Dog>();
            if(doorscript!=null||chestscript!=null||dogscript!=null){
                if(doorscript!=null){
                    text.text="Az ajtó kinyitásához nyomd meg az 'e' betüt";
                    if (Input.GetKey(KeyCode.E)&&usedelay<=0.01){
                        usedelay=usedelay_max;
                        doorscript.open_door();
                    }   
                }
                if(chestscript!=null){
                    text.text="Az láda tetejének levételéhez nyomd meg az 'e' betüt";
                    if (Input.GetKey(KeyCode.E)&&usedelay<=0.01){
                        usedelay=usedelay_max;
                        chestscript.open_chest();
                    }   
                }
                 if(dogscript!=null){
                    if(bone_number<=0){
                        text.text="nincs elég csontod,vigyázz";
                    }else{
                        text.text="nyomd meg az 'e' betüt, hogy megállitsd a kutyákat";
                    }
                    if (Input.GetKey(KeyCode.E)&&usedelay<=0.01&&bone_number>0){
                        usedelay=usedelay_max;
                        Debug.Log("hmm annyira meg lett állitva hogy el is tünt sad");
                        dogscript.stop_wait_a_minute();
                        bone_number--;
                    }
                
                }
            }else if(temporary.name=="bone_object"){
                text.text="A csont felvételéhez nyomd meg az 'e' betüt, ezzel tudod majd a kutyákat megállitani";
                    if (Input.GetKey(KeyCode.E)&&usedelay<=0.01){
                        usedelay=usedelay_max;
                        Debug.Log("felvettél egy csontot");
                        Destroy(temporary.gameObject);
                        bone_number++;
                        Debug.Log(bone_number);
                    } 
                
            }else{
                text.text="";//temporary.name;
            }
        }else{
            text.text="";
        }

        text_csont.text = "csontok:" + bone_number;
        text_cd.text =(usedelay<=0.001f)?"":"a következö cselekedetig ennyit kell várnod "+((usedelay-usedelay%0.5)+0.5)+"sec";

        /*
        if (Input.GetKey("escape")){
            Cursor.lockState = CursorLockMode.None;//the cursor is dont go out off the screen
            Cursor.visible = true;
            //SceneManager.LoadScene(1);
            mazegenerator.reset();
            SceneManager.LoadScene(0);
        }
        */
    }
    private void OnTriggerEnter(Collider other){
        //cellába belépés ->cella script-> kutyának elküld pozicio
        Cell cell = other.gameObject.GetComponent<Cell>();
        Dog dogscript_triggerCollider = other.gameObject.GetComponent<Dog>();
        if(cell != null){
            cell.playerEnter();
        }
        if(dogscript_triggerCollider != null){
            if(dogscript_triggerCollider.isDangerous()){
                backToMenu();
            }
        }

    }


    private void OnTriggerStay(Collider other){//a végső szoba terűletén van csak triggeres collider
        if(other.gameObject.name==end.name+"(Clone)"){
            //Debug.Log("benne van");
            end_text.text="A Lanbirintusból való kilépéshez nyomd meg az 'e' betűt";
            if (Input.GetKey(KeyCode.E)){
                backToMenu();

            }
        }else{
            //Debug.Log(other.name+" - "+end.name);
        }
    }


     private void OnTriggerExit(Collider other){//a végső szoba terűletén van csak triggeres collider
        if(other.gameObject.name==end.name+"(Clone)"){
            end_text.text="";
        }
        //cellába kilép ->cella script-> kutyának elküld az invalid pozicio
        //amig invalig a pozicio addig nem megy a következö node-ra és ha uj cellába lépűnk akkor  és a kutya elérte a következö nodeot akkor A* ujra számolás
        Cell cell =other.gameObject.GetComponent<Cell>();
        if(cell != null){
            cell.playerExit();
        }
    }
    
    /*
    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 2);
    }
    */
    private void backToMenu(){
        Cursor.lockState = CursorLockMode.None;//the cursor is dont go out off the screen
        Cursor.visible = true;
        mazegenerator.reset();
        SceneManager.LoadScene(0);
    }
}
