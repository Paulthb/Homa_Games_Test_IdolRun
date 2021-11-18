using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the enemy fan on the road
/// </summary>
public class FollowingFanScript : MonoBehaviour
{
    Transform target;
    [SerializeField] private float speed = 5;
    [SerializeField] private float rotateSpeed = 10;
    [SerializeField] private GameObject catchedParticle;

    private Rigidbody rb;
    private bool isPlayerAtSight = false;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    //When the enemy fan go to the player, they are slower and cannot make big rotate so they are easy to avoid
    void FixedUpdate()
    {
        if(isPlayerAtSight)
        {
            rb.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotateSpeed * Time.deltaTime);
            rb.velocity = transform.forward * speed;
            anim.SetBool("IdolAtSight", true);
        }
    }

    //it's only at a certain distance that the enemy fan will go on the player
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerPivot")
        {
            target = other.transform;
            isPlayerAtSight = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void PlayerCatched()
    {
        anim.SetBool("Catched", true);
        Instantiate(catchedParticle, transform.position, Quaternion.identity);
    }
}
