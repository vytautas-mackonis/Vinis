using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Vinis.Asmenys.Controllers
{
    [Route("[controller]")]
    public class AsmenysController : Controller
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (
                var conn =
                    new MySql.Data.MySqlClient.MySqlConnection(
                        "server=192.168.99.100;uid=root;pwd=abc123;database=asmenys;"))
            {
                conn.Open();

                using (MySqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = @"SELECT  t.sistema_id, t.vardas_pavarde_pavadinimas, t.asmens_kodas, t.istaiga, t.telefonas, t.el_pastas, t.papildoma_informacija, t.sukurimo_data, t.atnaujinimo_data
                                            FROM asmenys t
                                            where t.id = ?idas;";
                    command.Parameters.AddWithValue("?idas", id);

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        var asmuo = new Asmuo();
                        asmuo.Id = id;
                         if (!reader.IsDBNull(0))
                        {
                            asmuo.SistemaId = reader.GetInt32(0);
                        }
                        asmuo.VardasPavardePavadinimas = reader.GetString(1);
                        asmuo.AsmensKodas = reader.GetInt64(2);
                        asmuo.Istaiga = reader.GetString(3);
                        asmuo.Telefonas = reader.GetString(4);
                        asmuo.ElPastas = reader.GetString(5);
                        asmuo.PapildomaInformacija = reader.GetString(6);
                        asmuo.SukurimoData = reader.GetDateTime(7);
                        asmuo.AtnaujinimoData = reader.GetDateTime(8);

                        return Ok(asmuo);
                    }
                    else return NotFound();
                }
            }



        }

        [HttpGet]
        public List<Asmuo> GetAll([FromQuery] AsmensFiltras filtras)
        {
            using (
                var conn =
                    new MySql.Data.MySqlClient.MySqlConnection(
                        "server=192.168.99.100;uid=root;pwd=abc123;database=asmenys;"))
            {
                conn.Open();

                using (MySqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = @"SELECT t.sistema_id, t.vardas_pavarde_pavadinimas, t.asmens_kodas, t.istaiga, t.telefonas, t.el_pastas, t.papildoma_informacija, t.sukurimo_data, t.atnaujinimo_data,  t.id
                                            FROM asmenys t
                                            where (t.vardas_pavarde_pavadinimas like concat('%',?vardas_pavarde_pavadinimas,'%') or ?vardas_pavarde_pavadinimas is null)
                                            and (t.asmens_kodas = ?asmens_kodas or ?asmens_kodas is null)
                                            and (t.istaiga like concat('%',?istaiga,'%') or ?istaiga is null)
                                            and (t.telefonas like concat('%',?telefonas,'%') or ?telefonas is null)
                                            and (t.el_pastas like concat('%',?el_pastas,'%') or ?el_pastas is null)
                                            and (t.papildoma_informacija like concat('%',?papildoma_informacija,'%') or ?papildoma_informacija is null)
                                           ";
                    command.Parameters.AddWithValue("?vardas_pavarde_pavadinimas", filtras.VardasPavardePavadinimas);
                    command.Parameters.AddWithValue("?asmens_kodas", filtras.AsmensKodas);
                    command.Parameters.AddWithValue("?istaiga", filtras.Istaiga);
                    command.Parameters.AddWithValue("?telefonas", filtras.Telefonas);
                    command.Parameters.AddWithValue("?el_pastas", filtras.ElPastas);
                    command.Parameters.AddWithValue("?papildoma_informacija", filtras.PapildomaInformacija);


                    MySqlDataReader reader = command.ExecuteReader();
                    var asmenys = new List<Asmuo>();
                    while (reader.Read())
                    {
                        var asmuo = new Asmuo();
                        if (!reader.IsDBNull(0))
                        {
                            asmuo.SistemaId = reader.GetInt32(0);
                        }
                        asmuo.VardasPavardePavadinimas = reader.GetString(1);
                        asmuo.AsmensKodas = reader.GetInt64(2);
                        asmuo.Istaiga = reader.GetString(3);
                        asmuo.Telefonas = reader.GetString(4);
                        asmuo.ElPastas = reader.GetString(5);
                        asmuo.PapildomaInformacija = reader.GetString(6);
                        asmuo.SukurimoData = reader.GetDateTime(7);
                        asmuo.AtnaujinimoData = reader.GetDateTime(8);
                        asmuo.Id = reader.GetInt32(9);
                        asmenys.Add(asmuo);
                    }
                    return asmenys;
                }


            }



        }

        [HttpPost]
        public IActionResult Post([FromBody] AsmensKurimoRequest value)
        {
            using (
                var conn =
                    new MySql.Data.MySqlClient.MySqlConnection(
                        "server=192.168.99.100;uid=root;pwd=abc123;database=asmenys;"))
            {
                conn.Open();

                using (MySqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO `asmenys`
                                            (
                                            `vardas_pavarde_pavadinimas`,
                                            `asmens_kodas`,
                                            `istaiga`,
                                            `telefonas`,
                                            `el_pastas`,
                                            `papildoma_informacija`,
                                            `sukurimo_data`,
                                            `atnaujinimo_data`)
                                            VALUES
                                            (
                                            ?vardas_pavarde_pavadinimas,
                                            ?asmens_kodas,
                                            ?istaiga,
                                            ?telefonas,
                                            ?el_pastas,
                                            ?papildoma_informacija,
                                            ?sukurimo_data,
                                            ?atnaujinimo_data);
                                    select last_insert_id();";
                    command.Parameters.AddWithValue("?vardas_pavarde_pavadinimas", value.VardasPavardePavadinimas);
                    command.Parameters.AddWithValue("?asmens_kodas", value.AsmensKodas);
                    command.Parameters.AddWithValue("?istaiga", value.Istaiga);
                    command.Parameters.AddWithValue("?telefonas", value.Telefonas);
                    command.Parameters.AddWithValue("?el_pastas", value.ElPastas);
                    command.Parameters.AddWithValue("?papildoma_informacija", value.PapildomaInformacija);
                    command.Parameters.AddWithValue("?sukurimo_data", DateTime.UtcNow);
                    command.Parameters.AddWithValue("?atnaujinimo_data", DateTime.UtcNow);
                    int id = Convert.ToInt32(command.ExecuteScalar());

                    return CreatedAtAction("Get", new { id = id }, value);
                }
            }
        }


    }

    public class AsmensFiltras
    {
        public string VardasPavardePavadinimas { get; set; }
        public long? AsmensKodas { get; set; }
        public string Istaiga { get; set; }
        public string Telefonas { get; set; }
        public string ElPastas { get; set; }
        public string PapildomaInformacija { get; set; }
    }


    public class AsmensKurimoRequest
    {
        public string VardasPavardePavadinimas { get; set; }
        public long AsmensKodas { get; set; }
        public string Istaiga { get; set; }
        public string Telefonas { get; set; }
        public string ElPastas { get; set; }
        public string PapildomaInformacija { get; set; }
    }

    public class Asmuo
    {
        public int Id { get; set; }
        public int? SistemaId { get; set; }
        public string VardasPavardePavadinimas { get; set; }
        public long AsmensKodas { get; set; }
        public string Istaiga { get; set; }
        public string Telefonas { get; set; }
        public string ElPastas { get; set; }
        public string PapildomaInformacija { get; set; }
        public DateTime SukurimoData { get; set; }
        public DateTime AtnaujinimoData { get; set; }

    }
}
