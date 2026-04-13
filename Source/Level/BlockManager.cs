// Made By Teddy
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using MagicBrosMario.Source.Block;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Level;

public static class BlockManager
{
    private static readonly Dictionary<string, Func<int, int, IBlock>> BlockConstructors = new();
    private static string xmlPath = "Content/LevelData/Blocks.xml";
    private static readonly int _scale = 2;

    public static void Initialize(Texture2D texture)
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

    public static IBlock CreateBlock(string blockId, int x, int y, QuestionMarkBlock.InnerItem innerItem = QuestionMarkBlock.InnerItem.Coin)
    {   
        if(blockId == "07") // ?markblock
        {
            return BlockFactory.QuestionMarkBlock(innerItem).WithPosition(x, y).WithScale(_scale);
        }

        if (BlockConstructors.TryGetValue(blockId, out var constructor))
        {
             return constructor(x, y);
        }

        throw new KeyNotFoundException($"Block with ID '{blockId}' not found.");
    }

    private static Func<int, int, IBlock> GetBlockConstructor(string functionName)
    {
        return functionName switch
        {
            "VoidBlock" => (x, y) => BlockFactory.GroundBlock().WithPosition(x, y).WithScale(_scale), //fix xml
            "SkyBlock" => (x, y) => BlockFactory.GroundBlock().WithPosition(x, y).WithScale(_scale), // fix xml
            "GroundBlock" => (x, y) => BlockFactory.GroundBlock().WithPosition(x, y).WithScale(_scale),
            "BlueGroundBlock" => (x, y) => BlockFactory.BlueGroundBlock().WithPosition(x, y).WithScale(_scale),
            "Bricks" => (x, y) => BlockFactory.Bricks().WithPosition(x, y).WithScale(_scale),
            "BlueBricks" => (x, y) => BlockFactory.BlueBricks().WithPosition(x, y).WithScale(_scale),
            "BaseBlock" => (x, y) => BlockFactory.BaseBlock().WithPosition(x, y).WithScale(_scale),
            "QuestionMarkBlock" => (x, y) =>BlockFactory.GroundBlock().WithPosition(x, y).WithScale(_scale), // if every called, a huge mistake has happened
            "LeftPipe" => (x, y) => BlockFactory.PipeBlock(BlockFactory.PipeSegmentType.Vertical, PipeEntryBlock.PipeDirection.Left).WithPosition(x, y).WithScale(_scale),
            "RightPipe" => (x, y) => BlockFactory.PipeBlock(BlockFactory.PipeSegmentType.Vertical, PipeEntryBlock.PipeDirection.Right).WithPosition(x, y).WithScale(_scale),
            "LeftPipeOpening" => (x, y) => BlockFactory.PipeEntryBlock(PipeEntryBlock.PipeDirection.Up, PipeEntryBlock.PipeDirection.Left, null, null).WithPosition(x, y).WithScale(_scale),
            "RightPipeOpening" => (x, y) => BlockFactory.PipeEntryBlock(PipeEntryBlock.PipeDirection.Up, PipeEntryBlock.PipeDirection.Right, null, null).WithPosition(x, y).WithScale(_scale),
            "TopPipe" => (x, y) => BlockFactory.PipeBlock(BlockFactory.PipeSegmentType.Horizontal, PipeEntryBlock.PipeDirection.Up).WithPosition(x, y).WithScale(_scale),
            "BottomPipe" => (x, y) => BlockFactory.PipeBlock(BlockFactory.PipeSegmentType.Horizontal, PipeEntryBlock.PipeDirection.Down).WithPosition(x, y).WithScale(_scale),
            "TopPipeOpening" => (x, y) => BlockFactory.PipeEntryBlock(PipeEntryBlock.PipeDirection.Left, PipeEntryBlock.PipeDirection.Up, null, null).WithPosition(x, y).WithScale(_scale),
            "BottomPipeOpening" => (x, y) => BlockFactory.PipeEntryBlock(PipeEntryBlock.PipeDirection.Left, PipeEntryBlock.PipeDirection.Down, null, null).WithPosition(x, y).WithScale(_scale),
            _ => throw new ArgumentException($"Unknown block function: {functionName}")
        };
    }
}