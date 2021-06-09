using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockerForces
{
	private readonly Dictionary<FlockerFaction, float> forces = new Dictionary<FlockerFaction, float>();
	
	bool randomInit = true;

	private const float minRange = 0.9f;
	private const float maxRange = 1.1f;

	/// <summary>
	/// Randomized initialization
	/// </summary>
	public FlockerForces() {
		// in random case,
		// just let accessor generate faction-faction forces on the fly
	}

	public float this[FlockerFaction faction] {
		get {
			if (randomInit && !forces.ContainsKey(faction)) {
				forces[faction] = Random.Range(minRange, maxRange);
			}
			if (forces.ContainsKey(faction)) return forces[faction];

			// fallback - only get to here if we disabled randomInit and didn't define a process for setting faction-faction forces
			return default(float);
		}
	}
}
