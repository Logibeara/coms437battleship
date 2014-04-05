using UnityEngine;
using System.Collections;

public interface Station
{
	int Health{ get; set; }

	Vector2 getTarget(CrewMember crewIn);

	void addCrew(CrewMember crewIn);
	void removeCrew(CrewMember crewIn);
}