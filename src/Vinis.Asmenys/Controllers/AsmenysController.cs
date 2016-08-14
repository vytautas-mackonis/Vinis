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
            return null;



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

    public class AsmensKurimoRequest
    {
        public string VardasPavardePavadinimas { get; set; }
        public long AsmensKodas { get; set; }
        public string Istaiga { get; set; }
        public string Telefonas { get; set; }
        public string ElPastas { get; set; }
        public string PapildomaInformacija { get; set; }
    }
}
