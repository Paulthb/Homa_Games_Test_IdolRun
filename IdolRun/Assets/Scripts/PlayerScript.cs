using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Player control + player animation
/// </summary>
public class PlayerScript : MonoBehaviour
{
    //moving value on x for player pivot
    private float strafe;

    private Rigidbody rb;
    private TrailRenderer idolTrail;

    //Pos to check a movement has been made
    private float startPosX = 0;
    //Pos to check the value a of the movement
    private float prevTouchX = 0;

    //When punched, player is no longer movable by touch
    private bool isPunched = false;

    //we keep track of this coroutine cause she can be restart several time
    private Coroutine trailActivation;

    //Point of explosion when punch
    [SerializeField] private Transform explosionPoint;
    [SerializeField] private float explosionPower;
    [SerializeField] private float explosionRad;

    //Control animator of Idol
    [SerializeField] private IdolAnimationController animationController;
    //particle when punched by tourniquet
    [SerializeField] private GameObject tourniquetParticle;

    //Singleton pattern
    #region Singleton

    private static PlayerScript _instance;
    public static PlayerScript Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<PlayerScript>();
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
        rb = GetComponent<Rigidbody>();
        idolTrail = GetComponent<TrailRenderer>();
        idolTrail.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkControls();

        if(GameManager.Instance.actualPhase == GameManager.GAMEPHASE.GAMEOVER)
            animationController.UpdateAnimationState("GameOver", true);
    }

    public void FixedUpdate()
    {
        if (!isPunched)
        {
            //offset limit
            if (strafe > 3.5f) strafe = 3.5f;
            if (strafe < -3.5f) strafe = -3.5f;

            Vector3 currentPosition = new Vector3(strafe, transform.position.y, transform.position.z);

            //light rotation when moving
            transform.position = currentPosition;
            transform.rotation = Quaternion.LookRotation(currentPosition);
        }
    }

    void checkControls()
    {
        //Screen

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            float difX = touch.position.x - startPosX;

            switch (touch.phase)
            {
                
                case TouchPhase.Began:
                    //first touch launch the game
                    if (GameManager.Instance.actualPhase == GameManager.GAMEPHASE.WARM_UP)
                    {
                        GameManager.Instance.ReadyCamera();
                    }
                    //last touch restart the game
                    if (GameManager.Instance.actualPhase == GameManager.GAMEPHASE.GAMEOVER)
                    {
                        GameManager.Instance.RestartScene();
                    }

                    prevTouchX = touch.position.x;
                    startPosX = touch.position.x;
                    break;

                case TouchPhase.Moved:
                    if (startPosX != 0 && GameManager.Instance.actualPhase == GameManager.GAMEPHASE.RUN)
                    {
                        difX = (touch.position.x - prevTouchX);
                        prevTouchX = touch.position.x;

                        float step = (difX / Screen.width) * 200.5f * Time.deltaTime;

                        strafe += step;
                    }
                    break;

                case TouchPhase.Ended:
                    startPosX = 0;
                    break;
            }
            
        }

        // Keyboard

        else if (Input.GetMouseButton(0))
        {
            if (GameManager.Instance.actualPhase == GameManager.GAMEPHASE.WARM_UP)
            {
                GameManager.Instance.ReadyCamera();
            }
            if (GameManager.Instance.actualPhase == GameManager.GAMEPHASE.GAMEOVER)
            {
                GameManager.Instance.RestartScene();
            }
            strafe -= 5f * Time.deltaTime;
        }

        if (Input.GetMouseButton(1))
        {
            if (GameManager.Instance.actualPhase == GameManager.GAMEPHASE.WARM_UP)
            {
                GameManager.Instance.ReadyCamera();
            }
            if (GameManager.Instance.actualPhase == GameManager.GAMEPHASE.GAMEOVER)
            {
                GameManager.Instance.RestartScene();
            }
            strafe += 5f * Time.deltaTime;
        }
    }

    //Call when camera is on position
    public void BeginRaceAnimation()
    {
        animationController.UpdateAnimationState("Run", true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TourniquetSphere")
        {
            TourniquetPunch();
        }

        //do second punched particle and avoid player from to spin endlessly
        if (other.gameObject.tag == "OutsideWall")
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Instantiate(tourniquetParticle, transform.position, Quaternion.identity);
        }
    }



    void OnCollisionEnter(Collision other)
    {
        //unfortunately I wasn't able to find a proper way right now to make an explosion at the right point of a collision,
        //because add a rigidbody on the sphere isn't good for the tourniquet himself

        //if (other.gameObject.tag == "TourniquetSphere")
        //{
        //    TourniquetPunch(other.contacts[0].point);
        //}

        
        if (other.gameObject.tag == "EnemyFan")
        {
            StartCoroutine(FanCatch(other.gameObject, transform.position));
            other.gameObject.GetComponent<FollowingFanScript>().PlayerCatched();
        }
    }

    //When punch by tourniquet, the player is project to the wall and the game is over
    public void TourniquetPunch()
    {
        Instantiate(tourniquetParticle, transform.position, Quaternion.identity);

        //to follow the fall of the player
        CameraFollow.Instance.FollowPunchedPlayer();

        animationController.UpdateAnimationState("Punched", true);

        isPunched = true;
        transform.parent = null;
        idolTrail.enabled = false;

        rb.isKinematic = false;
        rb.AddExplosionForce(explosionPower, explosionPoint.position, explosionRad, 0.4f, ForceMode.Impulse);

        //to stop element of the game and prepare the GameOver
        GameManager.Instance.Punched();
    }

    //When a enemy catch the player, he can move for a short time before get rid of him
    public IEnumerator FanCatch(GameObject fanCatcher, Vector3 catchPos)
    {
        animationController.UpdateAnimationState("Catched", true);
        transform.parent = null;

        rb.isKinematic = true;
        transform.position = catchPos;

        yield return new WaitForSeconds(0.5f);
        animationController.UpdateAnimationState("Catched", false);

        Destroy(fanCatcher);

        transform.parent = RunningScript.Instance.gameObject.transform;
        rb.isKinematic = false;
    }

    //the player can make up for lost distance by recovering coins
    public void RegainSpeed()
    {
        if (transform.localPosition.z < 1)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.2f);

            if (trailActivation != null)
                StopCoroutine(trailActivation);

                trailActivation = StartCoroutine(TrailCoroutine());
        }
    }

    public IEnumerator TrailCoroutine()
    {
        idolTrail.enabled = true;
        yield return new WaitForSeconds(1f);
        idolTrail.enabled = false;
    }
}
