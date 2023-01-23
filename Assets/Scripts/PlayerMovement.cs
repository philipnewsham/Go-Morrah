using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float upwardsForce = 0;
    public float gravity = 3;
    public float gravityCap = -7;
    public float jumpAmount = 30;
    public float horizontalSpeed = 5;
    private void Update()
    {
        int x = Mathf.RoundToInt(Mathf.Clamp01(Input.GetAxisRaw("Horizontal")) * horizontalSpeed);
        //int y = Mathf.RoundToInt(upwardsForce);
        transform.position += new Vector3(x, 0, 0) * Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpAmount);
        }

        //upwardsForce = Mathf.Max((upwardsForce - (gravity * Time.deltaTime)), gravityCap);
    }
}
