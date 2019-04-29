Imports System.Drawing
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI

Namespace CreateMasterDetailReportInCode
	Public Class ReportCreator
		Public Shared Function CreateReport() As XtraReport
			Dim report As New XtraReport() With {
				.DataSource = CreateDataSource(),
				.DataMember = "Categories",
				.StyleSheet = {
					New XRControlStyle() With {
						.Name = "Title",
						.Font = New Font("Tahoma", 20F, FontStyle.Bold),
						.Padding = New PaddingInfo(2, 2, 0, 0)
					},
					New XRControlStyle() With {
						.Name = "Master",
						.Font = New Font("Tahoma", 12F, FontStyle.Bold),
						.TextAlignment = TextAlignment.BottomLeft,
						.Padding = New PaddingInfo(2, 2, 0, 8)
					},
					New XRControlStyle() With {
						.Name = "Detail",
						.Font = New Font("Tahoma", 9F),
						.Padding = New PaddingInfo(2, 2, 0, 0)
					}
				}
			}
			ConfigureMasterReport(report)

			Dim detailReportBand As New DetailReportBand() With {
				.DataSource = report.DataSource,
				.DataMember = "Categories.CategoriesProducts"
			}
			report.Bands.Add(detailReportBand)
			ConfigureDetailReport(detailReportBand)

			Return report
		End Function
		Private Shared Sub ConfigureMasterReport(ByVal report As XtraReportBase)
			Dim reportHeaderBand As New ReportHeaderBand() With {.HeightF = 30}
			Dim titleLabel As New XRLabel() With {
				.Text = "Products List",
				.BoundsF = New RectangleF(0, 0, 300, 30),
				.StyleName = "Title"
			}
			reportHeaderBand.Controls.Add(titleLabel)

			Dim detailBand As New DetailBand() With {
				.HeightF = 40,
				.KeepTogetherWithDetailReports = True
			}
			Dim detailLabel As New XRLabel() With {
				.ExpressionBindings = { New ExpressionBinding("Text", "CategoryName") },
				.BoundsF = New RectangleF(0, 0, 300, 40),
				.StyleName = "Master"
			}
			detailBand.Controls.Add(detailLabel)

			report.Bands.AddRange(New Band() { reportHeaderBand, detailBand })
		End Sub
		Private Shared Sub ConfigureDetailReport(ByVal report As XtraReportBase)
			Dim detailBand As New DetailBand() With {.HeightF = 22}
			Dim detailLabel As New XRLabel() With {
				.ExpressionBindings = { New ExpressionBinding("Text", "ProductName") },
				.BoundsF = New RectangleF(0, 0, 300, 22),
				.StyleName = "Detail"
			}
			detailBand.Controls.Add(detailLabel)
			report.Bands.Add(detailBand)
		End Sub
		Private Shared Function CreateDataSource() As Object
			Dim connectionParameters As New Access97ConnectionParameters("nwind.mdb", "", "")
			Dim sqlDataSource As New SqlDataSource(connectionParameters)

			Dim queryCategories As SelectQuery = SelectQueryFluentBuilder.AddTable("Categories").SelectAllColumnsFromTable().Build("Categories")
			Dim queryProducts As SelectQuery = SelectQueryFluentBuilder.AddTable("Products").SelectAllColumnsFromTable().Build("Products")
			sqlDataSource.Queries.AddRange(New SqlQuery() { queryCategories, queryProducts })

			sqlDataSource.Relations.Add("Categories", "Products", "CategoryID", "CategoryID")

			Return sqlDataSource
		End Function
	End Class
End Namespace
