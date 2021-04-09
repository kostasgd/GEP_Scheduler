using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Sql;
using System.Data.SqlClient;

namespace GEP_Scheduler
{
    public partial class WPF_Add_Activity : Window
    {
        private readonly MainWindow frm1;
        public WPF_Add_Activity(MainWindow mw)
        {
            InitializeComponent();
            datepicker.SelectedDate = DateTime.Today;
            frm1 = mw;
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_add_click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtxtdesc.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtxtdesc.Document.ContentEnd
                );
            if (textRange.ToString().Length > 0)
            {
                String myConnection = @"Data Source=.;Initial Catalog=Gep_Scheduler;Integrated Security=True";
                SqlConnection Connect = null;
                try
                {
                    Connect = new SqlConnection(myConnection);
                    Connect.Open();
                    String insertQuery = "INSERT INTO dbo.activity([Desc],[Date],[Priority],[Status]) VALUES(@Activity_Desc, @Date, " +
                        "@Priority,@Status)";
                    SqlCommand cmdActivityToDataBase = new SqlCommand(insertQuery, Connect);
                    cmdActivityToDataBase.Prepare();
                    //we will bound a value to the placeholder

                    var selectedpriority = ((ComboBoxItem)cbpriority.SelectedItem).Content.ToString();

                    cmdActivityToDataBase.Parameters.AddWithValue("@Activity_Desc", textRange.Text);
                    cmdActivityToDataBase.Parameters.AddWithValue("@Date", datepicker.SelectedDate.Value.Date.ToShortDateString());
                    cmdActivityToDataBase.Parameters.AddWithValue("@Priority", selectedpriority.ToString());
                    cmdActivityToDataBase.Parameters.AddWithValue("@Status", "Not Done");

                    cmdActivityToDataBase.ExecuteNonQuery(); //execute the sql command
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (Connect != null)
                    {
                        var Result = MessageBox.Show("Η εγγραφή έγινε με επιτυχία ,θέλετε να κάνετε και άλλη εγγραφή;", "Επιτυχία εισαγωγής στην βάση", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        //frm1.datagridactivity.Items.Refresh();
                        if (Result == MessageBoxResult.Yes)
                        {
                            rtxtdesc.Document.Blocks.Clear();
                            datepicker.DisplayDate = DateTime.Today;
                            cbpriority.SelectedIndex = 0;
                        }
                        else if (Result == MessageBoxResult.No)
                        {
                            this.Close();
                            Connect.Close(); //close the connection
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Σας παρακαλώ προσθέστε περιγραφή της δραστηριότητας","Προσοχή!",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        private void Rtxtdesc_TextChanged(object sender, TextChangedEventArgs e){}
        private void Window_GotFocus(object sender, RoutedEventArgs e) { }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rtxtdesc.Focus();
        }
    }
}
