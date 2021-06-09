using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A FlockerFaction defines faction-faction attitudes. 
/// </summary>
[System.Serializable]
public class FlockerFaction
{
	// define affinities of this faction for other factions
	private readonly Dictionary<FlockerFaction, float> affinities = new Dictionary<FlockerFaction, float>();
	
	private const float minRange = 0.9f;
	private const float maxRange = 1.1f;
	
	// serialize this var so we can see it in Unity inspector
	[SerializeField] int mID;

	// by default, faction-faction feelings should be random
	bool randomForceInit = true;

	public int id {
		set {
			mID = value;
		}
	}

	/// <summary>
	/// Defines how this faction flocks with another faction
	/// </summary>
	/// <param name="other">Another faction</param>
	/// <returns>A value which defines affinity (+) or hostility (-) of this faction, to the other faction</returns>
	public float this[FlockerFaction other] {
		get {
			if (randomForceInit && !affinities.ContainsKey(other)) {
				affinities[other] = Random.Range(minRange, maxRange);
			}
			if (affinities.ContainsKey(other)) return affinities[other];

			// fallback - only get to here if we disabled randomInit and didn't define a process for setting faction-faction forces
			return default(float);
		}
	}
}

public static class FlockerFactionFactory {
	const int maxFactionCount = 10;
	private static readonly List<FlockerFaction> factionsManifest = new List<FlockerFaction>();

	public static FlockerFaction GetRandomFaction() {
		if (factionsManifest.Count < maxFactionCount) {
			FlockerFaction faction = new FlockerFaction { id = factionsManifest.Count + 1 };
			factionsManifest.Add(faction);
		}

		return factionsManifest[Random.Range(0, factionsManifest.Count)];
	}
}
