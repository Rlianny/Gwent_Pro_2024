using UnityEngine;
using System;
public class CodeLocation
{
    public int Row { get; private set; }
    public int Column { get; private set; }

    public CodeLocation(int row, int column)
    {
        Row = row;
        Column = column;
    }
}
