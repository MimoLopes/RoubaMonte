# ProjetoFinal
Repositório do Projeto Final da Disciplina de Jogos Multiplayer
    
# Instrucoes
    *Regras
    - Cada jogador joga uma carta no seu turno;
    - Qualquer jogador pode roubar as cartas em qualquer turno;
    - Para as cartas serem roubadas o valor da ultima carta jogada deve corresponder com o valor do contador;
    - Se o jogador ficar sem cartas ele ainda precisa interagir como se fosse jogar uma carta no seu turno;
    - Mesmo sem cartas o jogador ainda pode roubar as cartas;
    - caso todos os jogadores fiquem sem cartas, o ultimo jogador a jogar a carta rouba as cartas;
    -o jogador que rouba todas as cartas ganha;

    *Controles
    - Botao esquerdo do mouse joga uma carta;
    - Botao direito do mouse rouba as cartas;

# Menu Layout
    - Ativacao e desativacao dos botes sao controladas pelos proprios events dos botoes;
    - Cada botao dispara um evento(Controlado pelo MenuManager) para a conecao(Observado pelo GameConnection);
    - O layout inteiro foi organizado por Layout Groups para padronizacao dos tamanhos e posicoes;
    - Tanto as listas de players como de room tem um prefab implementado para cada objeto do seu tipo (player ou room) na Networke possuem um scrool para possibilitar um lista mais extensa e adaptacao para varios tamanhos de tela;
    - os prefabs de player e room possuem scripts (PlayerData e RoomData) para controle dos mesmos;
    - Foi implementada uma area para Debug da conexa na parte inferior do menu(Controlada pelo GameConnection);

# Conexao
    - Todos os controles de conexao sao controlados pelo GameConnection;
    - A conexao ao server e ao lobby e feita automaticamente ao iniciar o jogo ;
    - Para se conectar a uma room e necessario digitar algo no camo "nickname";
    - Ao se conectar com a room o nickname e setado para o player;
    - Para iniciar o jogo, no minimo 2 jogadores precisam estar na room e todos devem apertar "ready" para sinalizarem que estao prontos mudando o simbolo ao lado de seus nome de "O" para "X";
    - Foi criada uma custom propertie no script GameConnection que permite o player guarda a informacao de "ready";
    - Apos todos os jogadores terem sinalizado que estao prontos o GameConnection muda para a cena "1" para dar inicio ao jogo;

# GamePlay
     
    *GameManager
    - Controla as jogadas enviando os dados entre os jogadores;
    - Recebe os enventos do PlayerController;
    - Possui a referencia a um ScriptableObject que contem todos os materias de cartas para geracao das cartas a partir do nome dos materiais;
    - As cartas sao geradas e embaralhadas somente no MasterClient e apos, enviadas para os outros jogadores para nao haver diferenca de dados;
    - os valores que determinan qual player deve jogar (turnNumber) e qual a numeracao de carta deve ser jogada para poder executar o roubo (countNumber) sao gerenciados somente peloMasterClient e apos, enviados para os outros jogadores.
    
    *PlayerManager
    - Controla somente os imputs dos players e dispara o evento para o GameManager;

    *CardManager
    - Controla a movimentacao, o material e o destroy das cartas.
    - tem a referencia a um ScriptableObject q possui todos os materiais de cartas e defini o material da carta comparando o nome do material e o valor passado para a carta;
    - Move para o centro da cena assim que criada;
    - E destruida assim que atinge a mesma posicao do player que a roubou;

# Tratamento de Dados
    *Card
    - Tipo criado para armazenar os valores de uma carta;
    - Recebe um tipo "string" e separa a primeira string para definir qual o nipe da carta e as outras strings para definir o valor numerico da carta;

    *Deck
    - Tipo criado para armazenar os tipos "Card" e manipula-las;
    - Possui dois construtores fora o padrao, podendo ser criado utilizando outro tipo "Deck" ou string com os valores das cartas separados por "|" para facilitar o envio dos valores de um Deck para outros players;
    - Possui um metodo para retornar um tipo "string" que contem todos os valores das cartas separado por "|";
    - Possui outros metodos que tratam de uma manipulacao normal de um baralho;

    *CustomSerialization
    - Tipo criado para armazenar dados do tipo "int" e "string", convertendo ambos para um array do tipo "byte" para melhor envio entre players;
    - Possui um metodo "Serialize" que recebe um tipo dele mesmo e converte para uma array do tipo "byte";
    - Possui um metodo "Deserialize" que recebe um array do tipo "byte" e converte para um tipo dele mesmo;