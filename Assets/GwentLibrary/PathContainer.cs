using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathContainer
{
    public static string TestFilePath { get; private set; } = "Assets/CardsCollection/TestFile/Test.txt"; // ruta del archivo de prueba
    public static string SerializedFilesDirectoryPath { get; private set; } = "Assets/CardsCollection/Serialized"; // ruta del directorio en el que se guardan los objetos serializados
    public static string CardDataBaseDirectoryPath { get; private set; } = "CardsCollection";  // ruta del directorio donde se encuentran guardados los archivos de texto equivalentes a una carta
}
