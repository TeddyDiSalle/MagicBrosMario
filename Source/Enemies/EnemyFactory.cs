using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source;

public static class EnemyFactory {
    private static readonly SharedTexture ENEMY_SHARED_TEXTURE = new SharedTexture();
    //public const string TEXTURE_NAME = "enemies";

    public static void BindTexture(Texture2D texture) {
        ENEMY_SHARED_TEXTURE.BindTexture(texture);
    }

    public static IEnemy CreateGoomba(int x, int y) {
        var enemy = new Goomba(ENEMY_SHARED_TEXTURE);
        enemy.Position = new Point(x, y);
        return enemy;
    }

    public static IEnemy CreateKoopa(int x, int y) {
        var enemy = new Koopa(ENEMY_SHARED_TEXTURE );
        enemy.Position = new Point(x, y);
        return enemy;
    }

    //then on and so forth
}