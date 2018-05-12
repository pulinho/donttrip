using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public GameObject ground;
    public Transform[] spawnPoint;
    public Transform directionalLight;
    public GameObject[] gunPrefab;

    private void Awake()
    {
        var groundScaleVec = new Vector3(Random.Range(30f, 50f), 1, 30);
        ground.transform.localScale = groundScaleVec;

        var randomRotate = Random.Range(-10f, 10f);
        ground.transform.Rotate(Vector3.up * randomRotate);

        var indexes = RandomizedSpawnIndexes();

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            var xMultiplier = ((i / 2) - 0.5f) * 2;
            var zMultiplier = ((i % 2) - 0.5f) * 2;
            
            spawnPoint[indexes[i]].localPosition = new Vector3(groundScaleVec.x * 0.4f * xMultiplier, 2, groundScaleVec.z * 0.4f * zMultiplier);
            spawnPoint[indexes[i]].RotateAround(Vector3.zero, Vector3.up, randomRotate);
        }

        ground.SetColor(Random.ColorHSV(0, 1, 0, 0.1f, 0.9f, 1, 1, 1));

        directionalLight.localEulerAngles = new Vector3(Random.Range(45f, 60f), Random.Range(-45f, 45f), 0);

        PlaceObjects();
        PlaceGuns();
    }

    private void PlaceGuns()
    {
        for (int i = 0; i < 8; i++)
        {
            PlaceRandomGun(i % gunPrefab.Length);
        }
    }

    private void PlaceRandomGun(int prefabIndex)
    {
        var instance = Instantiate(gunPrefab[prefabIndex], new Vector3(Random.Range(-12f, 12f), 40, Random.Range(-12f, 12f)), Quaternion.identity) as GameObject;
        var rb = instance.GetComponent<Rigidbody>();
        rb.AddTorque(Random.insideUnitSphere);
    }

    private void PlaceObjects()
    {
        for (int i = 0; i < 15; i++)
        {
            PlaceRandomPrimitive(i % 3);
        }
    }

    private void PlaceRandomPrimitive(int type)
    {
        var instance = GameObject.CreatePrimitive((type == 0) ? PrimitiveType.Sphere : PrimitiveType.Cube);

        Vector3 position;
        do {
            position = new Vector3(Random.Range(-14f, 14f), 2, Random.Range(-14f, 14f));
        }
        while (Mathf.Abs(position.magnitude - spawnPoint[0].localPosition.magnitude) < 2);

        instance.transform.position = position;

        var scale = Random.Range(1f, 2.5f);
        if (type < 2)
        {
            instance.transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            instance.transform.localScale = new Vector3(Random.Range(0.5f, 3f), Random.Range(0.5f, 3f), Random.Range(0.5f, 3f));
        }

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
