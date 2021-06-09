using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : MonoBehaviour
{
	// set in Start()
	[SerializeField] private FlockerFaction faction;
	
	// set once at static time
	private static readonly HashSet<Flocker> flock = new HashSet<Flocker>();

	private float flockDistanceThreshold = 10.0f;
	
	const float baseMoveSpeed = 10.0f;
	const float baseRotationSpeed = 5.0f;
	private float MoveSpeed {
		get {
			return faction.MoveSpeed * baseMoveSpeed;
		}
	}

	private float RotationSpeed {
		get {
			return faction.RotationSpeed * baseRotationSpeed;
		}
	}

	private Renderer mRenderer;
	private Renderer Renderer {
		get {
			if (mRenderer == null) mRenderer = GetComponent<Renderer>();
			return mRenderer;
		}
	}
    // Start is called before the first frame update
    void Start()
    {
		faction = FlockerFactionFactory.GetRandomFaction();
		Renderer.material = faction.GetMaterial(Renderer.material);
		
		flock.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
		transform.forward = Vector3.RotateTowards(transform.forward, GetForceVector(), RotationSpeed * Time.deltaTime, RotationSpeed * Time.deltaTime);
		transform.position += transform.forward * MoveSpeed * MoveSpeed * Time.deltaTime;
    }

	Vector3 GetForceVector() {
		Vector3 total = Vector3.zero;
	
		int adjacentCount = 0;

		foreach (Flocker flocker in flock) {
			Vector3 diff = flocker.transform.position - transform.position;
			if (diff.sqrMagnitude < flockDistanceThreshold * flockDistanceThreshold) {
				adjacentCount++;

				// when nearby, push / pull depending upon how this flocker's faction's affinity for that flocker's faction
				total += diff * faction.GetAffinityFor(flocker.faction);
			} else {
				// equivalent to diff * 1.0; aka flock toward that unit
				total += (diff);
			}
		}

		total /= flock.Count;
		
		// we do not want the force vector to have a scale factor
		total.Normalize();
		return total;
	}

	private void OnDestroy() {
		faction.Release();
	}
}
