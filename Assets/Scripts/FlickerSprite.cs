using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerSprite : MonoBehaviour
{
	public float flickerDuration = 0.1f; // Time in seconds for each flicker interval
	public float flickerDelay = 0.5f; // Time in seconds between flicker intervals
	public float waitTime = 1.0f; // Time to wait before flickering
	public int flickerCount = 5;// Number of flicker intervals


	private SpriteRenderer spriteRenderer;
	private bool isFlickering = false;

	private void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void StartFlickering() {
		if (!isFlickering) {
			isFlickering = true;
			StartCoroutine(FlickerCoroutine());
		}

	}

	private IEnumerator FlickerCoroutine() {

		yield return new WaitForSeconds(waitTime);
		for (int i = 0; i < flickerCount; i++) {
			spriteRenderer.enabled = !spriteRenderer.enabled;
			yield return new WaitForSeconds(flickerDuration);
			spriteRenderer.enabled = !spriteRenderer.enabled;
			yield return new WaitForSeconds(flickerDelay);
		}

		isFlickering = false;

		Destroy(this.gameObject);
	}
}
