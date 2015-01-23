using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public GameObject Target = null;

    void Start()
    {
        // Empty
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, Camera.main.transform.position.z);
    }
}
