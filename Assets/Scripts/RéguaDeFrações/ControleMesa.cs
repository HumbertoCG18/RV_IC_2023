using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using System.Threading;

/* Arquivo dos controles da mesa fracionária e das questões de frações.
 * 
 * Otimizações a serem feitas (Por ordem prioritária):
 * 1. Otimizar o uso e o nome das variáveis, que estão confusos.
 * 2. Arrumar a força bruta na geração de frações para a régua (Método duplicado)
 * 3. 
 * 
 * Bugs e defeitos:
 * - Frações são geradas apenas alinhadas ao eixo X global.
 * - Inputs pelos botões não estão funcionando com o VR.
 * 
 * Possibilidades:
 * - Separar o sistema de questões do sistema de controle, se for mais conveniente
 * 
 * Qualquer dúvida, referir ao doc da régua, ou perguntar. 
 * Email para contato: p.dallarosa@edu.pucrs.br
 */

public class ControleMesa : MonoBehaviour
{
    public XRBaseInteractable NumAdd, NumDim, DenAdd, DenDim, FracConfirm;
    public float Razao, RazaoInput, RazãoQuestão;
    public int Numerador, Denominador, NumeradorInicial, NumeradorAnt, DenominadorAnt, NumeradorInput, DenominadorInput, Acertos, QuestaoEmExecucao, QuestaoAnt, ContagemEquivalencia, ReguaCont;
    public bool Certo, CertoInput, confirmarDebug;
    public Vector3 Scale, Position;
    public GameObject PartesFracao, PartesVazias, PecaReferencia, PecaReferenciaQuestao, PrefabReferencia;
    public TMP_Text ViewNumerador, ViewDenominador, Enunciado, Detalhamento;
    private GameObject[] pecasGeradasInput, pecasGeradasQuestao, reguasEquivalencia, reguas, pecasRegua;
    private int[] equivalenciaUmMeio;

    //Start is called before the first frame update
    void Start()
    {
        //Aqui são adicionados Listeners aos botões, para pegar os inputs do usuário
        if (NumAdd != null)
        {
            NumAdd.activated.AddListener(AddNumerador);
        }
        if (NumDim != null)
        {
            NumDim.activated.AddListener(DimNumerador);
        }
        if (DenAdd != null)
        {
            DenAdd.activated.AddListener(AddDenominador);
        }
        if (DenDim != null)
        {
            DenDim.activated.AddListener(DimDenominador);
        }
        if (FracConfirm != null)
        {
            FracConfirm.activated.AddListener(FracaoConfirmar);
        }

        //(Int) Variáveis que armazenam as frações inputs do usuário
        NumeradorInput = 1;
        DenominadorInput = 1;

        //(Int) Contagem de quantas equivalências já foram encontradas pelo usuário
        ContagemEquivalencia = 0;

        //(Int) Usado nas questões aleatórias (Não sei o quão necessário é)
        NumeradorInicial = 1;

        //(Int e Float) Variáveis que acompanham o valor do numerador, denominador e a razão na medida em que são alterados pelo usuário
        Numerador = 1;
        Denominador = 1;
        RazãoQuestão = 1f;

        //(Int) Contadores da questao que está sendo executada e a "anterior" --> Devem ser inicializadas diferentes e serão igualadas durante a execução de uma questão
        QuestaoEmExecucao = 1;
        QuestaoAnt = 0;

        //(Arrays) Arrays que armazenam as equivalencias, as peças de cada fração gerada e cada regua gerada, respectivamente
        equivalenciaUmMeio = new int[6];
        pecasGeradasInput = new GameObject[12];
        pecasGeradasQuestao = new GameObject[12];
        reguasEquivalencia = new GameObject[12];
        reguas = new GameObject[12];
        pecasRegua = new GameObject[100];

        //(Booleanos) Variáveis checadoras do input e variável usada para executar o FracaoConfirmarDebug();
        Certo = false;
        CertoInput = false;
        confirmarDebug = false;

        //(Int) Contador das réguas geradas
        int ReguaCont = 0;

        //(Int) Contador dos acertos
        Acertos = 0;

        //(Float) Razao do Input
        RazaoInput = 1f;

        //Inicializa a geração das 
        GeraFracao(PecaReferencia, pecasGeradasInput, Numerador, Denominador);

        //Aleatoria(PecaReferenciaQuestao);
    }

