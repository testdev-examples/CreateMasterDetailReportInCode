Imports System
Imports DevExpress.XtraReports.UI

Namespace CreateMasterDetailReportInCode
	Friend Module Program
		<STAThread>
		Sub Main()
			Dim report As XtraReport = ReportCreator.CreateReport()

			Dim printTool As New ReportPrintTool(report)
			printTool.ShowRibbonPreviewDialog()
		End Sub
	End Module
End Namespace
