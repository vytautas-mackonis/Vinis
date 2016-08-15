using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Vinis.Uzklausos.Controllers
{
    [Route("[controller]")]
    public class UzklausosController : Controller
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (
                var conn =
                    new MySql.Data.MySqlClient.MySqlConnection(
                        "server=192.168.99.100;uid=root;pwd=abc123;database=vinis;"))
            {
                conn.Open();

                using (MySqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = @"SELECT t.tipas,t.iniciatorius,t.gavejas,t.skyrius,t.kategorija,t.vykdytojas,t.ivykdyti_iki,t.prioritetas,t.pavadinimas,t.tekstas, t.statusas
                                            FROM vinis.Uzklausos t
                                            where t.id = ?idas";
                    command.Parameters.AddWithValue("?idas", id);

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        var uzklausa = new Uzklausa();
                        uzklausa.Id = id;
                        uzklausa.Tipas = (Tipas)Enum.Parse(typeof(Tipas), reader.GetString(0));
                        uzklausa.Iniciatorius = reader.GetInt32(1);
                        uzklausa.Gavejas = reader.GetInt32(2);
                        uzklausa.Skyrius = reader.GetInt32(3);
                        uzklausa.Kategorija = reader.GetInt32(4);
                        uzklausa.Vykdytojas = reader.GetInt32(5);
                        uzklausa.IvykdytiIki = reader.GetDateTime(6);
                        uzklausa.Prioritetas = reader.GetString(7);
                        uzklausa.Pavadinimas = reader.GetString(8);
                        uzklausa.Tekstas = reader.GetString(9);
                        uzklausa.Statusas = reader.GetString(10);
                        return Ok(uzklausa);
                    }
                    else return NotFound();
                }
            }
          

           
        }

        [HttpGet]
        public List<Uzklausa> GetAll([FromQuery] UzklausosFiltras filtras)
        {
            using (
                var conn =
                    new MySql.Data.MySqlClient.MySqlConnection(
                        "server=192.168.99.100;uid=root;pwd=abc123;database=vinis;"))
            {
                conn.Open();

                using (MySqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = @"SELECT t.tipas,t.iniciatorius,t.gavejas,t.skyrius,t.kategorija,t.vykdytojas,t.ivykdyti_iki,t.prioritetas,t.pavadinimas,t.tekstas,t.id, t.statusas
                                            FROM vinis.Uzklausos t
                                            where (t.tipas = ?tipas or ?tipas is null)
                                            and (t.iniciatorius = ?iniciatorius or ?iniciatorius is null)
                                            and (t.gavejas = ?gavejas or ?gavejas is null)
                                            and (t.skyrius = ?skyrius or ?skyrius is null)
                                            and (t.kategorija = ?kategorija or ?kategorija is null)
                                            and (t.vykdytojas = ?vykdytojas or ?vykdytojas is null)
                                            and (t.prioritetas = ?prioritetas or ?prioritetas is null)
                                            and (t.pavadinimas = ?pavadinimas or ?pavadinimas is null)
                                            and (t.tekstas like concat('%',?tekstas,'%') or ?tekstas is null)
                                            and (t.statusas = ?statusas or ?statusas is null)
                                           ";
                    command.Parameters.AddWithValue("?tipas", filtras.Tipas.ToString());
                    command.Parameters.AddWithValue("?iniciatorius", filtras.Iniciatorius);
                    command.Parameters.AddWithValue("?gavejas", filtras.Gavejas);
                    command.Parameters.AddWithValue("?skyrius", filtras.Skyrius);
                    command.Parameters.AddWithValue("?kategorija", filtras.Kategorija);
                    command.Parameters.AddWithValue("?vykdytojas", filtras.Vykdytojas);
                    command.Parameters.AddWithValue("?prioritetas", filtras.Prioritetas);
                    command.Parameters.AddWithValue("?pavadinimas", filtras.Pavadinimas);
                    command.Parameters.AddWithValue("?tekstas", filtras.Tekstas);
                    command.Parameters.AddWithValue("?statusas", filtras.Statusas.ToString());

                    MySqlDataReader reader = command.ExecuteReader();
                    var uzklausos = new List<Uzklausa>(); 
                    while (reader.Read())
                    {
                        var uzklausa = new Uzklausa();
                        uzklausa.Tipas = (Tipas)Enum.Parse(typeof(Tipas), reader.GetString(0));
                        uzklausa.Iniciatorius = reader.GetInt32(1);
                        uzklausa.Gavejas = reader.GetInt32(2);
                        uzklausa.Skyrius = reader.GetInt32(3);
                        uzklausa.Kategorija = reader.GetInt32(4);
                        uzklausa.Vykdytojas = reader.GetInt32(5);
                        uzklausa.IvykdytiIki = reader.GetDateTime(6);
                        uzklausa.Prioritetas = reader.GetString(7);
                        uzklausa.Pavadinimas = reader.GetString(8);
                        uzklausa.Tekstas = reader.GetString(9);
                        uzklausa.Id = reader.GetInt32(10);
                        uzklausa.Statusas = reader.GetString(11); ///ciasusipiso
                        uzklausos.Add(uzklausa);
                    }
                    return uzklausos;
                }


            }



        }


        [HttpPost]
        public IActionResult Post([FromBody]UzklausosKurimoRequest value)
        {
            using (
                var conn =
                    new MySql.Data.MySqlClient.MySqlConnection(
                        "server=192.168.99.100;uid=root;pwd=abc123;database=vinis;"))
            {
                conn.Open();

                using (MySqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO `vinis`.`Uzklausos`
                                    (
                                    `tipas`,
                                    `iniciatorius`,
                                    `gavejas`,
                                    `skyrius`,
                                    `kategorija`,
                                    `vykdytojas`,
                                    `ivykdyti_iki`,
                                    `prioritetas`,
                                    `pavadinimas`,
                                    `tekstas`,
                                    `statusas`)
                                    VALUES
                                    (
                                    ?tipas,
                                    ?iniciatorius,
                                    ?gavejas,
                                    ?skyrius,
                                    ?kategorija,
                                    ?vykdytojas,
                                    ?ivykdyti_iki,
                                    ?prioritetas,
                                    ?pavadinimas,
                                    ?tekstas,
                                    ?statusas
                                    );
                                    select last_insert_id();";
                    command.Parameters.AddWithValue("?tipas", value.Tipas.ToString());
                    command.Parameters.AddWithValue("?iniciatorius", value.Iniciatorius);
                    command.Parameters.AddWithValue("?gavejas", value.Gavejas);
                    command.Parameters.AddWithValue("?skyrius", value.Skyrius);
                    command.Parameters.AddWithValue("?kategorija", value.Kategorija);
                    command.Parameters.AddWithValue("?vykdytojas", value.Vykdytojas);
                    command.Parameters.AddWithValue("?ivykdyti_iki", value.IvykdytiIki);
                    command.Parameters.AddWithValue("?prioritetas", value.Prioritetas);
                    command.Parameters.AddWithValue("?pavadinimas", value.Pavadinimas);
                    command.Parameters.AddWithValue("?tekstas", value.Tekstas);
                    command.Parameters.AddWithValue("?statusas", Statusas.Iregistruota.ToString());
                    int id = Convert.ToInt32(command.ExecuteScalar());

                    return CreatedAtAction("Get", new { id = id }, value);
                }
            }
        }


    }

    public class UzklausosKurimoRequest
    {
        public Tipas Tipas { get; set; }
        public int Iniciatorius { get; set; }
        public int Gavejas { get; set; }
        public int Skyrius { get; set; }
        public int Kategorija { get; set; }
        public int Vykdytojas { get; set; }
        public DateTime IvykdytiIki { get; set; }
        public string Prioritetas { get; set; }
        public string Pavadinimas { get; set; }
        public string Tekstas { get; set; }
    }

    public class Uzklausa
    {
        public int Id { get; set; }
        public Tipas Tipas { get; set; }
        public int Iniciatorius { get; set; }
        public int Gavejas { get; set; }
        public int Skyrius { get; set; }
        public int Kategorija { get; set; }
        public int Vykdytojas { get; set; }
        public DateTime IvykdytiIki { get; set; }
        public string Prioritetas { get; set; }
        public string Pavadinimas { get; set; }
        public string Tekstas { get; set; }
        public string Statusas { get; set; }
    }

    public class UzklausosFiltras
    {
        public Tipas Tipas { get; set; }
        public int? Iniciatorius { get; set; }
        public int? Gavejas { get; set; }
        public int? Skyrius { get; set; }
        public int? Kategorija { get; set; }
        public int? Vykdytojas { get; set; }
        public string Prioritetas { get; set; }
        public string Pavadinimas { get; set; }
        public string Tekstas { get; set; }
        public Statusas Statusas { get; set; }
    }

    public enum Statusas
    {
        Iregistruota,
        Patikslinta,
        Vykdoma,
        Uzdaryta,
        Atstatyta
    }

    public enum Tipas
    {
        Telefonu,
        ElPastu,
        Gyvai,
        Vidine
    }

}
