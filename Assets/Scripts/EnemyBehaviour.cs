using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float movementSpeed;

    private new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        movementSpeed = movementSpeed * Time.deltaTime;
    }

    void Update()
    {
        Vector3 newPosition = transform.position + (Vector3.forward * movementSpeed);
        rigidbody.MovePosition(newPosition);
        transform.LookAt(newPosition);
    }
}
