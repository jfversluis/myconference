namespace MyConference;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("sessions/detail", typeof(Views.SessionDetailPage));
        Routing.RegisterRoute("speakers/detail", typeof(Views.SpeakerDetailPage));
    }
}
