using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : BaseChar
{
	public EnemyType enemyType = EnemyType.Physical;
	public float speed;

	// Start is called before the first frame update
	void Start()
	{

		LocalStart();
	}

	// Update is called once per frame
	void Update()
	{

		LocalUpdate();
	}
}
public enum EnemyType
{
	Physical,
	Ghost
}
public enum GroundEnemyMoveDirection
{
	left = -1,
	stay = 0,
	right = 1
}
