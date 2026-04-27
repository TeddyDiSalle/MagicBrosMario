using System;
using MagicBrosMario.Source.Block;

namespace MagicBrosMario.Source.Level;

internal readonly record struct LevelCellToken(string BlockId, string? PipeLabel);

internal static class LevelCellTokenParser
{
    public static LevelCellToken ParseBlockToken(string token)
    {
        token = token.Trim();

        if (string.IsNullOrEmpty(token))
            return new LevelCellToken("", null);

        int open = token.IndexOf('[');
        if (open < 0)
            return new LevelCellToken(token, null);

        int close = token.IndexOf(']', open + 1);
        if (close < 0)
            throw new FormatException(
                $"Invalid block token '{token}'. Expected format like 10[A0] or [A1].");

        string blockId = token.Substring(0, open).Trim();
        string pipeLabel = token.Substring(open + 1, close - open - 1).Trim();

        return new LevelCellToken(blockId, pipeLabel);
    }

    public static bool IsPipeEntryBlockId(string blockId)
    {
        return blockId is "10" or "11" or "14" or "15";
    }

    public static void ParsePipeLabel(string pipeLabel, out string group, out int endpoint)
    {
        if (string.IsNullOrWhiteSpace(pipeLabel) || pipeLabel.Length < 2)
            throw new Exception($"Invalid pipe label '{pipeLabel}'. Use labels like A0 or B1.");

        char last = pipeLabel[^1];
        if (last != '0' && last != '1')
            throw new Exception($"Invalid pipe label '{pipeLabel}'. Final character must be 0 or 1.");

        group = pipeLabel.Substring(0, pipeLabel.Length - 1);
        endpoint = last - '0';
    }

    public static QuestionMarkBlock.InnerItem? ToQuestionBlockItem(string itemId)
    {
        return itemId switch
        {
            "00" => QuestionMarkBlock.InnerItem.Coin,
            "01" => QuestionMarkBlock.InnerItem.Mushroom,
            "02" => QuestionMarkBlock.InnerItem.Star,
            "03" => QuestionMarkBlock.InnerItem.OneUp,
            "04" => QuestionMarkBlock.InnerItem.Mushroom,
            "13" => QuestionMarkBlock.InnerItem.AntiGravityCloud,
            "15" => QuestionMarkBlock.InnerItem.PoisonMushroom,
            _ => null
        };
    }
}