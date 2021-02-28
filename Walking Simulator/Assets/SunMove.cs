using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMove : MonoBehaviour
{
    public float speed = 0.01f;
    private bool pause = false;
    private float cache = 0.00f;
    private int degree;

    // Start is called before the first frame update
    void Start()
    {
        degree = Random.Range(-180, 181);
        gameObject.transform.Rotate(degree, 0, 0);

        // If the sun is under the horizon, there is no light at all
        if (this.transform.localRotation.x <= 0 && this.transform.localRotation.x >= -1)
            GetComponent<Light>().intensity = 0;
        // If the sun is upper than the horizon, the light intensity is changed with sun's place
        if (this.transform.localRotation.x <= 1 && this.transform.localRotation.x > 0)
            GetComponent<Light>().intensity = gameObject.transform.localEulerAngles.x / 30;
    }

    // Update is called once per frame
    void Update()
    {
        // The sun move main function implementation
        this.transform.Rotate(-speed * Mathf.Cos(Time.deltaTime * Mathf.Deg2Rad), 0, 0);

        // Keep Rotate X value in (-180,180] 
        if (this.transform.localRotation.x <= -1)
            this.transform.Rotate(360, 0, 0);

        // If the sun is under the horizon, there is no light at all
        if (this.transform.localRotation.x <= 0 && this.transform.localRotation.x >= -1)
            GetComponent<Light>().intensity = 0;

        // If the sun is upper than the horizon, the light intensity is changed with sun's place
        if (this.transform.localRotation.x <= 1 && this.transform.localRotation.x > 0)
            GetComponent<Light>().intensity = this.transform.localEulerAngles.x / 30;

        // Pause sun move
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!pause)
            {
                pause = true;
                cache = speed;
                speed = 0.00f;
            }
            else if (pause) 
            {
                pause = false;
                speed = cache;
                cache = 0.00f; 
            }

        }
        // change sun move speed

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            pause = false;
            cache = 0.00f;
            speed = 0.01f;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            pause = false;
            cache = 0.00f;
            speed = 0.02f;
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            pause = false;
            cache = 0.00f;
            speed = 0.03f;
        }

    }
}
