using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Movable platform type
    /// </summary>
    public class MoveablePlatform : Platform
    {
        public Vector3 direction;
        public float distance;
        public float speed;
        public bool activeOnStart = false;
        public bool isLooping = false;

        protected Coroutine moveRoutine;
        protected Vector3 startPosition;
        protected Vector3 targetPosition;

		private void Awake()
		{
			startPosition = transform.position;
            targetPosition = startPosition + direction * distance;
		}

		private void Start()
		{
			startPosition = transform.position;
			targetPosition = startPosition + direction * distance;
			if (activeOnStart)
            {
                ActivatePlatform();
            }
		}

        public override void ActivatePlatform()
        {
            // prevent multiple routines
            if(isActive)
            {
                return;
            }
            moveRoutine = StartCoroutine(MovePlatform());
            isActive = true;
        }

        public override void DeactivatePlatform() 
        {
            if(moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
            }
            isActive = false;
        }

        protected IEnumerator MovePlatform()
        {
            // move until reached destination or looping
            while(isLooping || !Mathf.Approximately(0f, Vector2.SqrMagnitude(transform.position - targetPosition)))
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }
	}
}