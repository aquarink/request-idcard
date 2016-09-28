using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace JnyCardWebApplication
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Logout = Request.QueryString["logout"];
            if (Logout == "true")
            {
                Session["idUser"] = null;
                Session["kategori"] = null;
                //
                Session.Remove("idUser");
                Session.Remove("kategori");
            }

            string idUser = (string)(Session["idUser"]);
            string kategori = (string)(Session["kategori"]);

            if (kategori == "1")
            {
                Session["idUser"] = idUser;
                Session["kategori"] = kategori;
                Response.Redirect("HrdCheck.aspx");
            }
            else if (kategori == "2")
            {
                Session["idUser"] = idUser;
                Session["kategori"] = kategori;
                Response.Redirect("Admin.aspx");
            }
            else if (kategori == "3")
            {
                Session["idUser"] = idUser;
                Session["kategori"] = kategori;
                Response.Redirect("Mis.aspx");
            }
            else
            {
                // Stay
            }
        }

        protected void LoginBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(TxtEmail.Text) || String.IsNullOrEmpty(TxtPassword.Text))
            {
                msgLabel.Text = "Kolom Harus Diisi";
            }
            else
            {
                SqlConnection koneksi = new SqlConnection(ConfigurationManager.ConnectionStrings["JnyCardEntities"].ToString());
                try
                {
                    koneksi.Open();

                    SqlCommand sqlCommand = new SqlCommand("SELECT * FROM UserTable WHERE email = @email and password = @passWord AND status = 1", koneksi);
                    sqlCommand.Parameters.Add(new SqlParameter("@email", TxtEmail.Text));
                    sqlCommand.Parameters.Add(new SqlParameter("@passWord", TxtPassword.Text));
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        string idUser = sqlDataReader["idUser"].ToString();
                        string kategori = sqlDataReader["kategori"].ToString();

                        if (kategori == "1")
                        {
                            Session["idUser"] = idUser;
                            Session["kategori"] = kategori;
                            Response.Redirect("HrdCheck.aspx");
                        }
                        else if(kategori == "2")
                        {
                            Session["idUser"] = idUser;
                            Session["kategori"] = kategori;
                            Response.Redirect("Adm.aspx");
                        }
                        else if (kategori == "3")
                        {
                            Session["idUser"] = idUser;
                            Session["kategori"] = kategori;
                            Response.Redirect("Mis.aspx");
                        }
                        else
                        {
                            //Response.Redirect("Default.aspx");
                            msgLabel.Text = "Failerd Null Category";
                        }
                        
                        koneksi.Close();
                    }
                    else
                    {
                        msgLabel.Text = "Invalid email and password";
                    }
                }
                catch (Exception ex)
                {
                    msgLabel.Text = ex.Message;
                }
            }
        }
    }
}