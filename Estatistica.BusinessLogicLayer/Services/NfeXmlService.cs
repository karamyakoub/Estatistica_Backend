using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.BusinessLogicLayer.Utils;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Estatistica.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Estatistica.BusinessLogicLayer.Services
{
    public class NfeXmlService : INfeXmlService
    {
        private readonly IConcorrenteProdutoRepository concorrenteProdutoRepository;
        private readonly IConcorrenteFilialRepository concorrenteFilialRepository;
        private readonly INfcRespository nfcRespository;
        private readonly INfiRespository nfiRespository;
        private readonly IProdutoRepository produtoRepository;
        private readonly IMapper mapper;
        private readonly IUsuarioRepository usuarioRepository;
        const string chaveNfe = "http://www.portalfiscal.inf.br/nfe";

        public NfeXmlService(IConcorrenteProdutoRepository concorrenteProdutoRepository,
            IConcorrenteFilialRepository concorrenteFilialRepository,
            INfcRespository nfcRespository,
            INfiRespository nfiRespository,
            IProdutoRepository produtoRepository,
            IMapper mapper,
            IUsuarioRepository usuarioRepository)
        {
            this.concorrenteProdutoRepository=concorrenteProdutoRepository;
            this.concorrenteFilialRepository=concorrenteFilialRepository;
            this.nfcRespository=nfcRespository;
            this.nfiRespository=nfiRespository;
            this.produtoRepository=produtoRepository;
            this.mapper=mapper;
            this.usuarioRepository=usuarioRepository;
        }
        public async Task<bool> ImportNfeXml(string xmlContent)
        {
            var doc = XmlUtils.LoadFromContent(xmlContent);
            if (doc is null || doc.DocumentElement is null)
                throw new ArgumentException("Conteúdo XML inválido.");
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");
            XmlNode? cnpjNode = doc.SelectSingleNode("//nfe:emit/nfe:CNPJ", ns);
            if (cnpjNode is null)
                throw new Exception("CNPJ do emitente não encontrado no XML.");
            string cnpj = cnpjNode.InnerText;
            var concorrenteFilial = await concorrenteFilialRepository.GetConcorrenteFilialByCnpj(cnpj);
            if (concorrenteFilial is null)
                throw new ArgumentNullException("Concorrente filial nulo");
            var concorrente = concorrenteFilial.Concorrente;
            //Produtos
            var concorrenteProdutos = await getConcorrenteProdutosFromXml(doc, concorrente);
            await concorrenteProdutoRepository.AddConcorrenteProdutoRange(concorrenteProdutos);

            //Nfc
            var nfc = await GetNfcFromXml(doc, concorrenteFilial);
            if (nfc is not null)
                await nfcRespository.AddNfc(nfc);
            //Nfi
            var nfis = await GetItensNotaFromXml(doc, concorrente, concorrenteProdutos);
            await nfiRespository.AddNfiRange(nfis);
            return true;
        }

        public async Task<dynamic> CheckFilialPendente(string xmlContent)
        {
            var doc = XmlUtils.LoadFromContent(xmlContent);
            if (doc is null || doc.DocumentElement is null)
                throw new ArgumentException("Conteúdo XML inválido.");
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");

            // Caminho XPath até o CNPJ do emitente
            XmlNode? cnpjNode = doc.SelectSingleNode("//nfe:emit/nfe:CNPJ", ns);
            XmlNode? nomeNode = doc.SelectSingleNode("//nfe:emit/nfe:xNome", ns);

            if (cnpjNode is null || nomeNode is null)
                throw new Exception("CNPJ do emitente não encontrado no XML.");
            string cnpj = cnpjNode.InnerText;

            var filial = await concorrenteFilialRepository.GetConcorrenteFilialByCnpj(cnpj);
            string nome = nomeNode.InnerText;
            return filial is null ? new { Cnpj = cnpj, Nome = nome } : 0;
        }

        public bool IsValidNfe(string xmlContent)
        {
            var doc = XmlUtils.LoadFromContent(xmlContent);
            if (doc == null || doc.DocumentElement == null)
                return false;
            return true;
        }


        private async Task<List<ConcorrenteProduto>> getConcorrenteProdutosFromXml(XmlDocument doc, Concorrente concorrente)
        {
            List<ConcorrenteProduto> concorrenteProdutos = new List<ConcorrenteProduto>();
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("nfe", chaveNfe);
            XmlNodeList? detNodes = doc.SelectNodes("//nfe:det", ns);
            if (detNodes is null)
                throw new ArgumentNullException("Nao foi possivel identificar detNodes");
            foreach (XmlNode det in detNodes)
            {
                var prodNode = det.SelectSingleNode("nfe:prod", ns);
                if (prodNode == null) continue;

                var codigo = prodNode.SelectSingleNode("nfe:cProd", ns)?.InnerText ?? "";
                var descricao = prodNode.SelectSingleNode("nfe:xProd", ns)?.InnerText ?? "";
                var unidade = prodNode.SelectSingleNode("nfe:uCom", ns)?.InnerText;
                var codBarra = prodNode.SelectSingleNode("nfe:cEAN", ns)?.InnerText;

                var concorrenteProduto = new ConcorrenteProduto
                {
                    Id = $"{concorrente.Id}{codigo}",
                    Concorrente = concorrente,
                    CodigoProdutoConcorrente = codigo,
                    DescricaoProdutoConcorrente = descricao,
                    UnidadeProdutoConcorrente = string.IsNullOrWhiteSpace(unidade) ? null : unidade,
                    CodigoBarraConcorrente = string.IsNullOrWhiteSpace(codBarra) ? null : codBarra,
                    Produto = null, // optionally match with your internal product
                    Planilha = null,
                    TipoVinculo = null // or set if needed
                };

                //Check if not exists

                var concorrenteProdutoDb = await concorrenteProdutoRepository.GetConcorrenteProdutosByProdutoId(concorrenteProduto.Id);

                if (concorrenteProdutoDb is null)
                    concorrenteProdutos.Add(concorrenteProduto);
            }



            //Check for product link

            foreach (var concorrenteProduto in concorrenteProdutos)
            {
                if (!string.IsNullOrWhiteSpace(concorrenteProduto.CodigoProdutoConcorrente))
                {
                    var produto = await produtoRepository.GetProductByCodigoBarra(concorrenteProduto.CodigoProdutoConcorrente);
                    concorrenteProduto.Produto = produto;
                    if (produto is not null)
                        concorrenteProduto.TipoVinculo = "CB";
                }
            }

            return concorrenteProdutos;
        }
        private async Task<Nfc?> GetNfcFromXml(XmlDocument doc, ConcorrenteFilial concorrenteFilial)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("nfe", chaveNfe);

            var infNFeNode = doc.SelectSingleNode("//nfe:infNFe", ns);
            if (infNFeNode == null)
                throw new Exception("infNFe node não encontrado.");

            string? chave = infNFeNode.Attributes["Id"]?.InnerText.Replace("NFe", "");

            if (string.IsNullOrWhiteSpace(chave))
                throw new Exception("Chave NFe inválida.");

            var cnpjEmitente = infNFeNode.SelectSingleNode("nfe:emit/nfe:CNPJ", ns)?.InnerText;
            if (string.IsNullOrWhiteSpace(cnpjEmitente))
                throw new Exception("CNPJ do emitente não encontrado.");

            var cnpjCliente = infNFeNode.SelectSingleNode("nfe:dest/nfe:CNPJ", ns)?.InnerText;
            var nomeCliente = infNFeNode.SelectSingleNode("nfe:dest/nfe:xNome", ns)?.InnerText;

            var dhEmi = infNFeNode.SelectSingleNode("nfe:ide/nfe:dhEmi", ns)?.InnerText ??
                        infNFeNode.SelectSingleNode("nfe:ide/nfe:dEmi", ns)?.InnerText;

            DateTime? dataEmissao = null;
            if (!string.IsNullOrWhiteSpace(dhEmi))
            {
                DateTime parsed;
                if (DateTime.TryParse(dhEmi, out parsed))
                {
                    dataEmissao = parsed;
                }
                else if (DateTime.TryParseExact(dhEmi, "yyyy-MM-ddTHH:mm:sszzz", null, System.Globalization.DateTimeStyles.None, out parsed))
                {
                    dataEmissao = parsed;
                }
            }

            var nfc = new Nfc
            {
                ChaveNfe = chave,
                ConcorrenteCnpj = concorrenteFilial,
                DataEmissao = dataEmissao,
                CnpjCliente = cnpjCliente,
                NomeCliente = nomeCliente,
                Planilha = null
            };

            //Check if exists
            var nfcDb = (await nfcRespository.GetNfcsByCondition(x => x.ChaveNfe == nfc.ChaveNfe)).FirstOrDefault();

            return nfcDb is null ? nfc : null;
        }


        private async Task<List<Nfi>> GetItensNotaFromXml(XmlDocument doc, Concorrente concorrente, List<ConcorrenteProduto> concorrenteProdutos)
        {
            var itensNota = new List<Nfi>();
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("nfe", chaveNfe);

            XmlNodeList? detNodes = doc.SelectNodes("//nfe:det", ns);
            if (detNodes is null)
                throw new Exception("Itens da nota (detNodes) não encontrados.");

            // Obter UF origem e destino da nota
            string? ufOrigem = doc.SelectSingleNode("//nfe:emit/nfe:enderEmit/nfe:UF", ns)?.InnerText;
            string? ufDestino = doc.SelectSingleNode("//nfe:dest/nfe:enderDest/nfe:UF", ns)?.InnerText;
            string? codMunicipioCliente = doc.SelectSingleNode("//nfe:dest/nfe:enderDest/nfe:cMun", ns)?.InnerText;


            var infNFeNode = doc.SelectSingleNode("//nfe:infNFe", ns);
            if (infNFeNode == null)
                throw new Exception("infNFe node não encontrado.");
            string? chave = infNFeNode.Attributes?["Id"]?.InnerText.Replace("NFe", "");

            var nfc = (await nfcRespository.GetNfcsByCondition(x => x.ChaveNfe == chave)).FirstOrDefault();
            if (nfc is null)
                throw new Exception("Chave da nota nao encontrado");

            foreach (XmlNode det in detNodes)
            {
                var prodNode = det.SelectSingleNode("nfe:prod", ns);
                if (prodNode == null) continue;

                var codigo = prodNode.SelectSingleNode("nfe:cProd", ns)?.InnerText ?? "";
                var unidade = prodNode.SelectSingleNode("nfe:uCom", ns)?.InnerText;
                var codBarra = prodNode.SelectSingleNode("nfe:cEAN", ns)?.InnerText;
                var qtdeStr = prodNode.SelectSingleNode("nfe:qCom", ns)?.InnerText;
                var valorStr = prodNode.SelectSingleNode("nfe:vUnCom", ns)?.InnerText;

                int qtde = int.TryParse(qtdeStr?.Split('.')[0], out var q) ? q : 0;
                decimal valor = decimal.TryParse(valorStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : 0;
                var id = $"{concorrente.Id}{codigo}";
                // Buscar produto correspondente
                var concorrenteProduto = await concorrenteProdutoRepository.GetConcorrenteProdutosByProdutoId(id);

                if (concorrenteProduto == null)
                {
                    // Ignora itens não presentes na lista pré-validada
                    continue;
                }

                var nfi = new Nfi
                {
                    Id = $"{nfc.ChaveNfe}{codigo}",
                    ConcorrenteProduto = concorrenteProduto,
                    Nfc = nfc,
                    Qtde = qtde,
                    Valor = valor,
                    UfOrigin = ufOrigem,
                    UfDestino = ufDestino,
                    CodMunicipio = codMunicipioCliente,
                    CodigoBarra = string.IsNullOrWhiteSpace(codBarra) ? null : codBarra,
                    Unidade = string.IsNullOrWhiteSpace(unidade) ? null : unidade
                };
                //check if not exists 
                var nfiDb = (await nfiRespository.GetNfisByCondition(x => x.Id == nfi.Id)).FirstOrDefault();
                if (nfiDb is null)
                    itensNota.Add(nfi);
            }

            return itensNota;
        }

        public async Task<IEnumerable<NfcXmlGetResponse>> GetNfcXmlByPeriod(DateTime startDate, DateTime endDate)
        {
            var xmls = mapper.Map<IEnumerable<NfcXmlGetResponse>>(await nfcRespository.GetNfcsByConditionNoTracking(x => x.Planilha == null
            && (x.DataCadastro.HasValue && x.DataCadastro.Value.Date >= startDate && x.DataCadastro.Value.Date <= endDate)));

            foreach(var xml in xmls)
            {
                if (!string.IsNullOrWhiteSpace(xml.IdUsuario))
                {
                    xml.Usuario = usuarioRepository.GetUsuarioById(xml.IdUsuario).Result?.Email;
                }
            }
            return xmls;
        }

    }
}
