using UnityEngine;
using System.Collections;

public class GridMultiToken : GridToken {
	
	public override bool isMatch (int otherType)
	{	
		DisplayMultiToken mtd = (DisplayMultiToken)tokenDisplay;
		return base.isMatch(otherType) || (mtd.type2 == otherType && tokenDisplay.size > 0);
	}
	
	public override bool hasMatch (bool markMatched)
	{
		bool hasMatch = base.hasMatch(markMatched);
		
		DisplayMultiToken mtd = (DisplayMultiToken)tokenDisplay;

		hasMatch = hasMatch || hasDirMatch (DIRS.DIR_HORZ, mtd.type2, markMatched);
		
		hasMatch = hasDirMatch (DIRS.DIR_VERT, mtd.type2, markMatched) || hasMatch;
		hasMatch = hasDirMatch (DIRS.DIR_DIAG_DN, mtd.type2, markMatched) || hasMatch;
		hasMatch = hasDirMatch (DIRS.DIR_DIAG_UP, mtd.type2, markMatched) || hasMatch;
		
		return hasMatch;
	}
}
