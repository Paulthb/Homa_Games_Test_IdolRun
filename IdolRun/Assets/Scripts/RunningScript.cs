using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the running pivot, it's the global speed of the game
/// </summary>
public class RunningScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    #region Singleton

    private static RunningScript _instance;
    public static RunningScript Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<RunningScript>();
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.actualPhase == GameManager.GAMEPHASE.RUN)
            transform.position += Vector3.forward * Time.deltaTime * speed;
    }
}
