using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IObserver
{
    public void OnNotify(System.Enum action, Card card = null);
}
