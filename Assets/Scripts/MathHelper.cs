using UnityEngine;
using System.Collections;

public class MathHelper : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 Truncate(Vector3 input, float max)
    {
        float i = 0.0f;
        float length = Vector3.Magnitude(input);

        i = max/length;
        i = i < 1.0f ? i : 1.0f;

        Vector3 v = Vector3.zero;
        v = v*i;

        return v;
    }
}
