using System;
using System.Windows.Forms;
using GameEn;

public class Program {
    [STAThread]
    public static void Main() {
        /*SuperCoolFightingGame.SuperCoolFightingGame game = new SuperCoolFightingGame.SuperCoolFightingGame(800, 640, "Super Cool Fighting Game");

        game.Run();*/
        Application.Run(new WindowE("Super cool fighting game", 800, 640));
    }
}
