using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStatus: IEquatable<GridStatus>
{
	public int isX { get; set; }

	public int isY { get; set; }

	public string isEmpty { get; set; }
	
	public override string ToString()
	{
		return "X: " + isX + "   Y: " + isY ;
	}
	public override bool Equals(object obj)
	{
		if (obj == null) return false;
		GridStatus objAsPart = obj as GridStatus;
		if (objAsPart == null) return false;
		else return Equals(objAsPart);
	}
	public override int GetHashCode()
	{
		return isX;
	}
	public bool Equals(GridStatus other)
	{
		if (other == null) return false;
		return (this.isX.Equals(other.isX)) & (this.isY.Equals(other.isY)) & (this.isEmpty.Equals(other.isEmpty));
	}
}
