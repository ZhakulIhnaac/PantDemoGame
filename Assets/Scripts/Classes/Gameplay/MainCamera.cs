using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class MainCamera : MonoBehaviour
    {
        private float cameraSpeed = 5f;
        //private Vector2 initialPosition; // This will be used to 
        public float mouseMoveBorder = 10f;

        void Start()
        {

        }

        void Update()
        {
            Vector3 pos = transform.position;

            // Movement control
            if (Input.GetKey(KeyCode.UpArrow)/* || Input.mousePosition.y > Screen.height - mouseMoveBorder*/)
            {
                pos.y += cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow)/* || Input.mousePosition.x > Screen.width - mouseMoveBorder*/)
            {
                pos.x += cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow)/* || Input.mousePosition.y < mouseMoveBorder*/)
            {
                pos.y -= cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow)/* || Input.mousePosition.x < mouseMoveBorder*/)
            {
                pos.x -= cameraSpeed * Time.deltaTime;
            }

            transform.position = pos;
        }

    }
}