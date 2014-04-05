using UnityEngine;
using System.Collections;

public interface Station
{
	Vector2 getTarget(CrewMember crewIn);
}