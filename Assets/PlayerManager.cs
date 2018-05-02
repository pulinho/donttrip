using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [HideInInspector] public bool isAlive;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public GameObject instance;
    public Color playerColor;
    public Transform spawnPoint;
    public Transform gunPoint;

    private Transform gunRotation;
    private Quaternion shiftRotation;

    private PlayerMovement movement;
    private Gun gun;

    public void Setup()
    {
        isAlive = true;
        
        movement = instance.GetComponent<PlayerMovement>();
        
        gunRotation = instance.transform.Find("Body").transform.Find("GunRotation");
        gunPoint = gunRotation.transform.Find("GunPoint");

        movement.playerNumber = playerNumber;
        movement.gunPoint = gunPoint;

        setColor(playerColor);
    }

    void Update()
    {
        if (!movement.isAlive)
        {
            setColor(Color.white);
            isAlive = false;
            DisableControl();
        }
    }

    private void FixedUpdate()
    {
        var angle = Vector3.Angle(Vector3.up, instance.transform.up);
        if(angle == 0)
        {
            shiftRotation = Quaternion.Inverse(Quaternion.FromToRotation(Vector3.forward, instance.transform.forward));
        }

        //Debug.Log(playerNumber + " ang: " + instance.transform.rotation.y);
        
        var horizontal = Input.GetAxis("RightStickHorizontal" + playerNumber);
        var vertical = Input.GetAxis("RightStickVertical" + playerNumber);

        //var angleShift = Quaternion.Angle(transform.rotation, instance.transform.rotation);
        //Debug.Log(playerNumber + " angShift: " + angleShift);

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 lookDirection = new Vector3(horizontal, 0, vertical);

            gunRotation.rotation = instance.transform.rotation * shiftRotation * Quaternion.LookRotation(lookDirection);
        }
    }

    // Used during the phases of the game where the player shouldn't be able to control their tank.
    public void DisableControl()
    {
        enabled = false;
        //movement.enabled = false; // necessary?
        //gun.enabled = false;
        //m_CanvasGameObject.SetActive(false);
    }
    
    // Used during the phases of the game where the player should be able to control their tank.
    public void EnableControl()
    {
        enabled = false;
        //movement.enabled = true;
        //gun.enabled = true;
        //m_CanvasGameObject.SetActive(true);
    }
    
    // Used at the start of each round to put the tank into it's default state.
    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;

        instance.SetActive(false);
        instance.SetActive(true);
    }

    private void setColor(Color color)
    {
        MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = color;
        }
    }
}
