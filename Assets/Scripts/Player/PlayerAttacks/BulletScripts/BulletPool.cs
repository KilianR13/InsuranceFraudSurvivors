using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private static BulletPool _instance;
    public static BulletPool Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("BulletPool Instance missing");
            }
            return _instance;
        }   
    }
    [SerializeField] private Bullet _enemyBulletPrefab;
    [SerializeField] private int _initialPoolSize = 10;

    private List<Bullet> _bulletPool = new List<Bullet>();

    void Awake()
    {
        // SETUP SINGLETON
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;   
        }

        // SETUP POOL
        AddBulletsToPool(_initialPoolSize);
    }

    private void AddBulletsToPool(int ammount)
    {
        for (int i = 0; i < ammount; i++){
            Bullet bullet = Instantiate(_enemyBulletPrefab);
            bullet.gameObject.SetActive(false);
            _bulletPool.Add(bullet);
            bullet.transform.parent = transform;
        }
    }

    // Solicita una bala del Pool.
    public Bullet RequestBullet()
    {
        // Toma la primera que esté desactivada
        for (int i = 0; i < _bulletPool.Count; i++)
        {
            if (!_bulletPool[i].gameObject.activeSelf)
            {
                _bulletPool[i].gameObject.SetActive(true);
                return _bulletPool[i];
            }
        }

        // Si ninguna está desactivada, instancia una nueva, la añade al pool y la activa.
        AddBulletsToPool(1);
        _bulletPool[_bulletPool.Count - 1].gameObject.SetActive(true);
        return _bulletPool[_bulletPool.Count - 1];
    }
}
