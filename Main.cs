using System;
using System.Windows.Forms;
using GameEn;

public class Program {
    [STAThread]
    public static void Main() {
        Application.Run(new WindowE("Super cool fighting game", 800, 640));
    }
}
