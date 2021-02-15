using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movement_speed;
    [SerializeField]
    private float rotation_speed;
    [SerializeField]
    private bool FollowsMousePos;
    [SerializeField]
    private bool Q_and_E_to_rotate;
    public Vector3 centerPoint;
    private Rigidbody2D rb;
    public static Vector2 global_movement_dir = Vector2.zero;
    public GameObject[] children;
    public GameObject[] childs;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Start()
    {
        StartCoroutine(onCoroutine());
        //InvokeRepeating("UpdateTarget"), 1f, 0.5f); //0f, 0.01f
        //InvokeRepeating("LaunchProjectile", 2.0f, 0.3f);
    }
    IEnumerator onCoroutine()
        {
            while (true)
            {
                //Debug.Log("OnCoroutine: " + (int)Time.time);
                yield return new WaitForSeconds(0.3f);
                 children = GameObject.FindGameObjectsWithTag("Hero"); //get all children
                centerPoint = GetCenterPoint();
            }
        }
        Vector3 GetCenterPoint()
        {
            if (children.Length == 1)
            {
                return children[0].transform.position;
            }

            var bounds = new Bounds(children[0].transform.position, Vector3.zero);
            for (int i = 0; i < children.Length; i++)
            {
                bounds.Encapsulate(children[i].transform.position);
            }

            return bounds.center; //returns the center point of the GamObject Array
        }
    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            
            //children = GetChildren(this.gameObject);
           
            //foreach (Transform child in transform)
            //    print("Foreach loop: " + child);
        }

        
        //transform.GetChild(0).gameObject;

     

        if (FollowsMousePos) faceTowardsMouse();
        if (Q_and_E_to_rotate) Rotation();

        float y_movement = 0;
        if (Input.GetKey("w"))
        {
            y_movement = 1;
        }
        else
        {
            if (Input.GetKey("s"))
            {
                y_movement = -1;
            }
        }

        float x_movement = 0;
        if (Input.GetKey("d"))
        {
            x_movement = 1;

        }
        else
        {
            if (Input.GetKey("a"))
            {
                x_movement = -1;
            }
        }

        //bool moving = true;

        Vector2 movement = new Vector2(x_movement, y_movement);
        if (movement != Vector2.zero)
        {
            movement = movement.normalized * movement_speed;
            //moving = false;
        }

        rb.velocity = movement;
        global_movement_dir = movement;

        void faceTowardsMouse()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 dir = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

            transform.up = dir;
        }
        void Rotation()
        {
            if (Input.GetKey("q"))
            {
                foreach (var item in children)
                {
                    item.transform.RotateAround(centerPoint, new Vector3(0f, 0f, 1f), rotation_speed * Time.deltaTime);
                }
                //transform.GetChild(0).gameObject.transform.RotateAround(centerPoint, new Vector3(0f, 0f, 1f), rotation_speed * Time.deltaTime);          //(new Vector3(0, 0, rotation_speed) * Time.deltaTime);
            }

            if (Input.GetKey("e"))
            {
                foreach (var item in children)
                {
                    item.transform.RotateAround(centerPoint, new Vector3(0f, 0f, 1f), -rotation_speed * Time.deltaTime);
                }
                //transform.GetChild(0).gameObject.transform.RotateAround(centerPoint, new Vector3(0f, 0f, 1f), -rotation_speed * Time.deltaTime);
            }
        }



        

        

        GameObject[] GetChildren(GameObject parent)
        {
            for (int i = 0; i < parent.transform.childCount; ++i)
            {
                //print(parent.transform.childCount);
                //print(parent.transform.GetChild(i).gameObject.name);
                childs[i] = parent.transform.GetChild(i).gameObject;
            }
            return childs;
        }

        //static List<GameObject> GetAllChilds(this GameObject Go)
        //{
        //    List<GameObject> list = new List<GameObject>();
        //    for (int i = 0; i < Go.transform.childCount; i++)
        //    {
        //        list.Add(Go.transform.GetChild(i).gameObject);
        //    }
        //    return list;
        //}

        //static GameObject[] GetAllChildrenObjects()
        //{
        //    GameObject[] list; //= new GameObject[];
        //    public float hello;
        //    for (int i = 0; i< this.transform.childCount; i++)
        //    {
        //        list[i] = this.transform.GetChild(i).gameObject;

        //    }
        //    return list;
        //}

        
    }
}
