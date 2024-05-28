using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class SpawnerCube : Spawner<Cube>
{
    [SerializeField] private Cube _cube;
    [SerializeField] private float _delay;
    [SerializeField] private Transform[] _spawnPoints;

    public event Action<Vector3> CubeDeactivated;

    private void Start()
    {
        StartCoroutine(ExtractingElement());
    }

    public override void ReturnItem(Cube cube)
    {
        CubeDeactivated?.Invoke(cube.transform.position);
        base.ReturnItem(cube);
    }

    public override void ReceiveObject(Cube cube)
    {
        cube.CancelInvoke(nameof(cube.Deactivate));
        cube.Disable();
        int spawnPointNumber = Random.Range(0, _spawnPoints.Length);
        _spawnPoints[spawnPointNumber].transform.position = new Vector3(Random.Range(5.0f, 15.0f), Random.Range(5.0f, 15.0f), Random.Range(5.0f, 15.0f));
        cube.transform.position = _spawnPoints[spawnPointNumber].transform.position;
        cube.gameObject.SetActive(true);
        cube.SetStartColor();
    }

    protected override string GetObjectName()
    {
        return "Кубов";
    }

    protected override void InitializeObject(Cube obj, Vector3 position)
    {
        obj.Initialize(this, Vector3.zero);

    }

    private IEnumerator ExtractingElement()
    {
        var waitForSeconds = new WaitForSeconds(_delay);

        while (enabled)
        {
            GetObject();

            yield return waitForSeconds;
        }
    }
}
