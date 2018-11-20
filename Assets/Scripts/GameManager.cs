using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static int currentScene = 0;

    public GameObject playerPrefab;
    public PlayerManager[] players;
    public CameraController cameraController;

    private int playersAlive = 0;
    private bool firstBlood = false;

    private readonly WaitForSeconds endWait = new WaitForSeconds(2);

    private void Start()
    {
        Input.gyro.enabled = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        playersAlive = players.Length;

        SpawnAllPlayers();
        SetCameraTargets();
        
        StartCoroutine(GameLoop());
    }

    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[playersAlive];

        int index = 0;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isAlive)
            {
                targets[index] = players[i].instance.transform;
                index++;
            }
        }
        
        cameraController.targets = targets;
    }

    private void SpawnAllPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].instance =
                Instantiate(playerPrefab, players[i].spawnPoint.position, players[i].spawnPoint.rotation) as GameObject;
            players[i].playerNumber = i;
            players[i].Setup();

            var rb = players[i].instance.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.forward * 8000);
        }
    }

    public void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (Input.GetButton("Reset"))
        {
            SceneManager.LoadScene(0); // (++currentScene % 4);
        }
        UpdateGyroTilt();
    }

    public Transform phoneDummy;
    public static float gyroTilt;

    private void UpdateGyroTilt()
    {
        var gyroEuler = Input.gyro.attitude.eulerAngles;
        phoneDummy.transform.eulerAngles = new Vector3(-1.0f * gyroEuler.x, -1.0f * gyroEuler.y, gyroEuler.z);

        var upVec = phoneDummy.transform.InverseTransformDirection(-1f * Vector3.forward);
        gyroTilt = upVec.x * -1;
    }
    
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        SceneManager.LoadScene(++currentScene % 4);
    }

    private IEnumerator RoundStarting()
    {
        yield return null;
    }

    private IEnumerator RoundPlaying()
    {
        while (CountPlayersAlive() > 0)
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isAlive)
            {
                PlayerManager.playerHeightMultiplier[i] += 0.01f;
                break;
            }
        }
        yield return endWait;
    }

    private int CountPlayersAlive()
    {
        int playersAliveNow = 0;
        
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isAlive)
            {
                playersAliveNow++;
            }
            else if (!firstBlood)
            {
                firstBlood = true;
                PlayerManager.playerHeightMultiplier[i] -= 0.01f;
            }
        }

        if(playersAlive != playersAliveNow)
        {
            playersAlive = playersAliveNow;
            SetCameraTargets();
        }
        
        return playersAliveNow;
    }
}
