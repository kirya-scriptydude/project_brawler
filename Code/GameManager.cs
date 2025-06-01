using System;

public sealed class GameManager : Component {
	[Property, ReadOnly] public int Wave { get; private set; } = 0;
	[Property, ReadOnly] public int EnemyCount { get; set; } = 2;

	[Property, Group("Settings")] public int MaxEnemies { get; set; } = 1;
	[Property, Group("Settings")] public float SpawnCooldown = 1f;
	[Property, Group("Settings")] public List<GameObject> Spawnpoints { get; set; } = new();
	[Property, Group("Settings")] public GameObject EnemyPrefab { get; set; }

	private int enemiesToSpawn = 0;
	private float lastSpawned = Time.Now;
	private List<GameObject> aliveEnemies = new List<GameObject>();


	protected override void OnStart() {
		StartWave();
	}

	public void StartWave() {
		Wave++;

		EnemyCount = Math.Clamp(EnemyCount * 2, 1, MaxEnemies);
		enemiesToSpawn = EnemyCount;
	}

	protected override void OnFixedUpdate() {
		handleEnemySpawn();

		if (aliveEnemies.Count == 0 && enemiesToSpawn == 0) {
			StartWave();
		}
	}

	private void handleEnemySpawn() {
		if (enemiesToSpawn > 0) {
			if (Time.Now - lastSpawned > SpawnCooldown) {
				lastSpawned = Time.Now;
				enemiesToSpawn--;

				var pos = Spawnpoints[Random.Shared.Int(Spawnpoints.Count - 1)].WorldPosition;
				var obj = EnemyPrefab.Clone(pos);
				aliveEnemies.Add(obj);
			}
		}

		var allDestroyed = true;
		foreach (var enemy in aliveEnemies) {
			if (!enemy.IsDestroyed) allDestroyed = false;
		}

		if (allDestroyed) {
			aliveEnemies = new(20);
		}
	}
}
