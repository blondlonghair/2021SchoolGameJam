using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoDestroy<EnemyManager>
{
    public List<Unit> enemys = new List<Unit>();

    public void AddEnemy(Unit enemy)
    {
        enemys.Add(enemy);
    }

    public void RemoveEnemy(Unit enemy)
    {
        enemys.Remove(enemy);
    }
}
