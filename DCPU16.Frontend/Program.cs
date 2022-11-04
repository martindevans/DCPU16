using Terminal.Gui;

class Demo
{
    static void Main()
    {
        Application.Init();

        var menubar = new MenuBar(new[] {
            new MenuBarItem ("_File", new[] {
                new MenuItem ("_New", "", () => {
                    
                }),
                new MenuItem ("_Load", "", () => {

                }),
                new MenuItem ("_Save", "", () => {

                }),
                new MenuItem ("_Quit", "", () => {
                    Application.RequestStop ();
                })
            }),
            new MenuBarItem ("_Edit", new[] {
                new MenuItem ("_Copy", "", () => {

                }),
                new MenuItem ("C_ut", "", () => {

                }),
                new MenuItem ("_Paste", "", () => {

                }),
                new MenuItem ("_Delete", "", () => {
                    
                }),
                new MenuItem ("_Find", "", () => {

                })
            }),
            new MenuBarItem ("_Build", new[] {
                new MenuItem ("_Build", "", () => {

                }),
                new MenuItem ("_Clean", "", () => {

                }),
                new MenuItem ("_Run", "", () => {

                }),
            }),
        });

        var hex = new FrameView("Assembly")
        {
            X = 1 ,
            Y = 0,
            Width = 30,
            Height = Dim.Fill(),
        };
        hex.Add(new HexView(File.Open("C:\\Users\\Martin\\Documents\\dotnet\\DCPU16\\DCPU16.Tests\\Assembler\\DASM\\pacman.hex", FileMode.Open))
        {
            X = 1,
            Y = 0,
            Width = Dim.Fill() - 1,
            Height = Dim.Fill()
        });

        var txt = new FrameView("Code")
        {
            X = 31,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };
        var code = new TextView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        txt.Add(code);

        var status = new StatusBar(
            new []
            {
                new StatusItem(Key.K, "Hello", () => { })
            }
        );

        var view = new View
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 1,
        };
        view.Add(hex);
        view.Add(txt);

        // Add both menu and win in a single call
        Application.Top.Add(menubar, view, status);
        Application.Run();
        Application.Shutdown();
    }
}