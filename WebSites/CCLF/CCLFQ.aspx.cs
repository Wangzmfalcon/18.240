using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMS.DBUtility;
using System.Globalization;//日期格式化
using Aspose.Cells;//excel
using System.Linq;
//using System.Xml.Linq;
public partial class Home : System.Web.UI.Page
{

    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CCLF_Manage_Code"] == null)
        {
            Server.Transfer("Login.aspx");

        }
        else
        {
            Search.Attributes.Add("onclick", "return checkForm()");
            Download.Attributes.Add("onclick", "return checkForm()");
        }
    }


    public void getdata()
    {
        string lf = LF.SelectedValue;
        string from = From.Text;
        string to = To.Text;
        //查询详细信息
        string SQL = "if object_id('Tempdb..#Cabin_Staff_No') is not null                                                             "
            + " drop table #Cabin_Staff_No                                                                                     "
            + "   select top 100 ID=Identity(int,1,1) into #Cabin_Staff_No from                                                "
            + " syscolumns a,syscolumns b                                                                                      "
            + "                                                                                                                "
            + " select aa.*, bb.NAME                                                                                           "
            + " from(                                                                                                          "
            + " Select a.FLT_DATE,a.IATA_C,a.FLT_ID,                                                                           "
            + " a.AC_ID,a.AC_TYPE,a.DEP_APT,a.ARR_APT,a.CANCEL_FLAG,                                                           "
            + "  a.SEAT,a.PAX,A.LF,                                                                                            "
            + "  CABIN_STAFFNO_STR=substring(a.CABIN_STAFFNO_STR,b.ID,charindex('_',a.CABIN_STAFFNO_STR+'_',b.ID)-b.ID)        "
            + "  ,a.ICREW_NUMBER                                                                                               "
            + "from V_OMIS_GAD_Data a,#Cabin_Staff_No b                                                                        "
            + "where 1=1                                                                                                       "
            + "and charindex('_','_'+a.CABIN_STAFFNO_STR,b.ID)=b.ID                                                            "
            + "and FLT_DATE between '" + from + "' and '" + to + "'                                                            "
            + "and CABIN_STAFFNO_STR<>''                                                                                       "
            + "and CANCEL_FLAG=0                                                                                               "
            + "and LF>= '" + lf + "') as aa,                                                                                   "
            + "(SELECT  EMPLID,CASE WHEN PREFER='' THEN NAME ELSE NAME+'('+PREFER+')' END NAME                                 "
            + "  FROM [192.168.101.149].[HCMPRD].[dbo].[AM_STAFF]                                                              "
            + "  UNION                                                                                                         "
            + "SELECT  EMPLID, CASE WHEN PREFER='' THEN NAME ELSE NAME+'('+PREFER+')' END NAME                                 "
            + "  FROM [192.168.101.149].[HCMPRD].[dbo].[AM_STAFF_LEAVER] )  as bb                                              "
            + "  where aa.CABIN_STAFFNO_STR=bb.EMPLID  COLLATE Chinese_PRC_CI_AS                                               "
            + "  order by aa.FLT_DATE,aa.FLT_ID                                                                                ";


        DataTable dt = new DataTable() { };
        dt.TableName = "Detail";
        dt.Columns.Add("FLT_DATE", typeof(string));
        dt.Columns.Add("IATA_C", typeof(string));
        dt.Columns.Add("FLT_ID", typeof(string));
        dt.Columns.Add("AC_ID", typeof(string));
        dt.Columns.Add("AC_TYPE", typeof(string));
        dt.Columns.Add("DEP_APT", typeof(string));
        dt.Columns.Add("ARR_APT", typeof(string));
        dt.Columns.Add("CANCEL_FLAG", typeof(string));
        dt.Columns.Add("SEAT", typeof(string));
        dt.Columns.Add("PAX", typeof(string));
        dt.Columns.Add("LF", typeof(string));
        dt.Columns.Add("CABIN_STAFFNO", typeof(string));
        dt.Columns.Add("CREW_NUMBER", typeof(string));
        dt.Columns.Add("CABIN_NAME", typeof(string));
        using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL))
        {
            while (rdr.Read())
            {




                DataRow newrow = dt.NewRow();
                newrow["FLT_DATE"] = Convert.ToString(rdr.GetSqlValue(0));
                newrow["IATA_C"] = Convert.ToString(rdr.GetSqlValue(1));
                newrow["FLT_ID"] = Convert.ToString(rdr.GetSqlValue(2));
                newrow["AC_ID"] = Convert.ToString(rdr.GetSqlValue(3));
                newrow["AC_TYPE"] = Convert.ToString(rdr.GetSqlValue(4));
                newrow["DEP_APT"] = Convert.ToString(rdr.GetSqlValue(5));
                newrow["ARR_APT"] = Convert.ToString(rdr.GetSqlValue(6));
                newrow["CANCEL_FLAG"] = Convert.ToString(rdr.GetSqlValue(7));
                newrow["SEAT"] = Convert.ToString(rdr.GetSqlValue(8));
                newrow["PAX"] = Convert.ToString(rdr.GetSqlValue(9));
                newrow["LF"] = Convert.ToString(rdr.GetSqlValue(10));
                newrow["CABIN_STAFFNO"] = Convert.ToString(rdr.GetSqlValue(11));
                newrow["CREW_NUMBER"] = Convert.ToString(rdr.GetSqlValue(12));
                newrow["CABIN_NAME"] = Convert.ToString(rdr.GetSqlValue(13));
                dt.Rows.Add(newrow);



            }

        }

        ds.Tables.Add(dt);
        Report_detail.DataSource = ds.Tables["Detail"];
        Report_detail.DataBind();

        //统计人数

        var data = from t in dt.AsEnumerable()
                   group t by new { t1 = t.Field<string>("CABIN_STAFFNO"), t2 = t.Field<string>("CABIN_NAME") } into m
                   orderby m.Key.t1
                   select new
                   {
                       StaffID = m.Key.t1,
                       Name = m.Key.t2,
                       Count = m.Count()
                   };


        DataTable dt1 = new DataTable() { };
        dt1.TableName = "Statistical";
        dt1.Columns.Add("StaffID", typeof(string));
        dt1.Columns.Add("Name", typeof(string));
        dt1.Columns.Add("Count", typeof(string));


        if (data.ToList().Count > 0)
        {
            data.ToList().ForEach(q =>
            {

                DataRow newrow = dt1.NewRow();
                newrow["StaffID"] = q.StaffID;
                newrow["Name"] = q.Name;
                newrow["Count"] = q.Count;
                dt1.Rows.Add(newrow);

            });
        }


        ds.Tables.Add(dt1);
        Report.DataSource = ds.Tables["Statistical"];
        Report.DataBind();


    }

    protected void Search_Click(object sender, EventArgs e)
    {

        getdata();

    }
    protected void Download_Click(object sender, EventArgs e)
    {
        getdata();
        //生成EXCEL
        //获取用户选择的excel文件名称
        string ReportTitleName = "Cabin_Crew_Load_Factor";
        string savepath = Server.MapPath("Files/" + ReportTitleName.ToString() + ".xls");


        //sheet1 Statistical
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


        //设置EXCEL列宽
        int[] ColumnWidth1 = { 20, 20, 20 };

        Worksheet ws = wb.Worksheets[0];
        Cells cell = ws.Cells;
        ws.Name = "Statistical";
        //合并第一行单元格
        Range range1 = cell.CreateRange(0, 0, 1, ColumnWidth1.Length);
        range1.Merge();
        cell["A1"].PutValue("CCLF Report"); //标题
        //给单元格关联样式
        cell["A1"].SetStyle(style1); //报表名字 样式
        //设置Execl列名  可以采用单独传值
        cell[1, 0].PutValue("Staff Id");
        cell[1, 0].SetStyle(style2);
        cell[1, 1].PutValue("Name");
        cell[1, 1].SetStyle(style2);
        cell[1, 2].PutValue("Count");
        cell[1, 2].SetStyle(style2);
        //设置单元格内容
        int posStart = 2;
        int row = 0;

        for (int i = 0; i < ds.Tables["Statistical"].Rows.Count; i++)
        {
            DataRow Drow = ds.Tables["Statistical"].Rows[i];
            cell[row + posStart, 0].PutValue(Drow[0].ToString());
            cell[row + posStart, 0].SetStyle(style2);
            cell[row + posStart, 1].PutValue(Drow[1].ToString());
            cell[row + posStart, 1].SetStyle(style2);
            cell[row + posStart, 2].PutValue(Drow[2].ToString());
            cell[row + posStart, 2].SetStyle(style2);
            row++;
        }

        for (int i = 0; i < ColumnWidth1.Length; i++)
        {
            cell.SetColumnWidth(i, Convert.ToDouble(ColumnWidth1[i].ToString()));
        }

        //sheet2 Detail

        Workbook wb1 = new Workbook();
        //设置字体样式
        int[] ColumnWidth2 = { 30, 20, 20, 20, 20, 20, 20, 30, 20, 20, 20, 20, 50, 20 };
        Aspose.Cells.Style style11 = wb1.Styles[wb1.Styles.Add()];
        style11.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style11.Font.Name = "宋体";
        style11.Font.IsBold = true;//设置粗体
        style11.Font.Size = 12;//设置字体大小

        Aspose.Cells.Style style21 = wb.Styles[wb.Styles.Add()];
        style21.HorizontalAlignment = TextAlignmentType.Center;
        style21.Font.Size = 10;


        Worksheet ws1 = wb1.Worksheets[0];
        Cells cell1 = ws1.Cells;
        ws1.Name = "Detail";
        Range range2 = cell1.CreateRange(0, 0, 1, ColumnWidth2.Length);
        range2.Merge();
        cell1["A1"].PutValue("CCLF Report"); //标题
        //给单元格关联样式
        cell1["A1"].SetStyle(style11); //报表名字 样式
        //设置Execl列名  可以采用单独传值
        cell1[1, 0].PutValue("FLT_DATE");
        cell1[1, 0].SetStyle(style21);
        cell1[1, 1].PutValue("IATA_C");
        cell1[1, 1].SetStyle(style21);
        cell1[1, 2].PutValue("FLT_ID");
        cell1[1, 2].SetStyle(style21);
        cell1[1, 3].PutValue("AC_ID");
        cell1[1, 3].SetStyle(style21);
        cell1[1, 4].PutValue("AC_TYPE");
        cell1[1, 4].SetStyle(style21);
        cell1[1, 5].PutValue("DEP_APT");
        cell1[1, 5].SetStyle(style21);
        cell1[1, 6].PutValue("ARR_APT");
        cell1[1, 6].SetStyle(style21);
        cell1[1, 7].PutValue("CANCEL_FLAG");
        cell1[1, 7].SetStyle(style21);
        cell1[1, 8].PutValue("SEAT");
        cell1[1, 8].SetStyle(style21);
        cell1[1, 9].PutValue("PAX");
        cell1[1, 9].SetStyle(style21);
        cell1[1, 10].PutValue("LF");
        cell1[1, 10].SetStyle(style21);
        cell1[1, 11].PutValue("CABIN_STAFFNO");
        cell1[1, 11].SetStyle(style21);
        cell1[1, 12].PutValue("CABIN_NAME");
        cell1[1, 12].SetStyle(style21);
        cell1[1, 13].PutValue("CREW_NUMBER");
        cell1[1, 13].SetStyle(style21);

        posStart = 2;
        row = 0;
        for (int i = 0; i < ds.Tables["Detail"].Rows.Count; i++)
        {
            DataRow Drow = ds.Tables["Detail"].Rows[i];
            cell1[row + posStart, 0].PutValue(Convert.ToDateTime(Drow[0].ToString()).ToString("dd-MM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo));
            cell1[row + posStart, 0].SetStyle(style21);
            cell1[row + posStart, 1].PutValue(Drow[1].ToString());
            cell1[row + posStart, 1].SetStyle(style21);
            cell1[row + posStart, 2].PutValue(Drow[2].ToString());
            cell1[row + posStart, 2].SetStyle(style21);
            cell1[row + posStart, 3].PutValue(Drow[3].ToString());
            cell1[row + posStart, 3].SetStyle(style21);
            cell1[row + posStart, 4].PutValue(Drow[4].ToString());
            cell1[row + posStart, 4].SetStyle(style21);
            cell1[row + posStart, 5].PutValue(Drow[5].ToString());
            cell1[row + posStart, 5].SetStyle(style21);
            cell1[row + posStart, 6].PutValue(Drow[6].ToString());
            cell1[row + posStart, 6].SetStyle(style21);
            cell1[row + posStart, 7].PutValue(Drow[7].ToString());
            cell1[row + posStart, 7].SetStyle(style21);
            cell1[row + posStart, 8].PutValue(Drow[8].ToString());
            cell1[row + posStart, 8].SetStyle(style21);
            cell1[row + posStart, 9].PutValue(Drow[9].ToString());
            cell1[row + posStart, 9].SetStyle(style21);
            cell1[row + posStart, 10].PutValue(Drow[10].ToString());
            cell1[row + posStart, 10].SetStyle(style21);
            cell1[row + posStart, 11].PutValue(Drow[11].ToString());
            cell1[row + posStart, 11].SetStyle(style21);
            cell1[row + posStart, 12].PutValue(Drow[12].ToString());
            cell1[row + posStart, 12].SetStyle(style21);
            cell1[row + posStart, 13].PutValue(Drow[13].ToString());
            cell1[row + posStart, 13].SetStyle(style21);
            row++;
        }

        for (int i = 0; i < ColumnWidth2.Length; i++)
        {
            cell1.SetColumnWidth(i, Convert.ToDouble(ColumnWidth2[i].ToString()));
        }



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