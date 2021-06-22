using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformMove : MonoBehaviour, ISaveble<PlatformMoveModel>, ISaveble<ISaveModel>
{
	public LineRenderer lineRenderer;
	public SurfaceEffector2D SurfaceEffector;
	public Vector2 force;
	public float speed;
	public int nextPositionNum = 0;
	public Vector3 moveToPosition = Vector3.zero;
	public Vector2 currentPlatformVec = Vector2.zero;
	public List<Rigidbody2D> StayOnPlatform;
	private PlatformMovementType movementType = PlatformMovementType.forward;
	// Start is called before the first frame update
	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		moveToPosition = lineRenderer.GetPosition(nextPositionNum);
		SurfaceEffector = GetComponent<SurfaceEffector2D>();
		var toVec = (moveToPosition - transform.position).normalized;
		currentPlatformVec = toVec;
		Debug.Log(toVec);
	}

	// Update is called once per frame
	void Update()
	{

	}
	private void FixedUpdate()
	{
		foreach (var item in StayOnPlatform)
		{
			item.transform.Translate(currentPlatformVec * speed * Time.deltaTime);
		}
		transform.Translate(currentPlatformVec * speed * Time.deltaTime);

		if (Vector2.Distance(transform.position, moveToPosition) < 0.1)
		{
			if (nextPositionNum + 1 < lineRenderer.positionCount && nextPositionNum > 0)
			{
				if (movementType == PlatformMovementType.forward)
				{
					nextPositionNum++;
				}
				if (movementType == PlatformMovementType.back)
				{
					nextPositionNum--;
				}
			}
			else if (nextPositionNum + 1 >= lineRenderer.positionCount && lineRenderer.loop == true)
			{
				movementType = PlatformMovementType.forward;
				nextPositionNum = 0;
			}
			else
			{
				if (nextPositionNum == 0)
				{
					movementType = PlatformMovementType.forward;
					nextPositionNum++;
				}
				if (nextPositionNum == lineRenderer.positionCount - 1)
				{
					movementType = PlatformMovementType.back;
					nextPositionNum--;
				}

			}
			moveToPosition = lineRenderer.GetPosition(nextPositionNum);
			var toVec = (moveToPosition - transform.position).normalized;
			Debug.Log(toVec);
			currentPlatformVec = toVec;



		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{

	}
	private void OnCollisionExit2D(Collision2D collision)
	{


	}
	private void OnTriggerEnter2D(Collider2D collision)
	{

		if (!collision.isTrigger && collision.gameObject.TryGetComponent<Rigidbody2D>(out var rb))
		{
			if (!StayOnPlatform.Contains(rb))
			{
				StayOnPlatform.Add(rb);
				Debug.Log($"added forse: {currentPlatformVec}");
			}
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.isTrigger && collision.gameObject.TryGetComponent<Rigidbody2D>(out var rb))
		{
			StayOnPlatform.Remove(rb);
			//rb.velocity = Vector2.zero;
			rb.AddForce(currentPlatformVec * speed * rb.mass, ForceMode2D.Impulse);
			Debug.Log($"added Impulse: {currentPlatformVec}");
		}
	}

	public Type getTT()
	{
		return typeof(PlatformMoveModel);
	}

	public void Load(PlatformMoveModel model)
	{
		currentPlatformVec = model.CurrentPlatformVec;
		moveToPosition = model.MoveToPosition;
		nextPositionNum = model.NextPositionNum;
		speed = model.Speed;
	}

	public PlatformMoveModel Save()
	{
		var toSave = new PlatformMoveModel()
		{
			CurrentPlatformVec = currentPlatformVec,
			MoveToPosition = moveToPosition,
			NextPositionNum = nextPositionNum,
			Speed = speed
		};
		return toSave;
	}

	public void Load(ISaveModel model)
	{
		Load(model as PlatformMoveModel);
	}

	ISaveModel ISaveble<ISaveModel>.Save()
	{
		return Save();
	}
}
public enum PlatformMovementType
{
	forward,
	back
}

public class PlatformMoveModel : ISaveModel
{
	public override string SaveName { get => nameof(PlatformMoveModel); set { } }
	public float Speed { get; set; }
	public int NextPositionNum { get; set; }

	private PlatformMovementType MovementType { get; set; }
	public Vector2 CurrentPlatformVec { get; set; }
	public Vector3 MoveToPosition { get; set; }


}

