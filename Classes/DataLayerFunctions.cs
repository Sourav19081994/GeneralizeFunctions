using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Classes
{
    public class DataLayerFunctions
    {
        private connnection cnn = new connnection();
        protected DataSet Executeproc(string storedproc, string[] paraname, string[] paravalue)
        {
            try
            {
                SqlDataAdapter da;
                DataSet ds;
                SqlConnection cn = new SqlConnection(cnn.DbConnectionString);
                SqlCommand cmd = new SqlCommand(storedproc, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < paraname.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paraname[i], paravalue[i]);
                }
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                cn.Dispose();
                cn.Close();
                return ds;
            }
            catch
            {
                throw;
            }
        }

        protected int ExecuteNonproc(string storedproc, string[] paraname, string[] paravalue)
        {
            try
            {
                int result = 0;
                SqlConnection cn = new SqlConnection(cnn.DbConnectionString);
                SqlCommand cmd = new SqlCommand(storedproc, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < paraname.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paraname[i], paravalue[i]);
                }
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                cn.Open();
                result = cmd.ExecuteNonQuery();
                cn.Dispose();
                cn.Close();
                return result;
            }
            catch
            {
                throw;
            }
        }
        protected int ExecuteNonproc(string storedproc, string[] paraname, string[] paravalue, string[] Tparaaname, DataTable[] dtTables)
        {
            try
            {
                int result = 0;
                SqlConnection cn = new SqlConnection(cnn.DbConnectionString);
                SqlCommand cmd = new SqlCommand(storedproc, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < paraname.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paraname[i], paravalue[i]);
                }
                for (int i = 0; i < Tparaaname.Length; i++)
                {
                    cmd.Parameters.AddWithValue(Tparaaname[i], dtTables[i]);
                }
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                cn.Open();
                result = cmd.ExecuteNonQuery();
                cn.Dispose();
                cn.Close();
                return result;
            }
            catch
            {
                throw;
            }
        }
        protected int ExecuteNonproc(string storedproc, Dictionary<string, object> parameters)
        {
            try
            {
                int result = 0;
                SqlConnection cn = new SqlConnection(cnn.DbConnectionString);
                SqlCommand cmd = new SqlCommand(storedproc, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                //for (int i = 0; i < paraname.Length; i++)
                //{
                //    cmd.Parameters.AddWithValue(paraname[i], paravalue[i]);
                //}
                foreach (var obj in parameters)
                {
                    cmd.Parameters.AddWithValue(obj.Key, obj.Value);
                }

                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                cn.Open();
                result = cmd.ExecuteNonQuery();
                cn.Dispose();
                cn.Close();
                return result;
            }
            catch
            {
                throw;
            }
        }

        protected DataSet Executeproc(string storedproc, Dictionary<string, object> parameters)
        {
            try
            {
                SqlDataAdapter da;
                DataSet ds;
                SqlConnection cn = new SqlConnection(cnn.DbConnectionString);
                SqlCommand cmd = new SqlCommand(storedproc, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var obj in parameters)
                {
                    cmd.Parameters.AddWithValue(obj.Key, obj.Value);
                }

                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                cn.Dispose();
                cn.Close();
                return ds;
            }
            catch
            {
                throw;
            }
        }

        public DataSet Inline_Process(String Query)
        {


            SqlConnection con = new SqlConnection(cnn.DbConnectionString);
            SqlCommand cmd = new SqlCommand(Query, con);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            da.Dispose();
            con.Dispose();
            return ds;

        }

        public int Inline_ExecuteNonQry(String Query)
        {
            SqlConnection con = new SqlConnection(cnn.DbConnectionString);
            SqlCommand cmd = new SqlCommand(Query, con);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            return cmd.ExecuteNonQuery();
        }

        public string SaveSingleImages(string directory, HttpPostedFileBase f)
        {
            string path = "", retpath = "";
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));
                }

                if (f != null)
                {
                    if (f.ContentLength > 0)
                    {
                        int fCount = 0;
                        fCount = Directory.GetFiles(HttpContext.Current.Server.MapPath(directory), "*", SearchOption.AllDirectories).Length;
                        fCount++;
                        path = directory + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + fCount.ToString() + Path.GetExtension(f.FileName);

                        f.SaveAs(HttpContext.Current.Server.MapPath(path));
                        if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                        {
                            retpath = path;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return retpath;
        }

        //--------------------------------------------------------------------

        public string NewSaveSingleImages(string directory, HttpPostedFileBase f, string oldfile)
        {
            string path = "", retpath = "";
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
                {

                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));
                }
                if (f != null)
                {
                    try
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath(directory + oldfile)))
                        {
                            File.Delete(HttpContext.Current.Server.MapPath(directory + oldfile));
                        }
                    }
                    catch (Exception)
                    {
                    }

                    if (f.ContentLength > 0)
                    {
                        int fCount = 0;
                        fCount =
                            Directory.GetFiles(HttpContext.Current.Server.MapPath(directory), "*",
                                SearchOption.AllDirectories).Length;
                        fCount++;
                        oldfile = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + fCount.ToString() +
                                  Path.GetExtension(f.FileName);
                        path = directory + oldfile;

                        f.SaveAs(HttpContext.Current.Server.MapPath(path));
                        if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                        {
                            retpath = oldfile;
                        }
                    }
                }
                else
                {
                    if (oldfile != "")
                    {
                        path = directory + oldfile;
                        if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                        {
                            retpath = oldfile;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return retpath;
        }

        public string NewSaveSingleImages2(string directory, HttpPostedFile f, string oldfile)
        {
            string path = "", retpath = "";
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
                {

                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));
                }
                if (f != null)
                {
                    try
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath(directory + oldfile)))
                        {
                            File.Delete(HttpContext.Current.Server.MapPath(directory + oldfile));
                        }
                    }
                    catch (Exception)
                    {
                    }

                    if (f.ContentLength > 0)
                    {
                        int fCount = 0;
                        fCount =
                            Directory.GetFiles(HttpContext.Current.Server.MapPath(directory), "*",
                                SearchOption.AllDirectories).Length;
                        fCount++;
                        oldfile = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + fCount.ToString() +
                                  Path.GetExtension(f.FileName);
                        path = directory + oldfile;

                        f.SaveAs(HttpContext.Current.Server.MapPath(path));
                        if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                        {
                            retpath = oldfile;
                        }
                    }
                }
                else
                {
                    if (oldfile != "")
                    {
                        path = directory + oldfile;
                        if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                        {
                            retpath = oldfile;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return retpath;
        }

        //public string NewSaveSingleImages(string directory, HttpPostedFileBase f, string oldfile)
        //{
        //    string path = "", retpath = "";
        //    try
        //    {
        //        if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
        //        {

        //            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));
        //        }
        //        if (f != null)
        //        {
        //            try
        //            {
        //                if (File.Exists(directory + oldfile))
        //                {
        //                    File.Delete(directory + oldfile);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //            }

        //            if (f.ContentLength > 0)
        //            {
        //                int fCount = 0;
        //                fCount = Directory.GetFiles(HttpContext.Current.Server.MapPath(directory), "*", SearchOption.AllDirectories).Length;
        //                fCount++;
        //                oldfile = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + fCount.ToString() + Path.GetExtension(f.FileName);
        //                path = directory + oldfile;

        //                f.SaveAs(HttpContext.Current.Server.MapPath(path));
        //                if (File.Exists(HttpContext.Current.Server.MapPath(path)))
        //                {
        //                    retpath = oldfile;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (oldfile != "")
        //            {
        //                path = directory + oldfile;
        //                if (File.Exists(HttpContext.Current.Server.MapPath(path)))
        //                {
        //                    retpath = oldfile;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return retpath;
        //}

        public string NewSaveSingleImages(string directory, string ImgId, HttpPostedFileBase f, string oldfile)
        {
            string path = "", retpath = "";
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
                {

                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));
                }
                if (f != null)
                {
                    try
                    {
                        if (File.Exists(directory + oldfile))
                        {
                            File.Delete(directory + oldfile);
                        }
                    }
                    catch (Exception)
                    {
                    }

                    if (f.ContentLength > 0)
                    {
                        int fCount = 0;
                        fCount = Directory.GetFiles(HttpContext.Current.Server.MapPath(directory), "*", SearchOption.AllDirectories).Length;
                        fCount++;
                        oldfile = ImgId + "_" + fCount.ToString() + Path.GetExtension(f.FileName);
                        path = directory + oldfile;

                        f.SaveAs(HttpContext.Current.Server.MapPath(path));
                        if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                        {
                            retpath = oldfile;
                        }
                    }
                }
                else
                {
                    if (oldfile != "")
                    {
                        path = directory + oldfile;
                        if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                        {
                            retpath = oldfile;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return retpath;
        }


        public bool CheckImageExtention(string ext)
        {
            bool flag = false;
            string[] extensionlist = { "jpg", "jpeg", "bmp", "png" };
            for (int i = 0; i < extensionlist.Length; i++)
            {
                if (ext.ToString().ToLower() == extensionlist[i])
                {
                    flag = true;
                }
            }
            return flag;
        }

        public bool CheckResumeExtention(string ext)
        {
            bool resume = false;
            string[] extensionlist = { "txt", "pdf", "doc", "docx" };
            for (int i = 0; i < extensionlist.Length; i++)
            {
                if (ext.ToString().ToLower() == extensionlist[i])
                {
                    resume = true;
                }
            }
            return resume;
        }

        public string CheckYoutubeURL(string YURL)
        {
            try
            {
                string url = YURL;  //"https://www.youtube.com/watch?v=eRsGyueVLvQ";
                Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:(.*)v(/|=)|(.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);
                Match youtubeMatch = YoutubeVideoRegex.Match(url);

                if (youtubeMatch.Success)
                { YURL = youtubeMatch.Groups[4].Value; }
                else { YURL = ""; }
            }
            catch { YURL = ""; }
            return YURL;
        }

        //Page wise Inline query-------------
        public DataSet Inline_Process(String Query, string OrderBy, string AscDesc, long Page, long PageSize)
        {
            string[] paraname = { "@Qry", "@OrderBy", "@ASCDESC", "@Page", "@rowsPerPage" };
            string[] paravalue = { Query, OrderBy, AscDesc, Page.ToString(), PageSize.ToString() };
            DataSet ds = Executeproc("ExecuteQueryPageWise", paraname, paravalue);
            return ds;
        }
        // Order By ASC or Desc
        public DataSet Inline_Process(String Query, string OrderBy, long Page, long PageSize)
        {
            string[] paraname = { "@Qry", "@OrderBy", "@Page", "@rowsPerPage" };
            string[] paravalue = { Query, OrderBy, Page.ToString(), PageSize.ToString() };
            DataSet ds = Executeproc("ExecuteQueryPageWise_v3", paraname, paravalue);
            return ds;
        }


        // Inline Process with full text Search
        public DataSet Inline_Process(String Query, string OrderBy, long Page, long PageSize, string Searchtxt, string SearchOnColumns, string UniqueColId, string Schema, string TableName)
        {
            string[] paraname = { "@Qry", "@OrderBy", "@Page", "@rowsPerPage", "@Search", "@SearchOnColumns", "@UniqueColId", "@Schema", "@TableName" };
            string[] paravalue = { Query, OrderBy, Page.ToString(), PageSize.ToString(), Searchtxt, SearchOnColumns, UniqueColId, Schema, TableName };
            DataSet ds = Executeproc("PageWiseQuery_FullTxtSearch", paraname, paravalue);
            return ds;
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }


        public int InlineNonproc(string Query)
        {
            try
            {
                int result = 0;
                SqlConnection cn = new SqlConnection(cnn.DbConnectionString);
                SqlCommand cmd = new SqlCommand(Query, cn);
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                cn.Open();
                result = cmd.ExecuteNonQuery();
                cn.Dispose();
                cn.Close();
                return result;
            }
            catch
            {
                throw;
            }
        }

        public bool checkExists(string Table, string Column, string Value)
        {
            DataSet ds = Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkExists(string Table, string Column, string Value, string IdColumn, string IdValue)
        {
            DataSet ds = Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "' and " + IdColumn + "<>'" + IdValue + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkExistsforEdit(string Table, string Column, string Value, string uniquecolumn, string uniqueid)
        {
            DataSet ds = Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "' and " + uniquecolumn + "<>'" + uniqueid + "' ");
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkExistsCategoryWise(string Table, string Column, string Value, string IdColumn, string IdValue)
        {
            DataSet ds = Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "' and " + IdColumn + "='" + IdValue + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public DataRow getDataRow(string Table, string Column, string Value)
        {
            DataSet ds = Inline_Process("select * from " + Table + " where " + Column + "='" + Value + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0];
            }
            else
            {
                return null;
            }
        }

        public int DeleteRow(string Table, string Column, string Value)
        {
            return InlineNonproc("delete from " + Table + " where " + Column + "='" + Value + "'");
        }

        public string GeenrateRandomnumber(string prefix)
        {
            string randomnumber = "";


            Random a = new Random(DateTime.Now.GetHashCode());
            randomnumber = prefix + a.Next(0, 999999999).ToString("000000000");

            return randomnumber;

        }

        public string[] GetDistance_Duration_BetweenPoints(double lat1, double long1, double lat2, double long2)
        {
            double distance = 0, dist_km = 0;
            long dinMin = 0;

            string[] data = new string[2];
            string _distance = "", _duration = "";
            try
            {
                string url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + lat1 + "," + long1 + "&destinations=" + lat2 + "," + long2 + "";
                WebClient client = new WebClient();
                // result = Newtonsoft.Json.JsonConvert.DeserializeObject<VehicleTypeClass.DistanceModelApi>(client.DownloadString(url));
                dynamic stuff = JsonConvert.DeserializeObject(client.DownloadString(url));
                string daddress = stuff.destination_addresses[0];
                string saddress = stuff.origin_addresses[0];
                string status = stuff.status;
                if (status == "OK")
                {
                    if (stuff.rows.Count > 0)
                    {
                        //distance = stuff.rows[0].elements[0].distance.value;

                        try
                        {
                            if (stuff.rows[0].elements[0].status == "OK")
                            {
                                _distance = stuff.rows[0].elements[0].distance.value;
                                _duration = stuff.rows[0].elements[0].duration.value;
                            }
                        }
                        catch (Exception)
                        {
                        }
                        double.TryParse(_distance, out distance);
                    }
                }
            }
            catch (Exception)
            {
            }

            long.TryParse(_duration, out dinMin);
            dinMin = dinMin / 60;
            dist_km = distance / 1000;

            data[0] = dist_km.ToString();
            data[1] = dinMin.ToString();


            return data;

        }


        public bool checkExistsCategoryWiseEdit(string Table, string Column, string Value, string IdColumn, string IdValue, string UniqColumn, string UniqColumnValue)
        {
            DataSet ds = Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "' and " + IdColumn + "='" + IdValue + "' and " + UniqColumn + "<>'" + UniqColumnValue + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkExistsCategoryWiseForBothParamater(string Table, string Column, string Value, string IdColumn, string IdValue, string SecondColoumn, string Secondvalue)
        {
            string qry = "select " + Column + " from " + Table + " where " + Column + "='" + Value + "' and " + IdColumn + "='" + IdValue + "'and " + SecondColoumn + "='" + Secondvalue + "'  ";
            DataSet ds = Inline_Process(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkExistsCategoryWiseForBothParamaterEdit(string Table, string Column, string Value, string IdColumn, string IdValue, string SecondColoumn, string Secondvalue)
        {
            string qry = "select " + Column + " from " + Table + " where " + Column + "='" + Value + "' and " + IdColumn + "<>'" + IdValue + "'and " + SecondColoumn + "='" + Secondvalue + "'  ";
            DataSet ds = Inline_Process(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public  String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }

    }
}