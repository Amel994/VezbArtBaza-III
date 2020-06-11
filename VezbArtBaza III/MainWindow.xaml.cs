using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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

namespace VezbArtBaza_III
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Artikal a {get; set; } = new Artikal();
        public Racun r { get; set; } = new Racun();
        public ArtRac ar { get; set; } = new ArtRac();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            BindingGroup = new BindingGroup();

            Baza db = new Baza();
            dg.ItemsSource = db.AR.ToList();

        }

        private void UnosArt(object sender, RoutedEventArgs e)
        {
            Baza db = new Baza();
            BindingGroup.CommitEdit();
            
            ar.Art = a;
            ar.ArtID = a.ID;
            ar.Rac = r;
            ar.RacID = r.ID;
            ar.Kolicina = ar.Kolicina;
            db.AR.Add(ar);
            db.SaveChanges();

            dg.ItemsSource = db.AR.ToList();
        }
    }

    public class ArtRac : INotifyPropertyChanged
    {
        public int ArtID { get; set; }
        public Artikal Art { get; set; }
        public int RacID { get; set; }
        public Racun Rac { get; set; }
        private int _kolicina;
        public int Kolicina 
        {
            get => _kolicina;
            set
            {
                _kolicina = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kolicina"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        
    }


    public class Artikal: INotifyPropertyChanged
    {
        public int ID { get; set; }
        private string _naziv;
        public string Naziv
        {
            get => _naziv;
            set
            {
                _naziv = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Naziv"));
            }
        }

        private decimal _cena;
        public decimal Cena 
        { 
            get => _cena;
            set
            {
                _cena = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cena"));
            }
        }

        public Artikal(string n, decimal c)
        {
            Naziv = n;
            Cena = c;
        }
        public Artikal() { }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString() => $"Naziv artikla: {Naziv} -- Cena artikla: {Cena}";
    }

    public class Racun
    {
        public int ID { get; set; }
        public DateTime Vreme { get; set; } = DateTime.Now;

        public override string ToString() => $"Datum izdavanja: {Vreme}";
    }

    public class Baza : DbContext
    {
        public Baza(): base (@"Data Source=DESKTOP-41DTPBO\TESTSERVER;Initial Catalog=ArtRac3;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artikal>().HasKey(a => a.ID);
            modelBuilder.Entity<Racun>().HasKey(r => r.ID);
            modelBuilder.Entity<ArtRac>().HasKey(ar => new { ar.ArtID, ar.RacID });
        }

        public DbSet<Artikal> Artikals { get; set; }
        public DbSet<Racun> Racuns { get; set; }
        public DbSet<ArtRac> AR { get; set; }
    }

}
