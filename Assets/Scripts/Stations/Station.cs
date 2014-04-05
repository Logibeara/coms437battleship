using UnityEngine;
using System.Collections;

public interface Station
{
	int Health{ get; set; }

	Vector2 getTarget(CrewMember crewIn);

	bool doDamage(int dammageDone);
	/// <summary>
	/// return true of work was done	/// </summary>
	/// <returns>The work.</returns>
	/// <param name="position">Position.</param>
	bool doWork(Vector2 position); //does work based on posistion if possible
	void addCrew(CrewMember crewIn);
	void removeCrew(CrewMember crewIn);
}