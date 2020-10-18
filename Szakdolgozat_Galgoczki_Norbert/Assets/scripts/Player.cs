using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform kamera;
    public Transform playerBody;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;
    public CharacterController controller;
    public float vertikalisNezes = 0f;
    public float egerErzekenyseg = 1000f;
    public float egerX;
    public float egerY;
    public float moveX;
    public float moveZ;
    public float speed = 10f;
    public float gravitysize = 0f;//default -10f
    public float jumpHight = 1f;
    public Vector3 move;
    public Vector3 movegravity;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded= Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);
        if(isGrounded && movegravity.y < 0){
            movegravity.y=-1f;
        }
        //input kezelés
        egerX = Input.GetAxis("Mouse X") * egerErzekenyseg * Time.deltaTime;
        egerY = Input.GetAxis("Mouse Y") * egerErzekenyseg * Time.deltaTime;
        moveX = Input.GetAxis("Horizontal") * Time.deltaTime;
        moveZ = Input.GetAxis("Vertical") * Time.deltaTime;


        vertikalisNezes -= egerY;
        vertikalisNezes = Mathf.Clamp(vertikalisNezes, -90f, 90f);
        kamera.transform.localRotation = Quaternion.Euler(vertikalisNezes,0f,0f);
        playerBody.Rotate(Vector3.up * egerX);
        
        move = transform.right *moveX +transform.forward * moveZ;
        
        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButton("Jump") && isGrounded){
            movegravity.y= Mathf.Sqrt(jumpHight * -2 * gravitysize);
        }

        movegravity.y += gravitysize * Time.deltaTime;

        controller.Move(movegravity*Time.deltaTime);
    }
}
