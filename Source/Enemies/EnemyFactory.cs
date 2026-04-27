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
        var enemy = new Goomba(ENEMY_SHARED_TEXTURE,y,x);
        //enemy.Position = new Point(x, y);
        return enemy;
    }

    public static IEnemy CreateKoopa(int x, int y) {
        var enemy = new Koopa(ENEMY_SHARED_TEXTURE,y,x);
        //enemy.Position = new Point(x, y);
        return enemy;
    }

    public static IEnemy CreateBowser(int x, int y, SharedTexture fireTexture) {
        var enemy = new Bowser(ENEMY_SHARED_TEXTURE, fireTexture, y, x);
        return enemy;
    }

    public static IEnemy CreatePiranhaPlant(int x, int y) {
        var enemy = new PiranhaPlant(ENEMY_SHARED_TEXTURE,y,x);
        return enemy;
    }

    public static IEnemy CreateRotatingFireBar(int x, int y) {
        var enemy = new RotatingFireBar(MagicBrosMario.INSTANCE.FireTexture,y,x, 6, 16);
        return enemy;
    }

    public static IEnemy CreateMeteorMage(int x, int y) {
        var enemy = new MeteorMage(ENEMY_SHARED_TEXTURE,y,x);
        return enemy;
    }

    //then on and so forth
}