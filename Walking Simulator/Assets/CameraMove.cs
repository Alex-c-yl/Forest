using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float yaw = 0;                                  // Yaw angle, the angle of rotation about the y-axis
    float pitch = 0;                                // Pitch angle, the angle of rotation around the Z axis
    const float roll = 0;                           // Roll angle, the angle of rotation about the x-axis;
                                                    // Because the change of this value can easily cause discomfort to players, I disabled it. 
    float THeight = 0;                              // The height of Terrain at the position
    Vector3 pos0 = new Vector3 (0, 0, 0);           // The position of camera(player) in the last frame
    bool FlyMode = true;                            // The camera(player) can fly or not
    float gravity = 0.1f;                           // The gravity; It only work without fly mode.
    float fallSpeed = 0;                            // The fall speed of camera(player)
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* Move and View Implementation */

        // Move forward or backward
        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.ClampMagnitude(transform.forward, 0.025f);
        else if (Input.GetKey(KeyCode.S))
            transform.position -= Vector3.ClampMagnitude(transform.forward, 0.025f);

        // Move left or right
        if (Input.GetKey(KeyCode.A))
            transform.position -= Vector3.ClampMagnitude(transform.right, 0.025f);
        else if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.ClampMagnitude(transform.right, 0.025f);

        // Move up or down
        if (Input.GetKey(KeyCode.Space))
            transform.position += new Vector3(0, 0.025f, 0);
        else if (Input.GetKey(KeyCode.LeftShift))
            transform.position -= new Vector3(0, 0.025f, 0);

        // Rotate with mouse
        pitch -= Input.GetAxis("Mouse Y") * 5;
        pitch = Mathf.Clamp(pitch, -90, 90);
        yaw += Input.GetAxis("Mouse X") * 5;
        transform.rotation = Quaternion.Euler(pitch, yaw, roll);

        /* Basic Physics Engine */

        // Terrain position check
        THeight = Terrain.activeTerrain.SampleHeight(transform.position);

        // Flight mode switching
        if (Input.GetKeyDown(KeyCode.F))
            if (!FlyMode)
            {
                FlyMode = true;
                fallSpeed = 0;
            }
            else if (FlyMode)
                FlyMode = false;

        // Fly / Walk implementation
        if (FlyMode)
        {
            // Set height limitation
            if (transform.localPosition.y > 50.0f)
                transform.position = new Vector3(this.transform.localPosition.x, 50.0f, this.transform.localPosition.z);
            // Prevent clipping
            if (transform.localPosition.y < THeight + 1.0f)
                transform.position = new Vector3(this.transform.localPosition.x, THeight + 1.0f, this.transform.localPosition.z);
        }
        if (!FlyMode)
        {
            if (transform.localPosition.y > THeight + 1.0f)
            {
                // Ban fly up or accelerately fall when fall movement is allowed
                transform.position = new Vector3(this.transform.localPosition.x, pos0.y, this.transform.localPosition.z);
                // Gravity effect
                fallSpeed = fallSpeed + gravity * Time.deltaTime;
                // Calculate final position
                transform.position = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - fallSpeed, this.transform.localPosition.z);
            }

            if (transform.localPosition.y < THeight + 1.0f)
            {
                // Prevent clipping
                transform.position = new Vector3(this.transform.localPosition.x, THeight + 1.0f, this.transform.localPosition.z);
                fallSpeed = 0 ;
            }

        }
        // Map limitation implementation
        if (transform.localPosition.x < 0)
            transform.position = new Vector3(0, this.transform.localPosition.y, this.transform.localPosition.z);
        if (transform.localPosition.x > 60.0f)
            transform.position = new Vector3(60.0f, this.transform.localPosition.y, this.transform.localPosition.z);
        if (transform.localPosition.z < 0)
            transform.position = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0);
        if (transform.localPosition.z > 60.0f)
            transform.position = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 60.0f);

        pos0 = transform.position;
    }
    
}
