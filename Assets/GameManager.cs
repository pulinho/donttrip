using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;
    public PlayerManager[] players;
    public CameraController cameraController;

    private WaitForSeconds endWait = new WaitForSeconds(2);

    private void Start()
    {
        playersAliveBefore = players.Length;

        SpawnAllPlayers();
        SetCameraTargets();

        // Once the tanks have been created and the camera is using them as targets, start the game.
        StartCoroutine(GameLoop());
    }

    private void SetCameraTargets()
    {
        // Create a collection of transforms the same size as the number of tanks.
        Transform[] targets = new Transform[playersAliveBefore];

        int index = 0;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isAlive)
            {
                targets[index] = players[i].instance.transform;
                index++;
            }
        }

        // These are the targets the camera should follow.
        cameraController.targets = targets;
    }

    private void SpawnAllPlayers()
    {
        // For all the tanks...
        for (int i = 0; i < players.Length; i++)
        {
            // ... create them, set their player number and references needed for control.
            players[i].instance =
                Instantiate(playerPrefab, players[i].spawnPoint.position, players[i].spawnPoint.rotation) as GameObject;
            players[i].playerNumber = i;
            players[i].Setup();
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
            SceneManager.LoadScene(0);
        }
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStarting());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEnding());

        SceneManager.LoadScene(0);

        // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
        /*if (win)
        {
            Debug.Log("new game");
            SceneManager.LoadScene(0);
        }*/
        //else
        //{
        // If there isn't a winner yet, restart this coroutine so the loop continues.
        // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
        //StartCoroutine(GameLoop());
        //}
    }

    private IEnumerator RoundStarting()
    {
        //cameraController.DoStart();

        yield return null;
    }

    private IEnumerator RoundPlaying()
    {
        // As soon as the round begins playing let the players control the tanks.
        //EnableTankControl();

        // While there is not one tank left...
        while (PlayersAlive() > 1)
        {
            // ... return on the next frame.
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        yield return endWait;
    }

    private int playersAliveBefore = 0; // weird

    private int PlayersAlive()
    {
        int playersAlive = 0;
        
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isAlive)
            {
                playersAlive++;
            }
        }

        if(playersAliveBefore != playersAlive)
        {
            playersAliveBefore = playersAlive;
            SetCameraTargets();
        }
        
        return playersAlive;
    }
}
