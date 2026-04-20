using System;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Block;

public static class BlockFactory
{
    private static readonly SharedTexture BlockSharedTexture = new SharedTexture();
    public const string TextureName = "blocks";

    public static void BindTexture(Texture2D texture)
    {
        BlockSharedTexture.BindTexture(texture);
    }

    public static Block GroundBlock()
    {
        var sprite = BlockSharedTexture.NewSprite(16, 32, 16, 16);
        var block = new Block(sprite);
        return block;
    }

    public static Block BlueGroundBlock()
    {
        var sprite = BlockSharedTexture.NewSprite(32, 32, 16, 16);
        var block = new Block(sprite);
        return block;
    }

    public static BrickBlock Bricks()
    {
        var sprite = BlockSharedTexture.NewSprite(48, 32, 16, 16);
        var block = new BrickBlock(sprite);
        return block;
    }

    public static Block BlueBricks()
    {
        var sprite = BlockSharedTexture.NewSprite(64, 32, 16, 16);
        var block = new Block(sprite);
        return block;
    }

    public static Block BaseBlock()
    {
        var sprite = BlockSharedTexture.NewSprite(80, 32, 16, 16);
        var block = new Block(sprite);
        return block;
    }

    public static QuestionMarkBlock QuestionMarkBlock(QuestionMarkBlock.InnerItem item)
    {
        var sprite = BlockSharedTexture.NewAnimatedSprite(16, 48, 16, 16, 3, 0.2f);
        var emptySprite = BlockSharedTexture.NewSprite(64, 48, 16, 16);
        var block = new QuestionMarkBlock(sprite, emptySprite, item);
        return block;
    }

    public static QuestionMarkBlock InvisQuestionMarkBlock(QuestionMarkBlock.InnerItem item)
    {
        AnimatedSprite sprite = null;
        var emptySprite = BlockSharedTexture.NewSprite(64, 48, 16, 16);
        var block = new QuestionMarkBlock(sprite, emptySprite, item);
        return block;
    }

    public static QuestionMarkBlock BrickQuestionMarkBlock(QuestionMarkBlock.InnerItem item)
    {
        var bricksSprite = BlockSharedTexture.NewAnimatedSprite(48, 32, 16, 16, 1, 0.0f);
        var emptySprite = BlockSharedTexture.NewSprite(64, 48, 16, 16);
        var block = new QuestionMarkBlock(bricksSprite, emptySprite, item);
        return block;
    }


    public enum PipeSegmentType
    {
        Vertical,
        Horizontal,
        ConnectLeft,
        ConnectRight
    }

    public static Block PipeBlock(
        PipeSegmentType type,
        PipeEntryBlock.PipeDirection half
    )
    {
        Sprite.Sprite sprite;
        switch (type)
        {
            case PipeSegmentType.Vertical:
                if (half == Source.Block.PipeEntryBlock.PipeDirection.Left)
                {
                    sprite = BlockSharedTexture.NewSprite(16, 96, 16, 16);
                } else
                {
                    sprite = BlockSharedTexture.NewSprite(32, 96, 16, 16);
                }

                break;
            case PipeSegmentType.Horizontal:
                if (half == Source.Block.PipeEntryBlock.PipeDirection.Up)
                {
                    sprite = BlockSharedTexture.NewSprite(64, 80, 16, 16);
                } else
                {
                    sprite = BlockSharedTexture.NewSprite(64, 96, 16, 16);
                }

                break;
            case PipeSegmentType.ConnectLeft:
                if (half == Source.Block.PipeEntryBlock.PipeDirection.Up)
                {
                    sprite = BlockSharedTexture.NewSprite(80, 80, 16, 16);
                } else
                {
                    sprite = BlockSharedTexture.NewSprite(80, 96, 16, 16);
                }

                break;
            case PipeSegmentType.ConnectRight:
                if (half == Source.Block.PipeEntryBlock.PipeDirection.Up)
                {
                    sprite = BlockSharedTexture.NewSprite(80, 80, 16, 16);
                } else
                {
                    sprite = BlockSharedTexture.NewSprite(80, 96, 16, 16);
                }

                sprite.HFlipped = true;
                break;
            default:
                throw new Exception("unreachable default case");
        }

        return new Block(sprite);
    }

    public static PipeEntryBlock PipeEntryBlock(
        PipeEntryBlock.PipeDirection facing,
        PipeEntryBlock.PipeDirection half,
        Point? exit,
        PipeEntryBlock.PipeDirection? exitDirection
    )
    {
        Sprite.Sprite sprite;
        switch (half)
        {
            case Source.Block.PipeEntryBlock.PipeDirection.Left:
                sprite = BlockSharedTexture.NewSprite(16, 80, 16, 16);
                if (facing == Source.Block.PipeEntryBlock.PipeDirection.Down)
                {
                    sprite.VFlipped = true;
                }

                break;
            case Source.Block.PipeEntryBlock.PipeDirection.Right:
                sprite = BlockSharedTexture.NewSprite(32, 80, 16, 16);
                if (facing == Source.Block.PipeEntryBlock.PipeDirection.Down)
                {
                    sprite.VFlipped = true;
                }

                break;
            case Source.Block.PipeEntryBlock.PipeDirection.Up:
                sprite = BlockSharedTexture.NewSprite(48, 80, 16, 16);
                break;
            case Source.Block.PipeEntryBlock.PipeDirection.Down:
                sprite = BlockSharedTexture.NewSprite(48, 96, 16, 16);
                break;
            default:
                throw new Exception("unreachable default case");
        }

        return new PipeEntryBlock(
            sprite, facing, half, exit, exitDirection);
    }
}