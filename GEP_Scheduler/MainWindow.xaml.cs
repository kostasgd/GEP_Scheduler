using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid.Converter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace GEP_Scheduler
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnupdateactivity.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            btnupdateipconf.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));  
        }
        public DataTable fillDataTable(string table)
        {
            string query = "SELECT * FROM dbo."+table;

            using (SqlConnection sqlConn = new SqlConnection(@"Data Source=localhost;Initial Catalog=Gep_Scheduler;Integrated Security=True"))
            using (SqlCommand cmd = new SqlCommand(query, sqlConn))
            {
                sqlConn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e) { }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void Item0_Selected(object sender, RoutedEventArgs e)
        {
            titem1.IsSelected = true;
        }

        private void Item1_Selected(object sender, RoutedEventArgs e)
        {
            titem2.IsSelected = true;
        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e) { }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GEP_Scheduler.Gep_SchedulerDataSet gep_SchedulerDataSet = ((GEP_Scheduler.Gep_SchedulerDataSet)(this.FindResource("gep_SchedulerDataSet")));
            // Load data into the table Ip_config. You can modify this code as needed.
            GEP_Scheduler.Gep_SchedulerDataSetTableAdapters.Ip_configTableAdapter gep_SchedulerDataSetIp_configTableAdapter = new GEP_Scheduler.Gep_SchedulerDataSetTableAdapters.Ip_configTableAdapter();
            gep_SchedulerDataSetIp_configTableAdapter.Fill(gep_SchedulerDataSet.Ip_config);
            System.Windows.Data.CollectionViewSource ip_configViewSource1 = ((System.Windows.Data.CollectionViewSource)(this.FindResource("ip_configViewSource1")));
            ip_configViewSource1.View.MoveCurrentToFirst();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            WPF_Add_Activity waa = new WPF_Add_Activity(mw);
            waa.ShowDialog();
            btnupdateactivity.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = @"Data Source=localhost;Initial Catalog=Gep_Scheduler;Integrated Security=True";
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT [Activity_ID],[Desc],[Date],[Priority],[Status] FROM dbo.Activity", con);
                DataTable dt = new DataTable("Fill Activities");
                da.Fill(dt);
                dgvActivity.ItemsSource = dt.DefaultView;
                con.Close();
            }
        }

        private void Addip_Click(object sender, RoutedEventArgs e)
        {
            WPF_Add_IP wai = new WPF_Add_IP();
            wai.ShowDialog();
            btnupdateipconf.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void Border_Loaded(object sender, RoutedEventArgs e) { }

        private void Btnupdateipconf_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = @"Data Source=.;Initial Catalog=Gep_Scheduler;Integrated Security=True";
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT [Ip_ID],[IP_IN],[IP_OUT],[Pc_Name],[Office],[Full_Name] FROM dbo.Ip_config", con);
                DataTable dt = new DataTable("Fill Ip_conf");
                da.Fill(dt);
                DataGrid dg = new DataGrid();
                dgvipconfig.ItemsSource = dt.DefaultView;
                con.Close();
            }
        }


        private void Btndelete_Click(object sender, RoutedEventArgs e)
        {      
            var items = dgvActivity.SelectedItems;
            foreach (DataRowView item in items)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection())
                    {
                        var clRootSiteId = item["Activity_ID"];
                        con.ConnectionString = @"Data Source=.;Initial Catalog=Gep_Scheduler;Integrated Security=True";
                        con.Open();
                        String deleteQuery = "DELETE FROM dbo.Activity WHERE [Activity_ID]=@id";
                        SqlCommand cmdDeleteActivity = new SqlCommand(deleteQuery, con);
                        cmdDeleteActivity.Prepare();
                        cmdDeleteActivity.Parameters.AddWithValue("@id", Int32.Parse( clRootSiteId.ToString()));
                        cmdDeleteActivity.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
               
            }
            btnupdateactivity.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void btndeleteipconfig(object sender, RoutedEventArgs e)
        {
            var items = dgvipconfig.SelectedItems;
            foreach (DataRowView item in items)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection())
                    {
                        var clRootSiteId = item["Ip_ID"];
                        con.ConnectionString = @"Data Source=.;Initial Catalog=Gep_Scheduler;Integrated Security=True";
                        con.Open();
                        String deleteQuery = "DELETE FROM dbo.Ip_config WHERE [Ip_ID]=@id";
                        SqlCommand cmdDeleteActivity = new SqlCommand(deleteQuery, con);
                        cmdDeleteActivity.Prepare();
                        cmdDeleteActivity.Parameters.AddWithValue("@id", Int32.Parse(clRootSiteId.ToString()));
                        cmdDeleteActivity.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            btnupdateipconf.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e){}
        private void Window_LostFocus(object sender, RoutedEventArgs e){}
        private void Window_MouseLeave(object sender, MouseEventArgs e) { }
        private void Window_MouseMove(object sender, MouseEventArgs e){}

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var items = dgvActivity.SelectedItems;
            foreach (DataRowView item in items)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection())
                    {
                        var clRootSiteId = item["Activity_ID"];
                        con.ConnectionString = @"Data Source=.;Initial Catalog=Gep_Scheduler;Integrated Security=True";
                        con.Open();
                        String deleteQuery = "Update dbo.Activity SET Status='Done' WHERE [Activity_ID]=@id ";
                        SqlCommand cmdDeleteActivity = new SqlCommand(deleteQuery, con);
                        cmdDeleteActivity.Prepare();
                        cmdDeleteActivity.Parameters.AddWithValue("@id", Int32.Parse(clRootSiteId.ToString()));
                        cmdDeleteActivity.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            btnupdateactivity.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void Item2_Selected(object sender, RoutedEventArgs e){ }

        private void Item3_Selected(object sender, RoutedEventArgs e)
        {
            titem3.IsSelected = true;
        }

        public void ExportToPdf(DataTable dt, string strFilePath)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(strFilePath, FileMode.Create));
            document.Open();
            iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);

            PdfPTable table = new PdfPTable(dt.Columns.Count);
            PdfPRow row = null;
            float[] widths = new float[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
                widths[i] = 4f;

            table.SetWidths(widths);

            table.WidthPercentage = 100;
            int iCol = 0;
            string colname = "";
            PdfPCell cell = new PdfPCell(new Phrase("Ip_Config"));

            cell.Colspan = dt.Columns.Count;

            foreach (DataColumn c in dt.Columns)
            {
                table.AddCell(new Phrase(c.ColumnName, font5));
            }

            foreach (DataRow r in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int h = 0; h < dt.Columns.Count; h++)
                    {
                        table.AddCell(new Phrase(r[h].ToString(), font5));
                    }
                }
            }
            document.Add(table);
            document.Close();
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            DataTable dt = fillDataTable("Ip_config");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                ExportToPdf(dt,saveFileDialog.FileName +".pdf");
        }

        private void Dgvipconfig_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Dgvipconfig_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void Dgvipconfig_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
        }

        private void Dgvipconfig_MouseLeave(object sender, MouseEventArgs e)
        {
            var items = dgvipconfig.SelectedItems;
            foreach (DataRowView item in items)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection())
                    {
                        int ipid = Int32.Parse(item.Row.ItemArray[0].ToString());
                        string ipin = item.Row.ItemArray[1].ToString();
                        string ipout = item.Row.ItemArray[2].ToString();
                        string pcname = item.Row.ItemArray[3].ToString();
                        string office = item.Row.ItemArray[4].ToString();
                        string fullname = item.Row.ItemArray[5].ToString();
                        con.ConnectionString = @"Data Source=.;Initial Catalog=Gep_Scheduler;Integrated Security=True";
                        con.Open();
                        String deleteQuery = "UPDATE dbo.Ip_config SET [IP_IN]=@ipin,[IP_OUT]=@ipout,[Pc_Name]=@pcname," +
                            "[Office]=@office,[Full_Name]=@fullname WHERE [Ip_ID]=@id";
                        SqlCommand cmdDeleteActivity = new SqlCommand(deleteQuery, con);
                        cmdDeleteActivity.Prepare();
                        cmdDeleteActivity.Parameters.AddWithValue("@ipin", ipin);
                        cmdDeleteActivity.Parameters.AddWithValue("@ipout", ipout);
                        cmdDeleteActivity.Parameters.AddWithValue("@pcname", pcname);
                        cmdDeleteActivity.Parameters.AddWithValue("@office", office);
                        cmdDeleteActivity.Parameters.AddWithValue("@fullname", fullname);
                        cmdDeleteActivity.Parameters.AddWithValue("@id", 21);

                        cmdDeleteActivity.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            btnupdateipconf.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            //Console.WriteLine(ipid);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            var items = dgvipconfig.SelectedItems;
            if (items != null)
            {
                foreach (DataRowView item in items)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection())
                        {
                            int ipid = Int32.Parse(item.Row.ItemArray[0].ToString());
                            string ipin = item.Row.ItemArray[1].ToString();
                            string ipout = item.Row.ItemArray[2].ToString();
                            string pcname = item.Row.ItemArray[3].ToString();
                            string office = item.Row.ItemArray[4].ToString();
                            string fullname = item.Row.ItemArray[5].ToString();
                            con.ConnectionString = @"Data Source=.;Initial Catalog=Gep_Scheduler;Integrated Security=True";
                            con.Open();
                            String deleteQuery = "UPDATE dbo.Ip_config SET [IP_IN]=@ipin,[IP_OUT]=@ipout,[Pc_Name]=@pcname," +
                                "[Office]=@office,[Full_Name]=@fullname WHERE [Ip_ID]=@id";
                            SqlCommand cmdDeleteActivity = new SqlCommand(deleteQuery, con);
                            cmdDeleteActivity.Prepare();
                            cmdDeleteActivity.Parameters.AddWithValue("@ipin", ipin);
                            cmdDeleteActivity.Parameters.AddWithValue("@ipout", ipout);
                            cmdDeleteActivity.Parameters.AddWithValue("@pcname", pcname);
                            cmdDeleteActivity.Parameters.AddWithValue("@office", office);
                            cmdDeleteActivity.Parameters.AddWithValue("@fullname", fullname);
                            cmdDeleteActivity.Parameters.AddWithValue("@id", 21);

                            cmdDeleteActivity.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            btnupdateipconf.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void Btnexportact_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = fillDataTable("Activity");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                ExportToPdf(dt, saveFileDialog.FileName + ".pdf");
        }
    }
}


