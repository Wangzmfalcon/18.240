using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;//excel
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using SMS.DBUtility;
using System.Globalization;//日期格式化



public partial class DownloadExcel : System.Web.UI.Page
{
    string sqlstr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    public void Page_Load(object sender, EventArgs e)
    {


        //获取传递参数
        string StaffID = Request.QueryString["StaffID"];
        //string SQLP = StaffID.Replace("|", ",");
        string SQLP = StaffID.Replace("checkbox", "");
        string[] ar = SQLP.Split('|');
        for (int i = 0; i < ar.Length; i++)
        {
            ar[i] = "'" + ar[i] + "'";
        }
        string SQLS = "";
        for (int i = 0; i < ar.Length; i++)
        {

            if (i == ar.Length - 1)
                SQLS += ar[i];
            else
                SQLS += ar[i] + ",";

        }



        DataTable list = new DataTable();
        DataTable list1 = new DataTable();
        DataTable list2 = new DataTable();
         //DataTable dt3 = new DataTable();

        //获取电话数据
        using (SqlConnection sqlcnn = new SqlConnection(sqlstr))
        {
            using (SqlCommand sqlcmm = sqlcnn.CreateCommand())
            {
                string SQL = "  select A.EMPLID,A.PHONE_TYPE,A.PHONE,A.EXTENSION,A.PREF_PHONE_FLAG   from HR_PIF_Phone A WHERE 1=1 AND EMPLID IN(" + SQLS + ") ";


                sqlcmm.CommandText = SQL;
                sqlcnn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmm);

                adapter.Fill(list);


            }
        }


        //获取地址

