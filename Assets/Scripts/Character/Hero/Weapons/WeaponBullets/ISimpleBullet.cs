using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISimpleBullet
{
	public int MainDamage { get; set; }

}
public interface IFireBullet
{
	public int DebufPerTickDamage { get; set; }
}
public interface IPuncherBullet
{
	public float PunchForsee { get; set; }
}

