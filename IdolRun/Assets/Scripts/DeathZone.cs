using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the GameOver of the player if he lost to much distance and the delete of the avoid enemy fan.
/// </summary>
public class DeathZone : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "PlayerPivot")
        {
            GameManager.Instance.GameOver();
        }
        if (other.gameObject.tag == "EnemyFan")
        {
            Destroy(other.gameObject);
        }
    }
}