        using (SqlConnection sqlcnn = new SqlConnection(sqlstr))
        {
            using (SqlCommand sqlcmm1 = sqlcnn.CreateCommand())
            {
                string SQL1 = "  select *  from HR_PIF_Address A WHERE 1=1 AND EMPLID IN(" + SQLS + ") ";


                sqlcmm1.CommandText = SQL1;
                sqlcnn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmm1);

                adapter.Fill(list1);


            }
        }




        //获取联系人
        using (SqlConnection sqlcnn = new SqlConnection(sqlstr))
        {
            using (SqlCommand sqlcmm2 = sqlcnn.CreateCommand())
            {
                string SQL2 = "  select *  from HR_PIF_Dependent A WHERE 1=1 AND EMPLID IN(" + SQLS + ") ";


                sqlcmm2.CommandText = SQL2;
                sqlcnn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmm2);

                adapter.Fill(list2);


            }
        }


        string sqlstr1 = "Server=192.168.101.114;uid=falcon;pwd=airmacau;database=CSD";//ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        using (SqlConnection sqlcnn = new SqlConnection(sqlstr1))
        {
            using (SqlCommand sqlcmmd = sqlcnn.CreateCommand())
            {
                string SQL = "update HR_PIF_StaffList SET DOWNLOAD_FLAG=1 WHERE EMPLID IN(" + SQLS + ") ";


                sqlcmmd.CommandText = SQL;
                sqlcnn.Open();
                int d = sqlcmmd.ExecuteNonQuery();
               

            }
        }
        //设置EXCEL列宽
        int[] ColumnWidth = { 10, 10, 20, 20, 10 };
        int[] ColumnWidth1 = { 10, 10, 20, 10, 50, 50, 50, 50 };
        int[] ColumnWidth2 = { 10, 10, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 };
        //int[] list3 = { 10, 10, 20, 20, 10 };
        //ReportToExcel(dt, list, "人员登记表", dt1, list1, "人员登记表", dt2, list2, "人员登记表");
        //获取用户选择的excel文件名称
        string ReportTitleName = "New_Staff_information";
        string savepath = Server.MapPath("Template/" + ReportTitleName.ToString() + ".xls");
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
        cell["A1"].PutValue("Telephone"); //标题


        //给单元格关联样式
        cell["A1"].SetStyle(style1); //报表名字 样式


        //设置Execl列名  可以采用单独传值
        cell[1, 0].PutValue("EMPLID");
        cell[1, 0].SetStyle(style2);
        cell[1, 1].PutValue("PHONE_TYPE");
        cell[1, 1].SetStyle(style2);
        cell[1, 2].PutValue("PHONE");
        cell[1, 2].SetStyle(style2);
        cell[1, 3].PutValue("EXTENSION");
        cell[1, 3].SetStyle(style2);
        cell[1, 4].PutValue("PREF_PHONE_FLAG");
        cell[1, 4].SetStyle(style2);


        //设置单元格内容
        int posStart = 2;
        int row = 0;

        for (int i = 0; i < list.Rows.Count; i++)
        {
            DataRow Drow = list.Rows[i];
            cell[row + posStart, 0].PutValue(Drow[0].ToString());
            cell[row + posStart, 0].SetStyle(style2);
            cell[row + posStart, 1].PutValue(Drow[1].ToString());
            cell[row + posStart, 1].SetStyle(style2);
            cell[row + posStart, 2].PutValue(Drow[2].ToString());
            cell[row + posStart, 2].SetStyle(style2);
            cell[row + posStart, 3].PutValue(Drow[3].ToString());
            cell[row + posStart, 3].SetStyle(style2);
            cell[row + posStart, 4].PutValue(Drow[4].ToString());
            cell[row + posStart, 4].SetStyle(style2);

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

        //sheet2




        Worksheet ws1 = wb1.Worksheets[0];
        Cells cell1 = ws1.Cells;
        ws1.Name = "Address";
        //合并第一行单元格
        Range range1 = cell1.CreateRange(0, 0, 1, ColumnWidth1.Length);
        range1.Merge();
        cell1["A1"].PutValue("Address"); //标题


        //给单元格关联样式
        cell1["A1"].SetStyle(style11); //报表名字 样式


        //设置Execl列名  可以采用单独传值
        cell1[1, 0].PutValue("EMPLID");
        cell1[1, 0].SetStyle(style21);
        cell1[1, 1].PutValue("KEYPROP_ADDRESS_TYPE");
        cell1[1, 1].SetStyle(style21);
        cell1[1, 2].PutValue("KEYPROP_EFFDT");
        cell1[1, 2].SetStyle(style21);
        cell1[1, 3].PutValue("COUNTRY");
        cell1[1, 3].SetStyle(style21);
        cell1[1, 4].PutValue("ADDRESS1");
        cell1[1, 4].SetStyle(style21);
        cell1[1, 5].PutValue("ADDRESS2");
        cell1[1, 5].SetStyle(style21);
        cell1[1, 6].PutValue("ADDRESS3");
        cell1[1, 6].SetStyle(style21);
        cell1[1, 7].PutValue("ADDRESS4");
        cell1[1, 7].SetStyle(style21);

        //设置单元格内容
         posStart = 2;
         row = 0;

        for (int i = 0; i < list1.Rows.Count; i++)
        {
            DataRow Drow = list1.Rows[i];
            cell1[row + posStart, 0].PutValue(Drow[0].ToString());
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
  

            row++;
        }


        for (int i = 0; i < ColumnWidth1.Length; i++)
        {
            cell1.SetColumnWidth(i, Convert.ToDouble(ColumnWidth1[i].ToString()));
        }




        //sheet3
        Workbook wb2= new Workbook();

        //设置字体样式

        Aspose.Cells.Style style12 = wb2.Styles[wb2.Styles.Add()];
        style11.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style11.Font.Name = "宋体";
        style11.Font.IsBold = true;//设置粗体
        style11.Font.Size = 12;//设置字体大小

        Aspose.Cells.Style style22 = wb2.Styles[wb2.Styles.Add()];
        style22.HorizontalAlignment = TextAlignmentType.Center;
        style22.Font.Size = 10;



        Worksheet ws2 = wb2.Worksheets[0];
        Cells cell2 = ws2.Cells;
        ws2.Name = "Dependent";
        //合并第一行单元格
        Range range2 = cell2.CreateRange(0, 0, 1, ColumnWidth2.Length);
        range2.Merge();
        cell2["A1"].PutValue("Dependent"); //标题


        //给单元格关联样式
        cell2["A1"].SetStyle(style12); //报表名字 样式


        //设置Execl列名  可以采用单独传值
        cell2[1, 0].PutValue("EMPLID");
        cell2[1, 0].SetStyle(style22);
        cell2[1, 1].PutValue("DEPENDENT_BENEF");
        cell2[1, 1].SetStyle(style22);
        cell2[1, 2].PutValue("BIRTHDATE");
        cell2[1, 2].SetStyle(style22);
        cell2[1, 3].PutValue("BIRTHPLACE");
        cell2[1, 3].SetStyle(style22);
        cell2[1, 4].PutValue("BIRTHCOUNTRY");
        cell2[1, 4].SetStyle(style22);
        cell2[1, 5].PutValue("BIRTHSTATE");
        cell2[1, 5].SetStyle(style22);
        cell2[1, 6].PutValue("PHONE");
        cell2[1, 6].SetStyle(style22);
        cell2[1, 7].PutValue("EFFDT");
        cell2[1, 7].SetStyle(style22);
        cell2[1, 8].PutValue("COUNTRY_NM_FORMAT");
        cell2[1, 8].SetStyle(style22);
        cell2[1, 9].PutValue("LAST_NAME");
        cell2[1, 9].SetStyle(style22);
        cell2[1, 10].PutValue("FIRST_NAME");
        cell2[1, 10].SetStyle(style22);
        cell2[1, 11].PutValue("NAME_PREFIX");
        cell2[1, 11].SetStyle(style22);
        cell2[1, 12].PutValue("EFFDT_1");
        cell2[1, 12].SetStyle(style22);
        cell2[1, 13].PutValue("SAME_ADDRESS_EMPL");
        cell2[1, 13].SetStyle(style22);
        cell2[1, 14].PutValue("COUNTRY");
        cell2[1, 14].SetStyle(style22);
        cell2[1, 15].PutValue("ADDRESS1");
        cell2[1, 15].SetStyle(style22);
        cell2[1, 16].PutValue("ADDRESS2");
        cell2[1, 16].SetStyle(style22);
        cell2[1, 17].PutValue("ADDRESS3");
        cell2[1, 17].SetStyle(style22);
        cell2[1, 18].PutValue("ADDRESS4");
        cell2[1, 18].SetStyle(style22);
        cell2[1, 19].PutValue("CITY");
        cell2[1, 19].SetStyle(style22);
        cell2[1, 20].PutValue("EFFDT_3");
        cell2[1, 20].SetStyle(style22);
        cell2[1, 21].PutValue("RELATIONSHIP");
        cell2[1, 21].SetStyle(style22);
        cell2[1, 22].PutValue("MAR_STATUS");
        cell2[1, 22].SetStyle(style22);
        cell2[1, 23].PutValue("MAR_STATUS_DT");
        cell2[1, 23].SetStyle(style22);
        cell2[1, 24].PutValue("SEX");
        cell2[1, 24].SetStyle(style22);
        cell2[1, 25].PutValue("GC_DEP_COMMENT");
        cell2[1, 25].SetStyle(style22);
        cell2[1, 26].PutValue("GC_DEP_EMG_CONTACT");
        cell2[1, 26].SetStyle(style22);
        cell2[1, 27].PutValue("GC_DEP_PRMCONTACT");
        cell2[1, 27].SetStyle(style22);
        cell2[1, 28].PutValue("NATIONAL_ID_TYPE");
        cell2[1, 28].SetStyle(style22);
        cell2[1, 29].PutValue("NATIONAL_ID");
        cell2[1, 29].SetStyle(style22);
        cell2[1, 30].PutValue("PRIMARY_NID");
        cell2[1, 30].SetStyle(style22);
        cell2[1, 31].PutValue("EXPIRATION_DATE");
        cell2[1, 31].SetStyle(style22);
 ;
        //设置单元格内容
        posStart = 2;
        row = 0;

        for (int i = 0; i < list2.Rows.Count; i++)
        {
            DataRow Drow = list2.Rows[i];
            for (int j = 0; j < 32; j++)
            {
                cell2[row + posStart, j].PutValue(Drow[j].ToString());
                cell2[row + posStart, j].SetStyle(style22);
            } 




            row++;
        }


        for (int i = 0; i < ColumnWidth2.Length; i++)
        {
            cell2.SetColumnWidth(i, Convert.ToDouble(ColumnWidth2[i].ToString()));
        }

        //保存在服务器
        wb.Combine(wb1);
        wb.Combine(wb2);
        wb.Save(savepath);

        FileTo(ReportTitleName);



    }





    public void ReportToExcel(DataTable list, int[] ColumnWidth, string ReportTitleName, DataTable list1, int[] ColumnWidth1, string ReportTitleName1, DataTable list2, int[] ColumnWidth2, string ReportTitleName2)
    {
        //获取用户选择的excel文件名称

        string savepath = Server.MapPath("Template/" + ReportTitleName.ToString() + ".xls");
        //新建excel
        Workbook wb = new Workbook();


        //sheet1
        Worksheet ws = wb.Worksheets[0];
        Cells cell = ws.Cells;

        //合并第一行单元格
        Range range = cell.CreateRange(0, 0, 1, ColumnWidth.Length);
        range.Merge();
        cell["A1"].PutValue(ReportTitleName); //标题


        //设置字体样式

        Aspose.Cells.Style style1 = wb.Styles[wb.Styles.Add()];
        style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style1.Font.Name = "宋体";
        style1.Font.IsBold = true;//设置粗体
        style1.Font.Size = 12;//设置字体大小

        Aspose.Cells.Style style2 = wb.Styles[wb.Styles.Add()];
        style2.HorizontalAlignment = TextAlignmentType.Center;
        style2.Font.Size = 10;

        //给单元格关联样式
        cell["A1"].SetStyle(style1); //报表名字 样式



        //设置Execl列名  可以采用单独传值

        cell[1, 0].PutValue("EMPLID");
        cell[1, 0].SetStyle(style2);
        cell[1, 1].PutValue("PHONE_TYPE");
        cell[1, 1].SetStyle(style2);
        cell[1, 2].PutValue("PHONE");
        cell[1, 2].SetStyle(style2);
        cell[1, 3].PutValue("EXTENSION");
        cell[1, 3].SetStyle(style2);
        cell[1, 4].PutValue("PREF_PHONE_FLAG");
        cell[1, 4].SetStyle(style2);

        //for (int i = 0; i < list.Columns.Count; i++)
        //{
        //    cell[1, i].PutValue(list.Columns[i].Text);
        //    cell[1, i].SetStyle(style2);
        //}


        //设置单元格内容
        int posStart = 2;
        int row = 0;

        for (int i = 0; i < list.Rows.Count; i++)
        {
            DataRow Drow = list.Rows[i];
            cell[row + posStart, 0].PutValue(Drow[0].ToString());
            cell[row + posStart, 0].SetStyle(style2);
            cell[row + posStart, 1].PutValue(Drow[1].ToString());
            cell[row + posStart, 1].SetStyle(style2);
            cell[row + posStart, 2].PutValue(Drow[2].ToString());
            cell[row + posStart, 2].SetStyle(style2);
            cell[row + posStart, 3].PutValue(Drow[3].ToString());
            cell[row + posStart, 3].SetStyle(style2);
            cell[row + posStart, 4].PutValue(Drow[4].ToString());
            cell[row + posStart, 4].SetStyle(style2);

            row++;
        }
        //foreach (ESM item in list)
        //{

        //    cell[row + posStart, 0].PutValue(item.StaffNo);
        //    cell[row + posStart, 0].SetStyle(style2);
        //    cell[row + posStart, 1].PutValue(item.Name);
        //    cell[row + posStart, 1].SetStyle(style2);

        //    row++;
        //}


        for (int i = 0; i < ColumnWidth.Length; i++)
        {
            cell.SetColumnWidth(i, Convert.ToDouble(ColumnWidth[i].ToString()));
        }


        //保存在服务器
        wb.Save(savepath);

        FileTo(ReportTitleName);
    }



    //下载
    public void FileTo(string ReportTitleName)   //file文件名称
    {


        string path = Server.MapPath("Template/" + ReportTitleName.ToString() + ".xls");

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