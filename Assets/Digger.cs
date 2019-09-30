using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    public GameObject headObject;
    public GameObject groundObject;
    private GameObject shovelObject;
    private Terrain terrain;
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

        float[,] heights = new float[100, 100];
        for (int i=0; i<100; i++)
        {
            for (int j=0; j<100; j++)
            {
                heights[i, j] = 1f;
            }
        }
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = shovelObject.transform.localPosition + headObject.transform.localPosition;
        float currentHeight = terrain.terrainData.GetHeight((int)currentPos.x, (int)currentPos.z);
        if (isUnderground && currentPos.y >= currentHeight + groundObject.transform.localPosition.y)
        {
            isUnderground = false;

            int xCoor = (int)Mathf.Max(0, currentPos.x - 5);
            int yCoor = (int)Mathf.Max(0, currentPos.z - 5);
            float[,] heights = terrain.terrainData.GetHeights(xCoor, yCoor, 5, 5);
            float[,] newHeightFloats = new float[5, 5];
            Debug.Log(terrain.terrainData.heightmapScale.y);
            Debug.Log("Printing the heights now");
            for (int i=0; i<5; i++)
            {
                for (int j=0; j<5; j++)
                {
                    Debug.Log(heights[i, j]);
                    newHeightFloats[i, j] = heights[i, j] - (mask[i, j] / 600f);
                }
            }
            terrain.terrainData.SetHeights(xCoor, yCoor, newHeightFloats);
        }
        else if (!isUnderground && currentPos.y < currentHeight + groundObject.transform.localPosition.y - groundThreshold)
        {
            isUnderground = true;
        }
    }
}
