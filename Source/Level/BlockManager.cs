// Made By Teddy
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MagicBrosMario.Source.Block;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Level;

public class BlockManager
{
    private  readonly Dictionary<string, Func<IBlock>> BlockConstructors = new();
    private string xmlPath = "Content/LevelData/Blocks.xml";
    public void Initialize(Texture2D texture)
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
        BlockFactory.BindTexture(texture);
    }

    public IBlock CreateBlock(string blockId)
    {
        if (BlockConstructors.TryGetValue(blockId, out var constructor))
        {
            return constructor.Invoke();
        }

        throw new KeyNotFoundException($"Block with ID '{blockId}' not found.");
    }

    private Func<IBlock> GetBlockConstructor(string functionName)
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