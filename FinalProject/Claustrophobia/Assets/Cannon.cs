using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    public GameObject cannonBall;
    public float speed = 40f;
    internal float fireDelay;
    float rateOfFire = 0.5f;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) && Time.time > fireDelay) {
            fireDelay = Time.time + rateOfFire;
            GameObject clone = Instantiate(cannonBall, transform.position, transform.rotation);
            clone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 5f, speed));
            Physics.IgnoreCollision(clone.GetComponent<SphereCollider>(), transform.GetComponent<BoxCollider>());
        }
    }
}
