using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : MonoBehaviour
{
	[SerializeField] private FlockerFaction faction;

	// set once at static time
	private static readonly HashSet<Flocker> flock = new HashSet<Flocker>();

	private float flockDistanceThreshold = 10.0f;
	private int flockCountThreshold = 10;

	private float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
		faction = FlockerFactionFactory.GetRandomFaction();

		// randomize the flocker's speeds so we can see differentitation
		speed = speed * Random.Range(0.9f, 1.1f);
		flock.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
		transform.forward = Vector3.RotateTowards(transform.forward, GetForceVector(), 2f * Time.deltaTime, 2f * Time.deltaTime);
		transform.position += transform.forward * Time.deltaTime * speed;
    }

	Vector3 GetForceVector() {
		Vector3 total = new Vector3();

		int adjacentCount = 0;

		foreach (Flocker flocker in flock) {
			Vector3 diff = flocker.transform.position - transform.position;
			if (diff.sqrMagnitude < flockDistanceThreshold * flockDistanceThreshold)
				adjacentCount++;
			// push / pull depending upon how this flocker's forces / rules define behavior toward that flocker's faction
			total += (flocker.transform.position - transform.position) * faction.GetAffinityFor(flocker.faction);
		}

		total /= flock.Count;

		total = total * (adjacentCount > flockCountThreshold ? -1 : 1);
		// we do not want the force vector to have a scale factor
		total.Normalize();
		return total;
	}
}
