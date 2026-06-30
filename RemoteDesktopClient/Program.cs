namespace RemoteDesktopClient;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // CŨ: Application.Run(new FormClient());
        // MỚI: Mở Form lựa chọn trước
        Application.Run(new FormLauncher());
    }
}