using UnityEngine;
using System.Collections;

public class AccessRestricter : MonoBehaviour {

	public virtual void UpdateIfLocked(GameObject button){
		if(button.GetComponent<ChangeScreenButtonLock>().lockLevel > GetLockLevel()){
			UpdateToLockedDisplay(button);
		}
	}

	public virtual void UpdateToLockedDisplay(GameObject button){
	}

	public virtual int GetLockLevel(){
		return 0;
	}
}
