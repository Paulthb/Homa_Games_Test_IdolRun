using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage coin object
/// </summary>
public class CoinScript : MonoBehaviour
{
    [SerializeField] private GameObject CoinParticle;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    //each coin recovered informs the player and the GameManager and instantiate a particule before destroy himself.
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerPivot")
        {
            Instantiate(CoinParticle, transform.position, Quaternion.identity);
            GameManager.Instance.AddCoin();
            PlayerScript.Instance.RegainSpeed();
            Destroy(this.gameObject);
        }
    }
}
