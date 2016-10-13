using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMS.DBUtility;
using System.Data;
using System.Data.SqlClient;

public partial class User_Setting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Manage_Level"] == null || Session["UserID"] == null || Session["Receipt"] == null)
        {
            Server.Transfer("Login.aspx");
        }
        else
        {
            //读取数据

            DataSet ds = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter("select * from ERS_User", SqlHelper.Conn);
            sda.Fill(ds, "User");
            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = ds.Tables["User"].DefaultView;
            pds.AllowPaging = true;//允许分页
            pds.PageSize = 10;//单页显示项数
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
            User.DataSource = pds;
            User.DataBind();

            if (!IsPostBack)
            {
                //绑定下拉框
                Receipt.Items.Clear();
                Receipt.Items.Add(new ListItem("1:Yes", "1"));
                Receipt.Items.Add(new ListItem("0:No", "0"));
                //level
                Level.Items.Clear();
                Level.Items.Add(new ListItem("10:General User", "10"));
                Level.Items.Add(new ListItem("50:Reporter", "50"));
                Level.Items.Add(new ListItem("70:Super User", "70"));
                Level.Items.Add(new ListItem("100:System Administrator", "100"));
                //station
                Station.Items.Clear();
                Station.Items.Add("ALL");
                string SQL_query = " select STATION from ERS_STATION ";
                using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_query))
                {
                    while (rdr.Read())
                    {
                        Station.Items.Add(Convert.ToString(rdr.GetSqlValue(0)));
                    }

                }


            }

        }
    }
    protected void save_Click(object sender, EventArgs e)
    {

        string id = ID.Text;


        //检查是否存在
        string SQL_query = " select * from ERS_User where User_Id='" + id + "'";

        using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_query))
        {
            if (rdr.Read())
            {
                ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('Login Name already exists.');</script>");
            }
            else
            {
                //没有就插入
                string SQL_insert = " INSERT INTO ERS_User  (User_Id,User_Name,Password,Manage_Level,Station,Receipt) values (@User_Id , @User_Name ,@Password,@Manage_Level,@Station,@Receipt)";
                SqlParameter[] parms = new SqlParameter[]{
                     new SqlParameter("@User_Id", SqlDbType.VarChar, 50),
                     new SqlParameter("@User_Name", SqlDbType.VarChar, 50),
                     new SqlParameter("@Password", SqlDbType.VarChar, 50),
                     new SqlParameter("@Manage_Level", SqlDbType.Int),
                     new SqlParameter("@Station", SqlDbType.VarChar, 50),
                     new SqlParameter("@Receipt", SqlDbType.Int),
               };
                parms[0].Value = id;
                parms[1].Value = Staff_Name.Text;
                parms[2].Value = password.Text;
                parms[3].Value = Level.SelectedItem.Value;
                parms[4].Value = Station.SelectedItem.Value;
                parms[5].Value = Receipt.SelectedItem.Value;

                using (SqlConnection conn = new SqlConnection(SqlHelper.Conn))
                {
                    int actionrows = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, SQL_insert, parms);
                    if (actionrows > 0)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('Insert data success');</script>");
                        Response.AddHeader("Refresh", "0");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('Insert data faild');</script>");
                    }
                }
            }

        }


    }
    protected void delete_Click(object sender, EventArgs e)
    {
        string id = ID.Text;


        string SQL_delete = " delete ERS_User where User_Id='" + id + "' ";
        using (SqlConnection conn = new SqlConnection(SqlHelper.Conn))
        {
            int actionrows = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, SQL_delete);
            if (actionrows > 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('Delete data success');</script>");
                Response.AddHeader("Refresh", "0");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('Delete data faild');</script>");
            }
        }

    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        //更新
        string SQL_insert = " UPDATE ERS_User  set User_Name=@User_Name,Password=@Password,Manage_Level=@Manage_Level,Station=@Station,Receipt=@Receipt where User_Id=@User_Id";
        SqlParameter[] parms = new SqlParameter[]{
                     new SqlParameter("@User_Id", SqlDbType.VarChar, 50),
                     new SqlParameter("@User_Name", SqlDbType.VarChar, 50),
                     new SqlParameter("@Password", SqlDbType.VarChar, 50),
                     new SqlParameter("@Manage_Level", SqlDbType.Int),
                     new SqlParameter("@Station", SqlDbType.VarChar, 50),
                     new SqlParameter("@Receipt", SqlDbType.Int),
               };
        parms[0].Value = ID.Text;
        parms[1].Value = Staff_Name.Text;
        parms[2].Value = password.Text;
        parms[3].Value = Level.SelectedItem.Value;
        parms[4].Value = Station.SelectedItem.Value;
        parms[5].Value = Receipt.SelectedItem.Value;

        using (SqlConnection conn = new SqlConnection(SqlHelper.Conn))
        {
            int actionrows = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, SQL_insert, parms);
            if (actionrows > 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('Update data success');</script>");
                Response.AddHeader("Refresh", "0");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('Update data faild');</script>");
            }
        }
    }
}