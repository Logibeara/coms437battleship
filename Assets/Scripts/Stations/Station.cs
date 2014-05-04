using UnityEngine;
using System.Collections;

public interface Station
{
	Vector2 getTarget(CrewMember crewIn);

	/// <summary>
	/// return true of work was done	/// </summary>
	/// <returns>The work.</returns>
	/// <param name="position">Position.</param>
	bool doWork(CrewMember worker); //does work based on position if possible
}