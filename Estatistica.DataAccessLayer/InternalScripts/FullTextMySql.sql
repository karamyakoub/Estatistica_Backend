ALTER TABLE estatistica.produtos
ADD FULLTEXT(descricao);


SELECT 
  descricao,
  MATCH(descricao) AGAINST ('TOM. EMB. 2P+T 32A 380 VM') AS score
FROM 
  estatistica.produtos
WHERE 
  MATCH(descricao) AGAINST ('TOM. EMB. 2P+T 32A 380 VM')
ORDER BY 
  score DESC
LIMIT 10;
