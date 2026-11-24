using pizzafutar;
using System;
using System.Collections.Generic;
using System.Text;
namespace PizzaFutar
{
    internal class Feladatok
    {
        private readonly List<Rendeles> lista;

        public Feladatok()
        {
            Adatbazis adatbazis = new Adatbazis();
            lista = adatbazis.Feltolt();
        }

        public void OlcsoPizzak()
        {
            Console.WriteLine("\n3. feladat - 1500 Ft-nál olcsóbb pizzák");
            var eredmeny = lista
                .Select(r => r.Pizza)
                .Where(p => p.Ar < 1500)
                .DistinctBy(p => p.Id)
                .OrderByDescending(p => p.Meret)
                .ThenBy(p => p.Nev);

            foreach (var pizza in eredmeny)
                Console.WriteLine($"{pizza.Nev,-20} {pizza.Meret,3} cm {pizza.Ar,6} Ft");
        }

        public void LegnepszerubbPizza()
        {
            Console.WriteLine("\n4. feladat - Legtöbbet rendelt pizza");
            var osszesites = lista
                .GroupBy(r => new { r.Pizza.Nev, r.Pizza.Meret })
                .Select(g => new {
                    g.Key.Nev,
                    g.Key.Meret,
                    Darab = g.Sum(r => r.Db)
                })
                .ToList();

            if (osszesites.Count == 0)
            {
                Console.WriteLine("Nincs megjeleníthető rendelés.");
                return;
            }

            int maximum = osszesites.Max(x => x.Darab);
            foreach (var pizza in osszesites.Where(x => x.Darab == maximum)
                .OrderBy(x => x.Nev)
                .ThenBy(x => x.Meret))
            {
                Console.WriteLine($"{pizza.Nev}, {pizza.Meret} cm - {pizza.Darab} db");
            }
        }

        public void EstiRendelesek()
        {
            Console.WriteLine("\n5. feladat - 18 és 19 óra közötti rendelések");
            var eredmeny = lista
                .Where(r => r.Ido >= new TimeSpan(18, 0, 0)
                         && r.Ido <= new TimeSpan(19, 0, 0))
                .GroupBy(r => new { r.Pizza.Nev, r.Pizza.Meret })
                .Select(g => new {
                    g.Key.Nev,
                    g.Key.Meret,
                    Darab = g.Sum(r => r.Db)
                })
                .OrderBy(x => x.Nev)
                .ThenBy(x => x.Meret);

            foreach (var item in eredmeny)
                Console.WriteLine($"{item.Nev}, {item.Meret} cm - {item.Darab} db");
        }

        public void JolfogyottPizzak()
        {
            Console.WriteLine("\n6. feladat - Mindhárom méretben rendelt pizzák");
            var eredmeny = lista
                .Select(r => r.Pizza)
                .GroupBy(p => p.Nev)
                .Where(g => g.Select(p => p.Meret).Distinct().Count() == 3)
                .Select(g => g.Key)
                .OrderBy(nev => nev);

            foreach (var nev in eredmeny)
                Console.WriteLine(nev);
        }

        public void ReklamRendelesek()
        {
            Console.WriteLine("\n7. feladat - Ajándék üdítőital");
            var eredmeny = lista
                .GroupBy(r => new { r.Ido, r.Cim.Id })
                .Where(g => g.Sum(r => r.Db) > 3)
                .Select(g => g.First())
                .OrderBy(r => r.Ido)
                .ThenBy(r => r.Cim.Nev);

            foreach (var r in eredmeny)
                Console.WriteLine($"{r.Ido:hh\\:mm\\:ss} | {r.Cim.Nev} | {r.Cim.Utca} | {r.Cim.Hsz}");
        }

        public void FizetesiJegyzek()
        {
            Console.WriteLine("\n8. feladat - Fizetési jegyzék");
            Console.WriteLine($"{"Szállítási idő",-16} {"Név",-22} {"Utca",-22} {"Házszám",-10} {"Összeg (Ft)",12}");

            var eredmeny = lista
                .GroupBy(r => new { r.Ido, r.Cim.Id })
                .Select(g => new {
                    Ido = g.Key.Ido,
                    Cim = g.First().Cim,
                    Osszeg = g.Sum(r => r.Pizza.Ar * r.Db)
                })
                .OrderBy(x => x.Ido)
                .ThenBy(x => x.Cim.Nev);

            foreach (var item in eredmeny)
            {
                Console.WriteLine($"{item.Ido,-16:hh\\:mm\\:ss} {item.Cim.Nev,-22} {item.Cim.Utca,-22} {item.Cim.Hsz,-10} {item.Osszeg,12}");
            }
        }

        public void Futtat()
        {
            if (lista.Count == 0)
            {
                Console.WriteLine("Nem sikerült rendeléseket betölteni.");
                return;
            }

            OlcsoPizzak();
            LegnepszerubbPizza();
            EstiRendelesek();
            JolfogyottPizzak();
            ReklamRendelesek();
            FizetesiJegyzek();
        }
    }
}
