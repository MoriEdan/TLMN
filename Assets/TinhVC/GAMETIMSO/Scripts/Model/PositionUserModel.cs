using UnityEngine;
using System.Collections;

public class PositionUserModel {

    private bool isActive = false;
    private GameObject playerObj;

    public bool getIsActive() {
        return isActive;
    }
    public GameObject getPlayerObj()
    {
        return playerObj;
    }

    public void setData(bool _isActive, GameObject _playerObj)
    {
        this.isActive = _isActive;
        this.playerObj = _playerObj;
    }
}
