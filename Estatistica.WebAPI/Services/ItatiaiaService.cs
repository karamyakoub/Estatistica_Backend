
using Estatistica.BusinessLogicLayer.ServiceContracts;
using ExcelDataReader;
using Npgsql;
using System.Data;
using System.IO;
using System.Text;

namespace Estatistica.WebAPI.Services
{
    public class ItatiaiaService : BackgroundService
    {
        private readonly string connString;
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory scopeFactory;

        public ItatiaiaService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            connString = configuration.GetConnectionString("itatiaia") ?? string.Empty;
            this.configuration=configuration;
            this.scopeFactory=scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"Serviço Itatiaia Iniciado, {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");
            while (true)
            {
                await Task.Delay(1 * 60 * 60 * 1000);
                try
                {
                    //Get Product Prices from Itatiaia
                    var dtProductsPrices = readProductsPriceItatiaia();
                    if(dtProductsPrices is null)
                        throw new Exception("Erro ao ler os preços dos produtos da Itatiaia.");
                    await saveProductsPrice(dtProductsPrices!);



                    //Read the municipality-sector mapping table
                    var dtMuniSetor = readTableMuniSetor();
                    var dtSetorFrete = getSetorFrete();
                    var dtMunicipios = getMunicipios();

                    if (dtMuniSetor is null || dtSetorFrete is null || dtMunicipios is null)
                        throw new Exception("Erro ao ler as tabelas para atualizar os fretes da Itatiaia.");


                    var dtJoin = from muniSetor in dtMuniSetor.AsEnumerable()
                                  join frete in dtSetorFrete.AsEnumerable()
                                  on Convert.ToString(muniSetor["setocodi"]) equals Convert.ToString(frete["codsetor"])
                                  join muni in dtMunicipios.AsEnumerable()
                                  on Convert.ToString(muniSetor["municpri"]) equals Convert.ToString(muni["codmuni"])
                                  select new
                                  {
                                      CodSetor = Convert.ToString(muniSetor["setocodi"]),
                                      CodMuni = Convert.ToString(muni["codmuni"]),
                                      CodMuniIbeg = Convert.ToString(muni["codmuniibeg"]),
                                      PercFrete = Convert.ToString(frete["percfrete"])
                                  };                    
                    if(dtJoin?.Count() > 0)
                        await saveFretes(dtJoin);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro na execução do serviço Itatiaia\nErro:{ex.Message}");
                }
                await Task.Delay(23 * 60 * 60 * 1000);
            }
        }

        private async Task saveFretes(dynamic dtJoin)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var freteService = scope.ServiceProvider.GetRequiredService<IFreteService>();
                foreach (var row in dtJoin)
                {
                    try
                    {
                        decimal.TryParse(row.PercFrete, out decimal percFrete);
                        await freteService.AddUpdateFrete(row.CodSetor, row.CodMuniIbeg, percFrete);
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Method to save product prices into the system.
        /// </summary>
        /// <param name="dtProductsPrices"></param>
        /// <returns></returns>
        private async Task saveProductsPrice(DataTable dtProductsPrices)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var produtoService = scope.ServiceProvider.GetRequiredService<IProdutoService>();
                foreach (DataRow dr in dtProductsPrices.Rows)
                {
                    try
                    {
                        await produtoService.UpdateProdutoPrice(dr["codigo"].ToString() ?? string.Empty,
                            Convert.ToDecimal(dr["pvenda"]),
                            Convert.ToDecimal(dr["custo"]));
                    }
                    catch
                    {

                    }
                }
            }
        }
        /// <summary>
        /// Method to get freight percentages by sector from Itatiaia database.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private DataTable? getSetorFrete()
        {
            var dt = new DataTable();
            try
            {
                if (string.IsNullOrEmpty(connString))
                    throw new ArgumentNullException("ConnString Invalido.");
                var query = @"select
                                    s.setocodi codsetor,
                                    s.setodesc descsetor,
                                    f.pfreperc percfrete
                                from
                                    arqseto s,
                                    arqpfre f
                                where
                                    s.setopfre = f.pfrecpri";
                using (var conn = new NpgsqlConnection(connString))
                {
                    using (var adapter = new NpgsqlDataAdapter(query, conn))
                    {
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro de ler as fretes da Itatiaia.");
                throw;
            }
        }
        /// <summary>
        /// Method to get freight percentages by sector from Itatiaia database.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private DataTable? getMunicipios()
        {
            var dt = new DataTable();
            try
            {
                if (string.IsNullOrEmpty(connString))
                    throw new ArgumentNullException("ConnString Invalido.");
                var query = @"select municpri codmuni,municodi codmuniibeg from arqmuni";
                using (var conn = new NpgsqlConnection(connString))
                {
                    using (var adapter = new NpgsqlDataAdapter(query, conn))
                    {
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro de ler os municipios da Itatiaia.");
                throw;
            }
        }

        /// <summary>
        /// Method to read the Excel file that contains the mapping between municipalities and sectors.
        /// </summary>
        /// <returns></returns>
        private DataTable? readTableMuniSetor()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setor x muni.xlsx");
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var ds = reader.AsDataSet(new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        });
                        if (ds is null || ds.Tables.Count == 0)
                            return null;
                        return ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro de ler a planilha setor x muni");
                throw;
            }
        }

        /// <summary>
        /// Method to read product prices from Itatiaia database.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private DataTable? readProductsPriceItatiaia()
        {
            var dt = new DataTable();
            try
            {
                if (string.IsNullOrEmpty(connString))
                    throw new ArgumentNullException("ConnString Invalido.");
                var query = @"with
                                t1 as (
                                    select
                                        t.tabpprod codigo,
                                        t.tabpprec custo
                                    from
                                        arqtabp t
                                    where
                                        t.tabpcodi = 'C'
                                ),
                                t2 as (
                                    select
                                        t.tabpprod codigo,
                                        t.tabpprec pvenda
                                    from
                                        arqtabp t
                                    where
                                        t.tabpcodi = 'P'
                                )
                            select
                                t1.codigo,
                                t1.custo,
                                t2.pvenda
                            from
                                t1,
                                t2
                            where
                                t1.codigo = t2.codigo order by t1.codigo";
                using (var conn = new NpgsqlConnection(connString))
                {
                    using (var adapter = new NpgsqlDataAdapter(query, conn))
                    {
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro de ler os proços dos produtos da Itatiaia.");
                throw;
            }
        }
    }
}
