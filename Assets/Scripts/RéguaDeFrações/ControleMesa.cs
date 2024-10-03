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
using System.Runtime.InteropServices.WindowsRuntime;

/* Arquivo dos controles da mesa fracionária e das questões de frações.
 * 
 * Otimizações a serem feitas (Por ordem prioritária):
 * 1. Otimizar o uso e o nome das variáveis, que estão confusos.
 * 2. 
 * 3. 
 * 
 * Bugs e defeitos:
 * - Frações são geradas apenas alinhadas ao eixo X global.
 * - Inputs pelos botões não estão funcionando com o VR. --> OnHoverEntered funciona, mas activated não
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
    public float Razao, RazaoInput, RazaoQuestao;
    public int Numerador, Denominador, NumeradorInicial, NumeradorAnt, DenominadorAnt, NumeradorInput, DenominadorInput, Acertos, QuestaoEmExecucao, QuestaoAnt, ContagemEquivalencia, ReguaCont;
    public bool Certo, CertoInput, confirmarDebug;
    public GameObject PartesFracao, PartesVazias, PecaReferencia, PecaReferenciaQuestao, PrefabReferencia;
    public TMP_Text ViewNumerador, ViewDenominador, Enunciado, Detalhamento;
    private GameObject[] pecasGeradasInput, pecasGeradasQuestao, reguasEquivalencia, reguas, pecasRegua;
    private int[] equivalencias, zerado;

    //Start is called before the first frame update
    void Start()
    {
        //Aqui são adicionados Listeners aos botões, para pegar os inputs do usuário
        if (NumAdd != null)
        {
            NumAdd.selectEntered.AddListener(AddNumerador);
        }
        if (NumDim != null)
        {
            NumDim.selectEntered.AddListener(DimNumerador);
        }
        if (DenAdd != null)
        {
            DenAdd.selectEntered.AddListener(AddDenominador);
            //NumAdd.selectEntered.AddListener(SelectDimDenominador);
        }
        if (DenDim != null)
        {
            DenDim.selectEntered.AddListener(DimDenominador);
        }
        if (FracConfirm != null)
        {
            FracConfirm.selectEntered.AddListener(FracaoConfirmar);
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
        RazaoQuestao = 1f;

        //(Int) Contadores da questao que está sendo executada e a "anterior" --> Devem ser inicializadas diferentes e serão igualadas durante a execução de uma questão
        QuestaoEmExecucao = 1;
        QuestaoAnt = 0;

        //(Arrays[]) Arrays que armazenam as equivalencias, as peças de cada fração gerada e cada regua gerada, respectivamente
        equivalencias = new int[10];
        zerado = new int[10];
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

        //Inicializa a geração das frações
        GeraFracao(PecaReferencia, pecasGeradasInput, Numerador, Denominador);

        //Inicializa o display das frações
        ViewFracao();
    }

    void SelectDimDenominador(SelectEnterEventArgs args)
    {
        //O Denominador não pode ser nulo e deve ser maior que o numerador
        if (Denominador - 1 > 0 && Denominador > Numerador)
        {
            Denominador--;
        }
        Debug.Log("DimDenominador pressionado");
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("Hover sobre o botão detectado");
    }

    // Update is called once per frame
    void Update()
    {
        //Se houverem mudanças no numerador ou denominador o método é executado
        if (DenominadorAnt != Denominador || NumeradorAnt != Numerador)
        {
            //Atualiza a fração no display a cada frame
            ViewFracao();

            //Calcula a razao dos valores fluidos
            Razao = (float)Numerador / Denominador;

            //Apaga a fração (as peças geradas)
            ResetaFracao(pecasGeradasInput);

            //Gera a nova fração (novas peças)
            GeraFracao(PecaReferencia, pecasGeradasInput, Numerador, Denominador);
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
                case 4:
                    Aleatoria(PecaReferenciaQuestao);
                    break;
                default:
                    Debug.Log("Erro: Número de questão inválido");
                    QuestaoEmExecucao = 1;
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
    void AddNumerador(SelectEnterEventArgs args)
    {
        //O numerador não deve ultrapassar o denominador, evitando frações impróprias
        if (Numerador < Denominador)
        {
            Numerador++;
        }
        Debug.Log("AddNumerador pressionado");
    }
    //Método que decresce o numerador
    void DimNumerador(SelectEnterEventArgs args)
    {
        //O numerador não pode ser nulo --> não decresce, caso o (valor - 1) seja menor que 0
        if (Numerador - 1 > 0)
        {
            Numerador--;
        }
        Debug.Log("DimNumerador pressionado");
    }
    //Método que decresce o denominador
    void DimDenominador(SelectEnterEventArgs args)
    {
        //O Denominador não pode ser nulo e deve ser maior que o numerador
        if (Denominador - 1 > 0 && Denominador > Numerador)
        {
            Denominador--;
        }
        Debug.Log("DimDenominador pressionado");
    }
    //Método que acresce o denominador
    void AddDenominador(SelectEnterEventArgs args)
    {
        //O denominador deve ser maior que 0 e ter o valor máximo de 12
        if (Denominador > 0 && Denominador < 12)
        {
            Denominador++;
        }
        Debug.Log("AddDenominador pressionado");
    }
    //Método que confirma os valores pre-definidos
    void FracaoConfirmar(SelectEnterEventArgs args)
    {
        //Define os valores de input como os valores fluidos (É como tirar um print do valores inseridos)
        DenominadorInput = Denominador;
        NumeradorInput = Numerador;
        RazaoInput = Razao;

        //Variável armazena o valor booleando de checaResultado(). Checa se o input do usuário está correto
        CertoInput = ChecaResultado();

        //Se a questao em execucao for a 4, e a sua resposta estiver correta
        if (QuestaoEmExecucao == 4 && CertoInput)
        {
            //Aumentamos o número de acertos das questoes aleatorias
            Acertos++;

            //Geramos uma nova "questão" aleatória
            Aleatoria(PecaReferenciaQuestao);

        }

        Debug.Log("Fração Confirmada pelo Debug: " + CertoInput);   
    }
    //Método de confirmação para Debug (Sem o VR Conectado, ou em falha do sistema de input)
    void FracaoConfirmarDebug()
    {
        //Define os valores de input como os valores fluidos (É como tirar um print do valores inseridos)
        DenominadorInput = Denominador;
        NumeradorInput = Numerador;
        RazaoInput = Razao;

        //Variável armazena o valor booleando de checaResultado(). Checa se o input do usuário está correto
        CertoInput = ChecaResultado();

        //Se a questao em execucao for a 4, e a sua resposta estiver correta
        if(QuestaoEmExecucao == 4 && CertoInput)
        {
            //Aumentamos o número de acertos das questoes aleatorias
            Acertos++;

            //Geramos uma nova "questão" aleatória
            Aleatoria(PecaReferenciaQuestao);
            
        }

        Debug.Log("Fração Confirmada pelo Debug: " + CertoInput);
    }

      //=========================================================================================//=========================================================================================//
     //  Aqui se encerram as funções responsáveis por pegar o input do usuário                                                                                                             //
    //=========================================================================================//=========================================================================================//

    //Método que gera as frações
    void GeraFracao(GameObject PecaReferencia, GameObject[] pecasGeradas, int Numerador, int Denominador)
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

    void GeraRegua(int i, int Numerador, int Denominador)
    {
        //Armazena a posicao, tamanho e orientacao(rotacao) da peca de referencia e o tamanho original do prefab das partes, respectivamente
        Vector3 Posicao = new Vector3(0, 0, 0);
        Vector3 Escala = new Vector3(1, 1, 1);
        Quaternion Angulo = PecaReferenciaQuestao.transform.rotation;
        Vector3 TamanhoOriginalPartes = PrefabReferencia.transform.localScale;

        //Variavel que armazena a altura das pecas e a posicao de referencia
        float Altura = TamanhoOriginalPartes.y;

        //A nova posicao e inicializada a posicao da peca de referencia
        Vector3 NovaPosicao = Posicao;

        //Calcula a altura de onde a peca sera gerada
        NovaPosicao.y = 1 * (i - 1);

        //Criando a nova peça de referencia(Instantiate(Prefab, Position, Rotation, Parenting))
        GameObject pecaReferencia = Instantiate(PrefabReferencia, NovaPosicao, Angulo, PecaReferenciaQuestao.transform);

        //Seta o tamanho e a posicao da peça de referencia 
        pecaReferencia.transform.localScale = Escala;
        pecaReferencia.transform.localPosition = NovaPosicao;

        //Armazena a peça de referencia gerada no vetor reguas
        reguas[i] = pecaReferencia;

        //Gera a fracao de forma relativa a peca de referencia
        GeraFracao(pecaReferencia, pecasRegua, Numerador, Denominador);
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
     //  Aqui começa o sistema de questões (poderá ser passados para um script separado se for julgado mais conveniente)                                                                   //
    //=========================================================================================//=========================================================================================//
    /*
     * Questões:
     * 1. Utilizando os botões da interface, selecione a fração que representa 1/2 da unidade. Após, selecione todas as outras frações equivalentes a 1/2.
     * 2. Utilizando os botões da interface, seleciona todas as frações equivalentes a 1/3.
     * 3. Observe o valor desconhecido gerado acima. Utilizando os botões da interface e testando outros valores, encontre todas as frações equivalentes a este valor desconhecido.
     * 4. Bônus: Identificação aleatória
     * 
     * Pendências por ordem prioritária:
     * 1. Implementação das questões definidas (Atravez dos metodos checaQx(); --> Resolvido
     * 2. Implementar métodos que setam as questões (Falta ativar/desativar a renderizacao da peca de referencia da questao) --> Resolvido
     * 3. Questão3 não possuí um valor definido no enunciado (Definir com o João)
     * 
     * 
     * Defeitos e Bugs:
     * - checaQ1(), deve primeiro checar se o primeiro input é um meio e só depois armazenar as equivalências
     * - Bugs na checagem das equivalências (Razões retornando como diferentes)
     * -
     * 
     * Otimizações Possíveis:
     * - Passar grande parte da checagem para um método global, como a lógica é sempre a mesma
     * 
     */


    //Checa se todas as condições foram cumpridas
    bool ChecaResultado()
    {
        switch (QuestaoEmExecucao)
        {
            case 1:
                //Condições Q1
                return ChecaQuestaoEquivalencia(1, 2, 6);
            case 2:
                //Condições Q2
                return ChecaQuestaoEquivalencia(1, 3, 4);
            case 3:
                //Condições Q3
                return ChecaQuestaoEquivalencia(1, 4, 3);
            case 4:
                return ChecaAleatoria();
            default:
                Debug.Log("Erro: Número de questão inválido");
                QuestaoEmExecucao = 1;
                return false;

        }
    }

    void Aleatoria(GameObject FracaoInteira)
    {
        //Reseta as pecas geradas
        ResetaFracao(pecasGeradasQuestao);

        //Limpa o vetor da regua
        ResetaFracao(reguas);

        //Reseta Numerador e Denominador
        ResetNumDen();

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
        RazaoQuestao = (float)NumeradorInicial / Denominador;
        Debug.Log("Razão da Questão :" + RazaoQuestao);

        
    }//Talvez fazer uma função só que limita o denominador atravéz de um contador;

    bool ChecaAleatoria()
    {
        if(RazaoInput == RazaoQuestao && NumeradorInput == NumeradorInicial)
        {
            return true;
        }

        return false;
    }

    void Questao1()
    {
        //Limpa o vetor da regua (Caso esteja preenchido por algum motivo desconhecido)
        ResetaFracao(reguas);

        //Reseta Numerador e Denominador
        ResetNumDen();

        //Seta os enunciados
        Enunciado.text = ("Identifique 1/2");
        Detalhamento.text = ("Utilizando os botões da interface, selecione a fração que representa 1/2 da unidade");
    }

    //Método que seta o ambiente para a questão 2
    void Questao2()
    {
        //Reseta Numerador e Denominador
        ResetNumDen();

        //Limpa o vetor da regua
        ResetaFracao(reguas);

        //Seta os enunciados
        Enunciado.text = ("Identifique 1/3");
        Detalhamento.text = ("Utilizando os botões da interface, selecione todas frações que representam 1/3 da unidade");
    }

    //Método que seta o ambiente para a questão 3
    void Questao3()
    {
        //Reseta Numerador e Denominador
        ResetNumDen();

        //Limpa o vetor da regua
        ResetaFracao(reguas);

        //Seta os enunciados
        Enunciado.text = ("Identifique o valor");
        Detalhamento.text = ("Utilizando os botões da interface, identifique a fração do valor desconhecido gerado");

        //Gera a fração desconhecida na peca de referencia
        GeraFracao(PecaReferenciaQuestao, pecasGeradasQuestao, 1, 4); //Por hora colocado 1/4 --> Ver com o João o valor da questão
    }

    bool ChecaQuestaoEquivalencia(int NumeradorQuestao, int DenominadorQuestao, int NumeroDeEquivalencias)
    {
        //Calcula a razao da questao
        float RazaoDaQuestao = (float) NumeradorQuestao / DenominadorQuestao;

        //E possivel fazer um metodo que automaticamente retorna o numero de equivalencias, mas é realmente interessante?

        if (ContagemEquivalencia < (NumeroDeEquivalencias - 1))
        {
            //Se a fracao da questao ainda nao foi inserida e o input for a fracao da questao
            if (RazaoInput == RazaoDaQuestao && PertenceAoArray(equivalencias, DenominadorQuestao) == false && DenominadorInput == DenominadorQuestao)
            {
                //Seta o texto do enunciado e mostra quantas frações faltam
                switch (QuestaoEmExecucao)
                {
                    case 1:
                        GeraRegua((ContagemEquivalencia + 1), NumeradorInput, DenominadorInput);
                        Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/2\n" + 1 + " de " + 6 + " Descobertas");
                        break;
                    case 2:
                        GeraRegua((ContagemEquivalencia + 1), NumeradorInput, DenominadorInput);
                        Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/3\n" + 1 + " de " + 4 + " Descobertas");
                        break;
                    case 3:
                        Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/4\n" + 1 + " de " + 3 + " Descobertas");
                        break;
                    case 4:
                        break;
                    default:
                        Debug.Log("Erro: Número de questão inválido");
                        QuestaoEmExecucao = 1;
                        break;
                }
                //O Denominador do Input do usuário eh armazenado no array de equivalencias descobertas
                equivalencias[ContagemEquivalencia] = DenominadorInput;
                ContagemEquivalencia++;
            }
            //Se a fracao da questao ja foi inserida
            if (PertenceAoArray(equivalencias, DenominadorQuestao) == true)
            {
                //Se a RazaoInput for igual a RazaoDaQuestao e o input nao esta no vetor
                if (RazaoInput == RazaoDaQuestao && PertenceAoArray(equivalencias, DenominadorInput) == false)
                {
                    //Retorna no console a contagem das equivalencias
                    Debug.Log("ContagemEquivalencia: " + ContagemEquivalencia);

                    //Sinaliza no console que a questão está sendo checada
                    Debug.Log("Checando Questão " + QuestaoEmExecucao);

                    //Armazena o denominador da fração definida pelo usuário no array
                    equivalencias[ContagemEquivalencia] = DenominadorInput;

                    //Passa para o próximo index do array
                    ContagemEquivalencia++;

                    //Seta o texto do enunciado e mostra quantas fracoes faltam, com base na questao que esta sendo executada
                    switch (QuestaoEmExecucao)
                    {
                        case 1:
                            Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/2\n" + ContagemEquivalencia + " de " + 6 + " Descobertas");
                            break;
                        case 2:
                            Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/3\n" + ContagemEquivalencia + " de " + 4 + " Descobertas");
                            break;
                        case 3:
                            Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/4\n" + ContagemEquivalencia + " de " + 3 + " Descobertas");
                            break;
                        case 4:
                            break;
                        default:
                            Debug.Log("Erro: Número de questão inválido");
                            QuestaoEmExecucao = 1;
                            break;
                    }
                    //Gera a regua inserida
                    GeraRegua(ContagemEquivalencia, NumeradorInput, DenominadorInput);
                }
            }
        }
        else if (ContagemEquivalencia == NumeroDeEquivalencias)
        {
            //Deleta a régua fracionária gerada
            ResetaFracao(pecasRegua);

            //Reseta o array das equivalencias
            equivalencias = zerado;

            //Reseta a contagem das equivalencias
            ContagemEquivalencia = 0;

            //Incrementa o contador QuestaoEmExecucao (Passa para a próxima questão)
            QuestaoEmExecucao++;

            //Retorna verdadeiro
            return true;
        }
        else
        {
            //Gera a ultima regua inserida
            GeraRegua((ContagemEquivalencia + 1), NumeradorInput, DenominadorInput);

            //Retorna ao usuário que a questão foi resolvida e aguarda seu input para continuar.
            switch (QuestaoEmExecucao)
            {
                case 1:
                    Detalhamento.text = ("Muito bem! Você gerou a régua fracionária das equivalências de 1/2. Aperte o botão de confirmação para avançar para a próxima questão");
                    break;
                case 2:
                    Detalhamento.text = ("Muito bem! Você gerou a régua fracionária das equivalências de 1/3. Aperte o botão de confirmação para avançar para a próxima questão");
                    break;
                case 3:
                    Detalhamento.text = ("Muito bem! Você identificou o valor desconhecido como 1/4 e gerou todas suas equivalências. Aperte o botão de confirmação para avançar para a próxima questão");
                    break;
                default:
                    Debug.Log("Erro: Número de questão inválido");
                    QuestaoEmExecucao = 1;
                    break;
            }

            //Passa para o próximo passo da checagem (Else if(ContagemEquivalencia == 3))
            ContagemEquivalencia++;
        }

        return false;
    }

      //=========================================================================================//=========================================================================================//
     //  Aqui começam pequenos métodos auxiliares                                                                                                                                          //
    //=========================================================================================//=========================================================================================//

    //Exibe a fração no painel da mesa.
    void ViewFracao()
    {
        ViewNumerador.text = Numerador.ToString();
        ViewDenominador.text = Denominador.ToString();
    }

    //Método que reseta o numerador e denominador
    void ResetNumDen()
    {
        Numerador = 1;
        Denominador = 1;
    }

    //Método temporário que eu criei pra comparar os elementos de um array (Esqueci os do C# kkkkkkkkk)
    bool PertenceAoArray(int[] equivalentes, int DenominadorY)
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

}