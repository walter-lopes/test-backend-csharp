Easynvest.Infohub.Parse

A API Infohub Parse, é responsável pelo cadastro e consulta de DE-PARA
utilizado em nossas integrações com a CETIP.

Hoje utilizamos a persistência apenas no ORACLE, e estava atendendo muito bem.

Porem com o aumento de acesso a nossa API, precisamos tornar a leitura mais rápida

Nosso time teve a ideia de separar a leitura da escrita, e escolhemos o banco de dados REDIS para leitura

Agora precisamos implementar essa leitura nessa nova base.

Teste

O fluxo para gravação da classe Issuer não esta completa, implemente a gravação dos dados no REDIS.

Recomendamos o uso da biblioteca "StackExchange.Redis" - https://stackexchange.github.io/StackExchange.Redis/Basics

Para podermos ler a informação do REDIS vamos precisar salvar no mesmo, utilize um padrão simples de KEY como IssuerParse:{IdCustodyManagerBond}

Qualquer duvida conte com a gente para ajudar com o problema, afinal ninguém trabalha sozinho.

Boa sorte
