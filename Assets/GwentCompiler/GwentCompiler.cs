using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GwentCompiler
{
    public static void Compile(string path)
    {
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

        List<Card> compileds = new();

        ObjectCompiler objectCompiller = new ObjectCompiler(program);
        List<CompiledObject> compiledObjects = objectCompiller.CompileObjects();
        foreach (var obj in compiledObjects)
        {
            if (obj is CompiledCard compiledCard)
            {
                Debug.Log(compiledCard.ToString());

                if (CardsCollection.AllCardsName.ContainsKey(compiledCard.Name))
                {
                    if (CompilerOutput.compilerOutput != null)
                        CompilerOutput.compilerOutput.Report($"A card with the name '{compiledCard.Name}' already exists, please rename this card to save it");
                    Debug.Log($"A card with the name '{compiledCard.Name}' already exists, please rename this card to save it");
                }

                else
                {
                    obj.Save();
                    Card card = CardsCollection.TypeCreator(compiledCard);
                    if (CompilerOutput.compilerOutput != null)
                        CompilerOutput.compilerOutput.Report($"'{compiledCard.Name}' has arrived on the battlefield");
                    Debug.Log("Se ha guardado un objeto");
                    compileds.Add(card);
                    CardsCollection.AllCardsName.Add(card.Name, card);
                }
            }

            if (obj is CompiledEffect compiledEffect)
            {
                if (Effect.CheckEffectExistance(compiledEffect.Name))
                {
                    if (CompilerOutput.compilerOutput != null)
                        CompilerOutput.compilerOutput.Report($"A effect with the name '{compiledEffect.Name}' already exists, please rename this effect to save it"); Debug.Log($"A effect with the name '{compiledEffect.Name}' already exists, please rename this effect to save it");
                    Debug.Log($"A effect with the name '{compiledEffect.Name}' already exists, please rename this effect to save it"); Debug.Log($"A effect with the name '{compiledEffect.Name}' already exists, please rename this effect to save it");
                }
                else
                {
                    obj.Save();
                    Effect.RegisterEffect(compiledEffect);
                    if (CompilerOutput.compilerOutput != null)
                        CompilerOutput.compilerOutput.Report($"The effect '{compiledEffect.Name}' has been saved successfully");
                    Debug.Log("Se ha guardado un objeto");
                }
            }

        }

        if (CompilerOutput.compilerOutput != null && compileds.Count > 0)
        {
            CompilerOutput.compilerOutput.ShowNewCards(compileds);
        }
    }

    public static string GetFileContent(string root) // método que devuleve el contenido del archivo
    {
        StreamReader reader = new StreamReader(root); // leemos el contenido del archivo
        string FileContent = reader.ReadToEnd();
        reader.Close();
        return FileContent;
    }

    private static void CompilationError()
    {
        if (CompilerOutput.compilerOutput != null)
            CompilerOutput.compilerOutput.Report("Compilation failed, fix all errorr and try again");
        Debug.Log("Compilation failed, fix all errorr and try again");
    }

}
