using Platformer.Gameplay;
using System.Collections;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(Collider2D))]
	public class SecretWall : MonoBehaviour
	{
		public float xrayStrength = 0.5f;
		public float transitionDelay = 0.01f;
		public AudioClip wallEnteredAudio;

		protected SpriteRenderer spriteRenderer;
		protected new Collider2D collider;

		// save original alpha value of sprite
		private float cachedAlpha;

		private Coroutine enableXrayRoutine;
		private Coroutine disableXrayRoutine;

		// Start is called before the first frame update
		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			collider = GetComponent<Collider2D>();
			cachedAlpha = spriteRenderer.color.a;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			var player = collision.gameObject.GetComponent<PlayerController>();
			if (player != null)
			{
				PlayerEnter(player);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			var player = collision.gameObject.GetComponent<PlayerController>();
			if (player != null)
			{
				PlayerExit(player);
			}
		}

		protected void PlayerEnter(PlayerController player)
		{
			// cancel other routine if running
			if (disableXrayRoutine != null)
			{
				StopCoroutine(disableXrayRoutine);
			}
			enableXrayRoutine = StartCoroutine(EnableXray());

			var ev = Schedule<PlayerSecretWallEntered>();
			ev.wall = this;
			ev.player = player;
		}

		protected void PlayerExit(PlayerController player)
		{
			// cancel other routine if running
			if (enableXrayRoutine != null)
			{
				StopCoroutine(enableXrayRoutine);
			}
			disableXrayRoutine = StartCoroutine(DisableXray());
		}

		private IEnumerator EnableXray()
		{
			while (spriteRenderer.color.a > 1 - xrayStrength)
			{
				Color newColor = spriteRenderer.color;
				newColor.a -= 0.01f;
				spriteRenderer.color = newColor;
				yield return new WaitForSeconds(transitionDelay);
			}
		}

		private IEnumerator DisableXray()
		{
			while (spriteRenderer.color.a < cachedAlpha)
			{
				Color newColor = spriteRenderer.color;
				newColor.a += 0.01f;
				spriteRenderer.color = newColor;
				yield return new WaitForSeconds(transitionDelay);
			}
		}
	}
}
