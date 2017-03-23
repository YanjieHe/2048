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
        this.Resizable = false;
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
        for (uint i = 0; i < nRows; i++)
        {
            for (uint j = 0; j < nCols; j++)
            {
                this.cells[i * nCols + j].File = "./figures/" + this.lattice[(int)i, (int)j] + ".png";
            }
        }
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}
