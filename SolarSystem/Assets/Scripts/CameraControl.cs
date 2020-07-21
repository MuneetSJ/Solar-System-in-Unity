using UnityEngine;

public class CameraControl : MonoBehaviour {
    [Header("Camera and object properties")]
    public static CameraControl cameraInstance;
    public Transform followTarget;
    public Transform ChildCamera;

    [Header("Speed adjustments")]
    public float fastSpeed = 10f;
    public float slowSpeed = 1.5f;
    public float fastRotateAmount = 2.5f;
    public float slowRotateAmount = .25f;
    public Vector3 fastZoomAmount = new Vector3(0f, -50f, 50f);
    public Vector3 slowZoomAmount = new Vector3(0f, -10f, 10f);

    [Space]
    [Space]
    [Space]
    public Vector3 offest;
    private Vector3 pos, initPos;
    private Quaternion rot, initRot;
    private Vector3 zoom, initZoom;

    private Vector3 dragStartPos;
    private Vector3 dragEndPos;
    private Vector3 rotStartPos;
    private Vector3 rotEndPos;

    private void Start() {
        //Initialises instance and offsset the camera follows when object is selected
        cameraInstance = this;

        //sets the initaial positions of the camera
        initPos = pos = transform.position;
        initRot = rot = transform.rotation;
        initZoom = zoom = ChildCamera.localPosition;;

    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (followTarget != null)
        {
            //If there is an object to folllow zoom into it and only allow rotation and zoom movement
            offest = new Vector3(0f, -(followTarget.localScale.x * 2f), (followTarget.localScale.y * 2f));
          
            transform.localPosition = followTarget.position + offest;

            KeyMovementInput();
            MouseMovementInput();

            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 5);
            ChildCamera.localPosition = Vector3.Lerp(ChildCamera.localPosition, zoom, Time.deltaTime * 5);

        }
        else
        {
            //Gets all inputs and moves camera accordingly 
            KeyMovementInput();
            MouseMovementInput();

            //How each position is updated
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 5);
            ChildCamera.localPosition = Vector3.Lerp(ChildCamera.localPosition, zoom, Time.deltaTime * 5);

        }
        //If escape is pressed the camrea is reset 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            followTarget = null;
            Reset();
        }
    }

    /*Takes in the inputs from keyboard updates the postiion of the camera accordingly*/
    private void KeyMovementInput()
    {

        float movementSpeed;
        float rotationAmount;
        Vector3 zoomAmount;

        //Using left shift to speed up movement
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
            rotationAmount = fastRotateAmount;
            zoomAmount = fastZoomAmount;
        }
        else
        {
            movementSpeed = slowSpeed;
            rotationAmount = slowRotateAmount;
            zoomAmount = slowZoomAmount;
        }

        //WASD to move the camera position
        if (Input.GetKey(KeyCode.W))
        {
            pos += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos += (transform.right * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos += (transform.right * movementSpeed);
        }

        //Arrow keys to rotate the camera
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rot *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rot *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rot *= Quaternion.Euler(Vector3.right * -rotationAmount);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rot *= Quaternion.Euler(Vector3.right * rotationAmount);
        }

        //R and F key zoom into the screen
        if (Input.GetKey(KeyCode.R))
        {
            zoom += zoomAmount;

        }
        if (Input.GetKey(KeyCode.F))
        {
            zoom -= zoomAmount;
        }
    }

    /* Gets the input from the mouse to control camera*/
    private void MouseMovementInput()
    {
        //Zooms the camera in using scroll wheel
        if(Input.mouseScrollDelta.y != 0 && Input.GetKey(KeyCode.LeftShift))
        {
            zoom += Input.mouseScrollDelta.y * fastZoomAmount;
        }
        else if (Input.mouseScrollDelta.y != 0)
        {
            zoom += Input.mouseScrollDelta.y * slowZoomAmount;
        }

        //Left mouse button to get position of mouse using rays
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entryPoint;

            if(plane.Raycast(ray, out entryPoint))
            {
                dragStartPos = ray.GetPoint(entryPoint);
            }
        }
        //Gets the end position of the mouse and moves the camera by the difference of the two points
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entryPoint;

            if (plane.Raycast(ray, out entryPoint))
            {
                dragEndPos = ray.GetPoint(entryPoint);

                pos = transform.position + (dragStartPos - dragEndPos);
            }
        }
        //Gets the position of the mouse of the screen when the right mouse button is clicked
        if (Input.GetMouseButtonDown(1))
        {
            rotStartPos = Input.mousePosition;
        }
        //Gets the end position of the mouse and rotates the camera by the difference of the two points
        if (Input.GetMouseButton(1))
        {
            rotEndPos = Input.mousePosition;

            Vector3 difference = rotStartPos - rotEndPos;

            rotStartPos = rotEndPos;

            rot *= Quaternion.Euler(Vector3.up * (difference.x / 25f));
            rot *= Quaternion.Euler(Vector3.right * (-difference.y / 25f));
        }
    }

    //Resets the positon of the camera back to its initial position
    public void Reset()
    {
        pos = initPos;
        zoom = initZoom;
        rot = initRot;
    }
}
