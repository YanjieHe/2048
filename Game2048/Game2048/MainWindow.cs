using System;
using Gtk;
using Game2048;

public partial class MainWindow: Gtk.Window
{
    public MainWindow()
        : base(Gtk.WindowType.Toplevel)
    {
        Build();
        Initialize();
    }

    int nRows = 4;
    int nCols = 4;
    Image[] cells;
    Lattice lattice;
    public static readonly Gdk.Pixbuf[] figures = new Gdk.Pixbuf[]
    {
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 0 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 2 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 4 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 8 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 16 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 32 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 64 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 128 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 256 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 512 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 1024 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 2048 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 4096 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 8192 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 16384 + ".png")),
        new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./figures/" + 65536 + ".png"))
    };

    void Initialize()
    {
        this.cells = new Image[nRows * nCols];
        for (uint i = 0; i < nRows; i++)
        {
            for (uint j = 0; j < nCols; j++)
            {
                Image area = new Image("./figures/0.png");
                cells[i * nCols + j] = area;
                this.table.Attach(cells[i * nCols + j], i, i + 1, j, j + 1);
            }
        }
        this.table.BorderWidth = 10;
        this.Resizable = false;
        this.ModifyBg(StateType.Normal, new Gdk.Color(182, 165, 149));
        this.KeyReleaseEvent += MainWindow_KeyReleaseEvent;
        this.lattice = new Lattice();
        lattice.Reset();
        ShowLattice();
        this.NewGameAction.Activated += NewGameAction_Activated;
        ShowAll();
    }

    void NewGameAction_Activated(object sender, EventArgs e)
    {
        using (var dialog = new MessageDialog(
                                this, 
                                DialogFlags.DestroyWithParent, 
                                MessageType.Info, 
                                ButtonsType.OkCancel, 
                                "Do you want to start a new game?"))
        {
            int result = dialog.Run();
            const int OK = -5;
            dialog.Hide();
            if (result == OK)
            {
                this.lattice.Reset();
                ShowLattice();
            }
        }
    }

    void MainWindow_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
    {
        // The "Gdk.Key" has some bugs. Left -> Up, Right -> Down; Up -> Left, Down -> Right;
        switch (args.Event.Key)
        {
            case Gdk.Key.Left:
                PlayerTryShift(Game2048.Direction.Up);
                return;
            case Gdk.Key.Right:
                PlayerTryShift(Game2048.Direction.Down);
                return;
            case Gdk.Key.Up:
                PlayerTryShift(Game2048.Direction.Left);
                return;
            case Gdk.Key.Down:
                PlayerTryShift(Game2048.Direction.Right);
                return;
            default:
                return;
        }
    }

    void PlayerTryShift(Direction direction)
    {
        if (lattice.TryShift(direction))
        {
            lattice.RandomSet();
            ShowLattice();
            if (lattice.IsGameOver)
            {
                using (var dialog = new MessageDialog(
                                        this, 
                                        DialogFlags.DestroyWithParent, 
                                        MessageType.Info, 
                                        ButtonsType.Ok, 
                                        "Game over!"))
                {
                    dialog.Run();
                    dialog.Hide();
                }
                return;
            }
        }
    }

    void ShowLattice()
    {
        for (int i = 0; i < nRows; i++)
        {
            for (int j = 0; j < nCols; j++)
            {
                //this.cells[i * nCols + j].File = "./figures/" + this.lattice[(int)i, (int)j] + ".png";
                this.cells[i * nCols + j].Pixbuf = GetFigure(this.lattice[i, j]);
            }
        }
    }

    Gdk.Pixbuf GetFigure(int number)
    {
        if (number == 0)
        {
            return figures[0];
        }
        else
        {
            int n = (int)Math.Log(number, 2);
            return figures[n];
        }
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}
