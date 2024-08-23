using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Environment
{
    public Environment Enclosing {get; private set;}
    private Dictionary<string, object> values = new();

    public Environment(Environment environment)
    {
        Enclosing = environment;
    }

    public Environment()
    {
        Enclosing = null;
    }

    public void Assign(string name, object value)       // Assigna o define una variable
    {
        if (values.ContainsKey(name)) values[name] = value;     // si la variable se encuentra en el scope actual la actualiza

        if (Enclosing != null) Enclosing.Assign(name, value);   // si no, busca la variable en el scope anterior

        else if (Enclosing == null)     // si el scope actual es el global
        {
            Define(name, value);        // la variable se define o se asigna
        }
    }

    private void Define(String name, object value)
    {
        if(values.ContainsKey(name)) values[name] = value;
        else values.Add(name, value);
    }

    public object Get(Token name)       // método de acceso al valor del diccionario correspondiente a la llave que se pasa como parámetro
    {
        if (values.ContainsKey(name.Lexeme)) return values[name.Lexeme];
        if (Enclosing != null) return Enclosing.Get(name);
        else throw new RuntimeError($"Not defined variable {name}", name.Location);

    }

}