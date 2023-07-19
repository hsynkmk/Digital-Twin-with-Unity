using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMover : MonoBehaviour
{
    public float speed = 1f;
    enum Direction { Forward, Backward }

    [SerializeField] Direction chosenVec;

    Rigidbody myRigidbody;
    Material myMaterial;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        ScrollUV();
    }
    void FixedUpdate()
    {
        if (chosenVec == Direction.Forward)
        {
            Vector3 posB = myRigidbody.position;
            myRigidbody.position += Vector3.back * speed * Time.fixedDeltaTime;
            myRigidbody.MovePosition(posB);
        }

        else if (chosenVec == Direction.Backward)
        {
            Vector3 posU = myRigidbody.position;
            myRigidbody.position += Vector3.forward * speed * Time.fixedDeltaTime;
            myRigidbody.MovePosition(posU);
        }
    }

    void ScrollUV()
    {
        var material = myMaterial;
        Vector2 offset = material.mainTextureOffset;
        offset += Vector2.left * speed * Time.deltaTime / material.mainTextureScale.y;
        material.mainTextureOffset = offset;
    }
}
