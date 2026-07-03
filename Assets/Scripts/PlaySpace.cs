using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public abstract class PlaySpace<MatType> : MonoBehaviour where MatType : FieldMat
{
    [Header("PlaySpace Mat Typing")]
    [SerializeField] protected MatType matPrefab;

    protected readonly List<MatType> _activeMats = new();
    public IReadOnlyList<MatType> activeMats => _activeMats;

    // Properties
    public virtual int RawValue => _activeMats.Sum(mat => mat.value);

    // Public Methods
    public virtual void GenerateField(Transform[] spawnLocations)
    {
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            SpawnMatAtLocation(spawnLocations[i]);
        }
    }

    public virtual void ClearField()
    {
        foreach (var mat in _activeMats) Destroy(mat.gameObject);
        _activeMats.Clear();
    }

    // Protected Methods
    protected void SpawnMatAtLocation(Transform spawnLocation)
    {
        MatType newMat = Instantiate(matPrefab, spawnLocation.position, Quaternion.identity);
        _activeMats.Add(newMat);
    }
}
