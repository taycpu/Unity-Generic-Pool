# Unity-Generic-Pool
Generic pool system for Unity!

![On Hierarchy](https://github.com/taycpu/Unity-Generic-Pool/blob/main/PoolHierarchy.png)

## Add your items to pool via add a parent empty object and make your poolable objects its child.(It's also have auto-extend feature)
*All poolable sets must have one empty parent* This is how editor script get objects.




![On Inspector](https://github.com/taycpu/Unity-Generic-Pool/blob/main/poolInspector.png)
Select which component you want to add to pool from dropdown menu's.

# When use

GenericPool.Instance.GetFromPool<"YOUR TYPE">("POOLINDEX");
as an example
```cs
ParticleSystem particle=GenericPool.Instance.GetFroomPool<ParticleSystem>(0);
```

