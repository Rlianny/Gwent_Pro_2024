using UnityEngine;
using System;
public interface IVisitor<TResult>
{
    TResult VisitBase(IASTNode node, params object[] additionalParams);
}