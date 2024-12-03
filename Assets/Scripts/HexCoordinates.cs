﻿using UnityEngine;

[System.Serializable]
public struct HexCoordinates {

    [SerializeField]
    private int x, z;

    // X coordinate
    public int X {
        get {
            return x;
        }
    }

    // Z coordinate
    public int Z {
        get {
            return z;
        }
    }

    // Y coordinate calculated from X and Z
    public int Y {
        get {
            return -X - Z;
        }
    }

    // Constructor to initialize coordinates
    public HexCoordinates (int x, int z) {
        this.x = x;
        this.z = z;
    }

    // Convert offset coordinates to hex coordinates
    public static HexCoordinates FromOffsetCoordinates (int x, int z) {
        return new HexCoordinates(x - z / 2, z);
    }

    // Convert a position to hex coordinates
    public static HexCoordinates FromPosition (Vector3 position) {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;

        float offset = position.z / (HexMetrics.outerRadius * 3f);
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x -y);

        if (iX + iY + iZ != 0) {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x -y - iZ);

            if (dX > dY && dX > dZ) {
                iX = -iY - iZ;
            }
            else if (dZ > dY) {
                iZ = -iX - iY;
            }
        }

        return new HexCoordinates(iX, iZ);
    }

    // Return coordinates as a string
    public override string ToString () {
        return "(" +
               X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    // Return coordinates as a string with separate lines
    public string ToStringOnSeparateLines () {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}