using UnityEngine;
using System.Collections;

public class ChangeScreenButtonLock : ChangeScreenButton {

	public int lockLevel;
	public GameObject locker;

	void Start(){
		locker.GetComponent<AccessRestricter>().UpdateIfLocked(gameObject);
	}

	public override void ChangeToScreen(){
		if(lockLevel <= locker.GetComponent<AccessRestricter>().GetLockLevel()){
			base.ChangeToScreen();
		}
	}
}
