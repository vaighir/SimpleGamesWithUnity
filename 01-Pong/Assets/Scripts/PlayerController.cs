using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject topLeft;
    [SerializeField] GameObject bottomRight;
    [SerializeField] GameObject cameraMin;
    [SerializeField] GameObject cameraMax;
    private Vector3 position;
    private float movementSpeed = 10f;
    private bool upOk, downOk;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        Center();
    }

    // Update is called once per frame
    void Update()
    {
       Move();
    }

    private void Center()
    {
        transform.position = new Vector3(position.x, 0, position.z);
    }

    private void Move()
    {
        upOk = topLeft.transform.position.y < cameraMin.transform.position.y;
        downOk = bottomRight.transform.position.y > cameraMax.transform.position.y;

        if (position.x < 0 && Input.GetKey("w") && upOk)
        {
            transform.position = transform.position + new Vector3(0, movementSpeed * Time.deltaTime, 0);
        } else if (position.x < 0 && Input.GetKey("s") && downOk)
        {
            transform.position = transform.position + new Vector3(0, -movementSpeed * Time.deltaTime, 0);
        } else if (position.x > 0 && Input.GetKey(KeyCode.UpArrow) && upOk)
        {
            transform.position = transform.position + new Vector3(0, movementSpeed * Time.deltaTime, 0);
        } else if (position.x > 0 && Input.GetKey(KeyCode.DownArrow) && downOk)
        {
            transform.position = transform.position + new Vector3(0, -movementSpeed * Time.deltaTime, 0);
        }
    }
}
