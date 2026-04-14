// Made By Teddy
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MagicBrosMario.Source.Block;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Level;

public static class BlockManager
{
    private static readonly Dictionary<string, Func<int, int, IBlock>> BlockConstructors = new();
    private static string xmlPath = "Content/LevelData/Blocks.xml";
    private static readonly int _scale = 2;
    public readonly record struct PipeTeleport(
        Point ExitTile,
        PipeEntryBlock.PipeDirection ExitDirection
    );

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

    public static IBlock CreateBlock(
        string blockId,
        int x,
        int y,
        QuestionMarkBlock.InnerItem innerItem = QuestionMarkBlock.InnerItem.Coin,
        PipeTeleport? pipeTeleport = null,
        int tileSize = 32)
    {
        if (blockId == "07") // ? mark block
        {
            return BlockFactory.QuestionMarkBlock(innerItem)
                .WithPosition(x, y)
                .WithScale(_scale);
        }

        if (IsPipeEntryBlock(blockId))
        {
            Point? exitPixels = null;
            PipeEntryBlock.PipeDirection? exitDirection = null;

            if (pipeTeleport.HasValue)
            {
                exitPixels = new Point(
                    pipeTeleport.Value.ExitTile.X * tileSize,
                    pipeTeleport.Value.ExitTile.Y * tileSize
                );
                exitDirection = pipeTeleport.Value.ExitDirection;
                Console.WriteLine($"Creating pipe entry block with exit at tile {pipeTeleport.Value.ExitTile} ({exitPixels.Value}) and direction {exitDirection}");
            }

            return CreatePipeEntryBlock(blockId, x, y, exitPixels, exitDirection);
        }

        if (BlockConstructors.TryGetValue(blockId, out var constructor))
        {
            return constructor(x, y);
        }

        throw new KeyNotFoundException($"Block with ID '{blockId}' not found.");
    }

    private static bool IsPipeEntryBlock(string blockId)
    {
        return blockId is "10" or "11" or "14" or "15";
    }

    private static IBlock CreatePipeEntryBlock(
        string blockId,
        int x,
        int y,
        Point? exit,
        PipeEntryBlock.PipeDirection? exitDirection)
    {
        return blockId switch
        {
            "10" => BlockFactory.PipeEntryBlock(
                        PipeEntryBlock.PipeDirection.Up,
                        PipeEntryBlock.PipeDirection.Left,
                        exit,
                        exitDirection)
                    .WithPosition(x, y)
                    .WithScale(_scale),

            "11" => BlockFactory.PipeEntryBlock(
                        PipeEntryBlock.PipeDirection.Up,
                        PipeEntryBlock.PipeDirection.Right,
                        exit,
                        exitDirection)
                    .WithPosition(x, y)
                    .WithScale(_scale),

            "14" => BlockFactory.PipeEntryBlock(
                        PipeEntryBlock.PipeDirection.Left,
                        PipeEntryBlock.PipeDirection.Up,
                        exit,
                        exitDirection)
                    .WithPosition(x, y)
                    .WithScale(_scale),

            "15" => BlockFactory.PipeEntryBlock(
                        PipeEntryBlock.PipeDirection.Left,
                        PipeEntryBlock.PipeDirection.Down,
                        exit,
                        exitDirection)
                    .WithPosition(x, y)
                    .WithScale(_scale),

            _ => throw new ArgumentException($"Not a pipe entry block: {blockId}")
        };
    }

    private static Func<int, int, IBlock> GetBlockConstructor(string functionName)
    {
        return functionName switch
        {
            "VoidBlock" => (x, y) => BlockFactory.GroundBlock().WithPosition(x, y).WithScale(_scale), // fix xml
            "SkyBlock" => (x, y) => BlockFactory.GroundBlock().WithPosition(x, y).WithScale(_scale), // fix xml
            "GroundBlock" => (x, y) => BlockFactory.GroundBlock().WithPosition(x, y).WithScale(_scale),
            "BlueGroundBlock" => (x, y) => BlockFactory.BlueGroundBlock().WithPosition(x, y).WithScale(_scale),
            "Bricks" => (x, y) => BlockFactory.Bricks().WithPosition(x, y).WithScale(_scale),
            "BlueBricks" => (x, y) => BlockFactory.BlueBricks().WithPosition(x, y).WithScale(_scale),
            "BaseBlock" => (x, y) => BlockFactory.BaseBlock().WithPosition(x, y).WithScale(_scale),
            "QuestionMarkBlock" => (x, y) => BlockFactory.GroundBlock().WithPosition(x, y).WithScale(_scale), // should never be called directly
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