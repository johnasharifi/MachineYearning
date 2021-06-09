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
	[SerializeField] private Color mColor;

	// not disposed. Assume that disposal is needed
	bool disposed = false;

	private Material mMaterial;

	public Color Color {
		set {
			mColor = value;
		}
		get {
			return mColor;
		}

	}

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
	/// Public-facing API for affinity lookup
	/// </summary>
	/// <param name="other">Another faction</param>
	/// <returns>A value which defines affinity (+) or hostility (-) of this faction, to the other faction</returns>
	public float GetAffinityFor(FlockerFaction other) {
		return this[other];
	}

	/// <summary>
	/// Get this faction's material
	/// </summary>
	/// <param name="orig">Original material to spawn from</param>
	/// <returns>The faction's material</returns>
	public Material GetMaterial(Material orig) {
		if (mMaterial != null) return mMaterial;
		if (orig != null) {
			mMaterial = new Material(orig);
			mMaterial.SetColor("_Color", mColor);
			return mMaterial;
		}

		// panic
		Debug.LogErrorFormat("Warning: you reached an undefined Material case in {0}", new System.Diagnostics.StackFrame().GetMethod());
		return default;
	}

	/// <summary>
	/// Defines how this faction flocks with another faction
	/// </summary>
	/// <param name="other">Another faction</param>
	/// <returns>A value which defines affinity (+) or hostility (-) of this faction, to the other faction</returns>
	private float this[FlockerFaction other] {
		get {
			if (randomForceInit && !affinities.ContainsKey(other)) {
				affinities[other] = Random.Range(minRange, maxRange);
			}
			if (affinities.ContainsKey(other)) return affinities[other];

			// fallback - only get to here if we disabled randomInit and didn't define a process for setting faction-faction forces
			return default(float);
		}
	}
	
	/// <summary>
	/// Release the FlockerFaction's resources
	/// </summary>
	public void Release() {
		if (!disposed) {
			disposed = true;
			GameObject.Destroy(mMaterial);
		}
	}
}

public static class FlockerFactionFactory {
	const int maxFactionCount = 10;
	private static readonly List<FlockerFaction> factionsManifest = new List<FlockerFaction>();

	/// <summary>
	/// Get a new or existing faction
	/// </summary>
	/// <returns>Gets a faction. Do not call before Application isPlaying. Faction proportions are not guaranteed to be 1/n</returns>
	public static FlockerFaction GetRandomFaction() {
		if (factionsManifest.Count < maxFactionCount) {
			FlockerFaction faction = new FlockerFaction { id = factionsManifest.Count + 1, Color = new Color(Random.value, Random.value, Random.value, 1.0f) };
			factionsManifest.Add(faction);
		}

		return factionsManifest[Random.Range(0, factionsManifest.Count)];
	}
}
