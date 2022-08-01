using UnityEngine;

public class RapierController : MonoBehaviour, ITickUpdate
{
    public GameObject rapierRef;
    public Transform shotPoint;

    public Player prince;
    public Transform playerRapier;
    public Transform rapierTransform;
    public float offset;
    public float distance;
    public float canDelay;

    internal bool dropRapier;
    internal bool throwing;
    private float nextDelay;
    internal GameObject rapier;

    private Camera mainCamera;
    internal bool inTheAdditionalTargetAreaOfTheCamera;

    internal AirRapController airController;
    public LayerMask wtIsSolid;
    public delegate void RapierEvent();
    public RapierEvent DropRaier;
    public RapierEvent ReturnedRaier;
    public AudioClip audioClip;
    private AudioSource audioSource;


    private void Start()
    {
        prince = Game.player;
        mainCamera = Camera.main;
        dropRapier = false;
        Game.endGame += EndGame;
        UpdateManager.ticks.Add(this);
        airController = new AirRapController();

        airController = new AirRapController();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
    }




    public void OnUpdate()
    {
        bool canThrow = nextDelay < Time.time;
        //float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //int snappedAngle = (int)(angle / 90.0f) * 90;
        //transform.rotation = Quaternion.AngleAxis(snappedAngle, Vector3.forward);

        KeyController(canThrow);


        if (!inTheAdditionalTargetAreaOfTheCamera && (!dropRapier || Vector2.Distance(prince.transform.position, rapier.transform.position) > 15))
        {
            rapierTransform.position = prince.transform.position;
        }
        else if (dropRapier)
        {
            airController.OnUpdate(rapier, wtIsSolid);
            rapierTransform.position = rapier.transform.position;
        }
    }


    private void KeyController(bool canThrow)
    {
        if (Input.GetButtonUp("Fire2") && !prince.hitDamage.isItInvulnerable)
        {
            if (!dropRapier && canThrow)
            {
                Vector3 difference = Input.GetJoystickNames().Length > 1 ? new Vector3(-Input.GetAxis("vertical"), -Input.GetAxis("horizontal"), 0) :
                    Input.mousePosition - mainCamera.WorldToScreenPoint(transform.position);
                //Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                difference = difference == Vector3.zero ? new Vector3(prince.direction, 0, 0) : difference;

                float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);


                rapier = Instantiate(rapierRef, transform.position, transform.rotation);
                airController.Init();
                airController.Restart();
                rapier.transform.position = transform.position;
                rapier.transform.rotation = transform.rotation;
                nextDelay = Time.time + canDelay;
                //Create();
                playerRapier.gameObject.SetActive(false);
                dropRapier = true;
                DropRaier?.Invoke();
                audioSource.Play();
            }
            else if (dropRapier)
            {
                dropRapier = false;
                prince.transform.position = airController.point;
                playerRapier.gameObject.SetActive(true);
                dropRapier = false;
                Destroy(rapier);
                ReturnedRaier?.Invoke();
            }
        }

        if (dropRapier && rapier)
        {
            if (Input.GetButton("Fire3") || airController.isCameBack)
            {
                Destroy(rapier);
                playerRapier.gameObject.SetActive(true);
                dropRapier = false;
                ReturnedRaier?.Invoke();
            }
        }
    }


    private void EndGame()
    {
        UpdateManager.ticks.Remove(this);
        Game.endGame -= EndGame;
    }
}
