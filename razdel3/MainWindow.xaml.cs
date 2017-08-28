using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace razdel3
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		BD_Connect BD = new BD_Connect();
		List<perechen> for_gr1 = new List<perechen>();


		public MainWindow()
		{


			InitializeComponent();

			DirectoryInfo sdsd = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
			BD.path = sdsd + @"\access.accdb";


			List<perechen> perech = new List<perechen>();

			var reader = BD.conn("SELECT naimenov FROM `perechenb` ");

			while (reader.Read())
			{


				perech.Add(new perechen
				{
					naimenovanie = reader["naimenov"].ToString()
				});
			}
			lb1.ItemsSource = perech;





		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			BD.conn("INSERT INTO `perechenb` (`naimenov`) VALUES ('" + tb1.Text + "') ");
		}

		private void lb1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			string maimrab = (lb1.SelectedItem as perechen).naimenovanie;
			for_gr1.Add(new perechen
			{
				naimenovanie = maimrab
			});
			dataGrid.ItemsSource = null;
			dataGrid.ItemsSource = (for_gr1);
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{

			DateTime start = (DateTime)tpstart.SelectedDate;
			DateTime end = (DateTime)tpend.SelectedDate;
			DateTime tek = start;
			//MessageBox.Show(end.ToString());
			int i = 0;
			List<string> days = new List<string>();
			while (start <= end)
			{
				//MessageBox.Show(start.ToString());
				days.Add(start.ToString("dd.MM.yyyy"));
				start = start.AddDays(1);
				i++;

			}
			lb2.ItemsSource = days;
			//MessageBox.Show(i.ToString());
		}



		private void lb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			string massv = "";

			for (int i = 0; i < lb2.SelectedItems.Count; i++)
			{
				massv += lb2.SelectedItems[i].ToString() + ",";


			}

			var selit = (dataGrid.SelectedItem as perechen);
			selit.massivdat = massv;
			dataGrid.ItemsSource = null;
			dataGrid.ItemsSource = for_gr1;
			dataGrid.SelectedItem = selit;

		}
	}

	class perechen
	{
		public string naimenovanie { get; set; }
		public string obem { get; set; }
		public string massivdat { get; set; }

	}



	public class BD_Connect
	{

		public string path;
		public OleDbDataReader conn(string zapros)
		{

			OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + path + "");//подключаемся к базе
			OleDbCommand cmd = new OleDbCommand();//инициализируем запрос
			cmd.Connection = conn;//подключаемся к бд
			conn.Open();//открываем соединение
			cmd.CommandText = (zapros);
			OleDbDataReader reader = cmd.ExecuteReader();//выполняем запрос

			return reader;

		}


	}
}
