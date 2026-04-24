using System;
using System.Collections.Generic;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Level;

internal sealed class DeferredPipeLinkResolver
{
    private sealed class PendingPipeEntrance
    {
        public string Label { get; }
        public Dictionary<string, Point> HalvesByBlockId { get; } = new();

        public PendingPipeEntrance(string label)
        {
            Label = label;
        }
    }

    private sealed class PendingMarkerEndpoint
    {
        public string Label { get; }
        public List<Point> Tiles { get; } = new();

        public PendingMarkerEndpoint(string label)
        {
            Label = label;
        }
    }

    private sealed class PendingGroup
    {
        public PendingPipeEntrance? Pipe0 { get; set; }
        public PendingPipeEntrance? Pipe1 { get; set; }
        public PendingMarkerEndpoint? Marker0 { get; set; }
        public PendingMarkerEndpoint? Marker1 { get; set; }
    }

    private readonly Dictionary<string, PendingPipeEntrance> _pipeEntrances = new();
    private readonly Dictionary<string, PendingMarkerEndpoint> _markerEndpoints = new();

    public void Clear()
    {
        _pipeEntrances.Clear();
        _markerEndpoints.Clear();
    }

    public void RegisterPipeHalf(string blockId, int row, int col, string pipeLabel)
    {
        if (!LevelCellTokenParser.IsPipeEntryBlockId(blockId))
        {
            throw new Exception(
                $"Block id '{blockId}' is not a valid pipe entry block for label '{pipeLabel}'.");
        }

        if (!_pipeEntrances.TryGetValue(pipeLabel, out var entrance))
        {
            entrance = new PendingPipeEntrance(pipeLabel);
            _pipeEntrances[pipeLabel] = entrance;
        }

        if (entrance.HalvesByBlockId.ContainsKey(blockId))
        {
            throw new Exception($"Pipe label '{pipeLabel}' has duplicate block id '{blockId}'.");
        }

        entrance.HalvesByBlockId[blockId] = new Point(col, row);
    }

    public void RegisterMarker(int row, int col, string pipeLabel)
    {
        if (!_markerEndpoints.TryGetValue(pipeLabel, out var marker))
        {
            marker = new PendingMarkerEndpoint(pipeLabel);
            _markerEndpoints[pipeLabel] = marker;
        }

        marker.Tiles.Add(new Point(col, row));
    }

    public void FinalizeLinks(IBlock[][] blocks, int tileSize)
    {
        Dictionary<string, PendingGroup> groups = BuildGroups();

        foreach (var kvp in groups)
        {
            string groupName = kvp.Key;
            PendingGroup group = kvp.Value;

            bool has0 = group.Pipe0 != null || group.Marker0 != null;
            bool has1 = group.Pipe1 != null || group.Marker1 != null;

            if (!has0 || !has1)
            {
                throw new Exception(
                    $"Pipe group '{groupName}' is incomplete. Both {groupName}0 and {groupName}1 must exist.");
            }

            if (group.Pipe0 != null)
            {
                BuildSourcePipe(
                    group.Pipe0,
                    GetTeleportForEndpoint(group.Pipe1, group.Marker1),
                    blocks,
                    tileSize
                );
            }

            /*if (group.Pipe1 != null)
            {
                BuildSourcePipe(
                    group.Pipe1,
                    GetTeleportForEndpoint(group.Pipe0, group.Marker0),
                    blocks,
                    tileSize
                );
            }*/
            if (group.Pipe1 != null)
            {
                BuildEndPipe(
                    group.Pipe1,
                    blocks,
                    tileSize
                );
            }
            

            if (group.Pipe0 == null && group.Pipe1 == null)
            {
                throw new Exception(
                    $"Pipe group '{groupName}' has no actual source pipe. At least one endpoint must be a pipe.");
            }
        }

        Clear();
    }

    private Dictionary<string, PendingGroup> BuildGroups()
    {
        var grouped = new Dictionary<string, PendingGroup>();

        foreach (PendingPipeEntrance entrance in _pipeEntrances.Values)
        {
            ValidateEntrance(entrance);
            LevelCellTokenParser.ParsePipeLabel(entrance.Label, out string group, out int endpoint);

            if (!grouped.TryGetValue(group, out PendingGroup? pendingGroup))
            {
                pendingGroup = new PendingGroup();
                grouped[group] = pendingGroup;
            }

            if (endpoint == 0)
            {
                if (pendingGroup.Pipe0 != null)
                    throw new Exception($"Pipe group '{group}' has more than one pipe 0 endpoint.");
                pendingGroup.Pipe0 = entrance;
            }
            else
            {
                if (pendingGroup.Pipe1 != null)
                    throw new Exception($"Pipe group '{group}' has more than one pipe 1 endpoint.");
                pendingGroup.Pipe1 = entrance;
            }
        }

        foreach (PendingMarkerEndpoint marker in _markerEndpoints.Values)
        {
            LevelCellTokenParser.ParsePipeLabel(marker.Label, out string group, out int endpoint);

            if (!grouped.TryGetValue(group, out PendingGroup? pendingGroup))
            {
                pendingGroup = new PendingGroup();
                grouped[group] = pendingGroup;
            }

            if (endpoint == 0)
            {
                if (pendingGroup.Marker0 != null)
                    throw new Exception($"Pipe group '{group}' has more than one marker 0 endpoint.");
                pendingGroup.Marker0 = marker;
            }
            else
            {
                if (pendingGroup.Marker1 != null)
                    throw new Exception($"Pipe group '{group}' has more than one marker 1 endpoint.");
                pendingGroup.Marker1 = marker;
            }
        }

        return grouped;
    }

