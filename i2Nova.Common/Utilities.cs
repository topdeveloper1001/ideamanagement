using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace i2Nova.Common
{
    public class Utilities
    {
        public static void ExportGridViewDataToCsv(GridView gvObj, HttpResponseBase Response, List<string> Columns,
            string sExportedFileName)
        {
            StringBuilder sb = new StringBuilder();
            string sListSeprator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            sb.Append("sep=" + sListSeprator);
            sb.Append("\r\n");
            //gvObj.AllowPaging = false;
            //gvObj.DataBind();
            for (int k = 1; k < gvObj.Columns.Count; k++)
            {
                //add separator
                string ColumnName = gvObj.Columns[k].HeaderText;
                if (Columns.Contains(ColumnName))
                {
                    sb.Append(ColumnName + sListSeprator);

                }
            }
            //append new line
            sb.Append("\r\n");
            for (int i = 0; i < gvObj.Rows.Count; i++)
            {
                for (int k = 1; k < gvObj.Columns.Count; k++)
                {
                    //add separator
                    sb.Append(gvObj.Rows[i].Cells[k].Text.Replace("&nbsp;", string.Empty) + sListSeprator);
                }

                sb.Append("\r\n");
            }

            WriteCsvFile(Response, sb, sExportedFileName);

        }

        public static void ExportDataTableToCsv(DataTable dtData, HttpResponseBase Response, Dictionary<string, string> Columns,
            string sExportedFileName)
        {
            var sb = new StringBuilder();
            string sListSeprator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            sb.Append("sep=" + sListSeprator);
            sb.Append("\r\n");
            string[] columnNames = dtData.Columns.Cast<DataColumn>().
                Select(column => Columns[column.ColumnName]).
                ToArray();
            sb.Append(string.Join(",", columnNames));

            sb.Append("\r\n");
            string[] items = null;
            foreach (DataRow row in dtData.Rows)
            {
                items = row.ItemArray.Select(o => QuoteValue(o.ToString())).ToArray();
                sb.Append(String.Join(",", items));
                sb.Append("\r\n");
            }

            WriteCsvFile(Response, sb, sExportedFileName);
        }

        public static void ExportDataTableToExcel(HttpResponse Response, DataTable dt, string sFileName)
        {
            if (dt.Rows.Count > 0)
            {
                //string filename = "Job Application List.xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;

                dgGrid.DataBind();

                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + sFileName + ".xls");

                Response.Write(tw.ToString());
                Response.End();
            }
        }


        public static void ExportDataTableToWord(HttpResponse Response, DataTable dt, string sFileName)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            DataGrid dgGrid = new DataGrid();
            dgGrid.DataSource = dt;

            dgGrid.DataBind();
            dgGrid.RenderControl(hw);
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
                "attachment;filename=" + sFileName + ".doc");
            // Response.Charset = "";
            Response.ContentType = "application/vnd.ms-word";
            // Response.ContentEncoding = Encoding.GetEncoding("windows-1256");
            //Response.Charset = "windows-1254";
            //  GridView1.RenderControl(hw);
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.Write(sw.ToString());
            // Response.Flush();
            Response.End();
        }

        private static void WriteCsvFile(HttpResponseBase Response, StringBuilder sb, string sExportedFileName)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition",
                "attachment;filename=" + sExportedFileName + ".csv");
            // Response.ContentType = "application/text";
            Response.ContentEncoding = Encoding.GetEncoding("windows-1256");
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        private static string QuoteValue(string value)
        {
            return String.Concat("\"", value.Replace("\"", "\"\""), "\"");
        }

        public static string GetUrlWithoutQueryString(HttpRequest Request)
        {
            return Request.Url.GetLeftPart(UriPartial.Path);
        }

    }
}