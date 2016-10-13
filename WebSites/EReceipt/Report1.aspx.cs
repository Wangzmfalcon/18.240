﻿using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMS.DBUtility;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Drawing;
using System.IO;
using SMS.DBUtility;
using System.Globalization;//日期格式化
using Aspose.Cells;//excel

public partial class Home : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Manage_Level"] == null || Session["UserID"] == null || Session["Receipt"] == null)
        {
            Server.Transfer("Login.aspx");
        }
        else if (!IsPostBack)
        {

            Station.Items.Add("");
            string SQL_query0 = "select STATION from ERS_STATION ";
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_query0))
            {
                while (rdr.Read())
                {
                    Station.Items.Add(Convert.ToString(rdr.GetSqlValue(0)));
                }

            }
            //权限管理
            string stationsession = Session["Station"].ToString();
            if (stationsession != "ALL")
            {
                Station.SelectedValue = stationsession;
                Station.Enabled = false;
                Station.BackColor = ColorTranslator.FromHtml("#f9f9f9");
            }


            Sales.Items.Add("");
            string SQL_query = "select Trans_Value from ERS_Trans where Trans_Type='Sales'  order by Trans_Order";
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_query))
            {
                while (rdr.Read())
                {
                    Sales.Items.Add(Convert.ToString(rdr.GetSqlValue(0)));
                }

            }

            Deposit.Items.Add("");
            string SQL_query1 = "select Trans_Value from ERS_Trans where Trans_Type='Deposit'  order by Trans_Order";
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_query1))
            {
                while (rdr.Read())
                {
                    Deposit.Items.Add(Convert.ToString(rdr.GetSqlValue(0)));
                }

            }


            getdata();

        }
    }


    public void getdata()
    {
        //权限管理
        string stationsession = Session["Station"].ToString();
        string sql_query = "select A.*,CONVERT(varchar(100), A.Issue_Date, 103) As Issue  from ERS_Receipt A ";
        sql_query += " where 1=1   ";
        if (Station.Text != "" && Station.Text != null)
        {

            sql_query += " and  Station='" + Station.Text + "'";
        }



        if (Num.Text != "" && Num.Text != null)
        {

            sql_query += " and  Num='" + Num.Text + "'";
        }


        if (Issue_Date.Text != "" && Issue_Date.Text != null)
        {

            sql_query += " and  Issue_Date='" + Issue_Date.Text + "'";
        }


        if (Received.Text != "" && Received.Text != null)
        {

            sql_query += " and  Received='" + Received.Text + "'";
        }

        if (Balance.Text != "" && Balance.Text != null)
        {

            sql_query += " and  Balance='" + Balance.Text + "'";
        }

        if (Sales.Text != "" && Sales.Text != null)
        {

            sql_query += " and  Sales='" + Sales.Text + "'";
        }
        if (Deposit.Text != "" && Deposit.Text != null)
        {

            sql_query += " and  Deposit='" + Deposit.Text + "'";
        }

        sql_query += " order by id  DESC";
        //读取数据
        SqlConnection conn = new SqlConnection();
        
        SqlDataAdapter sda = new SqlDataAdapter(sql_query, SqlHelper.Conn);
        sda.Fill(ds, "Receipt");
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = ds.Tables["Receipt"].DefaultView;
        pds.AllowPaging = true;//允许分页
        pds.PageSize = 5;//单页显示项数
        int CurPage;
        if (Request.QueryString["Page"] != null)
            CurPage = Convert.ToInt32(Request.QueryString["Page"]);
        else
            CurPage = 1;
        pds.CurrentPageIndex = CurPage - 1;
        int Count = pds.PageCount;
        lblCurrentPage.Text = "Current Page：" + CurPage.ToString();
        labPage.Text = Count.ToString();
        if (Count > 1)
        {
            this.first.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1";
            this.last.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(Count);
        }
        if (!pds.IsFirstPage)
        {

            up.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(CurPage - 1);
        }

        if (!pds.IsLastPage)
        {

            next.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(CurPage + 1);
        }


        //Repeater
        Receipt_table.DataSource = pds;
        Receipt_table.DataBind();

    }

    protected void Search_Click(object sender, EventArgs e)
    {
        getdata();
    }
    protected void downloadexcelbtn_Click(object sender, EventArgs e)
    {

        getdata();
        ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('downloadexcelbtn_Click');</script>");
        //设置EXCEL列宽
        int[] ColumnWidth = { 10, 20, 20, 20, 20, 20, 20, 30, 20, 20, 20, 50 };
        //获取用户选择的excel文件名称
        string ReportTitleName = "ERS_Report";
        string savepath = Server.MapPath("Files/" + ReportTitleName.ToString() + ".xls");
        //新建excel
        Workbook wb = new Workbook();

        //设置字体样式

        Aspose.Cells.Style style1 = wb.Styles[wb.Styles.Add()];
        style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style1.Font.Name = "宋体";
        style1.Font.IsBold = true;//设置粗体
        style1.Font.Size = 12;//设置字体大小

        Aspose.Cells.Style style2 = wb.Styles[wb.Styles.Add()];
        style2.HorizontalAlignment = TextAlignmentType.Center;
        style2.Font.Size = 10;

        //sheet1
        Worksheet ws = wb.Worksheets[0];
        Cells cell = ws.Cells;
        ws.Name = "Telephone";
        //合并第一行单元格
        Range range = cell.CreateRange(0, 0, 1, ColumnWidth.Length);
        range.Merge();
        cell["A1"].PutValue("ERS Report"); //标题
        //给单元格关联样式
        cell["A1"].SetStyle(style1); //报表名字 样式
        //设置Execl列名  可以采用单独传值
        cell[1, 0].PutValue("Station");
        cell[1, 0].SetStyle(style2);
        cell[1, 1].PutValue("Number");
        cell[1, 1].SetStyle(style2);
        cell[1, 2].PutValue("Issue Date");
        cell[1, 2].SetStyle(style2);
        cell[1, 3].PutValue("Received");
        cell[1, 3].SetStyle(style2);
        cell[1, 4].PutValue("Balance");
        cell[1, 4].SetStyle(style2);
        cell[1, 5].PutValue("Sales");
        cell[1, 5].SetStyle(style2);
        cell[1, 6].PutValue("Deposit");
        cell[1, 6].SetStyle(style2);
        cell[1, 7].PutValue("RTNG");
        cell[1, 7].SetStyle(style2);
        cell[1, 8].PutValue("PNR");
        cell[1, 8].SetStyle(style2);
        cell[1, 9].PutValue("TTL");
        cell[1, 9].SetStyle(style2);
        cell[1, 10].PutValue("Total Amount");
        cell[1, 10].SetStyle(style2);
        cell[1, 11].PutValue("Remarks");
        cell[1, 11].SetStyle(style2);
        //设置单元格内容
        int posStart = 2;
        int row = 0;

        for (int i = 0; i < ds.Tables["Receipt"].Rows.Count; i++)
        {
            DataRow Drow = ds.Tables["Receipt"].Rows[i];
            cell[row + posStart, 0].PutValue(Drow[1].ToString());
            cell[row + posStart, 0].SetStyle(style2);
            cell[row + posStart, 1].PutValue( string.Format("{0:d6}",Convert.ToInt32( Drow[2])));
            cell[row + posStart, 1].SetStyle(style2);
            cell[row + posStart, 2].PutValue(Convert.ToDateTime(Drow[6].ToString()).ToShortDateString() );
            cell[row + posStart, 2].SetStyle(style2);
            cell[row + posStart, 3].PutValue(Drow[7].ToString());
            cell[row + posStart, 3].SetStyle(style2);
            cell[row + posStart, 4].PutValue(Drow[19].ToString());
            cell[row + posStart, 4].SetStyle(style2);
            cell[row + posStart, 5].PutValue(Drow[4].ToString());
            cell[row + posStart, 5].SetStyle(style2);
            cell[row + posStart, 6].PutValue(Drow[3].ToString());
            cell[row + posStart, 6].SetStyle(style2);
            cell[row + posStart, 7].PutValue(Drow[8].ToString());
            cell[row + posStart, 7].SetStyle(style2);
            cell[row + posStart, 8].PutValue(Drow[9].ToString());
            cell[row + posStart, 8].SetStyle(style2);
            cell[row + posStart, 9].PutValue(Drow[10].ToString());
            cell[row + posStart, 9].SetStyle(style2);
            cell[row + posStart, 10].PutValue(Drow[18].ToString());
            cell[row + posStart, 10].SetStyle(style2);
            cell[row + posStart, 11].PutValue(Drow[11].ToString());
            cell[row + posStart, 11].SetStyle(style2);
            row++;
        }


        for (int i = 0; i < ColumnWidth.Length; i++)
        {
            cell.SetColumnWidth(i, Convert.ToDouble(ColumnWidth[i].ToString()));
        }


        Workbook wb1 = new Workbook();

        //设置字体样式

        Aspose.Cells.Style style11 = wb1.Styles[wb1.Styles.Add()];
        style11.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style11.Font.Name = "宋体";
        style11.Font.IsBold = true;//设置粗体
        style11.Font.Size = 12;//设置字体大小

        Aspose.Cells.Style style21 = wb1.Styles[wb1.Styles.Add()];
        style21.HorizontalAlignment = TextAlignmentType.Center;
        style21.Font.Size = 10;



        //保存在服务器
        wb.Combine(wb1);
        wb.Save(savepath);

        FileTo(ReportTitleName);


    }




    //下载
    public void FileTo(string ReportTitleName)   //file文件名称
    {


        string path = Server.MapPath("Files/" + ReportTitleName.ToString() + ".xls");

        System.IO.FileInfo file = new System.IO.FileInfo(path);

        if (file.Exists)
        {

            Response.Clear();

            Response.ContentType = "application/vnd.ms-excel";

            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(ReportTitleName.ToString()) + ".xls");

            Response.AddHeader("Content-Length", file.Length.ToString());

            Response.ContentType = "application/octet-stream";

            Response.Filter.Close();

            Response.WriteFile(file.FullName);

            Response.End();


        }
    }
}