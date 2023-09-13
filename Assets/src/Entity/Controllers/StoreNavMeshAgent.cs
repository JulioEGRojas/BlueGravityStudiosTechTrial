using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StoreNavMeshAgent : NavMeshAgent2D {

    /// <summary>
    /// Nav mesh this agent uses for when its path is locked
    /// </summary>
    private NavMesh2DHL _backupNavMesh;
    
    public override void Stop() {
        StopAllCoroutines();
        xMove = 0;
        yMove = 0;
        arrived = true;
    }
    
    public void SetBackupNavMesh(NavMesh2DHL backupNavMesh) {
        _backupNavMesh = backupNavMesh;
    }

    public void SetNavMeshes(NavMesh2DHL mainNM, NavMesh2DHL backupNM) {
        _navMesh2D = mainNM;
        _backupNavMesh = backupNM;
    }
}
