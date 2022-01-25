using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMover : MonoBehaviour
{
    private Rigidbody rigid = null;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void MoveTo(Vector3 newPos)
    {
        rigid.MovePosition(newPos);
    }
}
