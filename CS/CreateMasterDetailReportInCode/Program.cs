using System;
using DevExpress.XtraReports.UI;

namespace CreateMasterDetailReportInCode {
    static class Program {
        [STAThread]
        static void Main() {
            XtraReport report = ReportCreator.CreateReport();

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowRibbonPreviewDialog();
        }
    }
}
