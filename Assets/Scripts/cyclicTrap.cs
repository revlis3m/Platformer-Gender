using UnityEngine;

public class CyclicTrap : MonoBehaviour
{
    [SerializeField] private float angle = 0f, rotationSpeed = 90f;
    private bool goingUp = true;

    void Update()
    {
        // Calculate the rotation angle based on time
        float deltaAngle = 0f;

        // Check for the rotation limit and change direction
        if (goingUp)
        {
            deltaAngle = Time.deltaTime * rotationSpeed;
            if(angle >= 90)
            {
                goingUp = false;
                angle = 90f;
            }
        }
        else if (!goingUp)
        {
            deltaAngle = Time.deltaTime * -rotationSpeed;
            if (angle <= -90f)
            {
                goingUp = true;
                angle = -90f;
            }
        }
        // Rotate the object
        transform.Rotate(new Vector3(0, 0, deltaAngle));

        // Update the cumulative angle
        angle += deltaAngle;
    }
}
