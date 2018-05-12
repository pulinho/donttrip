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

        ground.SetColor(Random.ColorHSV(0, 1, 0, 0.1f, 0.9f, 1, 1, 1));

        directionalLight.localEulerAngles = new Vector3(Random.Range(45f, 60f), Random.Range(-45f, 45f), 0);

        PlaceObjects();
    }

    private void PlaceObjects()
    {
        for (int i = 0; i < 15; i++)
        {
            PlaceRandomPrimitive();
        }
    }

    private void PlaceRandomPrimitive()
    {
        var type = Random.Range(0, 2);
        var instance = GameObject.CreatePrimitive((type == 0) ? PrimitiveType.Cube : PrimitiveType.Sphere);

        Vector3 position;
        do {
            position = new Vector3(Random.Range(-14f, 14f), 2, Random.Range(-14f, 14f));
        }
        while (Mathf.Abs(position.magnitude - spawnPoint[0].localPosition.magnitude) < 2);

        instance.transform.position = position;

        var scale = Random.Range(1f, 2.5f);
        instance.transform.localScale = new Vector3(scale, scale, scale);
        instance.transform.Rotate(Vector3.up * Random.Range(0f, 360f));
        instance.SetColor(Random.ColorHSV(0, 1, 0, 0.1f, 0.9f, 1, 1, 1));

        var rb = instance.AddComponent<Rigidbody>();
        rb.mass = scale;
    }

    // for shuffling spawn positions
    private int[] RandomizedSpawnIndexes()
    {
        System.Random rnd = new System.Random();
        return Enumerable.Range(0, spawnPoint.Length).OrderBy(r => rnd.Next()).ToArray();
    }
}
