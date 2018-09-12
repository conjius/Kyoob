using UnityEngine;

public class GameTimer {
    private float _boostTimer;
    private float _boostTimeLimit;
    public bool IsBoostTimeUp;

    public float DestructionTimer;
    public float DestructionTimeLimit;
    public bool IsDestructionTimeUp;

    public float MagnetTimer;
    public float MagnetTimeLimit;
    public bool IsMagnetTimeUp;

    public float ProjectilesTimer;
    public float ProjectilesTimeLimit;
    public bool IsProjectilesTimeUp;
    
    public float ExplosionTimer;
    public float ExplosionTimeLimit;
    public bool IsExplosionTimeUp;


    public GameTimer(float boostTimeLimit, float destructionTimeLimit,
        float magnetTimeLimit, float projectilesTimeLimit, float explosionTimeLimit) {
        _boostTimer = 0.0f;
        _boostTimeLimit = boostTimeLimit;
        IsBoostTimeUp = false;

        DestructionTimer = 0.0f;
        DestructionTimeLimit = destructionTimeLimit;
        IsDestructionTimeUp = false;

        MagnetTimer = 0.0f;
        MagnetTimeLimit = magnetTimeLimit;
        IsMagnetTimeUp = false;

        ProjectilesTimer = 0.0f;
        ProjectilesTimeLimit = projectilesTimeLimit;
        IsProjectilesTimeUp = false;
        
        ExplosionTimer = 0.0f;
        ExplosionTimeLimit = explosionTimeLimit;
        IsExplosionTimeUp = false;
    }

    public void TickBoost() {
        if (IsBoostTimeUp) return;
        _boostTimer += Time.deltaTime;
        if (_boostTimer < _boostTimeLimit) return;
        IsBoostTimeUp = true;
    }

    public void TickDestruction() {
        if (IsDestructionTimeUp) return;
        DestructionTimer += Time.deltaTime;
        if (DestructionTimer < DestructionTimeLimit) return;
        IsDestructionTimeUp = true;
    }

    public void TickMagnet() {
        if (IsMagnetTimeUp) return;
        MagnetTimer += Time.deltaTime;
        if (MagnetTimer < MagnetTimeLimit) return;
        IsMagnetTimeUp = true;
    }

    public void TickProjectiles() {
        if (IsProjectilesTimeUp) return;
        ProjectilesTimer += Time.deltaTime;
        if (ProjectilesTimer < ProjectilesTimeLimit) return;
        IsProjectilesTimeUp = true;
    }
    
    public void TickExplosion() {
        if (IsExplosionTimeUp) return;
        ExplosionTimer += Time.deltaTime;
        if (ExplosionTimer < ExplosionTimeLimit) return;
        IsExplosionTimeUp = true;
    }
    
    public void ZeroExplosion() {
        IsExplosionTimeUp = false;
        ExplosionTimer = 0.0f;
    }

    public void ZeroDestruction() {
        IsDestructionTimeUp = false;
        DestructionTimer = 0.0f;
    }

    public void ZeroBoost() {
        IsBoostTimeUp = false;
        _boostTimer = 0.0f;
    }

    public void ZeroMagnet() {
        IsMagnetTimeUp = false;
        MagnetTimer = 0.0f;
    }

    public void ZeroProjectiles() {
        IsProjectilesTimeUp = false;
        ProjectilesTimer = 0.0f;
    }
}