    // Update is called once per frame
    void Update()
    {

        //Atualiza a fração no display a cada frame
        ViewFracao();

        //if(Certo != CertoInput)
        //{
        //    Aleatoria(PecaReferenciaQuestao); //Se a resposta se tornar correta, executa a questão novamente
        //}

        if (DenominadorAnt != Denominador || NumeradorAnt != Numerador) //Se houverem mudanças no numerador ou denominador o método é executado
        {
            //Calcula a razao dos valores fluidos
            Razao = (float)Numerador / Denominador;

            GeraFracao(PecaReferencia, pecasGeradasInput, Numerador, Denominador); //Função que gera as frações 
        }

        //Checa se a QuestaoEmExecucao é distinta da auxiliar QuestaoAnt (Se for, significa que o contador QuestaoEmExecucao foi incrementado)
        if (QuestaoAnt != QuestaoEmExecucao)
        {
            //Informa no console a questão que está sendo executada
            Debug.Log("Atualmente executando a questão " + QuestaoEmExecucao);

            //Iguala a auxiliar QuestaoAnt à questão atual
            QuestaoAnt = QuestaoEmExecucao;

            //Com base no contador QuestaoEmExecucao, irá setar o ambiente para uma questão diferente
            switch (QuestaoEmExecucao)
            {
                case 1:
                    Questao1();
                    break;
                case 2:
                    Questao2(); 
                    break;
                case 3:
                    Questao3();
                    break;
                default:
                    Debug.Log("Erro, valor de questão inválido");
                    break;
            }
        }

        //Executa o método auxiliar de input (Utilizado para definir os inputs sem o VR conectado, ou em situação de falha do sistema de inputs)
        if (confirmarDebug == true)
        {
            FracaoConfirmarDebug();
            confirmarDebug = false;
        }

    }

      //=========================================================================================//=========================================================================================//
     //  Aqui começam as funções responsáveis por pegar o input do usuário                                                                                                                 //
    //=========================================================================================//=========================================================================================//

    //Método que acresce o numerador
    void AddNumerador(ActivateEventArgs args)
    {
        //O numerador não deve ultrapassar o denominador, evitando frações impróprias
        if (Numerador < Denominador)
        {
            Numerador++;
            Debug.Log("AddNumerador pressionado");
        }
    }
    //Método que decresce o numerador
    void DimNumerador(ActivateEventArgs args)
    {
        //O numerador não pode ser nulo --> não decresce, caso o (valor - 1) seja menor que 0
        if (Numerador - 1 > 0)
        {
            Numerador--;
            Debug.Log("DimNumerador pressionado");
        }
    }
    //Método que decresce o denominador
    void DimDenominador(ActivateEventArgs args)
    {
        //O Denominador não pode ser nulo e deve ser maior que o numerador
        if (Denominador - 1 > 0 && Denominador > Numerador)
        {
            Denominador--;
            Debug.Log("DimDenominador pressionado");
        }
    }
    //Método que acresce o denominador
    void AddDenominador(ActivateEventArgs args)
    {
        //O denominador deve ser maior que 0 e ter o valor máximo de 12
        if (Denominador > 0 && Denominador < 12)
        {
            Denominador++;
            Debug.Log("AddDenominador pressionado");
        }
    }
    //Método que confirma os valores pre-definidos
    void FracaoConfirmar(ActivateEventArgs args)
    {
        //Define o Numerador e Denominador de Input com o valor do Numerador e Denominador "fluido"
        DenominadorInput = Denominador;
        NumeradorInput = Numerador;

        //Variável assume o valor booleano de checaResultado();
        CertoInput = checaResultado();
        Debug.Log("Fração Confirmada!" + CertoInput);//É retornado pelo terminal se a resposta está correta

        //Ao fim, define CertoInput como falso
        CertoInput = false;
    }
    //Método de confirmação para Debug (Sem o VR Conectado, ou em falha do sistema de input)
    void FracaoConfirmarDebug()
    {
        //Define os valores de input como os valores fluidos (É como tirar um print do valores inseridos)
        DenominadorInput = Denominador;
        NumeradorInput = Numerador;
        RazaoInput = Razao;

        //Variável armazena o valor booleando de checaResultado(). Checa se o input do usuário está correto
        CertoInput = checaResultado();

        Debug.Log("Fração Confirmada pelo Debug: " + CertoInput);
    }

