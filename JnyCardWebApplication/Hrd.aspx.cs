using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace JnyCardWebApplication
{
    public partial class Hrd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idUser = (string)(Session["idUser"]);
            string kategori = (string)(Session["kategori"]);

            if (kategori == "1")
            {
                // Stay
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
                Response.Redirect("Default.aspx");
            }

            //

        }

        protected void NewEmployee_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(TxtIdEmployee.Text) || String.IsNullOrEmpty(TxtName.Text))
            {
                msgLabel.Text = "Kolom Harus Diisi";
            }
            else
            {
                SqlConnection koneksi = new SqlConnection(ConfigurationManager.ConnectionStrings["JnyCardEntities"].ToString());

                try
                {
                    koneksi.Open();

                    SqlCommand sqlCommandCheck = new SqlCommand("SELECT * FROM EmployeeTable WHERE nik = @nik AND statusEmp = 1", koneksi);
                    sqlCommandCheck.Parameters.Add(new SqlParameter("@nik", TxtIdEmployee.Text));

                    SqlDataReader sqlDataReaderCheck = sqlCommandCheck.ExecuteReader();

                    

                    if (sqlDataReaderCheck.Read())
                    {
                        msgLabel.Text = "Employee ID Alreade Exists";
                        sqlDataReaderCheck.Close();
                    }
                        
                    else
                    {
                        sqlDataReaderCheck.Close();
                        try
                        {
                            SqlCommand sqlCommand = new SqlCommand("INSERT INTO EmployeeTable (nik,namaEmp,statusEmp) VALUES(@nik,@namaEmp,@statusEmp)", koneksi);
                            sqlCommand.Parameters.Add(new SqlParameter("@nik", TxtIdEmployee.Text));
                            sqlCommand.Parameters.Add(new SqlParameter("@namaEmp", TxtName.Text));
                            sqlCommand.Parameters.Add(new SqlParameter("@statusEmp", 1));

                            int insertDataEmp = sqlCommand.ExecuteNonQuery();
                            if (insertDataEmp > 0)
                            {
                                try
                                {
                                    DateTime nowDate = DateTime.Now;
                                    SqlCommand sqlCommand2 = new SqlCommand("INSERT INTO RequestTable (idClient,ktgClient,reqDate,statusReq) VALUES(@idClient,@ktgClient,@reqDate,@statusReq)", koneksi);
                                    sqlCommand2.Parameters.Add(new SqlParameter("@idClient", TxtIdEmployee.Text));
                                    sqlCommand2.Parameters.Add(new SqlParameter("@ktgClient", 2));
                                    sqlCommand2.Parameters.Add(new SqlParameter("@reqDate", nowDate));
                                    sqlCommand2.Parameters.Add(new SqlParameter("@statusReq", 1));

                                    int insertDataReq = sqlCommand2.ExecuteNonQuery();
                                    if (insertDataReq > 0)
                                    {
                                        //Send Mail
                                        try
                                        {
                                            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                                            smtpClient.EnableSsl = true;
                                            smtpClient.Timeout = 10000;
                                            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                                            smtpClient.UseDefaultCredentials = false;
                                            smtpClient.Credentials = new NetworkCredential("juri.pebrianto@jny.sch.id","mangapsjunk");

                                            MailMessage mailMessage = new MailMessage();
                                            mailMessage.To.Add("juripebrianto@gmail.com");
                                            mailMessage.CC.Add("sugiyanto@jny.sch.id");
                                            mailMessage.CC.Add("welly.tarsisius@jny.sch.id");
                                            mailMessage.From = new MailAddress("juri.pebrianto@jny.sch.id", "Juri Pebrianto");
                                            mailMessage.Subject = "Request New Employee Card - " + TxtIdEmployee.Text + " - " + TxtName.Text;
                                            //
                                            mailMessage.Body += "<html xmlns='http://www.w3.org/1999/xhtml'><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8'> <meta name='viewport' content='width=device-width, initial-scale=1.0'> <title>Jakarta Nanyang School Card Request</title> <style type='text/css'> #outlook a { padding: 0; } body { width: 100% !important; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; margin: 0; padding: 0; } .ExternalClass { width: 100%; } /* Force Hotmail to display emails at full width */ .ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div { line-height: 100%; } #backgroundTable { margin: 0; padding: 0; width: 100% !important; line-height: 100% !important; } img { outline: none; text-decoration: none; border: none; -ms-interpolation-mode: bicubic; } a img { border: none; } .image_fix { display: block; } p { margin: 0px 0px !important; } table td { border-collapse: collapse; } table { border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; } table[class=full] { width: 100%; clear: both; } </style></head><body> <div class='block'> <table width='100%' bgcolor='#f6f4f5' cellpadding='0' cellspacing='0' border='0' id='backgroundTable' st-sortable='preheader'> <tbody> <tr> <td width='100%'> <table width='580' cellpadding='0' cellspacing='0' border='0' align='center' class='devicewidth'> <tbody> <tr> <td width='100%' height='5'></td> </tr> <tr> <td width='100%' height='5'></td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </div> <div class='block'> <table width='100%' bgcolor='#f6f4f5' cellpadding='0' cellspacing='0' border='0' id='backgroundTable' st-sortable='header'> <tbody> <tr> <td> <table width='580' bgcolor='#0db9ea' cellpadding='0' cellspacing='0' border='0' align='center' class='devicewidth' hlitebg='edit' shadow='edit'> <tbody> <tr> <td> <table width='280' cellpadding='0' cellspacing='0' border='0' align='left' class='devicewidth'> <tbody> <tr> <td valign='middle' width='270' style='padding: 10px 0 10px 20px;' class='logo'> <div class='imgpop'> <a href='#'><img style='width:50px' src='http://united-comm.com/www2/wp-content/uploads/2015/05/jny.jpg' alt='logo' border='0' style='display:block; border:none; outline:none; text-decoration:none;' st-image='edit' class='logo'></a> </div> </td> </tr> </tbody> </table> <table width='280' cellpadding='0' cellspacing='0' border='0' align='right' class='devicewidth'> <tbody> <tr> <td width='270' valign='middle' style='font-family: Helvetica, Arial, sans-serif;font-size: 14px; color: #ffffff;line-height: 24px; padding: 10px 0;' align='right' class='menu' st-content='menu'> M.I.S Department &nbsp;|&nbsp; New Card Request </td> <td width='20'></td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </div> <div class='block'> <table width='100%' bgcolor='#f6f4f5' cellpadding='0' cellspacing='0' border='0' id='backgroundTable' st-sortable='bigimage'> <tbody> <tr> <td> <table bgcolor='#ffffff' width='580' align='center' cellspacing='0' cellpadding='0' border='0' class='devicewidth' modulebg='edit'> <tbody> <tr> <td> <table width='540' align='center' cellspacing='0' cellpadding='0' border='0' class='devicewidthinner'> <tbody> <tr> <td width='100%' height='20'></td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </div> <div class='block'> <table width='100%' bgcolor='#f6f4f5' cellpadding='0' cellspacing='0' border='0' id='backgroundTable' st-sortable='fulltext'> <tbody> <tr> <td> <table bgcolor='#ffffff' width='580' cellpadding='0' cellspacing='0' border='0' align='center' class='devicewidth' modulebg='edit'> <tbody> <tr> <td width='100%' height='30'></td> </tr> <tr> <td> <table width='540' align='center' cellpadding='0' cellspacing='0' border='0' class='devicewidthinner'> <tbody> <tr> <td style='font-family: Helvetica, arial, sans-serif; font-size: 18px; color: #333333; text-align:center;line-height: 20px;' st-title='fulltext-title'> " + TxtIdEmployee.Text + " - " + TxtName.Text + " </td> </tr> <tr> <td height='5'></td> </tr> <tr> <td style='font-family: Helvetica, arial, sans-serif; font-size: 14px; color: #95a5a6; text-align:center;line-height: 30px;' st-content='fulltext-paragraph'> Request for new employe has been send to MIS Department </td> </tr> <tr> <td width='100%' height='10'></td> </tr> <tr> <td> <table height='36' align='center' valign='middle' border='0' cellpadding='0' cellspacing='0' class='tablet-button' st-button='edit'> <tbody> <tr> <td width='auto' align='center' valign='middle' height='36' style=' background-color:#0db9ea; border-top-left-radius:4px; border-bottom-left-radius:4px;border-top-right-radius:4px; border-bottom-right-radius:4px; background-clip: padding-box;font-size:13px; font-family:Helvetica, arial, sans-serif; text-align:center; color:#ffffff; font-weight: 300; padding-left:25px; padding-right:25px;'> <span style='color: #ffffff; font-weight: 300;'> <a style='color: #ffffff; text-align:center;text-decoration: none;' href='#'>Check Status</a> </span> </td> </tr> </tbody> </table> </td> </tr> <tr> <td width='100%' height='30'></td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </div> <div class='block'> <table width='100%' bgcolor='#f6f4f5' cellpadding='0' cellspacing='0' border='0' id='backgroundTable' st-sortable='postfooter'> <tbody> <tr> <td width='100%'> <table width='580' cellpadding='0' cellspacing='0' border='0' align='center' class='devicewidth'> <tbody> <tr> <td width='100%' height='5'></td> </tr> <tr> <td align='center' valign='middle' style='font-family: Helvetica, arial, sans-serif; font-size: 10px;color: #999999' st-content='preheader'> You can not <a class='hlite' href='#' style='text-decoration: none; color: #0db9ea'>unsubscribe</a>. </td> </tr> <tr> <td width='100%' height='5'></td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </div></body></html>";
                                            //
                                            mailMessage.IsBodyHtml = true;

                                            smtpClient.Send(mailMessage);
                                            msgLabel.Text = "Sukses Kirim Email";

                                        }

                                        catch (SmtpException ex)
                                        {
                                            throw new ApplicationException
                                              ("SmtpException has occured: " + ex.Message);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                        
                                        //

                                        koneksi.Close();
                                        Response.Redirect("HrdCheck.aspx");
                                    }
                                    else
                                    {
                                        msgLabel.Text = "Insert Data Request Failed";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    msgLabel.Text = "Insert Request " + ex.Message;
                                }
                            }
                            else
                            {
                                msgLabel.Text = "Insert Data Employee Failed";
                            }

                        }
                        catch (Exception ex)
                        {
                            msgLabel.Text = "Insert Employee " + ex.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    msgLabel.Text = "Sql Command Check " + ex.Message;
                }
            }
        }
    }
}