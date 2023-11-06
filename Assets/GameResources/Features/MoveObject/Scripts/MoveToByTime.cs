namespace GameJam.Features.Moving
{
    using System;
    using Unity.VisualScripting;
    using UnityEngine;
    
    public class MoveToByTime : MonoBehaviour
    {
        public Transform StartPoint = default;
        public Transform EndPoint = default;

        public float TimeMoving = default;

        private float dist = default;
        private float speed = default;

        private void Start()
        {
            dist = Vector3.Distance(StartPoint.position, EndPoint.position);
            speed = dist / TimeMoving;
            Debug.Log(dist + " " + speed);
            transform.position = StartPoint.position;

        }
        
        void Update()
        {
            
            float step =  speed * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(transform.position, EndPoint.position, step);
        }
    }

}
