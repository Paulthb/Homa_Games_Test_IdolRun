using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Script allows you to build the level as you go along by creating segment of road little by little
/// </summary>
public class GroundTriggerScript : MonoBehaviour
{
    //prefab of the road to create
    [SerializeField] private GameObject groundSegment;
    //useful for the first segment of the game
    [SerializeField] bool canTriggerDestroy = true;

    //to order the object create in the scene hierarchy
    private GameObject roadContainer;

    // Start is called before the first frame update
    void Start()
    {
        roadContainer = GameObject.Find("MainRoad");
    }

    public void OnTriggerEnter(Collider other)
    {
        //when reach a certain point, we create the next segment of road to go on
        if (other.gameObject.tag == "RunningPivot")
        {
            Instantiate(groundSegment, transform.position, Quaternion.identity, roadContainer.transform);
        }
        //past a certain point, we delete the segment of road behind us
        else if (other.gameObject.tag == "DestroyGround" && canTriggerDestroy)
        {
            GameObject objectToDestroy = roadContainer.transform.GetChild(0).gameObject;
            Destroy(objectToDestroy);
        }
    }
}