      //=========================================================================================//=========================================================================================//
     //  Aqui se encerram as funções responsáveis por pegar o input do usuário                                                                                                             //
    //=========================================================================================//=========================================================================================//
    
    //Exibe a fração no painel da mesa.
    void ViewFracao()
    {
        ViewNumerador.text = Numerador.ToString();
        ViewDenominador.text = Denominador.ToString();
    }

    //Método que gera as frações
    void GeraFracao(GameObject PecaReferencia, GameObject[] pecasGeradas, int Numerador, int Denominador)
    {
        //Método que reseta as frações geradas previamente
        ResetaFracao(pecasGeradas);

        //Armazena a posição, escala e a rotação da peça inteira, respectivamente.
        Vector3 PosicaoReferencia = PecaReferencia.transform.localPosition;
        Vector3 TamanhoOriginalPartes = PartesFracao.transform.localScale;
        Quaternion RotacaoReferencia = PecaReferencia.transform.rotation;
        
        //Calcula o tamanho das partes da fração
        float TamXPeca = TamanhoOriginalPartes.x / Denominador;

        //Iniciando em zero, gera as peças com base no numerador
        for (int i = 0; i < Numerador; i++)
        {
            //Pega a posição da peça inteira
            Vector3 NovaPosicao = new Vector3(PecaReferencia.transform.position.x, PecaReferencia.transform.position.y, PecaReferencia.transform.position.z);
            //Pega o tamanho da peça variável
            Vector3 NovoTamanho = PartesFracao.transform.localScale;

            //Cálculo da posição da peça i ((ComprimentoAjustado * pecasGeradas)/2) - ComprimentoOriginal + ComprimentoAjustado)
            NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (TamanhoOriginalPartes.x / 100) + (TamXPeca / 100), 0, 0);

            //Criando peças em posições diferentes (Instantiate(Prefab, Position, Rotation, Parenting))
            GameObject novaPeca = Instantiate(PartesFracao, NovaPosicao, RotacaoReferencia, PecaReferencia.transform);

            //Mudando o tamanho das peças
            NovoTamanho.x = TamXPeca;
            //Setando o tamanho das peças
            novaPeca.transform.localScale = NovoTamanho;

            //Armazena as peças geradas em um array, possibilitando excluí-las depois
            pecasGeradas[i] = novaPeca;
        }
        //Iniciando no numerador, gera as peças "vazias" com base no denominador
        for (int i = Numerador; i < Denominador; i++) 
        {
            //Vector3 da posição que será definida (é inicializado com a posição da peça inteira)
            Vector3 NovaPosicao = new Vector3(PecaReferencia.transform.position.x, PecaReferencia.transform.position.y, PecaReferencia.transform.position.z);
            //Vector3 que armazena a escala da peça que vamos modificar
            Vector3 NovoTamanho = PartesFracao.transform.localScale;

            //Calcula a posição da próxima peça a ser gerada (Comprimento ajustado de uma peça vezes a quantidade de peças já geradas dividido por 2, menos o comprimento original, mais o comprimento ajustado)
            NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (TamanhoOriginalPartes.x / 100) + (TamXPeca / 100), 0, 0); // ((ComprimentoAjustado * pecasGeradas)/2) - ComprimentoOriginal + ComprimentoAjustado)

            //Criando peças em posições diferentes (Instantiate(Prefab, Position, Rotation, Parenting))
            GameObject novaPeca = Instantiate(PartesVazias, NovaPosicao, RotacaoReferencia, PecaReferencia.transform);

            //Ajusta o X do Vector3 (Posição) da peça
            NovoTamanho.x = TamXPeca;
            //Define a escala da peça que será modificada para a nova escala
            novaPeca.transform.localScale = NovoTamanho;

