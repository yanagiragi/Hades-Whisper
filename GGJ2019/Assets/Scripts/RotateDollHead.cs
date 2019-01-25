using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDollHead : MonoBehaviour
{
    public GameObject head1, head2, head3;

    public float speed;

    public float diff;

    public Vector3 origin;

    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        origin = head1.transform.localPosition + head2.transform.localPosition + head3.transform.localPosition;
        origin /= 3.0f;

        radius = Mathf.Sqrt((origin - head1.transform.localPosition).sqrMagnitude);
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Time.time * speed;

        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);
        head1.transform.localPosition = origin + new Vector3(x, y, 0);

        x = radius * Mathf.Cos(angle + diff);
        y = radius * Mathf.Sin(angle + diff);
        head2.transform.localPosition = origin + new Vector3(x, y, 0);

        x = radius * Mathf.Cos(angle + diff * 2);
        y = radius * Mathf.Sin(angle + diff * 2);
        head3.transform.localPosition = origin + new Vector3(x, y, 0);
    }

}
