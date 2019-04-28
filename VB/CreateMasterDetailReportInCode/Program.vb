Imports System
Imports DevExpress.XtraReports.UI

Namespace CreateMasterDetailReportInCode
    Friend NotInheritable Class Program

        Private Sub New()
        End Sub

        <STAThread> _
        Shared Sub Main()
            Dim report As XtraReport = ReportCreator.CreateReport()

            Dim printTool As New ReportPrintTool(report)
            printTool.ShowRibbonPreviewDialog()
        End Sub
    End Class
End Namespace
