using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
    public bool freezeZ;
    public Quaternion targetRot;
    void Start()
    {
        targetRot = transform.rotation;
    }
    void Update()
    {
        if (freezeZ)
        {
            transform.rotation = new Quaternion(targetRot.x,targetRot.y, targetRot.z, targetRot.w);
        }
    }
}
