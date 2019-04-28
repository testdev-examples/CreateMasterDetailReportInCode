Imports System.Drawing
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI

Namespace CreateMasterDetailReportInCode
    Public Class ReportCreator
        Public Shared Function CreateReport() As XtraReport
            Dim report As New XtraReport() With { _
                .DataSource = CreateDataSource(), _
                .DataMember = "Categories", _
                .StyleSheet = { _
                    New XRControlStyle() With { _
                        .Name = "Title", _
                        .Font = New Font("Tahoma", 20F, FontStyle.Bold), _
                        .Padding = New PaddingInfo(2, 2, 0, 0) _
                    }, _
                    New XRControlStyle() With { _
                        .Name = "Master", _
                        .Font = New Font("Tahoma", 12F, FontStyle.Bold), _
                        .TextAlignment = TextAlignment.BottomLeft, _
                        .Padding = New PaddingInfo(2, 2, 0, 8) _
                    }, _
                    New XRControlStyle() With { _
                        .Name = "Detail", _
                        .Font = New Font("Tahoma", 9F), _
                        .Padding = New PaddingInfo(2, 2, 0, 0) _
                    } _
                } _
            }
            ConfigureMasterReport(report)

            Dim detailReportBand As New DetailReportBand() With { _
                .DataSource = report.DataSource, _
                .DataMember = "Categories.CategoriesProducts" _
            }
            report.Bands.Add(detailReportBand)
            ConfigureDetailReport(detailReportBand)

            Return report
        End Function
        Private Shared Sub ConfigureMasterReport(ByVal report As XtraReportBase)
            Dim reportHeaderBand As New ReportHeaderBand() With {.HeightF = 30}
            Dim titleLabel As New XRLabel() With { _
                .Text = "Products List", _
                .BoundsF = New RectangleF(0, 0, 300, 30), _
                .StyleName = "Title" _
            }
            reportHeaderBand.Controls.Add(titleLabel)

            Dim detailBand As New DetailBand() With { _
                .HeightF = 40, _
                .KeepTogetherWithDetailReports = True _
            }
            Dim detailLabel As New XRLabel() With { _
                .ExpressionBindings = { New ExpressionBinding("Text", "CategoryName") }, _
                .BoundsF = New RectangleF(0, 0, 300, 40), _
                .StyleName = "Master" _
            }
            detailBand.Controls.Add(detailLabel)

            report.Bands.AddRange(New Band() { reportHeaderBand, detailBand })
        End Sub
        Private Shared Sub ConfigureDetailReport(ByVal report As XtraReportBase)
            Dim detailBand As New DetailBand() With {.HeightF = 22}
            Dim detailLabel As New XRLabel() With { _
                .ExpressionBindings = { New ExpressionBinding("Text", "ProductName") }, _
                .BoundsF = New RectangleF(0, 0, 300, 22), _
                .StyleName = "Detail" _
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
