using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    public GameObject headObject;
    public GameObject groundObject;
    private GameObject shovelObject;
    private bool isUnderground;
    private const double groundThreshold = 0.2;

    // Start is called before the first frame update
    void Start()
    {
        shovelObject = this.gameObject;
        isUnderground = false;
    }

    // Update is called once per frame
    void Update()
    {
        double currentY = shovelObject.transform.localPosition.y + headObject.transform.localPosition.y;
        if (isUnderground && currentY >= groundObject.transform.localPosition.y)
        {
            isUnderground = false;
            groundObject.transform.SetPositionAndRotation(groundObject.transform.localPosition + Vector3.down, groundObject.transform.localRotation);
        }
        else if (!isUnderground && currentY < groundObject.transform.localPosition.y - groundThreshold)
        {
            isUnderground = true;
        }
    }
}
