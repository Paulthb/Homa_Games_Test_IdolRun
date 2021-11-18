using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the main camera of the game 
/// </summary>
public class CameraFollow : MonoBehaviour
{
    //the camera will stay on the same speed that the running pivot
    [SerializeField] private Transform runningPivot;
    //To follow the player when punched
    [SerializeField] private Transform playerPivot;

    //position the camera must go to to start the run 
    [SerializeField] private Transform cameraRacePos;

    
    private Vector3 offset;
    private Transform currentTarget;
    private bool followPlayer = false;

    private bool cameraOnPos = false;

    #region Singleton

    private static CameraFollow _instance;
    public static CameraFollow Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<CameraFollow>();
            }

            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        offset = cameraRacePos.position - runningPivot.position;
        currentTarget = runningPivot;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraOnPos)
        {
            Vector3 TargetPos = currentTarget.position + offset;
            if (!followPlayer) TargetPos.x = 0;
            transform.position = TargetPos;
        }
    }

    public void GoToRacePosition()
    {
        StartCoroutine(MoveToSpot());
    }

    IEnumerator MoveToSpot()
    {
        float elapsedTime = 0;
        float waitTime = 2f;
        Vector3 currentPos = transform.position;
        Quaternion currentRot = transform.rotation;

        //Lerp in coroutine allow to have an easy control of a movement on time
        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPos, cameraRacePos.position, (elapsedTime / waitTime));
            transform.rotation = Quaternion.Lerp(currentRot, cameraRacePos.rotation, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = cameraRacePos.position;
        transform.rotation = cameraRacePos.rotation;

        cameraOnPos = true;

        GameManager.Instance.StartGame();
        yield return null;
    }


    public void FollowPunchedPlayer()
    {
        followPlayer = true;
        currentTarget = playerPivot;
    }
}
