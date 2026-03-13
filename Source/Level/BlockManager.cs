// Made By Teddy
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using MagicBrosMario.Source.Block;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Level;

public class BlockManager
{
    private readonly Dictionary<string, Func<int, int, IBlock>> BlockConstructors = new();
    private string xmlPath = "Content/LevelData/Blocks.xml";
    private readonly int _scale = 4;

    public void Initialize(Texture2D texture)
    {
        BlockConstructors.Clear();
        var doc = XDocument.Load(xmlPath);

        foreach (var blockElement in doc.Descendants("Block"))
        {
            var id = blockElement.Attribute("id")?.Value;
            var function = blockElement.Attribute("function")?.Value;

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(function))
            {
                BlockConstructors[id] = GetBlockConstructor(function);
            }
        }
        BlockFactory.BindTexture(texture);
    }

    public IBlock CreateBlock(string blockId, int x, int y)
    {
        if (BlockConstructors.TryGetValue(blockId, out var constructor))
        {
             return constructor(x, y);
        }

        throw new KeyNotFoundException($"Block with ID '{blockId}' not found.");
    }

    private Func<int, int, IBlock> GetBlockConstructor(string functionName)
    {
        return functionName switch
        {
            "VoidBlock" => (x, y) => BlockFactory.VoidBlock().WithPosition(x, y).WithScale(_scale),
            "SkyBlock" => (x, y) => BlockFactory.SkyBlock().WithPosition(x, y).WithScale(_scale),
            "GroundBlock" => (x, y) => BlockFactory.GroundBlock().WithPosition(x, y).WithScale(_scale),
            "BlueGroundBlock" => (x, y) => BlockFactory.BlueGroundBlock().WithPosition(x, y).WithScale(_scale),
            "Bricks" => (x, y) => BlockFactory.Bricks().WithPosition(x, y).WithScale(_scale),
            "BlueBricks" => (x, y) => BlockFactory.BlueBricks().WithPosition(x, y).WithScale(_scale),
            "BaseBlock" => (x, y) => BlockFactory.BaseBlock().WithPosition(x, y).WithScale(_scale),
            "QuestionMarkBlock" => (x, y) => BlockFactory.QuestionMarkBlock(QuestionMarkBlock.InnerItem.Coin).WithPosition(x, y).WithScale(_scale),
            "EmptyQuestionMarkBlock" => (x, y) => BlockFactory.VoidBlock().WithPosition(x, y).WithScale(_scale),
            _ => throw new ArgumentException($"Unknown block function: {functionName}")
        };
    }
}