using System.Timers;
using MicrosLibrary;

namespace MicrosCashDrawer
{
    public class CashDrawer
    {
        private MicrosLibraryWrapper Micros = new MicrosLibraryWrapper();
        private Timer DrawerCheckTimer = new Timer();
        private CashDrawerStatus PreviousDrawerStatus = CashDrawerStatus.UNKNOWN;
        private uint CDNumber = 1;
        public event CashDrawerStatusChangedDelegate CashDrawerStatusChanged;
        public delegate void CashDrawerStatusChangedDelegate(CashDrawerStatus cashDrawerStatus);
        public CashDrawerStatus CashDrawerStatus { get; private set; } = CashDrawerStatus.UNKNOWN;
        public CashDrawer(uint CashDrawerNumber = 1)
        {
            CDNumber = CashDrawerNumber;
            Micros.CashDrawerVerifyPassword("000000", "0");
            CashDrawerStatus = (CashDrawerStatus)Micros.CashDrawerStatus(CDNumber);
            DrawerCheckTimer.Interval = 10;
            DrawerCheckTimer.Elapsed += DrawerCheckTimer_Elapsed;
            DrawerCheckTimer.Start();
        }
        public int OpenCashDrawer()
        {
            return Micros.OpenCashDrawer(CDNumber);
        }
        
        private void DrawerCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CashDrawerStatus = (CashDrawerStatus)Micros.CashDrawerStatus(CDNumber);
            if (PreviousDrawerStatus != CashDrawerStatus && CashDrawerStatusChanged != null)
            {
                CashDrawerStatusChanged.Invoke(CashDrawerStatus);
            }
            PreviousDrawerStatus = CashDrawerStatus;
        }
    }
    public enum CashDrawerStatus
    {
        UNKNOWN = -1,
        OPEN = 1,
        CLOSED = 0
    }
}
