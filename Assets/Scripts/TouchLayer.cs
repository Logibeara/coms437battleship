using UnityEngine;
using System.Collections;

public class TouchLayer : MonoBehaviour {
	Vector2 scrollPosition;
	Touch touch;
	// The string to display inside the scrollview. 2 buttons below add & clear this string.
	string longString = "This is a long-ish string";
	
	void OnGUI () { 
		
//		scrollPosition = GUI.BeginScrollView(new Rect(110,50,130,150),scrollPosition, new Rect(110,50,130,560),GUIStyle.none,GUIStyle.none);
//		
//		for(int i = 0;i < 20; i++)
//		{
//			GUI.Box(new Rect(110,50+i*28,100,25),"xxxx_"+i);
//		}
//		GUI.EndScrollView ();
	}

	void Update()
	{
//		if(Input.touchCount > 0)
//		{
//			touch = Input.touches[0];
//			if (touch.phase == TouchPhase.Moved)
//			{
//				scrollPosition.y += touch.deltaPosition.y;
//			}
//		}
		if(Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);

			float distance;

			print ("fire");
			if (xyPlane.Raycast (ray, out distance)) {



				print ("hit");
				Vector3 hitPoint = ray.GetPoint(distance);
				print (hitPoint);
				//rigidbody.AddExplosionForce(2000f, hit.point, 8f);
				float innerCircleRadius = .5f;
				float outerCircleRadius = 1.5f;

				float forceScale = 150;
				//Outward Force Logic
				Collider2D[] crewMembers = Physics2D.OverlapCircleAll(hitPoint, outerCircleRadius);

				foreach(Collider2D crewMember in crewMembers)
				{
					Rigidbody2D rb = crewMember.rigidbody2D;

					if(rb != null && rb.gameObject.GetComponent(typeof(CrewMember)) as CrewMember != null)
					{
						Vector3 deltaPos = rb.transform.position - hitPoint;
						float forceFactor, force;

						//force can be out or in
						if(deltaPos.magnitude < innerCircleRadius)
						{
							forceFactor = (innerCircleRadius-deltaPos.magnitude)/innerCircleRadius;
							force = Mathf.Pow (forceFactor,3) * forceScale;
						}
						else
						{
							forceFactor = (-outerCircleRadius+deltaPos.magnitude)/outerCircleRadius;
							force = Mathf.Pow (forceFactor,1) * forceScale;
						}
						rb.AddForce(force * deltaPos.normalized);

						//Reset this crew member's job
						(crewMember.gameObject.GetComponent(typeof(CrewMember)) as CrewMember).nullifyJob();
					}
				}
			}
		}
	}
}
