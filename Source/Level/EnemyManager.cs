// Made By Teddy
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Level;

public static class EnemyManager
{
    private static readonly Dictionary<string, Func<int, int, IEnemy>> EnemyConstructors = new();
    private static string xmlPath = "Content/LevelData/Enemies.xml";
    private static readonly int _scale = 2;

    public static void Initialize(Texture2D texture)
    {
        EnemyConstructors.Clear();
        var doc = XDocument.Load(xmlPath);

        foreach (var enemyElement in doc.Descendants("Enemy"))
        {
            var id = enemyElement.Attribute("id")?.Value;
            var function = enemyElement.Attribute("function")?.Value;

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(function))
            {
                
                EnemyConstructors[id] = GetEnemyConstructor(function);
            }
            
        }
        EnemyFactory.BindTexture(texture);
    }

    public static IEnemy CreateEnemy(string enemyId, int x, int y)
    {
        if (EnemyConstructors.TryGetValue(enemyId, out var constructor))
        {
             return constructor(x, y);
        }

        throw new KeyNotFoundException($"Enemy with ID '{enemyId}' not found.");
    }

    private static Func<int, int, IEnemy> GetEnemyConstructor(string functionName)
    {
        return functionName switch
        {
            "Goomba" => (x, y) => EnemyFactory.CreateGoomba(x, y),
            "Koopa" => (x, y) => EnemyFactory.CreateKoopa(x, y),
            "Bowser" => (x, y) => EnemyFactory.CreateBowser(x, y, MagicBrosMario.INSTANCE.FireTexture),
            "PiranhaPlant" => (x, y) => EnemyFactory.CreatePiranhaPlant(x, y),
            "RotatingFireBar" => (x, y) => EnemyFactory.CreateRotatingFireBar(x, y),
            _ => throw new ArgumentException($"Unknown enemy function: {functionName}")
        };
    }
}