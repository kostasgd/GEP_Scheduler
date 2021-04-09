using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GEP_Scheduler
{
    /// <summary>
    /// Interaction logic for WPF_Add_IP.xaml
    /// </summary>
    public partial class WPF_Add_IP : Window
    {
        public WPF_Add_IP()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            String myConnection = @"Data Source=.;Initial Catalog=Gep_Scheduler;Integrated Security=True";
            SqlConnection Connect = null;
            try
            {
                Connect = new SqlConnection(myConnection);
                Connect.Open();
                String insertQuery = "INSERT INTO dbo.Ip_config([IP_IN],[IP_OUT],[PC_Name],[Office],[Full_name]) VALUES(@Ip_in, @Ip_out, " +
                    "@Pc_name,@Office,@Fullname)";
                SqlCommand cmdIp_confingToDataBase = new SqlCommand(insertQuery, Connect);
                cmdIp_confingToDataBase.Parameters.AddWithValue("@Ip_in", txtipin.Text);
                cmdIp_confingToDataBase.Parameters.AddWithValue("@Ip_out", txtout.Text);
                cmdIp_confingToDataBase.Parameters.AddWithValue("@Pc_name", txtpcname.Text);
                cmdIp_confingToDataBase.Parameters.AddWithValue("@Office", txtoffice.Text);
                cmdIp_confingToDataBase.Parameters.AddWithValue("@Fullname", txtfullname.Text);
                cmdIp_confingToDataBase.ExecuteNonQuery(); //execute the sql command
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
                    
                    if (Result == MessageBoxResult.Yes)
                    {
                        txtfullname.Text = "";
                        txtipin.Text = "";
                        txtoffice.Text = "";
                        txtout.Text = "";
                        txtpcname.Text = "";
                        txtipin.Focus();
                    }
                    else if (Result == MessageBoxResult.No)
                    {
                        this.Close();
                        Connect.Close(); //close the connection
                    }
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Txtipin_TextInput(object sender, TextCompositionEventArgs e)
        {
        }

        private void Txtipin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Txtout_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e){ }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtipin.Focus();
        }
    }
}
