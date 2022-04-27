# ObjectPool
Easy to use, GameObject recycle system for Unity. Instead of constant Instantiate/Destroy reuse your objects with ObjectPool to lower Garbage Collection and CPU usage.

## Navigation
* [Features](#features)
* [Setup Guide](#setup-guide)
* [Methods](#methods)

## Features
- _**Reuse** Instantiated GameObjects_
- _**Enum** based_
- _Easy to **reference** throughout your project_

## Setup Guide

### Installation
Download the zip file and place ObjectPool folder in your project.
  
### Usage
  
#### Open PoolTags enum and add any pool tags you need.
<img src="https://user-images.githubusercontent.com/49119130/165413381-2826c05b-a820-48f6-994b-28b14b21382c.png" width="350" />

#### Create new ObjectPool Scriptable Object
<img src="https://user-images.githubusercontent.com/49119130/165413670-4fdfcb0c-a2d2-415f-a50b-daed78fbbbf0.png" width="500" />

#### Add as many pools as you need. Each pool takes PoolTag, Prefab, and amount of objects you want to create.
<img src="https://user-images.githubusercontent.com/49119130/165413851-9eed347b-f70e-4f80-8baa-40ac5a98b90a.png" width="500" />
  

## Methods
Make sure to include `using AoOkami.ObjectPool;` in your scripts, then simply reference your newly created ObjectPoolSO anywhere you need.

### _void_ InitializePool()
Invoke this funtion anywhere in your script when the scene starts. It is used to instantiate all of the prefabs you have selected in the pool.\
_Example usage:_

```csharp
using AoOkami.ObjectPool;

[SerializeField] ObjectPoolSO objectPool;

private void Start() => objectPool.InitializePool(); 
```

### _void_ AddToPool(_GameObject instance_)
Add GameObject back to the pool. If object wasn't instantiated with ObjectPool, it won't be added.\
_Example usage:_

```csharp
using AoOkami.ObjectPool;

[SerializeField] ObjectPoolSO objectPool;

private void OnTriggerEnter(Collider collision)
{
    if (collision.CompareTag("Powerup")) objectPool.AddToPool(collision.gameObject);
}

```

### _GameObject_ GetFromPool(_PoolTags tag_)
Get GameObject from pool marked with a **PooolTag**. When there is no object left in pool, instantiate new one.\
_Example usage:_

```csharp
using AoOkami.ObjectPool;

[SerializeField] ObjectPoolSO objectPool;

private void Update()
{
    if (Input.GetKeyDown(KeyCode.Space)) Shoot();
}

private void Shoot() => objectPool.GetFromPool(PoolTags.Bullet);
```

### _GameObject_ GetFromPool(_PoolTags tag, Vector3 position_)
Get GameObject from pool marked with a **PooolTag** and set its position. When there is no object left in pool, instantiate new one.\
_Example usage:_

```csharp
using AoOkami.ObjectPool;

[SerializeField] ObjectPoolSO objectPool;

private void Update()
{
    if (Input.GetKeyDown(KeyCode.Space)) Shoot();
}

private void Shoot() => objectPool.GetFromPool(PoolTags.Bullet, Vector3.zero);
```

### _GameObject_ GetFromPool(_PoolTags tag, Vector3 position, Quaternion rotation_)
Get GameObject from pool marked with a **PooolTag** and set its position and rotation. When there is no object left in pool, instantiate new one.\
_Example usage:_

```csharp
using AoOkami.ObjectPool;

[SerializeField] ObjectPoolSO objectPool;

private void Update()
{
    if (Input.GetKeyDown(KeyCode.Space)) Shoot();
}

private void Shoot() => objectPool.GetFromPool(PoolTags.Bullet, Vector3.zero, transform.rotation);
```

