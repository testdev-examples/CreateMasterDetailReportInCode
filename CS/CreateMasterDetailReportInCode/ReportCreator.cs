using System.Drawing;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace CreateMasterDetailReportInCode {
    public class ReportCreator {
        public static XtraReport CreateReport() {
            XtraReport report = new XtraReport() {
                DataSource = CreateDataSource(),
                DataMember = "Categories",
                StyleSheet = {
                    new XRControlStyle() { Name = "Title", Font = new Font("Tahoma", 20f, FontStyle.Bold), Padding = new PaddingInfo(2, 2, 0, 0) },
                    new XRControlStyle() { Name = "Master", Font = new Font("Tahoma", 12f, FontStyle.Bold), TextAlignment = TextAlignment.BottomLeft, Padding = new PaddingInfo(2, 2, 0, 8) },
                    new XRControlStyle() { Name = "Detail", Font = new Font("Tahoma", 9f), Padding = new PaddingInfo(2, 2, 0, 0) },
                }
            };
            ConfigureMasterReport(report);

            DetailReportBand detailReportBand = new DetailReportBand() {
                DataSource = report.DataSource,
                DataMember = "Categories.CategoriesProducts"
            };
            report.Bands.Add(detailReportBand);
            ConfigureDetailReport(detailReportBand);

            return report;
        }
        static void ConfigureMasterReport(XtraReportBase report) {
            ReportHeaderBand reportHeaderBand = new ReportHeaderBand() {
                HeightF = 30
            };
            XRLabel titleLabel = new XRLabel() {
                Text = "Products List",
                BoundsF = new RectangleF(0, 0, 300, 30),
                StyleName = "Title"
            };
            reportHeaderBand.Controls.Add(titleLabel);

            DetailBand detailBand = new DetailBand() {
                HeightF = 40,
                KeepTogetherWithDetailReports = true,
            };
            XRLabel detailLabel = new XRLabel() {
                ExpressionBindings = { new ExpressionBinding("Text", "CategoryName") },
                BoundsF = new RectangleF(0, 0, 300, 40),
                StyleName = "Master",
            };
            detailBand.Controls.Add(detailLabel);

            report.Bands.AddRange(new Band[] { reportHeaderBand, detailBand });
        }
        static void ConfigureDetailReport(XtraReportBase report) {
            DetailBand detailBand = new DetailBand() {
                HeightF = 22
            };
            XRLabel detailLabel = new XRLabel() {
                ExpressionBindings = { new ExpressionBinding("Text", "ProductName") },
                BoundsF = new RectangleF(0, 0, 300, 22),
                StyleName = "Detail",
            };
            detailBand.Controls.Add(detailLabel);
            report.Bands.Add(detailBand);
        }
        static object CreateDataSource() {
            Access97ConnectionParameters connectionParameters = new Access97ConnectionParameters("nwind.mdb", "", "");
            SqlDataSource sqlDataSource = new SqlDataSource(connectionParameters);

            SelectQuery queryCategories = SelectQueryFluentBuilder.AddTable("Categories").SelectAllColumnsFromTable().Build("Categories");
            SelectQuery queryProducts = SelectQueryFluentBuilder.AddTable("Products").SelectAllColumnsFromTable().Build("Products");
            sqlDataSource.Queries.AddRange(new SqlQuery[] { queryCategories, queryProducts });

            sqlDataSource.Relations.Add("Categories", "Products", "CategoryID", "CategoryID");

            return sqlDataSource;
        }
    }
}