            //Armazena as peças geradas em um array, possibilitando excluí-las depois
            pecasGeradas[i] = novaPeca;
        }

        //Armazena o valor do numerador no fim da execução
        NumeradorAnt = Numerador;
        //Armazena o valor do denominador no fim da execução
        DenominadorAnt = Denominador;
    }

    void GeraFracaoParaRegua(GameObject PecaReferencia, GameObject[] pecasGeradas, int Numerador, int Denominador)
    {
        //Armazena a posição, escala e a rotação da peça inteira, respectivamente.
        Vector3 PosicaoReferencia = PecaReferencia.transform.localPosition;
        Vector3 TamanhoOriginalPartes = PartesFracao.transform.localScale;
        Quaternion RotacaoReferencia = PecaReferencia.transform.rotation;

        //Calcula o tamanho das partes da fração
        float TamXPeca = TamanhoOriginalPartes.x / Denominador;

        //Iniciando em zero, gera as peças com base no numerador
        for (int i = 0; i < Numerador; i++)
        {
            //Pega a posição da peça inteira
            Vector3 NovaPosicao = new Vector3(PecaReferencia.transform.position.x, PecaReferencia.transform.position.y, PecaReferencia.transform.position.z);
            //Pega o tamanho da peça variável
            Vector3 NovoTamanho = PartesFracao.transform.localScale;

            //Cálculo da posição da peça i ((ComprimentoAjustado * pecasGeradas)/2) - ComprimentoOriginal + ComprimentoAjustado)
            NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (TamanhoOriginalPartes.x / 100) + (TamXPeca / 100), 0, 0);

            //Criando peças em posições diferentes (Instantiate(Prefab, Position, Rotation, Parenting))
            GameObject novaPeca = Instantiate(PartesFracao, NovaPosicao, RotacaoReferencia, PecaReferencia.transform);

            //Mudando o tamanho das peças
            NovoTamanho.x = TamXPeca;
            //Setando o tamanho das peças
            novaPeca.transform.localScale = NovoTamanho;

            //Armazena as peças geradas em um array, possibilitando excluí-las depois
            pecasGeradas[i] = novaPeca;
        }
        //Iniciando no numerador, gera as peças "vazias" com base no denominador
        for (int i = Numerador; i < Denominador; i++)
        {
            //Vector3 da posição que será definida (é inicializado com a posição da peça inteira)
            Vector3 NovaPosicao = new Vector3(PecaReferencia.transform.position.x, PecaReferencia.transform.position.y, PecaReferencia.transform.position.z);
            //Vector3 que armazena a escala da peça que vamos modificar
            Vector3 NovoTamanho = PartesFracao.transform.localScale;

            //Calcula a posição da próxima peça a ser gerada (Comprimento ajustado de uma peça vezes a quantidade de peças já geradas dividido por 2, menos o comprimento original, mais o comprimento ajustado)
            NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (TamanhoOriginalPartes.x / 100) + (TamXPeca / 100), 0, 0); // ((ComprimentoAjustado * pecasGeradas)/2) - ComprimentoOriginal + ComprimentoAjustado)

            //Criando peças em posições diferentes (Instantiate(Prefab, Position, Rotation, Parenting))
            GameObject novaPeca = Instantiate(PartesVazias, NovaPosicao, RotacaoReferencia, PecaReferencia.transform);

            //Ajusta o X do Vector3 (Posição) da peça
            NovoTamanho.x = TamXPeca;
            //Define a escala da peça que será modificada para a nova escala
            novaPeca.transform.localScale = NovoTamanho;

            //Armazena as peças geradas em um array, possibilitando excluí-las depois
            pecasGeradas[i] = novaPeca;
        }

        //Armazena o valor do numerador no fim da execução
        NumeradorAnt = Numerador;
        //Armazena o valor do denominador no fim da execução
        DenominadorAnt = Denominador;
    }

    void geraRegua(int i, int Numerador, int Denominador)
    {
        //Armazena a posicao, tamanho e orientacao(rotacao) da peca de referencia e o tamanho original do prefab das partes, respectivamente
        Vector3 Posicao = new Vector3(0, 0, 0);
        Vector3 Escala = new Vector3(1, 1, 1);
        Quaternion Angulo = PecaReferenciaQuestao.transform.rotation;
        Vector3 TamanhoOriginalPartes = PrefabReferencia.transform.localScale;

        //Variavel que armazena a altura das pecas e a posicao de referencia
        float Altura = TamanhoOriginalPartes.y;
        Debug.Log("TamanhoOriginalPartes.y: " + TamanhoOriginalPartes.y);

        //A nova posicao e inicializada a posicao da peca de referencia
        Vector3 NovaPosicao = Posicao;

        //Calcula a altura de onde a peca sera gerada
        NovaPosicao.y = 1 * (i - 1);
        Debug.Log("NovaPosicao: " + NovaPosicao);

        //ReguaCont;

        //Criando a nova peça de referencia(Instantiate(Prefab, Position, Rotation, Parenting)) --> Alguma coisa errada na posicao em que esta sendo gerada
        Debug.Log("Posicao: " + NovaPosicao + "\nAngulo: " + Angulo);
        GameObject pecaReferencia = Instantiate(PrefabReferencia, NovaPosicao, Angulo, PecaReferenciaQuestao.transform);
        Debug.Log("Criando nova peça de referência");

        //Seta o tamanho e a posicao da peça de referencia 
        pecaReferencia.transform.localScale = Escala;
        pecaReferencia.transform.localPosition = NovaPosicao;

        //Armazena a peça de referencia gerada no vetor reguas
        reguas[i] = pecaReferencia;
        Debug.Log("Reguas: " + reguas[i]);

        //Gera a fracao de forma relativa a peca de referencia
        GeraFracaoParaRegua(pecaReferencia, pecasRegua, Numerador, Denominador);
    }

    void ResetaFracao(GameObject[] pecasGeradas)
    {
        foreach(GameObject peca in pecasGeradas)
        {
            if (peca != null) 
            {
                Destroy(peca);
            }
        }
    }

      //=========================================================================================//=========================================================================================//
     //  Aqui começa o sistema de questões (poderá ser passados para um script separado se julgarmos mais conveniente)                                                                     //
    //=========================================================================================//=========================================================================================//
    /*
     * Questões:
     * 1. Utilizando os botões da interface, selecione a fração que representa 1/2 da unidade. Após, selecione todas as outras frações equivalentes a 1/2.
     * 2. Utilizando os botões da interface, seleciona todas as frações equivalentes a 1/3.
     * 3. Observe o valor desconhecido gerado acima. Utilizando os botões da interface e testando outros valores, encontre todas as frações equivalentes a este valor desconhecido.
     * Bônus: Identificação aleatória
     * 
     * Pendências por ordem prioritária:
     * 1. Implementação das questões definidas
     * 2. Implementar métodos que setam as questões
     * 3. 
     * 
     * 
     * Defeitos e Bugs:
     * - checaQ1(), deve primeiro checar se o primeiro input é um meio e só depois armazenar as equivalências
     * -
     * -
     * 
     */


    //Checa se todas as condições foram cumpridas
    bool checaResultado()
    {
        switch (QuestaoEmExecucao)
        {
            case 1:
                //Condições Q1 --> Fração 1/2 encontrada e todas as equivalentes dentro de (1 <= Denominador <= 12)
                return checaQ1();
            case 2:
                //Condições Q2
                return checaQ2();
            case 3:
                //Condições Q3
                return true;
            default:
                return false;

        }
    }

    void Aleatoria(GameObject FracaoInteira)
    {
        //Define a variável checadora como falsa
        CertoInput = false;
        //Seta o enunciado
        Enunciado.text = ("Identifique a fração");
        Detalhamento.text = ("Uma fração aleatória será gerada. Utilize a interface da mesa para identificar qual é a fração!");

        //Inicializa o denominador máximo
        int DenominadorBase;

        //Define o denominador máximo com base no número de acertos
        if(Acertos == 0)
        {
            DenominadorBase = 4;
        }
        else if(Acertos > 1 && Acertos < 4)
        {
            DenominadorBase = 4 * Acertos;
        }
        else
        {
            DenominadorBase = 12;
        }

        //Seleciona um denominador aleatório <= 4
        int Denominador = UnityEngine.Random.Range(2, DenominadorBase);
        //Seleciona um numerador aleatório <= denominador
        NumeradorInicial = UnityEngine.Random.Range(1, Denominador);

        //Gera a fração definida
        GeraFracao(FracaoInteira, pecasGeradasQuestao, NumeradorInicial, Denominador);

        //Pega a razão da fração
        RazãoQuestão = (float)NumeradorInicial / Denominador;
        Debug.Log("Razão da Questão :" + RazãoQuestão);

        
    }//Talvez fazer uma função só que limita o denominador atravéz de um contador;

    void Questao1()
    {
        //Setar textos
        Enunciado.text = ("Identifique 1/2");
        Detalhamento.text = ("Utilizando os botões da interface, selecione a fração que representa 1/2 da unidade");
        //Deixar peças não usadas invisíveis

        // <Inserir código aqui>
    }

    //Método bool que checa as condições da questão 1
    bool checaQ1()
    {
        //Se o o número de frações equivalentes encontradas não for == 6
        if (ContagemEquivalencia < 5)
        {
            //Se 1/2 não foi inserido
            if (RazaoInput == 0.5f && pertenceAoArray(equivalenciaUmMeio, 2) == false && DenominadorInput == 2)
            {
                equivalenciaUmMeio[ContagemEquivalencia] = DenominadorInput;
                geraRegua((ContagemEquivalencia + 1), NumeradorInput, DenominadorInput);
                ContagemEquivalencia++;
            }
            //Se 1/2 já foi inserido
            if (pertenceAoArray(equivalenciaUmMeio, 2) == true)
            {
                Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/2");

                //Se a RazaoInput (da fração Input do usuário) for igual a 0.5 e não pertence ao array
                if (RazaoInput == 0.5f && pertenceAoArray(equivalenciaUmMeio, DenominadorInput) == false)
                {
                    //Seta o texto do enunciado e mostra quantas frações faltam
                    Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/2\n" + ContagemEquivalencia + "de" + 6 + "Descobertas");
                    Debug.Log("ContagemEquivalencia: " + ContagemEquivalencia);

                    //Sinaliza no console que a questão está sendo checada
                    Debug.Log("ChecandoQ1");

                    //Armazena o denominador da fração definida pelo usuário no array
                    equivalenciaUmMeio[ContagemEquivalencia] = DenominadorInput;

                    //Passa para o próximo index do array
                    ContagemEquivalencia++;

                    //Gera a regua inserida
                    geraRegua(ContagemEquivalencia, NumeradorInput, DenominadorInput);
                    //ReguaCont++;

                }
            }

        }
        //Se o número de equivalências inseridos no vetor for igual a 6 (Todas as equivalências de um meio)
        else
        {
            //Retorna ao usuário que a próxima questão será executada
            Detalhamento.text = ("Muito bem! Carregando a próxima questão...");

            //Encontrar algum jeito de pausar a execução por 10 ou 15 segundos (Provavelmente pedir para apertar o botão de confirmar para continuar)

            //Incrementa o contador QuestaoEmExecucao (Passa para a próxima questão)
            QuestaoEmExecucao++;

            //Deleta a régua fracionária gerada
            ResetaFracao(pecasRegua);

            //Retorna verdadeiro
            return true;
        }
        return false;
    }

    //Método temporário que eu criei pra comparar os elementos de um array (Esqueci os do C# kkkkkkkkk)
    bool pertenceAoArray(int[] equivalentes, int DenominadorY)
    {
        //Para cada elemento dentro do array inserido
        foreach (int valor in equivalentes)
        {
            //Se o elemento do array for igual ao input do usuário, retorna verdadeiro
            if (valor == DenominadorY)
            {
                return true;
            }
        }
        //Se não pertence, retorna falso
        return false;
    }

    //Método que seta o ambiente para a questão 2
    void Questao2()
    {
        //Limpa o vetor da regua
        ResetaFracao(reguas);

        Enunciado.text = ("Identifique 1/3");
        Detalhamento.text = ("Utilizando os botões da interface, selecione todas frações que representam 1/3 da unidade");
    }

    //Método bool que checa as condições da questão 2
    bool checaQ2()
    {



        return true;
    }

    //Método que seta o ambiente para a questão 3
    void Questao3()
    {

    }

    //Método bool que checa as condições da questão 3
    bool checa3()
    {
        return true;
    }

}