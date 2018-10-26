using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;

    Vector3 velocity;
    float angle;
    Rigidbody body;

    void Start()
    {
        body = GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        velocity = direction * speed;
        angle = 90 - (Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg);
        //transform.eulerAngles = Vector3.up * angle;
    }

    void FixedUpdate()
    {
        body.position += velocity * Time.fixedDeltaTime;
        
    }
}
