using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A FlockerFaction defines faction-faction attitudes. 
/// </summary>
[System.Serializable]
public class FlockerFaction
{
	// serialize this var so we can see it in Unity inspector
	[SerializeField] int mID;

	public int id {
		set {
			mID = value;
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
