using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMover : MonoBehaviour
{
    public float speed = 1f;
    enum Direction { Forward, Backward }

    [SerializeField] Direction direction;
    Rigidbody beltRigidbody;
    Material beltMaterial;

    void Start()
    {
        beltRigidbody = GetComponent<Rigidbody>();
        beltMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        ScrollTexture();
    }
    void FixedUpdate()
    {
        if (direction == Direction.Forward)
        {
            Vector3 posB = beltRigidbody.position;
            beltRigidbody.position += Vector3.back * speed * Time.fixedDeltaTime;
            beltRigidbody.MovePosition(posB);
        }


        else if (direction == Direction.Backward)
        {
            Vector3 posU = beltRigidbody.position;
            beltRigidbody.position += Vector3.forward * speed * Time.fixedDeltaTime;
            beltRigidbody.MovePosition(posU);
        }
    }

    void ScrollTexture()
    {
        Vector2 offset = beltMaterial.mainTextureOffset;
        offset += Vector2.left * speed * Time.deltaTime / beltMaterial.mainTextureScale.y;
        beltMaterial.mainTextureOffset = offset;
    }
}
