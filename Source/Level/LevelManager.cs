using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MagicBrosMario.Source.Block;

namespace MagicBrosMario.Source.Level;

public class LevelManager
{
    private static readonly Dictionary<string, Func<IBlock>> BlockConstructors = new();

    public static void Initialize(string xmlPath)
    {
        BlockConstructors.Clear();
        var doc = XDocument.Load(xmlPath);

        foreach (var blockElement in doc.Descendants("Block"))
        {
            var id = blockElement.Attribute("id")?.Value;
            var function = blockElement.Attribute("function")?.Value;

            if (id != null && function != null)
            {
                BlockConstructors[id] = GetBlockConstructor(function);
            }
        }
    }

    public static IBlock CreateBlock(string blockId)
    {
        if (BlockConstructors.TryGetValue(blockId, out var constructor))
        {
            return constructor.Invoke();
        }

        throw new KeyNotFoundException($"Block with ID '{blockId}' not found.");
    }

    private static Func<IBlock> GetBlockConstructor(string functionName)
    {
        return functionName switch
        {
            "VoidBlock" => BlockFactory.VoidBlock,
            "SkyBlock" => BlockFactory.SkyBlock,
            "GroundBlock" => BlockFactory.GroundBlock,
            "BlueGroundBlock" => BlockFactory.BlueGroundBlock,
            "Bricks" => BlockFactory.Bricks,
            "BlueBricks" => BlockFactory.BlueBricks,
            "BaseBlock" => BlockFactory.BaseBlock,
            "QuestionMarkBlock" => BlockFactory.QuestionMarkBlock,
            "EmptyQuestionMarkBlock" => BlockFactory.EmptyQuestionMarkBlock,
            _ => throw new ArgumentException($"Unknown block function: {functionName}")
        };
    }
}