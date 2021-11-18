using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the destruction of all object left behind the player
/// </summary>
public class DestroyGroundScript : MonoBehaviour
{
    //Normally this task was handle by GroundTriggerScript and like this I could destroy all parent and there child,
    //but instantiate all object as child of a roadSegment had the effect to clone the child too. And so, I had clone of clone of clone of segment with all there object spawn previously.

    void OnTriggerEnter(Collider other)
    {
        if(GameManager.Instance.actualPhase == GameManager.GAMEPHASE.RUN)
            Destroy(other.gameObject);
    }
}
