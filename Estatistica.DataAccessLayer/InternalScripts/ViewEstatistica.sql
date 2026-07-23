CREATE OR ALTER VIEW
     VW_Estatistica AS
select
     tbl.Id,
     tbl.ConcorrenteId,
     tbl.ConcorrenteNome,
     tbl.ConcorrenteCnpj,
     tbl.ChaveNfe,
     tbl.DataEmissao,
     tbl.CnpjCliente,
     tbl.NomeCliente,
     tbl.ConcorrenteProdutoCodigo,
     tbl.ConcorrenteProdutoDescricao,
     tbl.ConcorrenteProdutoUnidade,
     tbl.Qtde,
     tbl.ConcorrenteProdutoValorOriginal,
     tbl.ConcorrenteProdutoValorCorregido,
     Round(tbl.Qtde * tbl.ConcorrenteProdutoValorCorregido, 2) ConcorrenteValorTotal,
     tbl.HouveCorrecao,
     tbl.CodigoProduto,
     tbl.CodigoFabrica,
     tbl.CodigoBarra,
     tbl.Descricao,
     tbl.Fabricante,
     tbl.Tipo,
     tbl.SubTipo,
     tbl.Linha,
     tbl.Famila,
     tbl.Unidade,
     tbl.Custo,
     Round(tbl.Qtde * tbl.Custo, 2) CustoTotal,
     tbl.Pvenda,
     Round(tbl.Qtde * tbl.Pvenda, 2) PvendaTotal,
     tbl.PercFrete,
     Round(
          coalesce(tbl.PercFrete, 0) / 100 * tbl.Pvenda + tbl.Pvenda,
          2
     ) PvendaComFrete,
     Round(
          tbl.Qtde * (
               coalesce(tbl.PercFrete, 0) / 100 * tbl.Pvenda + tbl.Pvenda
          ),
          2
     ) PvendaTotalComFrete
from
     (
          SELECT
               nfis.id Id, 
               con.ConId ConcorrenteId,
               con.nome ConcorrenteNome,
               confil.cnpj ConcorrenteCnpj,
               nfcs.chaveNfe ChaveNfe,
               nfcs.dtemissao DataEmissao,
               nfcs.cnpjCliente CnpjCliente,
               nfcs.nomeCliente NomeCliente,
               conprod.codProdCon ConcorrenteProdutoCodigo,
               conprod.descProdCon ConcorrenteProdutoDescricao,
               conprod.unProdCon ConcorrenteProdutoUnidade,
               nfis.qtde Qtde,
               nfis.valor ConcorrenteProdutoValorOriginal,
               CASE
                    WHEN nfis.qtdeCorrecao > 0
                    AND nfis.valor >= coalesce(prod.pvenda, 0) THEN round(nfis.valor / nfis.qtdeCorrecao, 2)
                    WHEN nfis.qtdeCorrecao > 0
                    AND coalesce(prod.pvenda, 0) >= nfis.valor THEN round(nfis.valor * nfis.qtdeCorrecao, 2)
                    WHEN nfis.valor > 2 * coalesce(prod.pvenda, 0)
                    AND coalesce(prod.pvenda, 0) > 0 THEN round(
                         ceiling(coalesce(prod.pvenda, 0) / nfis.valor) * nfis.valor,
                         2
                    )
                    WHEN coalesce(prod.pvenda, 0) > 2 * nfis.valor THEN round(
                         ceiling(nfis.valor / coalesce(prod.pvenda, 0)) * nfis.valor,
                         2
                    )
                    ELSE nfis.valor
               END ConcorrenteProdutoValorCorregido,
               CASE
                    WHEN nfis.qtdeCorrecao > 0
                    AND nfis.valor >= coalesce(prod.pvenda, 0) THEN 'MANUAL'
                    WHEN nfis.qtdeCorrecao > 0
                    AND coalesce(prod.pvenda, 0) >= nfis.valor THEN 'MANUAL'
                    WHEN nfis.valor > 2 * coalesce(prod.pvenda, 0)
                    AND coalesce(prod.pvenda, 0) > 0 THEN 'AUTOMATICA'
                    WHEN coalesce(prod.pvenda, 0) > 2 * nfis.valor THEN 'AUTOMATICA'
                    ELSE 'X'
               END HouveCorrecao,
               prod.codprod CodigoProduto,
               prod.codFab CodigoFabrica,
               prod.codBarra CodigoBarra,
               prod.descricao Descricao,
               prod.fabricante Fabricante,
               prod.tipo Tipo,
               prod.subtipo SubTipo,
               prod.linha Linha,
               prod.familia Famila,
               prod.unidade Unidade,
               coalesce(prod.custo, 0) Custo,
               coalesce(prod.pvenda, 0) Pvenda,
               fretes.percFrete PercFrete
          FROM
               nfis
               INNER JOIN nfcs ON nfcs.chaveNfe = nfis.NfcChaveNfe
               INNER JOIN concorrentefilials confil ON confil.cnpj = nfcs.ConcorrenteCnpjCnpj
               INNER JOIN concorrentes con ON con.conId = confil.ConcorrenteId
               INNER JOIN concorrenteprodutos conprod ON conprod.id = nfis.ConcorrenteProdutoId
               LEFT JOIN produtos prod ON prod.codProd = conprod.ProdutoCodigoProduto
               LEFT JOIN fretes ON nfis.codMuni = fretes.CodMuni
     ) as tbl order by tbl.ConcorrenteNome,tbl.ChaveNfe,tbl.DataEmissao,tbl.CodigoProduto;