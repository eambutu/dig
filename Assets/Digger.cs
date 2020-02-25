using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    public GameObject headObject;
    public GameObject groundObject;
    public int holeCenterX;
    public int holeCenterZ;
    public int holeRadius;
    public AudioClip shovelEnterSound;
    public AudioClip shovelExitSound;
    private GameObject shovelObject;
    private Terrain terrain;
    private AudioSource audioSource;
    private bool isUnderground;
    private const double groundThreshold = 0.2;
    private float contactX;
    private float contactZ;

    private float[,] mask = new float[5, 5] { { 0.95f, 0.96f, 0.98f, 0.96f, 0.95f }, { 0.96f, 0.98f, 0.99f, 0.98f, 0.96f }, { 0.98f, 0.99f, 1f, 0.99f, 0.98f }, { 0.96f, 0.98f, 0.99f, 0.98f, 0.96f }, { 0.95f, 0.96f, 0.98f, 0.96f, 0.95f } };

    // Start is called before the first frame update
    void Start()
    {
        shovelObject = this.gameObject;
        isUnderground = false;
        terrain = groundObject.GetComponent<Terrain>();
        audioSource = GetComponent<AudioSource>();

        float[,] heights = new float[100, 100];
        for (int i=0; i<100; i++)
        {
            for (int j=0; j<100; j++)
            {
                heights[i, j] = 0.95f;
            }
        }
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    bool inHoleRadius(float x, float z)
    {
        return Mathf.Pow(x - holeCenterX, 2) + Mathf.Pow(z - holeCenterZ, 2) <= Mathf.Pow(holeRadius, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = shovelObject.transform.localPosition + headObject.transform.localPosition;
        float currentHeight = terrain.terrainData.GetInterpolatedHeights(currentPos.x / (float)terrain.terrainData.size.x, currentPos.z / (float)terrain.terrainData.size.z, 1, 1, 1, 1)[0, 0];
        Debug.Log("currentPos.y: " + currentPos.y);
        Debug.Log("currentHeight: " + currentHeight);
        Debug.Log("ground height: " + (currentHeight + groundObject.transform.localPosition.y - groundThreshold));
        if (isUnderground && currentPos.y >= currentHeight + groundObject.transform.localPosition.y)
        {
            isUnderground = false;

            int xCoor = holeCenterX - holeRadius;
            int zCoor = holeCenterZ - holeRadius;

            float[,] heights = terrain.terrainData.GetHeights(xCoor / 2, zCoor / 2, 2 * holeRadius / 2, 2 * holeRadius / 2);
            float[,] newHeightFloats = new float[2 * holeRadius / 2, 2 * holeRadius / 2];
            Debug.Log("heights before " + heights[0, 0]);
            for (int i = 0; i < 2 * holeRadius / 2; i++)
            {
                for (int j = 0; j < 2 * holeRadius / 2; j++)
                {
                    newHeightFloats[i, j] = heights[i, j];
                    if (inHoleRadius(i * 2 + xCoor, j * 2 + zCoor))
                    {
                        newHeightFloats[i, j] = heights[i, j] - (5f / 3000f);
                    }
                }
            }
            terrain.terrainData.SetHeights(xCoor / 2, zCoor / 2, newHeightFloats);
            audioSource.PlayOneShot(audioSource.clip);
        }
        else if (!isUnderground && currentPos.y < currentHeight + groundObject.transform.localPosition.y - groundThreshold)
        {
            if (inHoleRadius(currentPos.x, currentPos.z))
            {
                isUnderground = true;
            }
        }
    }
}
