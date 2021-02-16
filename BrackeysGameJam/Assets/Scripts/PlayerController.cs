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
    private Vector3 centerPoint;
    private Rigidbody2D rb;
    public static Vector2 global_movement_dir = Vector2.zero;
    private GameObject[] children;
    private GameObject[] childs;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(PlacecenterPoint());
    }
    IEnumerator PlacecenterPoint()
    {
        while (true)
        {
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
        if (FollowsMousePos && Input.GetMouseButton(0)) faceTowardsMouse();
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

        Vector2 movement = new Vector2(x_movement, y_movement);
        if (movement != Vector2.zero)
        {
            movement = movement.normalized * movement_speed;
        }
        rb.velocity = movement;
        global_movement_dir = movement;


    }
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




        //Should Actually get the children but somehow doesnt work :/

        // GameObject[] GetChildren(GameObject parent)
        // {
        //     for (int i = 0; i < parent.transform.childCount; ++i)
        //     {
        //         childs[i] = parent.transform.GetChild(i).gameObject;
        //     }
        //     return childs;
        // }
    
}
