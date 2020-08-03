using UnityEngine;

namespace Assets.Scripts.Classes.Gameplay
{
    public class MainCamera : MonoBehaviour
    {
        private const float CameraSpeed = 5f;
        public float MouseMoveBorder = 10f;

        private void Update()
        {
            if (!Input.anyKey) return;
            var pos = transform.position;

            // Movement control
            if (Input.GetKey(KeyCode.UpArrow))
            {
                pos.y += CameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                pos.x += CameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                pos.y -= CameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                pos.x -= CameraSpeed * Time.deltaTime;
            }

            transform.position = pos;
        }
    }
}