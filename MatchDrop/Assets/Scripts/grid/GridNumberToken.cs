using UnityEngine;
using System.Collections;

public class GridNumberToken : GridToken {
	
	public override void markToken ()
	{		
		reduceToken (getSubBasedOnStreaks());
	}

	public void reduceToken (int reducer)
	{		
		int num = GetComponent<DisplayNumberToken>().num;
		
		num -= reducer;
		GetComponent<DisplayNumberToken>().UpdateGUI(num);
		
		if(num < 0){
			base.markToken();
		}
	}

	public int getSubBasedOnStreaks(){
		int subNum = 0;

		for (int dir = 0; dir < 4; dir++) {
			if (streak [dir] > 0) {
				int streakNum = streak [dir];
				
				switch (streakNum) {
				case 3:
					subNum+=1;
					break;
				case 4:
					subNum+=2;
					break;
				case 5:
					subNum+=3;
					break;
				}
			}
		}
		return subNum;
	}
}
