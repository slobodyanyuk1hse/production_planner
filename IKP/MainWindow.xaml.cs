using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel; 
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.IO;

namespace IKP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();

        }
        public class Product
        {
            public int Product_Id { get; set; }
            public string Product_Name { get; set; }

            public string Is_Set { get; set; }
            public string PF_Name { get; set; }
            public string Machine_Centre_Name { get; set; }
            public float Cycle { get; set; }
            public float Weight { get; set; }
            public string Capacity { get; set; }
            public string Colour { get; set; }
            public float Min_Amount_Colour_Design { get; set; }
            public float Min_Amount_Form { get; set; }
            public float Human_Hour { get; set; }
            public string PG_Name { get; set; }
            public string PF_Type_Name { get; set; }
            public string Robot_Name { get; set; }
        }
        public class Machine
        {
            public string Machine_Centre_Name { get; set; }
            public string Working_Centre { get; set; }
            public string MC_Type_Name { get; set; }
        }
        public class Press_Form
        {
            public string PF_Name { get; set; }
            public string Description { get; set; }
            public int Gaps_Amount { get; set; }
            public int Set_Time { get; set; }
            public int Withdrawal_Time { get; set; }
            public int Launch_Time { get; set; }
            public int Colour_Change_Time { get; set; }
            
        }
        public List<string> headers1 = new List<string>();
        public List<string> headers2 = new List<string>();
        public List<string> headers3 = new List<string>();
        public void LoadPr(List<Product> _pr)
        {
            SearchList.Items.Clear(); // очищаем лист с элементами

            for (int i = 0; i < _pr.Count; i++) // перебираем элементы
            {
                SearchList.Items.Add(_pr[i]); // добавляем элементы в ListBox
            }
        }
        public void LoadMC(List<Machine> _mc)
        {
            SearchList.Items.Clear(); // очищаем лист с элементами

            for (int i = 0; i < _mc.Count; i++) // перебираем элементы
            {
                SearchList.Items.Add(_mc[i]); // добавляем элементы в ListBox
            }
        }
        public void LoadPF(List<Press_Form> _pf)
        {
            SearchList.Items.Clear(); // очищаем лист с элементами

            for (int i = 0; i < _pf.Count; i++) // перебираем элементы
            {
                SearchList.Items.Add(_pf[i]); // добавляем элементы в ListBox
            }
        }
        List<Product> products = new List<Product>();
        List<Machine> machines = new List<Machine>();
        List<Press_Form> forms = new List<Press_Form>();
        public void LoadData()
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost\SQLEXPRESS01; Initial Catalog=IKP; Integrated Security=True;");
            sqlCon.Open();
            string sql1 = "select Product_Id, Product_Name, Is_Set, PF_Name, Machine_Centre_Name, Cycle, Weight, Capacity, Colour, Min_Amount_Colour_Design, Min_Amount_Form, Human_Hour, PG_Name, PF_Type_Name, Robot_Name from Product p " +
                "join Press_Form pf on p.PF_Id = pf.PF_Id " +
                "join Machine_Centre mc on mc.Machine_Centre_Id = p.Machine_Centre_Id " +
                "join Product_Group pg on pg.PG_Id = p.PG_Id " +
                "join Form_Type ft on ft.PF_Type_Id = p.PF_Type_Id " +
                "join Robot r on r.Robot_Id = p.Robot_Id ";
            SqlCommand sqlCmd1 = new SqlCommand(sql1, sqlCon);
            SqlDataReader reader = sqlCmd1.ExecuteReader();
            while (reader.Read()) // построчно считываем данные
            {
                products.Add(new Product() { Product_Id= Convert.ToInt32(reader.GetValue(0)), Product_Name= Convert.ToString(reader.GetValue(1)), Is_Set= Convert.ToString(reader.GetValue(2)), PF_Name=Convert.ToString(reader.GetValue(3)),
                Machine_Centre_Name= Convert.ToString(reader.GetValue(4)), Cycle= Convert.ToSingle(reader.GetValue(5)), Weight= Convert.ToSingle(reader.GetValue(6)), Capacity= Convert.ToString(reader.GetValue(7)), Colour= Convert.ToString(reader.GetValue(8)),
                Min_Amount_Colour_Design = Convert.ToSingle(reader.GetValue(9)), Min_Amount_Form= Convert.ToSingle(reader.GetValue(10)), Human_Hour= Convert.ToSingle(reader.GetValue(11)), PG_Name= Convert.ToString(reader.GetValue(12)), PF_Type_Name= Convert.ToString(reader.GetValue(13)),
                Robot_Name= Convert.ToString(reader.GetValue(14))});
            }
            for (int i = 0; i < 15; i++)
            {
                String header = Convert.ToString(reader.GetName(i));
                headers1.Add(header);
            }
            reader.Close();

            string sql2 = "select Machine_Centre_Name, Working_Centre, MC_Type_Name from Machine_Centre mc " +
                "join WC_Type wc on mc.Working_Centre_Id = wc.WC_Id " +
                "join MC_Type mt on mt.MC_Type_Id = mc.MC_Type_Id";
            SqlCommand sqlCmd2 = new SqlCommand(sql2, sqlCon);
            SqlDataReader reader2 = sqlCmd2.ExecuteReader();
            while (reader2.Read()) // построчно считываем данные
            {
                machines.Add(new Machine()
                {
                    Machine_Centre_Name = Convert.ToString(reader2.GetValue(0)),
                    Working_Centre = Convert.ToString(reader2.GetValue(1)),
                    MC_Type_Name = Convert.ToString(reader2.GetValue(2))
                });
            }
            for (int i = 0; i < 3; i++)
            {
                String header = Convert.ToString(reader2.GetName(i));
                headers2.Add(header);
            }
            reader2.Close();

            string sql3 = "select PF_Name, Description, Gaps_Amount, Set_Time, Withdrawal_Time, Launch_Time, Colour_Change_Time from Press_Form";
            SqlCommand sqlCmd3 = new SqlCommand(sql3, sqlCon);
            SqlDataReader reader3 = sqlCmd3.ExecuteReader(); 
            while (reader3.Read()) // построчно считываем данные
            {
                forms.Add(new Press_Form()
                {
                    PF_Name = Convert.ToString(reader3.GetValue(0)),
                    Description = Convert.ToString(reader3.GetValue(1)),
                    Gaps_Amount = Convert.ToInt32(reader3.GetValue(2)),
                    Set_Time = Convert.ToInt32(reader3.GetValue(3)),
                    Withdrawal_Time = Convert.ToInt32(reader3.GetValue(4)),
                    Launch_Time = Convert.ToInt32(reader3.GetValue(5)),
                    Colour_Change_Time = Convert.ToInt32(reader3.GetValue(6))
                });
            }
            for (int i = 0; i < 7; i++)
            {
                String header = Convert.ToString(reader3.GetName(i));
                headers3.Add(header);
            }
            reader3.Close();
        }
        public void ActiveFilter(object sender, RoutedEventArgs e)
        {

            if (Filter.SelectedIndex == 0)
            {
                GridView myGridView = new GridView();
                myGridView.AllowsColumnReorder = true;
                for (int i = 0; i < 15; i++)
                {
                    String header = headers1[i];
                    GridViewColumn gvc1 = new GridViewColumn();
                    gvc1.DisplayMemberBinding = new Binding(header);
                    gvc1.Header = header;
                    gvc1.Width = 100;
                    myGridView.Columns.Add(gvc1);
                }
                SearchList.View = myGridView;

                List<Product> pr = new List<Product>();
                LoadPr(products);
                Bsearch.Click += new RoutedEventHandler(SearchPr);
            }
            if (Filter.SelectedIndex == 1)
            {
                GridView myGridView = new GridView();
                myGridView.AllowsColumnReorder = true;
                for (int i = 0; i < 3; i++)
                {
                    String header = headers2[i];
                    GridViewColumn gvc1 = new GridViewColumn();
                    gvc1.DisplayMemberBinding = new Binding(header);
                    gvc1.Header = header;
                    gvc1.Width = 250;
                    myGridView.Columns.Add(gvc1);
                }
                SearchList.View = myGridView;

                List<Machine> mc = new List<Machine>();
                LoadMC(machines);
                Bsearch.Click += new RoutedEventHandler(SearchMC);
            }
            if (Filter.SelectedIndex == 2)
            {
                GridView myGridView = new GridView();
                myGridView.AllowsColumnReorder = true;
                for (int i = 0; i < 6; i++)
                {
                    String header = headers3[i];
                    GridViewColumn gvc1 = new GridViewColumn();
                    gvc1.DisplayMemberBinding = new Binding(header);
                    gvc1.Header = header;
                    gvc1.Width = 200;
                    myGridView.Columns.Add(gvc1);
                }
                SearchList.View = myGridView;

                List<Press_Form> pf = new List<Press_Form>();
                LoadPF(forms);
                Bsearch.Click += new RoutedEventHandler(SearchPF);
            }
        }
        void SearchPr(object sender, RoutedEventArgs e)
        {
            List<Product> newPr = new List<Product>();
            newPr = products;

            newPr = newPr.FindAll(x => x.Product_Name.Contains(Searcher.Text) | x.PF_Type_Name.Contains(Searcher.Text) | x.PG_Name.Contains(Searcher.Text) | x.Machine_Centre_Name.Contains(Searcher.Text) | x.PF_Name.Contains(Searcher.Text) | x.Capacity.Contains(Searcher.Text) | x.Colour.Contains(Searcher.Text));

            LoadPr(newPr);
        }
        void SearchMC(object sender, RoutedEventArgs e)
        {
            List<Machine> newMc = new List<Machine>();
            newMc = machines;

            newMc = newMc.FindAll(x => x.Machine_Centre_Name.Contains(Searcher.Text) | x.MC_Type_Name.Contains(Searcher.Text) | x.Working_Centre.Contains(Searcher.Text));

            LoadMC(newMc);
        }
        void SearchPF(object sender, RoutedEventArgs e)
        {
            List<Press_Form> newPf = new List<Press_Form>();
            newPf = forms;

            newPf = newPf.FindAll(x => x.PF_Name.Contains(Searcher.Text));

            LoadPF(newPf);
        }
        public void Timetable()
        {
            TextBlock t1 = new TextBlock();
            t1.Text = "?";
            t1.Background = Brushes.Gold;
            t1.ToolTip = "Голубой - работы с пресс-формой" + "\r\n" + "Желтый - ТО Пресс-Формы" + "\r\n" + "Оранжевый - ТО Машины" + "\r\n" + "Зеленый - Процесс производства";
            t1.Width = 100;
            t1.Height = 30;
            t1.Foreground = System.Windows.Media.Brushes.Black;
            Grid.SetRow(t1, 0);
            Grid.SetColumn(t1, 0);
            gr.Children.Add(t1);

            DateTime timenow = DateTime.Now;
            for (int i=1;i<32;i++)
            {
                string time = timenow.AddDays(i).ToShortDateString();
                TextBlock t4 = new TextBlock();
                t4.Text = time;
                t4.FontWeight = FontWeights.Bold;
                t4.Foreground = Brushes.Navy;
                t4.Background = Brushes.White;
                t4.Width = 1440;
                t4.Height = 30;
                h1.Children.Add(t4);
            }

            TextBlock t2 = new TextBlock();
            t2.Text = "Machine";
            t2.Background = Brushes.White;
            t2.Foreground = Brushes.Navy;
            t2.FontWeight = FontWeights.Bold;
            t2.Width = 100;
            t2.Height = 30;
            Grid.SetRow(t2, 1);
            Grid.SetColumn(t2, 0);
            gr.Children.Add(t2);

            for (int i = 0; i < 31; i++)
            {
                for (int k = 0; k < 24; k++)
                {
                    TextBlock t5 = new TextBlock();
                    t5.Text = Convert.ToString(k);
                    t5.Background = Brushes.White;
                    t5.Foreground = Brushes.Navy;
                    t5.Width = 60;
                    t5.Height = 30;
                    h2.Children.Add(t5);
                }
            }
            string[] filePaths = Directory.GetFiles(@"C:\Users\maksi\source\repos\IKP\files\", "*.txt");
            List<string> normPaths = new List<string>();
            foreach(string k in filePaths)
            {
                if (k != @"C:\Users\maksi\source\repos\IKP\files\отгрузка.txt")
                {
                    int ind = k.LastIndexOf(@"\") + 1;
                    normPaths.Add(k.Substring(ind, k.Length - ind - 4).Replace('_', '/'));
                }
            }

            for (int i=0; i<machines.Count();i++)
            {
                TextBlock t6 = new TextBlock();
                t6.Text = machines[i].Machine_Centre_Name;
                t6.Background = Brushes.White;
                t6.Foreground = Brushes.Navy;
                t6.Width = 100;
                t6.Height = 30;
                Grid.SetRow(t6, i + 2);
                Grid.SetColumn(t6, 0);
                gr.Children.Add(t6);

                foreach(string k in normPaths)
                {
                    if (machines[i].Machine_Centre_Name.Trim() == k)
                    {
                        StackPanel stack = new StackPanel();
                        stack.Orientation = Orientation.Horizontal;
                        Grid.SetRow(stack, i + 2);
                        Grid.SetColumn(stack, 1);
                        string nm = k.Replace(@"/", "_");
                        using (StreamReader sr = new StreamReader(@"C:\Users\maksi\source\repos\IKP\files\" + nm + ".txt", Encoding.GetEncoding(1251)))
                        {
                            String line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                List<string> listStrLineElements = line.Split(',').ToList();
                                TextBlock t7 = new TextBlock();
                                t7.Width = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(listStrLineElements[1].Replace('.', ',').Trim())));
                                t7.Height = 30;
                                if(listStrLineElements[0].StartsWith("Pr"))
                                {
                                    t7.Background = Brushes.Yellow;
                                    t7.ToolTip = listStrLineElements[0] + "\r\n" + "start time: " + listStrLineElements[2] + "\r\n" + "end time: " + listStrLineElements[3];
                                    stack.Children.Add(t7);
                                }
                                else if (listStrLineElements[0].StartsWith('M'))
                                {
                                    t7.Background = Brushes.Coral;
                                    t7.ToolTip = listStrLineElements[0] + "\r\n" + "start time: " + listStrLineElements[2] + "\r\n" + "end time: " + listStrLineElements[3];
                                    stack.Children.Add(t7);
                                }
                                else if (listStrLineElements[0].StartsWith('s') ^ listStrLineElements[0].Trim().StartsWith('w') ^ listStrLineElements[0].Trim().StartsWith('r'))
                                {
                                    t7.Background = Brushes.LightSkyBlue;
                                    t7.ToolTip = listStrLineElements[0] + "\r\n" + "start time: " + listStrLineElements[2] + "\r\n" + "end time: " + listStrLineElements[3];
                                    stack.Children.Add(t7);
                                }
                                else
                                {
                                    t7.Background = Brushes.LightGreen;
                                    t7.ToolTip = listStrLineElements[0] + "\r\n" + "start time: " + listStrLineElements[2] + "\r\n" + "end time: " + listStrLineElements[3];
                                    stack.Children.Add(t7);
                                }
                            }
                        }
                        stack.Background = Brushes.Black;
                        gr.Children.Add(stack);
                    }
                }
            }

        }
        public class finish
        {
            public string Name { get; set; }
            public string Amount { get; set; }
            public string Date { get; set; }
        }
        public List<finish> finishes = new List<finish>();
        public void zagr()
        {
            Calendar cal = new Calendar();
            cal.HorizontalAlignment = HorizontalAlignment.Center;
            cal.VerticalAlignment = VerticalAlignment.Center;
            List<string> headers5 = new List<string>();
            headers5.Add("Name");
            headers5.Add("Amount");
            headers5.Add("Date");
            GridView myGridView = new GridView();
            myGridView.AllowsColumnReorder = true;
            for (int i = 0; i < 3; i++)
            {
                String header = headers5[i];
                GridViewColumn gvc5 = new GridViewColumn();
                gvc5.DisplayMemberBinding = new Binding(header);
                gvc5.Header = header;
                gvc5.Width = zp.ActualWidth / 3;
                myGridView.Columns.Add(gvc5);
            }
            zp.View = myGridView;
            using (StreamReader sr = new StreamReader(@"C:\Users\maksi\source\repos\IKP\files\отгрузка.txt"))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    List<string> listStrLine = line.Split(',').ToList();
                    for (int i = 0; i < 3; i++)
                    {
                        finishes.Add(new finish() {Name = listStrLine[0], Amount = listStrLine[1], Date=listStrLine[2]});
                        cal.BlackoutDates.Add(new CalendarDateRange(Convert.ToDateTime(listStrLine[2]), Convert.ToDateTime(listStrLine[2])));
                    }
                }
            }
            for(int i = 0; i<finishes.Count;i++)
            {
                zp.Items.Add(finishes[i]);
            }
            vb.Child = cal;

        }
        public void zpr()
        {
            for (int i = 0; i < machines.Count(); i++)
            {
                TextBlock t6 = new TextBlock();
                t6.Text = machines[i].Machine_Centre_Name;
                t6.Background = Brushes.White;
                t6.Foreground = Brushes.Navy;
                t6.Width = 100;
                t6.Height = 30;
                Grid.SetRow(t6, i);
                Grid.SetColumn(t6, 0);
                grid2.Children.Add(t6);

                string[] filePaths = Directory.GetFiles(@"C:\Users\maksi\source\repos\IKP\files\", "*.txt");
                List<string> normPaths = new List<string>();
                foreach (string k in filePaths)
                {
                    if (k != @"C:\Users\maksi\source\repos\IKP\files\отгрузка.txt")
                    {
                        int ind = k.LastIndexOf(@"\") + 1;
                        normPaths.Add(k.Substring(ind, k.Length - ind - 4).Replace('_', '/'));
                    }
                }
                foreach (string k in normPaths)
                {
                    if (machines[i].Machine_Centre_Name.Trim() == k)
                    {
                        StackPanel stack = new StackPanel();
                        stack.Orientation = Orientation.Horizontal;
                        Grid.SetRow(stack, i);
                        Grid.SetColumn(stack, 1);
                        string nm = k.Replace(@"/", "_");

                        float prod = 0;
                        float to = 0;
                        float ust = 0;
                        using (StreamReader sr = new StreamReader(@"C:\Users\maksi\source\repos\IKP\files\" + nm + ".txt", Encoding.GetEncoding(1251)))
                        {
                            String line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                List<string> listStrLineElements = line.Split(',').ToList();
                                if (listStrLineElements[0].StartsWith("Pr"))
                                {
                                    to = to + Convert.ToInt32(Decimal.Round(Convert.ToDecimal(listStrLineElements[1].Replace('.', ',').Trim())));
                                }
                                else if (listStrLineElements[0].StartsWith('M'))
                                {
                                    to += Convert.ToInt32(Decimal.Round(Convert.ToDecimal(listStrLineElements[1].Replace('.', ',').Trim())));
                                }
                                else if (listStrLineElements[0].StartsWith('s') ^ listStrLineElements[0].Trim().StartsWith('w') ^ listStrLineElements[0].Trim().StartsWith('r'))
                                {
                                    ust = ust + Convert.ToInt32(Decimal.Round(Convert.ToDecimal(listStrLineElements[1].Replace('.', ',').Trim())));
                                }
                                else
                                {
                                    prod = prod + Convert.ToInt32(Decimal.Round(Convert.ToDecimal(listStrLineElements[1].Replace('.', ',').Trim())));
                                }
                            }
                        }
                        float overall = prod + ust + to;

                        TextBlock textBlock1 = new TextBlock();
                        textBlock1.Text = Convert.ToString((prod / overall) * 100).Substring(0, 3) + "%";
                        textBlock1.Background = Brushes.LightGreen;
                        textBlock1.Width = (prod / overall) * 680;
                        textBlock1.Height = 30;

                        TextBlock textBlock2 = new TextBlock();
                        textBlock2.Text = Convert.ToString((to / overall) * 100).Substring(0, 3) + "%";
                        textBlock2.Background = Brushes.Yellow;
                        textBlock2.Width = (to / overall) * 680;
                        textBlock2.Height = 30;

                        TextBlock textBlock3 = new TextBlock();
                        textBlock3.Text = Convert.ToString((ust / overall) * 100).Substring(0, 3) + "%";
                        textBlock3.Background = Brushes.Coral;
                        textBlock3.Width = (ust / overall) * 680;
                        textBlock3.Height = 30;

                        stack.Background = Brushes.White;
                        stack.Children.Add(textBlock1);
                        stack.Children.Add(textBlock2);
                        stack.Children.Add(textBlock3);

                        stack.HorizontalAlignment = HorizontalAlignment.Left;
                        grid2.Children.Add(stack);
                    }
                }
            }
        }



        private void Click1(object sender, RoutedEventArgs e)
        {
            Info.Visibility = Visibility.Visible;
            Mach_zagr.Visibility = Visibility.Hidden;
            Production_timetable.Visibility = Visibility.Hidden;
            zagruzka.Visibility = Visibility.Hidden;

            
        }
        private void Click2(object sender, RoutedEventArgs e)
        {
            Info.Visibility = Visibility.Hidden;
            Mach_zagr.Visibility = Visibility.Hidden;
            Production_timetable.Visibility = Visibility.Hidden;
            zagruzka.Visibility = Visibility.Visible;
            zpr();
        }
        private void Click3(object sender, RoutedEventArgs e)
        {
            Info.Visibility = Visibility.Hidden;
            Mach_zagr.Visibility = Visibility.Hidden;
            zagruzka.Visibility = Visibility.Hidden;
            Production_timetable.Visibility = Visibility.Visible;
            Timetable();
        }
        private void Click4(object sender, RoutedEventArgs e)
        {
            Info.Visibility = Visibility.Hidden;
            Production_timetable.Visibility = Visibility.Hidden;
            zagruzka.Visibility = Visibility.Hidden;
            Mach_zagr.Visibility = Visibility.Visible;
            zagr();
        }
    }
}
