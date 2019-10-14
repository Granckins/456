using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Model.Db
{
    public static class EventManager
    {
        public static EventCouch ConvertEventWarListToEventCouch(List<EventWar> Events)
        {
            EventCouch EC = new EventCouch();
            var valid = true;
            if (Events[0].Наименование_составной_единицы != "")
                valid = false;
            int i = 0;
            if (valid)
            {
                foreach (var e in Events)
                {
                    if (i == 0)
                    {
                        EC = ConvertEventWarToEventCouchParent(e);
                    }
                    else
                    {
                        if (e.Наименование_составной_единицы != "")
                        {
                            
                        }

                    }

                    i++;
                }
            }

            return EC;
        }
        public static List<string> ToListWitoutSoder(EventCouch EC)
        {
            var list = new List<string>();
            list.Add(EC.archive.ToString());
            list.Add(EC.Nomer_upakovki.ToString());
            list.Add(EC.Naimenovanie_izdeliya == "" ? null : EC.Naimenovanie_izdeliya);
            list.Add(EC.Zavodskoj_nomer == "" ? null : EC.Zavodskoj_nomer);
            list.Add(EC.Kolichestvo.ToString());
            list.Add(EC.Oboznachenie == "" ? null : EC.Oboznachenie);

            list.Add(EC.Sistema == "" ? null : EC.Sistema);
            list.Add(EC.Prinadlezhnost == "" ? null : EC.Prinadlezhnost);
            list.Add(EC.Stoimost.ToString());
            list.Add(EC.Otvetstvennyj == "" ? null : EC.Otvetstvennyj);
            list.Add(EC.Mestonahozhdenie_na_sklade == "" ? null : EC.Mestonahozhdenie_na_sklade);
            list.Add(EC.Ves_brutto.ToString());
            list.Add(EC.Ves_netto.ToString());
            list.Add(EC.Dlina.ToString());
            list.Add(EC.Shirina.ToString());
            list.Add(EC.Vysota.ToString());
            list.Add(EC.Data_priyoma.ToString());
            
                list.Add(EC.Data_priyoma.ToString() );
            
            list.Add(EC.Otkuda == "" ? null : EC.Otkuda);
            
                list.Add(EC.Data_vydachi.ToString() );
 
            list.Add(EC.Kuda == "" ? null : EC.Kuda);
            list.Add(EC.Nomer_plomby == "" ? null : EC.Nomer_plomby);
            list.Add(EC.Primechanie == "" ? null : EC.Primechanie);
            list.Add(EC.Dobavil == "" ? null : EC.Dobavil);

            return list;
        }
        static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        public static EventCouch ConvertEventWarToEventCouchParent(EventWar e)
        {
            EventCouch EC = new EventCouch();
            EC._rev = e._rev;
            EC._id = e._id;
            EC.archive = e.archive;
            EC.Nomer_upakovki = e.Номер_упаковки;
            EC.Naimenovanie_izdeliya = e.Наименование_изделия;
            EC.Zavodskoj_nomer = e.Заводской_номер;
            EC.Kolichestvo = e.Количество;
            EC.Oboznachenie = e.Обозначение; 
            EC.Sistema = e.Система;
            EC.Prinadlezhnost = e.Принадлежность;
            EC.Stoimost = e.Стоимость;
            EC.Otvetstvennyj = e.Ответственный;
            EC.Mestonahozhdenie_na_sklade = e.Местонахождение_на_складе;
            EC.Ves_brutto = e.Вес_брутто;
            EC.Ves_netto = e.Вес_нетто;
            EC.Dlina = e.Длина;
            EC.Shirina = e.Ширина;
            EC.Vysota = e.Высота;
            EC.Data_priyoma = ConvertToUnixTimestamp(e.Дата_приёма);
            EC.Otkuda = e.Откуда;
            EC.Data_vydachi = ConvertToUnixTimestamp(e.Дата_выдачи);
            EC.Kuda = e.Куда;
            EC.Nomer_plomby = e.Номер_пломбы;
            EC.Primechanie = e.Примечание;
            EC.Dobavil = e.Добавил;
            EC.Data_ismenen = ConvertToUnixTimestamp(e.Дата_изменения);
            return EC;
        }
        public static EventCouchFull ConvertEventWarToEventCouchFullParent(EventWar e)
        {
            EventCouchFull EC = new EventCouchFull();
            EC._rev = e._rev;
            EC._id = e._id;
            EC.archive = e.archive;
            EC.Nomer_upakovki = e.Номер_упаковки;
            EC.Naimenovanie_izdeliya = e.Наименование_изделия;
            EC.Zavodskoj_nomer = e.Заводской_номер;
            EC.Kolichestvo = e.Количество;
            EC.Oboznachenie = e.Обозначение; 
            EC.Sistema = e.Система;
            EC.Prinadlezhnost = e.Принадлежность;
            EC.Stoimost = e.Стоимость;
            EC.Otvetstvennyj = e.Ответственный;
            EC.Mestonahozhdenie_na_sklade = e.Местонахождение_на_складе;
            EC.Ves_brutto = e.Вес_брутто;
            EC.Ves_netto = e.Вес_нетто;
            EC.Dlina = e.Длина;
            EC.Shirina = e.Ширина;
            EC.Vysota = e.Высота;
            EC.Data_priyoma = ConvertToUnixTimestamp(e.Дата_приёма);
            EC.Otkuda = e.Откуда;
            EC.Data_vydachi = ConvertToUnixTimestamp(e.Дата_выдачи);
            EC.Kuda = e.Куда;
            EC.Nomer_plomby = e.Номер_пломбы;
            EC.Primechanie = e.Примечание;
            EC.Dobavil = e.Добавил;
            EC.Data_ismenen = ConvertToUnixTimestamp(e.Дата_изменения);
            return EC;
        }
        
        public static EventCouch ConvertEventCouchFullToEventCouch(EventCouchFull e)
        {
            EventCouch EC = new EventCouch();
            EC._rev = e._rev;
            EC._id = e._id;
            EC.archive = e.archive;
            EC.Nomer_upakovki = e.Nomer_upakovki;
            EC.Naimenovanie_izdeliya = e.Naimenovanie_izdeliya;
            EC.Zavodskoj_nomer = e.Zavodskoj_nomer;
            EC.Kolichestvo = e.Kolichestvo;
            EC.Oboznachenie = e.Oboznachenie; 
            EC.Sistema = e.Sistema;
            EC.Prinadlezhnost = e.Prinadlezhnost;
            EC.Stoimost = e.Stoimost;
            EC.Otvetstvennyj = e.Otvetstvennyj;
            EC.Mestonahozhdenie_na_sklade = e.Mestonahozhdenie_na_sklade;
            EC.Ves_brutto = e.Ves_brutto;
            EC.Ves_netto = e.Ves_netto;
            EC.Dlina = e.Dlina;
            EC.Shirina = e.Shirina;
            EC.Vysota = e.Vysota;
            EC.Data_priyoma = e.Data_priyoma;
            EC.Otkuda = e.Otkuda;
            EC.Data_vydachi = e.Data_vydachi;
            EC.Kuda = e.Kuda;
            EC.Nomer_plomby = e.Nomer_plomby;
            EC.Primechanie = e.Primechanie;
            EC.Dobavil = e.Dobavil;
            EC.Data_ismenen = e.Data_ismenen;
            return EC;
        }

         
    }
}
