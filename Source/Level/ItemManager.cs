// Made By Teddy
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using MagicBrosMario.Source.Items;

namespace MagicBrosMario.Source.Level;

public static class ItemManager
{
    private static readonly Dictionary<string, Func<int, int, IItems>> ItemConstructors = new();
    private static string xmlPath = "Content/LevelData/Items.xml";
    private static readonly int _scale = 2;

    public static void Initialize(Texture2D texture)
    {
        ItemConstructors.Clear();
        var doc = XDocument.Load(xmlPath);

        foreach (var itemElement in doc.Descendants("Item"))
        {
            var id = itemElement.Attribute("id")?.Value;
            var function = itemElement.Attribute("function")?.Value;
            //Console.WriteLine($"ItemManager: Found item with ID '{id}' and function '{function}'");
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(function))
            {
                ItemConstructors[id] = GetItemConstructor(function);
            }
        }
        ItemFactory.BindTexture(texture);
    }

    public static IItems CreateItem(string itemId, int x, int y)
    {
        if (ItemConstructors.TryGetValue(itemId, out var constructor))
        {
             return constructor(x, y);
        }

        throw new KeyNotFoundException($"Item with ID '{itemId}' not found.");
    }

    private static Func<int, int, IItems> GetItemConstructor(string functionName)
    {
        return functionName switch
        {
            "Coin" => (x, y) => ItemFactory.CreateCoin(x, y), // wont need anything but coin for sprint3
            //"Mushroom" => (x, y) => ItemFactory.CreateMushroom(x, y),
            "Fireflower" => (x, y) => ItemFactory.CreateFireFlower(x, y), // all of these items are in the mystery box
            "Star" => (x, y) => ItemFactory.CreateStar(x, y),
            "OneUp" => (x, y) => ItemFactory.CreateOneUp(x, y),
            _ => throw new ArgumentException($"Unknown item function: {functionName}")
        };
    }
}