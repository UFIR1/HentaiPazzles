using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : BaseChar
{
	public EnemyType enemyType = EnemyType.Physical;
	public float speed;
	[SerializeField]
	private int damage;
	public int Damage { get => damage; protected set => damage = value; }
	public virtual void Load(BaseEnemyModel model)
	{
		CurrentHeals = model.CurrentHeals;
		Damage = model.Damage;
		enemyType = model.EnemyType;
		MaxHeals = model.MaxHeals;
		speed = model.Speed;
	}

	public virtual BaseEnemyModel Save()
	{
		var toSave = new BaseEnemyModel()
		{
			CurrentHeals = CurrentHeals,
			Damage= Damage,
			EnemyType = enemyType,
			MaxHeals = MaxHeals,
			Speed = speed
		};
		return toSave;
	}




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

public class BaseEnemyModel : BaseCharSaveModel
{
	public float Speed { get; set; }
	public int Damage { get; set; }
	public EnemyType EnemyType { get; set; }

	public void PullChild(BaseEnemyModel enemyModel)
	{
		enemyModel.Damage = Damage;
		enemyModel.CurrentHeals = CurrentHeals;
		enemyModel.EnemyType = EnemyType;
		enemyModel.MaxHeals = MaxHeals;
		enemyModel.Speed = Speed;
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
