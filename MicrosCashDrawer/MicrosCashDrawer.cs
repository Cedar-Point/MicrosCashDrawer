using System.Timers;
using MicrosLibrary;

namespace MicrosCashDrawer
{
    class MicrosCashDrawer
    {
        private MicrosLibraryWrapper Micros = new MicrosLibraryWrapper();
        private Timer DrawerCheckTimer = new Timer();
        private int PreviousDrawerStatus = -1;
        public event CashDrawerStatusChangedDelegate CashDrawerStatusChanged;
        public delegate void CashDrawerStatusChangedDelegate(CashDrawerStatus cashDrawerStatus);
        public MicrosCashDrawer()
        {
            Micros.CashDrawerVerifyPassword("000000", "0");
            DrawerCheckTimer.Interval = 10;
            DrawerCheckTimer.Elapsed += DrawerCheckTimer_Elapsed;
            DrawerCheckTimer.Start();
        }
        public int OpenCashDrawer()
        {
            return Micros.OpenCashDrawer(1);
        }
        private void DrawerCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int CurrentDrawerStatus = Micros.CashDrawerStatus(1);
            if (PreviousDrawerStatus != CurrentDrawerStatus && CashDrawerStatusChanged != null)
            {
                CashDrawerStatusChanged.Invoke((CashDrawerStatus)CurrentDrawerStatus);
            }
            PreviousDrawerStatus = CurrentDrawerStatus;
        }
    }
    enum CashDrawerStatus
    {
        OPEN = 1,
        CLOSED = 0
    }
}
