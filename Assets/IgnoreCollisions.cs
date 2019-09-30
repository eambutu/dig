using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    public GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(this.GetComponent<Collider>(), playerObject.GetComponent<CharacterController>());
    }

    void OnCollisionEnter(Collision collision)
    {
        Physics.IgnoreCollision(this.GetComponent<Collider>(), playerObject.GetComponent<CharacterController>());
        Debug.Log(collision.gameObject.name);
    }
}
