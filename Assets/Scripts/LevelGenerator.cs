using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public GameObject ground;
    public Transform[] spawnPoint;
    public Transform directionalLight;

    private void Awake()
    {
        var groundScaleVec = new Vector3(Random.Range(3f, 5f), 1, 3);
        ground.transform.localScale = groundScaleVec;

        var randomRotate = Random.Range(-30f, 30f);
        ground.transform.Rotate(Vector3.up * randomRotate);

        var indexes = RandomizedSpawnIndexes();

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            var xMultiplier = ((i / 2) - 0.5f) * 2;
            var zMultiplier = ((i % 2) - 0.5f) * 2;
            
            spawnPoint[indexes[i]].localPosition = new Vector3(groundScaleVec.x * 4 * xMultiplier, 2, groundScaleVec.z * 4 * zMultiplier);
            spawnPoint[indexes[i]].RotateAround(Vector3.zero, Vector3.up, randomRotate);
        }

        ground.SetColor(Random.ColorHSV(0, 1, 0, 0.25f, 0.75f, 1, 1, 1));

        directionalLight.localEulerAngles = new Vector3(Random.Range(40f, 140f), Random.Range(0f, 360f), 0);

    }

    // for shuffling spawn positions
    private int[] RandomizedSpawnIndexes()
    {
        System.Random rnd = new System.Random();
        return Enumerable.Range(0, spawnPoint.Length).OrderBy(r => rnd.Next()).ToArray();
    }
}