    private static void ValidateEntrance(PendingPipeEntrance entrance)
    {
        bool upPipe = IsUpPipeEntrance(entrance);
        bool leftPipe = IsLeftPipeEntrance(entrance);

        if (upPipe && entrance.HalvesByBlockId.Count != 2)
            throw new Exception($"Pipe '{entrance.Label}' must contain exactly 10 and 11.");

        if (leftPipe && entrance.HalvesByBlockId.Count != 2)
            throw new Exception($"Pipe '{entrance.Label}' must contain exactly 14 and 15.");

        if (!upPipe && !leftPipe)
        {
            throw new Exception(
                $"Pipe '{entrance.Label}' is invalid. It must contain either [10,11] or [14,15].");
        }
    }

    private static bool IsUpPipeEntrance(PendingPipeEntrance entrance)
    {
        return entrance.HalvesByBlockId.ContainsKey("10")
            && entrance.HalvesByBlockId.ContainsKey("11");
    }

    private static bool IsLeftPipeEntrance(PendingPipeEntrance entrance)
    {
        return entrance.HalvesByBlockId.ContainsKey("14")
            && entrance.HalvesByBlockId.ContainsKey("15");
    }

    private static Point GetPipeAnchorTile(PendingPipeEntrance entrance)
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;

        foreach (Point p in entrance.HalvesByBlockId.Values)
        {
            if (p.X < minX) minX = p.X;
            if (p.Y < minY) minY = p.Y;
        }

        return new Point(minX, minY);
    }

    private static Point GetMarkerAnchorTile(PendingMarkerEndpoint marker)
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;

        foreach (Point p in marker.Tiles)
        {
            if (p.X < minX) minX = p.X;
            if (p.Y < minY) minY = p.Y;
        }

        return new Point(minX, minY);
    }

    private static PipeEntryBlock.PipeDirection GetExitDirectionFromDestination(PendingPipeEntrance destination)
    {
        if (IsUpPipeEntrance(destination))
            return PipeEntryBlock.PipeDirection.Up;

        if (IsLeftPipeEntrance(destination))
            return PipeEntryBlock.PipeDirection.Left;

        throw new Exception($"Could not determine exit direction for pipe '{destination.Label}'.");
    }

    private static BlockManager.PipeTeleport GetTeleportForEndpoint(
        PendingPipeEntrance? destinationPipe,
        PendingMarkerEndpoint? destinationMarker)
    {
        if (destinationPipe != null)
        {
            return new BlockManager.PipeTeleport(
                GetPipeAnchorTile(destinationPipe),
                GetExitDirectionFromDestination(destinationPipe)
            );
        }

        if (destinationMarker != null)
        {
            return new BlockManager.PipeTeleport(
                GetMarkerAnchorTile(destinationMarker),
                null
            );
        }

        throw new Exception("Destination endpoint is missing.");
    }

    private static void BuildSourcePipe(
        PendingPipeEntrance source,
        BlockManager.PipeTeleport teleport,
        IBlock[][] blocks,
        int tileSize)
    {
        foreach (var kvp in source.HalvesByBlockId)
        {
            string blockId = kvp.Key;
            Point tile = kvp.Value;

            IBlock block = BlockManager.CreateBlock(
                blockId,
                tile.X * tileSize,
                tile.Y * tileSize,
                null,
                teleport
            );

            blocks[tile.Y][tile.X] = block;
            CollisionController.Instance.AddBlock(block);
        }
    }

    private static void BuildEndPipe(
        PendingPipeEntrance source,
        IBlock[][] blocks,
        int tileSize)
    {
        foreach (var kvp in source.HalvesByBlockId)
        {
            string blockId = kvp.Key;
            Point tile = kvp.Value;

            IBlock block = BlockManager.CreateBlock(
                blockId,
                tile.X * tileSize,
                tile.Y * tileSize
            );

            blocks[tile.Y][tile.X] = block;
            CollisionController.Instance.AddBlock(block);
        }
    }
}