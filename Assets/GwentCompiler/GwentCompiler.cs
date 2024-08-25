using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GwentCompiler
{
    public static void Compile()
    {

        string file = "Assets/CardsCollection/TestFile/Test.txt";

        string path = GetFileContent(file);

        Lexer lexer = new Lexer();
        List<Token> tokens = lexer.Tokenize(path);
        if (lexer.hadError)
        {
            CompilationError();
            return;
        }

        Parser parser = new Parser(tokens);
        List<IProgramNode> program = parser.Program();
        if (parser.hadError)
        {
            CompilationError();
            return;
        }

        ObjectCompiler objectCompiller = new ObjectCompiler(program);
        List<CompiledObject> compiledObjects = objectCompiller.CompileObjects();
        foreach (var obj in compiledObjects)
        {
            if (obj is CompiledCard compiledCard)
            {
                Debug.Log(compiledCard.ToString());

                if (CardsCollection.AllCardsName.ContainsKey(compiledCard.Name))
                {
                    Debug.Log($"A card with the name '{compiledCard.Name}' already exists, please rename this card to save it");
                }

                else
                {
                    obj.Save();
                    Debug.Log("Se ha guardado un objeto");
                }
            }

            if(obj is CompiledEffect compiledEffect)
            {
                if(Effect.CheckEffectExistance(compiledEffect.Name))
                {
                    Debug.Log($"A effect with the name '{compiledEffect.Name}' already exists, please rename this effect to save it");
                }
            }

        }
    }

    private static string GetFileContent(string root) // método que devuleve el contenido del archivo
    {
        StreamReader reader = new StreamReader(root); // leemos el contenido del archivo
        string FileContent = reader.ReadToEnd();
        reader.Close();
        return FileContent;
    }

    private static void CompilationError()
    {
        Debug.Log("Compilation failed, fix all errorr and try again");
    }

}
