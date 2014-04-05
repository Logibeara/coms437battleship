using UnityEngine;
using System.Collections;

public interface Station
{
	int Health{ get; set; }

	Vector2 getTarget(CrewMember crewIn);

	bool doDammage(int dammageDone);

	void addCrew(CrewMember crewIn);
	void removeCrew(CrewMember crewIn);
}